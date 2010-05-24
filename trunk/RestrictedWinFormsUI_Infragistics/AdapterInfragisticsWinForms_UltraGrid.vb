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

    Private _readOnly As Boolean = False

    Sub New(ByVal control As UltraGrid)
        MyBase.New(control)
    End Sub


#Region "Customizing Enabled: Enabled or ReadOnly"

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
            _useReadOnly = value
        End Set
    End Property
    Protected Shared _useReadOnly As Boolean = False

    Public Overrides Sub SuperviseEnabled(ByVal security As ControlRestrictedUI)
        _security = security
        If Not _useReadOnly Then
            RemoveHandler _control.EnabledChanged, AddressOf control_EnabledChanged
            AddHandler _control.EnabledChanged, AddressOf control_EnabledChanged
        Else
            RemoveHandler DirectCast(_control, UltraGrid).BeforeEnterEditMode, AddressOf control_BeforeChanging
            AddHandler DirectCast(_control, UltraGrid).BeforeEnterEditMode, AddressOf control_BeforeChanging
            RemoveHandler DirectCast(_control, UltraGrid).BeforeRowInsert, AddressOf control_BeforeChanging
            AddHandler DirectCast(_control, UltraGrid).BeforeRowInsert, AddressOf control_BeforeChanging
            RemoveHandler DirectCast(_control, UltraGrid).BeforeRowsDeleted, AddressOf control_BeforeChanging
            AddHandler DirectCast(_control, UltraGrid).BeforeRowsDeleted, AddressOf control_BeforeChanging
        End If
    End Sub

    Protected Overrides Sub FinalizeSupervision()
        RemoveHandler _control.VisibleChanged, AddressOf control_VisibleChanged
        RemoveHandler _control.EnabledChanged, AddressOf control_EnabledChanged
        RemoveHandler DirectCast(_control, UltraGrid).BeforeEnterEditMode, AddressOf control_BeforeChanging
        RemoveHandler DirectCast(_control, UltraGrid).BeforeRowInsert, AddressOf control_BeforeChanging
        RemoveHandler DirectCast(_control, UltraGrid).BeforeRowsDeleted, AddressOf control_BeforeChanging
    End Sub

    Protected Sub control_BeforeChanging(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs)
        _security.VerifyChange(Me, ControlRestrictedUI.TChange.Enabled)
        e.Cancel = _readOnly
    End Sub

    Public Overrides Property Enabled() As Boolean
        Get
            If _useReadOnly Then
                Return _readOnly
            Else
                Return _control.Enabled
            End If
        End Get
        Set(ByVal value As Boolean)
            If _useReadOnly Then
                _readOnly = Not value
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