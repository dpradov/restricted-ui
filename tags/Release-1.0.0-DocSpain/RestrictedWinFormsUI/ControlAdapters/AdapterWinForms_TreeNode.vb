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
''' Adaptador que maneja los nodos los los controles (<see cref=" Windows.Forms.TreeView"/>), para su uso desde la librería de Interface Restringida (<see cref="RestrictedUI "/>)
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

#Region "Métodos de clase"

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
        _visible = True   ' Por construcción, sólo se creará un adaptador para un nodo de un TreeView si éste está visible (existe..)
        ' Si se hizo invisible, no se crará uno nuevo sino que se devolverá el existente. Ver comentario en propiedad Visible
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
        ' Esta propiedad no será controlada en este control
    End Sub

    Public Sub SuperviseVisible(ByVal seguridad As ControlRestrictedUI) Implements IControlAdapter.SuperviseVisible
        _security = seguridad
    End Sub

    Sub FinalizeSupervision() Implements IControlAdapter.FinalizeSupervision
    End Sub

    ' El control TreeView no permite mostrar u ocultar sus nodos. Aquí lo permitimos a base de eliminar o añadir los nodos
    ' Es por ello que no se necesita capturar ningún evento tipo VisibleChanged. Si se quiere hacer visible o no deberá ser a través
    ' de este método
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
            ' No hay nada que hacer
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
                ' Este control ha sido eliminado del TreeView por su adaptador, para hacerlo invisible
                ' Vamos a localizar el adaptador del nodo padre
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

        ' Tenemos que añadir también los adaptadores de aquellos nodos que puedan estar ocultos.
        ' Como hemos tenido que eliminarlos del TreeView no se encontrarán con el bucle anterior
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
                    ' Comprobamos si ese control buscado es uno que se encuentra oculto
                    ' Si es así devolvemos directamente el adaptador ya existente
                    For Each adapt As AdapterWinForms_TreeNode In AdapterWinForms_TreeNode.hiddenNodesAdapter(parent)
                        If AbsoluteIdentifier.StartsWith(adapt.Identification.ToUpper) Then
                            If AbsoluteIdentifier = adapt.Identification.ToUpper Then
                                Return adapt    ' Es directamente el elemento buscado
                            Else
                                ' El elemento buscado es un hijo de éste -> seguimos con el bucle
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
