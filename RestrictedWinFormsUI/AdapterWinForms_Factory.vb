Option Strict On

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
''' Clase Singleton que ofrece una factoría de adaptadores (<see cref="IControlAdapterFactory "/>) para los
''' controles de aplicaciones WinForms. Gestiona los controles TreeView y DataGridView a través de unos adaptadores
''' específicos, para el resto ofrece un único adaptador genérico, <see cref="AdapterWinForms_Control  "/>
''' </summary>
''' <remarks></remarks>
Public Class AdapterWinForms_Factory
    Implements IControlAdapterFactory

#Region "Singleton"
    Private Shared _instance As AdapterWinForms_Factory

    Private Sub New()
    End Sub

    Public Shared Function getInstance() As AdapterWinForms_Factory
        If _instance Is Nothing Then
            _instance = New AdapterWinForms_Factory
        End If
        Return _instance
    End Function
#End Region

    Function GetAdapter(ByVal control As Object, Optional ByVal parent As Object = Nothing) As IControlAdapter Implements IControlAdapterFactory.GetAdapter

        If control Is Nothing Then

#If DEBUG Then
            'MsgBox(">NullControlAdapter  (Nothing)")
#End If
            Return New NullControlAdapter
        End If


        ' WINFORMS
        '-------------
        If TypeOf control Is System.Windows.Forms.ToolStrip Then
            Return New AdapterWinForms_ToolStrip(DirectCast(control, System.Windows.Forms.ToolStrip))

        ElseIf TypeOf control Is System.Windows.Forms.ToolStripMenuItem Then
            Return New AdapterWinForms_ToolStripMenuItem(DirectCast(control, System.Windows.Forms.ToolStripMenuItem))

        ElseIf TypeOf control Is System.Windows.Forms.ToolStripItem Then
            Return New AdapterWinForms_ToolStripItem(DirectCast(control, System.Windows.Forms.ToolStripItem))

        ElseIf TypeOf control Is System.Windows.Forms.TreeNode Then
            Return New AdapterWinForms_TreeNode(DirectCast(control, System.Windows.Forms.TreeNode))

        ElseIf TypeOf control Is System.Windows.Forms.TreeView Then
            Return New AdapterWinForms_TreeView(DirectCast(control, System.Windows.Forms.TreeView))

        ElseIf TypeOf control Is System.Windows.Forms.DataGridView Then
            Return New AdapterWinForms_DataGridView(DirectCast(control, System.Windows.Forms.DataGridView))

        ElseIf TypeOf control Is System.Windows.Forms.DataGridViewColumn Then
            Return New AdapterWinForms_DataGridViewColumn(DirectCast(control, System.Windows.Forms.DataGridViewColumn), DirectCast(parent, System.Windows.Forms.DataGridView))

        ElseIf TypeOf control Is System.Windows.Forms.Control Then
            Return New AdapterWinForms_Control(DirectCast(control, System.Windows.Forms.Control))

        End If


        Return New NullControlAdapter

    End Function

End Class
