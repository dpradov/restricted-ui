﻿<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
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
        Me.ControlSeguridadWinForms1 = New RestrictedWinFormsUI.ControlRestrictedUIWinForms(Me.components)
        Me.UserControl31 = New TestWinForms.UserControl3
        Me.TabControl1.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        Me.TabPage3.SuspendLayout()
        Me.TabPage4.SuspendLayout()
        CType(Me.ControlSeguridadWinForms1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(251, 22)
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
        Me.TabControl1.Location = New System.Drawing.Point(12, 108)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(318, 212)
        Me.TabControl1.TabIndex = 1
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.UserControl11)
        Me.TabPage1.Location = New System.Drawing.Point(4, 22)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(310, 186)
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
        Me.TabPage2.Size = New System.Drawing.Size(310, 186)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "TabPage2"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'UserControl12
        '
        Me.UserControl12.Location = New System.Drawing.Point(71, 6)
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
        Me.TabPage3.Size = New System.Drawing.Size(310, 186)
        Me.TabPage3.TabIndex = 2
        Me.TabPage3.Text = "TabPage3"
        Me.TabPage3.UseVisualStyleBackColor = True
        '
        'UserControl21
        '
        Me.UserControl21.Location = New System.Drawing.Point(24, 9)
        Me.UserControl21.Name = "UserControl21"
        Me.UserControl21.Size = New System.Drawing.Size(262, 168)
        Me.UserControl21.TabIndex = 2
        '
        'TabPage4
        '
        Me.TabPage4.Controls.Add(Me.UserControl22)
        Me.TabPage4.Location = New System.Drawing.Point(4, 22)
        Me.TabPage4.Name = "TabPage4"
        Me.TabPage4.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage4.Size = New System.Drawing.Size(310, 186)
        Me.TabPage4.TabIndex = 3
        Me.TabPage4.Text = "TabPage4"
        Me.TabPage4.UseVisualStyleBackColor = True
        '
        'UserControl22
        '
        Me.UserControl22.Location = New System.Drawing.Point(16, 6)
        Me.UserControl22.Name = "UserControl22"
        Me.UserControl22.Size = New System.Drawing.Size(262, 168)
        Me.UserControl22.TabIndex = 0
        '
        'ControlSeguridadWinForms1
        '
        Me.ControlSeguridadWinForms1.ParentControl = Me
        Me.ControlSeguridadWinForms1.ConfigFile = "PruebaWinForms\Seguridad.txt"
        Me.ControlSeguridadWinForms1.ControlsFile = "PruebaWinForms\Controles.txt"
        Me.ControlSeguridadWinForms1.ID = "Form2"
        Me.ControlSeguridadWinForms1.InstanceID = "00"
        Me.ControlSeguridadWinForms1.Paused = False
        Me.ControlSeguridadWinForms1.RestrictionsDefinition = New String() {"-0/TabControl1.TabPage4.UserControl22.GroupBox1.RadioButton2,E"}
        '
        'UserControl31
        '
        Me.UserControl31.Location = New System.Drawing.Point(16, 22)
        Me.UserControl31.Name = "UserControl31"
        Me.UserControl31.Size = New System.Drawing.Size(132, 62)
        Me.UserControl31.TabIndex = 2
        '
        'Form2
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(362, 332)
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
        CType(Me.ControlSeguridadWinForms1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ControlSeguridadWinForms1 As RestrictedWinFormsUI.ControlRestrictedUIWinForms
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
End Class