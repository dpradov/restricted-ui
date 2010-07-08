<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form2
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form2))
        Me.Button1 = New System.Windows.Forms.Button
        Me.TabControl1 = New System.Windows.Forms.TabControl
        Me.TabPage1 = New System.Windows.Forms.TabPage
        Me.UserControl11 = New TestWinForms.UserControl1
        Me.TabPage2 = New System.Windows.Forms.TabPage
        Me.UserControl12 = New TestWinForms.UserControl1
        Me.TabPage3 = New System.Windows.Forms.TabPage
        Me.UserControl21 = New TestWinForms.UserControl2
        Me.TabPage4 = New System.Windows.Forms.TabPage
        Me.UserControl22 = New TestWinForms.UserControl2
        Me.ControlRestrictedUIWinForms1 = New RestrictedWinFormsUI.ControlRestrictedUIWinForms(Me.components)
        Me.UserControl31 = New TestWinForms.UserControl3
        Me.cConfigFile = New System.Windows.Forms.TextBox
        Me.btnChangeState_N = New System.Windows.Forms.Button
        Me.TabControl1.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        Me.TabPage3.SuspendLayout()
        Me.TabPage4.SuspendLayout()
        CType(Me.ControlRestrictedUIWinForms1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(154, 33)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(75, 23)
        Me.Button1.TabIndex = 0
        Me.Button1.Text = "Button1"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.TabPage1)
        Me.TabControl1.Controls.Add(Me.TabPage2)
        Me.TabControl1.Controls.Add(Me.TabPage3)
        Me.TabControl1.Controls.Add(Me.TabPage4)
        Me.TabControl1.Location = New System.Drawing.Point(12, 72)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(424, 200)
        Me.TabControl1.TabIndex = 1
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.UserControl11)
        Me.TabPage1.Location = New System.Drawing.Point(4, 22)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(416, 174)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "TabPage1"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'UserControl11
        '
        Me.UserControl11.Location = New System.Drawing.Point(15, 6)
        Me.UserControl11.Name = "UserControl11"
        Me.UserControl11.Size = New System.Drawing.Size(224, 165)
        Me.UserControl11.TabIndex = 0
        '
        'TabPage2
        '
        Me.TabPage2.Controls.Add(Me.UserControl12)
        Me.TabPage2.Location = New System.Drawing.Point(4, 22)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(416, 174)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "TabPage2"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'UserControl12
        '
        Me.UserControl12.Location = New System.Drawing.Point(15, 6)
        Me.UserControl12.Name = "UserControl12"
        Me.UserControl12.Size = New System.Drawing.Size(224, 165)
        Me.UserControl12.TabIndex = 0
        '
        'TabPage3
        '
        Me.TabPage3.Controls.Add(Me.UserControl21)
        Me.TabPage3.Location = New System.Drawing.Point(4, 22)
        Me.TabPage3.Name = "TabPage3"
        Me.TabPage3.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage3.Size = New System.Drawing.Size(416, 174)
        Me.TabPage3.TabIndex = 2
        Me.TabPage3.Text = "TabPage3"
        Me.TabPage3.UseVisualStyleBackColor = True
        '
        'UserControl21
        '
        Me.UserControl21.Location = New System.Drawing.Point(10, 9)
        Me.UserControl21.Name = "UserControl21"
        Me.UserControl21.Size = New System.Drawing.Size(262, 163)
        Me.UserControl21.TabIndex = 2
        '
        'TabPage4
        '
        Me.TabPage4.Controls.Add(Me.UserControl22)
        Me.TabPage4.Location = New System.Drawing.Point(4, 22)
        Me.TabPage4.Name = "TabPage4"
        Me.TabPage4.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage4.Size = New System.Drawing.Size(416, 174)
        Me.TabPage4.TabIndex = 3
        Me.TabPage4.Text = "TabPage4"
        Me.TabPage4.UseVisualStyleBackColor = True
        '
        'UserControl22
        '
        Me.UserControl22.Location = New System.Drawing.Point(11, 6)
        Me.UserControl22.Name = "UserControl22"
        Me.UserControl22.Size = New System.Drawing.Size(262, 167)
        Me.UserControl22.TabIndex = 0
        '
        'ControlRestrictedUIWinForms1
        '
        Me.ControlRestrictedUIWinForms1.ConfigFile = "PruebaWinForms\SecurityNotInfrag.txt"
        Me.ControlRestrictedUIWinForms1.ControlsFile = "PruebaWinForms\Controles.txt"
        Me.ControlRestrictedUIWinForms1.ID = "Form2"
        Me.ControlRestrictedUIWinForms1.InstanceID = "00"
        Me.ControlRestrictedUIWinForms1.ParentControl = Me
        Me.ControlRestrictedUIWinForms1.Paused = False
        Me.ControlRestrictedUIWinForms1.RestrictionsDefinition = New String() {"-0/TabControl1.TabPage4.UserControl22.GroupBox1.RadioButton2,E"}
        Me.ControlRestrictedUIWinForms1.SuperviseDeactivation = False
        '
        'UserControl31
        '
        Me.UserControl31.Location = New System.Drawing.Point(16, 9)
        Me.UserControl31.Name = "UserControl31"
        Me.UserControl31.Size = New System.Drawing.Size(132, 62)
        Me.UserControl31.TabIndex = 2
        '
        'cConfigFile
        '
        Me.cConfigFile.Location = New System.Drawing.Point(19, 278)
        Me.cConfigFile.Multiline = True
        Me.cConfigFile.Name = "cConfigFile"
        Me.cConfigFile.ReadOnly = True
        Me.cConfigFile.Size = New System.Drawing.Size(417, 120)
        Me.cConfigFile.TabIndex = 85
        Me.cConfigFile.Text = resources.GetString("cConfigFile.Text")
        '
        'btnChangeState_N
        '
        Me.btnChangeState_N.Location = New System.Drawing.Point(334, 12)
        Me.btnChangeState_N.Name = "btnChangeState_N"
        Me.btnChangeState_N.Size = New System.Drawing.Size(98, 23)
        Me.btnChangeState_N.TabIndex = 86
        Me.btnChangeState_N.Text = "Random State N times"
        Me.btnChangeState_N.UseVisualStyleBackColor = True
        '
        'Form2
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(448, 406)
        Me.Controls.Add(Me.btnChangeState_N)
        Me.Controls.Add(Me.cConfigFile)
        Me.Controls.Add(Me.UserControl31)
        Me.Controls.Add(Me.TabControl1)
        Me.Controls.Add(Me.Button1)
        Me.Name = "Form2"
        Me.Text = "Form2"
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage2.ResumeLayout(False)
        Me.TabPage3.ResumeLayout(False)
        Me.TabPage4.ResumeLayout(False)
        CType(Me.ControlRestrictedUIWinForms1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ControlRestrictedUIWinForms1 As RestrictedWinFormsUI.ControlRestrictedUIWinForms
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
    Friend WithEvents UserControl11 As TestWinForms.UserControl1
    Friend WithEvents UserControl12 As TestWinForms.UserControl1
    Friend WithEvents TabPage3 As System.Windows.Forms.TabPage
    Friend WithEvents UserControl21 As TestWinForms.UserControl2
    Friend WithEvents TabPage4 As System.Windows.Forms.TabPage
    Friend WithEvents UserControl22 As TestWinForms.UserControl2
    Friend WithEvents UserControl31 As TestWinForms.UserControl3
    Friend WithEvents cConfigFile As System.Windows.Forms.TextBox
    Friend WithEvents btnChangeState_N As System.Windows.Forms.Button
End Class
