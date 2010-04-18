Option Strict On
Imports System.ComponentModel
Imports System.Reflection

' RestrictedUI: MOZILLA PUBLIC LICENSE STATEMENT.
' -----------------------------------------------------------
' The contents of this file are subject to the Mozilla Public
' License Version 1.1 (the "License"); you may not use this file
' except in compliance with the License. You may obtain a copy of
' the License at http://www.mozilla.org/MPL/

' Software distributed under the License is distributed on an "AS
' IS" basis, WITHOUT WARRANTY OF ANY KIND, either express or
' implied. See the License for the specific language governing
' rights and limitations under the License.

' The Original Code is RestrictedUI 1.0.

' The Initial Developer of the Original Code is Daniel Prado Velasco
' <dpradov@gmail.com> (Spain).
' Portions created by Daniel Prado Velasco are
' Copyright (C) 2010. All Rights Reserved.
' -----------------------------------------------------------
' Contributor(s):
' -----------------------------------------------------------
' History:
' -----------------------------------------------------------
' Released: 13 April 2010
' -----------------------------------------------------------
' URLs:
'  http://code.google.com/p/restricted-ui/



#Region "Estructuras y clases que albergan la seguridad a nivel de entorno"

Public Structure Rol
    Public ID As Integer
    Public [Alias] As String
    Public Name As String
End Structure

Public Structure State
    Public ID As Integer
    Public Name As String
End Structure

Public Structure Group
    Public Name As String
    Public Controls As String()
End Structure

Public Class ComponentSecurity
    Public Roles As Rol()
    Public States As State()
    Public Groups As Group()
    Public Authorizations As String()
End Class

#End Region

