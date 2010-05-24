Option Strict On

Imports System.ComponentModel
Imports System.ComponentModel.Design
Imports System.Drawing.Design

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
''' Base component of the Restricted Interface library (<see cref="RestrictedUI"/>): can restrict the visibility and enabling 
''' state of the controls included in a form or user control based on a definition of established security, involved the current 
''' state of the application in the form or container as well as the role or roles of the user of the application.
''' </summary>
''' <remarks>Must be inherited</remarks>
Public MustInherit Class ControlRestrictedUI
    Implements INotifyPropertyChanged, ISupportInitialize

    Const ERROR_READING_CONTROLSFILE As String = "There was an error reading the controls file: "

    ''' <summary>
    ''' Occurs just before permitting or preventing the change of Visible or Enabled property, to give the option 
    ''' to allow or not the change on the basis of a more complex logic.
    ''' </summary>
    ''' <param name="adaptControl">Adaptador del control que se está supervisando</param>
    ''' <param name="type ">Indica si la propiedad que está siendo modificada es Enabled o Visible</param>
    ''' <param name="allowChange ">Indica el resultado de la política de seguridad para este control en base a la situación
    ''' actual (estado y roles)
    ''' </param>
    ''' <remarks>Sólo se verifica esta seguridad (y se dispara por tanto este evento) cuando la modificación busca hacer visible
    ''' o habilitar el control, no cuando lo hace invisible o lo deshabilita</remarks>
    Public Event BeforeApplyingRestriction(ByVal adaptControl As IControlAdapter, ByVal type As TChange, ByRef allowChange As Boolean)

    Private _initializing As Boolean

