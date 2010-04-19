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
    Implements IHost

    Public Event StateChanged(ByVal ID As String, ByVal instanceID As String, ByVal newState As Integer) Implements RestrictedUI.IHost.StateChanged
    Public Event RolesChanged(ByVal ID As String, ByVal instanceID As String) Implements RestrictedUI.IHost.RolesChanged

    Public Sub ShowError(ByVal [error] As String) Implements IHost.ShowError
        ' Hay controles que se crean dinámicamente y es normal que no se localicen en el evento HandleCreated (caso de WinForms)
        If [error].Contains("Control no localizado") Then
            If [error].Contains("cControles.") Then
                Exit Sub
            End If
        End If

        MessageBox.Show([error])
    End Sub

    ' En esta implementación, el estado podrá ser diferente según el control e instancia que pregunte
    Public Property State(ByVal ID As String, ByVal instanceID As String) As Integer Implements RestrictedUI.IHost.State
        Get
            Dim nState As Integer = 0
            _State.TryGetValue(ID + instanceID, nState)
            Return nState
        End Get
        Set(ByVal value As Integer)
            _State.Remove(ID + instanceID)
            _State.Add(ID + instanceID, value)
            RaiseEvent StateChanged(ID, instanceID, value)
        End Set
    End Property
    Private _State As New Dictionary(Of String, Integer)

    ' En esta implementación, los roles que devolvamos serán los mismos con independencia del formulario o de la instancia
    Public Property UserRoles(ByVal ID As String, ByVal instanceID As String) As Integer() Implements RestrictedUI.IHost.UserRoles
        Get
            Return _userRoles
        End Get
        Set(ByVal value As Integer())
            _userRoles = value
            RaiseEvent RolesChanged(ID, instanceID)
        End Set
    End Property
    Private _userRoles As Integer() = New Integer(1) {10, 11}

End Class


''' <summary>
''' Clase auxiliar con la que interactuar a modo de prueba desde UI. Permite enlazar controles de usuario
''' que recojan el estado y los roles de la aplicación, asumiendo que éstos se refieren al ID del componente
''' de seguridad y a la instancia concreta que los crea.
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
        AddHandler _host.RolesChanged, AddressOf OnRolesChanged
        AddHandler _host.StateChanged, AddressOf OnStateChanged
    End Sub

    Public Property State() As Integer
        Get
            Return _host.State(_ID, _instanceID)
        End Get
        Set(ByVal value As Integer)
            _host.State(_ID, _instanceID) = value
            NotifyPropertyChanged("State")
        End Set
    End Property

    Public Property StrUserRoles() As String
        Get
            Return UserRolesStr(_ID, _instanceID)
        End Get
        Set(ByVal value As String)
            UserRolesStr(_ID, _instanceID) = value
            NotifyPropertyChanged("StrUserRoles")
        End Set
    End Property

    ''' <summary>
    ''' Propiedad auxiliar con la que interactuar a modo de prueba desde UI (de manera indirecta a través de StrRolesUsuario) 
    ''' Leerá/actualizará RolesUsuario, propiedad que escucharán todos los controles
    ''' </summary>
    Public Property UserRolesStr(ByVal ID As String, ByVal instanceID As String) As String
        Get
            Return RestrictedUI.Util.ConvertToString(_host.UserRoles(ID, instanceID))
        End Get
        Set(ByVal value As String)
            If Not String.IsNullOrEmpty(value) Then
                _host.UserRoles(ID, instanceID) = RestrictedUI.Util.ConvertToArrayInt(value)
                NotifyPropertyChanged("UserRolesStr")
            End If
        End Set
    End Property

    ''' <summary>
    ''' Como cada instancia estará enlazada a un objeto Host_Aux distinto, no verán los cambios sobre los roles efectuados sobre el objeto Host 
    ''' salvo que escuchemos el evento que dispara Host y notifiquemos explícitamente la modificación 
    ''' de la propiedad 
    ''' </summary>
    Private Sub OnRolesChanged(ByVal ID As String, ByVal instanceID As String)
        'If (String.IsNullOrEmpty(ID) Or Me._ID = ID) AndAlso (String.IsNullOrEmpty(instanceID) Or Me._instanceID = instanceID) Then
        NotifyPropertyChanged("UserRolesStr")
        NotifyPropertyChanged("StrUserRoles")
        'End If
    End Sub

    Private Sub OnStateChanged(ByVal _ID As String, ByVal _instanceID As String, ByVal nuevoEstado As Integer)
        NotifyPropertyChanged("State")
    End Sub


#Region "Implementación de INotifyPropertyChanged"
    Private Sub NotifyPropertyChanged(ByVal info As String)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(info))
    End Sub

    Public Event PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
#End Region
End Class
