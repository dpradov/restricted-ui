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
''' Adapter for <see cref="Windows.Forms.TreeView"/> controls, for its use from the Restricted User Interface library (<see cref="RestrictedUI"/>)
''' </summary>
''' <remarks></remarks>
Public Class AdapterWinForms_TreeView
    Inherits AdapterWinForms_Control
    Implements IControlAdapter

    Sub New(ByVal control As TreeView)
        MyBase.New(control)
    End Sub

    Public Overrides Function Controls() As IList(Of IControlAdapter)
        Dim children As New List(Of IControlAdapter)
        Dim adapt As IControlAdapter

        For Each ctrl As TreeNode In DirectCast(_control, TreeView).Nodes
            adapt = New AdapterWinForms_TreeNode(ctrl)
            children.Add(adapt)
        Next

        ' We must also add adapters of nodes that may be hidden.
        ' As we had to remove them from the TreeView will not be found with the previous loop
        For Each adapt In AdapterWinForms_TreeNode.GetTreeView_NodesSecurity(CType(_control, TreeView)).HiddenNodesAdapter(_control)
            children.Add(adapt)
        Next

        Return children
    End Function

    Protected Overrides Function FindControl(ByVal identifier As String) As IControlAdapter
        Dim cad As String() = identifier.ToUpper.Split("."c)
        Dim control As TreeNode = Nothing
        Dim parent As Object
        Dim nodes As TreeNodeCollection
        Dim absoluteIdentifier As String = (Me.Identification + "." + identifier).ToUpper

        Try
            nodes = DirectCast(_control, TreeView).Nodes
            parent = _control

            For i As Integer = 0 To cad.Length - 1
                control = Nothing
                For Each c As TreeNode In nodes
                    If c.Text.ToUpper = cad(i) Then
                        control = c
                        parent = c
                        nodes = c.Nodes
                        Exit For
                    End If
                Next
                If control Is Nothing Then
                    ' We check whether that control is one that is hidden
                    ' If so we return directly the existing adapter
                    For Each adapt As AdapterWinForms_TreeNode In AdapterWinForms_TreeNode.GetTreeView_NodesSecurity(CType(_control, TreeView)).HiddenNodesAdapter(parent)
                        If absoluteIdentifier.StartsWith(adapt.Identification.ToUpper) Then
                            If absoluteIdentifier = adapt.Identification.ToUpper Then
                                Return adapt    ' It is directly the searched element
                            Else
                                ' The searched element is a child of this one -> continue with the loop
                                control = DirectCast(adapt.Control, TreeNode)
                                parent = control
                                nodes = control.Nodes
                            End If

                        End If
                    Next
                End If

            Next

        Catch ex As Exception
        End Try

        Return SecurityEnvironment.GetAdapter(control)
    End Function


End Class