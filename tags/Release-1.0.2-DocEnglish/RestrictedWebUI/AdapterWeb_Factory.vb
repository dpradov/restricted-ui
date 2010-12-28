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
''' Singleton class that provides a factory of adapters (<see cref="IControlAdapterFactory "/>) for the controls of Web applications.
''' Manages the controls <see cref="Web.UI.WebControls.GridView"/>, <see cref="Web.UI.WebControls.ListControl"/>, 
''' <see cref="Web.UI.WebControls.ListItem"/> and <see cref="Web.UI.WebControls.DataControlField"/> through some specific adapters;
''' for the rest of controls, <see cref="Web.UI.WebControls.WebControl"/> and <see cref="Web.UI.Control"/>, provides two generic 
''' adapters, <see cref="AdapterWeb_WebControl "/> and <see cref="AdapterWeb_UIControl "/>
''' </summary>
''' <remarks></remarks>
Public Class AdapterWeb_Factory
    Implements IControlAdapterFactory

#Region "Singleton"
    Private Shared _instance As AdapterWeb_Factory

    Private Sub New()
    End Sub

    Public Shared Function getInstance() As AdapterWeb_Factory
        If _instance Is Nothing Then
            _instance = New AdapterWeb_Factory
        End If
        Return _instance
    End Function
#End Region

    Function GetAdapter(ByVal control As Object, Optional ByVal parent As Object = Nothing) As IControlAdapter Implements IControlAdapterFactory.GetAdapter

        If control Is Nothing Then

#If DEBUG Then
            ' MsgBox(">NullControlAdapter  (Nothing)")
#End If
            Return New NullControlAdapter
        End If

        ' WEB
        '-------------
        If TypeOf control Is System.Web.UI.WebControls.GridView Then
            Return New AdapterWeb_GridView(DirectCast(control, System.Web.UI.WebControls.GridView))

        ElseIf TypeOf control Is System.Web.UI.WebControls.ListControl Then
            Return New AdapterWeb_ListControl(DirectCast(control, System.Web.UI.WebControls.ListControl))

        ElseIf TypeOf control Is System.Web.UI.WebControls.WebControl Then
            Return New AdapterWeb_WebControl(DirectCast(control, System.Web.UI.WebControls.WebControl))

        ElseIf TypeOf control Is System.Web.UI.WebControls.DataControlField Then
            Return New AdapterWeb_DataControlField(DirectCast(control, System.Web.UI.WebControls.DataControlField), DirectCast(parent, System.Web.UI.WebControls.GridView))

        ElseIf TypeOf control Is System.Web.UI.WebControls.ListItem Then
            Return New AdapterWeb_ListItem(DirectCast(control, System.Web.UI.WebControls.ListItem), DirectCast(parent, System.Web.UI.WebControls.WebControl))

        ElseIf TypeOf control Is System.Web.UI.Control Then
            Return New AdapterWeb_UIControl(DirectCast(control, System.Web.UI.Control))

        End If


#If DEBUG Then
        'MsgBox(">NullControlAdapter " + control.GetType.ToString)
#End If
        Return New NullControlAdapter

    End Function

End Class
