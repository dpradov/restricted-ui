<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MainForm
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
        Me.btnForm1 = New System.Windows.Forms.Button
        Me.btnForm2 = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'btnForm1
        '
        Me.btnForm1.Location = New System.Drawing.Point(35, 27)
        Me.btnForm1.Name = "btnForm1"
        Me.btnForm1.Size = New System.Drawing.Size(75, 23)
        Me.btnForm1.TabIndex = 0
        Me.btnForm1.Text = "Form1"
        Me.btnForm1.UseVisualStyleBackColor = True
        '
        'btnForm2
        '
        Me.btnForm2.Location = New System.Drawing.Point(35, 67)
        Me.btnForm2.Name = "btnForm2"
        Me.btnForm2.Size = New System.Drawing.Size(75, 23)
        Me.btnForm2.TabIndex = 1
        Me.btnForm2.Text = "Form2"
        Me.btnForm2.UseVisualStyleBackColor = True
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(152, 125)
        Me.Controls.Add(Me.btnForm2)
        Me.Controls.Add(Me.btnForm1)
        Me.Name = "MainForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "FormPpal"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnForm1 As System.Windows.Forms.Button
    Friend WithEvents btnForm2 As System.Windows.Forms.Button
End Class
