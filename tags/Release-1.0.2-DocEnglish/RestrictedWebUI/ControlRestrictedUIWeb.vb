Option Strict On

Imports System.ComponentModel
Imports System.Web.UI
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
''' Adaptation of <see cref="ControlRestrictedUI "/> component for Web applications
''' </summary>
''' <remarks>
''' Drag this component onto the form or user control for which you require to supervise 
''' the visibility and enabled state of its controls based on a security definition. You can 
''' configure that security policy at design time or runtime.
''' </remarks>
Public Class ControlRestrictedUIWeb
    Inherits ControlRestrictedUI

    <System.Diagnostics.DebuggerNonUserCode()> _
    Public Sub New()
        MyBase.New()

        SecurityEnvironment.BaseFactory = AdapterWeb_Factory.getInstance
    End Sub

    ''' <summary>
    ''' Gets or sets the parent control of the component
    ''' </summary>
    ''' <remarks>
    ''' When you set it at runtime (will be done from the designer of the container) it will make the form or user control attach itself to 
    ''' the event <see cref="System.Web.UI.Control.PreRender "/>.
    ''' Thus when the event takes place it will perform the monitoring of children controls selected.
    ''' </remarks>
    <Browsable(False)> _
    Public Overrides Property ParentControl() As Object
        Get
            Return _parentControl
        End Get
        Set(ByVal value As Object)
            _parentControl = value

            If Not Me.DesignMode Then
                ' TIEMPO DE EJECUCIÓN
                If Not value Is Nothing Then
                    ' WEB
                    If TypeOf value Is System.Web.UI.UserControl Then
                        _parentControl = value
                    Else
                        For Each c As System.Web.UI.Control In DirectCast(value, System.Web.UI.Control).Controls
                            If TypeOf c Is HtmlControls.HtmlForm Then
                                _parentControl = c
                            End If
                        Next
                    End If
                    AddHandler DirectCast(_parentControl, System.Web.UI.Control).PreRender, AddressOf InitializeSecurity
                End If

            Else
                ' DESIGN TIME
                ' If we are developing (we have entered in design mode) we ensure that the controls file exists, even empty,
                ' which shall be completed at runtime.
                If Not String.IsNullOrEmpty(ControlsFile) AndAlso Not My.Computer.FileSystem.FileExists(ControlsFile) Then
                    Try
                        My.Computer.FileSystem.WriteAllText(ControlsFile, " ", True)   ' In case that the referenced folder does not exists
                    Catch ex As Exception
                    End Try
                End If
            End If
            NotifyPropertyChanged("ParentControl")
        End Set
    End Property

    ''' <summary>
    ''' Indicates that the component is usable in Web applications but no in WinForms applications
    ''' </summary>
    Public Overrides ReadOnly Property WebComponent() As Boolean
        Get
            Return True
        End Get
    End Property
End Class
