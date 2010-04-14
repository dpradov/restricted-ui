Option Strict On

Imports System.Windows.Forms
Imports Infragistics.Win.UltraWinTree
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
''' Adaptador que maneja los nodos de los controles UltraTree de NetAdventage (Infragistics), para su uso desde
''' la librería de Interface Restringida (<see cref="RestrictedUI"/>)
''' </summary>
''' <remarks></remarks>
Public Class AdapterInfragisticsWinForms_UltraTreeNode
    Implements IControlAdapter

    Private _control As UltraTreeNode
    Private _security As ControlRestrictedUI

    Sub New(ByVal control As UltraTreeNode)
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
        RemoveHandler _control.SubObjectPropChanged, AddressOf control_EnabledChanged
        AddHandler _control.SubObjectPropChanged, AddressOf control_EnabledChanged
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
            Return _control.Visible
        End Get
        Set(ByVal value As Boolean)
            _control.Visible = value
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

    Public ReadOnly Property Identification(Optional ByVal parent As IControlAdapter = Nothing, Optional ByVal seguridad As ControlRestrictedUI = Nothing) As String Implements IControlAdapter.Identification
        Get
            Dim cad As String = ""
            Dim sep As String = ""
            Dim control As UltraTreeNode = _control

            Do
                cad = Util.FormatIdentifier(control.Text) + sep + cad
                sep = "."
                control = control.Parent
            Loop Until control Is Nothing

            cad = SecurityEnvironment.GetAdapter(_control.Control).Identification(, seguridad) + "." + cad
            Return cad
        End Get

    End Property

    Public Function Controls() As IList(Of IControlAdapter) Implements IControlAdapter.Controls
        Dim children As New List(Of IControlAdapter)
        Dim adapt As IControlAdapter

        For Each ctrl As UltraTreeNode In _control.Nodes
            adapt = New AdapterInfragisticsWinForms_UltraTreeNode(ctrl)
            children.Add(adapt)
        Next
        Return children
    End Function

    Private Function FindControl(ByVal identifier As String) As IControlAdapter Implements IControlAdapter.FindControl
        Dim cad As String() = identifier.Split("."c)
        Dim parent As UltraTreeNode = _control
        Dim control As UltraTreeNode = Nothing

        Try
            For i As Integer = 0 To cad.Length - 1
                For Each c As UltraTreeNode In parent.Nodes
                    If Util.FormatIdentifier(c.Text) = cad(i) Then
                        control = c
                        parent = c
                        Exit For
                    End If
                Next
            Next

        Catch ex As Exception
        End Try

        Return SecurityEnvironment.GetAdapter(control)
    End Function

End Class
