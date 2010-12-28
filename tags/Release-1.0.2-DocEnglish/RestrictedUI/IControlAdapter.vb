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
''' It indicates that the object can wrap a 'control' isolating its peculiarities and thus allowing its use in the
''' security environment.
''' </summary>
''' <remarks>
''' <para>Wraps a 'control' with the aim of providing an uniform access to its Enabled and Visible properties and its
''' 'controls' son, also allowing to provide oversight of the change of these properties, giving option to prevent such 
''' changes based on a security scheme.</para>
''' <para>What the adapter wraps needs not to be an object of a particular class, it can be anything that the adapter 
''' 'understands', on which it is possible and interesting to control the visibility and or enabled state, and that an
''' adapter 'father' has recognized it as a son.</para>
''' 
''' <para>Actually controlled properties need not be exactly 'Visible' and 'Enabled'; it is the responsibility of the 
''' control adapter to offer that interface and to act on the properties that the control has (for example, some controls
''' do not offer Enabled but ReadOnly)</para>
''' <para>It would be possible, for example, to considerar the elements of a ListBox as controls to supervise. The way the properties
''' Visible and Enabled (or only one of them) are implemented will depend on each control. These implementations can considerar,
''' e.g, the elimination (saving first) and insertion of one of the items. Something similar has been realized in the control
''' TreeView (fromAdapterWinForms_TreeView and AdapterWinForms_TreeNode)</para>
''' 
''' <para>The controls provided in the maintenance of security form (<see cref="FrmRestrictionsUIDefinition"/>) and the controls 
''' where the component <see cref="ControlRestrictedUI" /> seeks are those offered as 'children' for the control in which is embedded 
''' the component. The adapter for this parent control will be asking to their child controls' adapters, which will discover 
''' new controls. So, if you have an adapter that understand controls DataGridView, for example, it is possible to provide 
''' each of the columns as a child to monitor. Thus, the number of controls to monitor depends on the adapters with which 
''' to work, and this depends in turn on the factories (see <see cref="IControlAdapterFactory"/>) that have been incorporated</para>
''' </remarks>
Public Interface IControlAdapter
    ReadOnly Property Identification(Optional ByVal parent As IControlAdapter = Nothing, Optional ByVal security As ControlRestrictedUI = Nothing) As String

    ''' <summary>
    ''' Gets or sets a value that determines the visibility of the associated control (wrapped by this adapter)
    ''' </summary>
    ''' <remarks>
    ''' <para>Actually controlled property need not be exactly 'Visible'; it is the responsibility of the 
    ''' control adapter to offer that interface and to act on the properties that the control has (for example, some controls
    ''' do not offer Enabled but ReadOnly)</para>
    ''' <para>
    ''' Important: Although the host application normally should use directly the properties in the control to change its
    ''' visibility, it is also possible do it through the use of an IControlAdapter. So, changes on this property on the adaptar should 
    ''' also be controlled according to the security policy.
    ''' </para>
    ''' </remarks>
    Property Visible() As Boolean

    ''' <summary>
    ''' Gets or sets a value that enables or disables the associated control (wrapped by this adapter)
    ''' </summary>
    ''' <remarks>
    ''' <para>Actually controlled property need not be exactly 'Enabled'; it is the responsibility of the 
    ''' control adapter to offer that interface and to act on the properties that the control has (for example, some controls
    ''' do not offer Enabled but ReadOnly)</para>
    ''' <para>
    ''' Important: Although the host application normally should use directly the properties in the control to change its
    ''' enabled state, it is also possible do it through the use of an IControlAdapter. So, changes on this property on the adaptar should 
    ''' also be controlled according to the security policy.
    ''' </para>
    ''' </remarks>
    Property Enabled() As Boolean

    ''' <summary>
    ''' It forces the component provided as parameter, to monitor the property Visible of the associated control (wrapped by this adapter),
    ''' so that it can prevent the visibility of that control depending on the security policy.
    ''' </summary>
    Sub SuperviseVisible(ByVal compSecurity As ControlRestrictedUI)


    ''' <summary>
    ''' It forces the component provided as parameter, to monitor the property Enabled of the associated control (wrapped by this adapter),
    ''' so that it can prevent that control to be enabled depending on the security policy.
    ''' </summary>
    Sub SuperviseEnabled(ByVal compSecurity As ControlRestrictedUI)


    ''' <summary>
    ''' Finalizes the monitoring of the properties Enabled and Visible, if they were initiated, so any change
    ''' on that properties of the associated control (wrapped by this adapter) can't be prevented by a security
    ''' component.
    ''' </summary>
    Sub FinalizeSupervision()

    ''' <summary>
    ''' Returns the son 'controls' of the associated control, being also wrapped these controls with IControlAdapter objects.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' The wrapped objects returned need not be of type Control, they can be any element that can be considered 
    ''' as this control's child, like a detail or component of this control that could be interesting (and viable) to be controlled on
    ''' its visibility and enabled state.
    ''' </remarks>
    Function Controls() As IList(Of IControlAdapter)

    ''' <summary>
    ''' Returns the 'control' (wrapped by an object IControlAdapter) indicated with a full identification (<paramref name="id"/>), 
    ''' accessible to the 'control' associated with this adapter.
    ''' </summary>
    ''' <param name="id">Identification of the 'control' to look for.</param>
    ''' <returns></returns>
    ''' <remarks>
    ''' <para>The param <paramref name="id"/> should include the identification of all the ancestors
    ''' of the control to look for, visibles from the control on with this method is executing (via its adapter)</para>
    ''' <para>For example, if the adapter executing <c>FindControl</c> corresponds to a form with a groupbox control named <c>GroupBox1</c>,
    ''' wich in turn has a label named <c>Label1</c>, to find the label the identification should be "<c>GroupBox1.Label1</c>". If the
    ''' search is done over the groupbox control then the identification of that label should be simply "<c>Label1</c>".
    ''' </para>
    ''' <para>The search of the control should be case insensitive</para>
    ''' </remarks>
    Function FindControl(ByVal id As String) As IControlAdapter

    ''' <summary>
    ''' Indicates whether the adapter is the special case 'null', ie, it does not wrapped any control
    ''' </summary>
    ''' <remarks>(According to the pattern 'Special Case', as described by Martin Fowler: http://martinfowler.com/eaaCatalog/specialCase.html)</remarks>
    ReadOnly Property IsNull() As Boolean

    ''' <summary>
    ''' Gets the 'control' wrapped by this adapter
    ''' </summary>
    ReadOnly Property Control() As Object
End Interface
