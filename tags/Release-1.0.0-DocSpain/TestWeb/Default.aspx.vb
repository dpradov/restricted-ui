Imports System.Reflection

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


Partial Public Class _Default
    Inherits System.Web.UI.Page

    Friend WithEvents ControlSeguridad1 As RestrictedUI.ControlRestrictedUI
    Private components As System.ComponentModel.IContainer
    Friend WithEvents ControlSeguridadWeb1 As RestrictedWebUI.ControlRestrictedUIWeb

    Public Shared _host As New Host

    Private Sub InitializeComponent()
        Me.ControlSeguridadWeb1 = New RestrictedWebUI.ControlRestrictedUIWeb
        CType(Me.ControlSeguridadWeb1, System.ComponentModel.ISupportInitialize).BeginInit()
        '
        'ControlSeguridadWeb1
        '
        Me.ControlSeguridadWeb1.ConfigFile = Nothing
        Me.ControlSeguridadWeb1.ControlsFile = "TestWeb\controls.txt"
        Me.ControlSeguridadWeb1.ID = "TestWeb"
        Me.ControlSeguridadWeb1.InstanceID = "00"
        Me.ControlSeguridadWeb1.ParentControl = Me
        Me.ControlSeguridadWeb1.Paused = False
        Me.ControlSeguridadWeb1.RestrictionsDefinition = New String() {"-0/TextBox1,E/GridView1.ColB,E/Panel1.CheckBoxList1.4,E"}
        CType(Me.ControlSeguridadWeb1, System.ComponentModel.ISupportInitialize).EndInit()

    End Sub

    Protected Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        InitializeComponent()

        SecurityEnvironment.Host = _host

#If DEBUG Then
        ControlSeguridadWeb1.RegisterControls()
#End If

    End Sub

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Button1.Click
        ControlSeguridadWeb1.ShowConfigurationSecurityForm(_host)
    End Sub
End Class