Option Strict On

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

''' <summary>Special object <see cref="IControlAdapter "/>: null or empty, that wraps no control.</summary>
''' <remarks>
''' It allows to return a valid instance of <see cref="IControlAdapter "/>, not Nothing, thus no requiring the invoking code to 
''' check if the value returned is nothing or not.
''' (According to the patron 'Special Case' as described by Martin Fowler: http://martinfowler.com/eaaCatalog/specialCase.html)
''' </remarks>
Public Class NullControlAdapter
    Implements IControlAdapter

    Sub New()
    End Sub

    ReadOnly Property Control() As Object Implements IControlAdapter.Control
        Get
            Return Nothing
        End Get
    End Property

    ReadOnly Property IsNull() As Boolean Implements IControlAdapter.IsNull
        Get
            Return True
        End Get
    End Property

    Public Sub SuperviseEnabled(ByVal seguridad As ControlRestrictedUI) Implements IControlAdapter.SuperviseEnabled
    End Sub

    Public Sub SuperviseVisible(ByVal seguridad As ControlRestrictedUI) Implements IControlAdapter.SuperviseVisible
    End Sub

    Sub FinalizeSupervision() Implements IControlAdapter.FinalizeSupervision
    End Sub

    Public Function Controls() As IList(Of IControlAdapter) Implements IControlAdapter.Controls
        Return New List(Of IControlAdapter)
    End Function

    Public Property Enabled() As Boolean Implements IControlAdapter.Enabled
        Get
            Return False
        End Get
        Set(ByVal value As Boolean)
        End Set
    End Property

    Public ReadOnly Property Identification(Optional ByVal parent As IControlAdapter = Nothing, Optional ByVal security As ControlRestrictedUI = Nothing) As String Implements IControlAdapter.Identification
        Get
            Return ""
        End Get
    End Property

    Public Function FindControl(ByVal id As String) As IControlAdapter Implements IControlAdapter.FindControl
        Return Me
    End Function

    Public Property Visible() As Boolean Implements IControlAdapter.Visible
        Get
            Return False
        End Get
        Set(ByVal value As Boolean)
        End Set
    End Property
End Class
