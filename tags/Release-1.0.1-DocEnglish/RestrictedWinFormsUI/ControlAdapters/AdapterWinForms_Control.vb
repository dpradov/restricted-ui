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
''' Adapter for <see cref="Windows.Forms.Control"/> controls, for its use from the Restricted User Interface library (<see cref="RestrictedUI"/>)
''' It builds on existing Visible and Enabled properties and on have their children accessible through <see cref="Control.Controls "/> .
''' </summary>
''' <remarks></remarks>
Public Class AdapterWinForms_Control
    Implements IControlAdapter

    Protected _control As Control
    Protected _security As ControlRestrictedUI

    Sub New(ByVal control As Control)
        _control = control
    End Sub

    ReadOnly Property Control() As Object Implements IControlAdapter.Control
        Get
            Return _control
        End Get
    End Property

    ReadOnly Property IsNull() As Boolean Implements IControlAdapter.IsNull
        Get
            Return _control Is Nothing
        End Get
    End Property

#Region "Visible and Enabled Management"

    Public Overridable Property Enabled() As Boolean Implements IControlAdapter.Enabled
        Get
            Return _control.Enabled
        End Get
        Set(ByVal value As Boolean)
            _control.Enabled = value
        End Set
    End Property

    Public Property Visible() As Boolean Implements IControlAdapter.Visible
        Get
            Return _control.Visible
        End Get
        Set(ByVal value As Boolean)
            _control.Visible = value
        End Set
    End Property


    Public Overridable Sub SuperviseEnabled(ByVal security As ControlRestrictedUI) Implements IControlAdapter.SuperviseEnabled
        _security = security
        RemoveHandler _control.EnabledChanged, AddressOf control_EnabledChanged
        AddHandler _control.EnabledChanged, AddressOf control_EnabledChanged
    End Sub

    Public Sub SuperviseVisible(ByVal security As ControlRestrictedUI) Implements IControlAdapter.SuperviseVisible
        _security = security
        RemoveHandler _control.VisibleChanged, AddressOf control_VisibleChanged
        AddHandler _control.VisibleChanged, AddressOf control_VisibleChanged
    End Sub

    Protected Sub control_EnabledChanged(ByVal sender As Object, ByVal e As EventArgs)
        _security.VerifyChange(Me, ControlRestrictedUI.TChange.Enabled)
    End Sub
    Protected Sub control_VisibleChanged(ByVal sender As Object, ByVal e As EventArgs)
        _security.VerifyChange(Me, ControlRestrictedUI.TChange.Visible)
    End Sub

    Protected Overridable Sub FinalizeSupervision() Implements IControlAdapter.FinalizeSupervision
        RemoveHandler _control.VisibleChanged, AddressOf control_VisibleChanged
        RemoveHandler _control.EnabledChanged, AddressOf control_EnabledChanged
    End Sub
#End Region


    Public ReadOnly Property Identification(Optional ByVal parent As IControlAdapter = Nothing, Optional ByVal security As ControlRestrictedUI = Nothing) As String Implements IControlAdapter.Identification
        Get
            Dim cad As String = ""
            Dim sep As String = ""
            Dim control As Control = _control
            Dim _exit As Boolean = False

            Do
                cad = control.Name + sep + cad
                sep = "."
                control = control.Parent
            Loop Until control Is Nothing _
                 OrElse (security IsNot Nothing AndAlso control Is security.ParentControl) _
                 OrElse (security Is Nothing AndAlso ( _
                                  TypeOf control Is UserControl _
                           OrElse TypeOf control Is Form _
                           OrElse control.GetType.IsSubclassOf(GetType(Form)) _
                           ) _
                        )
            Return cad
        End Get
    End Property

    Public Overridable Function Controls() As IList(Of IControlAdapter) Implements IControlAdapter.Controls
        Dim children As New List(Of IControlAdapter)
        Dim adapt As IControlAdapter

        For Each ctrl As Control In _control.Controls
            adapt = SecurityEnvironment.GetAdapter(ctrl)
            If Not TypeOf adapt Is NullControlAdapter Then
                children.Add(adapt)
            End If
        Next
        Return children
    End Function

    Protected Overridable Function FindControl(ByVal identifier As String) As IControlAdapter Implements IControlAdapter.FindControl
        Dim pos As Integer = identifier.IndexOf("."c)
        Dim cad As String
        Dim parent As Control = _control

        If pos < 0 Then
            cad = identifier.ToUpper
        Else
            cad = identifier.Substring(0, pos).ToUpper
        End If

        Try
            For Each c As Control In parent.Controls
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
