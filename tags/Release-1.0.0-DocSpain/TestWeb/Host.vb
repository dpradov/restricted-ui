Imports RestrictedUI
Imports System.ComponentModel

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


Public Class Host
    Implements IHost, INotifyPropertyChanged


    Public Event CambioEstado(ByVal ID As String, ByVal instanceID As String, ByVal nuevoEstado As Integer) Implements RestrictedUI.IHost.StateChanged
    Public Event CambioRoles(ByVal ID As String, ByVal instanceID As String) Implements RestrictedUI.IHost.RolesChanged

    Public Sub ShowError(ByVal [error] As String) Implements IHost.ShowError
        'MessageBox.Show([error])
    End Sub

    ' En esta implementación, el estado podrá ser diferente según el control e instancia que pregunte
    Public Property EstadoActual(ByVal ID As String, ByVal instanceID As String) As Integer Implements RestrictedUI.IHost.State
        Get
            Dim estado As Integer = 0
            _estadoActual.TryGetValue(ID + instanceID, estado)
            Return estado
        End Get
        Set(ByVal value As Integer)
            _estadoActual.Remove(ID + instanceID)
            _estadoActual.Add(ID + instanceID, value)
            NotificarPropertyChanged("EstadoActual")
            RaiseEvent CambioEstado(ID, instanceID, value)
        End Set
    End Property
    Private _estadoActual As New Dictionary(Of String, Integer)

    ' En esta implementación, los roles que devolvamos serán los mismos con independencia del formulario o de la instancia
    Public Property RolesUsuario(ByVal ID As String, ByVal instanceID As String) As Integer() Implements RestrictedUI.IHost.UserRoles
        Get
            Return _rolesUsuario
        End Get
        Set(ByVal value As Integer())
            _rolesUsuario = value
            NotificarPropertyChanged("RolesUsuario")
            NotificarPropertyChanged("RolesUsuarioStr")
            NotificarPropertyChanged("StrRolesUsuario")
            RaiseEvent CambioRoles(ID, instanceID)
        End Set
    End Property
    Private _rolesUsuario As Integer() = New Integer(1) {10, 11}


#Region "Implementación de INotifyPropertyChanged"
    Private Sub NotificarPropertyChanged(ByVal info As String)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(info))
    End Sub

    Public Event PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
#End Region
End Class


''' <summary>
'''  Clase auxiliar con la que interactuar a modo de prueba desde UI
''' </summary>
''' <remarks></remarks>
Public Class Host_Aux
    Implements INotifyPropertyChanged

    Private _host As IHost
    Private _ID, _instanceID As String

    Sub New(ByVal host As IHost, ByVal ID As String, ByVal instanceID As String)
        _host = host
        _ID = ID
        _instanceID = instanceID
        AddHandler _host.RolesChanged, AddressOf OnCambioRoles
        AddHandler _host.StateChanged, AddressOf OnCambioEstado
    End Sub

    Public Property EstadoActual() As Integer
        Get
            Return _host.State(_ID, _instanceID)
        End Get
        Set(ByVal value As Integer)
            _host.State(_ID, _instanceID) = value
            NotificarPropertyChanged("EstadoActual")
        End Set
    End Property

    Public Property StrRolesUsuario() As String
        Get
            Return RolesUsuarioStr(_ID, _instanceID)
        End Get
        Set(ByVal value As String)
            RolesUsuarioStr(_ID, _instanceID) = value
            NotificarPropertyChanged("StrRolesUsuario")
        End Set
    End Property

    ' Propiedad auxiliar con la que interactuar a modo de prueba desde UI (de manera indirecta a través de StrRolesUsuario) 
    ' Leerá/actualizará RolesUsuario, propiedad que escucharán todos los controles
    Public Property RolesUsuarioStr(ByVal ID As String, ByVal instanceID As String) As String
        Get
            Return RestrictedUI.Util.ConvertToString(_host.UserRoles(ID, instanceID))
        End Get
        Set(ByVal value As String)
            If Not String.IsNullOrEmpty(value) Then
                _host.UserRoles(ID, instanceID) = RestrictedUI.Util.ConvertToArrayInt(value)
                NotificarPropertyChanged("RolesUsuarioStr")
            End If
        End Set
    End Property

    ' Como cada instancia estará enlazada a un objeto Host_Aux distinto, no verán los cambios sobre los roles efectuados sobre el objeto Host 
    ' salvo que escuchemos el evento que dispara Host y notifiquemos explícitamente la modificación 
    ' de la propiedad    
    Private Sub OnCambioRoles(ByVal ID As String, ByVal instanceID As String)
        'If (String.IsNullOrEmpty(ID) Or Me._ID = ID) AndAlso (String.IsNullOrEmpty(instanceID) Or Me._instanceID = instanceID) Then
        NotificarPropertyChanged("RolesUsuarioStr")
        NotificarPropertyChanged("StrRolesUsuario")
        'End If
    End Sub

    Private Sub OnCambioEstado(ByVal _ID As String, ByVal _instanceID As String, ByVal nuevoEstado As Integer)
        NotificarPropertyChanged("EstadoActual")
    End Sub


#Region "Implementación de INotifyPropertyChanged"
    Private Sub NotificarPropertyChanged(ByVal info As String)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(info))
    End Sub

    Public Event PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
#End Region
End Class
