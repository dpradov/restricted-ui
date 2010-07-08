Option Strict On
Imports System.ComponentModel
Imports System.Reflection

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



#Region "Structures and classes that contain security policy at environment level"

Public Structure Rol
    Public ID As Integer
    Public [Alias] As String
    Public Name As String
End Structure

Public Structure State
    Public ID As Integer
    Public Name As String
End Structure

Public Structure Group
    Public Name As String
    Public Controls As String()
End Structure

Public Class ComponentSecurity
    Public Roles As Rol()
    Public States As State()
    Public Groups As Group()
    Public Restrictions As String()
End Class

#End Region

#Region "Structure for HotKey management"
''' <summary>
''' Shortcut keys (Hot keys)
''' </summary>
Public Structure HotKey
    Public Sub New(ByVal KeyCode As Keys, ByVal Modifiers As Keys, Optional ByVal EnabledHotKey As Boolean = False)
        Me.KeyCode = KeyCode
        Me.Modifiers = Modifiers
        Enabled = EnabledHotKey
    End Sub
    Public KeyCode As Keys
    Public Modifiers As Keys
    Public Enabled As Boolean
End Structure

#End Region

''' <summary>
''' Singleton class (via Shared) that keeps general aspects to all security scheme.
''' </summary>
''' <remarks></remarks>
Public Class SecurityEnvironment

#Region "Events"
    Public Shared Event ControlAdapterFactoriesChanged()
    Public Shared Event SecurityChanged(ByVal IDControlRestriccionesUI As String)
    Public Shared Event HotKeyChanged()
    Protected Friend Shared Event SecurityChangedWithCancelInMind(ByVal IDControlRestriccionesUI As String, ByVal salvarPrioridad As Boolean, ByVal recuperarPrioridad As Boolean)


    ''' <summary>
    ''' It signals a change in the current state of the application, a change that could affect all security components, 
    ''' only to a specific component, even just a specific instance of one component.
    ''' </summary>
    ''' <remarks>If the parameter <paramref name="ID"/> is Nothing then it should be adressed by all components. It may also be 
    ''' restricted to some instances using <paramref name="instanceID"/>
    ''' </remarks> 
    ''' <param name="ID">Identifier of the security component (referring in turn to a certain form or container)</param> 
    ''' <param name="instanceID">Identifier of the instance of the security component</param> 
    Public Shared Event StateChanged(ByVal ID As String, ByVal instanceID As String, ByVal nuevoEstado As Integer)

    ''' <summary>
    ''' It signals a change on the roles of the application user, a change that could affect all security components, 
    ''' only to a specific component, even just a specific instance of one component.
    ''' </summary>
    ''' <remarks>If the parameter <paramref name="ID"/> is Nothing then it should be adressed by all components. 
    ''' It may also be restricted to some instances using <paramref name="instanceID"/>
    ''' </remarks> 
    ''' <param name="ID">Identifier of the security component (referring in turn to a certain form or container)</param> 
    ''' <param name="instanceID">Identifier of the instance of the security component</param> 
    Public Shared Event RolesChanged(ByVal ID As String, ByVal instanceID As String)

#End Region

    ''' <summary>
    ''' Returns the adapter (<see cref="IControlAdapter"/>) for the indicated control, based on registered factories in 
    ''' the <see cref="SecurityEnvironment"/>.
    ''' </summary>
    ''' <param name="control">The object for which you want to find an adapter</param>
    ''' <param name="parent ">Parent object of that shown in <paramref name="control"/></param>
    ''' <returns>If the factory has no proper adapter for this control, it will return the special adapter 
    ''' <see cref="NullControlAdapter "/></returns>
    ''' <remarks>
    ''' Adapter factories are used ( <see cref="IControlAdapterFactory"/> ) to locate the most suitable control adapter.
    ''' The main security components (ControlRestrictedUIWinForms and ControlRestrictedUIWeb) include their own internal factories
    ''' to recognize the most common controls in their respective environments.
    ''' It is possible to add new factories ( <see cref="IControlAdapterFactory"/> ) to the <see cref="SecurityEnvironment"/>, which 
    ''' will take precedence over internal one.
    ''' <seealso cref="IControlAdapter"/>
    ''' <seealso cref="IControlAdapterFactory"/>
    ''' </remarks>
    Public Shared Function GetAdapter(ByVal control As Object, Optional ByVal parent As Object = Nothing) As IControlAdapter
        Dim adapter As IControlAdapter = Nothing

        For Each factory As IControlAdapterFactory In _AditionalFactories
            adapter = factory.GetAdapter(control, parent)
            If Not adapter.IsNull Then
                Exit For
            End If
        Next

        If adapter Is Nothing OrElse adapter.IsNull Then
            ' Factoría Base corresponderá a Web o WinForms
            If _BaseFactory IsNot Nothing Then
                adapter = _BaseFactory.GetAdapter(control, parent)
            End If
        End If

        If adapter Is Nothing OrElse adapter.IsNull Then
#If DEBUG Then
            'ShowError(">NullControlAdapter " + control.GetType.ToString, padre)