#Region "Security Configuration: Public Interface"

    ''' <summary>
    ''' <para>Identifier of this <see cref=" ControlRestrictedUI "/> component, restrictions monitor. </para>
    ''' <para>From this identifier it can be read / updated the security definition from a file, to be established 
    ''' at an environment level</para>
    ''' <para>It will also be the key to index in <see cref="SecurityEnvironment.ComponentsSecurity" /></para>
    ''' </summary>
    ''' <remarks>You should assign a value. By default a GUID is assigned</remarks> 
    <Category("Configuration"), _
    Description("Identificador del componente ControlRestriccionesUI")> _
    Public Property ID() As String
        Get
            Return _ID
        End Get
        Set(ByVal value As String)
            _ID = value
            NotifyPropertyChanged("ID")
        End Set
    End Property
    Private _ID As String = Guid.NewGuid().ToString("N")

    ''' <summary>
    ''' Identifier of the component instance
    ''' </summary>
    ''' <remarks>
    ''' <para>With these attribute (together with the <see cref="ID"/> property) the <see cref="IHost"/> object will indicate the 
    ''' current status and roles, distinguishing not only on type of form but on specific instance.
    ''' (You can have multiple instances of a form, with different entities on probably different transaction state)</para>
    ''' <para>Typically the application will assign an ID to it. By default the ID is "00"</para>
    ''' </remarks> 
    <Browsable(False)> _
    Public Property InstanceID() As String
        Get
            If _instanceID IsNot Nothing Then
                Return _instanceID
            Else
                Return "00"
            End If
        End Get
        Set(ByVal value As String)
            _instanceID = value
            NotifyPropertyChanged("InstanceID")
        End Set
    End Property
    Private _instanceID As String = Nothing


    ''' <summary>
    ''' Configuration of the restrictions (prohibitions and permissions) to be set.
    ''' </summary>
    ''' <remarks>
    ''' More information on management of restrictions in <see cref="UIRestrictions "/> and <see cref="RestrictionOnControl "/>.
    ''' <seealso cref="UIRestrictions "/>
    ''' <seealso cref="RestrictionOnControl "/>
    ''' </remarks>
    <Category("Configuration"), _
    Description("Configuration of the restrictions (prohibitions and permissions) to be set." + _
                "All restrictions apply to individual controls, bearing in mind that the prohibitions will take precedence over permissions, " + _
                "that is, permissions will be aplied first and then restricted on the basis of the prohibitions" _
                )> _
    <Editor(GetType(RestrictionsDefinitionEditor), GetType(UITypeEditor))> _
    Public Property RestrictionsDefinition() As String()
        Get
            If _priorityEmbeddedSecurity Then
                Return _restrictionsDefinition
            Else
                Return SecurityEnvironment.GetRestrictionsDefinition(ID)
            End If
        End Get
        Set(ByVal value As String())
            _restrictionsDefinition = value
            If Me.DesignMode OrElse Not _initializing Then
                _priorityEmbeddedSecurity = True
            End If
            If Not _initializing Then ReinitializeSecurity()
            NotifyPropertyChanged("RestrictionsDefinition")
            NotifyPropertyChanged("RestrictionsDefinitionEmbedded")
        End Set
    End Property
    Protected _restrictionsDefinition As String()

    ''' <summary>
    ''' Returns the restrictions as defined and associated directly to the security component (probably at design time, 
    ''' though perhaps modified later)
    ''' </summary>
    ''' <remarks>
    ''' If <see cref="PriorityEmbeddedSecurity "/> is True then this restrictions will set the policy that will be applied in the component, 
    ''' otherwise it will be the one that has been loaded in the singleton object <see cref="SecurityEnvironment"/> either loading from
    ''' a file or from a string.
    ''' </remarks>
    <Browsable(False)> _
    Public ReadOnly Property RestrictionsDefinitionEmbedded() As String()
        Get
            Return _restrictionsDefinition
        End Get
    End Property


    ''' <summary>
    ''' It determines if the restrictions to apply to the security component are those defined and directly associated to the
    ''' component (embedded security) or else are those loaded in the <see cref="SecurityEnvironment"/> either loading from 
    ''' a file or from a string.
    ''' </summary>
    Protected Friend Property PriorityEmbeddedSecurity() As Boolean
        Get
            Return _priorityEmbeddedSecurity
        End Get
        Set(ByVal value As Boolean)
            If _priorityEmbeddedSecurity <> value Then
                _priorityEmbeddedSecurity = value
            End If
            NotifyPropertyChanged("RestrictionsDefinition")
            NotifyPropertyChanged("PriorityEmbeddedSecurity")
            NotifyPropertyChanged("Groups")
            NotifyPropertyChanged("Restrictions")
        End Set
    End Property
    Private _priorityEmbeddedSecurity As Boolean = True
    Private _priorityEmbeddedSecurityBAK As Boolean = True

    ''' <summary>
    ''' Returns the list of groups that may be defined in <see cref="RestrictionsDefinition "/>.
    ''' </summary>
    <Browsable(False)> _
    Public ReadOnly Property Groups() As Group()
        Get
            Return SecurityEnvironment.GetGroups(RestrictionsDefinition)
        End Get
    End Property


    ''' <summary>
    ''' Returns the list of restrictions (authorizations and prohibitions) that may be defined in <see cref="RestrictionsDefinition "/>,
    ''' not including the definition of groups, if any.
    ''' </summary>
    <Browsable(False)> _
    Public ReadOnly Property Restrictions() As String()
        Get
            Return SecurityEnvironment.GetRestrictions(RestrictionsDefinition)
        End Get
    End Property


    '''<summary>
    '''Gets or sets the path to a configuration file, to be used primarily at design time.
    '''</summary>
    '''<remarks>
    '''<para>The configuration file makes it possible to provide at design time and for the security definition 
    ''' in the form <see cref=" FrmRestrictionsUIDefinition "/> the list of roles and states to use, as well as additional 
    ''' control adapters factories, so to 'discover' new controls at design time.</para>
    ''' 
    '''<para>The own definitions of security restrictions, of all or only some components may be contained in this file.
    ''' These restrictions can be loaded at runtime using the method <see cref="SecurityEnvironment.LoadFrom"/> and saved 
    ''' and loaded at will from <see cref=" FrmRestrictionsUIDefinition "/> form, at design time or runtime.</para>
    ''' 
    '''<para>If the component does not force any path, then it will be offered the one which may have configured the object 
    ''' <see cref="SecurityEnvironment"/>, which is updated with the last assignment of this property (of any component)</para>
    ''' <para>Note: The path must be established in absolute way or relative to the folder of the solution (.sln)</para> 
    ''' <seealso cref=" FrmRestrictionsUIDefinition "/>
    ''' <seealso cref="SecurityEnvironment.LoadFrom"/>
    '''</remarks>
    <Category("Configuration"), _
    Description("Path to a configuration file, that makes it possible to provide at design time and for the security definition " + _
                "in the form FrmRestrictionsUIDefinition the list of roles and states to use, as well as additional " + _
                "control adapters factories, so to 'discover' new controls at design time." + _
                "The own definitions of security restrictions, of all or only some components may be contained in this file." + vbCrLf + _
                "Notas: - The path must be established in absolute way or relative to the folder of the solution (.sln)" + vbCrLf + _
                "       - If the component does not force any path, then it will be offered the one which may have configured the object " + _
                "SecurityEnvironment, which is updated with the last assignment of this property (of any component)")> _
    Public Property ConfigFile() As String
        Get
            If _configFile Is Nothing Then
                Return SecurityEnvironment.ConfigFile
            Else
                Return _configFile
            End If
        End Get
        Set(ByVal value As String)
            _configFile = value
            SecurityEnvironment.ConfigFile = value
            NotifyPropertyChanged("ConfigFile")
        End Set
    End Property
    Private _configFile As String


    ''' <summary>
    ''' Name of the file on wich can be written the list of the controls contained in the form or user control controlled by this component.
    ''' </summary>
    ''' <remarks>
    ''' <para>This file makes it possible to offer at design time controls from the form <see cref="FrmRestrictionsUIDefinition "/> 
    ''' to be created dynamically. For Web applications it necessarily must be used to configure security at design time with 
    ''' the help of that form.</para>
    ''' <para>After a first run of the application you can automatically update this file, and thus having the controls to use later, 
    ''' at design time.</para>
    ''' <para>Note: The path must be established in absolute way or relative to the folder of the solution (.sln)</para>
    ''' <para>See also <see cref="SecurityEnvironment.AutomaticUpdateOfControlsFile "/> and <see cref="RegisterControls"/> </para>
    ''' <seealso cref="SecurityEnvironment.AutomaticUpdateOfControlsFile "/>
    ''' <seealso cref="RegisterControls"/>
    ''' </remarks>
    <Category("Configuration"), _
    Description("Name of the file on wich can be written the list of the controls contained in the form or user control controlled by this component.")> _
    Public Property ControlsFile() As String
        Get
            Return _controlsFile
        End Get
        Set(ByVal value As String)
            _controlsFile = value
            NotifyPropertyChanged("ControlsFile")
        End Set
    End Property
    Private _controlsFile As String = ""

#End Region

#Region "Security External Controls: Public interface"

    ''' <summary>
    ''' Temporarily disables (pauses) the security policy imposed by the constraints of the component, so that
    ''' so that subsequent changes in monitored properties (Visible and Enabled) are permitted.
    ''' </summary>
    ''' <remarks>
    ''' Resetting to False (initial value) the definition of security is restored: some controls are disabled or hidden accordingly
    ''' </remarks>
    Property Paused() As Boolean
        Get
            Return _paused
        End Get
        Set(ByVal value As Boolean)
            If _paused <> value Then
                _paused = value
                If Not _paused And Not Me.DesignMode And _defSecurity IsNot Nothing Then
                    ReviseAppliedSecurity(Nothing)
                End If
                NotifyPropertyChanged("Paused")
            End If
        End Set
    End Property
    Private _paused As Boolean = False

    ''' <summary>
    ''' Forces resetting the security settings, processing set restrictions, assigning event handlers and reviewing for 
    ''' each monitored control the properties Visible and Enabled (or those for each control)
    ''' </summary>
    ''' <remarks>
    ''' This method is called internally in response to a change in security. It is offered as a public method to handle 
    ''' the dynamic creation of controls: when initializing the security at the beginning not all controls have been created, 
    ''' probably (e.g. columns of a DataGridView dynamically added).
    ''' Those controls can be covered by the security definition from the beginning, before they are created.
    ''' </remarks>
    Public Sub ReinitializeSecurity()
        Try
            If Me.DesignMode Or _defSecurity Is Nothing Then Exit Sub

            Dim lInitial As IList(Of IControlAdapter) = Me.SupervisedControls(True, True)

            _defSecurity = New UIRestrictions(Restrictions, ID, ParentControl, Groups)
            RemoveEventHandlers(lInitial)
            AddEventHandlers()
            ReviseAppliedSecurity(lInitial)

        Catch ex As Exception
            SecurityEnvironment.ShowError("ControlRestrictedUI.ReinitializeSecurity:" + ex.Message, Me.ParentControl, ID)
        End Try
    End Sub


    ''' <summary>
    ''' Forces visibility of the control or controls supplied, regardless of whether based on the existing restrictions 
    ''' may or may not be displayed.
    ''' </summary>
    ''' <param name="control">If it is Nothing will be forced the visibility of all monitored controls</param>
    ''' <remarks>
    ''' <para>
    ''' If the control whose visibility has been forced becomes invisible, its next possible visibility shall 
    ''' be subject to restrictions set</para>
    ''' <para>
    ''' The amendment to the security definition or a change in the current situation (change on user roles, state, ..) 
    ''' requiring review of applied security, may make invisible the control or controls
    ''' </para>
    ''' </remarks>
    Public Sub ForceVisibility(Optional ByVal control As Object = Nothing)
        Dim list As IList(Of IControlAdapter) = Me.SupervisedControls(False, True)

        _decidingChange = True
        If Not list Is Nothing Then
            For Each c As IControlAdapter In list
                If control Is Nothing OrElse c.Control Is control Then
                    c.Visible = True
                End If
            Next
        End If
        _decidingChange = False
    End Sub

    ''' <summary>
    ''' Forces the control or controls supplied to be enabled, regardless of whether based on the existing restrictions 
    ''' may or may not be enabled.
    ''' </summary>
    ''' <param name="control">If it is Nothing will be forced the enabled of all monitored controls</param>
    ''' <remarks>
    ''' <para>If the control forced to be enabled is disabled later, only will be enabled again based on the defined restrictions
    ''' </para>
    ''' <para>
    ''' The amendment to the security definition or a change in the current situation (change on user roles, state, ..) 
    ''' requiring review of applied security, can disable the control or controls
    ''' </para>
    ''' </remarks>
    Public Sub ForceEnabled(Optional ByVal control As Object = Nothing)
        Dim lista As IList(Of IControlAdapter) = Me.SupervisedControls(True, False)

        _decidingChange = True
        If Not lista Is Nothing Then
            For Each c As IControlAdapter In lista
                If control Is Nothing OrElse c.Control Is control Then
                    c.Enabled = True
                End If
            Next
        End If
        _decidingChange = False
    End Sub

    ''' <summary>
    ''' Removes the control from the supervised control list so that successive changes in the properties of visibility and enabled will not be intercepted
    ''' </summary>
    ''' <returns><b>True</b> if the control was monitored and was properly excluded, <b>False</b> otherwise</returns>
    ''' <param name="Control"></param>
    ''' <remarks>The review of applied security because of some change in the current situation (security definition, 
    ''' change in user roles, state, ..) will not affect this control
    ''' </remarks>
    Public Function ExcludeControl(ByVal Control As Object) As Boolean
        If _defSecurity Is Nothing Or Control Is Nothing Then Return False

        Try
            For Each c As IControlAdapter In Me.SupervisedControls(True, False)
                If c.Control Is Control Then
                    Return _defSecurity.ExcludeControl(Control)
                End If
            Next
            Return False

        Catch ex As Exception
            SecurityEnvironment.ShowError("ControlRestrictedUI.ExcludeControl:" + ex.Message, Me.ParentControl, ID)
            Return False
        End Try

    End Function

    ''' <summary>
    ''' Forces the record or update of the existing controls on the form or container where this component is embedded.
    ''' </summary>
    ''' <remarks>
    ''' <para>
    ''' The file in which it is written must be defined on the property <see cref="ControlsFile "/> and must exist, even empty.
    ''' The design time creation of a security component will ensure that this file exists (if property <see cref="ControlsFile "/> is set):
    ''' will be created empty if not found.
    ''' </para>
    ''' <para>
    ''' The component will automatically call this method to initialize and after the addition or elimination in the object <see cref="SecurityEnvironment"/> 
    ''' of some factory adapters (allowing you to discover more or less controls), but only if the property 
    ''' <see cref="SecurityEnvironment.AutomaticUpdateOfControlsFile "/> is True (it is by default)
    ''' </para>
    ''' <para>
    ''' May also be done explicitly, usually in the Load event or when dynamic controls are all created.
    ''' In Web applications using this file is the only way to determine at design time what are the controls contained in the form or
    ''' user control (At design time "Controls" properties don't contain any element. Nor does the use of Reflection to determine the properties WebControl
    ''' --works at runtime, not design time)<br/>
    ''' In WinForms applications this file makes it possible include in the security policy, at design time, controls to be created dynamically
    ''' </para>
    ''' </remarks>
    Public Sub RegisterControls()
        Dim lista As String = ""
        Dim file As String = ControlsFile
        If SecurityEnvironment.AdaptFilePath(file, Me.DesignMode) Then
            ' We can get the identification of the parent control without problems in WinForms, but it is not possible in Web
            ' (at least I could not get it)
            ' Therefore, rather than rely on the parent control to identify the form, will use the component identifier that is embedded in it
            'Dim idParent As String = Util.GetControlPadreID(_parentControl)
            Dim idParent As String = Me.ID

            Dim controlsLists As Dictionary(Of String, String) = ReadControls(SecurityEnvironment.ReadFile(file))
            MakeControlsListOf(lista, SecurityEnvironment.GetAdapter(_parentControl))
            lista = "[" + idParent + "]" + vbCrLf + lista
            controlsLists.Remove(idParent)
            controlsLists.Add(idParent, lista)
            My.Computer.FileSystem.DeleteFile(file)
            lista = ""
            For Each compName As String In controlsLists.Keys
                lista += controlsLists(compName) + vbCrLf + vbCrLf
            Next
            My.Computer.FileSystem.WriteAllText(file, lista, True)
        End If
    End Sub


    ''' <summary>
    ''' Finalizes the security management in this component, removing the event handlers to which are subscribed the various controls monitored.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub FinalizeSecurity()
        Try
            If ParentControl IsNot Nothing Then
                _defSecurity.Clear()
                RemoveHandler SecurityEnvironment.ControlAdapterFactoriesChanged, AddressOf OnControlAdapterFactoriesChanged
                RemoveHandler SecurityEnvironment.SecurityChanged, AddressOf OnSecurityChanged
                RemoveHandler SecurityEnvironment.SecurityChangedWithCancelInMind, AddressOf OnSecurityChangedWithCancelInMind
                RemoveHandler SecurityEnvironment.HotKeyChanged, AddressOf OnHotKeyChanged
                RemoveHandler SecurityEnvironment.StateChanged, AddressOf OnStateChanged
                RemoveHandler SecurityEnvironment.RolesChanged, AddressOf OnRolesChanged

                RemoveEventHandlers(Me.SupervisedControls(True, True))
                If TypeOf (ParentControl) Is System.Windows.Forms.Control Then
                    RemoveHandler DirectCast(ParentControl, System.Windows.Forms.Control).KeyDown, AddressOf OnParentControlKeyDown
                    If TypeOf (ParentControl) Is System.Windows.Forms.Form Then
                        _keyPreviewOriginal = DirectCast(ParentControl, System.Windows.Forms.Form).KeyPreview
                    End If
                End If
            End If

        Catch ex As Exception
        End Try
    End Sub


#End Region


#Region "New / Finalice  / ToString"

    <System.Diagnostics.DebuggerNonUserCode()> _
    Public Sub New()
        MyBase.New()

        'El Diseñador de componentes requiere esta llamada.
        InitializeComponent()
    End Sub

    Protected Overrides Sub Finalize()
        _defSecurity.Clear()
        _defSecurity = Nothing
    End Sub

    Private Sub ControlSeguridad_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed
        FinalizeSecurity()
    End Sub

    Public ReadOnly Property LongID() As String
        Get
            Dim parent As String = ""

            If ParentControl IsNot Nothing Then
                Dim grandParent As String = Util.GetParentID(ParentControl)
                parent = " (" + ParentControl.ToString + " / " + grandParent + ")"
            End If
            Return ID + " - " + InstanceID + parent
        End Get
    End Property

    Public Overrides Function ToString() As String
        Return LongID
    End Function

#End Region

#Region "Get Parent Form"
    ''' <summary>
    ''' Gets or sets the System.ComponentModel.ISite" of the Component
    ''' used to update ParentControl so it will be serialized to code
    ''' </summary>
    <Browsable(False)> _
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)> _
        Public Overrides Property Site() As System.ComponentModel.ISite
        Get
            Return MyBase.Site
        End Get
        Set(ByVal value As System.ComponentModel.ISite)
            MyBase.Site = value
            If Not MyBase.Site Is Nothing Then

                ' If the component is dropped onto a form during design-time,  
                ' set the ParentControl property.  
                Dim host As IDesignerHost = CType(value.GetService(GetType(IDesignerHost)), IDesignerHost)
                If host IsNot Nothing Then
                    Dim parent As Object = host.RootComponent
                    If Not parent Is Nothing Then
                        ParentControl = parent
                    End If
                End If

            End If

        End Set
    End Property


    Protected _parentControl As Object

    ''' <summary>
    ''' Gets or sets the parent control of the component
    ''' </summary>
    ''' <remarks>
    ''' Setting it at runtime (will be made from the designer of the container) will make the parent control to subscribe 
    ''' to the HandleCreated event (in WinForms) or to PreRender (in Web), with the method <see cref="AddEventHandlers "/>.
    ''' Thus when the event takes place (by then already has its children associated) will be carried out the monitoring 
    ''' of the selected control children, with the help of their corresponding <see cref="IControlAdapter"/> objects.
    ''' </remarks>
    <Browsable(False)> _
    Public MustOverride Property ParentControl() As Object