#Region "Estructura para la gestión de HotKey"
''' <summary>
''' Teclas de acceso rápido (hot keys)
''' </summary>
Public Structure HotKey
    Public Sub New(ByVal KeyCode As Keys, ByVal Modifiers As Keys, Optional ByVal EnabledHotKey As Boolean = False)
        Me.KeyCode = KeyCode
        Me.Modifiers = Modifiers
        Enabled = EnabledHotKey
    End Sub
    Public KeyCode As Keys
    Public Modifiers As Keys
    Public Enabled As Boolean
End Structure

#End Region

''' <summary>
''' Clase Singleton (vía Shared) que mantiene aspectos generales a todo el esquema de seguridad
''' </summary>
''' <remarks></remarks>
Public Class SecurityEnvironment

#Region "Eventos disparados"
    Public Shared Event ControlAdapterFactoriesChanged()
    Public Shared Event SecurityChanged(ByVal IDControlRestriccionesUI As String)
    Public Shared Event HotKeyChanged()
    Protected Friend Shared Event SecurityChangedWithCancelInMind(ByVal IDControlRestriccionesUI As String, ByVal salvarPrioridad As Boolean, ByVal recuperarPrioridad As Boolean)

    ''' <summary>
    ''' Señala un cambio en el estado actual de la aplicación (según comunica IHost). Este estado puede ser común a todas las pantallas
    ''' o ser incluso específico para cada instancia de un tipo concreto de formulario (o contenedor)
    ''' </summary>
    ''' <remarks>Si el ID del controlador es Nothing deberá ser atendido por todos los componentes de seguridad. Podrá también restringirse 
    ''' a determinadas instancias mediante 'instanceID'</remarks> 
    ''' <param name="ID">Identificador del controlador</param> 
    ''' <param name="instanceID">Identificador de la instancia del controlador</param> 
    Public Shared Event StateChanged(ByVal ID As String, ByVal instanceID As String, ByVal nuevoEstado As Integer)

    ''' <summary>
    ''' Señala un cambio sobre el/los perfiles del usuario conectado (según comunica IHost). Estos roles pueden ser comunes para todas las pantallas
    ''' o ser incluso específico para cada instancia de un tipo concreto de formulario (o contenedor)
    ''' </summary>
    ''' <remarks>Si el ID del controlador es Nothing deberá ser atendido por todos los componentes de seguridad. Podrá también restringirse 
    ''' a determinadas instancias mediante 'instanceID'</remarks> 
    ''' <param name="ID">Identificador del controlador</param> 
    ''' <param name="instanceID">Identificador de la instancia del controlador</param> 
    Public Shared Event RolesChanged(ByVal ID As String, ByVal instanceID As String)

#End Region

    ''' <summary>
    ''' Devuelve el adaptador <see cref="IControlAdapter"/> correspondiente al control indicado, en base a las factorías
    ''' registradas en el Entorno.
    ''' </summary>
    ''' <param name="control">Control para el que se busca un adaptador</param>
    ''' <param name="parent "></param>
    ''' <returns>Se devuelve siempre una instancia correcta de <see cref="IControlAdapter"/>. Cuando no se localiza ningún
    ''' adaptador adecuado se devolverá <see cref="NullControlAdapter"/> (Special Case)
    ''' </returns>
    ''' <remarks>
    ''' Se utilizan factorías de adaptadores (<see cref="IControlAdapterFactory"/>) para localizar el adaptador más idóneo. Los principales componentes de
    ''' seguridad (ControlRestrictedUIWinForms y ControlRestrictedUIWeb) incluyen sus propias factorías internas para 
    ''' reconocer los controles más usuales en sus respectivos entornos.
    ''' Es posible asociar nuevas factorías (<see cref="IControlAdapterFactory"/>) al Entorno, que tendrán prioridad sobre
    ''' las internas.
    ''' <seealso cref="IControlAdapter"/>
    ''' <seealso cref="IControlAdapterFactory"/>
    ''' </remarks>
    Public Shared Function GetAdapter(ByVal control As Object, Optional ByVal parent As Object = Nothing) As IControlAdapter
        Dim adapter As IControlAdapter = Nothing

        For Each factory As IControlAdapterFactory In _AditionalFactories
            adapter = factory.GetAdapter(control, parent)
            If Not adapter.IsNull Then
                Exit For
            End If
        Next

        If adapter Is Nothing OrElse adapter.IsNull Then
            ' Factoría Base corresponderá a Web o WinForms
            If _BaseFactory IsNot Nothing Then
                adapter = _BaseFactory.GetAdapter(control, parent)
            End If
        End If

        If adapter Is Nothing OrElse adapter.IsNull Then
#If DEBUG Then
            'ShowError(">NullControlAdapter " + control.GetType.ToString, padre)
#End If
            Return New NullControlAdapter
        Else
            Return adapter
        End If

    End Function

    ''' <summary>
    ''' Recupera o establece el objeto que implementa la interface <see cref="IHost"/> y a través de la cual el <see cref="SecurityEnvironment"/> y el resto
    ''' de componentes de seguridad puede interaccionar con la aplicación Host a efectos de conocer el estado y roles actuales que
    ''' deben considerarse.
    ''' </summary>
    Public Shared Property Host() As IHost
        Get
            Return _host
        End Get
        Set(ByVal value As IHost)
            If value IsNot Nothing Then
                _host = value
                AddHandler _host.StateChanged, AddressOf OnStateChanged
                AddHandler _host.RolesChanged, AddressOf OnRolesChanged
            End If
        End Set
    End Property
    Protected Shared _host As IHost

    ''' <summary>
    ''' Comunica errores o incidencias a la aplicación a través del objeto que implementa la interface <see cref="IHost"/>
    ''' </summary>
    ''' <remarks>Algunas incidencias podrán ser ignoradas tranquilamente por la aplicación si indican que no se ha
    ''' encontrado un determinado control y se sabe que éste es creado dinámicamente</remarks>
    Protected Friend Shared Sub ShowError(ByVal [error] As String, Optional ByVal ParentControl As Object = Nothing, Optional ByVal IDControlRestrictedUI As String = "")
        Dim cad As String = ""
        If ParentControl IsNot Nothing Then
            cad = cad + "ControlPadre=" + ParentControl.ToString
        End If
        If Not String.IsNullOrEmpty(IDControlRestrictedUI) Then
            cad = cad + " IDControlRestriccionesUI=" + IDControlRestrictedUI
        End If
        If cad <> "" Then cad = "[" + cad + "]  "

        cad = cad + [error]
        If _host Is Nothing Then
            MessageBox.Show(cad)
        Else
            _host.ShowError(cad)
        End If
    End Sub


#Region "Gestión de Factorías de adaptadores"

    ''' <summary>
    ''' Obtiene o establece la factoría de adaptadores que se considera base
    ''' </summary>
    ''' <remarks>Los principales componentes de seguridad UI (ControlRestrictedUIWinForms y ControlRestrictedUIWeb) incluyen sus 
    ''' propias factorías internas para reconocer los controles más usuales en sus respectivos entornos. Estos componentes
    ''' indican al Entorno a través de esta propiedad cuál debe ser la factoría base a utilizar.
    ''' </remarks>
    Public Shared Property BaseFactory() As IControlAdapterFactory
        Get
            Return _BaseFactory
        End Get
        Set(ByVal value As IControlAdapterFactory)
            If value IsNot Nothing Then
                _BaseFactory = value
            End If
        End Set
    End Property
    Protected Shared _BaseFactory As IControlAdapterFactory


    ''' <summary>
    ''' Obtiene la lista de factorías adicionales de adaptadores 
    ''' </summary>
    ''' <remarks>Las factorías adicionales tendrán prioridad sobre la factoría base (normalmente asociada al 
    ''' componente de seguridad). A través de estas factorias adicionales es posible gestionar o interpretar
    ''' controles específicos, o incluso ofrecer adaptadores altenativos para controles manejables por la 
    ''' factoría base (> Plugin) 
    ''' Como ejemplo de una factoría adicional, muy específica: AdapterInfragisticsWinForms_Factory
    ''' </remarks>
    Protected Friend Shared ReadOnly Property AditionalFactories() As IList(Of IControlAdapterFactory)
        Get
            Return _AditionalFactories
        End Get
    End Property
    Protected Shared _AditionalFactories As IList(Of IControlAdapterFactory) = New List(Of IControlAdapterFactory)

    ''' <summary>
    ''' Agrega la factoría de adaptadores de control indicada. Será consultada antes que la factoría interna
    ''' </summary>
    ''' <remarks>Si se agregan varias, se preguntarán en el orden en que se hayan añadido</remarks> 
    Public Shared Sub AddFactory(ByVal factory As IControlAdapterFactory)
        If Not _AditionalFactories.Contains(factory) Then
            _AditionalFactories.Add(factory)
            RaiseEvent ControlAdapterFactoriesChanged()
            RaiseEvent SecurityChanged(Nothing)
        End If
    End Sub

    ''' <summary>
    ''' Elimina la factoría de adaptadores de control indicada
    ''' </summary>
    ''' <remarks></remarks> 
    Public Shared Sub RemoveFactory(ByVal factory As IControlAdapterFactory)
        If Not _AditionalFactories.Contains(factory) Then
            _AditionalFactories.Remove(factory)
            RaiseEvent ControlAdapterFactoriesChanged()
            RaiseEvent SecurityChanged(Nothing)
        End If
    End Sub

    ''' <summary>
    ''' Inicializa las factorías adicionales de adaptadores a partir de la configuración establecida en el archivo que se indica
    ''' </summary>
    ''' <param name="file "></param>
    ''' <param name="auxDomain "></param>
    ''' <remarks>Utilizable en tiempo de diseño</remarks>
    Friend Shared Sub LoadFactories(ByVal file As String, ByRef auxDomain As AppDomain)
        If String.IsNullOrEmpty(file) Then Exit Sub

        Dim Factories As List(Of String) = Nothing
        LoadConfiguration(ReadFile(file, True), OptFileConfig.LoadOnlyFactories, Factories)
        SetFactories(Factories, True, auxDomain)
        RaiseEvent SecurityChanged(Nothing)
    End Sub


    ''' <summary>
    ''' Inicializa las factorías de adaptadores de controles mediante la lista de cadenas en las que se describen. Estas factorías
    ''' se instanciarán y añadirán a la lista <see cref="AditionalFactories "/>
    ''' </summary>
    ''' <param name="factories ">Lista de cadenas de la forma [ruta de la DLL], [nombre completo de la clase]
    ''' <example>PruebaWinForms\bin\Debug\Seguridad_AdaptadoresInfragistics.dll, Seguridad_AdaptadoresInfragistics.AdaptadorControlUltraGridWinFormsFactoria</example></param>
    ''' <param name="designTime"></param>
    ''' <param name="domainAux">Dominio de aplicación (AppDomain) dentro del cual se cargarán en tiempo de ejecución estas DLL (en tiempo de diseño)</param>
    ''' <remarks></remarks>
    Private Shared Sub SetFactories(ByVal factories As List(Of String), ByVal designTime As Boolean, Optional ByRef domainAux As System.AppDomain = Nothing)
        Dim campos As String()
        Dim factory As IControlAdapterFactory

        AditionalFactories.Clear()
        If factories IsNot Nothing Then
            For Each factoryStr As String In factories
                campos = factoryStr.Split(","c)

                If designTime Then
                    factory = InstanceFactoria(campos(0).Trim, campos(1).Trim, domainAux)
                Else
                    factory = InstanceFactoria(campos(0).Trim, campos(1).Trim)
                End If
                If factory IsNot Nothing AndAlso Not _AditionalFactories.Contains(factory) Then
                    _AditionalFactories.Add(factory)
                End If
            Next
        End If
        RaiseEvent ControlAdapterFactoriesChanged()
        RaiseEvent SecurityChanged(Nothing)
    End Sub

    ''' <summary>
    ''' Instancia y devuelve la factoría indicada mediante el nombre de la DLL y su nombre completo (para ser utilizada en tiempo de diseño)
    ''' </summary>
    ''' <remarks>Se deberá descargar el dominio utilizado</remarks>
    Private Shared Function InstanceFactoria(ByVal dllFactory As String, ByVal ClassFullName As String, ByRef DomainAux As AppDomain) As IControlAdapterFactory
        Dim factory As IControlAdapterFactory = Nothing
        Dim assembly As Assembly = Nothing
        Dim method As MethodInfo
        Dim cause As String = ""

        Try
            Dim posStartNameDLL As Integer = dllFactory.LastIndexOf("\")
            If Not AdaptFilePath(dllFactory, True) Then
                cause = Constants.ERROR_DLL_NOTFOUND
            Else
                Dim ads As New AppDomainSetup()
                ads.ApplicationBase = dllFactory.Substring(0, posStartNameDLL)
                ads.DisallowBindingRedirects = False
                ads.DisallowCodeDownload = True
                ads.ConfigurationFile = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile

                ' Dejo en dllFactoria exclusivamente el nombre de la dll, sin el path y sin la extensión
                dllFactory = dllFactory.Substring(posStartNameDLL + 1, dllFactory.Length - posStartNameDLL - 5)
                DomainAux = AppDomain.CreateDomain("DomainAux", Nothing, ads)
                DomainAux.Load(dllFactory)
                For Each a As Assembly In DomainAux.GetAssemblies
                    If a.FullName.StartsWith(dllFactory) Then
                        assembly = a
                        Exit For
                    End If
                Next
                method = assembly.GetType(ClassFullName).GetMethod("GetInstance")
                factory = DirectCast(method.Invoke(Nothing, New Object() {}), IControlAdapterFactory)
                SecurityEnvironment.AddFactory(factory)
            End If

        Catch ex As Exception
            cause = Constants.ERROR_CLASS_METHOD_NOTFOUND
        End Try

        If factory Is Nothing Then
            SecurityEnvironment.ShowError(Constants.ERROR_INSTANCING_FACTORY + " <" + dllFactory + "," + ClassFullName + "> : " + cause)
        End If

        Return factory
    End Function

    ''' <summary>
    ''' Instancia y devuelve la factoría indicada mediante el nombre de la DLL y su nombre completo (para ser utilizada en tiempo de ejecución)
    ''' </summary>
    Private Shared Function InstanceFactoria(ByVal dllFactory As String, ByVal ClassFullName As String) As IControlAdapterFactory
        Dim factory As IControlAdapterFactory = Nothing
        Dim cause As String = ""

        If Not AdaptFilePath(dllFactory, False) Then
            cause = Constants.ERROR_DLL_NOTFOUND
        Else
            Try
                Dim assembly As Assembly = assembly.LoadFrom(dllFactory)
                Dim method As MethodInfo = assembly.GetType(ClassFullName).GetMethod("GetInstance")
                factory = DirectCast(method.Invoke(Nothing, New Object() {}), IControlAdapterFactory)
                SecurityEnvironment.AddFactory(factory)

            Catch ex As Exception
                cause = Constants.ERROR_CLASS_METHOD_NOTFOUND
            End Try
        End If

        If factory Is Nothing Then
            SecurityEnvironment.ShowError(Constants.ERROR_INSTANCING_FACTORY + " <" + dllFactory + "," + ClassFullName + "> : " + cause)
        End If

        Return factory
    End Function


#End Region

#Region "Definición y acceso a la Seguridad a nivel de Entorno"
    ''' <summary>
    ''' Obtiene o establece los roles definidos como comunes (se ofrecen en todos los componentes de seguridad)
    ''' </summary>
    Public Shared Property CommonRoles() As List(Of Rol)
        Get
            Return _CommonRoles
        End Get
        Set(ByVal value As List(Of Rol))
            _CommonRoles = value
        End Set
    End Property
    Protected Shared _CommonRoles As New List(Of Rol)

    ''' <summary>
    ''' Obtiene o establece los estados definidos como comunes (se ofrecen en todos los componentes de seguridad)
    ''' </summary>
    Public Shared Property CommonStates() As List(Of State)
        Get
            Return _CommonStates
        End Get
        Set(ByVal value As List(Of State))
            _CommonStates = value
        End Set
    End Property
    Protected Shared _CommonStates As New List(Of State)

    ''' <summary>
    ''' Devuelve el objeto diccionario en el que se guarda la definición de seguridad (política de restricciones de interface)
    ''' de los diferentes componentes, según pueda haberse establecido a partir de la carga de un fichero de configuración o 
    ''' de una cadena de texto.
    ''' Ver <see cref="LoadFrom"/> y <see cref="LoadFromString"/> 
    ''' </summary>
    ''' <remarks>La seguridad aquí contenida no tiene por qué ser igual a la embebida en los distintos componentes. 
    ''' La que aplique finalmente en el componente depende de una propiedad del mismo (PrioridadSeguridadEmbebida)
    ''' Métodos como LoadFrom o LoadFromString, por ejemplo, forzarán la prioridad a la seguridad del entorno sobre la 
    ''' embebida, para un componente, cuando en el fichero o cadena (p.ej) aparezca referenciado ese componente, aunque
    ''' sea con un definición vacía.
    ''' </remarks>
    Public Shared ReadOnly Property ComponentsSecurity() As Dictionary(Of String, ComponentSecurity)
        Get
            Return _componentsSecurity
        End Get
    End Property
    Protected Shared _componentsSecurity As New Dictionary(Of String, ComponentSecurity)

    ''' <summary>
    ''' Incorpora a la seguridad a nivel de entorno la definición correspondiente al componente indicado con <paramref name="IDControlRestriccionesUI "/>
    ''' </summary>
    ''' <param name="IDControlRestriccionesUI"></param>
    ''' <param name="seg"></param>
    ''' <param name="restorePriority ">Indica si debe restaurarse la prioridad que se tenía respecto a la seguridad embebida</param>
    ''' <remarks>Comunica al componente el cambio en la seguridad mediante un evento que indica si debe o no recordar el cambio en el caso
    ''' de que deba ser deshecho posteriormente (este método es invocado desde el formulario de mantenimiento de la seguridad)</remarks>
    Protected Friend Shared Sub AddSecurityDefinition(ByVal IDControlRestriccionesUI As String, ByVal seg As ComponentSecurity, ByVal restorePriority As Boolean)
        If String.IsNullOrEmpty(IDControlRestriccionesUI) Then Exit Sub

        If ComponentsSecurity.ContainsKey(IDControlRestriccionesUI) Then
            ComponentsSecurity.Remove(IDControlRestriccionesUI)
        End If
        ComponentsSecurity.Add(IDControlRestriccionesUI, seg)

        RaiseEvent SecurityChangedWithCancelInMind(IDControlRestriccionesUI, False, restorePriority)
    End Sub

#End Region

#Region "Gestión de Archivos/Cadenas de configuración"
    ''' <summary>
    ''' Carga la seguridad establecida a nivel de entorno (RolesComunes, EstadosComunes, SeguridadContenedores) con la definición de seguridad 
    ''' establecida en el fichero facilitado como parámetro.
    ''' Es posible cargar exclusivamente la seguridad correspondiente al componente de seguridad identificado con <paramref name=" IDControlRestrictedUI  "/>
    ''' </summary>
    ''' <remarks>Mediante las opciones establecidas en <paramref name=" opc "/> es posible cargar los elementos comunes
    ''' Mediante este método no se cargarán las factorías que puedan estar aquí definidas. Se recomienda asociar las factorías en tiempo de compilación. En
    ''' cualquier caso se ofrece un método explícito para ello: <see cref="LoadFactories  "/> 
    ''' Nota: Aquellos componentes de seguridad para los que no haya ninguna referencia en este archivo no se verán afectados y seguirá teniendo
    ''' prioridad su definición de seguridad embebida. Si aparece el encabezado referente a ese componente de seguridad aunque vacío sí se tendrá 
    ''' en cuenta y se considerará que no deberá aplicarse ninguna supervisión sobre el formulario o contenedor correspondiente</remarks> 
    Public Shared Sub LoadFrom(ByVal file As String, _
                               Optional ByVal opt As OptFileConfig = OptFileConfig.None, _
                               Optional ByVal IDControlRestrictedUI As String = "")

        LoadConfiguration(ReadFile(file, False), _
                          opt, , _
                          _CommonRoles, _CommonStates, _componentsSecurity, _
                          IDControlRestrictedUI)
        RaiseEvent SecurityChanged(IDControlRestrictedUI)
    End Sub

    ''' <summary>
    ''' Carga la seguridad establecida a nivel de entorno, de manera equivalente a <see cref=" LoadFrom "/> con la única diferencia
    ''' de que este método está pensado para permitir indicar al componente de seguridad que deshaga los cambios.
    ''' </summary>
    ''' <remarks>Utilizado por el formulario de mantenimiento de la seguridad</remarks> 
    Protected Friend Shared Sub LoadFromWithCancelInMind(ByVal file As String, _
                               Optional ByVal opt As OptFileConfig = OptFileConfig.None, _
                               Optional ByVal IDControlRestrictedUI As String = "", Optional ByVal savePriority As Boolean = False, Optional ByVal restorePriority As Boolean = False)

        LoadConfiguration(ReadFile(file, False), _
                          opt, , _
                          _CommonRoles, _CommonStates, _componentsSecurity, _
                          IDControlRestrictedUI)
        RaiseEvent SecurityChangedWithCancelInMind(IDControlRestrictedUI, savePriority, restorePriority)
    End Sub

    ''' <summary>
    ''' Carga la seguridad establecida a nivel de entorno (RolesComunes, EstadosComunes, SeguridadContenedores) con la definición de seguridad 
    ''' ofrecida desde el stream facilitado.
    ''' Es posible cargar exclusivamente la seguridad correspondiente al componente de seguridad identificado con <paramref name=" IDControlRestrictedUI  "/>
    ''' </summary>
    ''' <remarks>Mediante las opciones establecidas en <paramref name=" opc "/> es posible cargar los elementos comunes
    ''' Mediante este método no se cargarán las factorías que puedan estar aquí definidas. Se recomienda asociar las factorías en tiempo de compilación. En
    ''' cualquier caso se ofrece un método explícito para ello: <see cref="LoadFactories  "/>
    ''' Nota: Aquellos componentes de seguridad para los que no haya ninguna referencia en este stream no se verán afectados y seguirá teniendo
    ''' prioridad su definición de seguridad embebida. Si aparece el encabezado referente a ese componente de seguridad aunque vacío sí se tendrá 
    ''' en cuenta y se considerará que no deberá aplicarse ninguna supervisión sobre el formulario o contenedor correspondiente</remarks>
    Public Shared Sub LoadFrom(ByVal stream As IO.StreamReader, _
                                     Optional ByVal opt As OptFileConfig = OptFileConfig.None, _
                                     Optional ByVal IDControlRestrictedUI As String = "")

        LoadConfiguration(stream.ReadToEnd, opt, , _
                                 _CommonRoles, _CommonStates, _componentsSecurity, _
                                 IDControlRestrictedUI)
        RaiseEvent SecurityChanged(IDControlRestrictedUI)
    End Sub

    ''' <summary>
    ''' Carga la seguridad establecida a nivel de entorno (RolesComunes, EstadosComunes, SeguridadContenedores) con la definición de seguridad 
    ''' ofrecida desde la cadena facilitada, de manera equivalente a <see  cref=" LoadFrom "/>
    ''' Es posible cargar exclusivamente la seguridad correspondiente al componente de seguridad identificado con <paramref name=" IDControlRestrictedUI  "/>
    ''' </summary>
    Public Shared Sub LoadFromString(ByVal TxtConfiguration As String, _
                                     Optional ByVal opt As OptFileConfig = OptFileConfig.None, _
                                     Optional ByVal IDControlRestrictedUI As String = "")

        LoadConfiguration(TxtConfiguration, opt, , _
                                 _CommonRoles, _CommonStates, _componentsSecurity, _
                                 IDControlRestrictedUI)
        RaiseEvent SecurityChanged(IDControlRestrictedUI)
    End Sub


    ''' <summary>
    ''' Devuelve el contenido del archivo de texto facilitado, cuya ruta se adaptará según se necesite acceder en tiempo de diseño 
    ''' o de ejecución
    ''' </summary>
    Protected Friend Shared Function ReadFile(ByVal file As String, ByVal designTime As Boolean) As String
        Dim result As String = ""
        If file Is Nothing Then file = ConfigFile
        If AdaptFilePath(file, designTime) Then
            Dim sr As System.IO.StreamReader = New System.IO.StreamReader(file, System.Text.Encoding.Default)
            result = sr.ReadToEnd
            sr.Close()
        End If
        Return result
    End Function

    ''' <summary>
    ''' Devuelve el contenido del archivo de texto facilitado
    ''' </summary>
    Protected Friend Shared Function ReadFile(ByVal file As String) As String
        Dim result As String = ""
        If file Is Nothing Then file = ConfigFile
        If My.Computer.FileSystem.FileExists(file) Then
            Dim sr As System.IO.StreamReader = New System.IO.StreamReader(file, System.Text.Encoding.Default)
            result = sr.ReadToEnd
            sr.Close()
        End If
        Return result
    End Function



    Private Enum ConfigurationBlocks
        Factories = 0
        CommonRoles
        CommonStates
        CONTROLSECURITY
        Groups
        Authorizations
        Roles
        States
    End Enum

    ''' <summary>
    ''' Enumeración (conjunto de bits) que ofrece las opciones a emplear sobre los métodos de lectura del fichero de configuración
    ''' </summary>
    <Flags()> _
    Public Enum OptFileConfig As Short
        None = 0
        LoadOnlyFactories = 2
        LoadOnlyCommonElements = 4
        AbortOnError = 8
    End Enum

    ''' <summary>
    ''' Carga las variables facilitadas Factorias, RolesComunes, EstadosComunes, SeguridadFormularios con la definición de seguridad 
    ''' establecida en la cadena de texto <paramref name=" TxtConfiguracion "/>
    ''' Es posible cargar exclusivamente la seguridad correspondiente al componente de seguridad identificado con <paramref name=" IDControlRestrictedUI  "/>
    ''' </summary>
    ''' <remarks>Mediante las opciones establecidas en <paramref name=" opc "/> es posible cargar sólo las factorías o sólo los elementos comunes</remarks> 
    Protected Friend Shared Sub LoadConfiguration(ByVal TxtConfiguration As String, _
                                         Optional ByVal opt As OptFileConfig = OptFileConfig.None, _
                                         Optional ByRef Factories As List(Of String) = Nothing, _
                                         Optional ByRef CommonRoles As List(Of Rol) = Nothing, _
                                         Optional ByRef CommonStates As List(Of State) = Nothing, _
                                         Optional ByRef ComponentsSecurity As Dictionary(Of String, ComponentSecurity) = Nothing, _
                                         Optional ByVal IDControlRestrictedUI As String = "")

        If Factories Is Nothing Then Factories = New List(Of String)
        If CommonRoles Is Nothing Then CommonRoles = New List(Of Rol)
        If CommonStates Is Nothing Then CommonStates = New List(Of State)
        If ComponentsSecurity Is Nothing Then ComponentsSecurity = New Dictionary(Of String, ComponentSecurity)
        Factories.Clear()
        CommonRoles.Clear()
        CommonStates.Clear()

        If TxtConfiguration Is Nothing Then Exit Sub

        If (opt And OptFileConfig.LoadOnlyCommonElements) <> 0 Then
            IDControlRestrictedUI = "%/%"
        End If

        Dim actualBlock, previousBlock As ConfigurationBlocks
        Dim fields As String()
        Dim _IDControlRestrictedUI As String = ""
        Dim _IDControlRestrictedUIPrevious As String = ""
        Dim securityComp As New ComponentSecurity
        Dim lCad As New List(Of String)
        Dim pos As Integer

        Dim lGroups As New List(Of Group)
        Dim lRoles As New List(Of Rol)
        Dim lStates As New List(Of State)


        Try
            Dim reader As New System.IO.StringReader(TxtConfiguration)


            Try
                Dim line As String
                previousBlock = Nothing

                Do
                    line = reader.ReadLine
                    If line Is Nothing Then Exit Do
                    line = line.Trim
                    If line = "" OrElse line(0) = ";"c Then Continue Do

                    ' Identificar si se entra en un nuevo bloque
                    '===========================
                    previousBlock = actualBlock
                    _IDControlRestrictedUIPrevious = _IDControlRestrictedUI

                    If line(0) = "["c Then
                        Select Case line.ToUpper
                            Case "[FACTORIES]"
                                actualBlock = ConfigurationBlocks.Factories
                                _IDControlRestrictedUI = ""
                            Case "[COMMONROLES]"
                                actualBlock = ConfigurationBlocks.CommonRoles
                                _IDControlRestrictedUI = ""
                            Case "[COMMONSTATES]"
                                actualBlock = ConfigurationBlocks.CommonStates
                                _IDControlRestrictedUI = ""
                            Case "[GROUPS]"
                                actualBlock = ConfigurationBlocks.Groups
                            Case "[AUTHORIZATIONS]"
                                actualBlock = ConfigurationBlocks.Authorizations
                            Case "[ROLES]"
                                actualBlock = ConfigurationBlocks.Roles
                            Case "[STATES]"
                                actualBlock = ConfigurationBlocks.States
                            Case Else
                                If line.Substring(0, 17) = "[SECURITYCONTROL=" Then
                                    actualBlock = ConfigurationBlocks.CONTROLSECURITY
                                    _IDControlRestrictedUI = line.Substring(17, line.Length - 18)
                                End If
                        End Select

                        ' Si se cambia de bloque, realizar las acciones necesarias en cada caso
                        '===========================
                        If actualBlock <> previousBlock Then
                            Select Case previousBlock
                                Case ConfigurationBlocks.Authorizations
                                    securityComp.Authorizations = lCad.ToArray
                                    lCad.Clear()
                                Case ConfigurationBlocks.States
                                    securityComp.States = lStates.ToArray
                                    lStates.Clear()
                                Case ConfigurationBlocks.Roles
                                    securityComp.Roles = lRoles.ToArray
                                    lRoles.Clear()
                                Case ConfigurationBlocks.Groups
                                    securityComp.Groups = lGroups.ToArray
                                    lGroups.Clear()
                            End Select
                            ' Si se sale del conjunto de bloques específico de la seguridad de un formulario, el ID cambiará
                            If _IDControlRestrictedUIPrevious <> "" And _IDControlRestrictedUI <> _IDControlRestrictedUIPrevious Then
                                ' Si se ha facilitado un ID de control de seguridad sólo se leerá los datos de seguridad de ese control
                                If String.IsNullOrEmpty(IDControlRestrictedUI) Or _IDControlRestrictedUIPrevious = IDControlRestrictedUI Then
                                    ComponentsSecurity.Remove(_IDControlRestrictedUIPrevious)
                                    ComponentsSecurity.Add(_IDControlRestrictedUIPrevious, securityComp)
                                    securityComp = New ComponentSecurity  ' para el siguiente
                                End If
                            End If
                        End If

                        Continue Do
                    End If



                    ' Tratar cada bloque
                    '===========================
                    ' Si sólo hay que cargar factorías..
                    If ((opt And OptFileConfig.LoadOnlyFactories) <> 0) And actualBlock <> ConfigurationBlocks.Factories Then
                        Continue Do
                    End If
                    ' Si sólo hay que leer el control de seguridad con ID facilitado como parámetro..
                    If (Not String.IsNullOrEmpty(IDControlRestrictedUI) And _IDControlRestrictedUI <> "") And _IDControlRestrictedUI <> IDControlRestrictedUI Then
                        Continue Do
                    End If

                    Select Case actualBlock
                        Case ConfigurationBlocks.Factories
                            Factories.Add(line)

                        Case ConfigurationBlocks.CommonRoles, ConfigurationBlocks.Roles
                            Dim rol As New Rol
                            fields = line.Split(","c)
                            If Not Integer.TryParse(fields(0), rol.ID) Then
                                MessageBox.Show(Constants.ERROR_INCORRECT_ROL_ID + fields(0))
                                Exit Sub
                            End If
                            rol.Name = fields(1).Trim
                            If fields.Length > 2 Then rol.Alias = fields(2).Trim
                            If actualBlock = ConfigurationBlocks.CommonRoles Then
                                CommonRoles.Add(rol)
                            Else
                                lRoles.Add(rol)
                            End If


                        Case ConfigurationBlocks.CommonStates, ConfigurationBlocks.States
                            Dim estado As New State
                            fields = line.Split(","c)
                            If Not Integer.TryParse(fields(0), estado.ID) Then
                                MessageBox.Show(Constants.ERROR_INCORRECT_STATE_ID + fields(0))
                                Exit Sub
                            End If
                            estado.Name = fields(1).Trim

                            If actualBlock = ConfigurationBlocks.CommonStates Then
                                CommonStates.Add(estado)
                            Else
                                lStates.Add(estado)
                            End If

                        Case ConfigurationBlocks.Authorizations
                            lCad.Add(line)

                        Case ConfigurationBlocks.Groups
                            Dim grupo As New Group
                            pos = line.IndexOf("="c)
                            grupo.Name = line.Substring(0, pos).Trim
                            grupo.Controls = line.Substring(pos + 1).Replace(" ", "").Split(","c)
                            lGroups.Add(grupo)
                    End Select

                Loop

                ' Atender al cierre del último bloque
                '===========================
                Select Case actualBlock
                    Case ConfigurationBlocks.Authorizations
                        securityComp.Authorizations = lCad.ToArray
                    Case ConfigurationBlocks.States
                        securityComp.States = lStates.ToArray
                    Case ConfigurationBlocks.Roles
                        securityComp.Roles = lRoles.ToArray
                    Case ConfigurationBlocks.Groups
                        securityComp.Groups = lGroups.ToArray
                End Select
                If _IDControlRestrictedUI <> "" And (String.IsNullOrEmpty(IDControlRestrictedUI) Or _IDControlRestrictedUI = IDControlRestrictedUI) Then
                    ComponentsSecurity.Remove(_IDControlRestrictedUI)
                    ComponentsSecurity.Add(_IDControlRestrictedUI, securityComp)
                End If
            Finally
                reader.Close()
            End Try

        Catch ex As Exception
            Dim cad As String = Constants.ERROR_PROCESSING_CONFIGFILE + ex.Message
            ShowError(cad, Nothing)
            MsgBox(cad, MsgBoxStyle.Exclamation)
            If (opt And OptFileConfig.AbortOnError) <> 0 Then
                MsgBox(Constants.ERROR_EXITING_APPLICATION, MsgBoxStyle.Critical)
                Application.Exit()
            End If
        End Try

    End Sub




    ''' <summary>
    ''' Permite salvar en el archivo indicado toda la definición de seguridad establecida a nivel de entorno (actualizando la seguridad que hubiera 
    ''' definida en el archivo), junto con la correspondiente a un determinado componente de seguridad. El guardar el componente indicado es opcional,
    ''' como lo es hacerlo junto con toda la seguridad a nivel de entorno. Es posible forzar los roles y estados comunes que se quiere enviar al
    ''' archivo.
    ''' </summary>
    ''' <remarks>
    ''' La configuración de seguridad de aquellos componentes que estuvieran en el archivo y no aparezcan en el entorno de seguridad ni sea el
    ''' explicitado con <paramref name=" IDControlRestrictedUI  "/> no se verá afectada. Permanecerá tal como estaba en el archivo (aunque se perderán los comentarios
    ''' que pudieran haberse incorporado)
    ''' La ruta del fichero estará ya adaptada según se invoque a este método en tpo. de diseño o de ejecución
    ''' </remarks> 
    Public Shared Sub SaveConfiguration(ByVal file As String, _
                                          ByVal includeAllKnownSecurity As Boolean, _
                                          Optional ByVal IDControlRestrictedUI As String = "", _
                                          Optional ByVal securityComp As ComponentSecurity = Nothing, _
                                          Optional ByVal CommonRoles As List(Of Rol) = Nothing, _
                                          Optional ByVal CommonStates As List(Of State) = Nothing, _
                                          Optional ByVal opt As OptFileConfig = OptFileConfig.None _
                                          )

        Dim CommonRolesAux As List(Of Rol) = Nothing
        Dim CommonStatesAux As List(Of State) = Nothing
        Dim FactoriesAux As List(Of String) = Nothing
        Dim ComponentsSecurityAux As Dictionary(Of String, ComponentSecurity) = Nothing


        LoadConfiguration(ReadFile(file), opt, _
                          FactoriesAux, CommonRolesAux, CommonStatesAux, ComponentsSecurityAux)

        ' Ahora modificamos lo leído con lo pasado por parámetro, a salvar.
        ' Las factorías incluidas en el archivo no se modificarán
        ' Como roles y estados comunes se utilizarán los facilitados
        ' Según se restrinja a un ControlRestriccionesUI o no, se reemplazarán
        If includeAllKnownSecurity Then
            For Each s As String In SecurityEnvironment.ComponentsSecurity.Keys
                ComponentsSecurityAux.Remove(s)
                ComponentsSecurityAux.Add(s, SecurityEnvironment.ComponentsSecurity(s))
            Next
        End If
        If securityComp IsNot Nothing Then
            ComponentsSecurityAux.Remove(IDControlRestrictedUI)
            ComponentsSecurityAux.Add(IDControlRestrictedUI, securityComp)
        End If

        If CommonRoles IsNot Nothing Then CommonRolesAux = CommonRoles
        If CommonStates IsNot Nothing Then CommonStatesAux = CommonStates

        Dim securityStr As String
        securityStr = SecurityToString(FactoriesAux, CommonRolesAux, CommonStatesAux, ComponentsSecurityAux)

        IO.File.WriteAllText(file, securityStr, System.Text.Encoding.Default)
    End Sub

    ''' <summary>
    ''' Serializa la definición de seguridad establecida a nivel de entorno (incluyendo la relación de factorías adicionales) en una
    ''' cadena con el formato que se emplea en el archivo de configuración (legible como tal por tanto)
    ''' </summary>
    Public Shared Function SecurityToString(ByVal Factories As List(Of String), _
                                             ByVal CommonRoles As List(Of Rol), _
                                             ByVal CommonStates As List(Of State), _
                                             ByVal ComponentsSecurity As Dictionary(Of String, ComponentSecurity) _
                                             ) As String

        Dim sw As New System.IO.StringWriter
        Dim result As String = ""
        Try
            sw.WriteLine("[Factories]")
            sw.WriteLine("; Las rutas relativas se expresarán de manera relativa a la carpeta de la solución (.sln). Esta ruta se utilizará para localizar las DLL en tiempo de diseño")
            sw.WriteLine("; Se supondrá que la DLL se encuentra en la misma carpeta que el ejecutable, por lo que en tpo. de ejecución se ignorará la ruta y se utilizará únicamente el nombre del fichero")
            sw.WriteLine("; Nota: es posible utilizar también rutas absolutas.")
            If Factories IsNot Nothing Then
                For Each factoria As String In Factories
                    sw.WriteLine(factoria)
                Next
            End If
            sw.WriteLine()

            sw.WriteLine("[CommonRoles]")
            If CommonRoles IsNot Nothing Then
                For Each rol As Rol In CommonRoles
                    sw.WriteLine(rol.ID.ToString + "," + rol.Name + "," + rol.Alias)
                Next
                sw.WriteLine()
            End If

            sw.WriteLine("[CommonStates]")
            If CommonStates IsNot Nothing Then
                For Each e As State In CommonStates
                    sw.WriteLine(e.ID.ToString + "," + e.Name)
                Next
            End If

            For Each s As String In ComponentsSecurity.Keys
                Dim sf As ComponentSecurity = ComponentsSecurity.Item(s)
                sw.WriteLine()
                sw.WriteLine(";=======================================================")
                sw.WriteLine("[SECURITYCONTROL=" + s + "]")
                If sf.Roles IsNot Nothing Then
                    sw.WriteLine("[Roles]")
                    For Each rol As Rol In sf.Roles
                        sw.WriteLine(rol.ID.ToString + "," + rol.Name + "," + rol.Alias)
                    Next
                    sw.WriteLine()
                End If
                If sf.States IsNot Nothing Then
                    sw.WriteLine("[States]")
                    For Each e As State In sf.States
                        sw.WriteLine(e.ID.ToString + "," + e.Name)
                    Next
                    sw.WriteLine()
                End If
                If sf.Groups IsNot Nothing Then
                    sw.WriteLine("[Groups]")
                    For Each g As Group In sf.Groups
                        sw.WriteLine(g.Name + "= " + Util.ConvertToString(g.Controls))
                    Next
                    sw.WriteLine()
                End If
                If sf.Authorizations IsNot Nothing Then
                    sw.WriteLine("[Authorizations]")
                    For Each p As String In sf.Authorizations
                        sw.WriteLine(p)
                    Next
                End If

            Next
            result = sw.ToString

        Finally
            sw.Close()
        End Try

        Return result

    End Function



    ''' <summary>
    ''' Modifica la ruta facilitada en <paramref name="fichero"/> de manera que el archivo correspondiente pueda ser localizado
    ''' en tiempo de diseño o de ejecución, según se indique en <paramref name="designTime"/>.
    ''' </summary>
    ''' <param name="file ">Ruta hacia el archivo</param>
    ''' <param name="designTime">Indica si se quiere adaptar la ruta para ser utilizada en tiempo de diseño</param>
    ''' <returns>Devuelve True si el archivo existe</returns>
    ''' <remarks>
    ''' El fichero de configuración, el fichero de controles y las DLL de las factorías estarán expresadas mediante rutas 
    ''' relativas a la carpeta de la solución (.sln)
    ''' Esta ruta se utilizará para localizar las DLL en tiempo de diseño. Se supondrá que la DLL se encuentra en la misma carpeta que 
    ''' el ejecutable, por lo que en tpo. de ejecución se ignorará la ruta y se utilizará únicamente el nombre del fichero
    ''' Nota: es posible utilizar también rutas absolutas.
    ''' </remarks>
    Public Shared Function AdaptFilePath(ByRef file As String, ByVal designTime As Boolean) As Boolean
        ' Si la ruta expresada lleva al archivo (es absoluta o no requiere ningún tipo de adaptación) la devolvemos sin más
        If String.IsNullOrEmpty(file) Then Return False

        Try
            If Not My.Computer.FileSystem.FileExists(file) Then
                If Not designTime Then
                    Dim fileName As String = My.Computer.FileSystem.GetName(file)
                    Dim ext As String
                    If fileName.LastIndexOf(".") < 0 Then
                        ext = ""
                    Else
                        ext = fileName.Substring(fileName.LastIndexOf(".") + 1).ToUpper
                    End If

                    If ext = "DLL" Then
                        file = fileName
                    Else
                        Dim filePath As String = My.Computer.FileSystem.GetParentPath(file)
                        file = Application.StartupPath.Substring(0, Application.StartupPath.ToUpper.IndexOf(filePath.ToUpper) + Len(filePath)) + "\" + fileName
                    End If
                End If
            End If
            Return My.Computer.FileSystem.FileExists(file)

        Catch ex As Exception
            Return False
        End Try
    End Function

#End Region

#Region "Tratamiento de teclas HotKey"

    ''' <summary>
    ''' Obtiene o establece la combinación de teclas que permirá abrir el formulario de mantenimiento de la política de restricciones UI
    ''' para este componente 
    ''' (Inicialmente ofrece como combinación: CTR-?, aunque deshabilitada)
    ''' </summary>
    ''' <remarks>El uso de HotKey está pensado para facilitar la configuración de la seguridad desde el tiempo de ejecución, pero
    ''' en las fases de desarrollo y prueba de las aplicaciones
    ''' </remarks>
    Public Shared Property HotKey() As HotKey
        Get
            Return _hotKey
        End Get
        Set(ByVal value As HotKey)
            _hotKey = value
            RaiseEvent HotKeyChanged()
        End Set
    End Property
    Private Shared _hotKey As New HotKey(Keys.End, Keys.Control Or Keys.Alt, False)

    ''' <summary>
    ''' Permite consultar y establecer si la combinación de teclas que permirá abrir el formulario de mantenimiento de la seguridad para este componente está o no habilitada
    ''' </summary>
    ''' <remarks>Inicialmente estará deshabilitada</remarks>
    Public Shared Property AllowedHotKey() As Boolean
        Get
            Return HotKey.Enabled
        End Get
        Set(ByVal value As Boolean)
            If _hotKey.Enabled <> value Then
                _hotKey.Enabled = value
                RaiseEvent HotKeyChanged()
            End If
        End Set
    End Property

#End Region


#Region "Utilidades para tratamiento de roles"

    Protected Friend Shared CommonRolesAUX As List(Of Rol)
    Protected Friend Shared ParticularRolesAUX As Rol()

    ''' <summary>
    ''' Devuelve una cadena con la representación de los roles facilitada en el array <paramref name="roles"/>. Cada rol de entrada se mostrará
    ''' con su alias si está disponible y si no con su ID. Estos roles estarán separados en la cadena de salida por comas.
    ''' Para obtener un alias a partir de su ID se utilizarán los diccionarios de roles comunes y particulares existentes en el entorno a no ser
    ''' que se hayan cargado unos roles auxiliares en <see cref="CommonRolesAUX"/> y <see cref="ParticularRolesAUX"/>, en cuyo caso se utilizarán
    ''' estos últimos.
    ''' </summary>
    ''' <param name="IDControlRestrictedUI ">Identificador del componente de seguridad sobre el se analizan los roles (necesario pues cada componente
    ''' puede tener roles particulares, propios)</param>
    Protected Friend Shared Function RolesToStrUsingAlias(ByVal roles As Integer(), ByVal IDControlRestrictedUI As String _
                           ) As String
        Dim sec As ComponentSecurity = Nothing
        Dim cadRoles As String = ""
        Dim sep As String = ""
        Dim particularRoles As Rol() = ParticularRolesAUX
        Dim commonRoles As List(Of Rol) = CommonRolesAUX

        If particularRoles Is Nothing Then
            If SecurityEnvironment.ComponentsSecurity.TryGetValue(IDControlRestrictedUI, sec) Then
                particularRoles = sec.Roles
            End If
        End If
        If commonRoles Is Nothing Then
            commonRoles = SecurityEnvironment.CommonRoles
        End If

        If particularRoles Is Nothing AndAlso SecurityEnvironment.CommonRoles.Count = 0 Then
            Return Util.ConvertToString(roles)
        Else
            If roles IsNot Nothing Then
                For Each r As Integer In roles
                    cadRoles += sep + RolToStrTryingAlias(r, particularRoles, commonRoles)
                    sep = ","
                Next
            End If
            Return cadRoles
        End If
    End Function


    ''' <summary>
    ''' Devuelve una cadena con la representación del rol facilitado a través de su ID en <paramref name="rolID"/>. Este rol se mostrará
    ''' con su alias si está disponible y si no con su ID.
    ''' Para obtener el alias a partir de su ID se utilizarán los diccionarios de roles comunes y particulares que se aporten como parámetro.
    ''' </summary>
    Public Shared Function RolToStrTryingAlias(ByVal rolID As Integer, ByVal particularRoles As Rol(), ByVal commonRoles As IList(Of Rol)) As String
        If particularRoles IsNot Nothing Then
            For Each Rol As Rol In particularRoles
                If Rol.ID = rolID Then Return Rol.Alias
            Next
        End If

        If commonRoles IsNot Nothing Then
            For Each Rol As Rol In commonRoles
                If Rol.ID = rolID Then Return Rol.Alias
            Next
        End If

        Return rolID.ToString
    End Function

    ''' <summary>
    ''' Devuelve el ID asociado al alias de rol facilitado en <paramref name="aliasRol"/>. Para realizar la traducción se utilizarán 
    ''' como diccionarios la definición de roles comunes y roles particulares que se aporten como parámetro.
    ''' </summary>
    ''' <remarks>Si el alias no es reconocido devolverá 999999</remarks>
    Public Shared Function AliasToRolID(ByVal aliasRol As String, ByVal particularRoles As Rol(), ByVal commonRoles As IList(Of Rol)) As Integer
        If particularRoles IsNot Nothing Then
            For Each Rol As Rol In particularRoles
                If Rol.Alias.ToUpper = aliasRol.ToUpper Then Return Rol.ID
            Next
        End If

        If commonRoles IsNot Nothing Then
            For Each Rol As Rol In commonRoles
                If Rol.Alias.ToUpper = aliasRol.ToUpper Then Return Rol.ID
            Next
        End If

        ShowError(Constants.ERROR_ROL_NOTRECOGNIZED + aliasRol, Nothing)
        Return 999999
    End Function

    ''' <summary>
    ''' Devuelve un array de enteros con los ID asociados a los roles incluidos en la cadena facilitada en <paramref name=" rolesConAlias "/> 
    ''' en la que pueden aparecer tanto directamente IDs como alias. Para traducir un alias a su ID se utilizarán los diccionarios de roles comunes
    ''' y particulares existentes en el entorno a no ser que se aporten como parámetro, en cuyo caso se utilizarán estos últimos.
    ''' </summary>
    ''' <param name="IDControlRestrictedUI ">Identificador del componente de seguridad sobre el se analizan los roles (necesario pues cada componente
    ''' puede tener roles particulares, propios)</param>
    Public Shared Function GetRolesID(ByVal rolesWithAlias As String, ByVal IDControlRestrictedUI As String) As Integer()
        Dim sec As ComponentSecurity = Nothing
        Dim particularRoles As Rol() = ParticularRolesAUX
        Dim commonRoles As List(Of Rol) = CommonRolesAUX

        If particularRoles Is Nothing Then
            If SecurityEnvironment.ComponentsSecurity.TryGetValue(IDControlRestrictedUI, sec) Then
                particularRoles = sec.Roles
            End If
        End If
        If commonRoles Is Nothing Then
            commonRoles = SecurityEnvironment.CommonRoles
        End If

        Dim rolesStr As String() = Util.ConvierteEnArrayStr(rolesWithAlias)
        Dim rolesID(rolesStr.Length - 1) As Integer

        For i As Integer = 0 To rolesStr.Length - 1
            If Not Integer.TryParse(rolesStr(i), rolesID(i)) Then
                rolesID(i) = AliasToRolID(rolesStr(i), particularRoles, commonRoles)
            End If
        Next

        Return rolesID

    End Function

#End Region

#Region "Utilidades para tratamiento de permisos y grupos"

    ''' <summary>
    ''' Descompone la definición de permisos facilitada en permisos propiamente dichos y grupos
    ''' </summary>
    Public Shared Sub GetAuthorizationsAndGroups(ByVal AuthorizationsDefinition As String(), ByRef Authorizations As String(), ByRef Groups As Group())
        Dim cad As String

        If AuthorizationsDefinition Is Nothing Then
            Authorizations = Nothing
            Groups = Nothing
            Exit Sub
        End If

        Dim AuthorizationsList As New List(Of String)
        Dim lGroups As New List(Of Group)

        For Each cad In AuthorizationsDefinition
            If cad = "" Then Continue For
            Select Case cad(0)
                Case "$"c
                    Try
                        Dim group As New Group
                        Dim pos As Integer
                        pos = cad.IndexOf("="c)
                        group.Name = cad.Substring(1, pos - 1).Trim
                        group.Controls = cad.Substring(pos + 1).Replace(" ", "").Split(","c)
                        lGroups.Add(group)
                    Catch ex As Exception
                        ShowError(Constants.GROUP_INCORRECT + cad)
                    End Try

                Case Else  'Case "+"c, "-"c
                    AuthorizationsList.Add(cad)
            End Select
        Next
        Authorizations = AuthorizationsList.ToArray
        Groups = lGroups.ToArray
    End Sub

    ''' <summary>
    ''' Devuelve la relación de permisos contenida en definición de seguridad facilitada como parámetro
    ''' </summary>
    Public Shared Function GetRestrictions(ByVal authorizationsDefinition As String()) As String()
        Dim _authorizations As String() = Nothing
        Dim _groups As Group() = Nothing
        If authorizationsDefinition Is Nothing Then
            Return New String(0) {""}
        Else
            GetAuthorizationsAndGroups(authorizationsDefinition, _authorizations, _groups)
            Return _authorizations
        End If
    End Function

    ''' <summary>
    ''' Devuelve la relación de grupos contenida en definición de seguridad facilitada como parámetro
    ''' </summary>
    Public Shared Function GetGroups(ByVal authorizationsDefinition As String()) As Group()
        Dim authorizations As String() = Nothing
        Dim groups As Group() = Nothing
        If authorizationsDefinition Is Nothing Then
            Return Nothing
        Else
            GetAuthorizationsAndGroups(authorizationsDefinition, authorizations, groups)
            Return groups
        End If
    End Function

    ''' <summary>
    ''' Devuelve los grupos y permisos establecidos a nivel de entorno de seguridad para el control de seguridad indicado, serializados
    ''' como aparecen en la propiedad PermisosDefinicion del componente
    ''' </summary>
    ''' <param name="IDControlRestrictedUI "></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetRestrictionsDefinition(ByVal IDControlRestrictedUI As String) As String()
        Dim sec As ComponentSecurity = Nothing
        ComponentsSecurity.TryGetValue(IDControlRestrictedUI, sec)

        If sec IsNot Nothing AndAlso Not (sec.Authorizations Is Nothing And sec.Groups Is Nothing) Then
            Dim nGroups As Integer = 0, nAuthorizations As Integer = 0
            Dim cad As String, x As Integer = 0

            If sec.Authorizations IsNot Nothing Then nAuthorizations = sec.Authorizations.Length
            If sec.Groups IsNot Nothing Then nGroups = sec.Groups.Length
            Dim def(nGroups + nAuthorizations - 1) As String

            If sec.Groups IsNot Nothing Then
                For Each g As Group In sec.Groups
                    cad = "$" + g.Name + "= " + Util.ConvertToString(g.Controls)
                    def(x) = cad
                    x += 1
                Next
            End If
            If sec.Authorizations IsNot Nothing Then sec.Authorizations.CopyTo(def, x)
            Return def
        Else
            Return New String(0) {""}
        End If
    End Function

#End Region

#Region "Varios"

    ''' <summary>
    ''' Ruta hacia un archivo de configuración, con definición de seguridad. Se utiliza como valor por defecto
    ''' a ofrecer si el componente no fuerza ninguna ruta propia. Este ruta se actualiza con la última asignación
    ''' de la propiedad FicheroConfiguración de cualquier componente de seguridad (de restricciones UI).
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared ConfigFile As String

    ''' <summary>
    ''' Controla la llamada automática al método RegistrarControlesDeFormulario de cada componente de seguridad, lo que fuerza el registro 
    ''' o actualización de los controles existentes en el formulario o contenedor en donde está incrustado ese componente ControlSeguridad.
    ''' </summary>
    ''' <remarks>La llamada automática (si esta propiedad es True) se producirá al inicializarse el control y tras la incorporación o eliminación
    ''' en el EntornoSeguridad de alguna factoría de adaptadores (lo que permitirá descubrir más o menos controles)</remarks>
    Public Shared Property AutomaticUpdateOfControlsFile() As Boolean
        Get
            Return _automaticUpdateOfControlsFile
        End Get
        Set(ByVal value As Boolean)
            If _automaticUpdateOfControlsFile <> value Then
                _automaticUpdateOfControlsFile = value
                If value Then RaiseEvent ControlAdapterFactoriesChanged() ' Así forzaremos a que se actualicen los archivos de los formularios abiertos
            End If
        End Set
    End Property
    Protected Shared _automaticUpdateOfControlsFile As Boolean = True

#End Region



#Region "Escucha eventos IHost"
    ''' <summary>
    ''' Atiende el evento CambioEstado del objeto que implemente la interface IHost
    ''' </summary>
    ''' <param name="_ID"></param>
    ''' <param name="_instanceID"></param>
    ''' <param name="nuevoEstado"></param>
    ''' <remarks>Sólo se deberá atender este evento si el ID del control al que va dirigido, así como la instancia es Nothing o coincide con la del control que lo escucha</remarks>
    Private Shared Sub OnStateChanged(ByVal _ID As String, ByVal _instanceID As String, ByVal nuevoEstado As Integer)
        RaiseEvent StateChanged(_ID, _instanceID, nuevoEstado)
    End Sub

    ''' <summary>
    ''' Atiende el evento CambioRoles del objeto que implemente la interface IHost
    ''' </summary>
    ''' <param name="_ID"></param>
    ''' <param name="_instanceID"></param>
    ''' <remarks>Sólo se deberá atender este evento si el ID del control al que va dirigido, así como la instancia es Nothing o coincide con la del control que lo escucha</remarks>
    Private Shared Sub OnRolesChanged(ByVal _ID As String, ByVal _instanceID As String)
        RaiseEvent RolesChanged(_ID, _instanceID)
    End Sub

#End Region


End Class

