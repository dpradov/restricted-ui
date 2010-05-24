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
''' Singleton class that provides a factory of adapters (<see cref="IControlAdapterFactory "/>) for WinForm controls of 
''' NetAdventage (Infragistics): <see cref="Infragistics.Win.UltraWinTree.UltraTree"/> and <see cref="Infragistics.Win.UltraWinGrid.UltraGrid"/> 
''' </summary>
''' <remarks>Other WinForms controls of Infragistics: http://www.infragistics.com/dotnet/netadvantage/winforms.aspx#Overview</remarks>
Public Class AdapterInfragisticsWinForms_Factory
    Implements IControlAdapterFactory

#Region "Singleton"
    Private Shared _instance As AdapterInfragisticsWinForms_Factory

    Private Sub New()
    End Sub

    Public Shared Function GetInstance() As AdapterInfragisticsWinForms_Factory
        If _instance Is Nothing Then
            _instance = New AdapterInfragisticsWinForms_Factory
        End If
        Return _instance
    End Function
#End Region

    Function GetAdapter(ByVal control As Object, Optional ByVal parent As Object = Nothing) As IControlAdapter Implements IControlAdapterFactory.GetAdapter

        If control Is Nothing Then

#If DEBUG Then
            'MsgBox(">NullControlAdapter  (Nothing)")
#End If
            Return New NullControlAdapter
        End If


        ' ULTRAGRID WINFORMS
        '-------------
        If TypeOf control Is Infragistics.Win.UltraWinTree.UltraTree Then
            Return New AdapterInfragisticsWinForms_UltraTree(DirectCast(control, Infragistics.Win.UltraWinTree.UltraTree))

        ElseIf TypeOf control Is Infragistics.Win.UltraWinTree.UltraTreeNode Then
            Return New AdapterInfragisticsWinForms_UltraTreeNode(DirectCast(control, Infragistics.Win.UltraWinTree.UltraTreeNode))

        ElseIf TypeOf control Is Infragistics.Win.UltraWinGrid.UltraGrid Then
            Return New AdapterInfragisticsWinForms_UltraGrid(DirectCast(control, Infragistics.Win.UltraWinGrid.UltraGrid))

        ElseIf TypeOf control Is Infragistics.Win.UltraWinGrid.UltraGridColumn Then
            Return New AdapterInfragisticsWinForms_UltraGridColumn(DirectCast(control, Infragistics.Win.UltraWinGrid.UltraGridColumn))

        End If

        Return New NullControlAdapter

    End Function

End Class
