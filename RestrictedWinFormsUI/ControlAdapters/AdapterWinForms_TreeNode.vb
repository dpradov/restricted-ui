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
''' Adapter for the nodes of <see cref="Windows.Forms.TreeView"/> controls, for its use from the Restricted User Interface library (<see cref="RestrictedUI"/>)
''' </summary>
''' <remarks></remarks>
Public Class AdapterWinForms_TreeNode
    Implements IControlAdapter

    Protected _security As ControlRestrictedUI
    Protected _control As TreeNode
    Protected _visible As Boolean
    Protected _parent As Object
    Private _position As Integer
    Private _identification As String

#Region "Shared methods"

    Protected Shared _hiddenControlsAdapter As New List(Of AdapterWinForms_TreeNode)

    Protected Friend Shared Function hiddenNodesAdapter(Optional ByVal padre As Object = Nothing) As IList(Of AdapterWinForms_TreeNode)
        Dim list As New List(Of AdapterWinForms_TreeNode)
        For Each adapt As AdapterWinForms_TreeNode In _hiddenControlsAdapter
            If padre Is Nothing OrElse adapt._parent Is padre Then
                list.Add(adapt)
            End If
        Next
        Return list
    End Function

#End Region

    Sub New(ByVal control As TreeNode)
        _control = control
        _visible = True   ' By construction, only create an adapter for a node in a TreeView if it is visible (it exists..)
        ' If it was invisible, not create a new one but return the existing one. See comment to Visible property
        _position = -1
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

    Public Sub SuperviseEnabled(ByVal seguridad As ControlRestrictedUI) Implements IControlAdapter.SuperviseEnabled
        ' This property will not be monitored in this control
    End Sub

    Public Sub SuperviseVisible(ByVal seguridad As ControlRestrictedUI) Implements IControlAdapter.SuperviseVisible
        _security = seguridad
    End Sub

    Sub FinalizeSupervision() Implements IControlAdapter.FinalizeSupervision
    End Sub

    ''' <remarks>The TreeView control does not allow to show or hide its nodes.
    ''' Here we allow it by removing or adding nodes. That is why we do not need to capture any event like VisibleChanged.
    ''' If it is wanted to make a node visible or invisible, must be done through this method
    ''' </remarks>
    Public Property Visible() As Boolean Implements IControlAdapter.Visible
        Get
            Return _visible
        End Get
        Set(ByVal value As Boolean)
            If value <> _visible Then
                Try
                    Dim nodos As TreeNodeCollection

                    If value = False Then
                        If _control.Parent IsNot Nothing Then
                            _parent = _control.Parent
                            nodos = DirectCast(_parent, TreeNode).Nodes
                        Else
                            _parent = _control.TreeView
                            nodos = DirectCast(_parent, TreeView).Nodes
                        End If
                        _position = nodos.IndexOf(_control)
                        _identification = Identification
                        _visible = False
                        _control.Remove()
                        _hiddenControlsAdapter.Add(Me)
                    Else
                        If _security Is Nothing OrElse _security.ChangeAllowed(Me, ControlRestrictedUI.TChange.Visible) Then
                            If TypeOf _parent Is TreeView Then
                                nodos = DirectCast(_parent, TreeView).Nodes
                            Else
                                nodos = DirectCast(_parent, TreeNode).Nodes
                            End If
                            nodos.Insert(_position, _control)
                            _visible = True
                            _hiddenControlsAdapter.Remove(Me)
                        End If
                    End If
                Catch ex As Exception
                End Try
            End If
        End Set
    End Property

    Public Property Enabled() As Boolean Implements IControlAdapter.Enabled
        Get
            Return True
        End Get
        Set(ByVal value As Boolean)
            ' Nothing to be done
        End Set
    End Property

    Public ReadOnly Property Identification(Optional ByVal padre As IControlAdapter = Nothing, Optional ByVal seguridad As ControlRestrictedUI = Nothing) As String Implements IControlAdapter.Identification
        Get
            If Not _visible Then
                Return _identification
            End If

            Dim cad As String = ""
            Dim sep As String = ""
            Dim control As TreeNode = _control
            Dim _parent As TreeNode = control

            Do
                control = _parent
                cad = Util.FormatIdentifier(control.Text) + sep + cad
                sep = "."
                _parent = control.Parent
            Loop Until _parent Is Nothing

            If _control.TreeView IsNot Nothing Then
                cad = SecurityEnvironment.GetAdapter(_control.TreeView).Identification(, seguridad) + "." + cad
            Else
                ' This control has been removed from TreeView by its adapter, to make it invisible
                ' We will locate the parent node adapter
                For Each adapt As AdapterWinForms_TreeNode In AdapterWinForms_TreeNode.hiddenNodesAdapter(Nothing)
                    If adapt._control Is control Then
                        cad = adapt.Identification + "." + cad.Substring(cad.IndexOf("."c) + 1)
                        Exit For
                    End If
                Next

            End If

            Return cad
        End Get

    End Property

    Public Function Controls() As IList(Of IControlAdapter) Implements IControlAdapter.Controls
        Dim children As New List(Of IControlAdapter)
        Dim adapt As IControlAdapter

        For Each ctrl As TreeNode In _control.Nodes
            adapt = New AdapterWinForms_TreeNode(ctrl)
            children.Add(adapt)
        Next

        ' We must also add the adapters of nodes that may be hidden.
        ' As we had to remove them from the TreeView will not be found with the previous loop
        For Each adapt In AdapterWinForms_TreeNode.hiddenNodesAdapter(_control)
            children.Add(adapt)
        Next

        Return children
    End Function

    Private Function FindControl(ByVal identifier As String) As IControlAdapter Implements IControlAdapter.FindControl
        Dim cad As String() = identifier.ToUpper.Split("."c)
        Dim AbsoluteIdentifier As String = (Me.Identification + "." + identifier).ToUpper
        Dim parent As TreeNode = _control
        Dim control As TreeNode = Nothing

        Try
            For i As Integer = 0 To cad.Length - 1
                control = Nothing
                For Each c As TreeNode In parent.Nodes
                    If Util.FormatIdentifier(c.Text).ToUpper = cad(i) Then
                        control = c
                        parent = c
                        Exit For
                    End If
                Next
                If control Is Nothing Then
                    ' We check whether that searched control is one that is hidden
                    ' If so we return directly the existing adapter
                    For Each adapt As AdapterWinForms_TreeNode In AdapterWinForms_TreeNode.hiddenNodesAdapter(parent)
                        If AbsoluteIdentifier.StartsWith(adapt.Identification.ToUpper) Then
                            If AbsoluteIdentifier = adapt.Identification.ToUpper Then
                                Return adapt    ' It is directly the searched element
                            Else
                                ' The searched element is a child of this one -> continue with the loop
                                control = DirectCast(adapt.Control, TreeNode)
                                parent = control
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
