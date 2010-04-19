Option Strict On

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


'=========================================================
'        UIRestrictions
'=========================================================

''' <summary>
''' Contiene todas las restricciones de interface (UI) (permisos y prohibiciones) que definen la seguridad
''' para un componente <see cref=" ControlRestrictedUI"/>, y que afectan normalmente por tanto a un 
''' formulario o control de usuario.
''' </summary>
''' <remarks>
''' <para>Todas las restricciones se aplican a nivel de controles individuales, teniendo presente que las 
''' prohibiciones tendrán prioridad sobre los permisos, esto es, primero se aplicarán los permisos y luego 
''' se restringirán en base a las prohibiciones:</para>
''' 
''' <para>Prohibiciones: sólo se impedirán las modificaciones de las propiedades Visible / Enabled en las 
''' situaciones aquí señaladas.</para>
''' 
''' <para>Permisos: sólo se autorizarán las modificaciones de las propiedades Visible / Enabled en las situaciones
''' aquí señaladas</para>
''' 
''' <para>Ver detalle de la interpretación de restricciones en <see cref="RestrictionOnControl "/></para>
''' <seealso cref="RestrictionOnControl "/>
''' </remarks>
Public Class UIRestrictions
    Private _ParentControl As Object
    Private _prohibitions As IList(Of RestrictionOnControl) = New List(Of RestrictionOnControl)
    Private _authorizations As IList(Of RestrictionOnControl) = New List(Of RestrictionOnControl)
    Private _IDControlRestrictedUI As String = ""

    Sub New(Optional ByVal IDControlRestrictedUI As String = "")
        _IDControlRestrictedUI = IDControlRestrictedUI
    End Sub

    ''' <summary>
    ''' Devuelve la lista de prohibiciones asociadas a este componente de seguridad (<see cref=" ControlRestrictedUI"/>),
    ''' y por tanto relacionadas con un formulario o control de usuario.
    ''' </summary>
    ''' <remarks>
    ''' Más información sobre el tratamiento de las restricciones en <see cref="UIRestrictions "/> y en <see cref="RestrictionOnControl "/>.
    ''' </remarks>
    Public Property Prohibitions() As IList(Of RestrictionOnControl)
        Get
            Return _prohibitions
        End Get
        Set(ByVal value As IList(Of RestrictionOnControl))
            _prohibitions = value
        End Set
    End Property

    ''' <summary>
    ''' Devuelve la lista de permisos asociados a este componente de seguridad (<see cref=" ControlRestrictedUI"/>),
    ''' y por tanto relacionadas con un formulario o control de usuario.
    ''' </summary>
    ''' <remarks>
    ''' Más información sobre el tratamiento de las restricciones en <see cref="UIRestrictions "/> y en <see cref="RestrictionOnControl "/>.
    ''' </remarks>
    Public Property Authorizations() As IList(Of RestrictionOnControl)
        Get
            Return _authorizations
        End Get
        Set(ByVal value As IList(Of RestrictionOnControl))
            _authorizations = value
        End Set
    End Property


    ''' <summary>
    ''' Elimina el control indicado de la lista de controles supervisados que contempla esta definición
    ''' de seguridad (Ver también <see cref="ControlRestrictedUI.ExcludeControl "/>)
    ''' </summary>
    Friend Function ExcludeControl(ByVal control As Object) As Boolean
        Dim found As Boolean = False

        For i As Integer = 0 To _authorizations.Count - 1
            If _authorizations(i).ControlAdapt.Control Is control Then
                _authorizations.Remove(_authorizations(i))
                found = True
            End If
        Next
        For i As Integer = 0 To _prohibitions.Count - 1
            If _prohibitions(i).ControlAdapt.Control Is control Then
                _prohibitions.Remove(_prohibitions(i))
                found = True
            End If
        Next
        Return found
    End Function

    ''' <summary>
    ''' Serializa las prohibiciones establecidas en un array de cadenas de la forma: -roles/bloqueo/../bloqueo
    ''' </summary>
    ''' <remarks>Internamente las listas de restricciones están ordenadas por roles</remarks>
    Public Function NegativeAuthorizationsToArrayString(Optional ByVal useAlias As Boolean = False) As String()
        Return AuthorizationsListToArrayString(Prohibitions, "-"c, useAlias)
    End Function

    ''' <summary>
    ''' Serializa los permisos establecidos en un array de cadenas de la forma: +roles/permiso/../permiso
    ''' </summary>
    ''' <remarks>Internamente las listas de restricciones están ordenadas por roles</remarks>
    Public Function PositiveAuthorizationsToArrayString(Optional ByVal useAlias As Boolean = False) As String()
        Return AuthorizationsListToArrayString(Authorizations, "+"c, useAlias)
    End Function

    ''' <summary>
    ''' Serializa las restricciones (permisos y prohibiciones) establecidas en un array de cadenas de la forma: +-roles/bloqueo/../bloqueo
    ''' </summary>
    ''' <remarks>Internamente las listas de restricciones están ordenadas por roles</remarks>
    Public Function AuthorizationsToArrayString(Optional ByVal useAlias As Boolean = False) As String()
        Dim positivos, negativos As String()
        Dim n As Integer
        positivos = AuthorizationsListToArrayString(Authorizations, "+"c, useAlias)
        negativos = AuthorizationsListToArrayString(Prohibitions, "-"c, useAlias)
        n = positivos.Length
        ReDim Preserve positivos(n + negativos.Length - 1)
        negativos.CopyTo(positivos, n)
        Return positivos
    End Function


    ''' <summary>
    ''' Serializa las restricciones (permisos y prohibiciones) establecidas en un array de cadenas de la forma: listaRoles/bloqueo/../bloqueo
    ''' </summary>
    ''' <remarks>Se supone que las restricciones de la lista ya vienen ordenados por rol</remarks>
    Private Function AuthorizationsListToArrayString(ByVal lista As IList(Of RestrictionOnControl), ByVal typeAuthorizations As Char, Optional ByVal useAlias As Boolean = False) As String()
        Dim l As New ArrayList

        Dim authorizationsList As String = ""
        Dim roles() As Integer = Nothing

        For Each c As RestrictionOnControl In lista
            If Util.ConvertToString(roles) <> Util.ConvertToString(c.roles) Then
                If roles IsNot Nothing Then
                    l.Add(authorizationsList)
                End If
                roles = c.roles
                If Not useAlias Then
                    authorizationsList = typeAuthorizations + Util.ConvertToString(c.roles)
                Else
                    authorizationsList = typeAuthorizations + SecurityEnvironment.RolesToStrUsingAlias(c.roles, _IDControlRestrictedUI)
                End If
            End If
            authorizationsList += "/" + c.ToString
        Next
        If roles IsNot Nothing Then
            l.Add(authorizationsList)
        End If

        Dim cad(l.Count - 1) As String
        l.CopyTo(cad)
        Return cad
    End Function


    ''' <summary>
    ''' Constructor a partir de una cadena de restricciones (tanto positivas --permisos-- como negativas --prohibiciones) serializada
    ''' </summary>
    ''' <param name="restrictions">Cadena con el contenido (restricciones) serializado</param>
    ''' <param name="parentControl">Control de usuario al que está vinculada esta seguridad (normalmente será un formulario)</param>
    ''' <param name="groups">Grupos de controles, en base a los cuales han podido establecerse algunas restricciones</param>
    ''' <param name="IDControlRestrictedUI">Identificador del componente de seguridad <see cref="ControlRestrictedUI "/>para el que se
    ''' definen estas restricciones. Se necesitará si en las restricciones se hace uso de alias, ya que éstos pueden ser comunes a todos
    ''' los componentes de seguridad o particulares a uno concreto.
    ''' </param>
    ''' <remarks>El control padre <paramref name=" parentControl "/>permite localizar los controles incluidos en la cadena de restricciones serializada</remarks>
    Sub New(ByVal restrictions() As String, ByVal IDControlRestrictedUI As String, ByVal parentControl As Object, Optional ByVal groups As Group() = Nothing)
        Dim cad As String

        If restrictions Is Nothing Then Exit Sub

        _ParentControl = parentControl
        _IDControlRestrictedUI = IDControlRestrictedUI


        ' Preparar un diccionario con los posibles grupos definidos, ya listando los adaptadores de los controles a los que referencian
        ' para su posible uso en la deserialización de las restricciones

        Dim lGroupsControls As New Dictionary(Of String, List(Of IControlAdapter))
        Dim controlAdapt As IControlAdapter

        If parentControl IsNot Nothing And groups IsNot Nothing Then

            For Each g As Group In groups
                Dim lctrlAdapt As New List(Of IControlAdapter)

                For Each c As String In g.Controls
                    controlAdapt = SecurityEnvironment.GetAdapter(parentControl).FindControl(c)
                    If controlAdapt.IsNull Then
                        SecurityEnvironment.ShowError(Constants.ERROR_CONTROL_NOTFOUND + c, parentControl, _IDControlRestrictedUI)
                    Else
                        lctrlAdapt.Add(controlAdapt)
                    End If
                Next
                lGroupsControls.Add(g.Name.ToUpper, lctrlAdapt)
            Next

        End If


        ' Deserializar cada línea de restricciones de manera independiente
        For Each cad In restrictions
            If cad = "" Then Continue For
            Select Case cad(0)
                Case "-"c
                    ReadRestrictionsLine(cad.Substring(1), _prohibitions, lGroupsControls)
                Case "+"c
                    ReadRestrictionsLine(cad.Substring(1), _authorizations, lGroupsControls)
                Case Else
                    ReadRestrictionsLine(cad, _authorizations, lGroupsControls)
            End Select
        Next

    End Sub


    ''' <summary>
    ''' Deserializa restricciones (permisos y prohibiciones) de un rol (o roles) en una cadena de la forma: listaRoles/permiso/../permiso
    ''' </summary>
    ''' <param name="line">Línea de texto con restricciones serializadas relativas a un rol o roles</param>
    ''' <param name="list">Lista a donde ir añadiendo las restricciones individuales ya instanciadas (en objetos <see cref=" RestrictionOnControl"/></param>
    ''' <param name="lGroupsControls">Diccionario con los grupos definidos, contemplando para cada uno la relación de los adaptadores de control
    ''' que incluyen, para su posible uso en la deserialización de las restricciones</param> 
    ''' <remarks></remarks>
    Private Sub ReadRestrictionsLine(ByVal line As String, ByVal list As IList(Of RestrictionOnControl), ByVal lGroupsControls As Dictionary(Of String, List(Of IControlAdapter)))
        Dim cad() As String = line.Split("/"c)
        Dim pos As Integer
        Dim v As RestrictionOnControl

        If cad.Length < 2 Then
            Exit Sub
        End If

        ' Leer la lista de roles
        '---------------------
        Dim roles() As Integer = SecurityEnvironment.GetRolesID(cad(0), _IDControlRestrictedUI)
        If roles.Length = 0 Then
            SecurityEnvironment.ShowError(Constants.ERROR_ROLES_LIST_INCORRECT + cad(0), _ParentControl, _IDControlRestrictedUI)
            Exit Sub
        End If

        ' Leer las restricciones individuales
        '---------------------
        For i As Integer = 1 To cad.Length - 1

            ' Si contiene el carácter $ indicará que se está haciendo uso de un grupo de controles
            ' Si _ControlPadre is Nothing no se descompondrá el posible grupo (p.ej para su uso desde frmDefinicionSeguridad)
            pos = cad(i).IndexOf("$"c)
            If _ParentControl Is Nothing Or pos < 0 Then
                v = New RestrictionOnControl(cad(i), roles, _ParentControl)
                ' Si el padre no es nothing el control debe necesariamente haberse localizado si es correcto.
                ' Si es Nothing estaremos en modo diseño (en proyecto Web), por lo que puede no haberse localizado.
                If _ParentControl Is Nothing OrElse Not v.ControlAdapt.IsNull Then
                    list.Add(v)
                End If

            Else
                ' Será algo de la forma: $grupo, V,E, listaEstados
                ' Llamaremos al constructor de RestrictionOnControl por cada control del grupo, pero ya pasándole el IControlAdapter, e indicándole
                ' que ignore el campo control.
                Dim groupName As String = cad(i).Split(","c)(0).Substring(1).Trim
                Dim lAdaptControl As List(Of IControlAdapter) = Nothing
                If lGroupsControls.TryGetValue(groupName.ToUpper, lAdaptControl) Then
                    For Each c As IControlAdapter In lAdaptControl
                        v = New RestrictionOnControl(cad(i), roles, _ParentControl, c)
                        If Not v.ControlAdapt.IsNull Then
                            list.Add(v)
                        End If
                    Next
                Else
                    SecurityEnvironment.ShowError(Constants.GROUP_NOTDEFINED + groupName + Constants.RESTRICTION_WILL_BE_IGNORED + cad(i), _ParentControl, _IDControlRestrictedUI)
                End If
            End If

        Next

    End Sub

    ''' <summary>
    ''' Elmina los permisos y prohibiciones de la definición de seguridad
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Clear()
        _prohibitions.Clear()
        _authorizations.Clear()
    End Sub

