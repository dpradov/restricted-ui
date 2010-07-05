Option Strict On

Imports System.Windows.Forms
Imports Infragistics.Win.UltraWinGrid
Imports RestrictedUI
Imports RestrictedWinFormsUI

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
''' Adapter for UltraGrid controls (NetAdventage, Infragistics), for its use from the Restricted User Interface library (<see cref="RestrictedUI"/>)
''' </summary>
''' <remarks></remarks>
Public Class AdapterInfragisticsWinForms_UltraGrid
    Inherits AdapterWinForms_Control
    Implements IControlAdapter

    Private _internalChange As Boolean = False
    Protected Shared DictReadOnly As New Dictionary(Of Control, Boolean)

    Sub New(ByVal control As UltraGrid)
        MyBase.New(control)
        If Not DictReadOnly.ContainsKey(_control) Then
            DictReadOnly.Add(_control, False)
        End If
    End Sub

#Region "Customizing Enabled: Enabled or ReadOnly"

    Protected Shared Event UseReadOnlyChanged()

    ''' <summary>
    ''' The adapter manages the enabling of the wrapped control through the ReadOnly property, rather than Enabled property.
    ''' </summary>
    ''' <remarks>
    ''' <para>If set to <b>true</b> it is possible to move around the UltraGrid control, but you can not modify the contents of their cells.</para>
    ''' <para>If set to <b>false</b> (default) the UltraGrid control is completely disabled and you can not be moved around it.</para>
    ''' <para>This property is not common to all <see cref="IControlAdapter"/></para>
    ''' </remarks>
    Public Shared Property UseReadOnly() As Boolean
        Get
            Return _useReadOnly
        End Get
        Set(ByVal value As Boolean)
            If _useReadOnly <> value Then
                _useReadOnly = value
                RaiseEvent UseReadOnlyChanged()
            End If
        End Set
    End Property
    Protected Shared _useReadOnly As Boolean = False

    Protected Shared Event ReadOnlyChanged()
    Protected Property [ReadOnly]() As Boolean
        Get
            Return DictReadOnly.Item(_control)
        End Get
        Set(ByVal value As Boolean)
            If [ReadOnly] <> value Then
                DictReadOnly.Item(_control) = value
                If Not _internalChange Then
                    RaiseEvent ReadOnlyChanged()   ' To give chances to undo the change
                End If
            End If
        End Set
    End Property


    Public Overrides Sub SuperviseEnabled(ByVal security As ControlRestrictedUI)
        _security = security
        RemoveHandler AdapterInfragisticsWinForms_UltraGrid.UseReadOnlyChanged, AddressOf OnUseReadOnlyChanged
        AddHandler AdapterInfragisticsWinForms_UltraGrid.UseReadOnlyChanged, AddressOf OnUseReadOnlyChanged
        RemoveHandler AdapterInfragisticsWinForms_UltraGrid.ReadOnlyChanged, AddressOf OnReadOnlyChanged
        AddHandler AdapterInfragisticsWinForms_UltraGrid.ReadOnlyChanged, AddressOf OnReadOnlyChanged

        RemoveHandler _control.EnabledChanged, AddressOf UltraGrid_EnabledChanged
        AddHandler _control.EnabledChanged, AddressOf UltraGrid_EnabledChanged

        RemoveHandler DirectCast(_control, UltraGrid).BeforeEnterEditMode, AddressOf control_BeforeChanging
        AddHandler DirectCast(_control, UltraGrid).BeforeEnterEditMode, AddressOf control_BeforeChanging
        RemoveHandler DirectCast(_control, UltraGrid).BeforeRowInsert, AddressOf control_BeforeChanging
        AddHandler DirectCast(_control, UltraGrid).BeforeRowInsert, AddressOf control_BeforeChanging
        RemoveHandler DirectCast(_control, UltraGrid).BeforeRowsDeleted, AddressOf control_BeforeChanging
        AddHandler DirectCast(_control, UltraGrid).BeforeRowsDeleted, AddressOf control_BeforeChanging
    End Sub

    Protected Overrides Sub FinalizeSupervision()
        RemoveHandler _control.VisibleChanged, AddressOf control_VisibleChanged
        RemoveHandler _control.EnabledChanged, AddressOf UltraGrid_EnabledChanged
        RemoveHandler DirectCast(_control, UltraGrid).BeforeEnterEditMode, AddressOf control_BeforeChanging
        RemoveHandler DirectCast(_control, UltraGrid).BeforeRowInsert, AddressOf control_BeforeChanging
        RemoveHandler DirectCast(_control, UltraGrid).BeforeRowsDeleted, AddressOf control_BeforeChanging
        RemoveHandler AdapterInfragisticsWinForms_UltraGrid.UseReadOnlyChanged, AddressOf OnUseReadOnlyChanged
        RemoveHandler AdapterInfragisticsWinForms_UltraGrid.ReadOnlyChanged, AddressOf OnReadOnlyChanged
    End Sub

    Protected Sub control_BeforeChanging(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs)
        e.Cancel = [ReadOnly]
    End Sub

    Protected Sub UltraGrid_EnabledChanged(ByVal sender As Object, ByVal e As EventArgs)
        If _internalChange Then Exit Sub
        _internalChange = True
        Try
            If Not _useReadOnly Then
                _security.VerifyChange(Me, ControlRestrictedUI.TChange.Enabled)
            Else
                _control.Enabled = True
                If _security.ChangeAllowed(Me, ControlRestrictedUI.TChange.Enabled, False) Then
                    [ReadOnly] = True
                End If
            End If

        Finally
            _internalChange = False
        End Try
    End Sub

    Protected Sub OnUseReadOnlyChanged()
        Dim EnabledState As Boolean

        _internalChange = True
        Try
            If _useReadOnly Then
                ' useReadOnly was False before, and so Enabled state is actually set by .Enabled property
                EnabledState = _control.Enabled
                _control.Enabled = True
            Else
                EnabledState = Not [ReadOnly]
                [ReadOnly] = False
            End If

            Enabled = EnabledState

        Finally
            _internalChange = False
        End Try
    End Sub

    Protected Sub OnReadOnlyChanged()
        If Not _security.ChangeAllowed(Me, ControlRestrictedUI.TChange.Enabled, Not [ReadOnly]) Then  ' It isn't necessary when UseReadOnly = False. The event triggered on Enabled change will be controlled
            _internalChange = True
            Try
                [ReadOnly] = Not [ReadOnly]
            Finally
                _internalChange = False
            End Try
        End If
    End Sub

    Public Overrides Property Enabled() As Boolean
        Get
            If _useReadOnly Then
                Return Not [ReadOnly]
            Else
                Return _control.Enabled
            End If
        End Get
        Set(ByVal value As Boolean)
            If _useReadOnly Then
                [ReadOnly] = Not value
            Else
                _control.Enabled = value
            End If
        End Set
    End Property

#End Region


    Public Overrides Function Controls() As IList(Of IControlAdapter)
        Dim hijos As New List(Of IControlAdapter)
        Dim adapt As IControlAdapter

        Dim control As UltraGrid
        control = DirectCast(_control, UltraGrid)

        For Each banda As UltraGridBand In control.DisplayLayout.Bands
            For Each columna As UltraGridColumn In banda.Columns
                adapt = SecurityEnvironment.GetAdapter(columna)
                If Not TypeOf adapt Is NullControlAdapter Then
                    hijos.Add(adapt)
                End If
            Next
        Next
        Return hijos
    End Function

    Protected Overrides Function FindControl(ByVal identificador As String) As IControlAdapter
        Dim cad As String() = identificador.Split("·"c)   '<band nº>·<columnHeader>
        Dim control As Object = Nothing

        Dim padre As UltraGrid
        padre = DirectCast(_control, UltraGrid)

        If padre IsNot Nothing Then
            Try
                Dim banda As UltraGridBand = padre.DisplayLayout.Bands(CType(cad(0), Integer))
                For Each columna As UltraGridColumn In banda.Columns
                    If Util.FormatIdentifier(columna.Header.Caption) = cad(1) Then
                        control = columna
                        Exit For
                    End If
                Next
            Catch ex As Exception
            End Try
        End If

        Return SecurityEnvironment.GetAdapter(control)
    End Function

End Class