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
''' It contains all the interface restrictions (UI) (permissions and prohibitions) that define security for 
''' a <see cref=" ControlRestrictedUI"/> component, and that therefore they usually involve a form or user control.
''' </summary>
''' <remarks>
''' <para>All restrictions apply to individual controls, bearing in mind that the prohibitions will take precedence 
''' over permissions, that is, permissions will be aplied first and then restricted on the basis of the prohibitions:</para>
''' 
''' <para>Prohibitions: only will be prevented changes to the property Visible / Enabled in the situations outlined here.</para>
''' <para>Permissions: only will be authorized changes to the property Visible / Enabled in the situations outlined here</para>
''' 
''' <para>This class saves individual restrictions (see <see cref="RestrictionOnControl "/>) and determines if they 
''' should be considered in positive logic (permissions) or negative logic (prohibitions). 
''' It is responsible for serializing and deserializing these permissions.
''' </para>
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
    ''' Returns the list of prohibitions associated to this security component (<see cref=" ControlRestrictedUI"/>) and so 
    ''' related with a form or user control.
    ''' </summary>
    ''' <remarks>
    ''' More information on the treatment of restrictions in <see cref="UIRestrictions "/> and <see cref="RestrictionOnControl "/>.
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
    ''' Returns the list of authorizations associated to this security component (<see cref=" ControlRestrictedUI"/>) and so 
    ''' related with a form or user control.
    ''' </summary>
    ''' <remarks>
    ''' More information on the treatment of restrictions in <see cref="UIRestrictions "/> and <see cref="RestrictionOnControl "/>.
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
    ''' Removes the control from the list of supervised controls, considered in this security definition 
    ''' (See also <see cref="ControlRestrictedUI.ExcludeControl "/>)
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
    ''' Serializes the prohibitions defined in an array of strings of the form: -roles/prohibition/../prohibition
    ''' </summary>
    ''' <remarks>Internally the lists of restrictions are ordered by role</remarks>
    Public Function ProhibitionsToArrayString(Optional ByVal useAlias As Boolean = False) As String()
        Return RestrictionsListToArrayString(Prohibitions, "-"c, useAlias)
    End Function

    ''' <summary>
    ''' Serializes the authorizations defined in an array of strings of the form: +roles/authorization/../authorization
    ''' </summary>
    ''' <remarks>Internally the lists of restrictions are ordered by role</remarks>
    Public Function AuthorizationsToArrayString(Optional ByVal useAlias As Boolean = False) As String()
        Return RestrictionsListToArrayString(Authorizations, "+"c, useAlias)
    End Function

    ''' <summary>
    ''' Serializes the restrictions (authorizations and prohibitions) defined in an array of strings of the form: +-roles/restriction/../restriction
    ''' </summary>
    ''' <remarks>Internally the lists of restrictions are ordered by role</remarks>
    Public Function RestrictionsToArrayString(Optional ByVal useAlias As Boolean = False) As String()
        Dim positivos, negativos As String()
        Dim n As Integer
        positivos = RestrictionsListToArrayString(Authorizations, "+"c, useAlias)
        negativos = RestrictionsListToArrayString(Prohibitions, "-"c, useAlias)
        n = positivos.Length
        ReDim Preserve positivos(n + negativos.Length - 1)
        negativos.CopyTo(positivos, n)
        Return positivos
    End Function


    ''' <summary>
    ''' Serializes the restrictions (authorizations and prohibitions) defined in an array of strings of the form: +-roles/restriction/../restriction
    ''' </summary>
    ''' <remarks>List of restrictions is supposed to be ordered by user role</remarks>
    Private Function RestrictionsListToArrayString(ByVal lista As IList(Of RestrictionOnControl), ByVal typeAuthorizations As Char, Optional ByVal useAlias As Boolean = False) As String()
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
    ''' Constructor from a string of serialized restrictions (positive --authorizations-- and negative --prohibitions).
    ''' </summary>
    ''' <param name="restrictions">String with the serialized content (restrictions)</param>
    ''' <param name="parentControl">Control UI to which this security is linked (usually it will be a form)</param>
    ''' <param name="groups">Control groups, based on which restrictions have been established</param>
    ''' <param name="IDControlRestrictedUI">Security component identifier (<see cref="ControlRestrictedUI "/>) for which these 
    ''' restrictions are defined. It will be necessary if the restrictions use alias, as these may be common to all security
    ''' components or particular to one.
    ''' </param>
    ''' <remarks>The parent control <paramref name=" parentControl "/> allows to locate the controls included in the serialized restrictions string</remarks>
    Sub New(ByVal restrictions() As String, ByVal IDControlRestrictedUI As String, ByVal parentControl As Object, Optional ByVal groups As Group() = Nothing)
        Dim cad As String

        If restrictions Is Nothing Then Exit Sub

        _ParentControl = parentControl
        _IDControlRestrictedUI = IDControlRestrictedUI


        ' We prepare a dictionary with the possible defined groups, including the adapters of the controls they reference, for
        ' use in deserialization of the restrictions

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

        ' Deserialize each line of restrictions, separately
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
    ''' Deserializes the restrictions (authorizations and prohibitions) of a rol (or roles) in a string of the form: RolesList/restriction/../restriction
    ''' </summary>
    ''' <param name="line">Text line with serialized restrictions on a role or roles</param>
    ''' <param name="list">List to go on adding individual restrictions, and instanced (on objects <see cref=" RestrictionOnControl"/>)</param>
    ''' <param name="lGroupsControls">Dictionary with the defined groups, including the relation of control adapters, for its possible use
    ''' in the deserialization of the restrictions</param> 
    ''' <remarks></remarks>
    Private Sub ReadRestrictionsLine(ByVal line As String, ByVal list As IList(Of RestrictionOnControl), ByVal lGroupsControls As Dictionary(Of String, List(Of IControlAdapter)))
        Dim cad() As String = line.Split("/"c)
        Dim pos As Integer
        Dim v As RestrictionOnControl

        If cad.Length < 2 Then
            Exit Sub
        End If

        ' Read the list of roles
        '---------------------
        Dim roles() As Integer = SecurityEnvironment.GetRolesID(cad(0), _IDControlRestrictedUI)
        If roles.Length = 0 Then
            SecurityEnvironment.ShowError(Constants.ERROR_ROLES_LIST_INCORRECT + cad(0), _ParentControl, _IDControlRestrictedUI)
            Exit Sub
        End If

        ' Read individual restrictions
        '---------------------
        For i As Integer = 1 To cad.Length - 1

            ' If it contains a $ character indicates that it is using a group of controls
            ' If _ControPadre is Nothing we will not break down the group (e.g. for its use from frmDefinicionSeguridad)
            pos = cad(i).IndexOf("$"c)
            If _ParentControl Is Nothing Or pos < 0 Then
                v = New RestrictionOnControl(cad(i), roles, _ParentControl)
                ' Si el padre no es nothing el control debe necesariamente haberse localizado si es correcto.
                ' Si es Nothing estaremos en modo diseño (en proyecto Web), por lo que puede no haberse localizado.
                If _ParentControl Is Nothing OrElse Not v.ControlAdapt.IsNull Then
                    list.Add(v)
                End If

            Else
                ' It will be something like: $grupo, V,E, listaEstados
                ' We will call the RestrictionOnControl's constructor for each group control, passing it the IControlAdapter
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
    ''' Removes the permissions and prohibitions of the security definition
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
''' It defines the elements that make up a particular restriction to monitor. This restriction 
''' will only have its full meaning when read in conjunction with other restrictions included 
''' in a security policy defined in a <see cref=" UIRestrictions "/> object and managed 
''' by a <see cref=" ControlRestrictedUI "/> component.
''' </summary>
''' <remarks>
''' The elements which form an individual restriction are:
''' <list type="bullet">
''' <item><description>The control to be monitored (via an adapter)</description></item>
''' <item><description>The properties to be monitored (Visible and/or Enabled)</description></item>
''' <item><description>The context of the application for which the restriction is defined:</description></item>
''' </list>
'''    - Rol or roles of the application user
'''    - State or states of the application
''' 
''' <para>
''' These elements may be applied in positive logic (permissions) or negative (prohibitions). 
''' This interpretation is not offered by this entity, but by <see cref="UIRestrictions "/> depending 
''' on whether this restriction has been placed in a line of authorizations or prohibitions.
''' </para>
''' 
''' <para>- If the restriction is a permission, it will indicate that the supervised properties 
''' can only be 'activated' (make visible or enable) by the established roles and only when the 
''' application is in the established states.</para>
''' 
''' <para>- If the restriction is a prohibition, it will indicate that the supervised properties 
''' can only be 'activated' (make visible or enable) by the established roles when the application 
''' is in the mentioned states. 
''' For any other combination of roles / state the activation will be possible.</para>
''' 
''' <para>- If no role is provided (by default, role is assumed = 0) then it will apply to all 
''' roles: all of them will be allowed or prevented (depending) in the indicated states.</para>
''' 
''' <para>- If no state is provided, then the restriction will apply to all concerning roles, 
''' regardless of the state in which the application is.</para>
''' 
''' <para>- If a control has no associated restriction element (neither positive nor negative) then 
''' it will not be monitored, and any role and in any state could activate its Visible and Enabled 
''' properties.</para>
''' 
''' <para>
''' Actually monitored properties in the control need not be exactly 'Visible' and 'Enabled' (as shown in IControlAdapter). 
''' It is only supervised and perhaps prevented (depending on the policy defined) the 'activation' of 
''' these properties, namely the attempt to make visible or enable the control. It is not prevented to 
''' make invisible or disabled a control.
''' </para>
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
    ''' Deserializes the prohibition or authorization from a text of the form:
    ''' controlName,V,E[,StatesList]
    ''' It can appear V, E or both, and the order is not important
    ''' </summary>
    ''' <param name="text">String with the serialized content</param>
    ''' <param name="_roles">User roles to which it applies. This param is needed as this information is not contained in the serialized string</param>
    ''' <param name="parentControl">Parent control for which the control in the restriction is referenced. 
    ''' (The identification of control consists of the identification of all their parents up to this <paramref name=" parentControl "/></param>
    ''' <param name="adaptCtrl">If you provide this adapter you are giving localized the control that appears in the restriction</param>
    ''' 
    ''' <remarks>If the parameter <paramref name="parentControl "/> is not provided, it will not attempt to locate the control to monitor, 
    ''' neither will use the adapter provided (if any), it will only be saved its identifier;
    ''' it will be localized when it is provided the parent control
    ''' </remarks>
    Sub New(ByVal text As String, ByVal _roles As Integer(), ByVal parentControl As Object, Optional ByVal adaptCtrl As IControlAdapter = Nothing)
        Dim cancel As Boolean
        Dim numFields, posStates As Integer
        Dim cad() As String = text.Split(","c)
        Dim errorIndicated As Boolean = False

        If cad.Length < 2 Then cancel = True

        ' First: identify the control
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

        ' Second: identify the properties to monitor
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

            ' Third: identify the states that control the permission
            '----------------------
            If Not cancel AndAlso numFields > posStates - 1 Then      ' Load the states, if any
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
    ''' Serialize the definition of the authorization/prohibition of the form:
    ''' controlName,visible,enabled[,StatesList]
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