#End Region

#Region "Internal Security Management"
    Private _defSecurity As UIRestrictions

    ''' <summary>
    ''' Initializes the security component, processing the defined restrictions, subscribing to certain events of <see cref="SecurityEnvironment"/>,
    ''' and adding event handlers to supervised controls according to the defined security
    ''' </summary>
    Protected Sub InitializeSecurity(ByVal sender As Object, ByVal e As EventArgs)
        If Not ParentControl Is Nothing Then
            _defSecurity = New UIRestrictions(Restrictions, ID, ParentControl, Groups)
            AddHandler SecurityEnvironment.ControlAdapterFactoriesChanged, AddressOf OnControlAdapterFactoriesChanged
            AddHandler SecurityEnvironment.SecurityChanged, AddressOf OnSecurityChanged
            AddHandler SecurityEnvironment.SecurityChangedWithCancelInMind, AddressOf OnSecurityChangedWithCancelInMind
            AddHandler SecurityEnvironment.HotKeyChanged, AddressOf OnHotKeyChanged
            AddHandler SecurityEnvironment.StateChanged, AddressOf OnStateChanged
            AddHandler SecurityEnvironment.RolesChanged, AddressOf OnRolesChanged

            If TypeOf (ParentControl) Is System.Windows.Forms.Form Then
                _keyPreviewOriginal = DirectCast(ParentControl, System.Windows.Forms.Form).KeyPreview
            End If
            OnHotKeyChanged()  ' Verify if it is already established a HotKey
            AddEventHandlers()

            ' In WinForms this method will be called in the event HandleCreated of the parent control (form),
            ' so all Visible and Enabled properties are set and automatically controlled.
            ' However, the properties of other specific controls as UltraGrid no. Hence the following call
            ReviseAppliedSecurity(Nothing)

            If SecurityEnvironment.AutomaticUpdateOfControlsFile Then
                RegisterControls()
            End If
        End If
    End Sub

    ''' <summary>
    ''' It is ordered the monitoring of Visible and Enabled properties. Each control adapter (<see cref="IControlAdapter"/>)
    ''' will subscribe to the corresponding events (e.g. VisibleChanged y EnabledChanged o PreRender)
    ''' </summary>
    Private Sub AddEventHandlers()

        If Not _defSecurity.Prohibitions Is Nothing Then
            For Each b As RestrictionOnControl In _defSecurity.Prohibitions
                If b.Visible Then
                    b.ControlAdapt.SuperviseVisible(Me)
                End If
                If b.Enabled Then
                    b.ControlAdapt.SuperviseEnabled(Me)
                End If
            Next
        End If

        If Not _defSecurity.Authorizations Is Nothing Then
            For Each b As RestrictionOnControl In _defSecurity.Authorizations
                If b.Visible Then
                    b.ControlAdapt.SuperviseVisible(Me)
                End If
                If b.Enabled Then
                    b.ControlAdapt.SuperviseEnabled(Me)
                End If
            Next
        End If

    End Sub

    ''' <summary>
    ''' Removes the subscription to the events that allow to control changes in properties Visible and Enabled, to the list of adapters indicated
    ''' </summary>
    ''' <remarks>It is invoked by changing the security definition, before calling the method <see cref="AddEventHandlers "/></remarks>
    Private Sub RemoveEventHandlers(ByVal lInitial As IList(Of IControlAdapter))
        For Each c As IControlAdapter In lInitial
            c.FinalizeSupervision()
        Next
    End Sub

    ''' <summary>
    ''' It references the property whose change is being monitored: Enabled or Visible
    ''' </summary>
    ''' <remarks>The specific properties will depend on the control involved, which is abstracted through 
    ''' the adapter (<see cref="IControlAdapter"/>)
    ''' </remarks>
    Public Enum TChange
        Enabled = 0
        Visible = 1
    End Enum

    Private _decidingChange As Boolean = False

    ''' <summary>
    ''' Checks if the change on the specified property (<see cref="TChange"/>) is valid considering the definition of security, 
    ''' user roles and the current state of the application.
    ''' If the change is invalid will be undone, this is, set again to False.
    ''' </summary>
    ''' <param name="controlAdapt">Control adapter on which must be verified the changed</param>
    ''' <param name="type">Specifies whether to verify the change of Visible or Enabled</param>
    ''' <remarks>
    ''' Control adapters (<see cref="IControlAdapter"/>) call this method of the security component in response to 
    ''' the attempt to change the monitored properties.
    ''' </remarks>
    Public Sub VerifyChange(ByVal controlAdapt As IControlAdapter, ByVal type As TChange)
        If _decidingChange Then Exit Sub
        _decidingChange = True

        Select Case type
            Case TChange.Enabled
                ' If set to False there is nothing to prevent
                If controlAdapt.Enabled Then
                    If Not ChangeAllowed(controlAdapt, type) Then
                        controlAdapt.Enabled = False
                    End If
                End If

            Case TChange.Visible
                ' If set to False there is nothing to prevent
                If controlAdapt.Visible Then
                    If Not ChangeAllowed(controlAdapt, type) Then
                        controlAdapt.Visible = False
                    End If
                End If
        End Select
        _decidingChange = False
    End Sub

    ''' <summary>
    ''' Checks if the change on the specified property (<see cref="TChange"/>) is valid considering the security 
    ''' definition, user roles and the current state of the application.
    ''' </summary>
    ''' <param name="controlAdapt">Control adapter on which must be verified the changed</param>
    ''' <param name="type ">Specifies whether to verify the change of Visible or Enabled</param>
    ''' <returns><b>False</b> is the change is not allowed. <b>True</b> otherwise</returns>
    Public Function ChangeAllowed(ByVal controlAdapt As IControlAdapter, ByVal type As TChange) As Boolean
        If _paused Then Return True ' If monitoring in this control is paused we will prevent anything. Neither we will generate the event BeforeApplyingRestrinction

        Dim allowed As Boolean = True  ' Until proven otherwise be permitted

        Try
            ' Apply first the logic of the rule of AUTHORIZATIONS
            '-------
            ' Find a criterion by which to allow
            For Each p As RestrictionOnControl In _defSecurity.Authorizations
                If Not p.ControlAdapt.Control Is controlAdapt.Control Then Continue For
                If (type = TChange.Visible And Not p.Visible) OrElse (type = TChange.Enabled And Not p.Enabled) Then Continue For
                allowed = False    ' To this control (and property) has been applied a positive logic: only allowed changing to explicit ones
                'If p.rol <> 0 AndAlso Array.IndexOf(ActualRoles, p.rol) < 0 Then Continue For
                If Array.IndexOf(p.roles, 0) < 0 Then     ' 0 => All roles
                    Dim authorizedRole As Boolean = False
                    For Each r As Integer In UserRoles
                        If Array.IndexOf(p.roles, r) >= 0 Then
                            authorizedRole = True
                            Exit For
                        End If
                    Next
                    If Not authorizedRole Then Continue For
                End If

                If Not p.states Is Nothing AndAlso Array.IndexOf(p.states, HostState) < 0 Then Continue For

                allowed = True
                Exit For
            Next

            If allowed Then

                ' Apply second the logic of the rule of PROHIBITIONS
                '-------
                ' We seek a user role for which it is not prohibited
                ' If we find it then the change is allowed. Otherwise no
                For Each rol As Integer In UserRoles
                    allowed = True    ' Initially we suppose that the user rol is not restringed
                    For Each p As RestrictionOnControl In _defSecurity.Prohibitions
                        'If p.rol <> 0 And p.rol <> rol Then Continue For
                        If Array.IndexOf(p.roles, 0) < 0 AndAlso Array.IndexOf(p.roles, rol) < 0 Then Continue For
                        If p.ControlAdapt.Control IsNot controlAdapt.Control Then Continue For
                        If (type = TChange.Visible And Not p.Visible) OrElse (type = TChange.Enabled And Not p.Enabled) Then Continue For
                        If p.states IsNot Nothing AndAlso Array.IndexOf(p.states, HostState) < 0 Then Continue For

                        allowed = False
                        Exit For
                    Next
                    If allowed Then Exit For ' To this rol is not prevented
                Next

            End If

            'We give the option to allow or not change based on more complex logic
            RaiseEvent BeforeApplyingRestriction(controlAdapt, type, allowed)
            Return allowed

        Catch ex As Exception
            SecurityEnvironment.ShowError("ControlRestrictedUI.ChangeAllowed (" + controlAdapt.Identification(, Me) + ") :" + ex.Message, Me.ParentControl, ID)
            Return True
        End Try

    End Function

    ''' <summary>
    ''' Checks if after a change in the current situacion (security definition, change on user roles, application state, ...) 
    ''' properties Visible and Enabled must be activated.
    ''' </summary>
    ''' <param name="lInitial ">In the case of a change in security definition includes controls that have been monitoring</param>
    ''' <remarks></remarks>
    Private Sub ReviseAppliedSecurity(ByVal lInitial As IList(Of IControlAdapter))
        If _paused Then Exit Sub ' If monitoring in this control is paused we will do nothing. This method will be invoked when it is reset
        Try
            _decidingChange = True

            ' ENABLED
            '----------
            Dim lFinal As IList(Of IControlAdapter) = SupervisedControls(True, False)

            If Not lInitial Is Nothing Then
                For Each c As IControlAdapter In lInitial
                    c.Enabled = ChangeAllowed(c, TChange.Enabled)
                Next
            End If
            For Each c As IControlAdapter In lFinal
                c.Enabled = ChangeAllowed(c, TChange.Enabled)
            Next


            ' VISIBLE
            '----------
            lFinal = SupervisedControls(False, True)

            If Not lInitial Is Nothing Then
                For Each c As IControlAdapter In lInitial
                    c.Visible = ChangeAllowed(c, TChange.Visible)
                Next
            End If
            For Each c As IControlAdapter In lFinal
                c.Visible = ChangeAllowed(c, TChange.Visible)
            Next

            _decidingChange = False

        Catch ex As Exception
            _decidingChange = False
            SecurityEnvironment.ShowError("ControlRestrictedUI.ReviseAppliedSecurity:" + ex.Message, Me.ParentControl, ID)
        End Try

    End Sub

    ''' <summary>
    ''' Returns a list of all controls that are being monitored
    ''' <param name="enabled"><b>true</b>: consider those who have controlled the Enabled property</param>
    ''' <param name="visible"><b>true</b>: consider those who have controlled the Visible property</param>
    ''' </summary>
    Private Function SupervisedControls(ByVal cEnabled As Boolean, ByVal cVisible As Boolean) As IList(Of IControlAdapter)
        If _defSecurity Is Nothing Then Return Nothing

        Dim l As New List(Of IControlAdapter)
        For Each v As RestrictionOnControl In _defSecurity.Prohibitions
            If Not l.Contains(v.ControlAdapt) Then
                If (v.Enabled And cEnabled) Or (v.Visible And cVisible) Then
                    l.Add(v.ControlAdapt)
                End If

            End If
        Next

        For Each v As RestrictionOnControl In _defSecurity.Authorizations
            If Not l.Contains(v.ControlAdapt) Then
                If (v.Enabled And cEnabled) Or (v.Visible And cVisible) Then
                    l.Add(v.ControlAdapt)
                End If
            End If
        Next

        Return l
    End Function

    ''' <summary>
    ''' Returns the list of roles that have a user according to what is signaled by Host application (<see cref="IHost"/>) 
    ''' </summary>
    ''' <remarks>If there is no IHost object set, will return a list with only one role: 0</remarks>
    Private ReadOnly Property UserRoles() As Integer()
        Get
            If SecurityEnvironment.Host Is Nothing Then
                If _userRoles Is Nothing Then
                    _userRoles = New Integer(0) {0}
                End If
                Return _userRoles
            Else
                Return SecurityEnvironment.Host.UserRoles(ID, InstanceID)
            End If
        End Get
    End Property
    Private _userRoles As Integer()

    ''' <summary>
    ''' Returns the current state of the application (for the form --security component-- and concrete instance), according 
    ''' to the Host application (<see cref="IHost"/>)
    ''' </summary>
    ''' <remarks>If there is no IHost object set, will return as state: 0</remarks>
    Private ReadOnly Property HostState() As Integer
        Get
            If SecurityEnvironment.Host Is Nothing Then
                Return 0
            Else
                Return SecurityEnvironment.Host.State(ID, InstanceID)
            End If
        End Get
    End Property

    ''' <summary>
    ''' Indicates (True) if its a component used in Web applications or WinForms applications
    ''' </summary>
    ''' <remarks>Inherited classes should override this method</remarks>
    <Browsable(False)> _
    Public MustOverride ReadOnly Property WebComponent() As Boolean

