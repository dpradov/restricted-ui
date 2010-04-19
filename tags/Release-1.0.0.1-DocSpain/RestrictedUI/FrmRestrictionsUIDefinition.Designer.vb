<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmRestrictionsUIDefinition
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
        Dim DataGridViewCellStyle16 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle17 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle18 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle19 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle20 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle21 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle22 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle23 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle24 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle25 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle26 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle27 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle28 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle29 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle30 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Me.btnApply = New System.Windows.Forms.Button
        Me.btnDeleteRestriction = New System.Windows.Forms.Button
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.btnSelectFile = New System.Windows.Forms.Button
        Me.lbFichero = New System.Windows.Forms.Label
        Me.btnLoad = New System.Windows.Forms.Button
        Me.btnSave = New System.Windows.Forms.Button
        Me.cbOnlyActualComponent = New System.Windows.Forms.CheckBox
        Me.btnProhibit = New System.Windows.Forms.Button
        Me.btnAuthorize = New System.Windows.Forms.Button
        Me.cComponents = New System.Windows.Forms.ComboBox
        Me.lblAviso = New System.Windows.Forms.Label
        Me.btnResize = New System.Windows.Forms.Button
        Me.btnDuplicateRestriction = New System.Windows.Forms.Button
        Me.btnCancel = New System.Windows.Forms.Button
        Me.gbSecuritySource = New System.Windows.Forms.GroupBox
        Me.cbFilterOnSelectedRol = New System.Windows.Forms.CheckBox
        Me.btnRestrictionsGrid = New System.Windows.Forms.Button
        Me.btnRestrictionsTxt = New System.Windows.Forms.Button
        Me.txtRestrictions = New System.Windows.Forms.TextBox
        Me.cRestrictions = New System.Windows.Forms.DataGridView
        Me.gbRestrictionsDefinition = New System.Windows.Forms.GroupBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.cbShowOnlySelectedGroup = New System.Windows.Forms.CheckBox
        Me.btnAddGroup = New System.Windows.Forms.Button
        Me.lblGrupos = New System.Windows.Forms.Label
        Me.cGroups = New System.Windows.Forms.DataGridView
        Me.btnDeleteState = New System.Windows.Forms.Button
        Me.btnDeleteRol = New System.Windows.Forms.Button
        Me.cbStatesSelectAll = New System.Windows.Forms.CheckBox
        Me.cbRolesSelectAll = New System.Windows.Forms.CheckBox
        Me.CheckBox2 = New System.Windows.Forms.CheckBox
        Me.cbControlsSelectAll = New System.Windows.Forms.CheckBox
        Me.Button2 = New System.Windows.Forms.Button
        Me.Button1 = New System.Windows.Forms.Button
        Me.cEnabled = New System.Windows.Forms.CheckBox
        Me.CheckBox1 = New System.Windows.Forms.CheckBox
        Me.lbSupervisar = New System.Windows.Forms.Label
        Me.cVisibility = New System.Windows.Forms.CheckBox
        Me.lbEstados = New System.Windows.Forms.Label
        Me.lbRoles = New System.Windows.Forms.Label
        Me.lblControles = New System.Windows.Forms.Label
        Me.cStates = New System.Windows.Forms.DataGridView
        Me.cControls = New System.Windows.Forms.DataGridView
        Me.cRoles = New System.Windows.Forms.DataGridView
        Me.btnDeleteGroup = New System.Windows.Forms.Button
        Me.cbSaveOnApplying = New System.Windows.Forms.CheckBox
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog
        Me.btnAccept = New System.Windows.Forms.Button
        Me.txtState = New System.Windows.Forms.TextBox
        Me.lblEstado = New System.Windows.Forms.Label
        Me.txtRoles = New System.Windows.Forms.TextBox
        Me.lblRoles = New System.Windows.Forms.Label
        Me.lblSituacion = New System.Windows.Forms.Label
        Me.cHostSituation = New System.Windows.Forms.Panel
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.cConfigFile = New System.Windows.Forms.TextBox
        Me.cbRestrictionsSelectAll = New System.Windows.Forms.CheckBox
        Me.gbSecuritySource.SuspendLayout()
        CType(Me.cRestrictions, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.gbRestrictionsDefinition.SuspendLayout()
        CType(Me.cGroups, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.cStates, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.cControls, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.cRoles, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.cHostSituation.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnApply
        '
        Me.btnApply.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.btnApply.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnApply.Location = New System.Drawing.Point(644, 674)
        Me.btnApply.Name = "btnApply"
        Me.btnApply.Size = New System.Drawing.Size(74, 23)
        Me.btnApply.TabIndex = 1
        Me.btnApply.Text = "Aplicar"
        Me.btnApply.UseVisualStyleBackColor = True
        '
        'btnDeleteRestriction
        '
        Me.btnDeleteRestriction.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnDeleteRestriction.Location = New System.Drawing.Point(749, 183)
        Me.btnDeleteRestriction.Name = "btnDeleteRestriction"
        Me.btnDeleteRestriction.Size = New System.Drawing.Size(58, 23)
        Me.btnDeleteRestriction.TabIndex = 33
        Me.btnDeleteRestriction.Text = "Eliminar"
        Me.btnDeleteRestriction.UseVisualStyleBackColor = True
        '
        'btnSelectFile
        '
        Me.btnSelectFile.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSelectFile.Location = New System.Drawing.Point(513, 12)
        Me.btnSelectFile.Name = "btnSelectFile"
        Me.btnSelectFile.Size = New System.Drawing.Size(54, 23)
        Me.btnSelectFile.TabIndex = 51
        Me.btnSelectFile.Text = "Selec."
        Me.ToolTip1.SetToolTip(Me.btnSelectFile, "Seleccionar fichero de configuración en el que guardar la configuración aplicada")
        Me.btnSelectFile.UseVisualStyleBackColor = True
        '
        'lbFichero
        '
        Me.lbFichero.AutoSize = True
        Me.lbFichero.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbFichero.Location = New System.Drawing.Point(16, 15)
        Me.lbFichero.Name = "lbFichero"
        Me.lbFichero.Size = New System.Drawing.Size(75, 13)
        Me.lbFichero.TabIndex = 68
        Me.lbFichero.Text = "Fichero cfg:"
        Me.ToolTip1.SetToolTip(Me.lbFichero, "Fichero de configuración")
        '
        'btnLoad
        '
        Me.btnLoad.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnLoad.Location = New System.Drawing.Point(15, 37)
        Me.btnLoad.Name = "btnLoad"
        Me.btnLoad.Size = New System.Drawing.Size(54, 23)
        Me.btnLoad.TabIndex = 69
        Me.btnLoad.Text = "Leer"
        Me.ToolTip1.SetToolTip(Me.btnLoad, "Carga la configuracion de seguridad desde el archivo seleccionado")
        Me.btnLoad.UseVisualStyleBackColor = True
        '
        'btnSave
        '
        Me.btnSave.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSave.Location = New System.Drawing.Point(75, 37)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(54, 23)
        Me.btnSave.TabIndex = 70
        Me.btnSave.Text = "Grabar"
        Me.ToolTip1.SetToolTip(Me.btnSave, "Guarda la configuracion de Seleccionar fichero de configuración en el fichero sel" & _
                "eccionado")
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'cbOnlyActualComponent
        '
        Me.cbOnlyActualComponent.AutoSize = True
        Me.cbOnlyActualComponent.Location = New System.Drawing.Point(137, 42)
        Me.cbOnlyActualComponent.Name = "cbOnlyActualComponent"
        Me.cbOnlyActualComponent.Size = New System.Drawing.Size(141, 17)
        Me.cbOnlyActualComponent.TabIndex = 71
        Me.cbOnlyActualComponent.Text = "Solo componente actual"
        Me.ToolTip1.SetToolTip(Me.cbOnlyActualComponent, "Leer y aplicar o grabar únicamente la informacion especifica del componente de se" & _
                "guridad actual")
        Me.cbOnlyActualComponent.UseVisualStyleBackColor = True
        '
        'btnProhibit
        '
        Me.btnProhibit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnProhibit.Location = New System.Drawing.Point(500, 347)
        Me.btnProhibit.Name = "btnProhibit"
        Me.btnProhibit.Size = New System.Drawing.Size(75, 23)
        Me.btnProhibit.TabIndex = 58
        Me.btnProhibit.Text = "Impedir"
        Me.ToolTip1.SetToolTip(Me.btnProhibit, "Impedir activar las propiedades supervisadas para la selección hecha: controles o" & _
                " grupos de controles / Roles / Estados")
        Me.btnProhibit.UseVisualStyleBackColor = True
        '
        'btnAuthorize
        '
        Me.btnAuthorize.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnAuthorize.Location = New System.Drawing.Point(419, 347)
        Me.btnAuthorize.Name = "btnAuthorize"
        Me.btnAuthorize.Size = New System.Drawing.Size(75, 23)
        Me.btnAuthorize.TabIndex = 57
        Me.btnAuthorize.Text = "Permitir"
        Me.ToolTip1.SetToolTip(Me.btnAuthorize, "Permitir activar las propiedades supervisadas para la selección hecha: controles " & _
                "o grupos de controles / Roles / Estados")
        Me.btnAuthorize.UseVisualStyleBackColor = True
        '
        'cComponents
        '
        Me.cComponents.DisplayMember = "LongID"
        Me.cComponents.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cComponents.FormattingEnabled = True
        Me.cComponents.Location = New System.Drawing.Point(96, 13)
        Me.cComponents.Name = "cComponents"
        Me.cComponents.Size = New System.Drawing.Size(280, 21)
        Me.cComponents.TabIndex = 82
        '
        'lblAviso
        '
        Me.lblAviso.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAviso.ForeColor = System.Drawing.Color.Gray
        Me.lblAviso.Location = New System.Drawing.Point(21, 373)
        Me.lblAviso.Name = "lblAviso"
        Me.lblAviso.Size = New System.Drawing.Size(552, 13)
        Me.lblAviso.TabIndex = 47
        '
        'btnResize
        '
        Me.btnResize.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnResize.Image = Global.RestrictedUI.My.Resources.Resources.CanvasScale
        Me.btnResize.Location = New System.Drawing.Point(450, -1)
        Me.btnResize.Name = "btnResize"
        Me.btnResize.Size = New System.Drawing.Size(24, 24)
        Me.btnResize.TabIndex = 68
        Me.btnResize.UseVisualStyleBackColor = True
        '
        'btnDuplicateRestriction
        '
        Me.btnDuplicateRestriction.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnDuplicateRestriction.Location = New System.Drawing.Point(817, 183)
        Me.btnDuplicateRestriction.Name = "btnDuplicateRestriction"
        Me.btnDuplicateRestriction.Size = New System.Drawing.Size(62, 23)
        Me.btnDuplicateRestriction.TabIndex = 47
        Me.btnDuplicateRestriction.Text = "Duplicar"
        Me.btnDuplicateRestriction.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCancel.Location = New System.Drawing.Point(816, 674)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.TabIndex = 48
        Me.btnCancel.Text = "Cancelar"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'gbSecuritySource
        '
        Me.gbSecuritySource.Controls.Add(Me.cbRestrictionsSelectAll)
        Me.gbSecuritySource.Controls.Add(Me.cbFilterOnSelectedRol)
        Me.gbSecuritySource.Controls.Add(Me.btnRestrictionsGrid)
        Me.gbSecuritySource.Controls.Add(Me.btnRestrictionsTxt)
        Me.gbSecuritySource.Controls.Add(Me.txtRestrictions)
        Me.gbSecuritySource.Controls.Add(Me.cRestrictions)
        Me.gbSecuritySource.Controls.Add(Me.btnDuplicateRestriction)
        Me.gbSecuritySource.Controls.Add(Me.btnDeleteRestriction)
        Me.gbSecuritySource.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.gbSecuritySource.Location = New System.Drawing.Point(12, 423)
        Me.gbSecuritySource.Name = "gbSecuritySource"
        Me.gbSecuritySource.Size = New System.Drawing.Size(900, 211)
        Me.gbSecuritySource.TabIndex = 49
        Me.gbSecuritySource.TabStop = False
        Me.gbSecuritySource.Text = " Permisos definidos "
        '
        'cbFilterOnSelectedRol
        '
        Me.cbFilterOnSelectedRol.AutoSize = True
        Me.cbFilterOnSelectedRol.Enabled = False
        Me.cbFilterOnSelectedRol.Location = New System.Drawing.Point(584, 16)
        Me.cbFilterOnSelectedRol.Name = "cbFilterOnSelectedRol"
        Me.cbFilterOnSelectedRol.Size = New System.Drawing.Size(168, 17)
        Me.cbFilterOnSelectedRol.TabIndex = 75
        Me.cbFilterOnSelectedRol.Text = "Filtrar según Rol seleccionado"
        Me.cbFilterOnSelectedRol.UseVisualStyleBackColor = True
        '
        'btnRestrictionsGrid
        '
        Me.btnRestrictionsGrid.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnRestrictionsGrid.Location = New System.Drawing.Point(763, 12)
        Me.btnRestrictionsGrid.Name = "btnRestrictionsGrid"
        Me.btnRestrictionsGrid.Size = New System.Drawing.Size(54, 21)
        Me.btnRestrictionsGrid.TabIndex = 74
        Me.btnRestrictionsGrid.Text = "Tabular"
        Me.btnRestrictionsGrid.UseVisualStyleBackColor = True
        '
        'btnRestrictionsTxt
        '
        Me.btnRestrictionsTxt.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnRestrictionsTxt.Location = New System.Drawing.Point(823, 12)
        Me.btnRestrictionsTxt.Name = "btnRestrictionsTxt"
        Me.btnRestrictionsTxt.Size = New System.Drawing.Size(54, 21)
        Me.btnRestrictionsTxt.TabIndex = 73
        Me.btnRestrictionsTxt.Text = "Texto"
        Me.btnRestrictionsTxt.UseVisualStyleBackColor = True
        '
        'txtRestrictions
        '
        Me.txtRestrictions.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(205, Byte), Integer))
        Me.txtRestrictions.Location = New System.Drawing.Point(16, 41)
        Me.txtRestrictions.Multiline = True
        Me.txtRestrictions.Name = "txtRestrictions"
        Me.txtRestrictions.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtRestrictions.Size = New System.Drawing.Size(859, 133)
        Me.txtRestrictions.TabIndex = 23
        Me.txtRestrictions.Visible = False
        '
        'cRestrictions
        '
        Me.cRestrictions.AllowUserToAddRows = False
        DataGridViewCellStyle16.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle16.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle16.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle16.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle16.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle16.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle16.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.cRestrictions.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle16
        Me.cRestrictions.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewCellStyle17.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle17.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(205, Byte), Integer))
        DataGridViewCellStyle17.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle17.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle17.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle17.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle17.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.cRestrictions.DefaultCellStyle = DataGridViewCellStyle17
        Me.cRestrictions.Location = New System.Drawing.Point(14, 41)
        Me.cRestrictions.Name = "cRestrictions"
        DataGridViewCellStyle18.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle18.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle18.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle18.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle18.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle18.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle18.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.cRestrictions.RowHeadersDefaultCellStyle = DataGridViewCellStyle18
        Me.cRestrictions.RowHeadersVisible = False
        Me.cRestrictions.Size = New System.Drawing.Size(863, 133)
        Me.cRestrictions.TabIndex = 62
        '
        'gbRestrictionsDefinition
        '
        Me.gbRestrictionsDefinition.Controls.Add(Me.cComponents)
        Me.gbRestrictionsDefinition.Controls.Add(Me.Label1)
        Me.gbRestrictionsDefinition.Controls.Add(Me.lblAviso)
        Me.gbRestrictionsDefinition.Controls.Add(Me.cbShowOnlySelectedGroup)
        Me.gbRestrictionsDefinition.Controls.Add(Me.btnAddGroup)
        Me.gbRestrictionsDefinition.Controls.Add(Me.lblGrupos)
        Me.gbRestrictionsDefinition.Controls.Add(Me.cGroups)
        Me.gbRestrictionsDefinition.Controls.Add(Me.btnDeleteState)
        Me.gbRestrictionsDefinition.Controls.Add(Me.btnDeleteRol)
        Me.gbRestrictionsDefinition.Controls.Add(Me.cbStatesSelectAll)
        Me.gbRestrictionsDefinition.Controls.Add(Me.cbRolesSelectAll)
        Me.gbRestrictionsDefinition.Controls.Add(Me.CheckBox2)
        Me.gbRestrictionsDefinition.Controls.Add(Me.cbControlsSelectAll)
        Me.gbRestrictionsDefinition.Controls.Add(Me.Button2)
        Me.gbRestrictionsDefinition.Controls.Add(Me.Button1)
        Me.gbRestrictionsDefinition.Controls.Add(Me.btnProhibit)
        Me.gbRestrictionsDefinition.Controls.Add(Me.btnAuthorize)
        Me.gbRestrictionsDefinition.Controls.Add(Me.cEnabled)
        Me.gbRestrictionsDefinition.Controls.Add(Me.CheckBox1)
        Me.gbRestrictionsDefinition.Controls.Add(Me.lbSupervisar)
        Me.gbRestrictionsDefinition.Controls.Add(Me.cVisibility)
        Me.gbRestrictionsDefinition.Controls.Add(Me.lbEstados)
        Me.gbRestrictionsDefinition.Controls.Add(Me.lbRoles)
        Me.gbRestrictionsDefinition.Controls.Add(Me.lblControles)
        Me.gbRestrictionsDefinition.Controls.Add(Me.cStates)
        Me.gbRestrictionsDefinition.Controls.Add(Me.cControls)
        Me.gbRestrictionsDefinition.Controls.Add(Me.cRoles)
        Me.gbRestrictionsDefinition.Controls.Add(Me.btnDeleteGroup)
        Me.gbRestrictionsDefinition.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.gbRestrictionsDefinition.Location = New System.Drawing.Point(12, 25)
        Me.gbRestrictionsDefinition.Name = "gbRestrictionsDefinition"
        Me.gbRestrictionsDefinition.Size = New System.Drawing.Size(900, 390)
        Me.gbRestrictionsDefinition.TabIndex = 50
        Me.gbRestrictionsDefinition.TabStop = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(10, 16)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(81, 13)
        Me.Label1.TabIndex = 83
        Me.Label1.Text = "Componente:"
        '
        'cbShowOnlySelectedGroup
        '
        Me.cbShowOnlySelectedGroup.AutoSize = True
        Me.cbShowOnlySelectedGroup.Enabled = False
        Me.cbShowOnlySelectedGroup.Location = New System.Drawing.Point(395, 35)
        Me.cbShowOnlySelectedGroup.Name = "cbShowOnlySelectedGroup"
        Me.cbShowOnlySelectedGroup.Size = New System.Drawing.Size(181, 17)
        Me.cbShowOnlySelectedGroup.TabIndex = 67
        Me.cbShowOnlySelectedGroup.Text = "Mostrar sólo Grupo seleccionado"
        Me.cbShowOnlySelectedGroup.UseVisualStyleBackColor = True
        '
        'btnAddGroup
        '
        Me.btnAddGroup.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnAddGroup.Location = New System.Drawing.Point(768, 126)
        Me.btnAddGroup.Name = "btnAddGroup"
        Me.btnAddGroup.Size = New System.Drawing.Size(56, 23)
        Me.btnAddGroup.TabIndex = 66
        Me.btnAddGroup.Text = "Añadir"
        Me.btnAddGroup.UseVisualStyleBackColor = True
        '
        'lblGrupos
        '
        Me.lblGrupos.AutoSize = True
        Me.lblGrupos.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblGrupos.Location = New System.Drawing.Point(612, 39)
        Me.lblGrupos.Name = "lblGrupos"
        Me.lblGrupos.Size = New System.Drawing.Size(121, 13)
        Me.lblGrupos.TabIndex = 65
        Me.lblGrupos.Text = "Grupos de controles"
        '
        'cGroups
        '
        Me.cGroups.AllowUserToAddRows = False
        Me.cGroups.AllowUserToDeleteRows = False
        DataGridViewCellStyle19.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle19.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle19.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle19.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle19.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle19.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle19.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.cGroups.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle19
        Me.cGroups.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewCellStyle20.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle20.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle20.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle20.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle20.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle20.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle20.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.cGroups.DefaultCellStyle = DataGridViewCellStyle20
        Me.cGroups.Location = New System.Drawing.Point(615, 55)
        Me.cGroups.Name = "cGroups"
        DataGridViewCellStyle21.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle21.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle21.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle21.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle21.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle21.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle21.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.cGroups.RowHeadersDefaultCellStyle = DataGridViewCellStyle21
        Me.cGroups.RowHeadersVisible = False
        Me.cGroups.Size = New System.Drawing.Size(271, 67)
        Me.cGroups.TabIndex = 64
        '
        'btnDeleteState
        '
        Me.btnDeleteState.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnDeleteState.Location = New System.Drawing.Point(824, 348)
        Me.btnDeleteState.Name = "btnDeleteState"
        Me.btnDeleteState.Size = New System.Drawing.Size(60, 23)
        Me.btnDeleteState.TabIndex = 63
        Me.btnDeleteState.Text = "Eliminar"
        Me.btnDeleteState.UseVisualStyleBackColor = True
        '
        'btnDeleteRol
        '
        Me.btnDeleteRol.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnDeleteRol.Location = New System.Drawing.Point(826, 240)
        Me.btnDeleteRol.Name = "btnDeleteRol"
        Me.btnDeleteRol.Size = New System.Drawing.Size(60, 23)
        Me.btnDeleteRol.TabIndex = 62
        Me.btnDeleteRol.Text = "Eliminar"
        Me.btnDeleteRol.UseVisualStyleBackColor = True
        '
        'cbStatesSelectAll
        '
        Me.cbStatesSelectAll.AutoSize = True
        Me.cbStatesSelectAll.Location = New System.Drawing.Point(624, 349)
        Me.cbStatesSelectAll.Name = "cbStatesSelectAll"
        Me.cbStatesSelectAll.Size = New System.Drawing.Size(15, 14)
        Me.cbStatesSelectAll.TabIndex = 61
        Me.cbStatesSelectAll.UseVisualStyleBackColor = True
        '
        'cbRolesSelectAll
        '
        Me.cbRolesSelectAll.AutoSize = True
        Me.cbRolesSelectAll.Location = New System.Drawing.Point(626, 240)
        Me.cbRolesSelectAll.Name = "cbRolesSelectAll"
        Me.cbRolesSelectAll.Size = New System.Drawing.Size(15, 14)
        Me.cbRolesSelectAll.TabIndex = 60
        Me.cbRolesSelectAll.UseVisualStyleBackColor = True
        '
        'CheckBox2
        '
        Me.CheckBox2.AutoSize = True
        Me.CheckBox2.Location = New System.Drawing.Point(29, 466)
        Me.CheckBox2.Name = "CheckBox2"
        Me.CheckBox2.Size = New System.Drawing.Size(15, 14)
        Me.CheckBox2.TabIndex = 59
        Me.CheckBox2.UseVisualStyleBackColor = True
        '
        'cbControlsSelectAll
        '
        Me.cbControlsSelectAll.AutoSize = True
        Me.cbControlsSelectAll.Location = New System.Drawing.Point(22, 349)
        Me.cbControlsSelectAll.Name = "cbControlsSelectAll"
        Me.cbControlsSelectAll.Size = New System.Drawing.Size(15, 14)
        Me.cbControlsSelectAll.TabIndex = 59
        Me.cbControlsSelectAll.UseVisualStyleBackColor = True
        '
        'Button2
        '
        Me.Button2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button2.Location = New System.Drawing.Point(1204, 436)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(10, 27)
        Me.Button2.TabIndex = 58
        Me.Button2.Text = "Impedir"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'Button1
        '
        Me.Button1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button1.Location = New System.Drawing.Point(1206, 300)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(10, 27)
        Me.Button1.TabIndex = 58
        Me.Button1.Text = "Impedir"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'cEnabled
        '
        Me.cEnabled.AutoSize = True
        Me.cEnabled.Location = New System.Drawing.Point(344, 352)
        Me.cEnabled.Name = "cEnabled"
        Me.cEnabled.Size = New System.Drawing.Size(65, 17)
        Me.cEnabled.TabIndex = 56
        Me.cEnabled.Text = "Enabled"
        Me.cEnabled.UseVisualStyleBackColor = True
        '
        'CheckBox1
        '
        Me.CheckBox1.AutoSize = True
        Me.CheckBox1.Location = New System.Drawing.Point(1351, 598)
        Me.CheckBox1.Name = "CheckBox1"
        Me.CheckBox1.Size = New System.Drawing.Size(56, 17)
        Me.CheckBox1.TabIndex = 55
        Me.CheckBox1.Text = "Visible"
        Me.CheckBox1.UseVisualStyleBackColor = True
        '
        'lbSupervisar
        '
        Me.lbSupervisar.AutoSize = True
        Me.lbSupervisar.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbSupervisar.Location = New System.Drawing.Point(204, 352)
        Me.lbSupervisar.Name = "lbSupervisar"
        Me.lbSupervisar.Size = New System.Drawing.Size(71, 13)
        Me.lbSupervisar.TabIndex = 54
        Me.lbSupervisar.Text = "Supervisar:"
        '
        'cVisibility
        '
        Me.cVisibility.AutoSize = True
        Me.cVisibility.Checked = True
        Me.cVisibility.CheckState = System.Windows.Forms.CheckState.Checked
        Me.cVisibility.Location = New System.Drawing.Point(283, 352)
        Me.cVisibility.Name = "cVisibility"
        Me.cVisibility.Size = New System.Drawing.Size(56, 17)
        Me.cVisibility.TabIndex = 55
        Me.cVisibility.Text = "Visible"
        Me.cVisibility.UseVisualStyleBackColor = True
        '
        'lbEstados
        '
        Me.lbEstados.AutoSize = True
        Me.lbEstados.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbEstados.Location = New System.Drawing.Point(612, 260)
        Me.lbEstados.Name = "lbEstados"
        Me.lbEstados.Size = New System.Drawing.Size(56, 13)
        Me.lbEstados.TabIndex = 54
        Me.lbEstados.Text = "Estados:"
        '
        'lbRoles
        '
        Me.lbRoles.AutoSize = True
        Me.lbRoles.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbRoles.Location = New System.Drawing.Point(614, 150)
        Me.lbRoles.Name = "lbRoles"
        Me.lbRoles.Size = New System.Drawing.Size(43, 13)
        Me.lbRoles.TabIndex = 54
        Me.lbRoles.Text = "Roles:"
        '
        'lblControles
        '
        Me.lblControles.AutoSize = True
        Me.lblControles.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblControles.Location = New System.Drawing.Point(10, 38)
        Me.lblControles.Name = "lblControles"
        Me.lblControles.Size = New System.Drawing.Size(60, 13)
        Me.lblControles.TabIndex = 54
        Me.lblControles.Text = "Controles"
        '
        'cStates
        '
        DataGridViewCellStyle22.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle22.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle22.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle22.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle22.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle22.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle22.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.cStates.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle22
        Me.cStates.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewCellStyle23.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle23.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle23.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle23.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle23.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle23.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle23.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.cStates.DefaultCellStyle = DataGridViewCellStyle23
        Me.cStates.Location = New System.Drawing.Point(615, 276)
        Me.cStates.Name = "cStates"
        DataGridViewCellStyle24.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle24.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle24.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle24.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle24.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle24.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle24.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.cStates.RowHeadersDefaultCellStyle = DataGridViewCellStyle24
        Me.cStates.RowHeadersVisible = False
        Me.cStates.Size = New System.Drawing.Size(269, 67)
        Me.cStates.TabIndex = 50
        '
        'cControls
        '
        Me.cControls.AllowUserToAddRows = False
        Me.cControls.AllowUserToDeleteRows = False
        DataGridViewCellStyle25.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle25.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle25.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle25.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle25.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle25.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle25.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.cControls.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle25
        Me.cControls.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewCellStyle26.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle26.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle26.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle26.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle26.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle26.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle26.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.cControls.DefaultCellStyle = DataGridViewCellStyle26
        Me.cControls.Location = New System.Drawing.Point(13, 55)
        Me.cControls.Name = "cControls"
        DataGridViewCellStyle27.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle27.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle27.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle27.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle27.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle27.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle27.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.cControls.RowHeadersDefaultCellStyle = DataGridViewCellStyle27
        Me.cControls.RowHeadersVisible = False
        Me.cControls.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.cControls.Size = New System.Drawing.Size(561, 288)
        Me.cControls.TabIndex = 50
        '
        'cRoles
        '
        DataGridViewCellStyle28.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle28.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle28.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle28.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle28.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle28.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle28.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.cRoles.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle28
        Me.cRoles.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewCellStyle29.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle29.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle29.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle29.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle29.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle29.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle29.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.cRoles.DefaultCellStyle = DataGridViewCellStyle29
        Me.cRoles.Location = New System.Drawing.Point(617, 167)
        Me.cRoles.Name = "cRoles"
        DataGridViewCellStyle30.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle30.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle30.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle30.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle30.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle30.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle30.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.cRoles.RowHeadersDefaultCellStyle = DataGridViewCellStyle30
        Me.cRoles.RowHeadersVisible = False
        Me.cRoles.Size = New System.Drawing.Size(269, 67)
        Me.cRoles.TabIndex = 50
        '
        'btnDeleteGroup
        '
        Me.btnDeleteGroup.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnDeleteGroup.Location = New System.Drawing.Point(830, 126)
        Me.btnDeleteGroup.Name = "btnDeleteGroup"
        Me.btnDeleteGroup.Size = New System.Drawing.Size(56, 23)
        Me.btnDeleteGroup.TabIndex = 51
        Me.btnDeleteGroup.Text = "Eliminar"
        Me.btnDeleteGroup.UseVisualStyleBackColor = True
        '
        'cbSaveOnApplying
        '
        Me.cbSaveOnApplying.AutoSize = True
        Me.cbSaveOnApplying.Checked = True
        Me.cbSaveOnApplying.CheckState = System.Windows.Forms.CheckState.Checked
        Me.cbSaveOnApplying.Location = New System.Drawing.Point(305, 43)
        Me.cbSaveOnApplying.Name = "cbSaveOnApplying"
        Me.cbSaveOnApplying.Size = New System.Drawing.Size(267, 17)
        Me.cbSaveOnApplying.TabIndex = 68
        Me.cbSaveOnApplying.Text = "Guardar configuración en fichero al aplicar/aceptar"
        Me.cbSaveOnApplying.UseVisualStyleBackColor = True
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.FileName = "OpenFileDialog1"
        '
        'btnAccept
        '
        Me.btnAccept.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.btnAccept.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnAccept.Location = New System.Drawing.Point(736, 674)
        Me.btnAccept.Name = "btnAccept"
        Me.btnAccept.Size = New System.Drawing.Size(74, 23)
        Me.btnAccept.TabIndex = 72
        Me.btnAccept.Text = "Aceptar"
        Me.btnAccept.UseVisualStyleBackColor = True
        '
        'txtState
        '
        Me.txtState.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(205, Byte), Integer))
        Me.txtState.Location = New System.Drawing.Point(200, 0)
        Me.txtState.Name = "txtState"
        Me.txtState.Size = New System.Drawing.Size(80, 20)
        Me.txtState.TabIndex = 77
        '
        'lblEstado
        '
        Me.lblEstado.AutoSize = True
        Me.lblEstado.Location = New System.Drawing.Point(155, 3)
        Me.lblEstado.Name = "lblEstado"
        Me.lblEstado.Size = New System.Drawing.Size(40, 13)
        Me.lblEstado.TabIndex = 78
        Me.lblEstado.Text = "Estado"
        '
        'txtRoles
        '
        Me.txtRoles.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(205, Byte), Integer))
        Me.txtRoles.Location = New System.Drawing.Point(364, 1)
        Me.txtRoles.Name = "txtRoles"
        Me.txtRoles.Size = New System.Drawing.Size(80, 20)
        Me.txtRoles.TabIndex = 79
        '
        'lblRoles
        '
        Me.lblRoles.AutoSize = True
        Me.lblRoles.Location = New System.Drawing.Point(286, 3)
        Me.lblRoles.Name = "lblRoles"
        Me.lblRoles.Size = New System.Drawing.Size(73, 13)
        Me.lblRoles.TabIndex = 80
        Me.lblRoles.Text = "Roles Usuario"
        '
        'lblSituacion
        '
        Me.lblSituacion.AutoSize = True
        Me.lblSituacion.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSituacion.Location = New System.Drawing.Point(3, 4)
        Me.lblSituacion.Name = "lblSituacion"
        Me.lblSituacion.Size = New System.Drawing.Size(133, 13)
        Me.lblSituacion.TabIndex = 78
        Me.lblSituacion.Text = "Situación actual Host:"
        '
        'cHostSituation
        '
        Me.cHostSituation.Controls.Add(Me.btnResize)
        Me.cHostSituation.Controls.Add(Me.lblSituacion)
        Me.cHostSituation.Controls.Add(Me.txtState)
        Me.cHostSituation.Controls.Add(Me.lblRoles)
        Me.cHostSituation.Controls.Add(Me.txtRoles)
        Me.cHostSituation.Controls.Add(Me.lblEstado)
        Me.cHostSituation.Location = New System.Drawing.Point(438, 4)
        Me.cHostSituation.Name = "cHostSituation"
        Me.cHostSituation.Size = New System.Drawing.Size(476, 22)
        Me.cHostSituation.TabIndex = 81
        Me.cHostSituation.Visible = False
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.cConfigFile)
        Me.GroupBox1.Controls.Add(Me.btnLoad)
        Me.GroupBox1.Controls.Add(Me.lbFichero)
        Me.GroupBox1.Controls.Add(Me.cbOnlyActualComponent)
        Me.GroupBox1.Controls.Add(Me.btnSelectFile)
        Me.GroupBox1.Controls.Add(Me.btnSave)
        Me.GroupBox1.Controls.Add(Me.cbSaveOnApplying)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 635)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(596, 66)
        Me.GroupBox1.TabIndex = 82
        Me.GroupBox1.TabStop = False
        '
        'cConfigFile
        '
        Me.cConfigFile.Location = New System.Drawing.Point(96, 13)
        Me.cConfigFile.Name = "cConfigFile"
        Me.cConfigFile.ReadOnly = True
        Me.cConfigFile.Size = New System.Drawing.Size(411, 20)
        Me.cConfigFile.TabIndex = 83
        '
        'cbRestrictionsSelectAll
        '
        Me.cbRestrictionsSelectAll.AutoSize = True
        Me.cbRestrictionsSelectAll.Location = New System.Drawing.Point(22, 180)
        Me.cbRestrictionsSelectAll.Name = "cbRestrictionsSelectAll"
        Me.cbRestrictionsSelectAll.Size = New System.Drawing.Size(15, 14)
        Me.cbRestrictionsSelectAll.TabIndex = 76
        Me.cbRestrictionsSelectAll.UseVisualStyleBackColor = True
        '
        'FrmRestrictionsUIDefinition
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(926, 705)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.cHostSituation)
        Me.Controls.Add(Me.btnAccept)
        Me.Controls.Add(Me.btnApply)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.gbRestrictionsDefinition)
        Me.Controls.Add(Me.gbSecuritySource)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Name = "FrmRestrictionsUIDefinition"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Definición de Seguridad"
        Me.gbSecuritySource.ResumeLayout(False)
        Me.gbSecuritySource.PerformLayout()
        CType(Me.cRestrictions, System.ComponentModel.ISupportInitialize).EndInit()
        Me.gbRestrictionsDefinition.ResumeLayout(False)
        Me.gbRestrictionsDefinition.PerformLayout()
        CType(Me.cGroups, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.cStates, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.cControls, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.cRoles, System.ComponentModel.ISupportInitialize).EndInit()
        Me.cHostSituation.ResumeLayout(False)
        Me.cHostSituation.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnApply As System.Windows.Forms.Button
    Friend WithEvents btnDeleteRestriction As System.Windows.Forms.Button
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents btnDuplicateRestriction As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents gbSecuritySource As System.Windows.Forms.GroupBox
    Friend WithEvents gbRestrictionsDefinition As System.Windows.Forms.GroupBox
    Friend WithEvents btnProhibit As System.Windows.Forms.Button
    Friend WithEvents btnAuthorize As System.Windows.Forms.Button
    Friend WithEvents cEnabled As System.Windows.Forms.CheckBox
    Friend WithEvents cVisibility As System.Windows.Forms.CheckBox
    Friend WithEvents lblControles As System.Windows.Forms.Label
    Friend WithEvents btnDeleteGroup As System.Windows.Forms.Button
    Friend WithEvents lblAviso As System.Windows.Forms.Label
    Friend WithEvents CheckBox1 As System.Windows.Forms.CheckBox
    Friend WithEvents lbSupervisar As System.Windows.Forms.Label
    Friend WithEvents lbEstados As System.Windows.Forms.Label
    Friend WithEvents lbRoles As System.Windows.Forms.Label
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents cbControlsSelectAll As System.Windows.Forms.CheckBox
    Friend WithEvents cRoles As System.Windows.Forms.DataGridView
    Friend WithEvents CheckBox2 As System.Windows.Forms.CheckBox
    Friend WithEvents cStates As System.Windows.Forms.DataGridView
    Friend WithEvents cControls As System.Windows.Forms.DataGridView
    Friend WithEvents cbStatesSelectAll As System.Windows.Forms.CheckBox
    Friend WithEvents cbRolesSelectAll As System.Windows.Forms.CheckBox
    Friend WithEvents cRestrictions As System.Windows.Forms.DataGridView
    Friend WithEvents btnDeleteRol As System.Windows.Forms.Button
    Friend WithEvents btnDeleteState As System.Windows.Forms.Button
    Friend WithEvents cGroups As System.Windows.Forms.DataGridView
    Friend WithEvents cbShowOnlySelectedGroup As System.Windows.Forms.CheckBox
    Friend WithEvents btnAddGroup As System.Windows.Forms.Button
    Friend WithEvents lblGrupos As System.Windows.Forms.Label
    Friend WithEvents btnSelectFile As System.Windows.Forms.Button
    Friend WithEvents cbSaveOnApplying As System.Windows.Forms.CheckBox
    Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents lbFichero As System.Windows.Forms.Label
    Friend WithEvents btnLoad As System.Windows.Forms.Button
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents cbOnlyActualComponent As System.Windows.Forms.CheckBox
    Friend WithEvents btnAccept As System.Windows.Forms.Button
    Friend WithEvents txtRestrictions As System.Windows.Forms.TextBox
    Friend WithEvents btnRestrictionsGrid As System.Windows.Forms.Button
    Friend WithEvents btnRestrictionsTxt As System.Windows.Forms.Button
    Friend WithEvents txtState As System.Windows.Forms.TextBox
    Friend WithEvents lblEstado As System.Windows.Forms.Label
    Friend WithEvents txtRoles As System.Windows.Forms.TextBox
    Friend WithEvents lblRoles As System.Windows.Forms.Label
    Friend WithEvents lblSituacion As System.Windows.Forms.Label
    Friend WithEvents btnResize As System.Windows.Forms.Button
    Friend WithEvents cHostSituation As System.Windows.Forms.Panel
    Friend WithEvents cbFilterOnSelectedRol As System.Windows.Forms.CheckBox
    Friend WithEvents cComponents As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents cConfigFile As System.Windows.Forms.TextBox
    Friend WithEvents cbRestrictionsSelectAll As System.Windows.Forms.CheckBox
End Class

