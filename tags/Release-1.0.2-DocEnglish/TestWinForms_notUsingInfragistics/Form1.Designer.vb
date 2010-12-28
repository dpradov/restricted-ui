<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form reemplaza a Dispose para limpiar la lista de componentes.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Requerido por el Diseñador de Windows Forms
    Private components As System.ComponentModel.IContainer

    'NOTA: el Diseñador de Windows Forms necesita el siguiente procedimiento
    'Se puede modificar usando el Diseñador de Windows Forms.  
    'No lo modifique con el editor de código.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim TreeNode1 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Node1")
        Dim TreeNode2 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Node2")
        Dim TreeNode3 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Node0", New System.Windows.Forms.TreeNode() {TreeNode1, TreeNode2})
        Dim TreeNode4 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Node5")
        Dim TreeNode5 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Node4", New System.Windows.Forms.TreeNode() {TreeNode4})
        Dim TreeNode6 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Node3", New System.Windows.Forms.TreeNode() {TreeNode5})
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Me.TextBox = New System.Windows.Forms.TextBox
        Me.combo = New System.Windows.Forms.ComboBox
        Me.btnEnableVisible = New System.Windows.Forms.Button
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.CheckBox1 = New System.Windows.Forms.CheckBox
        Me.TextBox2 = New System.Windows.Forms.TextBox
        Me.EntidadBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.btnEnableEnabled = New System.Windows.Forms.Button
        Me.btnDisableVisible = New System.Windows.Forms.Button
        Me.btnEnableVisible_N = New System.Windows.Forms.Button
        Me.btnTest = New System.Windows.Forms.Button
        Me.txtState = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.txtRoles = New System.Windows.Forms.TextBox
        Me.btnDisableEnabled = New System.Windows.Forms.Button
        Me.Label4 = New System.Windows.Forms.Label
        Me.txtRestrictions = New System.Windows.Forms.TextBox
        Me.TreeView1 = New System.Windows.Forms.TreeView
        Me.Label6 = New System.Windows.Forms.Label
        Me.gbCommands = New System.Windows.Forms.GroupBox
        Me.cbSuperviseDeactivation = New System.Windows.Forms.CheckBox
        Me.btnChangeState_N = New System.Windows.Forms.Button
        Me.btnReinitialize = New System.Windows.Forms.Button
        Me.cbUseReadOnly = New System.Windows.Forms.CheckBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.cbAditionalLogicOfRestriction = New System.Windows.Forms.CheckBox
        Me.lblInstance = New System.Windows.Forms.Label
        Me.btnExcludeControl = New System.Windows.Forms.Button
        Me.btnForceEnabled = New System.Windows.Forms.Button
        Me.btnForceVisibility = New System.Windows.Forms.Button
        Me.cbPaused = New System.Windows.Forms.CheckBox
        Me.btnRestrictionsMngt = New System.Windows.Forms.Button
        Me.cControls = New System.Windows.Forms.DataGridView
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip
        Me.ToolStripButton1 = New System.Windows.Forms.ToolStripButton
        Me.ToolStripSplitButton1 = New System.Windows.Forms.ToolStripSplitButton
        Me.ToolStripComboBox1 = New System.Windows.Forms.ToolStripComboBox
        Me.ToolStripTextBox1 = New System.Windows.Forms.ToolStripTextBox
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip
        Me.FileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.CloseToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem
        Me.Option1ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.Option2ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.EditToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.CutToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.CopyToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.PasteToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripComboBox2 = New System.Windows.Forms.ToolStripComboBox
        Me.ControlRestrictedUIWinForms1 = New RestrictedWinFormsUI.ControlRestrictedUIWinForms(Me.components)
        Me.GroupBox1.SuspendLayout()
        CType(Me.EntidadBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.gbCommands.SuspendLayout()
        CType(Me.cControls, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ToolStrip1.SuspendLayout()
        Me.MenuStrip1.SuspendLayout()
        CType(Me.ControlRestrictedUIWinForms1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'TextBox
        '
        Me.TextBox.Location = New System.Drawing.Point(536, 352)
        Me.TextBox.Name = "TextBox"
        Me.TextBox.Size = New System.Drawing.Size(190, 20)
        Me.TextBox.TabIndex = 1
        Me.TextBox.Text = "TextBox"
        '
        'combo
        '
        Me.combo.FormattingEnabled = True
        Me.combo.Items.AddRange(New Object() {"Linea una", "Línea Dos"})
        Me.combo.Location = New System.Drawing.Point(536, 378)
        Me.combo.Name = "combo"
        Me.combo.Size = New System.Drawing.Size(190, 21)
        Me.combo.TabIndex = 2
        Me.combo.Text = "combo"
        '
        'btnEnableVisible
        '
        Me.btnEnableVisible.Location = New System.Drawing.Point(265, 179)
        Me.btnEnableVisible.Name = "btnEnableVisible"
        Me.btnEnableVisible.Size = New System.Drawing.Size(124, 23)
        Me.btnEnableVisible.TabIndex = 3
        Me.btnEnableVisible.Text = "Make Visible"
        Me.btnEnableVisible.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.CheckBox1)
        Me.GroupBox1.Controls.Add(Me.TextBox2)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 342)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(235, 207)
        Me.GroupBox1.TabIndex = 4
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "GroupBox1"
        '
        'CheckBox1
        '
        Me.CheckBox1.AutoSize = True
        Me.CheckBox1.Location = New System.Drawing.Point(23, 31)
        Me.CheckBox1.Name = "CheckBox1"
        Me.CheckBox1.Size = New System.Drawing.Size(81, 17)
        Me.CheckBox1.TabIndex = 28
        Me.CheckBox1.Text = "CheckBox1"
        Me.CheckBox1.UseVisualStyleBackColor = True
        '
        'TextBox2
        '
        Me.TextBox2.Location = New System.Drawing.Point(23, 58)
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.Size = New System.Drawing.Size(190, 20)
        Me.TextBox2.TabIndex = 1
        Me.TextBox2.Text = "TextBox2"
        '
        'EntidadBindingSource
        '
        Me.EntidadBindingSource.DataSource = GetType(TestWinForms.Entity)
        '
        'btnEnableEnabled
        '
        Me.btnEnableEnabled.Location = New System.Drawing.Point(95, 179)
        Me.btnEnableEnabled.Name = "btnEnableEnabled"
        Me.btnEnableEnabled.Size = New System.Drawing.Size(116, 23)
        Me.btnEnableEnabled.TabIndex = 6
        Me.btnEnableEnabled.Text = "Enable controls"
        Me.btnEnableEnabled.UseVisualStyleBackColor = True
        '
        'btnDisableVisible
        '
        Me.btnDisableVisible.Location = New System.Drawing.Point(265, 208)
        Me.btnDisableVisible.Name = "btnDisableVisible"
        Me.btnDisableVisible.Size = New System.Drawing.Size(124, 23)
        Me.btnDisableVisible.TabIndex = 11
        Me.btnDisableVisible.Text = "Make Invisible"
        Me.btnDisableVisible.UseVisualStyleBackColor = True
        '
        'btnEnableVisible_N
        '
        Me.btnEnableVisible_N.Location = New System.Drawing.Point(392, 179)
        Me.btnEnableVisible_N.Name = "btnEnableVisible_N"
        Me.btnEnableVisible_N.Size = New System.Drawing.Size(58, 23)
        Me.btnEnableVisible_N.TabIndex = 12
        Me.btnEnableVisible_N.Text = "N times Enable Visibility"
        Me.btnEnableVisible_N.UseVisualStyleBackColor = True
        '
        'btnTest
        '
        Me.btnTest.Location = New System.Drawing.Point(750, 65)
        Me.btnTest.Name = "btnTest"
        Me.btnTest.Size = New System.Drawing.Size(82, 35)
        Me.btnTest.TabIndex = 13
        Me.btnTest.Text = "Test Adapters"
        Me.btnTest.UseVisualStyleBackColor = True
        Me.btnTest.Visible = False
        '
        'txtState
        '
        Me.txtState.BackColor = System.Drawing.SystemColors.Info
        Me.txtState.Location = New System.Drawing.Point(95, 27)
        Me.txtState.Name = "txtState"
        Me.txtState.Size = New System.Drawing.Size(80, 20)
        Me.txtState.TabIndex = 14
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(45, 30)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(32, 13)
        Me.Label1.TabIndex = 15
        Me.Label1.Text = "State"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(194, 30)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(54, 13)
        Me.Label2.TabIndex = 17
        Me.Label2.Text = "User roles"
        '
        'txtRoles
        '
        Me.txtRoles.BackColor = System.Drawing.SystemColors.Info
        Me.txtRoles.Location = New System.Drawing.Point(285, 27)
        Me.txtRoles.Name = "txtRoles"
        Me.txtRoles.Size = New System.Drawing.Size(80, 20)
        Me.txtRoles.TabIndex = 16
        '
        'btnDisableEnabled
        '
        Me.btnDisableEnabled.Location = New System.Drawing.Point(95, 208)
        Me.btnDisableEnabled.Name = "btnDisableEnabled"
        Me.btnDisableEnabled.Size = New System.Drawing.Size(116, 23)
        Me.btnDisableEnabled.TabIndex = 18
        Me.btnDisableEnabled.Text = "Disable controls"
        Me.btnDisableEnabled.UseVisualStyleBackColor = True
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(16, 68)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(62, 13)
        Me.Label4.TabIndex = 23
        Me.Label4.Text = "Restrictions"
        '
        'txtRestrictions
        '
        Me.txtRestrictions.BackColor = System.Drawing.SystemColors.Info
        Me.txtRestrictions.Location = New System.Drawing.Point(95, 65)
        Me.txtRestrictions.Multiline = True
        Me.txtRestrictions.Name = "txtRestrictions"
        Me.txtRestrictions.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtRestrictions.Size = New System.Drawing.Size(636, 106)
        Me.txtRestrictions.TabIndex = 22
        '
        'TreeView1
        '
        Me.TreeView1.Location = New System.Drawing.Point(256, 370)
        Me.TreeView1.Name = "TreeView1"
        TreeNode1.Name = "Node1"
        TreeNode1.Text = "Node1"
        TreeNode2.Name = "Node2"
        TreeNode2.Text = "Node2"
        TreeNode3.Name = "Node0"
        TreeNode3.Text = "Node0"
        TreeNode4.Name = "Node5"
        TreeNode4.Text = "Node5"
        TreeNode5.Name = "Node4"
        TreeNode5.Text = "Node4"
        TreeNode6.Name = "Node3"
        TreeNode6.Text = "Node3"
        Me.TreeView1.Nodes.AddRange(New System.Windows.Forms.TreeNode() {TreeNode3, TreeNode6})
        Me.TreeView1.Size = New System.Drawing.Size(125, 176)
        Me.TreeView1.TabIndex = 25
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(253, 352)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(52, 13)
        Me.Label6.TabIndex = 27
        Me.Label6.Text = "TreeView"
        '
        'gbCommands
        '
        Me.gbCommands.Controls.Add(Me.cbSuperviseDeactivation)
        Me.gbCommands.Controls.Add(Me.btnChangeState_N)
        Me.gbCommands.Controls.Add(Me.btnReinitialize)
        Me.gbCommands.Controls.Add(Me.cbUseReadOnly)
        Me.gbCommands.Controls.Add(Me.Label3)
        Me.gbCommands.Controls.Add(Me.cbAditionalLogicOfRestriction)
        Me.gbCommands.Controls.Add(Me.lblInstance)
        Me.gbCommands.Controls.Add(Me.btnExcludeControl)
        Me.gbCommands.Controls.Add(Me.btnForceEnabled)
        Me.gbCommands.Controls.Add(Me.btnForceVisibility)
        Me.gbCommands.Controls.Add(Me.cbPaused)
        Me.gbCommands.Controls.Add(Me.btnRestrictionsMngt)
        Me.gbCommands.Controls.Add(Me.btnEnableVisible)
        Me.gbCommands.Controls.Add(Me.btnEnableEnabled)
        Me.gbCommands.Controls.Add(Me.btnDisableVisible)
        Me.gbCommands.Controls.Add(Me.btnEnableVisible_N)
        Me.gbCommands.Controls.Add(Me.Label4)
        Me.gbCommands.Controls.Add(Me.btnTest)
        Me.gbCommands.Controls.Add(Me.txtRestrictions)
        Me.gbCommands.Controls.Add(Me.txtState)
        Me.gbCommands.Controls.Add(Me.Label1)
        Me.gbCommands.Controls.Add(Me.txtRoles)
        Me.gbCommands.Controls.Add(Me.btnDisableEnabled)
        Me.gbCommands.Controls.Add(Me.Label2)
        Me.gbCommands.Location = New System.Drawing.Point(12, 50)
        Me.gbCommands.Name = "gbCommands"
        Me.gbCommands.Size = New System.Drawing.Size(863, 285)
        Me.gbCommands.TabIndex = 28
        Me.gbCommands.TabStop = False
        Me.gbCommands.Text = "gbCommands"
        '
        'cbSuperviseDeactivation
        '
        Me.cbSuperviseDeactivation.AutoSize = True
        Me.cbSuperviseDeactivation.Location = New System.Drawing.Point(469, 14)
        Me.cbSuperviseDeactivation.Name = "cbSuperviseDeactivation"
        Me.cbSuperviseDeactivation.Size = New System.Drawing.Size(136, 17)
        Me.cbSuperviseDeactivation.TabIndex = 37
        Me.cbSuperviseDeactivation.Text = "Supervise Deactivation"
        Me.ToolTip1.SetToolTip(Me.cbSuperviseDeactivation, resources.GetString("cbSuperviseDeactivation.ToolTip"))
        Me.cbSuperviseDeactivation.UseVisualStyleBackColor = True
        '
        'btnChangeState_N
        '
        Me.btnChangeState_N.Location = New System.Drawing.Point(177, 25)
        Me.btnChangeState_N.Name = "btnChangeState_N"
        Me.btnChangeState_N.Size = New System.Drawing.Size(68, 23)
        Me.btnChangeState_N.TabIndex = 36
        Me.btnChangeState_N.Text = "Random N"
        Me.ToolTip1.SetToolTip(Me.btnChangeState_N, "Changes state 1000 times randomly between 0 and 4")
        Me.btnChangeState_N.UseVisualStyleBackColor = True
        '
        'btnReinitialize
        '
        Me.btnReinitialize.Location = New System.Drawing.Point(469, 35)
        Me.btnReinitialize.Name = "btnReinitialize"
        Me.btnReinitialize.Size = New System.Drawing.Size(124, 23)
        Me.btnReinitialize.TabIndex = 35
        Me.btnReinitialize.Text = "Reinitialize Security"
        Me.btnReinitialize.UseVisualStyleBackColor = True
        '
        'cbUseReadOnly
        '
        Me.cbUseReadOnly.AutoSize = True
        Me.cbUseReadOnly.Checked = True
        Me.cbUseReadOnly.CheckState = System.Windows.Forms.CheckState.Checked
        Me.cbUseReadOnly.Location = New System.Drawing.Point(97, 240)
        Me.cbUseReadOnly.Name = "cbUseReadOnly"
        Me.cbUseReadOnly.Size = New System.Drawing.Size(95, 17)
        Me.cbUseReadOnly.TabIndex = 34
        Me.cbUseReadOnly.Text = "Use ReadOnly"
        Me.ToolTip1.SetToolTip(Me.cbUseReadOnly, "Use ReadOnly instead of Enabled when it is possible (DataGridView)")
        Me.cbUseReadOnly.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(617, 15)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(51, 13)
        Me.Label3.TabIndex = 33
        Me.Label3.Text = "Instance:"
        '
        'cbAditionalLogicOfRestriction
        '
        Me.cbAditionalLogicOfRestriction.AutoSize = True
        Me.cbAditionalLogicOfRestriction.Location = New System.Drawing.Point(601, 266)
        Me.cbAditionalLogicOfRestriction.Name = "cbAditionalLogicOfRestriction"
        Me.cbAditionalLogicOfRestriction.Size = New System.Drawing.Size(125, 17)
        Me.cbAditionalLogicOfRestriction.TabIndex = 32
        Me.cbAditionalLogicOfRestriction.Text = "Additional Restriction"
        Me.ToolTip1.SetToolTip(Me.cbAditionalLogicOfRestriction, resources.GetString("cbAditionalLogicOfRestriction.ToolTip"))
        Me.cbAditionalLogicOfRestriction.UseVisualStyleBackColor = True
        '
        'lblInstance
        '
        Me.lblInstance.AutoSize = True
        Me.lblInstance.Location = New System.Drawing.Point(672, 15)
        Me.lblInstance.Name = "lblInstance"
        Me.lblInstance.Size = New System.Drawing.Size(0, 13)
        Me.lblInstance.TabIndex = 31
        '
        'btnExcludeControl
        '
        Me.btnExcludeControl.Location = New System.Drawing.Point(601, 237)
        Me.btnExcludeControl.Name = "btnExcludeControl"
        Me.btnExcludeControl.Size = New System.Drawing.Size(130, 23)
        Me.btnExcludeControl.TabIndex = 30
        Me.btnExcludeControl.Text = "Exclude TextBox"
        Me.btnExcludeControl.UseVisualStyleBackColor = True
        '
        'btnForceEnabled
        '
        Me.btnForceEnabled.Location = New System.Drawing.Point(601, 179)
        Me.btnForceEnabled.Name = "btnForceEnabled"
        Me.btnForceEnabled.Size = New System.Drawing.Size(130, 23)
        Me.btnForceEnabled.TabIndex = 29
        Me.btnForceEnabled.Text = "Force TextBox Enabled"
        Me.btnForceEnabled.UseVisualStyleBackColor = True
        '
        'btnForceVisibility
        '
        Me.btnForceVisibility.Location = New System.Drawing.Point(601, 208)
        Me.btnForceVisibility.Name = "btnForceVisibility"
        Me.btnForceVisibility.Size = New System.Drawing.Size(130, 23)
        Me.btnForceVisibility.TabIndex = 28
        Me.btnForceVisibility.Text = "Force TextBox Visible"
        Me.btnForceVisibility.UseVisualStyleBackColor = True
        '
        'cbPaused
        '
        Me.cbPaused.AutoSize = True
        Me.cbPaused.Location = New System.Drawing.Point(606, 42)
        Me.cbPaused.Name = "cbPaused"
        Me.cbPaused.Size = New System.Drawing.Size(119, 17)
        Me.cbPaused.TabIndex = 27
        Me.cbPaused.Text = "Supervision paused"
        Me.cbPaused.UseVisualStyleBackColor = True
        '
        'btnRestrictionsMngt
        '
        Me.btnRestrictionsMngt.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnRestrictionsMngt.Location = New System.Drawing.Point(735, 119)
        Me.btnRestrictionsMngt.Name = "btnRestrictionsMngt"
        Me.btnRestrictionsMngt.Size = New System.Drawing.Size(99, 37)
        Me.btnRestrictionsMngt.TabIndex = 25
        Me.btnRestrictionsMngt.Text = "Restrictions Mngt."
        Me.btnRestrictionsMngt.UseVisualStyleBackColor = True
        '
        'cControls
        '
        Me.cControls.AllowUserToAddRows = False
        Me.cControls.AllowUserToDeleteRows = False
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.cControls.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.cControls.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.cControls.DefaultCellStyle = DataGridViewCellStyle2
        Me.cControls.Location = New System.Drawing.Point(527, 418)
        Me.cControls.Name = "cControls"
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.cControls.RowHeadersDefaultCellStyle = DataGridViewCellStyle3
        Me.cControls.RowHeadersVisible = False
        Me.cControls.Size = New System.Drawing.Size(348, 128)
        Me.cControls.TabIndex = 51
        '
        'ToolStrip1
        '
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripButton1, Me.ToolStripSplitButton1, Me.ToolStripComboBox1, Me.ToolStripTextBox1})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 24)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(887, 25)
        Me.ToolStrip1.TabIndex = 52
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'ToolStripButton1
        '
        Me.ToolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripButton1.Image = CType(resources.GetObject("ToolStripButton1.Image"), System.Drawing.Image)
        Me.ToolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButton1.Name = "ToolStripButton1"
        Me.ToolStripButton1.Size = New System.Drawing.Size(23, 22)
        Me.ToolStripButton1.Text = "ToolStripButton1"
        '
        'ToolStripSplitButton1
        '
        Me.ToolStripSplitButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripSplitButton1.Image = CType(resources.GetObject("ToolStripSplitButton1.Image"), System.Drawing.Image)
        Me.ToolStripSplitButton1.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripSplitButton1.Name = "ToolStripSplitButton1"
        Me.ToolStripSplitButton1.Size = New System.Drawing.Size(32, 22)
        Me.ToolStripSplitButton1.Text = "ToolStripSplitButton1"
        '
        'ToolStripComboBox1
        '
        Me.ToolStripComboBox1.Name = "ToolStripComboBox1"
        Me.ToolStripComboBox1.Size = New System.Drawing.Size(121, 25)
        '
        'ToolStripTextBox1
        '
        Me.ToolStripTextBox1.Name = "ToolStripTextBox1"
        Me.ToolStripTextBox1.Size = New System.Drawing.Size(100, 25)
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FileToolStripMenuItem, Me.EditToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(887, 24)
        Me.MenuStrip1.TabIndex = 53
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'FileToolStripMenuItem
        '
        Me.FileToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NewToolStripMenuItem, Me.CloseToolStripMenuItem, Me.ToolStripSeparator1, Me.ToolStripMenuItem1, Me.ExitToolStripMenuItem})
        Me.FileToolStripMenuItem.Name = "FileToolStripMenuItem"
        Me.FileToolStripMenuItem.Size = New System.Drawing.Size(35, 20)
        Me.FileToolStripMenuItem.Text = "File"
        '
        'NewToolStripMenuItem
        '
        Me.NewToolStripMenuItem.Name = "NewToolStripMenuItem"
        Me.NewToolStripMenuItem.Size = New System.Drawing.Size(111, 22)
        Me.NewToolStripMenuItem.Text = "New"
        '
        'CloseToolStripMenuItem
        '
        Me.CloseToolStripMenuItem.Name = "CloseToolStripMenuItem"
        Me.CloseToolStripMenuItem.Size = New System.Drawing.Size(111, 22)
        Me.CloseToolStripMenuItem.Text = "Close"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(108, 6)
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.Option1ToolStripMenuItem, Me.Option2ToolStripMenuItem})
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(111, 22)
        Me.ToolStripMenuItem1.Text = "Add"
        '
        'Option1ToolStripMenuItem
        '
        Me.Option1ToolStripMenuItem.Name = "Option1ToolStripMenuItem"
        Me.Option1ToolStripMenuItem.Size = New System.Drawing.Size(123, 22)
        Me.Option1ToolStripMenuItem.Text = "Option1"
        '
        'Option2ToolStripMenuItem
        '
        Me.Option2ToolStripMenuItem.Name = "Option2ToolStripMenuItem"
        Me.Option2ToolStripMenuItem.Size = New System.Drawing.Size(123, 22)
        Me.Option2ToolStripMenuItem.Text = "Option2"
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(111, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        '
        'EditToolStripMenuItem
        '
        Me.EditToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.CutToolStripMenuItem, Me.CopyToolStripMenuItem, Me.PasteToolStripMenuItem, Me.ToolStripComboBox2})
        Me.EditToolStripMenuItem.Name = "EditToolStripMenuItem"
        Me.EditToolStripMenuItem.Size = New System.Drawing.Size(37, 20)
        Me.EditToolStripMenuItem.Text = "Edit"
        '
        'CutToolStripMenuItem
        '
        Me.CutToolStripMenuItem.Name = "CutToolStripMenuItem"
        Me.CutToolStripMenuItem.Size = New System.Drawing.Size(181, 22)
        Me.CutToolStripMenuItem.Text = "Cut"
        '
        'CopyToolStripMenuItem
        '
        Me.CopyToolStripMenuItem.Name = "CopyToolStripMenuItem"
        Me.CopyToolStripMenuItem.Size = New System.Drawing.Size(181, 22)
        Me.CopyToolStripMenuItem.Text = "Copy"
        '
        'PasteToolStripMenuItem
        '
        Me.PasteToolStripMenuItem.Name = "PasteToolStripMenuItem"
        Me.PasteToolStripMenuItem.Size = New System.Drawing.Size(181, 22)
        Me.PasteToolStripMenuItem.Text = "Paste"
        '
        'ToolStripComboBox2
        '
        Me.ToolStripComboBox2.Name = "ToolStripComboBox2"
        Me.ToolStripComboBox2.Size = New System.Drawing.Size(121, 21)
        '
        'ControlRestrictedUIWinForms1
        '
        Me.ControlRestrictedUIWinForms1.ConfigFile = "TestWinForms\SecurityNotInfrag.txt"
        Me.ControlRestrictedUIWinForms1.ControlsFile = "TestWinForms\Controls.txt"
        Me.ControlRestrictedUIWinForms1.ID = "Form1"
        Me.ControlRestrictedUIWinForms1.InstanceID = "00"
        Me.ControlRestrictedUIWinForms1.ParentControl = Me
        Me.ControlRestrictedUIWinForms1.Paused = False
        Me.ControlRestrictedUIWinForms1.RestrictionsDefinition = New String() {"$Group 0= GroupBox1.CheckBox1, GroupBox1.TextBox2", "$Group 1= TextBox", "+0/GroupBox1.CheckBox1,E,0", "+99/Combo,V"}
        Me.ControlRestrictedUIWinForms1.SuperviseDeactivation = False
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(887, 564)
        Me.Controls.Add(Me.ToolStrip1)
        Me.Controls.Add(Me.MenuStrip1)
        Me.Controls.Add(Me.cControls)
        Me.Controls.Add(Me.gbCommands)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.TreeView1)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.combo)
        Me.Controls.Add(Me.TextBox)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "Form1"
        Me.Text = "Form1"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        CType(Me.EntidadBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        Me.gbCommands.ResumeLayout(False)
        Me.gbCommands.PerformLayout()
        CType(Me.cControls, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        CType(Me.ControlRestrictedUIWinForms1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents TextBox As System.Windows.Forms.TextBox
    Friend WithEvents Control1 As System.Windows.Forms.Control
    Friend WithEvents Control2 As System.Windows.Forms.Control
    Friend WithEvents Control3 As System.Windows.Forms.Control
    Friend WithEvents combo As System.Windows.Forms.ComboBox
    Friend WithEvents btnEnableVisible As System.Windows.Forms.Button
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents TextBox2 As System.Windows.Forms.TextBox
    Friend WithEvents btnEnableEnabled As System.Windows.Forms.Button
    Friend WithEvents btnDisableVisible As System.Windows.Forms.Button
    Friend WithEvents btnEnableVisible_N As System.Windows.Forms.Button
    Friend WithEvents btnTest As System.Windows.Forms.Button
    Friend WithEvents txtState As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtRoles As System.Windows.Forms.TextBox
    Friend WithEvents btnDisableEnabled As System.Windows.Forms.Button
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txtRestrictions As System.Windows.Forms.TextBox
    Friend WithEvents EntidadBindingSource As System.Windows.Forms.BindingSource
    Friend WithEvents TreeView1 As System.Windows.Forms.TreeView
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents CheckBox1 As System.Windows.Forms.CheckBox
    Friend WithEvents gbCommands As System.Windows.Forms.GroupBox
    Friend WithEvents btnRestrictionsMngt As System.Windows.Forms.Button
    Friend WithEvents cbPaused As System.Windows.Forms.CheckBox
    Friend WithEvents btnForceEnabled As System.Windows.Forms.Button
    Friend WithEvents btnForceVisibility As System.Windows.Forms.Button
    Friend WithEvents btnExcludeControl As System.Windows.Forms.Button
    Friend WithEvents ControlRestrictedUIWinForms1 As RestrictedWinFormsUI.ControlRestrictedUIWinForms
    Friend WithEvents cControls As System.Windows.Forms.DataGridView
    Friend WithEvents lblInstance As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents cbAditionalLogicOfRestriction As System.Windows.Forms.CheckBox
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents cbUseReadOnly As System.Windows.Forms.CheckBox
    Friend WithEvents btnReinitialize As System.Windows.Forms.Button
    Friend WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
    Friend WithEvents ToolStripButton1 As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSplitButton1 As System.Windows.Forms.ToolStripSplitButton
    Friend WithEvents ToolStripComboBox1 As System.Windows.Forms.ToolStripComboBox
    Friend WithEvents ToolStripTextBox1 As System.Windows.Forms.ToolStripTextBox
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents FileToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CloseToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Option1ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Option2ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents EditToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CutToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CopyToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents PasteToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripComboBox2 As System.Windows.Forms.ToolStripComboBox
    Friend WithEvents btnChangeState_N As System.Windows.Forms.Button
    Friend WithEvents cbSuperviseDeactivation As System.Windows.Forms.CheckBox

End Class
