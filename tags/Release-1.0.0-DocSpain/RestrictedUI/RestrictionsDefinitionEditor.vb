Option Strict On

Imports System.ComponentModel
Imports System.Drawing.Design
Imports System.Windows.Forms.Design
Imports System.Security.Permissions

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


<PermissionSet(SecurityAction.Demand, Name:="FullTrust")> _
Public Class RestrictionsDefinitionEditor
    Inherits UITypeEditor

    Private edSvc As IWindowsFormsEditorService


    Public Overrides Function EditValue(ByVal context As ITypeDescriptorContext, ByVal provider As IServiceProvider, ByVal value As Object) As Object
        If (((Not context Is Nothing) And (Not context.Instance Is Nothing)) And (Not provider Is Nothing)) Then
            Dim domainAux As AppDomain = Nothing
            Dim form As FrmRestrictionsUIDefinition
            Me.edSvc = DirectCast(provider.GetService(GetType(IWindowsFormsEditorService)), IWindowsFormsEditorService)

            If (Not Me.edSvc Is Nothing) Then
                Dim c As ControlRestrictedUI = DirectCast(context.Instance, ControlRestrictedUI)
                Try
                    If SecurityEnvironment.AditionalFactories.Count = 0 Then
                        SecurityEnvironment.LoadFactories(c.ConfigFile, domainAux)
                    End If
                    Dim fichero As String = c.ConfigFile
                    If SecurityEnvironment.AdaptFilePath(fichero, True) Then
                        SecurityEnvironment.LoadConfiguration(SecurityEnvironment.ReadFile(fichero), _
                                          , , SecurityEnvironment.CommonRoles, SecurityEnvironment.CommonStates, _
                                              SecurityEnvironment.ComponentsSecurity, c.ID)
                    End If
                    form = New FrmRestrictionsUIDefinition(c, DirectCast(value, String()), fichero, , True)
                    If Me.edSvc.ShowDialog(form) = DialogResult.OK Then
                        value = form.RestrictionsDefinition
                    End If
                Finally
                    If domainAux IsNot Nothing Then AppDomain.Unload(domainAux)
                End Try

            End If

        End If
        Return value
    End Function

    Public Overrides Function GetEditStyle(ByVal context As ITypeDescriptorContext) As UITypeEditorEditStyle
        If ((Not context Is Nothing) And (Not context.Instance Is Nothing)) Then
            Return UITypeEditorEditStyle.Modal
        End If
        Return MyBase.GetEditStyle(context)
    End Function

End Class

