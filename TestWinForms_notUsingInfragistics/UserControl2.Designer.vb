<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UserControl2
    Inherits System.Windows.Forms.UserControl

    'UserControl reemplaza a Dispose para limpiar la lista de componentes.
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
        Dim TreeNode7 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Nodo1")
        Dim TreeNode8 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Nodo2")
        Dim TreeNode9 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Nodo0", New System.Windows.Forms.TreeNode() {TreeNode7, TreeNode8})
        Dim TreeNode10 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Nodo5")
        Dim TreeNode11 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Nodo4", New System.Windows.Forms.TreeNode() {TreeNode10})
        Dim TreeNode12 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Nodo3", New System.Windows.Forms.TreeNode() {TreeNode11})
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.RadioButton2 = New System.Windows.Forms.RadioButton
        Me.RadioButton1 = New System.Windows.Forms.RadioButton
        Me.TreeView1 = New System.Windows.Forms.TreeView
        Me.ControlSeguridadWinForms1 = New RestrictedWinFormsUI.ControlRestrictedUIWinForms(Me.components)
        Me.GroupBox1.SuspendLayout()
        CType(Me.ControlSeguridadWinForms1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.RadioButton2)
        Me.GroupBox1.Controls.Add(Me.RadioButton1)
        Me.GroupBox1.Location = New System.Drawing.Point(19, 13)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(200, 61)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "GroupBox1"
        '
        'RadioButton2
        '
        Me.RadioButton2.AutoSize = True
        Me.RadioButton2.Location = New System.Drawing.Point(19, 38)
        Me.RadioButton2.Name = "RadioButton2"
        Me.RadioButton2.Size = New System.Drawing.Size(90, 17)
        Me.RadioButton2.TabIndex = 2
        Me.RadioButton2.TabStop = True
        Me.RadioButton2.Text = "RadioButton2"
        Me.RadioButton2.UseVisualStyleBackColor = True
        '
        'RadioButton1
        '
        Me.RadioButton1.AutoSize = True
        Me.RadioButton1.Location = New System.Drawing.Point(19, 19)
        Me.RadioButton1.Name = "RadioButton1"
        Me.RadioButton1.Size = New System.Drawing.Size(90, 17)
        Me.RadioButton1.TabIndex = 1
        Me.RadioButton1.TabStop = True
        Me.RadioButton1.Text = "RadioButton1"
        Me.RadioButton1.UseVisualStyleBackColor = True
        '
        'TreeView1
        '
        Me.TreeView1.Location = New System.Drawing.Point(29, 80)
        Me.TreeView1.Name = "TreeView1"
        TreeNode7.Name = "Nodo1"
        TreeNode7.Text = "Nodo1"
        TreeNode8.Name = "Nodo2"
        TreeNode8.Text = "Nodo2"
        TreeNode9.Name = "Nodo0"
        TreeNode9.Text = "Nodo0"
        TreeNode10.Name = "Nodo5"
        TreeNode10.Text = "Nodo5"
        TreeNode11.Name = "Nodo4"
        TreeNode11.Text = "Nodo4"
        TreeNode12.Name = "Nodo3"
        TreeNode12.Text = "Nodo3"
        Me.TreeView1.Nodes.AddRange(New System.Windows.Forms.TreeNode() {TreeNode9, TreeNode12})
        Me.TreeView1.Size = New System.Drawing.Size(125, 72)
        Me.TreeView1.TabIndex = 26
        '
        'ControlSeguridadWinForms1
        '
        Me.ControlSeguridadWinForms1.ConfigFile = "PruebaWinForms\Seguridad.txt"
        Me.ControlSeguridadWinForms1.ControlsFile = "PruebaWinForms\Controles.txt"
        Me.ControlSeguridadWinForms1.ID = "Form2_sub2"
        Me.ControlSeguridadWinForms1.InstanceID = "00"
        Me.ControlSeguridadWinForms1.ParentControl = Me
        Me.ControlSeguridadWinForms1.Paused = False
        Me.ControlSeguridadWinForms1.RestrictionsDefinition = New String() {"-0/TreeView1.Nodo0.Nodo1,E/TreeView1.Nodo3.Nodo4.Nodo5,V"}
        '
        'UserControl2
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.TreeView1)
        Me.Controls.Add(Me.GroupBox1)
        Me.Name = "UserControl2"
        Me.Size = New System.Drawing.Size(262, 168)
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        CType(Me.ControlSeguridadWinForms1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents RadioButton2 As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButton1 As System.Windows.Forms.RadioButton
    Friend WithEvents TreeView1 As System.Windows.Forms.TreeView
    Friend WithEvents ControlSeguridadWinForms1 As RestrictedWinFormsUI.ControlRestrictedUIWinForms

End Class
