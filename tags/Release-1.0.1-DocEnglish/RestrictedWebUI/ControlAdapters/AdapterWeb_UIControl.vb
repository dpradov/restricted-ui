Option Strict On

Imports System.Web.UI.WebControls
Imports RestrictedUI

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
''' Generic adapter for controls that inherit from <see cref="Web.UI.Control"/>, for its use from the Restricted User Interface library (<see cref="RestrictedUI"/>)
''' </summary>
''' <remarks></remarks>
Public Class AdapterWeb_UIControl
    Implements IControlAdapter

    Private _control As System.Web.UI.Control
    Private _security As ControlRestrictedUI

    Sub New(ByVal control As System.Web.UI.Control)
        _control = control
    End Sub

    ReadOnly Property Control() As Object Implements IControlAdapter.Control
        Get
            Return _control
        End Get
    End Property

    ReadOnly Property IsNull() As Boolean Implements IControlAdapter.IsNull
        Get
            Return _control Is Nothing
        End Get
    End Property


    Public Property Visible() As Boolean Implements IControlAdapter.Visible
        Get
            Return _control.Visible
        End Get
        Set(ByVal value As Boolean)
            _control.Visible = value
        End Set
    End Property

    Public Property Enabled() As Boolean Implements IControlAdapter.Enabled
        Get
            Return True
        End Get
        Set(ByVal value As Boolean)
        End Set
    End Property

    Public Sub SuperviseEnabled(ByVal security As ControlRestrictedUI) Implements IControlAdapter.SuperviseEnabled
        _security = security
        RemoveHandler _control.PreRender, AddressOf control_EnabledChanged
        AddHandler _control.PreRender, AddressOf control_EnabledChanged
    End Sub

    Public Sub SuperviseVisible(ByVal security As ControlRestrictedUI) Implements IControlAdapter.SuperviseVisible
        _security = security
        RemoveHandler _control.PreRender, AddressOf control_VisibleChanged
        AddHandler _control.PreRender, AddressOf control_VisibleChanged
    End Sub

    Sub FinalizeSupervision() Implements IControlAdapter.FinalizeSupervision
        RemoveHandler _control.PreRender, AddressOf control_VisibleChanged
        RemoveHandler _control.PreRender, AddressOf control_EnabledChanged
    End Sub

    Private Sub control_EnabledChanged(ByVal sender As Object, ByVal e As EventArgs)
        _security.VerifyChange(Me, ControlRestrictedUI.TChange.Enabled)
    End Sub
    Private Sub control_VisibleChanged(ByVal sender As Object, ByVal e As EventArgs)
        _security.VerifyChange(Me, ControlRestrictedUI.TChange.Visible)
    End Sub


    Public Overridable Function Controls() As IList(Of IControlAdapter) Implements IControlAdapter.Controls
        Dim children As New List(Of IControlAdapter)
        Dim adapt As IControlAdapter

        For Each ctrl As System.Web.UI.Control In _control.Controls
            If ctrl.ID IsNot Nothing Then
                adapt = SecurityEnvironment.GetAdapter(ctrl)
                If Not TypeOf adapt Is NullControlAdapter Then
                    children.Add(adapt)
                End If
            End If
        Next

        Return children
    End Function

    Public ReadOnly Property Identification(Optional ByVal parent As IControlAdapter = Nothing, Optional ByVal security As ControlRestrictedUI = Nothing) As String Implements IControlAdapter.Identification
        Get
            Dim cad As String = ""
            Dim sep As String = ""

            Dim control As System.Web.UI.Control = DirectCast(_control, System.Web.UI.Control)
            Do
                cad = control.ID + sep + cad
                sep = "."
                control = control.Parent
            Loop Until control Is Nothing _
                 OrElse (security IsNot Nothing AndAlso control Is security.ParentControl) _
                 OrElse (security Is Nothing AndAlso ( _
                                  TypeOf control Is System.Web.UI.UserControl _
                           OrElse TypeOf control Is System.Web.UI.HtmlControls.HtmlForm _
                           OrElse control.GetType.IsSubclassOf(GetType(System.Web.UI.HtmlControls.HtmlForm)) _
                           ) _
                        )

            Return cad
        End Get
    End Property


    Protected Overridable Function FindControl(ByVal identifier As String) As IControlAdapter Implements IControlAdapter.FindControl
        Dim pos As Integer = identifier.ToUpper.IndexOf("."c)
        Dim cad As String
        Dim parent As System.Web.UI.Control = _control

        If pos < 0 Then
            cad = identifier.ToUpper
        Else
            cad = identifier.Substring(0, pos).ToUpper
        End If

        Try
            For Each c As Object In parent.Controls
                If TypeOf c Is WebControl AndAlso DirectCast(c, WebControl).ID.ToUpper = cad Then
                    cad = identifier.Substring(pos + 1)
                    If cad = "" Or pos < 0 Then
                        Return SecurityEnvironment.GetAdapter(c)
                    Else
                        Return SecurityEnvironment.GetAdapter(c).FindControl(cad)
                    End If
                End If
            Next
        Catch ex As Exception
        End Try

        Return New NullControlAdapter
    End Function


End Class
