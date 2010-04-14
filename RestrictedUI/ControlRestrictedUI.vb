Option Strict On

Imports System.ComponentModel
Imports System.ComponentModel.Design
Imports System.Drawing.Design

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


''' <summary>
''' Componente base de la librer�a de Interface Restringida (<see cref="RestrictedUI"/>): permite restringir la visibilidad y estado de habilitaci�n de 
''' los controles incluidos en un formulario o control de usuario en base a una definici�n de seguridad
''' establecida, en la que intervienen el estado actual de la aplicaci�n en dicho formulario o contenedor as� como el
''' rol o roles que tenga el usuario de la aplicaci�n.
''' </summary>
''' <remarks>Debe ser heredado</remarks>
Public MustInherit Class ControlRestrictedUI
    Implements INotifyPropertyChanged, ISupportInitialize

    Const ERROR_READING_CONTROLSFILE As String = "Se produjo un error al leer el archivo de controles: "

    ''' <summary>
    ''' Ocurre justo antes de llegar a permitir o impedir el cambio de la propiedad Visible o Enabled, para dar la opci�n 
    ''' de permitir o no el cambio en base a una l�gica m�s compleja
    ''' </summary>
    ''' <param name="adaptControl">Adaptador del control que se est� supervisando</param>
    ''' <param name="type ">Indica si la propiedad que est� siendo modificada es Enabled o Visible</param>
    ''' <param name="allowChange ">Indica el resultado de la pol�tica de seguridad para este control en base a la situaci�n
    ''' actual (estado y roles)
    ''' </param>
    ''' <remarks>S�lo se verifica esta seguridad (y se dispara por tanto este evento) cuando la modificaci�n busca hacer visible
    ''' o habilitar el control, no cuando lo hace invisible o lo deshabilita</remarks>
    Public Event BeforeApplyingRestriction(ByVal adaptControl As IControlAdapter, ByVal type As TChange, ByRef allowChange As Boolean)

    Private _initializing As Boolean