#End Region

#Region "Listen SecurityEnvironment events"

    Private Sub OnControlAdapterFactoriesChanged()
        If SecurityEnvironment.AutomaticUpdateOfControlsFile Then
            RegisterControls()
        End If
    End Sub

    Private Sub OnSecurityChanged(ByVal IDControlRestrictedUI As String)
        If IDControlRestrictedUI = "" Or IDControlRestrictedUI = Me.ID Then
            If SecurityEnvironment.ComponentsSecurity.ContainsKey(ID) Then
                PriorityEmbeddedSecurity = False
            Else
                ' Removed security set to Environment level -> it apply the embedded one
                ' (if you want no security at environment level, for this component, you should define an empty security)
                PriorityEmbeddedSecurity = True
            End If
            ReinitializeSecurity()
        End If

    End Sub

    Private Sub OnSecurityChangedWithCancelInMind(ByVal IDControlRestriccionesUI As String, ByVal savePriority As Boolean, ByVal recoverPriority As Boolean)
        If IDControlRestriccionesUI = "" Or IDControlRestriccionesUI = Me.ID Then
            If savePriority Then
                _priorityEmbeddedSecurityBAK = PriorityEmbeddedSecurity
            End If
            If recoverPriority Then
                PriorityEmbeddedSecurity = _priorityEmbeddedSecurityBAK
            Else
                PriorityEmbeddedSecurity = Not SecurityEnvironment.ComponentsSecurity.ContainsKey(ID)
            End If
            ReinitializeSecurity()
        End If

    End Sub

    ''' <summary>
    ''' Handles the event <see cref="IHost.StateChanged"/> of the object that implements the <see cref="IHost"/> interface (captured and reissued by <see cref="SecurityEnvironment "/>)
    ''' </summary>
    ''' <param name="_ID"></param>
    ''' <param name="_instanceID"></param>
    ''' <param name="newState "></param>
    ''' <remarks>Should only be handled this event if control ID and the instance is Nothing or or coincides with that of the control that listens</remarks>
    Private Sub OnStateChanged(ByVal _ID As String, ByVal _instanceID As String, ByVal newState As Integer)
        If (String.IsNullOrEmpty(_ID) Or Me.ID = _ID) AndAlso (String.IsNullOrEmpty(_instanceID) Or Me.InstanceID = _instanceID) Then
            ReviseAppliedSecurity(Nothing)
        End If
    End Sub

    ''' <summary>
    ''' Handles the event (<see cref="IHost.RolesChanged "/>) of the object that implements the <see cref="IHost"/> interface (captured and reissued by <see cref="SecurityEnvironment "/>)
    ''' </summary>
    ''' <param name="_ID"></param>
    ''' <param name="_instanceID"></param>
    ''' <remarks>Should only be handled this event if control ID and the instance is Nothing or or coincides with that of the control that listens</remarks>
    Private Sub OnRolesChanged(ByVal _ID As String, ByVal _instanceID As String)
        If (String.IsNullOrEmpty(_ID) Or Me.ID = _ID) AndAlso (String.IsNullOrEmpty(_instanceID) Or Me.InstanceID = _instanceID) Then
            ReviseAppliedSecurity(Nothing)
        End If
    End Sub


#End Region

#Region "Maintenance security Form"

    ''' <summary>
    ''' Shows the maintenance security form, allowing both consultation and modification of restrictions of the security component.
    ''' As in design time it is possible to recover or save the security to a file, among other things.
    ''' </summary>
    ''' <remarks>To be invoked at runtime in WinForms applications, in test or configuration mode</remarks>
    Public Sub ShowConfigurationSecurityForm(Optional ByVal host As IHost = Nothing, _
                                                 Optional ByVal restrictionsDef As String() = Nothing)
        If WebComponent Then Exit Sub

        Dim form As FrmRestrictionsUIDefinition
        Dim file As String = ConfigFile
        SecurityEnvironment.AdaptFilePath(file, False)

        form = New FrmRestrictionsUIDefinition(Me, restrictionsDef, file, host, False)
        form.Show()
    End Sub