End Class


'---------------------------------------------------------
'        RestrictionOnControl
'---------------------------------------------------------
''' <summary>
''' Define los elementos que configuran una restricción concreta a supervisar. Esta restricción
''' sólo tendrá pleno sentido interpretada conjuntamente con el resto de restricciones incluidas
''' en una definición de seguridad definida en un objeto <see cref=" UIRestrictions "/> y gestionada 
''' por un componente <see cref=" ControlRestrictedUI "/>
''' </summary>
''' <remarks>
''' Los elementos que configuran una restricción individual son:
''' <list type="bullet">
''' <item><description>Control a supervisar (a través de un adaptador)</description></item>
''' <item><description>Propiedades a controlar (Visible y/o Enabled)</description></item>
''' <item><description>Contexto de la aplicación para el que se define la restricción:</description></item>
''' </list>
'''    - Rol o roles del usuario de la aplicación
'''    - Estado o estados de la aplicación
''' 
''' <para>
''' Estos elementos podrán ser aplicados en lógica positiva (permisos) o negativa (prohibiciones).
''' Esta interpretación no la ofrece esta estructura sino la clase <see cref="UIRestrictions "/> 
''' dependiendo de que esta restricción haya sido incluida en una línea de permisos o prohibiciones.</para>
'''
''' <para>- Si la restricción consiste en un permiso indicará que las propiedades
''' supervisadas del control sólo las podrán 'activar' (hacer visible o habilitar) los roles
''' indicados y únicamente cuando la aplicación esté en los estados señalados.</para>
''' 
''' <para>- Si la restricción consiste en una prohibición indicará que las propiedades
''' supervisadas del control no podrán ser 'activadas' por los roles indicados cuando la
''' aplicación esté en los estados señalados. Para cualquier combinación de roles/estado 
''' distinta sí será posible la activación.</para>
''' 
''' <para>- Si no se aporta ningún rol (por defecto se asume rol = 0) entonces aplicará a todos los
''' roles: a todos se les permitirá o impedirá (según) en los estados señalados</para>
''' <para>- Si no se aporta ningún estado entonces la restricción aplicará a los roles que correspondan
''' con independencia del estado en que esté la aplicación.</para>
''' 
''' <para>- Si un control no tiene ningún elemento de restricción asociado (ni positivo ni negativo)
''' entonces no será supervisado, y cualquier rol y en cualquier estado podrá activar sus 
''' propiedades Visible y Enabled.</para>
''' 
''' <para>Las propiedades realmente controladas en el control no tienen por qué ser exactamente 'Visible' 
''' y 'Enabled'; es responsabilidad del adaptador del control ofrecer esa interface y actuar sobre 
''' las propiedades que tenga el control (por ejemplo, algunos controles no ofrecen Enabled pero sí ReadOnly)
''' Sólo se supervisa y tal vez se impida (según la política definida) la 'activación' de esas propiedades,
''' esto es, el intento de hacer visible o habilitar el control. No se impide el hacer invisible o 
''' deshabilitar un control.</para>
''' <seealso cref="UIRestrictions "/>
''' <seealso cref="ControlRestrictedUI"/>
''' </remarks>
Public Structure RestrictionOnControl
    Private _controlAdapt As IControlAdapter
    Private _IDControl As String

    Public Visible As Boolean
    Public Enabled As Boolean
    Public roles() As Integer
    Public states() As Integer

    Public ReadOnly Property IDControl() As String
        Get
            Return _IDControl
        End Get
    End Property

    Public ReadOnly Property ControlAdapt() As IControlAdapter
        Get
            Return _controlAdapt
        End Get
    End Property

    Sub New(ByVal ctrlAdapt As IControlAdapter, ByVal _Visible As Boolean, ByVal _Enabled As Boolean, ByVal userRoles As Integer(), Optional ByVal _states As Integer() = Nothing, Optional ByVal parentControl As IControlAdapter = Nothing)
        If ctrlAdapt Is Nothing Then ctrlAdapt = New NullControlAdapter
        _controlAdapt = ctrlAdapt
        _IDControl = ctrlAdapt.Identification(parentControl)
        Visible = _Visible
        Enabled = _Enabled
        roles = userRoles
        If Not _states Is Nothing Then
            states = _states
        End If
    End Sub

    Sub New(ByVal IDcontrol As String, ByVal _Visible As Boolean, ByVal _Enabled As Boolean, ByVal userRoles As Integer(), Optional ByVal _states As Integer() = Nothing)
        _controlAdapt = New NullControlAdapter
        _IDControl = IDcontrol
        Visible = _Visible
        Enabled = _Enabled
        roles = userRoles
        If Not _states Is Nothing Then
            states = _states
        End If
    End Sub


    ''' <summary>
    ''' Deserializa la prohibición o permiso a partir de texto en la forma:
    ''' nombrecontrol,V,E[,listaEstados]
    ''' Puede aparecer V, E o los dos, y en ese caso el orden no es importante
    ''' </summary>
    ''' <param name="text">Cadena con el contenido serializado</param>
    ''' <param name="_roles">Roles de usuario a los que aplica. Deben facilitarse pues no viene en la cadena serializada</param>
    ''' <param name="parentControl">Control padre en base al cual está referenciado el control que aparece en la restricción 
    ''' (La identificación del control se compondrá de la identificación de todos sus padres hasta llegar a este <paramref name=" parentControl "/></param>
    ''' <param name="adaptCtrl">Si se aporta se estará ya dando localizado el control que aparece en la restricción</param>
    ''' 
    ''' <remarks>Si no se aporta el parámetro <paramref name="parentControl "/> no se intentará localizar el control a vigilar, 
    ''' ni se utilizará tampoco el adaptador que se haya facilitado (lo que tampoco será normal) simplemente se guardará su identificador;
    ''' se localizará en el momento en que se aporte el control padre.
    ''' </remarks>
    Sub New(ByVal text As String, ByVal _roles As Integer(), ByVal parentControl As Object, Optional ByVal adaptCtrl As IControlAdapter = Nothing)
        Dim cancel As Boolean
        Dim numFields, posStates As Integer
        Dim cad() As String = text.Split(","c)
        Dim errorIndicated As Boolean = False

        If cad.Length < 2 Then cancel = True

        ' Primero: identificar el control
        '----------------------
        If Not cancel AndAlso parentControl IsNot Nothing Then
            If adaptCtrl IsNot Nothing Then
                _controlAdapt = adaptCtrl
            Else
                _controlAdapt = SecurityEnvironment.GetAdapter(parentControl).FindControl(cad(0))
                If _controlAdapt.IsNull Then
                    SecurityEnvironment.ShowError(Constants.ERROR_CONTROL_NOTFOUND + cad(0), parentControl)
                    errorIndicated = True
                    cancel = True
                End If
            End If
        Else
            _controlAdapt = New NullControlAdapter
        End If

        ' Segundo: identificar las propiedades a supervisar
        '----------------------
        If Not cancel Then
            numFields = cad.Length
            _IDControl = cad(0)
            roles = _roles
            If cad(1).ToUpper = "V" OrElse (numFields > 2 AndAlso cad(2).ToUpper = "V") Then
                Visible = True
            End If
            If cad(1).ToUpper = "E" OrElse (numFields > 2 AndAlso cad(2).ToUpper = "E") Then
                Enabled = True
            End If
            If Not Enabled And Not Visible Then
                cancel = True
            Else
                If Enabled And Visible Then
                    posStates = 4
                Else
                    posStates = 3
                End If
            End If

            ' Tercero: identificar los estados que controlan el permiso
            '----------------------
            If Not cancel AndAlso numFields > posStates - 1 Then      ' Carga de los estados, si los hay
                states = New Integer(numFields - posStates) {}
                For i As Integer = posStates - 1 To numFields - 1
                    If Not Integer.TryParse(cad(i), states(i - (posStates - 1))) Then
                        cancel = True
                    End If
                Next
            End If
        End If

        If cancel Then
            _IDControl = ""
            _controlAdapt = New NullControlAdapter
            If Not errorIndicated Then SecurityEnvironment.ShowError("RestrictionOnControl. " + Constants.ERROR_INCORRECT_DEFINITION + text, parentControl)
        End If
    End Sub

    ''' <summary>
    ''' Serializa la definición del permiso/prohibición de la forma:
    ''' nombrecontrol,visible,enabled[,listaEstados]
    ''' </summary>
    Overrides Function ToString() As String
        Dim cad As String
        Dim cadVisible As String = ""
        Dim cadEnabled As String = ""

        If Visible Then cadVisible = ",V"
        If Enabled Then cadEnabled = ",E"

        cad = _IDControl + cadVisible + cadEnabled
        If Not states Is Nothing Then
            For Each estado As Integer In states
                cad += "," + estado.ToString
            Next
        End If
        Return cad
    End Function


End Structure

