Option Strict On

Imports System.ComponentModel
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
''' Adaptation of <see cref="ControlRestrictedUI "/> component for WinForms applications
''' </summary>
''' <remarks>
''' Drag this component onto the form or user control for which you require to supervise 
''' the visibility and enabled state of its controls based on a security definition. You can 
''' configure that security policy at design time or runtime.
''' </remarks>
<System.Drawing.ToolboxBitmap(GetType(ControlRestrictedUIWinForms))> _
Public Class ControlRestrictedUIWinForms
    Inherits ControlRestrictedUI

    <System.Diagnostics.DebuggerNonUserCode()> _
    Public Sub New()
        MyBase.New()

        SecurityEnvironment.BaseFactory = AdapterWinForms_Factory.getInstance

        ' The Component Designer requires this call.
        InitializeComponent()
    End Sub

    ''' <summary>
    ''' Gets or sets the parent control of the component
    ''' </summary>
    ''' <remarks>
    ''' When you set it at runtime (will be done from the designer of the container) it will make the form or user control attach itself to 
    ''' the event <see cref="Windows.Forms.Control.HandleCreated "/>.
    ''' Thus when the event takes place (by then its children controls will be associated) it will perform the monitoring of children controls selected.
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
                    ' WINFORMS
                    RemoveHandler DirectCast(_parentControl, System.Windows.Forms.Control).HandleCreated, AddressOf InitializeSecurity
                    AddHandler DirectCast(_parentControl, System.Windows.Forms.Control).HandleCreated, AddressOf InitializeSecurity
                End If

            Else
                ' DESIGN TIME
                ' If we are developing (we have entered in design mode) we ensure that the controls file exists, even empty,
                ' which shall be completed at runtime.
                If Not String.IsNullOrEmpty(ControlsFile) AndAlso Not My.Computer.FileSystem.FileExists(ControlsFile) Then
                    Try
                        My.Computer.FileSystem.WriteAllText(ControlsFile, " ", True)
                    Catch ex As Exception   ' In case that the referenced folder does not exists
                    End Try
                End If
            End If
            NotifyPropertyChanged("ParentControl")
        End Set
    End Property

    ''' <summary>
    ''' Indicates that the component is usable in WinForms applications but no in Web applications
    ''' </summary>
    Public Overrides ReadOnly Property WebComponent() As Boolean
        Get
            Return False
        End Get
    End Property

End Class
