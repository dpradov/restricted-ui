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
''' Adapter for <see cref="Web.UI.WebControls.DataControlField"/> controls, for its use from the Restricted User Interface library (<see cref="RestrictedUI"/>)
''' </summary>
''' <remarks>
''' At the moment it is only allowed to monitor the Enabled property (through ReadOnly) for the subclasses
''' <see cref="Web.UI.WebControls.CheckBoxField "/> and <see cref="Web.UI.WebControls.BoundField"/>. 
''' The rest (<see cref="Web.UI.WebControls.ButtonField"/>, <see cref="Web.UI.WebControls.TemplateField"/> and <see cref="Web.UI.WebControls.CommandField"/>) 
''' do not offer ReadOnly or any other property that make possible that supervision, at least directly
''' </remarks>
Public Class AdapterWeb_DataControlField
    Implements IControlAdapter

    Private _control As DataControlField
    Private _parent As GridView
    Private _security As ControlRestrictedUI

    Sub New(ByVal control As DataControlField, ByVal parent As GridView)
        _control = control
        _parent = parent
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

    Public Sub SuperviseEnabled(ByVal security As ControlRestrictedUI) Implements IControlAdapter.SuperviseEnabled
        If TypeOf _control Is BoundField Or TypeOf _control Is CheckBoxField Then
            _security = security
            RemoveHandler _parent.PreRender, AddressOf control_EnabledChanged
            AddHandler _parent.PreRender, AddressOf control_EnabledChanged
        End If
    End Sub

    Public Sub SuperviseVisible(ByVal security As ControlRestrictedUI) Implements IControlAdapter.SuperviseVisible
        _security = security
        RemoveHandler _parent.PreRender, AddressOf control_VisibleChanged
        AddHandler _parent.PreRender, AddressOf control_VisibleChanged
    End Sub

    Sub FinalizeSupervision() Implements IControlAdapter.FinalizeSupervision
        RemoveHandler _parent.PreRender, AddressOf control_VisibleChanged
        RemoveHandler _parent.PreRender, AddressOf control_EnabledChanged
    End Sub

    Private Sub control_VisibleChanged(ByVal sender As Object, ByVal ev As EventArgs)
        _security.VerifyChange(Me, ControlRestrictedUI.TChange.Visible)
    End Sub

    Private Sub control_EnabledChanged(ByVal sender As Object, ByVal ev As EventArgs)
        If TypeOf _control Is BoundField Or TypeOf _control Is CheckBoxField Then
            _security.VerifyChange(Me, ControlRestrictedUI.TChange.Enabled)
        End If
    End Sub

    Public Property Visible() As Boolean Implements IControlAdapter.Visible
        Get
            Return Not _control.Visible
        End Get
        Set(ByVal value As Boolean)
            _control.Visible = value
        End Set
    End Property

    Public Property Enabled() As Boolean Implements IControlAdapter.Enabled
        Get
            If TypeOf _control Is BoundField Then
                Return Not DirectCast(_control, BoundField).ReadOnly

            ElseIf TypeOf _control Is CheckBoxField Then
                Return Not DirectCast(_control, CheckBoxField).ReadOnly
            Else
                Return True
            End If
        End Get
        Set(ByVal value As Boolean)
            If TypeOf _control Is BoundField Then
                DirectCast(_control, BoundField).ReadOnly = Not value

            ElseIf TypeOf _control Is CheckBoxField Then
                DirectCast(_control, CheckBoxField).ReadOnly = Not value
            End If
        End Set
    End Property

    Public ReadOnly Property Identification(Optional ByVal parent As IControlAdapter = Nothing, Optional ByVal security As ControlRestrictedUI = Nothing) As String Implements IControlAdapter.Identification
        Get
            If parent IsNot Nothing Then
                Return parent.Identification(, security) + "." + Util.FormatIdentifier(_control.HeaderText)
            Else
                Return "<padre>" + "." + Util.FormatIdentifier(_control.HeaderText)
            End If

        End Get
    End Property

    Public Function Controls() As IList(Of IControlAdapter) Implements IControlAdapter.Controls
        Return New List(Of IControlAdapter)
    End Function

    Private Function FindControl(ByVal identifier As String) As IControlAdapter Implements IControlAdapter.FindControl
        Return New NullControlAdapter
    End Function

End Class