#Region "Configuraci�n de la Seguridad: Interface P�blica"

    ''' <summary>
    ''' <para>Identificador de este componente <see cref=" ControlRestrictedUI "/>, controlador de restricciones. </para>
    ''' <para>A trav�s de este identificador podr� leerse/actualizarse la seguridad 
    ''' asociada (definici�n de restricciones) desde un archivo.</para>
    ''' <para>Ser� tambi�n la clave a partir de la cual se indexar� en EntornoSeguridad.RestriccionesContenedor</para>
    ''' </summary>
    ''' <remarks>Se le deber�a asignar un valor. Por defecto se le asignar� un GUID</remarks> 
    <Category("Configuration"), _
    Description("Identificador del componente ControlRestriccionesUI")> _
    Public Property ID() As String
        Get
            Return _ID
        End Get
        Set(ByVal value As String)
            _ID = value
            NotifyPropertyChanged("ID")
        End Set
    End Property
    Private _ID As String = Guid.NewGuid().ToString("N")

    ''' <summary>
    ''' Identificador de la instancia del componente
    ''' </summary>
    ''' 
    ''' <remarks>
    ''' <para>El componente pasar� este identificador (junto con la propiedad <see cref="ID"/>) al
    ''' objeto <see cref="IHost"/>, para permitirle de este modo determinar entre otras cosas qu� estado ofrecer 
    ''' (Es posible tener m�ltiples pantallas tramitando diferentes entidades)</para>
    ''' <para>Lo normal ser� que la aplicaci�n le asigne un ID. Por defecto el ID ser� "00"</para>
    ''' </remarks> 
    <Browsable(False)> _
    Public Property InstanceID() As String
        Get
            If _instanceID IsNot Nothing Then
                Return _instanceID
            Else
                Return "00"
            End If
        End Get
        Set(ByVal value As String)
            _instanceID = value
            NotifyPropertyChanged("InstanceID")
        End Set
    End Property
    Private _instanceID As String = Nothing


    ''' <summary>
    ''' Configuraci�n de las restricciones (permisos y prohibiciones) que se establecer�n. 
    ''' </summary>
    ''' <remarks>
    ''' M�s informaci�n sobre el tratamiento de las restricciones en <see cref="UIRestrictions "/> y en <see cref="RestrictionOnControl "/>.
    ''' <seealso cref="UIRestrictions "/>
    ''' <seealso cref="RestrictionOnControl "/>
    ''' </remarks>
    <Category("Configuration"), _
    Description("Configuraci�n de las restricciones que se establecer�n (permisos y prohibiciones). Se interpretan a nivel de control individual: " + _
                "s�lo se impedir�n las modificaciones de las propiedades Visible / Enabled en las situaciones aqu� se�aladas " + _
                "(Las prohibiciones tendr�n prioridad sobre los permisos: primero se aplicar�n los permisos y luego se restringir�n en base a las prohibiciones)")> _
    <Editor(GetType(RestrictionsDefinitionEditor), GetType(UITypeEditor))> _
    Public Property RestrictionsDefinition() As String()
        Get
            If _priorityEmbeddedSecurity Then
                Return _restrictionsDefinition
            Else
                Return SecurityEnvironment.GetRestrictionsDefinition(ID)
            End If
        End Get
        Set(ByVal value As String())
            _restrictionsDefinition = value
            If Me.DesignMode OrElse Not _initializing Then
                _priorityEmbeddedSecurity = True
            End If
            If Not _initializing Then ReinitializeSecurity()
            NotifyPropertyChanged("RestrictionsDefinition")
            NotifyPropertyChanged("RestrictionsDefinitionEmbedded")
        End Set
    End Property
    Protected _restrictionsDefinition As String()

    ''' <summary>
    ''' Devuelve las restricciones tal y como se han definido y asociado directamente al componente de seguridad (inicialmente en tiempo de dise�o,
    ''' aunque tal vez modificadas posteriormente).
    ''' </summary>
    ''' <remarks>
    ''' Si <see cref="PriorityEmbeddedSecurity "/> es True entonces �sta ser� la pol�tica que se aplicar� en el componente, en caso
    ''' contrario ser� la que se haya cargado en el objeto singleton <see cref="SecurityEnvironment"/>. ya sea leyendo desde un archivo
    ''' o una cadena de texto.
    ''' </remarks>
    <Browsable(False)> _
    Public ReadOnly Property RestrictionsDefinitionEmbedded() As String()
        Get
            Return _restrictionsDefinition
        End Get
    End Property


    ''' <summary>
    ''' Determina si la pol�tica que debe mandar en el componente es la que resulta de aplicar las restricciones tal y
    ''' como se han definido y asociado directamente al componente (seguridad embebida) o por el contrario la que se haya cargado 
    ''' en el EntornoSeguridad, ya sea leyendo desde un archivo o una cadena de texto.
    ''' </summary>
    Protected Friend Property PriorityEmbeddedSecurity() As Boolean
        Get
            Return _priorityEmbeddedSecurity
        End Get
        Set(ByVal value As Boolean)
            If _priorityEmbeddedSecurity <> value Then
                _priorityEmbeddedSecurity = value
            End If
            NotifyPropertyChanged("RestrictionsDefinition")
            NotifyPropertyChanged("PriorityEmbeddedSecurity")
            NotifyPropertyChanged("Groups")
            NotifyPropertyChanged("Restrictions")
        End Set
    End Property
    Private _priorityEmbeddedSecurity As Boolean = True
    Private _priorityEmbeddedSecurityBAK As Boolean = True

    ''' <summary>
    ''' Devuelve la relaci�n de grupos que pueda haber definida en <see cref="RestrictionsDefinition "/>.
    ''' </summary>
    <Browsable(False)> _
    Public ReadOnly Property Groups() As Group()
        Get
            Return SecurityEnvironment.GetGroups(RestrictionsDefinition)
        End Get
    End Property


    ''' <summary>
    ''' Devuelve la relaci�n de restricciones (permisos y prohibiciones) que pueda haber definida en <see cref="RestrictionsDefinition"/>, sin
    ''' incluir la definici�n de grupos, en el caso de que la hubiera.
    ''' </summary>
    <Browsable(False)> _
    Public ReadOnly Property Restrictions() As String()
        Get
            Return SecurityEnvironment.GetRestrictions(RestrictionsDefinition)
        End Get
    End Property


    '''<summary>
    '''Obtiene o establece la ruta hacia un fichero de configuraci�n, a utilizar fundamentalmente en tiempo de dise�o.
    '''</summary>
    '''<remarks>
    '''<para>El fichero de configuraci�n hace posible ofrecer en tiempo de dise�o y durante la definici�n de la seguridad 
    '''en el formulario <see cref=" FrmRestrictionsUIDefinition "/> la relaci�n de roles y estados a utilizar, as� como 
    '''de factor�as de adaptadores adicionales, para as� 'descubrir' nuevos controles en tiempo de dise�o.
    '''de control adicionales.</para>
    '''<para>Las propias definiciones de restricciones de seguridad, de todos o s�lo algunos componentes pueden estar contenidas
    '''en este archivo. Estas restricciones pueden cargarse en tiempo de ejecuci�n mediante <see cref="SecurityEnvironment.LoadFrom"/> 
    ''' as� como cargarse y grabarse a voluntad desde el formulario <see cref=" FrmRestrictionsUIDefinition "/>, en tiempo de
    ''' dise�o o de ejecuci�n.</para>
    '''<para>Si el componente no fuerza ninguna ruta se ofrecer� la que pueda tener configurada el propio objeto
    ''' <see cref="SecurityEnvironment"/>, la cual se actualiza con la �ltima asignaci�n de esta propiedad (de cualquier
    '''  componente)</para>
    ''' <para>Nota: La ruta deber� estar establecida de manera absoluta o relativa a la carpeta de la soluci�n (.sln)</para> 
    ''' <seealso cref=" FrmRestrictionsUIDefinition "/>
    ''' <seealso cref="SecurityEnvironment.LoadFrom"/>
    '''</remarks>
    <Category("Configuration"), _
    Description("Ruta hacia un fichero de configuraci�n, a utilizar por defecto desde el formulario de definici�n de restricciones " + _
       "permitiendo ofrecer en tiempo de dise�o la relaci�n de roles y estados a utilizar, as� como factor�as de adaptadores de " + _
       "control adicionales. Las propias definiciones de restricciones de seguridad, de todos o s�lo algunos componentes pueden estar contenidas " + _
       "en este archivo. " + vbCrLf + _
       "Notas: - La ruta deber� estar establecida de manera absoluta o relativa a la carpeta de la soluci�n (.sln)" + vbCrLf + _
       "       - Si el componente no fuerza ninguna ruta se ofrecer� la que pueda tener configurada el propio objeto SecurityEnvironment " + _
       "  que se actualiza con la �ltima asignaci�n de esta propiedad (de cualquier componente)")> _
    Public Property ConfigFile() As String
        Get
            If _configFile Is Nothing Then
                Return SecurityEnvironment.ConfigFile
            Else
                Return _configFile
            End If
        End Get
        Set(ByVal value As String)
            _configFile = value
            SecurityEnvironment.ConfigFile = value
            NotifyPropertyChanged("ConfigFile")
        End Set
    End Property
    Private _configFile As String

    ''' <summary>
    ''' Nombre del archivo sobre el que podr� escribirse la relaci�n los controles contenidos en el formulario o 
    ''' control de usuario controlado por este componente.
    ''' </summary>
    ''' <remarks>
    ''' <para>Este archivo hace posible ofrecer en tiempo de dise�o desde el formulario <see cref="FrmRestrictionsUIDefinition "/> 
    ''' controles que se crear�n din�micamente. En aplicaciones WinForms los controles definidos en tiempo de dise�o pueden conocerse
    ''' directamene en ese formulario, tambi�n en tiempo de dise�o, pero en aplicaciones Web no es posible (al menos no he sido capaz)
    ''' y para solucionarlo se ofrece el uso de este archivo. Tras una primera ejecuci�n de la aplicaci�n es posible alimentar autom�ticamente
    ''' este archivo, y as� contar con los controles ya despu�s, en tiempo de dise�o.</para>
    ''' <para>Nota: La ruta deber� estar establecida de manera absoluta o relativa a la carpeta de la soluci�n (.sln)</para>
    ''' <para>V�ase tambi�n <see cref="SecurityEnvironment.AutomaticUpdateOfControlsFile "/> y <see cref="RegisterControls"/> </para>
    ''' <seealso cref="SecurityEnvironment.AutomaticUpdateOfControlsFile "/>
    ''' <seealso cref="RegisterControls"/>
    ''' </remarks>
    <Category("Configuraci�n"), _
    Description("Nombre del archivo sobre el que podr� escribirse la relaci�n los controles contenidos en el formulario o control de usuario controlado por este componente.")> _
    Public Property ControlsFile() As String
        Get
            Return _controlsFile
        End Get
        Set(ByVal value As String)
            _controlsFile = value
            NotifyPropertyChanged("ControlsFile")
        End Set
    End Property
    Private _controlsFile As String = ""

#End Region

#Region "Controles Externos de Seguridad: Interface p�blica"

    ''' <summary>
    ''' Deshabilita temporalmente (pausa) la seguridad impuesta por la pol�tica de restricciones del componente, de manera que 
    ''' los posteriores cambios en las propiedades supervisadas (Visible y Enabled) sean permitidos.
    ''' </summary>
    ''' <remarks>Al restablecer a False (valor inicial) la definici�n de la seguridad se restablecer�: algunos controles se 
    ''' deshabilitar�n u ocultar�n en consecuencia
    ''' </remarks>
    Property Paused() As Boolean
        Get
            Return _paused
        End Get
        Set(ByVal value As Boolean)
            If _paused <> value Then
                _paused = value
                If Not _paused And Not Me.DesignMode And _defSecurity IsNot Nothing Then
                    ReviseAppliedSecurity(Nothing)
                End If
                NotifyPropertyChanged("Paused")
            End If
        End Set
    End Property
    Private _paused As Boolean = False

    ''' <summary>
    ''' Fuerza la reinicializaci�n de la configuraci�n de la seguridad, procesando las restricciones establecidos, asignando manejadores de
    ''' eventos y revisando sobre cada control supervisado las propiedades Visible y Enabled (o las correspondientes a cada control).
    ''' </summary>
    ''' <remarks>
    ''' Este m�todo es llamado internamente en respuesta a una modificaci�n de la seguridad. Se ofrece como m�todo p�blico para contemplar la
    ''' creaci�n din�mica de controles: cuando se inicializa la seguridad al inicio no todos los controles tienen por qu� haber sido creados (ej: 
    ''' columnas de un DataGridView a�adidas din�micamente). Esos controles pueden estar contemplados en la definici�n de seguridad desde el
    ''' principio, antes de haber sido creados.
    ''' </remarks>
    Public Sub ReinitializeSecurity()
        Try
            If Me.DesignMode Or _defSecurity Is Nothing Then Exit Sub

            Dim lInitial As IList(Of IControlAdapter) = Me.SupervisedControls(True, True)

            _defSecurity = New UIRestrictions(Restrictions, ID, ParentControl, Groups)
            RemoveEventHandlers(lInitial)
            AddEventHandlers()
            ReviseAppliedSecurity(lInitial)

        Catch ex As Exception
            SecurityEnvironment.ShowError("ControlRestrictedUI.ReinitializeSecurity:" + ex.Message, Me.ParentControl, ID)
        End Try
    End Sub


    ''' <summary>
    ''' Fuerza la visibilidad del control o controles indicados, con independencia de que en base a las restricciones existentes 
    ''' pueda o no mostrarse.
    ''' </summary>
    ''' <param name="control">Si es Nothing se forzar� la vibilidad de todos los controles que est�n siendo supervisados</param>
    ''' <remarks>
    ''' <para>Si el control cuya visibilidad ha sido forzada se hace invisible, su posible pr�xima visibilidad quedar� supeditada 
    ''' a las restricciones definidas</para>
    ''' <para>La modificaci�n de la definici�n de la seguridad o un cambio en la situaci�n actual (definici�n de la seguridad,
    ''' cambios de roles del usuario, de estado..) que obligue a revisar la seguridad aplicada podr� hacer invisible el control/es</para>
    ''' </remarks>
    Public Sub ForceVisibility(Optional ByVal control As Object = Nothing)
        Dim list As IList(Of IControlAdapter) = Me.SupervisedControls(False, True)

        _decidingChange = True
        If Not list Is Nothing Then
            For Each c As IControlAdapter In list
                If control Is Nothing OrElse c.Control Is control Then
                    c.Visible = True
                End If
            Next
        End If
        _decidingChange = False
    End Sub

    ''' <summary>
    ''' Fuerza la habilitaci�n del control o controles indicados, con independencia de que en base a las restricciones existentes 
    ''' pueda o no estar habilitado
    ''' </summary>
    ''' <param name="control">Si es Nothing se forzar� el Enabled=true de todos los controles que est�n siendo supervisados</param>
    ''' <remarks>
    ''' <para>Si el control cuyo Enabled ha sido forzado a True se deshabilita, s�lo ser� habilitado nuevamente en base a las 
    ''' restricciones definidas</para>
    ''' <para>La modificaci�n de la definici�n de la seguridad o un cambio en la situaci�n actual (definici�n de la seguridad,
    ''' cambios de roles del usuario, de estado..) que obligue a revisar la seguridad aplicada podr� deshabilitar el control/es</para>
    ''' </remarks>
    Public Sub ForceEnabled(Optional ByVal control As Object = Nothing)
        Dim lista As IList(Of IControlAdapter) = Me.SupervisedControls(True, False)

        _decidingChange = True
        If Not lista Is Nothing Then
            For Each c As IControlAdapter In lista
                If control Is Nothing OrElse c.Control Is control Then
                    c.Enabled = True
                End If
            Next
        End If
        _decidingChange = False
    End Sub

    ''' <summary>
    ''' Elimina el control de la lista de controles supervisados, de manera que los sucesivos cambios en las propiedades de visibilidad y Enabled
    ''' no ser�n interceptadas
    ''' </summary>
    ''' <returns><b>True</b> si el control estaba supervisado y ha sido excluido correctamente, <b>False</b> en caso contrario</returns>
    ''' <param name="Control"></param>
    ''' <remarks>La revisi�n de la seguridad aplicada debido a alg�n cambio en la situaci�n actual (definici�n de la seguridad,
    ''' cambios de roles del usuario, de estado..) no afectar� a este control
    ''' </remarks>
    Public Function ExcludeControl(ByVal Control As Object) As Boolean
        If _defSecurity Is Nothing Or Control Is Nothing Then Return False

        Try
            For Each c As IControlAdapter In Me.SupervisedControls(True, False)
                If c.Control Is Control Then
                    Return _defSecurity.ExcludeControl(Control)
                End If
            Next
            Return False

        Catch ex As Exception
            SecurityEnvironment.ShowError("ControlRestrictedUI.ExcludeControl:" + ex.Message, Me.ParentControl, ID)
            Return False
        End Try

    End Function

    ''' <summary>
    ''' Fuerza el registro o actualizaci�n de los controles existentes en el formulario o contenedor en donde est�
    ''' incrustado este componente.
    ''' </summary>
    ''' <remarks>
    ''' <para>El archivo en donde se escribe debe estar definido en la propiedad <see cref="ControlsFile "/> y debe
    ''' adem�s existir, aunque sea vac�o. La creaci�n en tiempo de dise�o de un componente de seguridad asegurar� que ese
    ''' archivo exista (si est� establecida la propiedad <see cref="ControlsFile "/>): se crear� vac�o si no se encuentra.</para>
    ''' <para>El componente llamar� autom�ticamente a este m�todo al inicializarse y tras la incorporaci�n o eliminaci�n
    ''' en en objeto <see cref="SecurityEnvironment"/> de alguna factor�a de adaptadores (lo que permite descubrir m�s o menos controles), 
    ''' pero s�lo si la propiedad <see cref="SecurityEnvironment.AutomaticUpdateOfControlsFile "/> es True (lo es por defecto).</para>
    ''' 
    ''' <para>Podr� hacerse tambi�n de manera expl�cita, normalmente en el evento Load o cuando est�n todos los controles din�micos creados.
    ''' En aplicaciones Web el uso de este archivo es la �nica forma de determinar en tiempo de dise�o cu�les son los controles
    ''' que contiene el formulario/contenedor (En tpo de dise�o las propiedades Controls no contienen ning�n elemento. Tampoco funciona
    ''' el uso de Reflection para determinar las propiedades WebControl (funciona en tpo de ejecuci�n, no de dise�o)
    ''' En aplicaciones WinForms este archivo permitir� definir la seguridad sobre controles que se crear�n din�micamente durante la
    ''' vida del formulario o contenedor. </para>
    ''' </remarks>
    Public Sub RegisterControls()
        Dim lista As String = ""
        Dim file As String = ControlsFile
        If SecurityEnvironment.AdaptFilePath(file, Me.DesignMode) Then
            ' La identificaci�n del control padre podemos hacerla sin problemas en WinForms, pero en Web no es posible (al menos no lo consigo)
            ' Por ello, en lugar de apoyarnos en el control padre para identificar el formulario, utilizaremos el identificador del componente
            ' de seguridad que est� en �l embebido
            'Dim idParent As String = Util.GetControlPadreID(_parentControl)
            Dim idParent As String = Me.ID

            Dim controlsLists As Dictionary(Of String, String) = ReadControls(SecurityEnvironment.ReadFile(file))
            MakeControlsListOf(lista, SecurityEnvironment.GetAdapter(_parentControl))
            lista = "[" + idParent + "]" + vbCrLf + lista
            controlsLists.Remove(idParent)
            controlsLists.Add(idParent, lista)
            My.Computer.FileSystem.DeleteFile(file)
            lista = ""
            For Each compName As String In controlsLists.Keys
                lista += controlsLists(compName) + vbCrLf + vbCrLf
            Next
            My.Computer.FileSystem.WriteAllText(file, lista, True)
        End If
    End Sub


    ''' <summary>
    ''' Cierra la gesti�n de la seguridad en este componente, eliminando los manejadores a los que se ha hecho subscribir a los distintos
    ''' controles supervisados.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub FinalizeSecurity()
        Try
            If ParentControl IsNot Nothing Then
                _defSecurity.Clear()
                RemoveHandler SecurityEnvironment.ControlAdapterFactoriesChanged, AddressOf OnControlAdapterFactoriesChanged
                RemoveHandler SecurityEnvironment.SecurityChanged, AddressOf OnSecurityChanged
                RemoveHandler SecurityEnvironment.SecurityChangedWithCancelInMind, AddressOf OnSecurityChangedWithCancelInMind
                RemoveHandler SecurityEnvironment.HotKeyChanged, AddressOf OnHotKeyChanged
                RemoveHandler SecurityEnvironment.StateChanged, AddressOf OnStateChanged
                RemoveHandler SecurityEnvironment.RolesChanged, AddressOf OnRolesChanged

                RemoveEventHandlers(Me.SupervisedControls(True, True))
                If TypeOf (ParentControl) Is System.Windows.Forms.Control Then
                    RemoveHandler DirectCast(ParentControl, System.Windows.Forms.Control).KeyDown, AddressOf OnParentControlKeyDown
                    If TypeOf (ParentControl) Is System.Windows.Forms.Form Then
                        _keyPreviewOriginal = DirectCast(ParentControl, System.Windows.Forms.Form).KeyPreview
                    End If
                End If
            End If

        Catch ex As Exception
        End Try
    End Sub


#End Region


#Region "New / Finalice  / ToString"

    <System.Diagnostics.DebuggerNonUserCode()> _
    Public Sub New()
        MyBase.New()

        'El Dise�ador de componentes requiere esta llamada.
        InitializeComponent()
    End Sub

    Protected Overrides Sub Finalize()
        _defSecurity.Clear()
        _defSecurity = Nothing
    End Sub

    Private Sub ControlSeguridad_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed
        FinalizeSecurity()
    End Sub

    Public ReadOnly Property LongID() As String
        Get
            Dim parent As String = ""

            If ParentControl IsNot Nothing Then
                Dim grandParent As String = Util.GetParentID(ParentControl)
                parent = " (" + ParentControl.ToString + " / " + grandParent + ")"
            End If
            Return ID + " - " + InstanceID + parent
        End Get
    End Property

    Public Overrides Function ToString() As String
        Return LongID
    End Function

#End Region

#Region "Recuperar Form padre"
    ''' <summary>
    ''' Gets or sets the System.ComponentModel.ISite" of the Component
    ''' used to update ParentControl so it will be serialized to code
    ''' </summary>
    <Browsable(False)> _
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)> _
        Public Overrides Property Site() As System.ComponentModel.ISite
        Get
            Return MyBase.Site
        End Get
        Set(ByVal value As System.ComponentModel.ISite)
            MyBase.Site = value
            If Not MyBase.Site Is Nothing Then

                ' If the component is dropped onto a form during design-time,  
                ' set the ParentControl property.  
                Dim host As IDesignerHost = CType(value.GetService(GetType(IDesignerHost)), IDesignerHost)
                If host IsNot Nothing Then
                    Dim parent As Object = host.RootComponent
                    If Not parent Is Nothing Then
                        ParentControl = parent
                    End If
                End If

            End If

        End Set
    End Property


    Protected _parentControl As Object

    ''' <summary>
    ''' Obtiene o establece el control padre del componente. 
    ''' </summary>
    ''' <remarks>
    ''' Al establecerlo en ejecuci�n (se har� desde el dise�ador del contenedor) se subscribir� 
    ''' al evento HandleCreated (en WinForms) o al PreRender (en Web) con el m�todo <see cref="AddEventHandlers "/>.  
    ''' De esta manera cuando tenga lugar el evento (para entonces ya tendr� asociados sus controles hijos) se realizar� la
    ''' vigilancia de los controles hijos seleccionados, con la ayuda de los objetos <see cref="IControlAdapter"/> correspondientes 
    ''' </remarks>
    <Browsable(False)> _
    Public MustOverride Property ParentControl() As Object

#End Region

#Region "Gesti�n interna de la Seguridad"
    Private _defSecurity As UIRestrictions

    ''' <summary>
    ''' Inicializa la seguridad del componente, procesando las restricciones definidas, subscribi�ndose a determinados
    ''' eventos de <see cref="SecurityEnvironment"/>, y a�adiendo manejadores de eventos a los controles a supervisar seg�n la seguridad 
    ''' definida
    ''' </summary>
    Protected Sub InitializeSecurity(ByVal sender As Object, ByVal e As EventArgs)
        If Not ParentControl Is Nothing Then
            _defSecurity = New UIRestrictions(Restrictions, ID, ParentControl, Groups)
            AddHandler SecurityEnvironment.ControlAdapterFactoriesChanged, AddressOf OnControlAdapterFactoriesChanged
            AddHandler SecurityEnvironment.SecurityChanged, AddressOf OnSecurityChanged
            AddHandler SecurityEnvironment.SecurityChangedWithCancelInMind, AddressOf OnSecurityChangedWithCancelInMind
            AddHandler SecurityEnvironment.HotKeyChanged, AddressOf OnHotKeyChanged
            AddHandler SecurityEnvironment.StateChanged, AddressOf OnStateChanged
            AddHandler SecurityEnvironment.RolesChanged, AddressOf OnRolesChanged

            If TypeOf (ParentControl) Is System.Windows.Forms.Form Then
                _keyPreviewOriginal = DirectCast(ParentControl, System.Windows.Forms.Form).KeyPreview
            End If
            OnHotKeyChanged()  ' Verificamos si ya se encuentra establecida una HotKey
            AddEventHandlers()

            ' En WinForms este m�todo se llamar� en el evento HandleCreated del control padre (formulario)
            ' por lo que todas las propiedades Visible y Enabled ya establecidas ser�n controladas autom�ticamente
            ' Sin embargo, las propiedades de otros controles m�s espec�ficos como los UltraGrid no. De ah� la llamada siguiente
            ReviseAppliedSecurity(Nothing)

            If SecurityEnvironment.AutomaticUpdateOfControlsFile Then
                RegisterControls()
            End If
        End If
    End Sub

    ''' <summary>
    ''' Se ordena la supervisi�n de las propiedades Visible y Enabled. El adaptador de cada control (<see cref="IControlAdapter"/>) 
    ''' se suscribir� a los eventos correspondientes (ej: VisibleChanged y EnabledChanged o PreRender) 
    ''' </summary>
    Private Sub AddEventHandlers()

        If Not _defSecurity.Prohibitions Is Nothing Then
            For Each b As RestrictionOnControl In _defSecurity.Prohibitions
                If b.Visible Then
                    b.ControlAdapt.SuperviseVisible(Me)
                End If
                If b.Enabled Then
                    b.ControlAdapt.SuperviseEnabled(Me)
                End If
            Next
        End If

        If Not _defSecurity.Authorizations Is Nothing Then
            For Each b As RestrictionOnControl In _defSecurity.Authorizations
                If b.Visible Then
                    b.ControlAdapt.SuperviseVisible(Me)
                End If
                If b.Enabled Then
                    b.ControlAdapt.SuperviseEnabled(Me)
                End If
            Next
        End If

    End Sub

    ''' <summary>
    ''' Elimina la subscripci�n a los eventos que permiten controlar los cambios sobre las propiedades Visible y Enabled para la lista 
    ''' de adaptadores indicada 
    ''' </summary>
    ''' <remarks>Se invoca al cambiar la definici�n de la seguridad antes de llamar al m�todo <see cref="AddEventHandlers "/></remarks>
    Private Sub RemoveEventHandlers(ByVal lInitial As IList(Of IControlAdapter))
        For Each c As IControlAdapter In lInitial
            c.FinalizeSupervision()
        Next
    End Sub

    ''' <summary>
    ''' Referencia la propiedad cuyo cambio se est� controlando: Enabled o Visible
    ''' </summary>
    ''' <remarks>Las propiedades concretas depender�n del control implicado, lo que es abstraido a trav�s del adaptador (<see cref="IControlAdapter"/>) 
    ''' </remarks>
    Public Enum TChange
        Enabled = 0
        Visible = 1
    End Enum

    Private _decidingChange As Boolean = False

    ''' <summary>
    ''' Comprueba si el cambio sobre la propiedad indicada (<see cref="TChange"/>) es v�lido atendiendo a la definici�n de seguridad, 
    ''' a los roles del usuario y al estado actual de la aplicaci�n.
    ''' Si el cambio no es v�lido se deshar�, esto es, se establecer� nuevamente a False
    ''' </summary>
    ''' <param name="controlAdapt">Adaptador del control sobre el que se debe verificar el cambio</param>
    ''' <param name="type">Especifica si se debe verificar el cambio de Visible o Enabled</param>
    ''' <remarks>Se ofrece como m�todo p�blico para su uso por parte de los adaptadores de control (<see cref="IControlAdapter"/>) </remarks>
    Public Sub VerifyChange(ByVal controlAdapt As IControlAdapter, ByVal type As TChange)
        If _decidingChange Then Exit Sub
        _decidingChange = True

        Select Case type
            Case TChange.Enabled
                ' Si se ha establecido =False no hay nada que impedir
                If controlAdapt.Enabled Then
                    If Not ChangeAllowed(controlAdapt, type) Then
                        controlAdapt.Enabled = False
                    End If
                End If

            Case TChange.Visible
                ' Si se ha establecido =False no hay nada que impedir
                If controlAdapt.Visible Then
                    If Not ChangeAllowed(controlAdapt, type) Then
                        controlAdapt.Visible = False
                    End If
                End If
        End Select
        _decidingChange = False
    End Sub

    ''' <summary>
    ''' Comprueba si el cambio sobre la propiedad Visible o Enabled (o la correspondiente del control)
    ''' es v�lido atendiendo a las restricciones establecidas, a los roles del usuario y al estado actual
    ''' de la aplicaci�n.
    ''' </summary>
    ''' <param name="controlAdapt">Adaptador del control sobre el que se debe verificar el cambio</param>
    ''' <param name="type ">Especifica si se debe verificar el cambio de Visible o Enabled</param>
    ''' <returns><b>False</b> si el cambio no est� permitido. <b>True</b> en caso contrario</returns>
    Public Function ChangeAllowed(ByVal controlAdapt As IControlAdapter, ByVal type As TChange) As Boolean
        If _paused Then Return True ' Si est� pausada la supervisi�n en este control no impediremos nada. Tampoco generaremos el evento BeforeApplyingRestrinction

        Dim allowed As Boolean = True  ' Mientras no se demuestre lo contrario se permitir�

        Try
            ' Aplicar en primer lugar la l�gica de la regla de PERMISOS
            '-------
            ' Buscar un criterio por el que permitir
            For Each p As RestrictionOnControl In _defSecurity.Authorizations
                If Not p.ControlAdapt.Control Is controlAdapt.Control Then Continue For
                If (type = TChange.Visible And Not p.Visible) OrElse (type = TChange.Enabled And Not p.Enabled) Then Continue For
                allowed = False    ' A este control (y propiedad) se le ha aplicado una l�gica positiva: s�lo se permitir� el cambio a los explicitados
                'If p.rol <> 0 AndAlso Array.IndexOf(ActualRoles, p.rol) < 0 Then Continue For
                If Array.IndexOf(p.roles, 0) < 0 Then     ' 0 => Todos los roles
                    Dim authorizedRole As Boolean = False
                    For Each r As Integer In UserRoles
                        If Array.IndexOf(p.roles, r) >= 0 Then
                            authorizedRole = True
                            Exit For
                        End If
                    Next
                    If Not authorizedRole Then Continue For
                End If

                If Not p.states Is Nothing AndAlso Array.IndexOf(p.states, HostState) < 0 Then Continue For

                allowed = True
                Exit For
            Next

            If allowed Then

                ' Aplicar en segundo lugar la l�gica de la regla de PROHIBICIONES
                '-------
                ' Buscamos un rol del usuario para el que no se prohiba
                ' Si se encuentra se permite la modificaci�n. En caso contrario no
                For Each rol As Integer In UserRoles
                    allowed = True    ' Inicialmente suponemos que el rol no estar� restringido
                    For Each p As RestrictionOnControl In _defSecurity.Prohibitions
                        'If p.rol <> 0 And p.rol <> rol Then Continue For
                        If Array.IndexOf(p.roles, 0) < 0 AndAlso Array.IndexOf(p.roles, rol) < 0 Then Continue For
                        If p.ControlAdapt.Control IsNot controlAdapt.Control Then Continue For
                        If (type = TChange.Visible And Not p.Visible) OrElse (type = TChange.Enabled And Not p.Enabled) Then Continue For
                        If p.states IsNot Nothing AndAlso Array.IndexOf(p.states, HostState) < 0 Then Continue For

                        allowed = False
                        Exit For
                    Next
                    If allowed Then Exit For ' Este rol no est� bloqueado
                Next

            End If

            'Dar la opci�n de permitir o no el cambio en base a una l�gica m�s compleja
            RaiseEvent BeforeApplyingRestriction(controlAdapt, type, allowed)
            Return allowed

        Catch ex As Exception
            SecurityEnvironment.ShowError("ControlRestrictedUI.ChangeAllowed (" + controlAdapt.Identification(, Me) + ") :" + ex.Message, Me.ParentControl, ID)
            Return True
        End Try

    End Function

    ''' <summary>
    ''' Comprueba si tras el cambio de la situaci�n actual (definici�n de la seguridad,
    ''' cambios de roles del usuario, de estado..) hay que permitir o no los estados Visible y Enabled buscados.
    ''' </summary>
    ''' <param name="lInitial ">En el caso de un cambio en la definici�n de la seguridad recoge los controles que han estado vigil�ndose</param>
    ''' <remarks></remarks>
    Private Sub ReviseAppliedSecurity(ByVal lInitial As IList(Of IControlAdapter))
        If _paused Then Exit Sub ' Si est� pausada la supervisi�n en este control, no haremos nada. Se invocar� este m�todo cuando se restablezca

        Try
            _decidingChange = True

            ' ENABLED
            '----------
            Dim lFinal As IList(Of IControlAdapter) = SupervisedControls(True, False)

            If Not lInitial Is Nothing Then
                For Each c As IControlAdapter In lInitial
                    c.Enabled = ChangeAllowed(c, TChange.Enabled)
                Next
            End If
            For Each c As IControlAdapter In lFinal
                c.Enabled = ChangeAllowed(c, TChange.Enabled)
            Next


            ' VISIBLE
            '----------
            lFinal = SupervisedControls(False, True)

            If Not lInitial Is Nothing Then
                For Each c As IControlAdapter In lInitial
                    c.Visible = ChangeAllowed(c, TChange.Visible)
                Next
            End If
            For Each c As IControlAdapter In lFinal
                c.Visible = ChangeAllowed(c, TChange.Visible)
            Next

            _decidingChange = False

        Catch ex As Exception
            _decidingChange = False
            SecurityEnvironment.ShowError("ControlRestrictedUI.ReviseAppliedSecurity:" + ex.Message, Me.ParentControl, ID)
        End Try

    End Sub

    ''' <summary>
    ''' Devuelve una lista con todos los controles que se est�n supervisando
    ''' <param name="enabled"><b>true</b>: considerar los que tienen la propiedad Enabled controlada</param>
    ''' <param name="visible"><b>true</b>:Considerar los que tienen la propiedad Visible controlada</param>
    ''' </summary>
    Private Function SupervisedControls(ByVal cEnabled As Boolean, ByVal cVisible As Boolean) As IList(Of IControlAdapter)
        If _defSecurity Is Nothing Then Return Nothing

        Dim l As New List(Of IControlAdapter)
        For Each v As RestrictionOnControl In _defSecurity.Prohibitions
            If Not l.Contains(v.ControlAdapt) Then
                If (v.Enabled And cEnabled) Or (v.Visible And cVisible) Then
                    l.Add(v.ControlAdapt)
                End If

            End If
        Next

        For Each v As RestrictionOnControl In _defSecurity.Authorizations
            If Not l.Contains(v.ControlAdapt) Then
                If (v.Enabled And cEnabled) Or (v.Visible And cVisible) Then
                    l.Add(v.ControlAdapt)
                End If
            End If
        Next

        Return l
    End Function

    ''' <summary>
    ''' Devuelve la relaci�n de roles que tiene el usuario seg�n se�ala la aplicaci�n Host (<see cref="IHost"/>) 
    ''' </summary>
    ''' <remarks>Si no se ha establecido ning�n objeto IHost se devolver� una lista con un �nico rol: 0</remarks>
    Private ReadOnly Property UserRoles() As Integer()
        Get
            If SecurityEnvironment.Host Is Nothing Then
                If _userRoles Is Nothing Then
                    _userRoles = New Integer(0) {0}
                End If
                Return _userRoles
            Else
                Return SecurityEnvironment.Host.UserRoles(ID, InstanceID)
            End If
        End Get
    End Property
    Private _userRoles As Integer()

    ''' <summary>
    ''' Devuelve el estado actual de la aplicaci�n (para el tipo de pantalla --control de seguridad-- e instancia concreta) seg�n
    ''' la aplicaci�n Host (<see cref="IHost"/>)
    ''' </summary>
    ''' <remarks>Si no se ha establecido ning�n objeto IHost se devolver� como estado: 0</remarks>
    Private ReadOnly Property HostState() As Integer
        Get
            If SecurityEnvironment.Host Is Nothing Then
                Return 0
            Else
                Return SecurityEnvironment.Host.State(ID, InstanceID)
            End If
        End Get
    End Property

    ''' <summary>
    ''' Indica (True) si es un componente a usar en aplicaciones Web o en aplicaciones WinForms
    ''' </summary>
    ''' <remarks>Este componente debe ser heredado, y las clases que lo refinen deben sobrescribir este m�todo</remarks>
    <Browsable(False)> _
    Public MustOverride ReadOnly Property WebComponent() As Boolean

#End Region

#Region "Escucha eventos EntornoSeguridad"

    Private Sub OnControlAdapterFactoriesChanged()
        If SecurityEnvironment.AutomaticUpdateOfControlsFile Then
            RegisterControls()
        End If
    End Sub

    Private Sub OnSecurityChanged(ByVal IDControlRestrictedUI As String)
        If IDControlRestrictedUI = "" Or IDControlRestrictedUI = Me.ID Then
            If SecurityEnvironment.ComponentsSecurity.ContainsKey(ID) Then
                PriorityEmbeddedSecurity = False
            Else
                ' Se ha eliminado la seguridad establecido a nivel de Entorno -> aplica la embebida
                ' (si desde el entorno se quisiera que no hubiera ning�n tipo de seguridad -> s� existiria una definici�n de seguridad
                ' aunque vac�a)
                PriorityEmbeddedSecurity = True
            End If
            ReinitializeSecurity()
        End If

    End Sub

    Private Sub OnSecurityChangedWithCancelInMind(ByVal IDControlRestriccionesUI As String, ByVal savePriority As Boolean, ByVal recoverPriority As Boolean)
        If IDControlRestriccionesUI = "" Or IDControlRestriccionesUI = Me.ID Then
            If savePriority Then
                _priorityEmbeddedSecurityBAK = PriorityEmbeddedSecurity
            End If
            If recoverPriority Then
                PriorityEmbeddedSecurity = _priorityEmbeddedSecurityBAK
            Else
                PriorityEmbeddedSecurity = Not SecurityEnvironment.ComponentsSecurity.ContainsKey(ID)
            End If
            ReinitializeSecurity()
        End If

    End Sub

    ''' <summary>
    ''' Atiende el evento (<see cref="IHost.StateChanged"/>) del objeto que implemente la interface (<see cref="IHost"/>)(capturado y relanzado por <see cref="SecurityEnvironment "/>)
    ''' </summary>
    ''' <param name="_ID"></param>
    ''' <param name="_instanceID"></param>
    ''' <param name="newState "></param>
    ''' <remarks>S�lo se deber� atender este evento si el ID del control al que va dirigido, as� como la instancia es Nothing o coincide con la del control que lo escucha</remarks>
    Private Sub OnStateChanged(ByVal _ID As String, ByVal _instanceID As String, ByVal newState As Integer)
        If (String.IsNullOrEmpty(_ID) Or Me.ID = _ID) AndAlso (String.IsNullOrEmpty(_instanceID) Or Me.InstanceID = _instanceID) Then
            ReviseAppliedSecurity(Nothing)
        End If
    End Sub

    ''' <summary>
    ''' Atiende el evento (<see cref="IHost.RolesChanged "/>) del objeto que implemente la interface (<see cref="IHost"/>)(capturado y relanzado por <see cref="SecurityEnvironment "/>)
    ''' </summary>
    ''' <param name="_ID"></param>
    ''' <param name="_instanceID"></param>
    ''' <remarks>S�lo se deber� atender este evento si el ID del control al que va dirigido, as� como la instancia es Nothing o coincide con la del control que lo escucha</remarks>
    Private Sub OnRolesChanged(ByVal _ID As String, ByVal _instanceID As String)
        If (String.IsNullOrEmpty(_ID) Or Me.ID = _ID) AndAlso (String.IsNullOrEmpty(_instanceID) Or Me.InstanceID = _instanceID) Then
            ReviseAppliedSecurity(Nothing)
        End If
    End Sub


#End Region

#Region "Formulario de edici�n de seguridad"

    ''' <summary>
    ''' Muesta el formulario de mantenimiento de la seguridad, lo que permitir� tanto la consulta como modificaci�n de las restricciones
    ''' del componente de seguridad que se pasa como par�metro. Al igual que en tiempo de dise�o es posible recuperar o guardar la seguridad
    ''' hacia un archivo, entre otras cosas.
    ''' </summary>
    ''' <remarks>Para ser invocado en tiempo de ejecuci�n en aplicaciones WinForms, en modo de test o configuraci�n</remarks>
    Public Sub ShowConfigurationSecurityForm(Optional ByVal host As IHost = Nothing, _
                                                 Optional ByVal restrictionsDef As String() = Nothing)
        If WebComponent Then Exit Sub

        Dim form As FrmRestrictionsUIDefinition
        Dim file As String = ConfigFile
        SecurityEnvironment.AdaptFilePath(file, False)

        form = New FrmRestrictionsUIDefinition(Me, restrictionsDef, file, host, False)
        form.Show()
    End Sub
#End Region

#Region "Interceptar Tecla de Acceso R�pido (Hot key)"

    ''' <summary>
    ''' Atiende el evento <see cref="SecurityEnvironment.HotKeyChanged"/>. Si se ha habilitado una tecla de acceso r�pido para abrir
    ''' el mantenimiento de la seguridad se asociar� el evento KeyDown del formulario (o contenedor) en el que est� incrustado el 
    ''' componente de seguridad con el m�todo <see cref="OnParentControlKeyDown"/>, que lanzar� la pantalla de mantenimiento. 
    ''' </summary>
    ''' <remarks>El uso de HotKey est� pensado para facilitar la configuraci�n de la seguridad desde el tiempo de ejecuci�n, pero
    ''' en las fases de desarrollo y prueba de las aplicaciones
    ''' </remarks>
    Private Sub OnHotKeyChanged()
        If Not TypeOf (ParentControl) Is System.Windows.Forms.Control Then Exit Sub

        RemoveHandler DirectCast(ParentControl, System.Windows.Forms.Control).KeyDown, AddressOf OnParentControlKeyDown
        If SecurityEnvironment.AllowedHotKey Then
            AddHandler DirectCast(ParentControl, System.Windows.Forms.Control).KeyDown, AddressOf OnParentControlKeyDown
            If TypeOf (ParentControl) Is System.Windows.Forms.Form Then
                DirectCast(ParentControl, System.Windows.Forms.Form).KeyPreview = True
            End If
        Else
            If TypeOf (ParentControl) Is System.Windows.Forms.Form Then
                DirectCast(ParentControl, System.Windows.Forms.Form).KeyPreview = _keyPreviewOriginal
            End If
        End If
    End Sub
    Private _keyPreviewOriginal As Boolean

    Private Sub OnParentControlKeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        If e.KeyCode = SecurityEnvironment.HotKey.KeyCode And e.Modifiers = SecurityEnvironment.HotKey.Modifiers Then
            ShowConfigurationSecurityForm(SecurityEnvironment.Host, Me.RestrictionsDefinition)
        End If
    End Sub

#End Region

#Region "Registro de controles en Fichero"

    ''' <summary>
    ''' Devuelve la relaci�n de controles asociados a un determinado formulario (o contenedor), seg�n aparece registrado
    ''' en el archivo que se indica
    ''' </summary>
    ''' <param name="controlsFile ">Ruta del archivo desde el que leer los controles</param>
    ''' <param name="idControlRestrictedUI  ">Cadena que identifica el formulario (o contenedor)</param>
    ''' <remarks>Ver <see cref="RegisterControls "/>, <see cref="SecurityEnvironment.AutomaticUpdateOfControlsFile "/> 
    ''' y <see cref="ConfigFile "/></remarks>
    Protected Friend Function ReadComponentControls(ByVal controlsFile As String, ByVal idControlRestrictedUI As String) As String()
        Dim list As String = ""
        Dim fichero As String = controlsFile
        Dim result As String() = Nothing
        If SecurityEnvironment.AdaptFilePath(fichero, Me.DesignMode) Then
            Dim controlsLists As Dictionary(Of String, String) = ReadControls(SecurityEnvironment.ReadFile(fichero))
            list = ""
            If controlsLists.TryGetValue(idControlRestrictedUI, list) Then
                result = list.Split(New String() {vbCrLf}, StringSplitOptions.RemoveEmptyEntries)
            End If
        End If

        If result Is Nothing Then
            result = New String(0) {}
        End If
        Return result
    End Function

    Protected Function ReadControls(ByVal TxtControls As String) As Dictionary(Of String, String)
        Dim idControlRestriccionesUI As String = ""
        Dim controlsLists As New Dictionary(Of String, String)
        Dim controls As String = ""
        Try
            Dim reader As New System.IO.StringReader(TxtControls)
            Try
                Dim line As String
                Do
                    line = reader.ReadLine
                    If line Is Nothing Then Exit Do
                    line = line.Trim
                    If line = "" Then Continue Do

                    If line(0) = "["c Then
                        If idControlRestriccionesUI <> "" Then
                            controlsLists.Add(idControlRestriccionesUI, controls)
                        End If
                        idControlRestriccionesUI = line.Substring(1, line.Length - 2)
                        controls = line
                    Else
                        If idControlRestriccionesUI <> "" Then
                            controls += vbCrLf + line
                        End If
                    End If

                Loop
            Finally
                reader.Close()
            End Try
            If idControlRestriccionesUI <> "" Then
                controlsLists.Add(idControlRestriccionesUI, controls)
            End If
            Return controlsLists

        Catch ex As Exception
            Dim cad As String = ERROR_READING_CONTROLSFILE + ex.Message
            SecurityEnvironment.ShowError(cad, Nothing)
            Return controlsLists
        End Try
    End Function

    ''' <summary>
    ''' Devuelve en <paramref name="lista"/> una relaci�n de todos los controles contenidos en el formulario o contenedor (control) referenciado
    ''' por <paramref name="controlAdapt"/>.
    ''' </summary>
    ''' <param name="list">Cadena que recoge la lista de controles</param>
    ''' <param name="controlAdapt">Adaptador que envuelve el control del que se buscar�n sus controles hijo. Ser� igual al formulario (o contenedor) padre en la llamada inicial (es un m�todo recursivo)</param>
    ''' <param name="parent ">Adaptador que envuelve el control padre referenciado por <paramref name=" controlAdapt"/>. Se dejar� vac�o en la llamada inicial (es un m�todo recursivo)</param>
    ''' <remarks>
    ''' El n�mero de controles 'descubiertos' dentro de ese contenedor depender� de la existencia o no de factor�as de adaptadores de controles
    ''' que entiendan o no de determinados controles y busquen dentro de ellos. As�, por ejemplo, la factor�a AdapterInfragisticsWinForms_Factory
    ''' ofrece adaptadores como AdapterInfragisticsWinForms_UltraGrid, que permite descubrir (y gestionar) las columnas de un control UltraGrid
    ''' </remarks>
    Protected Sub MakeControlsListOf(ByRef list As String, ByVal controlAdapt As IControlAdapter, Optional ByVal parent As IControlAdapter = Nothing)

        If Not controlAdapt.Control Is _parentControl Then
            list += controlAdapt.Identification(parent, Me) + vbCrLf
        End If
        For Each c As IControlAdapter In controlAdapt.Controls
            MakeControlsListOf(list, c, controlAdapt)
        Next
    End Sub

#End Region

#Region "Implementaci�n de INotifyPropertyChanged"
    'Nota: esta interfaz es nueva en la versi�n 2.0 de .NET Framework. 
    'Notifica a los clientes que un valor de propiedad ha cambiado. 
    'Espacio de nombres: System.ComponentModel
    'If your data source implements INotifyPropertyChanged and you are performing asynchronous operations, you should not make changes to the data source on a background thread. Instead, you should read the data on a background thread and merge the data into a list on the UI thread.
    '
    'Ver tb. art�culo "Windows Forms Data Binding and Objects"  (Rockford Lhotka)
    'Esa interfaz es una alternativa (recomendada) a :
    'When we bind a control to a property on our object, data binding automatically starts listening for a property changed event named propertyChanged, 
    'where property is the name of the property of the object .
    'For instance, our Order class defines an ID property. When the ID property is bound to a control, data binding starts listening for an IDChanged event. 
    'If this event is raised by our object, data binding automatically refreshes all controls bound to our object.
    '
    'Tambi�n interesante: 
    '" A generic asynchronous INotifyPropertyChanged helper" (http://www.claassen.net/geek/blog/2007/07/generic-asynchronous.html)

    Protected Sub NotifyPropertyChanged(ByVal info As String)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(info))
    End Sub

    Public Event PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
#End Region

#Region "Implementaci�n de ISupportInitialice"

    ' La implementaci�n de la interface ISupportInitialice nos permite conocer en qu� momento el dise�ador inicia y acaba de cargar las
    ' propiedades. Esto nos permite llamar a ReinitializeSecurity (lo que ocurrir� ya posteriomente al modificar RestrictionsDefinition) una
    ' vez que est�n el resto de propiedades ya rellenas, entre ellas ID.
    ' (Designer asigna las propiedades en orden alfab�tico)
    ' ->Design-Time Integration�Batch Initialization: http://en.csharp-online.net/Design-Time_Integration%25E2%2580%2594Batch_Initialization

    Private Sub BeginInit() Implements System.ComponentModel.ISupportInitialize.BeginInit
        _initializing = True
    End Sub

    Private Sub EndInit() Implements System.ComponentModel.ISupportInitialize.EndInit
        _initializing = False
        If SecurityEnvironment.ComponentsSecurity.ContainsKey(ID) Then
            _priorityEmbeddedSecurity = False
        End If
        ReinitializeSecurity()
    End Sub

#End Region

End Class




