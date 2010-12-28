Option Strict On

Imports System.Windows.Forms
Imports Infragistics.Win.UltraWinTree
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
''' Adapter for UltreTree controls (NetAdventage, Infragistics), for its use from the Restricted User Interface library (<see cref="RestrictedUI"/>)
''' </summary>
''' <remarks></remarks>
Public Class AdapterInfragisticsWinForms_UltraTree
    Inherits AdapterWinForms_Control
    Implements IControlAdapter

    Sub New(ByVal control As UltraTree)
        MyBase.New(control)
    End Sub

    Public Overrides Function Controls() As IList(Of IControlAdapter)
        Dim children As New List(Of IControlAdapter)
        Dim adapt As IControlAdapter

        For Each ctrl As UltraTreeNode In DirectCast(_control, UltraTree).Nodes
            adapt = New AdapterInfragisticsWinForms_UltraTreeNode(ctrl)
            children.Add(adapt)
        Next
        Return children
    End Function

    Protected Overrides Function FindControl(ByVal identifier As String) As IControlAdapter
        Dim cad As String() = identifier.Split("."c)
        Dim control As UltraTreeNode = Nothing
        Dim nodes As TreeNodesCollection

        Try
            nodes = DirectCast(_control, UltraTree).Nodes

            For i As Integer = 0 To cad.Length - 1
                For Each c As UltraTreeNode In nodes
                    If Util.FormatIdentifier(c.Text).ToUpper = cad(i).ToUpper Then
                        control = c
                        nodes = c.Nodes
                        Exit For
                    End If
                Next
            Next

        Catch ex As Exception
        End Try

        Return SecurityEnvironment.GetAdapter(control)
    End Function

End Class