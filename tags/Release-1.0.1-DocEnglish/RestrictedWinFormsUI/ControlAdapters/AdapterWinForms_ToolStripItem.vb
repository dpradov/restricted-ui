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
''' Adapter for <see cref=" ToolStripItem"/> controls, not <see cref="ToolStripMenuItem"/> (it is assumed that the latter will have its own adapter),
''' for its use from the Restricted User Interface library (<see cref="RestrictedUI"/>)
''' </summary>
''' <remarks>It is assumed that the factory will check first if the object is <see cref="ToolStripMenuItem"/> before <see cref=" ToolStripItem"/>
''' </remarks>
Public Class AdapterWinForms_ToolStripItem
    Implements IControlAdapter

    Protected _control As ToolStripItem
    Protected _security As ControlRestrictedUI

    Sub New(ByVal control As ToolStripItem)
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
            Dim control As Object = _control
            Dim _exit As Boolean = False

            Do
                If TypeOf control Is ToolStripItem Then
                    cad = DirectCast(control, ToolStripItem).Name + sep + cad
                    sep = "."
                    If DirectCast(control, ToolStripItem).OwnerItem IsNot Nothing Then
                        control = DirectCast(control, ToolStripItem).OwnerItem
                    Else
                        control = DirectCast(control, ToolStripItem).Owner
                    End If
                Else
                    cad = DirectCast(control, Control).Name + sep + cad
                    sep = "."
                    control = DirectCast(control, Control).Parent
                End If

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
        Return New List(Of IControlAdapter)
    End Function

    Protected Overridable Function FindControl(ByVal identifier As String) As IControlAdapter Implements IControlAdapter.FindControl
        Return New NullControlAdapter
    End Function

End Class