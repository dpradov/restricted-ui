Option Strict On

Imports RestrictedUI
Imports RestrictedWinFormsUI
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


Public Class Form1

    ' Security to apply may vary not only on the type of form or control (where the security component is embedded),
    ' but on the concrete instance of that form or control.
    ' We will distinguish each instance in the form Form1. By default they will be "00"
    Public _InstanceID As String = "00"

    ' En la clase Host (IHost) se depende del ID del componente de seguridad y de la instancia concreta de éste. Con esta 
    ' clase auxiliar podemos interactuar a modo de prueba desde UI. Permite enlazar controles de usuario
    ' para el estado y los roles de la aplicación, asumiendo que éstos se refieren al ID del componente
    ' de seguridad y a la instancia concreta que los crea (lo que se establece en la creación de este objeto Host_Aux)
    Private _hostAux As Host_Aux

    ' Private fields
    Private dtControls As DataTable
    Private ds As New DataSet
    Private aa As New Entity


    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        ' If we want the application to be able to determine a state and independent role or roles for each instance of a 
        ' form, so that we can open several ones simultaneously and these ones may have independent lives, then we must identify each instance:
        ControlRestrictedUIWinForms1.InstanceID = _InstanceID
        lblInstance.Text = _InstanceID

        ' To link controls in this form, as an example, to show and modify the state and user roles, for this ID and instance
        _hostAux = New Host_Aux(MainForm._host, ControlRestrictedUIWinForms1.ID, _InstanceID)


        CreateDataTableControls()

        'aa.a = "10"
        'aa.b = 10
        'Me.EntidadBindingSource.DataSource = aa
        Me.EntidadBindingSource.DataSource = ds
        Me.EntidadBindingSource.DataMember = "Controls"

        ' If we have created controls dynamically then we can update the controls file (if we are developing we might want
        ' to be allowed to establish security over controls available only in runtime)
#If DEBUG Then
        ' To update the controls file and thus be able to consider those controls when editting the security in design time.
        ControlRestrictedUIWinForms1.RegisterControls()
