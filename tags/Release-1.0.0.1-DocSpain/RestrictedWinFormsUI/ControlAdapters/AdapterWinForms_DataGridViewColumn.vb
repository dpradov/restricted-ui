Option Strict On

Imports System.Windows.Forms
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
''' Adaptador que maneja las columnas de los controles <see cref=" DataGridView"/>, para su uso desde la librería de Interface Restringida (<see cref="RestrictedUI "/>)
''' </summary>
''' <remarks></remarks>
Public Class AdapterWinForms_DataGridViewColumn
    Implements IControlAdapter

    Private _control As DataGridViewColumn
    Private _parent As DataGridView
    Private _security As ControlRestrictedUI
    Private _superviseEnabled As Boolean = False
    Private _superviseVisible As Boolean = False

    Sub New(ByVal control As DataGridViewColumn, ByVal parent As DataGridView)
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
        _superviseEnabled = True
        RemoveHandler _parent.ColumnStateChanged, AddressOf DataGridView_ColumnStateChanged
        AddHandler _parent.ColumnStateChanged, AddressOf DataGridView_ColumnStateChanged
    End Sub

    Public Sub SuperviseVisible(ByVal security As ControlRestrictedUI) Implements IControlAdapter.SuperviseVisible
        _security = security
        _superviseVisible = True
        RemoveHandler _parent.ColumnStateChanged, AddressOf DataGridView_ColumnStateChanged
        AddHandler _parent.ColumnStateChanged, AddressOf DataGridView_ColumnStateChanged
    End Sub

    Sub FinalizeSupervision() Implements IControlAdapter.FinalizeSupervision
        RemoveHandler _parent.ColumnStateChanged, AddressOf DataGridView_ColumnStateChanged
    End Sub

    Private Sub DataGridView_ColumnStateChanged(ByVal sender As Object, ByVal e As DataGridViewColumnStateChangedEventArgs)
        If e.Column Is _control Then
            Select Case e.StateChanged
                Case DataGridViewElementStates.Visible
                    If _superviseVisible Then
                        _security.VerifyChange(Me, ControlRestrictedUI.TChange.Visible)
                    End If
                Case DataGridViewElementStates.ReadOnly
                    If _superviseEnabled Then
                        _security.VerifyChange(Me, ControlRestrictedUI.TChange.Enabled)
                    End If
            End Select
        End If
    End Sub

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
            Return Not _control.ReadOnly
        End Get
        Set(ByVal value As Boolean)
            _control.ReadOnly = Not value
        End Set
    End Property

    Public ReadOnly Property Identification(Optional ByVal parent As IControlAdapter = Nothing, Optional ByVal security As ControlRestrictedUI = Nothing) As String Implements IControlAdapter.Identification
        Get
            If parent IsNot Nothing Then
                Return parent.Identification(, security) + "." + _control.Name
            Else
                Return "<padre>" + "." + _control.Name
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
