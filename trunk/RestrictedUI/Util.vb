Option Strict Off

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
''' Singleton class (via Shared) that provides various utilities and functions
''' </summary>
Public Class Util

    ''' <summary>
    ''' Replaces points by · and commas by ´
    ''' </summary>
    Public Shared Function FormatIdentifier(ByVal id As String) As String
        Return id.Replace("."c, "·"c).Replace(","c, "´"c)
    End Function

    ''' <summary>
    ''' Gets an array of integers from a string with numbers separated by commas
    ''' </summary>
    Public Shared Function ConvertToArrayInt(ByVal valores As String) As Integer()
        If valores.Trim = "" Then
            Return New Integer() {}
        Else
            Dim valoresStr() As String = valores.Split(","c)
            Dim valoresInt(valoresStr.Length - 1) As Integer
            Dim x As Integer = 0
            For i As Integer = 0 To valoresStr.Length - 1
                If Integer.TryParse(valoresStr(i).Trim, valoresInt(x)) Then
                    x += 1
                End If
            Next
            ReDim Preserve valoresInt(x - 1)

            Return valoresInt
        End If

    End Function

    ''' <summary>
    ''' Gets a string array from another string with items separated by commas
    ''' </summary>
    Public Shared Function ConvierteEnArrayStr(ByVal valores As String) As String()
        If valores.Trim = "" Then
            Return New String() {}
        Else
            Dim valoresStr() As String = valores.Split(","c)
            Dim valoresStrOut(valoresStr.Length - 1) As String
            Dim x As Integer = 0
            For i As Integer = 0 To valoresStr.Length - 1
                If valoresStr(i).Trim <> "" Then
                    valoresStrOut(x) = valoresStr(i).Trim
                    x += 1
                End If
            Next
            ReDim Preserve valoresStrOut(x - 1)

            Return valoresStrOut
        End If

    End Function

    ''' <summary>
    ''' Gets a text string with comma-separated numbers from an array of integers
    ''' </summary>
    Public Shared Function ConvertToString(ByVal valores() As Integer) As String
        If valores Is Nothing OrElse valores.Length = 0 Then
            Return ""
        Else
            Dim valoresStr As String = ""
            Dim sep As String = ""
            For Each valor As Integer In valores
                valoresStr += sep + valor.ToString
                sep = ", "
            Next
            Return valoresStr
        End If

    End Function

    ''' <summary>
    ''' Gets a text string with items separated by commas from a string array
    ''' </summary>
    Public Shared Function ConvertToString(ByVal valores() As String) As String
        If valores Is Nothing OrElse valores.Length = 0 Then
            Return ""
        Else
            Dim valoresStr As String = ""
            Dim sep As String = ""
            For Each valor As String In valores
                If valor <> "" Then
                    valoresStr += sep + valor.ToString
                    sep = ", "
                End If
            Next
            Return valoresStr
        End If

    End Function

    ''' <summary>
    ''' Gets the identifier of the control facilitated, via Reflection, by using the Name property if it is an WinForms object or the ID property 
    ''' if it's a Web object.
    ''' </summary>
    Public Shared Function GetControlID(ByVal control As Object) As String
        If control Is Nothing Then Return ""

        Dim t As System.Type = control.GetType
        Dim id As String
        If TypeOf control Is System.Windows.Forms.Control Then
            id = DirectCast(t.InvokeMember("Name", Reflection.BindingFlags.GetProperty, Nothing, control, Nothing, Nothing, Nothing, Nothing), String)
        Else
            id = DirectCast(t.InvokeMember("ID", Reflection.BindingFlags.GetProperty, Nothing, control, Nothing, Nothing, Nothing, Nothing), String)
        End If

        Return id
    End Function

    ''' <summary>
    ''' Gets, dinamically, the identifier of the parent of the control provided, by using the Name property if it is an WinForms object or the ID property 
    ''' if it's a Web object.
    ''' </summary>
    Public Shared Function GetParentID(ByVal control As Object) As String
        If control Is Nothing Then Return ""

        Try
            Dim t As System.Type = control.GetType
            If TypeOf control Is System.Windows.Forms.Control Then
                Return control.Parent.Name
            Else
                Return control.Parent.ID
            End If
        Catch ex As Exception
            Return ""
        End Try

    End Function

End Class
