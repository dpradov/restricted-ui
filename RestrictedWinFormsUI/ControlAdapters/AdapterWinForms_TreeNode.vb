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

    Protected _control As TreeNode
    Protected _visible As Boolean
    Protected _parent As Object
    Private _identification As String

    Protected _security As ControlRestrictedUI
    Protected _TV As TreeView_NodesSecurity = Nothing

    Private ReadOnly Property TV() As TreeView_NodesSecurity
        Get
            If _TV Is Nothing OrElse Not BelongsToTree(_control, _TV) Then
                _TV = GetTreeView_NodesSecurity(_control)
            End If
            Return _TV
        End Get
    End Property


    Protected Friend Class TreeView_NodesSecurity
        Public ReadOnly TreeView As TreeView
        Public ReadOnly Childs As New Dictionary(Of Object, ArrayList)
        Public ReadOnly NodesSupervised As New List(Of TreeNode)
        Protected Friend _HiddenControlsAdapter As New List(Of AdapterWinForms_TreeNode)

        Public Function HiddenNodesAdapter(Optional ByVal padre As Object = Nothing) As IList(Of AdapterWinForms_TreeNode)
            If padre Is Nothing Then
                Return _HiddenControlsAdapter
            Else
                Dim list As New List(Of AdapterWinForms_TreeNode)
                For Each adapt As AdapterWinForms_TreeNode In _HiddenControlsAdapter
                    If adapt._parent Is padre Then
                        list.Add(adapt)
                    End If
                Next
                Return list
            End If
        End Function

        Sub New(ByVal TreeView As TreeView)
            Me.TreeView = TreeView
        End Sub

    End Class


#Region "Shared"

    Private Shared _ListTV As New List(Of WeakReference)

    Protected Friend Shared Function GetTreeView_NodesSecurity(ByVal TreeView As TreeView) As TreeView_NodesSecurity
        For Each wr As WeakReference In _ListTV
            If wr.IsAlive AndAlso CType(wr.Target, TreeView_NodesSecurity).TreeView Is TreeView Then
                Return CType(wr.Target, TreeView_NodesSecurity)
            End If
        Next

        Dim TV = New TreeView_NodesSecurity(TreeView)
        _ListTV.Add(New WeakReference(TV))
        Return TV
    End Function

    Protected Friend Shared Function GetTreeView_NodesSecurity(ByVal TreeNode As TreeNode) As TreeView_NodesSecurity
        If TreeNode.TreeView IsNot Nothing Then
            Return GetTreeView_NodesSecurity(TreeNode.TreeView)

        Else
            For Each wr As WeakReference In _ListTV
                If wr.IsAlive Then
                    Dim TV = CType(wr.Target, TreeView_NodesSecurity)
                    If BelongsToTree(TreeNode, TV) Then
                        Return TV
                    End If
                End If
            Next
            Return Nothing
        End If
    End Function

    Protected Shared Function BelongsToTree(ByVal node As TreeNode, ByVal TV As TreeView_NodesSecurity) As Boolean
        If node.TreeView Is TV.TreeView Then Return True

        Dim belongs = False
        Dim auxNode As TreeNode

        For Each adapter In TV.HiddenNodesAdapter
            auxNode = node
            Do
                If auxNode Is adapter._control Then
                    belongs = True
                    Exit For
                End If
                auxNode = auxNode.Parent
            Loop Until auxNode Is Nothing
        Next

        Return belongs
    End Function

