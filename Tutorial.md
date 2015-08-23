
<br />

---

# 1. Introduction #
A delicate part in the implementation of applications's security, specially corporate applications, is to determine which functionalities must or not be accessible to the user depending on his role, or which elements should or should not be shown.

It is usual to include in the code logic to hide or disable certain options, buttons, etc, depending on the user accessing the application. This can get complicated when apart from the type of user should also be considered the state of the application, such as the processing status of an entity.

Also, it is quite usual that this policy should be changed with some frequency, in the same way that should must evolve the application requirements. Due to changes in the organizational structure of the company, modifications in management protocols or simply the identification of gaps after the use of the application, among other things, would be necessary to make adjustments to this logic related to the interface.

In response to this problem it is also possible to centralize the definition of this policy, in a common repository for multiple applications. Based on this definition, supported by a particular data model, you can define a library used by the applications that makes it possible to modify the security policy without having to recompile and redeploy applications. The main idea is that the programmer develops as if there were only one type of user, administrator, with permission to do anything. It is the library that controls whether an attempt should be prevented from making visible certain controls or enable them.

This approach is currently used in the company where I work. While offering great flexibility in design, it's concrete implementation presents, from my point of view, a number of constraints and practical difficulties that I have tried to correct with this other library / framework:

<br />
## Key Features ##
On the one hand the library / architecture described here does not require to centralize the definition of this security, but it's seen as a case; secondly, to ensure compliance with these interface restrictions it is not necessary to require the programmer to use specific methods or properties of the library, to check if the change is allowed or not, nor has to be controlled by any code; simply any attempt to make visible or enabled any interface element supervised not allowed by the security policy is intercepted and prevented. By default, it is only supervised the attempt of making visible or enabled the control, but it can be configured to monitor also the 'deactivation' of that properties, that is, the attempt to make invisible or disabled the control.


It is allowed to define restrictions policy not only on the basis of roles but also on the application states, in both WinForms and Web applications; and to simplify the definition of this policy a form of maintenace is provided, available both at design time and runtime.

Notes:
  * It isn't objective of this library to control user authentication. It is assumed that this is done correctly and it is known, with guarantees, the role or roles that the user holds.
  * The name of the library, RestrictedUI refers to the fact that certain elements of the interface will be restricted according to established security ('Restricted area')

<br /><br />


---

# 2. Using the code #

I will describe now in detail this code:

## 2.1. Main entities ##
<br />
![http://restricted-ui.googlecode.com/svn/wiki/images/modelo1.gif](http://restricted-ui.googlecode.com/svn/wiki/images/modelo1.gif)

<br /><br />

![http://restricted-ui.googlecode.com/svn/wiki/images/modelo2.gif](http://restricted-ui.googlecode.com/svn/wiki/images/modelo2.gif)

<br />
The main classes and with wich the programmer will primarily interact are fundamentally: **`ControlRestrictedUI`** (through some specific components that are offered either for WinForms as for Web), **`SecurityEnvironment`** and **`IHost`**.

The first contains most of the business logic, and is responsible for ensuring the UI security in the control where it is, relying on other classes.
The second, `SecurityEnvironment`, is a Singleton object that lets you configure aspects that are common to all components and makes it possible to massively set security for all or part of the components (necessary for loading restrictions from a centralized repository).
The third, `IHost`, is the interface that should provide the application that makes use of this library and will let know the status of the application and the user role or roles.

Constrictions policy that apply to a `ControlRestrictedUI` component are stored in the form of an `UIRestrictions` object and this in turn contains a list of restrictions (permissions and prohibitions), which are only structures `RestrinctionOnControl`. These two entities are described below with a little more detail and along with them how to define security policy.

In case that it is needed to work with controls that are not already considered or some of them need to be managed differently, it will be necessary to create the corresponding adapters (`IControlAdapter`) along with the factory that generate them (`IControlAdapterFactory`). All that has to be done in these cases is to develop a new DLL that includes the new adapters required and the corresponding factory. This factory will be added to the `SecurityEnvironment` object, as explained in the example described below. You can create these adapters and factories basing them on those already available.

Clasess and methods more noteworthy are:


<br />
### `ControlRestrictedUI` ###
Base component of the Restricted Interface library (RestrictedUI): can restrict the visibility and enabling state of the controls included in a form or user control based on a definition of established security, involved the current state of the application in the form or container as well as the role or roles of the user of the application.

  * **`ID, InstanceID`** - Identifier of the component and the specific instance. From the `ID` it can be read / updated the security definition from a file, to be established at an environment level. With these two attributes the `IHost` object will indicate the status and roles that apply, distinguishing not only on type of form but on specific instance.

  * **`RestrictionsDefinition`** - Configuration of the prohibitions and permissions to be set.

  * **`ConfigFile`** - Gets or sets the path to a configuration file, to be used primarily at design time.
> The configuration file makes it possible to provide at design time and for the definition of security in the form `FrmRestrictionsUIDefinition` the list of roles and states to use, as well as additional control adapters factories, so to 'discover' new controls at design time.
> The own definitions of security restrictions, of all or only some components may be contained in this file. These restrictions can be loaded at runtime using the method `LoadFrom()` and saved and loaded at will from `FrmRestrictionsUIDefinition` form, at design time or runtime.

  * **`ControlsFile`** - Name of the file on wich can be written the list of the controls contained in the form or user control controlled by this component.
> This file makes it possible to offer at design time controls from the form `FrmRestrictionsUIDefinition` to be created dynamically. For Web applications it necessarily must be used to configure security at design time with the help of that form.

  * **`RegisterControls`** - Forces the record or update of the existing controls on the form or container where this `ControlRestrictedUI` component is embedded.

  * **`BeforeApplyingRestriction`** - Occurs just before permitting or preventing the change of `Visible` or `Enabled` property, to give the option to allow or not the change on the basis of a more complex logic.

  * **`ChangeAllowed`** - It is the method that assesses the restrictions policy and decides whether to allow a certain change in a control. It checks if the change on `Visible` or `Enabled` property (or the corresponding on the control) is valid considering the definition of security, user roles and the current state of the application.

  * **`VerifyChange`** - Checks if the change on the specified property (`TChange`) is valid considering the definition of security, user roles and the current state of the application. If the change is invalid will be undone, this is, set again to `False`.
> Control adapters call this method of the security component in response to the attempt to change the monitored properties.

  * **`SuperviseDeactivation`** - It determines if the restrictions will be applied also in the 'deactivation' of the controls (controlling the attempt of making a control invisible or disabled), not only in the activation.


<br />
### `ControlRestrictedUIWeb` ###
Adaptation, for Web applications, of the `ControlRestrictedUI` component.


<br />
### `ControlRestrictedUIWinForms` ###
Adaptation, for WinForms applications, of the `ControlRestrictedUI` component.


<br />
### `IControlAdapter` ###
Wraps a 'control' with the aim of providing an uniform access to its `Enabled` and `Visible` properties and its 'controls' son, also allowing to provide oversight of the change of these properties, giving option to prevent such changes based on a security scheme. What the adapter wraps needs not to be an object of a particular class, it can be anything that the adapter 'understands', on which it is possible and interesting to control the visibility and or enabled state, and that an adapter 'father' has recognized it as a son.

Actually controlled properties need not be exactly '`Visible`' and '`Enabled`'; it is the responsibility of the control adapter to offer that interface and to act on the properties that the control has (for example, some controls do not offer Enabled but `ReadOnly`)

The controls provided in the maintenance of security form (`FrmRestrictionsUIDefinition`) and the controls where the component `ControlRestrictedUI` seeks are those offered as 'children' for the control in which is embedded the component. The adapter for this parent control will be asking to their child controls' adapters, which will discover new controls. So, if you have an adapter that understand controls `DataGridView`, for example, it is possible to provide each of the columns as a child to monitor. Thus, the number of controls to monitor depends on the adapters with which to work, and this depends in turn on the factories that have been incorporated:


<br />
### `IControlAdapterFactory` ###
Defines an external factory of control adapters. These will be consulted before the base factory (internal) when it comes to locating the adapter to be used.

It only defines the following method: `GetAdapter`. Returns the most suitable adapter for the control that is being passed, according to the factory. If the factory has no proper adapter for this control, it will return the special adapter `NullControlAdapter`.


<br />
### `UIRestrictions` ###
It contains all the interface restrictions (UI) (permissions and prohibitions) that define security for a `ControlRestrictedUI` component, and that therefore they usually involve a form or user control.

All restrictions apply to individual controls, bearing in mind that the prohibitions will take precedence over permissions, that is, permissions will be aplied first and then restricted on the basis of the prohibitions:

  * **Prohibitions**: only will be prevented changes to the property `Visible` / `Enabled` in the situations outlined here.

  * **Permissions**: only will be authorized changes to the property `Visible` / `Enabled` in the situations outlined here.

This class saves individual restrictions (see `RestrictionOnControl`) and determines if they should be considered in positive logic (permissions) or negative logic (prohibitions). It is responsible for serializing and deserializing these permissions.



<br />
### `RestrictionOnControl` ###
It defines the elements that make up a particular restriction to monitor. This restriction will only have its full meaning when read in conjunction with other restrictions included in a security policy defined in a `UIRestrictions` object and managed by a `ControlRestrictedUI` component.

The elements which form an individual restriction are:

  * The control to be monitored (via an adapter)
  * The properties to be monitored (`Visible` and/or `Enabled`)
  * The context of the application for which the restriction is defined:
    * Rol or roles of the application user
    * State or states of the application

These elements may be applied in positive logic (permissions) or negative (prohibitions). This interpretation is not offered by this entity, but by `UIRestrictions` depending on whether this restriction has been placed in a line of permissions or prohibitions.

  * If the restriction is a permission, it will indicate that the supervised properties can only be 'activated' (make visible or enable) by the established roles and only when the application is in the established states.
  * If the restriction is a prohibition, it will indicate that the supervised properties can only be 'activated' (make visible or enable) by the established roles when the application is in the mentioned states. For any other combination of roles / state the activation will be possible.
  * If no role is provided (by default, role is assumed = 0) then it will apply to all roles: all of them will be allowed or prevented (depending) in the indicated states.
  * If no state is provided, then the restriction will apply to all concerning roles, regardless of the state in which the application is.
  * If a control has no associated restriction element (neither positive nor negative) then it will not be monitored, and any role and in any state could activate its `Visible` and `Enabled` properties.


Actually controlled properties in the control need not be exactly '`Visible`' and '`Enabled`' (as shown in `IControlAdapter`).

By default, it is only supervised and perhaps prevented (depending on the policy defined) the 'activation' of the properties, namely the attempt to make visible or enable the control. It is not prevented to make invisible or disabled a control. With the property `SuperviseDeactivation` (in `ControlRestrictedUI` component) it is possible to change that general behaviour and monitor also the 'deactivation' of the controls, allowing or preventing it according to the security definition.
It is also possible to indicate a list of controls for which the attempt of making it invisible or disabled should or not be supervised, and so it may or not be prevented (independently of the value of that `SuperviseDeactivation` property).



<br />
### `IHost` ###
It indicates that the object can act as intermediary in the necessary communication between the Host and the Restricted Interface library, enabling it to view at any time the application status and the role or roles that may have the user. It also reports through events of changes to these values.

These data (state and roles) may be dependent on the form or user control from which we are asking (type and specific instance).

This approach allows, for example, keep different instances of a particular window open, each one in a state or a different processing stage and therefore with different security requirements. While it is more common that the user role or roles should be uniform throughout the application, they are also allowed to depend on where you place the security component and the specific instance. This will give more leeway when configuring the security.


<br />
### `SecurityEnvironment` ###
Singleton class (via Shared) that keeps general aspects to all security scheme

  * **`Host`** - Gets or sets the object that implements the interface `IHost` and through which the environment and other security components may interact with the Host application in order to know the current status and roles to be considered.

  * **`LoadFrom`**, **`LoadFromString`** - They let you load the security policy (for all or a subset of security components) from a file, a stream or a text string, at environment level.

  * **`CommonStates`**, **`CommonRoles`** - They allow to establish the common roles and states that are offered in all security components.

  * **`ComponentsSecurity`** - It returns the dictionary object which stores the definition of security (policy interface constraints) of the various components, as may have been established from the load of a configuration file or a text string.
> The security herein needs not be equal to the embedded in the various components. The restrictions finally applied in the component depend on one of its properties (`PriorityEmbeddedSecurity`). Methods like `LoadFrom` or `LoadFromString`, for example, will establish on the `SecurityEnvironment` the restrictions to apply to each of the components that are referenced in the file or string (even with an empty definition), and set the indicated property so that the components use that restrictions.

  * **`GetAdapter`** - Returns the adapter (`IControlAdapter`) for the indicated control, based on registered factories in the `SecurityEnvironment`.

  * **`BaseFactory`**, **`AditionalFactories`** - Gets or sets the adapter factory considered as base, as well as additional ones. When it comes to finding the best control adapter it will start looking for it in the additional ones and finally in the base factory if none is found before.

  * **`AddFactoria`** - Adds the specified factory of control adapters. It will be consulted earlier than the internal factory.


<br /><br />

---

## 2.2. Example Usage ##

It is necessary to include a reference to the DLLs `RestrictedUI.dll` and `RestrictedWebUI.dll` or `RestrictedWinFormsUI.dll`, depending on the type of application. If you include in your application some special controls that you want to address in a personalized way then you will add a reference to the corresponding DLL, which will have implemented a factory of adapters for that particular control. The following example uses a library that interprets the controls of Infragistics `UltraGrid` and `UltraTree` ([NetAdventage](http://www.infragistics.com/dotnet/netadvantage.aspx)).

At the beginning of the application we do the following initialization. Only the first line is really important:
<br />

```
' Initial setup of the security library:
'-----------------------------------------

' We set the object IHost that will reveal the status and roles of the application'
SecurityEnvironment.Host = _host

' We dont want config files to be automatically updated on the initialization of the security components.'
' We will update them when we have created all the controls (some of them will be build dynamically: datagrid columns)'
SecurityEnvironment.AutomaticUpdateOfControlsFile = False

' Because we use Infragistics controls and they are not covered by internal factory included with the library'
' ControlRestrictedUIWinForms, we will record the factory that manages them'
SecurityEnvironment.AddFactory(AdapterInfragisticsWinForms_Factory.getInstance)

' Some adapters like these can allow to control the enabled state with the ReadOnly property instead of Enabled'
' (the latter would be the default)'
AdapterWinForms_DataGridView.UseReadOnly = True
AdapterInfragisticsWinForms_UltraGrid.UseReadOnly = True

' Apart from that we have defined some security policy embedded in a component, we will use the one defined in a file'
' (We could also have read the security from a stream providing a System.IO.StreamReader, or directly from a string '
' with EntornoSeguridad.LoadFromString)'
SecurityEnvironment.LoadFrom("TestWinForms\Security.txt")
```

> Below is a sample configuration file that can be loaded by `LoadFrom`.


<br />
The object _host would be of the class Host, which would implement the interface `IHost`, for example like this (`TestForm`):_

```
Public Class Host
    Implements IHost

    Public Event StateChanged(ByVal ID As String, ByVal instanceID As String, ByVal newState As Integer) Implements RestrictedUI.IHost.StateChanged
    Public Event RolesChanged(ByVal ID As String, ByVal instanceID As String) Implements RestrictedUI.IHost.RolesChanged

    Public Sub ShowError(ByVal [error] As String) Implements IHost.ShowError
       ' There are controls that are dynamically created and it is common not to locate them in the event HandleCreated (if WinForms)'
        If [error].Contains("Control not found") Then
            If [error].Contains("cControles.") Then
                Exit Sub
            End If
        End If

        MessageBox.Show([error])
    End Sub


    ' In this implementation, the state may be different depending on the component and instance.'
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


    ' In this implementation, the roles that we refer will be the same regardless of the form or the instance'
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
```

Although in the `IHost` interface the properties `State` and `UserRoles` have not been set as ReadOnly, the reading of these properties is the most important thing. The change in state or roles will be used (if allowed) to test security from the maintenance form. If it is not necessary, the implementation of the modification (set) can be left empty.


<br />
We will add a `ControlRestrictedUI` component for every form or user control in wich we want to apply a restrictions policy. The configuration of this component is usually made at design time. As an example (looking at it through the code generated by the Designer):

```
'ControlRestrictedUIWinForms1
'
Me.ControlRestrictedUIWinForms1.ConfigFile = "TestWinForms\Security.txt"
Me.ControlRestrictedUIWinForms1.ControlsFile = "TestWinForms\Controls.txt"
Me.ControlRestrictedUIWinForms1.ID = "Form1"
Me.ControlRestrictedUIWinForms1.InstanceID = "00"
Me.ControlRestrictedUIWinForms1.ParentControl = Me
Me.ControlRestrictedUIWinForms1.Paused = False
Me.ControlRestrictedUIWinForms1.RestrictionsDefinition = New String() {
  "$Group 0= GroupBox1.CheckBox1, GroupBox1.TextBox2",
  "$Group 2= TextBox",
  "+0/GroupBox1.CheckBox1,E,0", "+99/Combo,V"}
```

Roles alias are not used in the constraints defined directly on the component, not depending thus on any translation table

<br />
Apart from that configuration, we can add the following code at the beginning of the form or container, for example in the `Load` event:

```
' If we want the application to be able to determine a state and independent role or roles for each instance of a '
' form, so that we can open several ones simultaneously and these ones may have independent lives, then we must identify each instance:'
 ControlRestrictedUIWinForms1.InstanceID = _InstanceID

' If we have created controls dynamically then we can update the controls file (if we are developing we might want'
' to be allowed to establish security over controls available only in runtime)'
#If DEBUG Then
  ' To update the controls file and thus be able to consider those controls when editting the security in design time.'
   ControlRestrictedUIWinForms1.RegisterControls()
#End If

' In any case, if we dynamically created controls they will not have been localized when security initializes.'
' So we will force a reinitilization of the security:'
 ControlRestrictedUIWinForms1.ReinitializeSecurity()
```

<br />
Security can be set directly at environment level for all or only a selection of components using a text string, for example contained in a file, similar to the following:

```
[Factories]
; Relative paths will be expressed in relation to the folder containing the solution (. sln). This path will be 
; used to locate the DLL in design time. We will assume that the DLL is in the same folder as the executable, 
; so in runtime the path will be ignored and used only the name of the file
; Note: You may also use absolute paths.
TestWinForms\bin\Debug\RestrictedWinFormsUI_Infragistics.dll, RestrictedWinFormsUI_Infragistics.AdapterInfragisticsWinForms_Factory

[CommonRoles]
99,Administrator,Adm
10,Director,Dtor
20,Consultant,Cons

[CommonStates]
0,Initial
1,Pending Validating
2,Validated

;=======================================================
[SECURITYCONTROL=Form1]
[Roles]
30,Auxiliar,Aux

[States]
3,Special State

[Groups]
Group Disable Buttons= gbCommands.btnDisableEnabled, gbCommands.btnDisableVisible
Group Enabled Buttons= gbCommands.btnEnableEnabled, gbCommands.btnEnableVisible, gbCommands.btnEnableVisible_N
Group Trees= TreeView1, UltraTree1

[Restrictions]
#Yes=TextBox,$Group Trees
#No=GroupBox1.CheckBox1
+123/TextBox,V,2
+Adm/$Group Trees,E/MenuStrip1.EditarToolStripMenuItem.CortarToolStripMenuItem,V
+Dtor/GroupBox1.CheckBox1,V
-0/MenuStrip1.ArchivoToolStripMenuItem.NuevoToolStripMenuItem,E/ToolStrip1.ToolStripComboBox1,E/ToolStrip1.ToolStripSplitButton1,E
-Aux/cControles.Name,V,1,2,3/$Group Enabled Buttons,V
-Cons/combo,V/$Group Disable Buttons,E

;=======================================================
[SECURITYCONTROL=Form2_Sub1]
[Roles]

[States]

[Groups]
Grupo 0= CheckBox1, TextBox1
Grupo 1 - nuevo grupo= Button1, TextBox1

[Restrictions]
+0/$Grupo 0,E
-0/CheckBox1,E/Button1,E
```

<br />
Through this feature you can store security restrictions in a centralized repository, for example a relational database and then retrieve and apply them at the beginning of the application. This way we can modify the restrictions without recompiling applications.

In the definition to be loaded into the `SecurityEnvironment`, for instance from a file, roles alias can be used to define restrictions, because it is assumed that they are described in the same file. Anyway, when it is applied to the security component they are translated into their corresponding codes.

You must keep in mind that the use of aliases is entirely optional, and the definition of lists of states and roles too. They are nothing more than a help to facilitate the identification of constraints. All that matters is that the codes of the roles and application states are understood by the application through the object that implements the interface `IHost`.

When calling `SecurityEnvironment.LoadFrom` or `LoadFormString`, the restrictions defined in the file or text string will take precedence over the ones embedded in the components. If in that text string there is no section relative to a particular component it will continue to apply the security embedded in that component. If you would like to remove all restrictions from a component it would be enough to include an empty paragraph related to that component:

```
;=======================================================
[SECURITYCONTROL=Form3]

;=======================================================
[SECURITYCONTROL=Form4]
[Restrictions]
-0/CheckBox1,E/Button1,E
```


<br /><br />
### Security policy maintenance: `FrmRestrictionsUIDefinition` ###

Settings directly embedded in the component or through configuration files similar to above, can be generated with the maintenance form included, and easily modified by hand if necessary. This form is accessible both at design time and runtime. At design time you can access it by clicking on the button '...' in the property `RestrictionsDefinition` of a component `ControlRestrictedUI`; at runtime using the method `ShowConfigurationSecurityForm` and also directly by pressing a key combination ready for it by means of `SecurityEnvironment.AllowedHotKey` (disabled by default)

![http://restricted-ui.googlecode.com/svn/wiki/images/English/frmrestrictionsuidefinition.gif](http://restricted-ui.googlecode.com/svn/wiki/images/English/frmrestrictionsuidefinition.gif)

Basically what we can do is to select (checkbox) one or more controls, the property or properties to be monitored (Visible, Enabled) and optionally one or more roles together with one or more states. Pressing the button 'Allow' a new restriction (positive) will be added, related to that selection. Clicking on 'Prevent' a negative restriction will be added.

We can define groups of controls, with an associated name, and place restrictions directly to the group. To add or update an existing group we will mark a series of controls and click 'Add'. It will ask us for a name for the group. If it already exists it will be replaced by the new selection, otherwise it will be added.<br />
By selecting one or another group will be selecting in the controls's grid the elements in the group. When you checked the box 'Show only selected Group' then clicking on 'Allow' or 'Prevent', the restriction is directly associated to the corresponding group.

We have the option of saving the restrictions in a configuration file, being added to the restrictions that may already exist for other components. This file will be initially the one established (if any) for the component. We can read the security settings of the component from which we have launched this form, or all that may be included in the file.

We have for convenience two ways of displaying the permissions: tabular or text, both editable.

<br>
<img src='http://restricted-ui.googlecode.com/svn/wiki/images/English/frmRestrictionsUIDefinition_text.gif' />

From this text view, it is also posible to indicate controls with an explicit treatment of the 'deactivation' supervision: controls for which the attempt of making it invisible or disabled should or not be supervised (#Yes=... or #No=...), and so it may or not be prevented.<br>
<br>
<br>
<br>
If we use this form at runtime we can also double click a control to highlight it (it will blink several times). We will also have the possibility to modify (if the object implementing <code>IHost</code> interface permits it) the state and the role or roles associated to this component and instance, so that we can test security. For convenience we can reduce this form to offer only these controls:<br>
<br>
<img src='http://restricted-ui.googlecode.com/svn/wiki/images/English/frmrestrictionsuidefinition_reduced.gif' />


<br /><br />
<hr />
<h2>2.3. Test projects</h2>

Along with the solution three projects are included, two on WinForms and another one on Web: TestWeb, TestWinForms and TestWinForms_notUsingInfragistics.<br>
<br>
The two projects in WinForms are equivalent with the only difference that the latter does not use any control NetAdventage of Infragistics; library of UI controls used in the company I work and that I have included to demonstrate the use of additional factories to deal with special controls.<br>
<br>
WinForm examples include a form (<code>Form1</code>) with which you can play with all the features of this library, including the possibility to pause the security, force visibility or enabled state for a control, or apply a more elaborate logic with the help of <code>BeforeApplyingRestriction</code> event. The form <code>Form1</code> in TestWinForms is as follows:<br>
<br>
<br>
<img src='http://restricted-ui.googlecode.com/svn/wiki/images/English/testform1.gif' />

And <code>Form2</code>:<br>
<br>
<img src='http://restricted-ui.googlecode.com/svn/wiki/images/English/testform2.gif' />

<br />
This second form allows you to verify the treatment of security on user controls: the tabs 1 and 2 have an embedded user control on which a series of restrictions has been applied that make editable only the text box. The tabs 3 and 4 include other user control whose security policy hides a node of the TreeView control.<br>
<br>
Apart from such specific restriction on the second user control, the security component of this form, <code>Form2</code>, applies a restriction on the user control instance of the tab 4 so that the second radiobutton is disabled.<br>
<br>
Both on <code>Form1</code> as on <code>Form2</code> the key combination CTR-ALT-End is enabled to show at runtime the form <code>FrmRestrictionsUIDefinition</code>.<br>
<br>
<br>
<br /><br />
<hr />
<h1>3. Points of Interest</h1>
<ul><li>When developing this code I found interesting to discover the possibilities offered by this type of code "injection" that is able to control or influence the Host application without having to make changes to it (except of course for the normal configuration of the library). This approach has been used to act on a pair of common properties of controls, Visible and Enabled (by preventing its activation according to a security policy), but it could be used in many other cases.</li></ul>

<ul><li>The pattern Indirection through <code>IControlAdapter</code> interface, has been essential in achieving this goal, and it is very powerful. Using it with Factory pattern has also been very convenient.<br>
</li></ul><blockquote>The use of these two patterns has led to the extensibility of this architecture, allowing to add new libraries that would offer a personalized treatment of certain controls</blockquote>

<ul><li>Although it might seem something minor it is interesting to note the use of Special Case pattern, as described by Martin Fowler in <a href='http://martinfowler.com/eaaCatalog/specialCase.html'>P of EAA: Special Case</a> and the clear advantages affecting the code. This pattern is being used by the object <code>NullControlAdapter</code>.</li></ul>

<ul><li>During the development of the component <code>ControlRestrictedUI</code> I had difficulty with the fact that the properties set at design time were initialized by the Designer in alphabetical order. The initialization of the security in a component is done by setting the property <code>RestrictionsDefinition</code>, but at that moment, in its setting by Designer, other required properties were not established. The solution to this problem is provided by the interface <code>ISupportInitialize</code> (<a href='http://en.csharp-online.net/Design-Time_Integration%E2%80%94Batch_Initialization'>Design-Time Integration—Batch Initialization</a>)</li></ul>

<ul><li>It has also been very comfortable and clean the use of <code>INotifyPropertyChanged</code> interface to ensure proper bidirectional data link (DataBindings) between user controls and business entities (<a href='http://www.claassen.net/geek/blog/2007/07/generic-asynchronous.html'>A generic asynchronous INotifyPropertyChanged helper</a>)</li></ul>

<ul><li>A major difficulty of this development has been associated with the component's design time. It was specially problematic the dynamic instantiation of objects <code>IControlAdapterFactory</code> at design time, which caused exceptions <code>InvalidCastException</code>.<br>
</li></ul><blockquote>Among others, the following article helped me understand the problem: <a href='http://www.yoda.arachsys.com/csharp/plugin.html'>Plug-ins and cast exceptions</a>.<br>
I could check later that, as I was instantiating the library to load the auxiliar factory with <code>Assembly.LoadFrom</code> at design time, I was loading it into the default application domain associated with the IDE process, and so this DLL was released only when you closed the IDE, thus having in memory many definitions apparently identical. I solved the problem loading the DLL into a separate application domain, isolated, using <code>AppDomain.LoadFrom</code> instead of <code>Assembly.LoadFrom</code> and unloading the domain once used the factory.</blockquote>

<ul><li>For documenting the classes and methods, I have relied on the code comments and I have used <a href='http://docproject.codeplex.com/'>DocProject</a>, tool built over <a href='http://sandcastle.codeplex.com/'>Sandcastle - Documentation Compiler for Managed Class Libraries</a>. It is a very easy and powerful combination. In particular I have used version 1.11.0 Release Candidate of DocProject and Sandcastle May 2008 Release.