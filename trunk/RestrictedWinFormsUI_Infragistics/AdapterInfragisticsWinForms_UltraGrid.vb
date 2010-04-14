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
''' Adaptador para controles UltraGrid de NetAdventage (Infragistics), para su uso desde la librería de Interface Restringida (<see cref="RestrictedUI"/>)
''' </summary>
''' <remarks></remarks>
Public Class AdapterInfragisticsWinForms_UltraGrid
    Inherits AdapterWinForms_Control
    Implements IControlAdapter

    Private _readOnly As Boolean = False

    Sub New(ByVal control As UltraGrid)
        MyBase.New(control)
    End Sub


#Region "Personalización de Enabled: Enabled o ReadOnly"
    ''' <summary>
    ''' El adaptador controla la habilitación del control de usuario que envuelve, mediante la propiedad ReadOnly, en lugar de Enabled
    ''' </summary>
    ''' <remarks>
    ''' <para>Si se establece a <b>true</b> será posible moverse por el Grid, aunque no se podrá modificar el contenido de sus celdas.</para>
    ''' <para>Estando a <b>false</b> (valor por defecto) el Grid estará completamente deshabilitado y no se podrá desplazar por él.</para>
    ''' <para>Esta propiedad no es común a todos los <see cref="IControlAdapter"/></para>
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
        Dim cad As String() = identificador.Split("·"c)   '<nºbanda>·<tituloColumna>
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