#End Region

    Sub New(ByVal control As TreeNode)
        _control = control
        _visible = True   ' By construction, only create an adapter for a node in a TreeView if it is visible (it exists..)
        ' If it was invisible, not create a new one but return the existing one. See comment to Visible property
        If _control.Parent IsNot Nothing Then
            _parent = _control.Parent
        Else
            _parent = _control.TreeView
        End If
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
        If TV Is Nothing Then Exit Sub ' It will update _TV

        _security = seguridad
        If Not _TV.NodesSupervised.Contains(_control) Then
            _TV.NodesSupervised.Add(_control)
        End If
        SaveSiblings()
    End Sub

    Sub FinalizeSupervision() Implements IControlAdapter.FinalizeSupervision
        If TV Is Nothing Then Exit Sub ' It will update _TV
        _TV.NodesSupervised.Remove(_control)
    End Sub

    ''' <remarks>
    ''' <para>The TreeView control does not allow to show or hide its nodes.</para>
    ''' Here we allow it by removing or adding nodes. That is why we do not need to capture any event like VisibleChanged.
    ''' <para>If it is wanted to make a node visible or invisible, must be done through this method</para>
    ''' </remarks>
    Public Property Visible() As Boolean Implements IControlAdapter.Visible
        Get
            Return _visible
        End Get
        Set(ByVal value As Boolean)
            If value <> _visible Then
                If TV Is Nothing Then Exit Property ' It will update _TV

                Dim supervised As Boolean = (_security IsNot Nothing AndAlso _TV.NodesSupervised.Contains(_control))
                Try
                    If Not supervised OrElse _
                       _security.ChangeAllowed(Me, ControlRestrictedUI.TChange.Visible, value) Then

                        If value = False Then
                            If Not supervised Then SaveSiblings() ' Supervised controls saved siblings on 'SuperviseVisible'
                            _identification = Identification
                            _visible = False
                            _control.Remove()
                            _TV._HiddenControlsAdapter.Add(Me)
                        Else
                            Dim nodes As TreeNodeCollection
                            If TypeOf _parent Is TreeView Then
                                nodes = DirectCast(_parent, TreeView).Nodes
                            Else
                                nodes = DirectCast(_parent, TreeNode).Nodes
                            End If
                            Dim siblings As ArrayList = _TV.Childs.Item(_parent)
                            Dim indexMe As Integer
                            Dim position As Integer = 0
                            Dim lastPrevNode As TreeNode = Nothing
                            indexMe = siblings.IndexOf(_control)
                            For Each node As TreeNode In nodes
                                If siblings.IndexOf(node) < indexMe Then
                                    lastPrevNode = node
                                Else
                                    Exit For
                                End If
                            Next
                            If lastPrevNode IsNot Nothing Then
                                position = nodes.IndexOf(lastPrevNode) + 1
                            End If
                            nodes.Insert(position, _control)
                            _visible = True
                            _TV._HiddenControlsAdapter.Remove(Me)
                        End If
                    End If

                Catch ex As Exception
                End Try
            End If
        End Set
    End Property

    Public Sub SaveSiblings()
        If TV Is Nothing Then Exit Sub ' It will update _TV

        If Not _TV.Childs.ContainsKey(_parent) Then

            Dim nodes As TreeNodeCollection
            If _control.Parent IsNot Nothing Then
                nodes = DirectCast(_parent, TreeNode).Nodes
            Else
                nodes = DirectCast(_parent, TreeView).Nodes
            End If

            Dim siblings As New ArrayList(nodes.Count)
            For Each node As TreeNode In nodes
                siblings.Add(node)
            Next
            _TV.Childs.Add(_parent, siblings)
        End If
    End Sub


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
                If TV Is Nothing Then Return cad

                ' This control has been removed from TreeView by its adapter, to make it invisible
                ' We will locate the parent node adapter
                For Each adapt As AdapterWinForms_TreeNode In _TV.HiddenNodesAdapter(Nothing)
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

        If TV IsNot Nothing Then     ' It will update _TV
            ' We must also add the adapters of nodes that may be hidden.
            ' As we had to remove them from the TreeView will not be found with the previous loop
            For Each adapt In _TV.HiddenNodesAdapter(_control)
                children.Add(adapt)
            Next
        End If

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
                    If TV Is Nothing Then Return New NullControlAdapter

                    ' We check whether that searched control is one that is hidden
                    ' If so we return directly the existing adapter
                    For Each adapt As AdapterWinForms_TreeNode In _TV.HiddenNodesAdapter(parent)
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