#End Region

#Region "Capture Hot key"

    ''' <summary>
    ''' Handles the event <see cref="SecurityEnvironment.HotKeyChanged"/>. If you have enabled a Hotkey to open the maintenance
    ''' security form it will associate the KeyDown event of the form (or user control) on which is embedded the security
    ''' component with the method <see cref="OnParentControlKeyDown"/>, that will launch the maintenance form.
    ''' </summary>
    ''' <remarks>Using HotKey is designed for easy configuration of security at runtime, but in phases of development and 
    ''' testing of application</remarks>
    Private Sub OnHotKeyChanged()
        If Not TypeOf (ParentControl) Is System.Windows.Forms.Control Then Exit Sub

        RemoveHandler DirectCast(ParentControl, System.Windows.Forms.Control).KeyDown, AddressOf OnParentControlKeyDown
        If SecurityEnvironment.AllowedHotKey Then
            AddHandler DirectCast(ParentControl, System.Windows.Forms.Control).KeyDown, AddressOf OnParentControlKeyDown
            If TypeOf (ParentControl) Is System.Windows.Forms.Form Then
                DirectCast(ParentControl, System.Windows.Forms.Form).KeyPreview = True
            End If
        Else
            If TypeOf (ParentControl) Is System.Windows.Forms.Form Then
                DirectCast(ParentControl, System.Windows.Forms.Form).KeyPreview = _keyPreviewOriginal
            End If
        End If
    End Sub
    Private _keyPreviewOriginal As Boolean

    Private Sub OnParentControlKeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        If e.KeyCode = SecurityEnvironment.HotKey.KeyCode And e.Modifiers = SecurityEnvironment.HotKey.Modifiers Then
            ShowConfigurationSecurityForm(SecurityEnvironment.Host, Me.RestrictionsDefinition)
        End If
    End Sub

#End Region

#Region "Controls Register in File"

    ''' <summary>
    ''' Returns the list of controls associated to a certain form (or user control), as found recorded in the file indicated
    ''' </summary>
    ''' <param name="controlsFile ">File path from which to read controls</param>
    ''' <param name="idControlRestrictedUI  ">String identifying the form (or container)</param>
    ''' <remarks>See <see cref="RegisterControls "/>, <see cref="SecurityEnvironment.AutomaticUpdateOfControlsFile "/> 
    ''' and <see cref="ConfigFile "/></remarks>
    Protected Friend Function ReadComponentControls(ByVal controlsFile As String, ByVal idControlRestrictedUI As String) As String()
        Dim list As String = ""
        Dim fichero As String = controlsFile
        Dim result As String() = Nothing
        If SecurityEnvironment.AdaptFilePath(fichero, Me.DesignMode) Then
            Dim controlsLists As Dictionary(Of String, String) = ReadControls(SecurityEnvironment.ReadFile(fichero))
            list = ""
            If controlsLists.TryGetValue(idControlRestrictedUI, list) Then
                result = list.Split(New String() {vbCrLf}, StringSplitOptions.RemoveEmptyEntries)
            End If
        End If

        If result Is Nothing Then
            result = New String(0) {}
        End If
        Return result
    End Function

    Protected Function ReadControls(ByVal TxtControls As String) As Dictionary(Of String, String)
        Dim idControlRestriccionesUI As String = ""
        Dim controlsLists As New Dictionary(Of String, String)
        Dim controls As String = ""
        Try
            Dim reader As New System.IO.StringReader(TxtControls)
            Try
                Dim line As String
                Do
                    line = reader.ReadLine
                    If line Is Nothing Then Exit Do
                    line = line.Trim
                    If line = "" Then Continue Do

                    If line(0) = "["c Then
                        If idControlRestriccionesUI <> "" Then
                            controlsLists.Add(idControlRestriccionesUI, controls)
                        End If
                        idControlRestriccionesUI = line.Substring(1, line.Length - 2)
                        controls = line
                    Else
                        If idControlRestriccionesUI <> "" Then
                            controls += vbCrLf + line
                        End If
                    End If

                Loop
            Finally
                reader.Close()
            End Try
            If idControlRestriccionesUI <> "" Then
                controlsLists.Add(idControlRestriccionesUI, controls)
            End If
            Return controlsLists

        Catch ex As Exception
            Dim cad As String = ERROR_READING_CONTROLSFILE + ex.Message
            SecurityEnvironment.ShowError(cad, Nothing)
            Return controlsLists
        End Try
    End Function

    ''' <summary>
    ''' Returns in <paramref name="lista"/> the list of all controls contained in the form (or user control) referenced 
    ''' by <paramref name="controlAdapt"/>.
    ''' </summary>
    ''' <param name="list">String that receives the list of controls</param>
    ''' <param name="controlAdapt">
    ''' Adapter that wraps the control from which will seek its child controls. It will be the parent form (or container) 
    ''' on the first call (it is a recursive method)
    ''' </param>
    ''' <param name="parent ">
    ''' Adapter that wraps the parent control referenced by <paramref name=" controlAdapt"/>. It will be empty on the first call (it is a recursive method)
    ''' </param>
    ''' <remarks>
    ''' The number of controls 'discovered' within that container will depend on the presence or absence of controls adapter factories
    ''' that understand certain controls and look into them. For example, the factory AdapterInfragisticsWinForms_Factory offers
    ''' adapters like AdapterInfragisticsWinForms_UltraGrid, that lets discover (and manage) the columns of a UltraGrid control
    ''' </remarks>
    Protected Sub MakeControlsListOf(ByRef list As String, ByVal controlAdapt As IControlAdapter, Optional ByVal parent As IControlAdapter = Nothing)

        If Not controlAdapt.Control Is _parentControl Then
            list += controlAdapt.Identification(parent, Me) + vbCrLf
        End If
        For Each c As IControlAdapter In controlAdapt.Controls
            MakeControlsListOf(list, c, controlAdapt)
        Next
    End Sub

#End Region

#Region "INotifyPropertyChanged implementation"
    'Note: this interface is new in version 2.0 of .NET Framework. 
    'Notifies clients that a property value has changed.
    'Namespace: System.ComponentModel
    'If your data source implements INotifyPropertyChanged and you are performing asynchronous operations, you should not make changes to the data source on a background thread. Instead, you should read the data on a background thread and merge the data into a list on the UI thread.
    '
    'See also article "Windows Forms Data Binding and Objects"  (Rockford Lhotka)
    'That interface is an alternative (recommended) to:
    'When we bind a control to a property on our object, data binding automatically starts listening for a property changed event named propertyChanged, 
    'where property is the name of the property of the object .
    'For instance, our Order class defines an ID property. When the ID property is bound to a control, data binding starts listening for an IDChanged event. 
    'If this event is raised by our object, data binding automatically refreshes all controls bound to our object.
    '
    'Also interesting: 
    '" A generic asynchronous INotifyPropertyChanged helper" (http://www.claassen.net/geek/blog/2007/07/generic-asynchronous.html)

    Protected Sub NotifyPropertyChanged(ByVal info As String)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(info))
    End Sub

    Public Event PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
#End Region

#Region "ISupportInitialice implementation"

    ' The implementation of ISupportInitialice interface allows us to know at what point the designer starts and just load the properties.
    ' This allows us to call ReinitializeSecurity (which will occur subsequently when modifying RestrictionsDefinition)
    ' once the other properties are already filled, including ID.
    ' (Designer assigns the properties in alphabetical order)
    ' ->Design-Time Integration—Batch Initialization: http://en.csharp-online.net/Design-Time_Integration%25E2%2580%2594Batch_Initialization

    Private Sub BeginInit() Implements System.ComponentModel.ISupportInitialize.BeginInit
        _initializing = True
    End Sub

    Private Sub EndInit() Implements System.ComponentModel.ISupportInitialize.EndInit
        _initializing = False
        If SecurityEnvironment.ComponentsSecurity.ContainsKey(ID) Then
            _priorityEmbeddedSecurity = False
        End If
        ReinitializeSecurity()
    End Sub

#End Region

End Class




