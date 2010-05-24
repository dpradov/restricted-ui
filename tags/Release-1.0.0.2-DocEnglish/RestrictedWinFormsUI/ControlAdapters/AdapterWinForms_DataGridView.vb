Option Strict On

Imports System.Windows.Forms
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
''' Adapter for <see cref="Windows.Forms.DataGridView"/> controls, for its use from the Restricted User Interface library (<see cref="RestrictedUI"/>)
''' </summary>
''' <remarks></remarks>
Public Class AdapterWinForms_DataGridView
    Inherits AdapterWinForms_Control
    Implements IControlAdapter

    Sub New(ByVal control As DataGridView)
        MyBase.New(control)
    End Sub

#Region "Customizing Enabled: Enabled or ReadOnly"
    ''' <summary>
    ''' The adapter manages the enabling of the wrapped control through the ReadOnly property, rather than Enabled property.
    ''' </summary>
    ''' <remarks>
    ''' <para>If set to <b>true</b> it is possible to move around the Grid, but you can not modify the contents of their cells.</para>
    ''' <para>If set to <b>false</b> (default) the Grid control is completely disabled and you can not be moved around it.</para>
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

    Public Overrides Sub SuperviseEnabled(ByVal seguridad As ControlRestrictedUI)
        _security = seguridad
        If Not _useReadOnly Then
            RemoveHandler _control.EnabledChanged, AddressOf control_EnabledChanged
            AddHandler _control.EnabledChanged, AddressOf control_EnabledChanged
        Else
            RemoveHandler DirectCast(_control, DataGridView).ReadOnlyChanged, AddressOf control_EnabledChanged
            AddHandler DirectCast(_control, DataGridView).ReadOnlyChanged, AddressOf control_EnabledChanged
        End If
    End Sub

    Protected Overrides Sub FinalizeSupervision()
        RemoveHandler _control.VisibleChanged, AddressOf control_VisibleChanged
        RemoveHandler _control.EnabledChanged, AddressOf control_EnabledChanged
        RemoveHandler DirectCast(_control, DataGridView).ReadOnlyChanged, AddressOf control_EnabledChanged
    End Sub

    Public Overrides Property Enabled() As Boolean
        Get
            If _useReadOnly Then
                Return Not DirectCast(_control, DataGridView).ReadOnly
            Else
                Return _control.Enabled
            End If
        End Get
        Set(ByVal value As Boolean)
            If _useReadOnly Then
                DirectCast(_control, DataGridView).ReadOnly = Not value
            Else
                _control.Enabled = value
            End If
        End Set
    End Property

#End Region

    Public Overrides Function Controls() As IList(Of IControlAdapter)
        Dim children As New List(Of IControlAdapter)
        Dim adapt As IControlAdapter

        Dim control As DataGridView = DirectCast(_control, DataGridView)
        For Each column As DataGridViewColumn In control.Columns
            adapt = SecurityEnvironment.GetAdapter(column, _control)
            If Not adapt.IsNull Then
                children.Add(adapt)
            End If
        Next
        Return children
    End Function

    Protected Overrides Function FindControl(ByVal identificador As String) As IControlAdapter
        Dim cad As String = identificador.ToUpper    '<columnHeader>
        Dim control As Object = Nothing

        Dim parent As DataGridView = DirectCast(_control, DataGridView)

        If parent IsNot Nothing Then
            Try
                For Each column As DataGridViewColumn In parent.Columns
                    If column.Name.ToUpper = cad Then
                        control = column
                        Exit For
                    End If
                Next

            Catch ex As Exception
            End Try
        End If

        Return SecurityEnvironment.GetAdapter(control, _control)
    End Function

End Class