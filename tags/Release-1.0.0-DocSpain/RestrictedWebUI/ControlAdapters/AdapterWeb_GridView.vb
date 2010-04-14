Option Strict On

Imports System.Web.UI.WebControls
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
''' Adaptador para controles <see cref="Web.UI.WebControls.GridView "/>, para su uso desde la librería de Interface Restringida (<see cref="RestrictedUI"/>)
''' </summary>
''' <remarks></remarks>
Public Class AdapterWeb_GridView
    Inherits AdapterWeb_WebControl
    Implements IControlAdapter

    Sub New(ByVal control As GridView)
        MyBase.New(control)
    End Sub

    Public Overrides Function Controls() As IList(Of IControlAdapter)
        Dim children As New List(Of IControlAdapter)
        Dim adapt As IControlAdapter

        Dim control As GridView
        control = DirectCast(_control, GridView)

        For Each column As DataControlField In control.Columns
            adapt = New AdapterWeb_DataControlField(column, control)
            children.Add(adapt)
        Next
        Return children
    End Function

    Protected Overrides Function FindControl(ByVal identifier As String) As IControlAdapter
        Dim cad As String = identifier.ToUpper   '<tituloColumna>
        Dim control As Object = Nothing

        Dim parent As GridView
        parent = DirectCast(_control, GridView)

        If parent IsNot Nothing Then
            Try
                For Each column As DataControlField In parent.Columns
                    If Util.FormatIdentifier(column.HeaderText).ToUpper = cad Then
                        control = column
                        Exit For
                    End If
                Next
            Catch ex As Exception
            End Try
        End If
        Return SecurityEnvironment.GetAdapter(control, parent)
    End Function

End Class