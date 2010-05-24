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
''' Adapter for <see cref="Web.UI.WebControls.ListItem"/> controls, for its use from the Restricted User Interface library (<see cref="RestrictedUI"/>)
''' </summary>
''' <remarks>
''' <para>
''' A <see cref="ListItem"/> control represents an individual data item in a list control bound to data,
''' for example a control <see cref="ListBox"/> or <see cref="RadioButtonList"/>
''' </para>
''' <para>
''' Currently are only covered the <see cref="ListItem"/> used in <see cref="RadioButtonList"/> and <see cref="CheckBoxList"/>.
''' Those used in <see cref="ListBox"/> or <see cref="DropDownList"/> do not allow the use of the Enabled property.
''' If we want to offer at least Visible we must remove or add those elements similarly to how it is implemented the adapter 
''' for TreeNode (WinForms) (this would be easier because there isn't a hierarchy of nodes)
''' </para>
''' </remarks>
Public Class AdapterWeb_ListItem
    Implements IControlAdapter

    Private _control As System.Web.UI.WebControls.ListItem
    Private _parent As WebControl
    Private _security As ControlRestrictedUI

    Sub New(ByVal control As ListItem, ByVal parent As WebControl)
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
        _security = security
        RemoveHandler _parent.PreRender, AddressOf control_EnabledChanged
        AddHandler _parent.PreRender, AddressOf control_EnabledChanged
    End Sub

    Public Sub SuperviseVisible(ByVal security As ControlRestrictedUI) Implements IControlAdapter.SuperviseVisible
        '_security = security
        'RemoveHandler _parent.PreRender, AddressOf control_VisibleChanged
        'AddHandler _parent.PreRender, AddressOf control_VisibleChanged
    End Sub

    Sub FinalizarSupervision() Implements IControlAdapter.FinalizeSupervision
        'RemoveHandler _parent.PreRender, AddressOf control_VisibleChanged
        RemoveHandler _parent.PreRender, AddressOf control_EnabledChanged
    End Sub

    Private Sub control_VisibleChanged(ByVal sender As Object, ByVal ev As EventArgs)
        '_security.VerifyChange(Me, ControlSeguridad.TCambio.Visible)
    End Sub

    Private Sub control_EnabledChanged(ByVal sender As Object, ByVal ev As EventArgs)
        _security.VerifyChange(Me, ControlRestrictedUI.TChange.Enabled)
    End Sub

    Public Property Visible() As Boolean Implements IControlAdapter.Visible
        Get
            Return True
        End Get
        Set(ByVal value As Boolean)
        End Set
    End Property

    Public Property Enabled() As Boolean Implements IControlAdapter.Enabled
        Get
            Return _control.Enabled
        End Get
        Set(ByVal value As Boolean)
            _control.Enabled = value
        End Set
    End Property

    Public ReadOnly Property Identification(Optional ByVal parent As IControlAdapter = Nothing, Optional ByVal security As ControlRestrictedUI = Nothing) As String Implements IControlAdapter.Identification
        Get
            If parent IsNot Nothing Then
                Return parent.Identification(, security) + "." + Util.FormatIdentifier(_control.Value)
            Else
                Return "<padre>" + "." + Util.FormatIdentifier(_control.Value)
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
