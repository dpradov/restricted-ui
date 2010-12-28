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
''' Adapter for <see cref="Windows.Forms.ToolStripMenuItem"/> controls, for its use from the Restricted User Interface library (<see cref="RestrictedUI"/>)
''' </summary>
''' <remarks></remarks>
Public Class AdapterWinForms_ToolStripMenuItem
    Inherits AdapterWinForms_ToolStripItem
    Implements IControlAdapter

    Sub New(ByVal control As ToolStripMenuItem)
        MyBase.New(control)
        _control = control
    End Sub


    Public Overrides Function Controls() As IList(Of IControlAdapter)
        Dim children As New List(Of IControlAdapter)
        Dim adapt As IControlAdapter

        For Each ctrl As ToolStripItem In DirectCast(_control, ToolStripMenuItem).DropDownItems
            adapt = SecurityEnvironment.GetAdapter(ctrl)
            If Not TypeOf adapt Is NullControlAdapter Then
                children.Add(adapt)
            End If
        Next
        Return children
    End Function

    Protected Overrides Function FindControl(ByVal identifier As String) As IControlAdapter
        Dim pos As Integer = identifier.IndexOf("."c)
        Dim cad As String
        Dim parent As ToolStripMenuItem = DirectCast(_control, ToolStripMenuItem)

        If pos < 0 Then
            cad = identifier.ToUpper
        Else
            cad = identifier.Substring(0, pos).ToUpper
        End If

        Try
            For Each c As ToolStripItem In parent.DropDownItems
                If c.Name.ToUpper = cad Then
                    cad = identifier.Substring(pos + 1)
                    If cad = "" Or pos < 0 Then
                        Return SecurityEnvironment.GetAdapter(c)
                    Else
                        Return SecurityEnvironment.GetAdapter(c).FindControl(cad)
                    End If
                End If
            Next
        Catch ex As Exception
        End Try

        Return New NullControlAdapter
    End Function

End Class