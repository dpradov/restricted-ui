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
''' Adaptador para controles (<see cref="Web.UI.WebControls.ListItem"/>), para su uso desde la librería de Interface Restringida (<see cref="RestrictedUI"/>)
''' (Un control <see cref="ListItem"/> representa un elemento de datos individual dentro de un control de lista enlazado a datos,
''' por ejemplo un control <see cref="ListBox"/> o <see cref="RadioButtonList"/>)
''' </summary>
''' <remarks>De momento sólo se tratan los <see cref="ListItem"/> utilizados en <see cref="RadioButtonList"/> y <see cref="CheckBoxList"/>. Los que se utilizan desde 
''' <see cref="ListBox"/> o <see cref="DropDownList"/> no permiten el uso de la propiedad Enabled. De querer que ofrezcan al menos Visible deberemos eliminar
''' o añadir esos elementos de igual manera a como está implementado el adaptador de TreeNode (WinForms) -- Aquí sería más sencillo
''' pues no hay una jerarquía de nodos
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
