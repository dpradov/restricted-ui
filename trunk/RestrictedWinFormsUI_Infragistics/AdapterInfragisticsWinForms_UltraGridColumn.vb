Option Strict On

Imports System.Windows.Forms
Imports Infragistics.Win.UltraWinGrid
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
''' Adaptador que maneja las columnas de los controles UltraGrid de NetAdventage (Infragistics), para su uso desde
''' la librería de Interface Restringida (<see cref="RestrictedUI"/>)
''' </summary>
''' <remarks></remarks>
Public Class AdapterInfragisticsWinForms_UltraGridColumn
    Implements IControlAdapter

    Private _control As UltraGridColumn
    Private _security As ControlRestrictedUI

    Sub New(ByVal control As UltraGridColumn)
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

    Public Sub SuperviseEnabled(ByVal security As ControlRestrictedUI) Implements IControlAdapter.SuperviseEnabled
        _security = security
        'RemoveHandler _control.SubObjectPropChanged, AddressOf control_EnabledChanged
        'AddHandler _control.SubObjectPropChanged, AddressOf control_EnabledChanged

        RemoveHandler _control.Band.SubObjectPropChanged, AddressOf control_EnabledChanged
        AddHandler _control.Band.SubObjectPropChanged, AddressOf control_EnabledChanged
    End Sub

    Public Sub SuperviseVisible(ByVal security As ControlRestrictedUI) Implements IControlAdapter.SuperviseVisible
        _security = security
        RemoveHandler _control.SubObjectPropChanged, AddressOf control_VisibleChanged
        AddHandler _control.SubObjectPropChanged, AddressOf control_VisibleChanged
    End Sub

    Sub FinalizeSupervision() Implements IControlAdapter.FinalizeSupervision
        RemoveHandler _control.SubObjectPropChanged, AddressOf control_VisibleChanged
        RemoveHandler _control.SubObjectPropChanged, AddressOf control_EnabledChanged
    End Sub

    Private Sub control_VisibleChanged(ByVal propChange As Infragistics.Shared.PropChangeInfo)
        _security.VerifyChange(Me, ControlRestrictedUI.TChange.Visible)
    End Sub

    Private Sub control_EnabledChanged(ByVal propChange As Infragistics.Shared.PropChangeInfo)
        _security.VerifyChange(Me, ControlRestrictedUI.TChange.Enabled)
    End Sub

    Public Property Visible() As Boolean Implements IControlAdapter.Visible
        Get
            Return Not _control.Hidden
        End Get
        Set(ByVal value As Boolean)
            _control.Hidden = Not value
        End Set
    End Property

    Public Property Enabled() As Boolean Implements IControlAdapter.Enabled
        Get
            Return _control.CellActivation = Infragistics.Win.UltraWinGrid.Activation.AllowEdit
        End Get
        Set(ByVal value As Boolean)
            If value Then
                _control.CellActivation = Infragistics.Win.UltraWinGrid.Activation.AllowEdit
            Else
                _control.CellActivation = Infragistics.Win.UltraWinGrid.Activation.ActivateOnly
            End If
        End Set
    End Property

    Public ReadOnly Property Identification(Optional ByVal parent As IControlAdapter = Nothing, Optional ByVal security As ControlRestrictedUI = Nothing) As String Implements IControlAdapter.Identification
        Get
            If parent IsNot Nothing Then
                Return parent.Identification(, security) + "." + _control.Band.Index.ToString + "·" + Util.FormatIdentifier(_control.Header.Caption)
            Else
                Return "<padre>" + "." + _control.Band.Index.ToString + "·" + Util.FormatIdentifier(_control.Header.Caption)
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
