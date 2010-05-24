Imports RestrictedUI
Imports RestrictedWinFormsUI_Infragistics
Imports RestrictedWinFormsUI

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


Public Class MainForm
    Private nextInstanceForm1 As Integer = 0

    Public Shared _host As New Host

    Private Sub btnForm1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnForm1.Click
        Dim f As New Form1

        ' Security to apply may vary not only on the type of form or control (where the security component is embedded),
        ' but on the concrete instance of that form or control.
        ' We will distinguish each instance in the form Form1:
        f._InstanceID = nextInstanceForm1.ToString("00")
        f.Show()
        nextInstanceForm1 += 1
    End Sub

    Private Sub btnForm2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnForm2.Click
        Dim f As New Form2
        f.Show()
    End Sub


    Private Sub FormPpal_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ' Initial setup of the security library:
        '-----------------------------------------

        ' We set the object IHost that will reveal the status and roles of the application
        SecurityEnvironment.Host = _host

        ' We don't want config files to be automatically updated on the initialization of the security components.
        ' We will update them when we have created all the controls (some of them will be build dynamically: datagrid columns)
        SecurityEnvironment.AutomaticUpdateOfControlsFile = False

        ' Because we use Infragistics controls and they are not covered by internal factory included with 
        ' the(library) ControlRestrictedUIWinForms, we will record the factory that manages them       
        SecurityEnvironment.AddFactory(AdapterInfragisticsWinForms_Factory.GetInstance)

        ' Some adapters like these can allow to control the enabled state with the ReadOnly property instead of Enabled
        ' (the latter would be the default)
        AdapterWinForms_DataGridView.UseReadOnly = True
        AdapterInfragisticsWinForms_UltraGrid.UseReadOnly = True

        ' Apart from that we have defined some security policy embedded in a component, we will use the one defined in a file
        ' (We could also have read the security from a stream providing a System.IO.StreamReader, or directly from a string 
        ' with EntornoSeguridad.LoadFromString)
        'SecurityEnvironment.LoadFrom("TestWinForms\Security.txt")
        SecurityEnvironment.LoadFrom("Security.txt")

        ' We may change the combination of keys that activate the security maintenance form.
        ' For example, if we want to set CTR+Shift+F5 we can do:
        '   SecurityEnvironment.HotKey = New HotKey(Keys.F5, Keys.Control Or Keys.Shift, True)    ' Note: The last parameter is optional and defaults to False, that is, the key combination is created disabled
        ' We will maintain for now the default key combination: CTR+Alt+End
        ' As initially the key combination is disabled we will enable it:
        SecurityEnvironment.AllowedHotKey = True

    End Sub
End Class