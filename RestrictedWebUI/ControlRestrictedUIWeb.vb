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
''' Adaptación del componente <see cref="ControlRestrictedUI "/> para aplicaciones Web
''' </summary>
''' <remarks>Arrastre este componente hacia el formulario o control de usuario sobre el que quiera
''' controlar la visibilidad y el estado de habilitación de sus controles en base a una definición
''' de seguridad. Puede configurar esa seguridad en tiempo de diseño o de ejecución</remarks>
Public Class ControlRestrictedUIWeb
    Inherits ControlRestrictedUI

    <System.Diagnostics.DebuggerNonUserCode()> _
    Public Sub New()
        MyBase.New()

        SecurityEnvironment.BaseFactory = AdapterWeb_Factory.getInstance
    End Sub

    ''' <summary>
    ''' Obtiene o establece el control padre del componente. 
    ''' </summary>
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
                ' TIEMPO DE DISEÑO
                ' Si estamos desarrollando (hemos entrado en modo diseño) nos aseguramos de que 
                ' exista el archivo de controles, aunque vacío, que se rellenará al estar
                ' en ejecución.
                If Not String.IsNullOrEmpty(ControlsFile) AndAlso Not My.Computer.FileSystem.FileExists(ControlsFile) Then
                    Try
                        My.Computer.FileSystem.WriteAllText(ControlsFile, " ", True)   ' Por si no existe la carpeta a la que se hace referencia
                    Catch ex As Exception
                    End Try
                End If
            End If
            NotifyPropertyChanged("ParentControl")
        End Set
    End Property

    ''' <summary>
    ''' Indica que el componente es utilizable en aplicaciones Web y no aplicaciones WinForms
    ''' </summary>
    Public Overrides ReadOnly Property WebComponent() As Boolean
        Get
            Return True
        End Get
    End Property
End Class