#End If
            Return New NullControlAdapter
        Else
            Return adapter
        End If

    End Function

    ''' <summary>
    ''' Gets or sets the object that implements the interface <see cref="IHost"/> and through which the <see cref="SecurityEnvironment"/> and 
    ''' other security components may interact with the Host application in order to know the current status and roles to be considered.
    ''' </summary>
    Public Shared Property Host() As IHost
        Get
            Return _host
        End Get
        Set(ByVal value As IHost)
            If value IsNot Nothing Then
                _host = value
                AddHandler _host.StateChanged, AddressOf OnStateChanged
                AddHandler _host.RolesChanged, AddressOf OnRolesChanged
            End If
        End Set
    End Property
    Protected Shared _host As IHost

    ''' <summary>
    ''' It sends error o incidence messages generated by the security library to the Application through the object that implements 
    ''' the interface <see cref="IHost"/>
    ''' </summary>
    ''' <remarks>Some of the errors or incidences may be safely ignored by the application if they are related to a not found control
    ''' and we know that this control is created dinamically
    ''' </remarks>
    Protected Friend Shared Sub ShowError(ByVal [error] As String, Optional ByVal ParentControl As Object = Nothing, Optional ByVal IDControlRestrictedUI As String = "")
        Dim cad As String = ""
        If ParentControl IsNot Nothing Then
            cad = cad + "ParentControl=" + ParentControl.ToString
        End If
        If Not String.IsNullOrEmpty(IDControlRestrictedUI) Then
            cad = cad + " IDControlRestriccionesUI=" + IDControlRestrictedUI
        End If
        If cad <> "" Then cad = "[" + cad + "]  "

        cad = cad + [error]
        If _host Is Nothing Then
            MessageBox.Show(cad)
        Else
            _host.ShowError(cad)
        End If
    End Sub


#Region "Adapters Factory Management"

    ''' <summary>
    ''' Gets or sets the adapter factory that will be considered as base
    ''' </summary>
    ''' <remarks>
    ''' The main security components (ControlRestrictedUIWinForms and ControlRestrictedUIWeb) include their own internal factories
    ''' to recognize the most common controls in their respective environments.
    ''' These components indicate the SecurityEnvironment through this property which should be the base factory to use.
    ''' </remarks>
    Public Shared Property BaseFactory() As IControlAdapterFactory
        Get
            Return _BaseFactory
        End Get
        Set(ByVal value As IControlAdapterFactory)
            If value IsNot Nothing Then
                _BaseFactory = value
            End If
        End Set
    End Property
    Protected Shared _BaseFactory As IControlAdapterFactory

    ''' <summary>
    ''' Gets the list of additional factories of control adapters
    ''' </summary>
    ''' <remarks>
    ''' <para>
    ''' Through these additional factories can be managed or interpret specific controls, or even provide alternative 
    ''' adapters for controls contemplated in the factory base (-> plugin)
    ''' As an example of an additional factory, very specific: AdapterInfragisticsWinForms_Factory
    ''' </para>
    ''' <para>
    ''' The additional factories will take precedence over the factory base: 
    ''' When it comes to finding the best control adapter it will start looking for it in the additional ones and 
    ''' finally in the base factory if none is found before
    ''' </para>
    ''' </remarks>
    Protected Friend Shared ReadOnly Property AditionalFactories() As IList(Of IControlAdapterFactory)
        Get
            Return _AditionalFactories
        End Get
    End Property
    Protected Shared _AditionalFactories As IList(Of IControlAdapterFactory) = New List(Of IControlAdapterFactory)

    ''' <summary>
    ''' Adds the specified factory of control adapters. It will be consulted earlier than the internal factory.
    ''' </summary>
    ''' <remarks>If you add several ones, will be asked in the order they were added</remarks> 
    Public Shared Sub AddFactory(ByVal factory As IControlAdapterFactory)
        If Not _AditionalFactories.Contains(factory) Then
            _AditionalFactories.Add(factory)
            RaiseEvent ControlAdapterFactoriesChanged()
            RaiseEvent SecurityChanged(Nothing)
        End If
    End Sub

    ''' <summary>
    ''' Removes the specified factory of control adapters.
    ''' </summary>
    ''' <remarks></remarks> 
    Public Shared Sub RemoveFactory(ByVal factory As IControlAdapterFactory)
        If Not _AditionalFactories.Contains(factory) Then
            _AditionalFactories.Remove(factory)
            RaiseEvent ControlAdapterFactoriesChanged()
            RaiseEvent SecurityChanged(Nothing)
        End If
    End Sub

    ''' <summary>
    ''' Initializes the additional factories of adapters from your settings in the file indicated
    ''' </summary>
    ''' <param name="file "></param>
    ''' <param name="auxDomain "></param>
    ''' <remarks>Usable in design time</remarks>
    Friend Shared Sub LoadFactories(ByVal file As String, ByRef auxDomain As AppDomain)
        If String.IsNullOrEmpty(file) Then Exit Sub

        Dim Factories As List(Of String) = Nothing
        LoadConfiguration(ReadFile(file, True), OptFileConfig.LoadOnlyFactories, Factories)
        SetFactories(Factories, True, auxDomain)
        RaiseEvent SecurityChanged(Nothing)
    End Sub


    ''' <summary>
    ''' Initializes the factories of control adapter through the list of strings where they are described.
    ''' These factories are instantiated and added to the list <see cref="AditionalFactories "/>
    ''' </summary>
    ''' <param name="factories ">List of strings of the form [DLL path], [class full name]
    ''' <example>TestWinForms\bin\Debug\RestrictedWinFormsUI_Infragistics.dll, RestrictedWinFormsUI_Infragistics.AdapterInfragisticsWinForms_Factory</example></param>
    ''' <param name="designTime"></param>
    ''' <param name="domainAux">Application domain (AppDomain) within which these DLL are loaded at design time (only used if <paramref name=" designTime "/> is True</param>
    ''' <remarks></remarks>
    Private Shared Sub SetFactories(ByVal factories As List(Of String), ByVal designTime As Boolean, Optional ByRef domainAux As System.AppDomain = Nothing)
        Dim campos As String()
        Dim factory As IControlAdapterFactory

        AditionalFactories.Clear()
        If factories IsNot Nothing Then
            For Each factoryStr As String In factories
                campos = factoryStr.Split(","c)

                If designTime Then
                    factory = InstanceFactoria(campos(0).Trim, campos(1).Trim, domainAux)
                Else
                    factory = InstanceFactoria(campos(0).Trim, campos(1).Trim)
                End If
                If factory IsNot Nothing AndAlso Not _AditionalFactories.Contains(factory) Then
                    _AditionalFactories.Add(factory)
                End If
            Next
        End If
        RaiseEvent ControlAdapterFactoriesChanged()
        RaiseEvent SecurityChanged(Nothing)
    End Sub

    ''' <summary>
    ''' Instances and returns the factory indicated by the name of the DLL and its full name (to be used at design time)
    ''' </summary>
    ''' <remarks>It should unload the domain used</remarks>
    Private Shared Function InstanceFactoria(ByVal dllFactory As String, ByVal ClassFullName As String, ByRef DomainAux As AppDomain) As IControlAdapterFactory
        Dim factory As IControlAdapterFactory = Nothing
        Dim assembly As Assembly = Nothing
        Dim method As MethodInfo
        Dim cause As String = ""

        Try
            Dim posStartNameDLL As Integer = dllFactory.LastIndexOf("\")
            If Not AdaptFilePath(dllFactory, True) Then
                cause = Constants.ERROR_DLL_NOTFOUND
            Else
                Dim ads As New AppDomainSetup()
                ads.ApplicationBase = dllFactory.Substring(0, posStartNameDLL)
                ads.DisallowBindingRedirects = False
                ads.DisallowCodeDownload = True
                ads.ConfigurationFile = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile

                ' I assign to dllFactoria only the name of the dll, without path and without extension
                dllFactory = dllFactory.Substring(posStartNameDLL + 1, dllFactory.Length - posStartNameDLL - 5)
                DomainAux = AppDomain.CreateDomain("DomainAux", Nothing, ads)
                DomainAux.Load(dllFactory)
                For Each a As Assembly In DomainAux.GetAssemblies
                    If a.FullName.StartsWith(dllFactory) Then
                        assembly = a
                        Exit For
                    End If
                Next
                method = assembly.GetType(ClassFullName).GetMethod("GetInstance")
                factory = DirectCast(method.Invoke(Nothing, New Object() {}), IControlAdapterFactory)
                SecurityEnvironment.AddFactory(factory)
            End If

        Catch ex As Exception
            cause = Constants.ERROR_CLASS_METHOD_NOTFOUND
        End Try

        If factory Is Nothing Then
            SecurityEnvironment.ShowError(Constants.ERROR_INSTANCING_FACTORY + " <" + dllFactory + "," + ClassFullName + "> : " + cause)
        End If

        Return factory
    End Function

    ''' <summary>
    ''' Instances and returns the factory indicated by the name of the DLL and its full name (to be used at runtime)
    ''' </summary>
    Private Shared Function InstanceFactoria(ByVal dllFactory As String, ByVal ClassFullName As String) As IControlAdapterFactory
        Dim factory As IControlAdapterFactory = Nothing
        Dim cause As String = ""

        If Not AdaptFilePath(dllFactory, False) Then
            cause = Constants.ERROR_DLL_NOTFOUND
        Else
            Try
                Dim assembly As Assembly = assembly.LoadFrom(dllFactory)
                Dim method As MethodInfo = assembly.GetType(ClassFullName).GetMethod("GetInstance")
                factory = DirectCast(method.Invoke(Nothing, New Object() {}), IControlAdapterFactory)
                SecurityEnvironment.AddFactory(factory)

            Catch ex As Exception
                cause = Constants.ERROR_CLASS_METHOD_NOTFOUND
            End Try
        End If

        If factory Is Nothing Then
            SecurityEnvironment.ShowError(Constants.ERROR_INSTANCING_FACTORY + " <" + dllFactory + "," + ClassFullName + "> : " + cause)
        End If

        Return factory
    End Function


#End Region

#Region "Definition and access to the Security policy at Environment level"
    ''' <summary>
    ''' Gets or sets the roles defined as common (offered in all security components)
    ''' </summary>
    Public Shared Property CommonRoles() As List(Of Rol)
        Get
            Return _CommonRoles
        End Get
        Set(ByVal value As List(Of Rol))
            _CommonRoles = value
        End Set
    End Property
    Protected Shared _CommonRoles As New List(Of Rol)

    ''' <summary>
    ''' Gets or sets the states defined as common (offered in all security components)
    ''' </summary>
    Public Shared Property CommonStates() As List(Of State)
        Get
            Return _CommonStates
        End Get
        Set(ByVal value As List(Of State))
            _CommonStates = value
        End Set
    End Property
    Protected Shared _CommonStates As New List(Of State)

    ''' <summary>
    ''' It returns the dictionary object which stores the definition of security (policy interface constraints) 
    ''' of the various components, as may have been established from the load of a configuration file or a text string.
    ''' See <see cref="LoadFrom"/> and <see cref="LoadFromString"/>
    ''' </summary>
    ''' <remarks>
    ''' The security herein needs not be equal to the embedded in the various components. 
    ''' The restrictions finally applied in the component depend on one of its properties 
    ''' (<see cref="ControlRestrictedUI.PriorityEmbeddedSecurity "/>). Methods like <see cref="LoadFrom"/> or 
    ''' <see cref="LoadFromString"/>, for example, will establish on the <see cref="SecurityEnvironment"/> the restrictions to 
    ''' apply to each of the components that are referenced in the file or string (even with an empty definition), 
    ''' and set the indicated property so that the components use that restrictions.
    ''' </remarks>
    Public Shared ReadOnly Property ComponentsSecurity() As Dictionary(Of String, ComponentSecurity)
        Get
            Return _componentsSecurity
        End Get
    End Property
    Protected Shared _componentsSecurity As New Dictionary(Of String, ComponentSecurity)

    ''' <summary>
    ''' Incorporates to SecurityEnvironment the corresponding definition for the component indicated with <paramref name="IDControlRestriccionesUI "/>
    ''' </summary>
    ''' <param name="IDControlRestriccionesUI"></param>
    ''' <param name="seg"></param>
    ''' <param name="restorePriority ">Indicates if the priority to the embedded security must be restored</param>
    ''' <remarks> 
    ''' Communicates to the component the change in security with an event that indicates if it should remember the change 
    ''' in case it is needed to be undone later (this method is invoked from the security maintenance form)
    ''' </remarks>
    Protected Friend Shared Sub AddSecurityDefinition(ByVal IDControlRestriccionesUI As String, ByVal seg As ComponentSecurity, ByVal restorePriority As Boolean)
        If String.IsNullOrEmpty(IDControlRestriccionesUI) Then Exit Sub

        If ComponentsSecurity.ContainsKey(IDControlRestriccionesUI) Then
            ComponentsSecurity.Remove(IDControlRestriccionesUI)
        End If
        ComponentsSecurity.Add(IDControlRestriccionesUI, seg)

        RaiseEvent SecurityChangedWithCancelInMind(IDControlRestriccionesUI, False, restorePriority)
    End Sub

#End Region

#Region "Configuration Files/Strings Management"
    ''' <summary>
    ''' Loads the security policy (common roles, common states, components security) from a file, at environment level.
    ''' You can load only the security definition for the component identified by <paramref name=" IDControlRestrictedUI "/>
    ''' </summary>
    ''' <remarks>
    ''' <para>Using the options set out in <paramref name=" opt "/> you can load common elements.></para>
    ''' <para>This method will not load the factories that may be defined in the file. It is recommended to attach the 
    ''' factories at compile time. In any case it is provided an explicit method for this: <see cref="LoadFactories "/>
    ''' </para>
    ''' <para>
    ''' When calling <see cref="LoadFrom "/> or <see cref="LoadFromString "/>, the restrictions defined in the file, text string 
    ''' or stream will take precedence over the ones embedded in the components. If in that text string there is no section 
    ''' relative to a particular component it will continue to apply the security embedded in that component. If you would like 
    ''' to remove all restrictions from a component it would be enough to include an empty paragraph related to that component.
    ''' </para>
    ''' </remarks> 
    Public Shared Sub LoadFrom(ByVal file As String, _
                               Optional ByVal opt As OptFileConfig = OptFileConfig.None, _
                               Optional ByVal IDControlRestrictedUI As String = "")

        LoadConfiguration(ReadFile(file, False), _
                          opt, , _
                          _CommonRoles, _CommonStates, _componentsSecurity, _
                          IDControlRestrictedUI)
        RaiseEvent SecurityChanged(IDControlRestrictedUI)
    End Sub

    ''' <summary>
    ''' Loads the security policy at environment level from a file, in equivalent manner to <see cref=" LoadFrom "/>,
    ''' with the only difference being that this method is intended to allow the security component to undo the changes.
    ''' </summary>
    ''' <remarks>It is used by the security maintenance form (<see cref="FrmRestrictionsUIDefinition "/>)</remarks> 
    Protected Friend Shared Sub LoadFromWithCancelInMind(ByVal file As String, _
                               Optional ByVal opt As OptFileConfig = OptFileConfig.None, _
                               Optional ByVal IDControlRestrictedUI As String = "", Optional ByVal savePriority As Boolean = False, Optional ByVal restorePriority As Boolean = False)

        LoadConfiguration(ReadFile(file, False), _
                          opt, , _
                          _CommonRoles, _CommonStates, _componentsSecurity, _
                          IDControlRestrictedUI)
        RaiseEvent SecurityChangedWithCancelInMind(IDControlRestrictedUI, savePriority, restorePriority)
    End Sub

    ''' <summary>
    ''' Loads the security policy (common roles, common states, components security) from a stream, at environment level.
    ''' You can load only the security definition for the component identified by <paramref name=" IDControlRestrictedUI "/>
    ''' </summary>
    ''' <remarks>
    ''' <para>Using the options set out in <paramref name=" opt "/> you can load common elements</para>
    ''' <para>This method will not load the factories that may be defined in the stream. It is recommended to attach the 
    ''' factories at compile time. In any case it is provided an explicit method for this: <see cref="LoadFactories "/>
    ''' </para>
    ''' <para>
    ''' When calling <see cref="LoadFrom "/> or <see cref="LoadFromString "/>, the restrictions defined in the file, text string 
    ''' or stream will take precedence over the ones embedded in the components. If in that text string there is no section 
    ''' relative to a particular component it will continue to apply the security embedded in that component. If you would like 
    ''' to remove all restrictions from a component it would be enough to include an empty paragraph related to that component.
    ''' </para>
    ''' </remarks> 
    Public Shared Sub LoadFrom(ByVal stream As IO.StreamReader, _
                                     Optional ByVal opt As OptFileConfig = OptFileConfig.None, _
                                     Optional ByVal IDControlRestrictedUI As String = "")

        LoadConfiguration(stream.ReadToEnd, opt, , _
                                 _CommonRoles, _CommonStates, _componentsSecurity, _
                                 IDControlRestrictedUI)
        RaiseEvent SecurityChanged(IDControlRestrictedUI)
    End Sub

    ''' <summary>
    ''' Loads the security policy (common roles, common states, components security) from a string, at environment level, 
    ''' in equivalent manner to <see  cref=" LoadFrom "/>
    ''' You can load only the security definition for the component identified by <paramref name=" IDControlRestrictedUI "/>
    ''' </summary>
    ''' <remarks>
    ''' When calling <see cref="LoadFrom "/> or <see cref="LoadFromString "/>, the restrictions defined in the file, text string 
    ''' or stream will take precedence over the ones embedded in the components. If in that text string there is no section 
    ''' relative to a particular component it will continue to apply the security embedded in that component. If you would like 
    ''' to remove all restrictions from a component it would be enough to include an empty paragraph related to that component.
    ''' </remarks>
    Public Shared Sub LoadFromString(ByVal TxtConfiguration As String, _
                                     Optional ByVal opt As OptFileConfig = OptFileConfig.None, _
                                     Optional ByVal IDControlRestrictedUI As String = "")

        LoadConfiguration(TxtConfiguration, opt, , _
                                 _CommonRoles, _CommonStates, _componentsSecurity, _
                                 IDControlRestrictedUI)
        RaiseEvent SecurityChanged(IDControlRestrictedUI)
    End Sub


    ''' <summary>
    ''' Returns the contents of the text file provided, whose path will be adjusted as needed access at design time or runtime
    ''' </summary>
    Protected Friend Shared Function ReadFile(ByVal file As String, ByVal designTime As Boolean) As String
        Dim result As String = ""
        If file Is Nothing Then file = ConfigFile
        If AdaptFilePath(file, designTime) Then
            Dim sr As System.IO.StreamReader = New System.IO.StreamReader(file, System.Text.Encoding.Default)
            result = sr.ReadToEnd
            sr.Close()
        End If
        Return result
    End Function

    ''' <summary>
    ''' Returns the contents of the text file provided
    ''' </summary>
    Protected Friend Shared Function ReadFile(ByVal file As String) As String
        Dim result As String = ""
        If file Is Nothing Then file = ConfigFile
        If My.Computer.FileSystem.FileExists(file) Then
            Dim sr As System.IO.StreamReader = New System.IO.StreamReader(file, System.Text.Encoding.Default)
            result = sr.ReadToEnd
            sr.Close()
        End If
        Return result
    End Function



    Private Enum ConfigurationBlocks
        Factories = 0
        CommonRoles
        CommonStates
        CONTROLSECURITY
        Groups
        Authorizations
        Roles
        States
    End Enum

    ''' <summary>
    ''' Enumeration (set of flags) offering the options to use on the methods of reading the configuration file
    ''' </summary>
    <Flags()> _
    Public Enum OptFileConfig As Short
        None = 0
        LoadOnlyFactories = 2
        LoadOnlyCommonElements = 4
        AbortOnError = 8
    End Enum

    ''' <summary>
    ''' Loads the supplied variables Factories, CommonRoles, CommonStates, ComponentsSecurity with the security definition 
    ''' established in the text string <paramref name=" TxtConfiguracion "/>
    ''' You can load only the security definition for the component identified by <paramref name=" IDControlRestrictedUI "/>
    ''' </summary>
    ''' <remarks>
    ''' Using the options set out in <paramref name=" opt "/> you can load only the factories or only the common elements
    ''' </remarks> 
    Protected Friend Shared Sub LoadConfiguration(ByVal TxtConfiguration As String, _
                                         Optional ByVal opt As OptFileConfig = OptFileConfig.None, _
                                         Optional ByRef Factories As List(Of String) = Nothing, _
                                         Optional ByRef CommonRoles As List(Of Rol) = Nothing, _
                                         Optional ByRef CommonStates As List(Of State) = Nothing, _
                                         Optional ByRef ComponentsSecurity As Dictionary(Of String, ComponentSecurity) = Nothing, _
                                         Optional ByVal IDControlRestrictedUI As String = "")

        If Factories Is Nothing Then Factories = New List(Of String)
        If CommonRoles Is Nothing Then CommonRoles = New List(Of Rol)
        If CommonStates Is Nothing Then CommonStates = New List(Of State)
        If ComponentsSecurity Is Nothing Then ComponentsSecurity = New Dictionary(Of String, ComponentSecurity)
        Factories.Clear()
        CommonRoles.Clear()
        CommonStates.Clear()

        If TxtConfiguration Is Nothing Then Exit Sub

        If (opt And OptFileConfig.LoadOnlyCommonElements) <> 0 Then
            IDControlRestrictedUI = "%/%"
        End If

        Dim actualBlock, previousBlock As ConfigurationBlocks
        Dim fields As String()
        Dim _IDControlRestrictedUI As String = ""
        Dim _IDControlRestrictedUIPrevious As String = ""
        Dim securityComp As New ComponentSecurity
        Dim lCad As New List(Of String)
        Dim pos As Integer

        Dim lGroups As New List(Of Group)
        Dim lRoles As New List(Of Rol)
        Dim lStates As New List(Of State)


        Try
            Dim reader As New System.IO.StringReader(TxtConfiguration)


            Try
                Dim line As String
                previousBlock = Nothing

                Do
                    line = reader.ReadLine
                    If line Is Nothing Then Exit Do
                    line = line.Trim
                    If line = "" OrElse line(0) = ";"c Then Continue Do

                    ' Identify when entering a new block
                    '===========================
                    previousBlock = actualBlock
                    _IDControlRestrictedUIPrevious = _IDControlRestrictedUI

                    If line(0) = "["c Then
                        Select Case line.ToUpper
                            Case "[FACTORIES]"
                                actualBlock = ConfigurationBlocks.Factories
                                _IDControlRestrictedUI = ""
                            Case "[COMMONROLES]"
                                actualBlock = ConfigurationBlocks.CommonRoles
                                _IDControlRestrictedUI = ""
                            Case "[COMMONSTATES]"
                                actualBlock = ConfigurationBlocks.CommonStates
                                _IDControlRestrictedUI = ""
                            Case "[GROUPS]"
                                actualBlock = ConfigurationBlocks.Groups
                            Case "[RESTRICTIONS]"
                                actualBlock = ConfigurationBlocks.Authorizations
                            Case "[ROLES]"
                                actualBlock = ConfigurationBlocks.Roles
                            Case "[STATES]"
                                actualBlock = ConfigurationBlocks.States
                            Case Else
                                If line.Substring(0, 17) = "[SECURITYCONTROL=" Then
                                    actualBlock = ConfigurationBlocks.CONTROLSECURITY
                                    _IDControlRestrictedUI = line.Substring(17, line.Length - 18)
                                End If
                        End Select

                        ' If you change of block, take the necessary action in each case
                        '===========================
                        If actualBlock <> previousBlock Then
                            Select Case previousBlock
                                Case ConfigurationBlocks.Authorizations
                                    securityComp.Restrictions = lCad.ToArray
                                    lCad.Clear()
                                Case ConfigurationBlocks.States
                                    securityComp.States = lStates.ToArray
                                    lStates.Clear()
                                Case ConfigurationBlocks.Roles
                                    securityComp.Roles = lRoles.ToArray
                                    lRoles.Clear()
                                Case ConfigurationBlocks.Groups
                                    securityComp.Groups = lGroups.ToArray
                                    lGroups.Clear()
                            End Select
                            ' If you exit the set of blocks specific to a form's security, the ID will change
                            If _IDControlRestrictedUIPrevious <> "" And _IDControlRestrictedUI <> _IDControlRestrictedUIPrevious Then
                                ' If given a security component's ID, it only will read data of that security component
                                If String.IsNullOrEmpty(IDControlRestrictedUI) Or _IDControlRestrictedUIPrevious = IDControlRestrictedUI Then
                                    ComponentsSecurity.Remove(_IDControlRestrictedUIPrevious)
                                    ComponentsSecurity.Add(_IDControlRestrictedUIPrevious, securityComp)
                                    securityComp = New ComponentSecurity  ' to the following
                                End If
                            End If
                        End If

                        Continue Do
                    End If



                    ' Treat each block
                    '===========================
                    ' If you only have to load factories ..
                    If ((opt And OptFileConfig.LoadOnlyFactories) <> 0) And actualBlock <> ConfigurationBlocks.Factories Then
                        Continue Do
                    End If
                    ' If we only have to load the security component whose ID is the provided as parameter..
                    If (Not String.IsNullOrEmpty(IDControlRestrictedUI) And _IDControlRestrictedUI <> "") And _IDControlRestrictedUI <> IDControlRestrictedUI Then
                        Continue Do
                    End If

                    Select Case actualBlock
                        Case ConfigurationBlocks.Factories
                            Factories.Add(line)

                        Case ConfigurationBlocks.CommonRoles, ConfigurationBlocks.Roles
                            Dim rol As New Rol
                            fields = line.Split(","c)
                            If Not Integer.TryParse(fields(0), rol.ID) Then
                                MessageBox.Show(Constants.ERROR_INCORRECT_ROL_ID + fields(0))
                                Exit Sub
                            End If
                            rol.Name = fields(1).Trim
                            If fields.Length > 2 Then rol.Alias = fields(2).Trim
                            If actualBlock = ConfigurationBlocks.CommonRoles Then
                                CommonRoles.Add(rol)
                            Else
                                lRoles.Add(rol)
                            End If


                        Case ConfigurationBlocks.CommonStates, ConfigurationBlocks.States
                            Dim estado As New State
                            fields = line.Split(","c)
                            If Not Integer.TryParse(fields(0), estado.ID) Then
                                MessageBox.Show(Constants.ERROR_INCORRECT_STATE_ID + fields(0))
                                Exit Sub
                            End If
                            estado.Name = fields(1).Trim

                            If actualBlock = ConfigurationBlocks.CommonStates Then
                                CommonStates.Add(estado)
                            Else
                                lStates.Add(estado)
                            End If

                        Case ConfigurationBlocks.Authorizations
                            lCad.Add(line)

                        Case ConfigurationBlocks.Groups
                            Dim grupo As New Group
                            pos = line.IndexOf("="c)
                            grupo.Name = line.Substring(0, pos).Trim
                            grupo.Controls = line.Substring(pos + 1).Replace(" ", "").Split(","c)
                            lGroups.Add(grupo)
                    End Select

                Loop

                ' Addressing the closing of the last block
                '===========================
                Select Case actualBlock
                    Case ConfigurationBlocks.Authorizations
                        securityComp.Restrictions = lCad.ToArray
                    Case ConfigurationBlocks.States
                        securityComp.States = lStates.ToArray
                    Case ConfigurationBlocks.Roles
                        securityComp.Roles = lRoles.ToArray
                    Case ConfigurationBlocks.Groups
                        securityComp.Groups = lGroups.ToArray
                End Select
                If _IDControlRestrictedUI <> "" And (String.IsNullOrEmpty(IDControlRestrictedUI) Or _IDControlRestrictedUI = IDControlRestrictedUI) Then
                    ComponentsSecurity.Remove(_IDControlRestrictedUI)
                    ComponentsSecurity.Add(_IDControlRestrictedUI, securityComp)
                End If
            Finally
                reader.Close()
            End Try

        Catch ex As Exception
            Dim cad As String = Constants.ERROR_PROCESSING_CONFIGFILE + ex.Message
            ShowError(cad, Nothing)
            MsgBox(cad, MsgBoxStyle.Exclamation)
            If (opt And OptFileConfig.AbortOnError) <> 0 Then
                MsgBox(Constants.ERROR_EXITING_APPLICATION, MsgBoxStyle.Critical)
                Application.Exit()
            End If
        End Try

    End Sub



    ''' <summary>
    ''' It lets you save in the specified file all the security definition established at the environment level (<see cref=" SecurityEnvironment" />)
    ''' (updating the security previously contained in that file), together with the corresponding to a security component.
    ''' Saving a specific component is optional, as it is to do along with all security at environment level.
    ''' You can force common roles and common states that you want to send to the file.
    ''' </summary>
    ''' <remarks>
    ''' <para>
    ''' Security settings for those components that were in the file and do not appear in the security environment nor correspond to the explicit <paramref name=" IDControlRestrictedUI "/>, will not be affected.
    ''' It will remain as it was in the file (although you will lose the comments that may have been incorporated)
    ''' </para>
    ''' <para>Note: the file path must already be adapted according to this method is invoked at design time or runtime</para>
    ''' </remarks> 
    Public Shared Sub SaveConfiguration(ByVal file As String, _
                                          ByVal includeAllKnownSecurity As Boolean, _
                                          Optional ByVal IDControlRestrictedUI As String = "", _
                                          Optional ByVal securityComp As ComponentSecurity = Nothing, _
                                          Optional ByVal CommonRoles As List(Of Rol) = Nothing, _
                                          Optional ByVal CommonStates As List(Of State) = Nothing, _
                                          Optional ByVal opt As OptFileConfig = OptFileConfig.None _
                                          )

        Dim CommonRolesAux As List(Of Rol) = Nothing
        Dim CommonStatesAux As List(Of State) = Nothing
        Dim FactoriesAux As List(Of String) = Nothing
        Dim ComponentsSecurityAux As Dictionary(Of String, ComponentSecurity) = Nothing


        LoadConfiguration(ReadFile(file), opt, _
                          FactoriesAux, CommonRolesAux, CommonStatesAux, ComponentsSecurityAux)

        ' Now we modify what we have read with the data provided as parameter, to be saved.
        ' The factories included in the file will not change
        ' Will use as common roles and common states the provided ones
        ' As restricted to ControlRestriccionesUI or not, will be replaced
        If includeAllKnownSecurity Then
            For Each s As String In SecurityEnvironment.ComponentsSecurity.Keys
                ComponentsSecurityAux.Remove(s)
                ComponentsSecurityAux.Add(s, SecurityEnvironment.ComponentsSecurity(s))
            Next
        End If
        If securityComp IsNot Nothing Then
            ComponentsSecurityAux.Remove(IDControlRestrictedUI)
            ComponentsSecurityAux.Add(IDControlRestrictedUI, securityComp)
        End If

        If CommonRoles IsNot Nothing Then CommonRolesAux = CommonRoles
        If CommonStates IsNot Nothing Then CommonStatesAux = CommonStates

        Dim securityStr As String
        securityStr = SecurityToString(FactoriesAux, CommonRolesAux, CommonStatesAux, ComponentsSecurityAux)

        IO.File.WriteAllText(file, securityStr, System.Text.Encoding.Default)
    End Sub

    ''' <summary>
    ''' Serializes the security definition established at environment level (including the relation of additional factories) to 
    ''' a string with the format used in the configuration file.
    ''' </summary>
    Public Shared Function SecurityToString(ByVal Factories As List(Of String), _
                                             ByVal CommonRoles As List(Of Rol), _
                                             ByVal CommonStates As List(Of State), _
                                             ByVal ComponentsSecurity As Dictionary(Of String, ComponentSecurity) _
                                             ) As String

        Dim sw As New System.IO.StringWriter
        Dim result As String = ""
        Try
            sw.WriteLine("[Factories]")
            sw.WriteLine("; Relative paths will be expressed in relation to the folder containing the solution (. sln). This path will be used to locate the DLL in design time.")
            sw.WriteLine("; We will assume that the DLL is in the same folder as the executable, so in runtime the path will be ignored and used only the name of the file")
            sw.WriteLine("; Note: You may also use absolute paths.")
            If Factories IsNot Nothing Then
                For Each factoria As String In Factories
                    sw.WriteLine(factoria)
                Next
            End If
            sw.WriteLine()

            sw.WriteLine("[CommonRoles]")
            If CommonRoles IsNot Nothing Then
                For Each rol As Rol In CommonRoles
                    sw.WriteLine(rol.ID.ToString + "," + rol.Name + "," + rol.Alias)
                Next
                sw.WriteLine()
            End If

            sw.WriteLine("[CommonStates]")
            If CommonStates IsNot Nothing Then
                For Each e As State In CommonStates
                    sw.WriteLine(e.ID.ToString + "," + e.Name)
                Next
            End If

            For Each s As String In ComponentsSecurity.Keys
                Dim sf As ComponentSecurity = ComponentsSecurity.Item(s)
                sw.WriteLine()
                sw.WriteLine(";=======================================================")
                sw.WriteLine("[SECURITYCONTROL=" + s + "]")
                If sf.Roles IsNot Nothing Then
                    sw.WriteLine("[Roles]")
                    For Each rol As Rol In sf.Roles
                        sw.WriteLine(rol.ID.ToString + "," + rol.Name + "," + rol.Alias)
                    Next
                    sw.WriteLine()
                End If
                If sf.States IsNot Nothing Then
                    sw.WriteLine("[States]")
                    For Each e As State In sf.States
                        sw.WriteLine(e.ID.ToString + "," + e.Name)
                    Next
                    sw.WriteLine()
                End If
                If sf.Groups IsNot Nothing Then
                    sw.WriteLine("[Groups]")
                    For Each g As Group In sf.Groups
                        sw.WriteLine(g.Name + "= " + Util.ConvertToString(g.Controls))
                    Next
                    sw.WriteLine()
                End If
                If sf.Restrictions IsNot Nothing Then
                    sw.WriteLine("[Restrictions]")
                    For Each p As String In sf.Restrictions
                        sw.WriteLine(p)
                    Next
                End If

            Next
            result = sw.ToString

        Finally
            sw.Close()
        End Try

        Return result

    End Function



    ''' <summary>
    ''' It changes the path given in <paramref name="file"/> so that the corresponding file can be located at design time or 
    ''' runtime, as indicated in <paramref name="designTime"/>.
    ''' </summary>
    ''' <param name="file ">Path to the file</param>
    ''' <param name="designTime">Indicates if you want to adapt the path to be used at design time</param>
    ''' <returns>Returns True if file exists</returns>
    ''' <remarks>
    ''' The configuration file, the controls file and the DLL of factories are expressed with paths relative to the folder 
    ''' containing the solution (. sln). This path will be used to locate the DLL in design time.
    ''' We will assume that the DLL is in the same folder as the executable, so in runtime the path will be 
    ''' ignored and used only the name of the file
    ''' Note: You may also use absolute paths.
    ''' </remarks>
    Public Shared Function AdaptFilePath(ByRef file As String, ByVal designTime As Boolean) As Boolean
        ' If the path leads to the file (it's absolute or doesn't require any adaptation) we return it, without more ado
        If String.IsNullOrEmpty(file) Then Return False

        Try
            If Not My.Computer.FileSystem.FileExists(file) Then
                If Not designTime Then
                    Dim fileName As String = My.Computer.FileSystem.GetName(file)
                    Dim ext As String
                    If fileName.LastIndexOf(".") < 0 Then
                        ext = ""
                    Else
                        ext = fileName.Substring(fileName.LastIndexOf(".") + 1).ToUpper
                    End If

                    If ext = "DLL" Then
                        file = fileName
                    Else
                        Dim filePath As String = My.Computer.FileSystem.GetParentPath(file)
                        file = Application.StartupPath.Substring(0, Application.StartupPath.ToUpper.IndexOf(filePath.ToUpper) + Len(filePath)) + "\" + fileName
                    End If
                End If
            End If
            Return My.Computer.FileSystem.FileExists(file)

        Catch ex As Exception
            Return False
        End Try
    End Function

#End Region

#Region "HotKey Management"

    ''' <summary>
    ''' Gets or sets the keys combination that allows to open the security maintenance form (<see cref="FrmRestrictionsUIDefinition "/>)
    ''' (Initially offered as combination: CTR+Alt+End, but disabled)
    ''' </summary>
    ''' <remarks>
    ''' Using HotKey is designed for easy configuration of security at runtime, but in phases of development and testing of applications
    ''' <seealso cref="AllowedHotKey "/>
    ''' </remarks>
    Public Shared Property HotKey() As HotKey
        Get
            Return _hotKey
        End Get
        Set(ByVal value As HotKey)
            _hotKey = value
            RaiseEvent HotKeyChanged()
        End Set
    End Property
    Private Shared _hotKey As New HotKey(Keys.End, Keys.Control Or Keys.Alt, False)

    ''' <summary>
    ''' Boolean value that enables or disables the keys combination to open the security maintenance form (<see cref="FrmRestrictionsUIDefinition "/>)
    ''' <seealso cref="HotKey "/>
    ''' </summary>
    ''' <remarks>Initially will be disabled</remarks>
    Public Shared Property AllowedHotKey() As Boolean
        Get
            Return HotKey.Enabled
        End Get
        Set(ByVal value As Boolean)
            If _hotKey.Enabled <> value Then
                _hotKey.Enabled = value
                RaiseEvent HotKeyChanged()
            End If
        End Set
    End Property

#End Region


#Region "Utilities for processing roles"

    Protected Friend Shared CommonRolesAUX As List(Of Rol)
    Protected Friend Shared ParticularRolesAUX As Rol()


    ''' <summary>
    ''' Returns a string representation of the roles provided in the array <paramref name="roles"/>.
    ''' Each role of entry will be displayed with its alias if available and if not with its ID. 
    ''' These roles will be separated in the output string by commas.
    ''' </summary>
    ''' <param name="IDControlRestrictedUI ">Identifier of the security component for which roles are analyzed 
    ''' (necessary since each component can have particular roles, own)</param>
    ''' <remarks>
    ''' To obtain an alias from its ID it will be used the dictionaries of common and specific roles in the environment, 
    ''' unless you have loaded some auxiliary roles in <see cref="CommonRolesAUX"/> and <see cref="ParticularRolesAUX"/>,
    ''' in which case the latter will be used.
    ''' </remarks>
    Public Shared Function RolesToStrUsingAlias(ByVal roles As Integer(), ByVal IDControlRestrictedUI As String, _
                                                          ByVal particularRoles As Rol(), ByVal commonRoles As List(Of Rol) _
                                                          ) As String
        Try
            SecurityEnvironment.CommonRolesAUX = commonRoles
            SecurityEnvironment.ParticularRolesAUX = particularRoles
            Return RolesToStrUsingAlias(roles, IDControlRestrictedUI)
        Finally
            SecurityEnvironment.CommonRolesAUX = Nothing
            SecurityEnvironment.ParticularRolesAUX = Nothing
        End Try
    End Function

    ''' <summary>
    ''' Returns a string representation of the roles provided in the array <paramref name="roles"/>.
    ''' Each role of entry will be displayed with its alias if available and if not with its ID. 
    ''' These roles will be separated in the output string by commas.
    ''' </summary>
    ''' <param name="IDControlRestrictedUI ">Identifier of the security component for which roles are analyzed 
    ''' (necessary since each component can have particular roles, own)</param>
    ''' <remarks>
    ''' To obtain an alias from its ID it will be used the dictionaries of common and specific roles in the environment, 
    ''' unless you have loaded some auxiliary roles in <see cref="CommonRolesAUX"/> and <see cref="ParticularRolesAUX"/>,
    ''' in which case the latter will be used.
    ''' </remarks>
    Protected Friend Shared Function RolesToStrUsingAlias(ByVal roles As Integer(), ByVal IDControlRestrictedUI As String _
                           ) As String
        Dim sec As ComponentSecurity = Nothing
        Dim cadRoles As String = ""
        Dim sep As String = ""
        Dim particularRoles As Rol() = ParticularRolesAUX
        Dim commonRoles As List(Of Rol) = CommonRolesAUX

        If particularRoles Is Nothing Then
            If SecurityEnvironment.ComponentsSecurity.TryGetValue(IDControlRestrictedUI, sec) Then
                particularRoles = sec.Roles
            End If
        End If
        If commonRoles Is Nothing Then
            commonRoles = SecurityEnvironment.CommonRoles
        End If

        If particularRoles Is Nothing AndAlso SecurityEnvironment.CommonRoles.Count = 0 Then
            Return Util.ConvertToString(roles)
        Else
            If roles IsNot Nothing Then
                For Each r As Integer In roles
                    cadRoles += sep + RolToStrTryingAlias(r, particularRoles, commonRoles)
                    sep = ", "
                Next
            End If
            Return cadRoles
        End If
    End Function


    ''' <summary>
    ''' Returns a string representation of the role provided through its ID in <paramref name="rolID"/>. 
    ''' This role will be displayed with its alias if available and if not with its ID. 
    ''' To obtain the alias from its ID it will be used the dictionaries of common and specific roles provided as parameter.
    ''' </summary>
    Public Shared Function RolToStrTryingAlias(ByVal rolID As Integer, ByVal particularRoles As Rol(), ByVal commonRoles As IList(Of Rol)) As String
        If particularRoles IsNot Nothing Then
            For Each Rol As Rol In particularRoles
                If Rol.ID = rolID Then Return Rol.Alias
            Next
        End If

        If commonRoles IsNot Nothing Then
            For Each Rol As Rol In commonRoles
                If Rol.ID = rolID Then Return Rol.Alias
            Next
        End If

        Return rolID.ToString
    End Function

    ''' <summary>
    ''' Returns the ID associated with the alias of role given in <paramref name="aliasRol"/>.
    ''' To make the translation it will be used as dictionaries the definition of common and specific roles provided as parameter.
    ''' </summary>
    ''' <remarks>If the alias is not recognized returns 999 999</remarks>
    Public Shared Function AliasToRolID(ByVal aliasRol As String, ByVal particularRoles As Rol(), ByVal commonRoles As IList(Of Rol)) As Integer
        If particularRoles IsNot Nothing Then
            For Each Rol As Rol In particularRoles
                If Rol.Alias.ToUpper = aliasRol.ToUpper Then Return Rol.ID
            Next
        End If

        If commonRoles IsNot Nothing Then
            For Each Rol As Rol In commonRoles
                If Rol.Alias.ToUpper = aliasRol.ToUpper Then Return Rol.ID
            Next
        End If

        ShowError(Constants.ERROR_ROL_NOTRECOGNIZED + aliasRol, Nothing)
        Return 999999
    End Function


    ''' <summary>
    ''' Returns an array of integers with the ID associated with the roles included in the string supplied in <paramref name=" rolesWithAlias "/>,
    ''' string in which can appear both IDs and alias.
    ''' </summary>
    ''' <param name="IDControlRestrictedUI ">Identifier of the security component for which roles are analyzed 
    ''' (necessary since each component can have particular roles, own)</param>
    ''' <remarks>
    ''' To translate an alias to its ID it will be used the dictionaries of common and specific roles in the environment, 
    ''' unless you have loaded some auxiliary roles in <see cref="CommonRolesAUX"/> and <see cref="ParticularRolesAUX"/>,
    ''' in which case the latter will be used.
    ''' </remarks>
    Public Shared Function GetRolesID(ByVal rolesWithAlias As String, ByVal IDControlRestrictedUI As String, ByVal particularRoles As Rol(), ByVal commonRoles As List(Of Rol)) As Integer()
        Dim sec As ComponentSecurity = Nothing
        Try
            SecurityEnvironment.CommonRolesAUX = commonRoles
            SecurityEnvironment.ParticularRolesAUX = particularRoles
            Return GetRolesID(rolesWithAlias, IDControlRestrictedUI)
        Finally
            SecurityEnvironment.CommonRolesAUX = Nothing
            SecurityEnvironment.ParticularRolesAUX = Nothing
        End Try
    End Function


    ''' <summary>
    ''' Returns an array of integers with the ID associated with the roles included in the string supplied in <paramref name=" rolesWithAlias "/>,
    ''' string in which can appear both IDs and alias.
    ''' </summary>
    ''' <param name="IDControlRestrictedUI ">Identifier of the security component for which roles are analyzed 
    ''' (necessary since each component can have particular roles, own)</param>
    ''' <remarks>
    ''' To translate an alias to its ID it will be used the dictionaries of common and specific roles in the environment, 
    ''' unless you have loaded some auxiliary roles in <see cref="CommonRolesAUX"/> and <see cref="ParticularRolesAUX"/>,
    ''' in which case the latter will be used.
    ''' </remarks>
    Public Shared Function GetRolesID(ByVal rolesWithAlias As String, ByVal IDControlRestrictedUI As String) As Integer()
        Dim sec As ComponentSecurity = Nothing
        Dim particularRoles As Rol() = ParticularRolesAUX
        Dim commonRoles As List(Of Rol) = CommonRolesAUX

        If particularRoles Is Nothing Then
            If SecurityEnvironment.ComponentsSecurity.TryGetValue(IDControlRestrictedUI, sec) Then
                particularRoles = sec.Roles
            End If
        End If
        If commonRoles Is Nothing Then
            commonRoles = SecurityEnvironment.CommonRoles
        End If

        Dim rolesStr As String() = Util.ConvierteEnArrayStr(rolesWithAlias)
        Dim rolesID(rolesStr.Length - 1) As Integer

        For i As Integer = 0 To rolesStr.Length - 1
            If Not Integer.TryParse(rolesStr(i), rolesID(i)) Then
                rolesID(i) = AliasToRolID(rolesStr(i), particularRoles, commonRoles)
            End If
        Next

        Return rolesID

    End Function

#End Region

#Region "Utilities for processing restrictions and groups"

    ''' <summary>
    ''' Separates the restrictions definition provided in proper restrictions and groups
    ''' </summary>
    Public Shared Sub GetRestrictionsAndGroups(ByVal RestrictionsDefinition As String(), ByRef Restrictions As String(), ByRef Groups As Group())
        Dim cad As String

        If RestrictionsDefinition Is Nothing Then
            Restrictions = Nothing
            Groups = Nothing
            Exit Sub
        End If

        Dim RestrictionsList As New List(Of String)
        Dim lGroups As New List(Of Group)

        For Each cad In RestrictionsDefinition
            If cad = "" Then Continue For
            Select Case cad(0)
                Case "$"c
                    Try
                        Dim group As New Group
                        Dim pos As Integer
                        pos = cad.IndexOf("="c)
                        group.Name = cad.Substring(1, pos - 1).Trim
                        group.Controls = cad.Substring(pos + 1).Replace(" ", "").Split(","c)
                        lGroups.Add(group)
                    Catch ex As Exception
                        ShowError(Constants.GROUP_INCORRECT + cad)
                    End Try

                Case Else  'Case "+"c, "-"c, '#'c
                    RestrictionsList.Add(cad)
            End Select
        Next
        Restrictions = RestrictionsList.ToArray
        Groups = lGroups.ToArray
    End Sub

    ''' <summary>
    ''' Returns the list of restrictions contained in the security definition provided as a parameter
    ''' </summary>
    Public Shared Function GetRestrictions(ByVal RestrictionsDefinition As String()) As String()
        Dim _restrictions As String() = Nothing
        Dim _groups As Group() = Nothing
        If RestrictionsDefinition Is Nothing Then
            Return New String(0) {""}
        Else
            GetRestrictionsAndGroups(RestrictionsDefinition, _restrictions, _groups)
            Return _restrictions
        End If
    End Function

    ''' <summary>
    ''' Returns the list of groups contained in the security definition provided as a parameter
    ''' </summary>
    Public Shared Function GetGroups(ByVal RestrictionsDefinition As String()) As Group()
        Dim restrictions As String() = Nothing
        Dim groups As Group() = Nothing
        If RestrictionsDefinition Is Nothing Then
            Return Nothing
        Else
            GetRestrictionsAndGroups(RestrictionsDefinition, restrictions, groups)
            Return groups
        End If
    End Function

    ''' <summary>
    ''' Returns the groups and restrictions set at the level of security environment for the security component indicated,
    ''' serialized as they appear in the <see cref="ControlRestrictedUI.RestrictionsDefinition  "/> property
    ''' </summary>
    ''' <param name="IDControlRestrictedUI "></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetRestrictionsDefinition(ByVal IDControlRestrictedUI As String) As String()
        Dim sec As ComponentSecurity = Nothing
        ComponentsSecurity.TryGetValue(IDControlRestrictedUI, sec)

        If sec IsNot Nothing AndAlso Not (sec.Restrictions Is Nothing And sec.Groups Is Nothing) Then
            Dim nGroups As Integer = 0, nAuthorizations As Integer = 0
            Dim cad As String, x As Integer = 0

            If sec.Restrictions IsNot Nothing Then nAuthorizations = sec.Restrictions.Length
            If sec.Groups IsNot Nothing Then nGroups = sec.Groups.Length
            Dim def(nGroups + nAuthorizations - 1) As String

            If sec.Groups IsNot Nothing Then
                For Each g As Group In sec.Groups
                    cad = "$" + g.Name + "= " + Util.ConvertToString(g.Controls)
                    def(x) = cad
                    x += 1
                Next
            End If
            If sec.Restrictions IsNot Nothing Then sec.Restrictions.CopyTo(def, x)
            Return def
        Else
            Return New String(0) {""}
        End If
    End Function

#End Region

#Region "Various"

    ''' <summary>
    ''' Path to a configuration file, with definition of security.
    ''' It is used as default value to provide if the component itself does not force any own path.
    ''' </summary>
    ''' <remarks>This path is updated with the last assignment of the property <see cref="ControlRestrictedUI.ConfigFile "/> of any security component (of <see cref="RestrictedUI "/>)</remarks>
    Public Shared ConfigFile As String

    ''' <summary>
    ''' Controls the automatic call to the method <see cref="ControlRestrictedUI.RegisterControls "/> of each security component,
    ''' which forces the registration or update of existing controls on the form or user control where this security 
    ''' component (<see cref="ControlRestrictedUI"/>) is embedded.
    ''' </summary>
    ''' <remarks>
    ''' The automatic call (if this property is True) will occur when control initializes and after the addition or removal
    ''' on <see cref="SecurityEnvironment"/> of any adapters factory (allowing to discover a different number of controls)
    ''' </remarks>
    Public Shared Property AutomaticUpdateOfControlsFile() As Boolean
        Get
            Return _automaticUpdateOfControlsFile
        End Get
        Set(ByVal value As Boolean)
            If _automaticUpdateOfControlsFile <> value Then
                _automaticUpdateOfControlsFile = value
                If value Then RaiseEvent ControlAdapterFactoriesChanged() ' Thus forcing the upgrade of the open forms files
            End If
        End Set
    End Property
    Protected Shared _automaticUpdateOfControlsFile As Boolean = True

#End Region



#Region "Listen IHost events"
    ''' <summary>
    ''' Attends the <see cref="IHost.StateChanged"/> event of the object implementing the IHost interface
    ''' </summary>
    ''' <remarks>
    ''' Only must attend this event if the ID of the control that is addressed, and the instance is Nothing 
    ''' or coincides with that of the control that listens
    ''' </remarks>
    Private Shared Sub OnStateChanged(ByVal _ID As String, ByVal _instanceID As String, ByVal nuevoEstado As Integer)
        RaiseEvent StateChanged(_ID, _instanceID, nuevoEstado)
    End Sub

    ''' <summary>
    ''' Attends the <see cref="IHost.RolesChanged"/> event of the object implementing the IHost interface
    ''' </summary>
    ''' <remarks>
    ''' Only must attend this event if the ID of the control that is addressed, and the instance is Nothing 
    ''' or coincides with that of the control that listens
    ''' </remarks>
    Private Shared Sub OnRolesChanged(ByVal _ID As String, ByVal _instanceID As String)
        RaiseEvent RolesChanged(_ID, _instanceID)
    End Sub

#End Region


End Class