#End If

        ' In any case, if we dynamically created controls (DataGridView columns)they will not have been 
        ' localized when security initializes. So we will force a reinitilization of the security:
        ControlRestrictedUIWinForms1.ReinitializeSecurity()

        ' Easily display and modify property RestrictionsDefinition (of the security component) and the status and roles of the application
        ' (for this form and instance) binding with the security comonent and with Host_Aux.
        ' The bidirectional data link is achieved because both objects implement the INotifyPropertyChanged interface,
        ' so changes to properties on that objects are communicated to the bound controls.
        Me.txtRestrictions.DataBindings.Add("Lines", ControlRestrictedUIWinForms1, "RestrictionsDefinition")
        Me.txtState.DataBindings.Add("Text", _hostAux, "State")
        Me.txtRoles.DataBindings.Add("Text", _hostAux, "StrUserRoles")

    End Sub

    Private Sub CreateDataTableControls()
        Dim c As DataColumn
        dtControls = New DataTable("Controls")
        c = dtControls.Columns.Add("Sel", GetType(Boolean))
        c.DefaultValue = False
        dtControls.Columns.Add("IDControl", GetType(String))
        dtControls.Columns.Add("Name", GetType(String))
        dtControls.Columns.Add("Type", GetType(String))
        dtControls.Columns.Add("Groups", GetType(String))
        ds.Tables.Add(dtControls)

        Dim r As DataRow = dtControls.NewRow
        r("IDControl") = "ID1"
        r("Name") = "Name1"
        r("Type") = "Type1"
        r("Groups") = ""
        dtControls.Rows.Add(r)
        r = dtControls.NewRow
        r("IDControl") = "ID2"
        r("Name") = "Name2"
        r("Type") = "Type2"
        r("Groups") = ""
        dtControls.Rows.Add(r)

        cControls.DataSource = ds
        cControls.DataMember = "Controls"
    End Sub

    ''' <summary>
    ''' Method to open the security maintenance form at runtime
    ''' </summary>
    Private Sub btnRestrictionsMngt_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRestrictionsMngt.Click
        ControlRestrictedUIWinForms1.ShowConfigurationSecurityForm(MainForm._host)

        ' The definition could also be forced to be offered as a starting point:
        'ControlRestrictedUIWinForms1.ShowConfigurationSecurityForm(MainForm._host, txtRestrictions.Lines)
    End Sub

#Region "Methods to force the Visible and Enabled properties and thus verify the operation of the Security library"

    Private Sub btnEnableVisible_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEnableVisible.Click
        Dim adapt As IControlAdapter
        Me.combo.Visible = True
        Me.TextBox.Visible = True
        'Me.GroupBox1.Visible = True
        Me.TextBox2.Visible = True
        Me.CheckBox1.Visible = True

        'TreeView1.Visible = True

        ' TreeNodes cannot be made invisible/visible directly with the control. That is something
        ' offered by adapter
        adapt = New AdapterWinForms_TreeView(TreeView1)
        For Each c As IControlAdapter In adapt.Controls
            c.Visible = True
        Next
        ' Also:
        adapt.FindControl("Node3").Visible = True
        adapt.FindControl("Node0.Node1").Visible = True
        adapt.FindControl("Node0.Node2").Visible = True
        adapt.FindControl("Node3.Node4").Visible = True
        adapt.FindControl("Node3.Node4.Node5").Visible = True

        'cControles.Visible = True
        adapt = New AdapterWinForms_DataGridView(cControls)
        For Each c As IControlAdapter In adapt.Controls
            c.Visible = True
        Next
        ' Also directly with the control:
        cControls.Columns(0).Visible = True
        cControls.Columns(1).Visible = True
        cControls.Columns(2).Visible = True
        cControls.Columns(3).Visible = True
    End Sub

    Private Sub btnEnableEnabled_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEnableEnabled.Click
        Dim adapt As IControlAdapter
        'Me.GroupBox1.Enabled = True
        Me.combo.Enabled = True
        Me.TextBox.Enabled = True
        Me.TextBox2.Enabled = True
        Me.CheckBox1.Enabled = True

        TreeView1.Enabled = True
        ' TreeNodes cannot be made disabled with the control.
        cControls.Enabled = True
        cControls.ReadOnly = False
        adapt = New AdapterWinForms_DataGridView(cControls)
        For Each c As IControlAdapter In adapt.Controls
            c.Enabled = True
        Next
        ' Also directly with the control:
        cControls.Columns(0).ReadOnly = False
        cControls.Columns(1).ReadOnly = False
        cControls.Columns(2).ReadOnly = False
        cControls.Columns(3).ReadOnly = False
    End Sub

    Private Sub btnDisableVisible_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisableVisible.Click
        Dim adapt As IControlAdapter
        'Me.GroupBox1.Visible = False
        Me.combo.Visible = False
        Me.TextBox.Visible = False
        Me.TextBox2.Visible = False
        Me.CheckBox1.Visible = False

        'TreeView1.Visible = False
        ' TreeNodes cannot be made invisible/visible directly with the control. That is something
        ' offered by adapter
        adapt = New AdapterWinForms_TreeView(TreeView1)
        For Each c As IControlAdapter In adapt.Controls
            c.Visible = False
        Next
        'Also:
        adapt.FindControl("Node0.Node1").Visible = False
        adapt.FindControl("Node0.Node2").Visible = False
        adapt.FindControl("Node3.Node4").Visible = False
        adapt.FindControl("Node3.Node4.Node5").Visible = False

        'cControles.Visible = False
        adapt = New AdapterWinForms_DataGridView(cControls)
        For Each c As IControlAdapter In adapt.Controls
            c.Visible = False
        Next
        ' Also directly with the control:
        cControls.Columns(0).Visible = False
        cControls.Columns(1).Visible = False
        cControls.Columns(2).Visible = False
        cControls.Columns(3).Visible = False
    End Sub

    Private Sub btnDisableEnabled_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisableEnabled.Click
        Dim adapt As IControlAdapter
        Me.combo.Enabled = False
        Me.TextBox.Enabled = False
        'Me.GroupBox1.Enabled = False
        Me.TextBox2.Enabled = False
        Me.CheckBox1.Enabled = False

        TreeView1.Enabled = False
        'cControles.Enabled = False
        adapt = New AdapterWinForms_DataGridView(cControls)
        For Each c As IControlAdapter In adapt.Controls
            c.Enabled = False
        Next
        ' Also directly with the control:
        cControls.Columns(0).ReadOnly = True
        cControls.Columns(1).ReadOnly = True
        cControls.Columns(2).ReadOnly = True
        cControls.Columns(3).ReadOnly = True
    End Sub

    Private Sub btnEnableVisible_N_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEnableVisible_N.Click
        For i As Integer = 0 To 10
            btnEnableVisible_Click(Nothing, Nothing)
        Next
    End Sub

#End Region

#Region "Test of actions to perform on the security component"
    ''' <summary>
    ''' Changes the value of the property SuperviseDeactivation
    ''' </summary>
    ''' <remarks>
    ''' <para>By the use of the <see cref="ControlRestrictedUI.SuperviseDeactivation"/> property now we can supervise 
    ''' also the deactivation of properties (the attempt to make invisible or disabled a control).</para>
    ''' <para>If we must supervise the deactivation of a property we will assume that if the activation is allowed 
    ''' then there is no reason to make invisible or disabled the control, and so the deactivation will not be allowed.</para>
    ''' </remarks>
    Private Sub cbSuperviseDeactivation_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbSuperviseDeactivation.CheckedChanged
        ControlRestrictedUIWinForms1.SuperviseDeactivation = cbSuperviseDeactivation.Checked
    End Sub

    ''' <summary>
    ''' Test of the property Pause on the security component
    ''' </summary>
    ''' <remarks>
    ''' From the description of that property:
    ''' "Temporarily disables (pauses) the security policy imposed by the constraints of the component, so that
    ''' so that subsequent changes in monitored properties (Visible and Enabled) are permitted."
    ''' "Resetting to False (initial value) the definition of security is restored: some controls are disabled or hidden accordingly"
    ''' </remarks>
    Private Sub cbPaused_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbPaused.Click
        ControlRestrictedUIWinForms1.Paused = cbPaused.Checked
    End Sub

    ''' <summary>
    ''' Verification of the functionality of ForceVisibility on the TextBox control
    ''' </summary>
    Private Sub btnForceVisibility_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnForceVisibility.Click
        ControlRestrictedUIWinForms1.ForceVisibility(TextBox)
    End Sub

    ''' <summary>
    ''' Verification of the functionality of ForceEnabled on the TextBox control
    ''' </summary>
    Private Sub btnForceEnabled_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnForceEnabled.Click
        ControlRestrictedUIWinForms1.ForceEnabled(TextBox)
    End Sub

    ''' <summary>
    ''' Verification of the functionality of ExcludeControl on the TextBox control
    ''' </summary>
    Private Sub btnExcludeControl_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExcludeControl.Click
        ControlRestrictedUIWinForms1.ExcludeControl(TextBox)
    End Sub

    ''' <summary>
    ''' Sometimes it may be of interest to force a reconsideration of security, for example if we added controls dynamically
    ''' </summary>
    Private Sub btnReinitialize_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnReinitialize.Click
        ControlRestrictedUIWinForms1.ReinitializeSecurity()
    End Sub


    ''' <summary>
    ''' Activation / deactivation of the constraint additional logic, as an example of use of the event available: BeforeApplyingRestriction
    ''' </summary>
    Private Sub cbAditionalLogicOfRestriction_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbAditionalLogicOfRestriction.CheckedChanged
        If cbAditionalLogicOfRestriction.Checked Then
            AddHandler ControlRestrictedUIWinForms1.BeforeApplyingRestriction, AddressOf OnBeforeApplyingRestriction
        Else
            RemoveHandler ControlRestrictedUIWinForms1.BeforeApplyingRestriction, AddressOf OnBeforeApplyingRestriction
        End If
    End Sub

    ''' <summary>
    ''' Example of use of the BeforeApplyingRestriction event, as an option to allow or prevent the 
    ''' change on the basis of a more complex logic:
    ''' We only allow changes on CheckBox1 if, in addition to the security restrictions imposed, 
    ''' the contents of the controls TextBox and TextoBox2 are the same.
    ''' </summary>
    ''' <remarks>
    ''' Note: Changes in TextBox1 or TextBox2 don't trigger automatically the visible or enabled state of the CheckBox1. This extra
    ''' condition is analyzed only when there is a modification in the conditions (application state, user roles, restrictions definition..)
    ''' that makes necessary to reconsider the security applied, and of course, when it is tried to make CheckBox1 visible.
    ''' </remarks>
    Private Sub OnBeforeApplyingRestriction(ByVal adaptControl As IControlAdapter, ByVal tipo As RestrictedUI.ControlRestrictedUI.TChange, ByRef permitirCambio As Boolean)
        If permitirCambio And adaptControl.Control Is CheckBox1 Then
            If TextBox.Text <> TextBox2.Text Then
                permitirCambio = False
            End If
        End If
    End Sub

    Private Sub cbUseReadOnly_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbUseReadOnly.CheckedChanged

        AdapterWinForms_DataGridView.UseReadOnly = cbUseReadOnly.Checked

    End Sub

#End Region

#Region "Tests on TreeView and Control adapters"

    Private Sub btnTest_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTest.Click
        test_WinForms()
        test_TreeView()
    End Sub

    Private Sub test_WinForms()
        Dim a As IControlAdapter
        Dim a2 As IControlAdapter
        Dim a3 As IControlAdapter

        a = SecurityEnvironment.GetAdapter(Me)
        For Each elem As IControlAdapter In a.Controls
            Debug.Print(elem.Identification)
        Next

        a2 = a.FindControl("combo")
        a2 = a.FindControl("GroupBox1")

        For Each elem As IControlAdapter In a2.Controls
            Debug.Print(elem.Identification)
        Next

        a2 = a.FindControl("GroupBox1.TextBox2")
        a2 = a.FindControl("UltraGrid1")
        Debug.Print(a2.Identification)
        a2 = a.FindControl("GroupBox1.UltraGrid1")
        Debug.Print(a2.Identification)
        a3 = a.FindControl("GroupBox1.UltraGrid1.0.a")
        Debug.Print(a3.Identification)
        Debug.Print(a3.Identification(a2))
    End Sub

    Private Sub test_TreeView()
        Dim control As TreeView = Me.TreeView1

        Dim a As IControlAdapter
        Dim a2 As IControlAdapter
        Dim a3 As IControlAdapter

        a = New AdapterWinForms_TreeView(control)
        a.Visible = False
        MsgBox(a.Identification)
        a.Visible = True
        For Each elem As IControlAdapter In a.Controls
            Debug.Print(elem.Identification)
        Next

        'a = New AdaptadorControlTreeNode(Me.TreeView1.Nodes(0))

        MsgBox("Searching NON_EXISTENT_Node")
        a2 = a.FindControl("NON_EXISTENT_Node")
        a2 = a.FindControl("Node0")
        a2 = a2.FindControl("Node2")
        a2.SuperviseVisible(Me.ControlRestrictedUIWinForms1)
        a2.Visible = False

        ' Comprobamos que realmente se oculta y muestra el control
        MsgBox(a2.Identification)
        a2.Visible = True
        MsgBox(a2.Identification)

        ' Vamos a recorrer los controles de Nodo0, teniendo en cuenta que uno de ellos (Nodo2)
        ' estará oculto, por lo que habrá sido eliminado del TreeView..
        a2.Visible = False
        a2 = a.FindControl("Node0")
        For Each elem As IControlAdapter In a2.Controls
            Debug.Print(elem.Identification)
        Next

        a2 = a.FindControl("Node0.Node2")
        Debug.Print(a2.Identification)
        a2.SuperviseVisible(Me.ControlRestrictedUIWinForms1)
        a2.Visible = True

        ' We are looking for a node that hangs from one hidden
        MsgBox("Searching Node3.Node4")
        a2 = a.FindControl("Node3.Node4")
        a2.SuperviseVisible(Me.ControlRestrictedUIWinForms1)
        a2.Visible = False
        MsgBox(a2.Identification)
        MsgBox("Searching Node3.Node4.Node5")
        a3 = a.FindControl("Node3.Node4.Node5")
        Debug.Print(a3.Visible.ToString)
        Debug.Print(a3.Identification)
        a3.SuperviseVisible(Me.ControlRestrictedUIWinForms1)
        a3.Visible = False
        MsgBox(a3.Identification)
        a2.Visible = True
        MsgBox(a3.Identification)
        a3.Visible = True
        MsgBox("END")
    End Sub

#End Region

    ''' <summary>
    ''' Method to test the performance overhead of this library when there is many changes that force the
    ''' revision of the security applied.
    ''' Will be forced 1000! state changes
    ''' </summary>
    Private Sub btnChangeState_N_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnChangeState_N.Click
        Dim dt1 As DateTime
        Dim dt2 As DateTime
        Dim diff As TimeSpan
        Dim r As New Random()

        dt1 = Now
        For i As Integer = 1 To 500
            _hostAux.State = r.Next(4, 6)
            _hostAux.State = r.Next(3)
        Next
        dt2 = Now
        diff = dt2 - dt1
        MsgBox("1000 state changes in " + diff.TotalMilliseconds.ToString() + " ms")
    End Sub
End Class
