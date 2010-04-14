﻿Option Strict On

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
''' Adaptador para los controles <see cref="Web.UI.WebControls.ListControl"/>: <see cref="Web.UI.WebControls.CheckBoxList "/> y <see cref="Web.UI.WebControls.RadioButtonList"/>, para su uso desde la 
''' librería de Interface Restringida (<see cref="RestrictedUI"/>)
''' </summary>
''' <remarks><see cref="ListBox"/> y <see cref="DropDownList "/> no admiten la modificación de la propiedad Enabled. Por ello de momento no se ofrecerá
''' un tratamiento especial para estos controles (tampoco ofrecen Visible). Pero sí sería posible ofrecer la funcionalidad
''' Visible a base de eliminar o añadir ese elemento en el control (algo análogo a lo que está hecho en TreeView
''' Se gestionarán por tanto con el adaptador genérico <see cref="AdapterWeb_WebControl"/>
''' </remarks>
Public Class AdapterWeb_ListControl
    Inherits AdapterWeb_WebControl
    Implements IControlAdapter

    Sub New(ByVal control As ListControl)
        MyBase.New(control)
    End Sub

    Public Overrides Function Controls() As IList(Of IControlAdapter)
        If TypeOf _control Is ListBox OrElse TypeOf _control Is DropDownList Then
            Return MyBase.Controls()
        End If

        Dim children As New List(Of IControlAdapter)
        Dim adapt As IControlAdapter

        For Each ctrl As ListItem In DirectCast(_control, ListControl).Items
            'adapt = New AdapterWeb_ListItem(ctrl, _control)   'Llamar a SecurityEnvironment.GetAdapter y no instanciar directamente el adaptador, para permitir implementaciones distintas
            adapt = SecurityEnvironment.GetAdapter(ctrl, _control)
            If Not TypeOf adapt Is NullControlAdapter Then
                children.Add(adapt)
            End If
        Next

        Return children
    End Function

    Protected Overrides Function FindControl(ByVal identifier As String) As IControlAdapter
        If TypeOf _control Is ListBox OrElse TypeOf _control Is DropDownList Then
            Return MyBase.FindControl(Identification)
        End If

        Dim pos As Integer = identifier.ToUpper.IndexOf("."c)
        Dim cad As String

        If pos < 0 Then
            cad = identifier.ToUpper
        Else
            cad = identifier.Substring(0, pos).ToUpper
        End If

        Try
            For Each c As ListItem In DirectCast(_control, ListControl).Items
                If c.Value.ToUpper = cad Then
                    Return SecurityEnvironment.GetAdapter(c, _control)
                End If
            Next
        Catch ex As Exception
        End Try

        Return New NullControlAdapter
    End Function


End Class