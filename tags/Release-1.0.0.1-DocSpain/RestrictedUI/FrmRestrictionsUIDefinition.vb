Option Strict On

Imports System.IO
Imports System.ComponentModel
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


''' <summary>
''' Formulario de mantenimiento de la política de restricciones asociada a un componente de seguridad <see cref="ControlRestrictedUI "/>. 
''' </summary>
''' <remarks>
''' <para>Permite tanto la consulta como la modificación de restricciones (permisos y prohibiciones), de grupos de controles,
''' estados y roles (tanto comunes a todos los compontes como particulares a uno sólo).</para>
''' <para>Utilizable tanto en tiempo de diseño como de ejecución (esto último se ofrece sólo para aplicaciones WinForms).</para>
''' <para>Ofrece la posibilidad de recuperar o guardar la seguridad desde y hacia un archivo</para>
''' </remarks>
Friend Class FrmRestrictionsUIDefinition
    Implements INotifyPropertyChanged

#Region "Campos privados"
    Private _securityComponent As ControlRestrictedUI
    Private _parentControlAdapter As IControlAdapter
    Private _configFile As String = Nothing
    Private _InitialRestrictionsDefinition As String() = Nothing
    Private _componentsSecurityBak As New Dictionary(Of String, ComponentSecurity)
    Private _backupAvailable As Boolean = False
    Private _host As IHost = Nothing

    Private ds As DataSet = New DataSet()
    Private dtControls As DataTable
    Private dtRestrictions As DataTable
    Private dtRoles As DataTable
    Private dtStates As DataTable
    Private dtGroups As DataTable

    Private _sizeForm As Size
    Private _posPanelSituationHost As Point
    Private _posForm As Point

    Private _updatingGroups As Boolean = False
    Private _txtRestrictionsChanged As Boolean = False
    Private _findingControl As Object = Nothing

    Private _DesignTime As Boolean = False
    Private _selecControlsMng As GridSelectionMng
    Private _selecRolesMng As GridSelectionMng
    Private _selecStatesMng As GridSelectionMng
    Private _selecRestrictionsMng As GridSelectionMng
#End Region

    Const REDUCE_WINDOW As String = "Reducir la ventana al mínimo para poder jugar con el estado y roles actuales"
    Const RESTORE_WINDOW As String = "Restaurar la ventana a su tamaño original"
    Const SOURCE_FILE As String = " Permisos definidos (desde Archivo de configuración) "
    Const SOURCE_PARAM As String = " Permisos definidos (desde Parámetro facilitado) "
    Const SOURCE_EMBEDED As String = " Permisos definidos (desde valor Embebido en control) "
    Const SOURCE_ACTUAL As String = " Permisos definidos (desde seguridad actualmente Aplicada) "

    ' Guardan las últimas opciones marcadas por el usuario, de modo que se ofrezca la siguiente vez que se abra el formulario de mantenimiento
    Private Shared OPTSaveOnApplying As Boolean = False
    Private Shared OPTOnlyActualComponent As Boolean = False

    Sub New(ByVal securityComponent As ControlRestrictedUI, _
            Optional ByVal initialRestrictionsDefinition As String() = Nothing, _
            Optional ByVal fileConfig As String = Nothing, _
            Optional ByVal host As IHost = Nothing, _
            Optional ByVal designTime As Boolean = False)

        InitializeComponent()

        _securityComponent = securityComponent
        _parentControlAdapter = SecurityEnvironment.GetAdapter(securityComponent.ParentControl)
        _DesignTime = designTime

        _InitialRestrictionsDefinition = initialRestrictionsDefinition
        If fileConfig IsNot Nothing Then
            Me.ConfigFile = fileConfig
        Else
            Me.ConfigFile = securityComponent.ConfigFile
        End If
        SecurityEnvironment.AdaptFilePath(Me.ConfigFile, designTime)
        _host = host

        _selecControlsMng = New GridSelectionMng(cControls, 0, cbControlsSelectAll)
        _selecRolesMng = New GridSelectionMng(cRoles, 0, cbRolesSelectAll)
        _selecStatesMng = New GridSelectionMng(cStates, 0, cbStatesSelectAll)
        _selecRestrictionsMng = New GridSelectionMng(cRestrictions, 0, cbRestrictionsSelectAll)

        CreateDataTableControls()
        CreateDataTableRestrictions()
        CreateDataTableRoles()
        CreateDataTableStates()
        CreateDataTableGroups()
    End Sub

    Public ReadOnly Property RestrictionsDefinition() As String()
        Get
            Return GetDefinedRestrictions(False)
        End Get
    End Property

    Private Property ConfigFile() As String
        Get
            Return _configFile
        End Get
        Set(ByVal value As String)
            _configFile = value
            cConfigFile.Text = value
        End Set
    End Property


    Private Sub Form_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        cControls.DataSource = ds
        cControls.DataMember = "Controls"
        ' Sel, IDControl, Nombre, Tipo, Grupos
        cControls.Columns(0).Width = 28
        cControls.Columns(1).Width = 227
        cControls.Columns(2).Width = 107
        cControls.Columns(3).Width = 170
        cControls.Columns(1).ReadOnly = True
        cControls.Columns(2).ReadOnly = True
        cControls.Columns(3).ReadOnly = True
        cControls.Columns(4).Visible = False

        cRestrictions.DataSource = ds
        cRestrictions.DataMember = "Restrictions"
        ' Sel, Roles, Estados, Permiso, Visible, Enabled, IDControl, Tipo
        cRestrictions.Columns(0).Width = 28
        cRestrictions.Columns(1).Width = 134
        cRestrictions.Columns(2).Width = 114
        cRestrictions.Columns(3).Width = 69
        cRestrictions.Columns(4).Width = 43
        cRestrictions.Columns(5).Width = 52
        cRestrictions.Columns(6).Width = 236
        cRestrictions.Columns(7).Width = 167
        cRestrictions.Columns(3).ReadOnly = True
        cRestrictions.Columns(7).ReadOnly = True

        cRoles.DataSource = ds
        cRoles.DataMember = "Roles"
        ' Sel, ID, Alias, Nombre, Tipo
        cRoles.Columns(0).Width = 28
        cRoles.Columns(1).Width = 43
        cRoles.Columns(2).Width = 29
        cRoles.Columns(3).Width = 130
        cRoles.Columns(4).Width = 19

        cStates.DataSource = ds
        cStates.DataMember = "States"
        ' Sel, ID, Nombre, Tipo
        cStates.Columns(0).Width = 28
        cStates.Columns(1).Width = 26
        cStates.Columns(2).Width = 176
        cStates.Columns(3).Width = 19

        cGroups.DataSource = ds
        cGroups.DataMember = "Groups"
        ' ID, Nombre
        cGroups.Columns(0).Width = 26
        cGroups.Columns(1).Width = 225
        cGroups.Columns(0).ReadOnly = True

        LoadControlsToSupervise()
        LoadSecurity()

        Dim bmp As Bitmap = My.Resources.CanvasScale
        bmp.MakeTransparent()
        btnResize.Image = bmp

        If _host IsNot Nothing Then
            cHostSituation.Visible = True
            Me.ToolTip1.SetToolTip(btnResize, REDUCE_WINDOW)
            Me.txtState.DataBindings.Add("Text", Me, "HostState")
            Me.txtRoles.DataBindings.Add("Text", Me, "UserRolesStr")
            AddHandler _host.StateChanged, AddressOf OnStateChanged
            AddHandler _host.RolesChanged, AddressOf OnRolesChanged

            ' Si host no es Nothing será también indicador de que se está llamando a ese formulario en tpo.de ejecución
            AddHandler _securityComponent.BeforeApplyingRestriction, AddressOf OnBeforeApplyingRestriction
        End If
        cbSaveOnApplying.Checked = OPTSaveOnApplying

        If _DesignTime Then
            cbOnlyActualComponent.Checked = True
            cbOnlyActualComponent.Enabled = False
        Else
            cbOnlyActualComponent.Checked = OPTOnlyActualComponent
            cbOnlyActualComponent.Enabled = True
        End If
    End Sub


    Private Sub LoadSecurity()
        dtStates.Clear()
        dtRoles.Clear()
        _updatingGroups = True
        dtGroups.Clear()
        _updatingGroups = False
        dtRestrictions.Clear()

        LoadStatesAndRoles()

        Dim commonRoles As List(Of Rol) = GetCommonRoles()
        Dim particularRoles As List(Of Rol) = GetParticularRoles()

        ' En tpo. de ejecución el formulario recibirá a la entrada opcionalmente un String() con líneas de permisos (y grupos incluidos) a utilizar 
        ' como valores por defecto
        ' Si ese String() no se aporta se utilizarán como permisos y grupos los siguientes:
        '  * Si estamos en tiempo de diseño
        '    - Se utilizará la definición de permisos embebida en la propiedad del control
        '      (Si se quiere trabajar sobre la seguridad del archivo de configuración bastará con cargarlo. Por defecto ya se apuntará a ese 
        '       archivo) 
        '  * Si estamos en tiempo de ejecución
        '    - Se utilizará la seguridad que esté en ese momento siendo aplicada
        '   (En ambos casos si se quiere trabajar sobre la seguridad del archivo de configuración bastará con cargarlo. Por defecto ya se apuntará a ese 
        '    archivo) 


        Dim defSecurity As UIRestrictions
        If _InitialRestrictionsDefinition IsNot Nothing Then
            Dim Restrictions As String() = Nothing
            Dim Groups As Group() = Nothing
            SecurityEnvironment.GetAuthorizationsAndGroups(_InitialRestrictionsDefinition, Restrictions, Groups)
            defSecurity = New UIRestrictions(Restrictions, _securityComponent.ID, Nothing, Nothing)  ' ControlPadre a Nothing porque no queremos buscar los IAdaptdoresControles ni traducir los Grupos

            If Groups.Length > 0 Then
                LoadGroups(Groups)
            Else
                LoadGroups(SecurityEnvironment.GetGroups(SecurityEnvironment.GetRestrictionsDefinition(_securityComponent.ID)))
            End If
            LoadRestrictions(defSecurity, commonRoles, particularRoles.ToArray)
            gbSecuritySource.Text = SOURCE_PARAM
        Else
            defSecurity = New UIRestrictions(_securityComponent.Restrictions, _securityComponent.ID, Nothing, Nothing)  ' ControlPadre a Nothing porque no queremos buscar los IAdaptdoresControles ni traducir los Grupos
            LoadGroups(_securityComponent.Groups)
            LoadRestrictions(defSecurity, commonRoles, particularRoles.ToArray)
            If _DesignTime Then
                gbSecuritySource.Text = SOURCE_EMBEDED
            Else
                gbSecuritySource.Text = SOURCE_ACTUAL
            End If
        End If

    End Sub

    Private Sub LoadStatesAndRoles()
        Dim SF As New ComponentSecurity
        Dim existsSecComp As Boolean = False

        existsSecComp = SecurityEnvironment.ComponentsSecurity.TryGetValue(_securityComponent.ID, SF)

        For Each E As State In SecurityEnvironment.CommonStates
            AddState(E, False)
        Next
        If existsSecComp AndAlso SF.States IsNot Nothing Then
            For Each E As State In SF.States
                AddState(E, True)
            Next
        End If

        For Each R As Rol In SecurityEnvironment.CommonRoles
            AddRol(R, False)
        Next
        If existsSecComp AndAlso SF.Roles IsNot Nothing Then
            For Each R As Rol In SF.Roles
                AddRol(R, True)
            Next
        End If

        dtStates.AcceptChanges()
        dtRoles.AcceptChanges()
    End Sub


    Private Sub ControlSeguridad_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed
        If _host IsNot Nothing Then
            RemoveHandler _host.StateChanged, AddressOf OnStateChanged
            RemoveHandler _host.RolesChanged, AddressOf OnRolesChanged
            RemoveHandler _securityComponent.BeforeApplyingRestriction, AddressOf OnBeforeApplyingRestriction
        End If
    End Sub


#Region "Creación de objetos DataTable"

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
    End Sub

    Private Sub CreateDataTableRestrictions()
        Dim c As DataColumn
        dtRestrictions = New DataTable("Restrictions")
        c = dtRestrictions.Columns.Add("Sel", GetType(Boolean))
        c.DefaultValue = False
        dtRestrictions.Columns.Add("Roles", GetType(String))
        dtRestrictions.Columns.Add("States", GetType(String))
        dtRestrictions.Columns.Add("Restriction", GetType(String))
        dtRestrictions.Columns.Add("Visible", GetType(Boolean))
        dtRestrictions.Columns.Add("Enabled", GetType(Boolean))
        dtRestrictions.Columns.Add("IDControl", GetType(String))
        dtRestrictions.Columns.Add("Type", GetType(String))
        ds.Tables.Add(dtRestrictions)
    End Sub

    Private Sub CreateDataTableRoles()
        Dim c As DataColumn
        dtRoles = New DataTable("Roles")
        c = dtRoles.Columns.Add("Sel", GetType(Boolean))
        c.DefaultValue = False
        dtRoles.Columns.Add("ID", GetType(Int32))
        c = dtRoles.Columns.Add("Alias", GetType(String))
        c.DefaultValue = ""
        c = dtRoles.Columns.Add("Name", GetType(String))
        c.DefaultValue = ""
        c = dtRoles.Columns.Add("Particular", GetType(Boolean))
        c.DefaultValue = False
        ds.Tables.Add(dtRoles)
    End Sub

    Private Sub CreateDataTableStates()
        Dim c As DataColumn
        dtStates = New DataTable("States")
        c = dtStates.Columns.Add("Sel", GetType(Boolean))
        c.DefaultValue = False
        dtStates.Columns.Add("ID", GetType(Int32))
        c = dtStates.Columns.Add("Name", GetType(String))
        c.DefaultValue = ""
        c = dtStates.Columns.Add("Particular", GetType(Boolean))
        c.DefaultValue = False
        ds.Tables.Add(dtStates)
    End Sub

    Private Sub CreateDataTableGroups()
        dtGroups = New DataTable("Groups")
        dtGroups.Columns.Add("ID", GetType(Int32))
        dtGroups.Columns.Add("Name", GetType(String))
        ds.Tables.Add(dtGroups)
    End Sub

#End Region


#Region "Componentes"
    Private Sub cComponents_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cComponents.SelectedIndexChanged
        Dim c As ControlRestrictedUI = DirectCast(cComponents.Items(cComponents.SelectedIndex), ControlRestrictedUI)

        If c Is _securityComponent Then Exit Sub

        RestoreBackupSecurity()

        If _host IsNot Nothing Then
            ' Si host no es Nothing será también indicador de que se está llamando a ese formulario en tpo.de ejecución
            RemoveHandler _securityComponent.BeforeApplyingRestriction, AddressOf OnBeforeApplyingRestriction
            AddHandler c.BeforeApplyingRestriction, AddressOf OnBeforeApplyingRestriction
        End If

        _securityComponent = c
        _parentControlAdapter = SecurityEnvironment.GetAdapter(c.ParentControl)

        _InitialRestrictionsDefinition = Nothing
        If c.ConfigFile IsNot Nothing Then
            Me.ConfigFile = c.ConfigFile
            SecurityEnvironment.AdaptFilePath(Me.ConfigFile, _DesignTime)
        End If

        LoadControlsToSupervise()
        LoadSecurity()

        If Not cRestrictions.Visible Then
            btnRestrictionsTxt_MouseClick(Nothing, Nothing)
        End If

    End Sub

    Private Sub cComponents_DropDown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cComponents.DropDown
        Me.ToolTip1.SetToolTip(Me.cComponents, "AVISO: Los cambios no salvados sobre el componente de seguridad actual se perderán al seleccionar otro")
    End Sub

    Private Sub cComponents_DropDownClosed(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cComponents.DropDownClosed
        Me.ToolTip1.SetToolTip(Me.cComponents, "")
    End Sub

#End Region

#Region "Controles supervisables"

    ''' <summary>
    ''' Cargar los controles que pueden ser supervisados o 'vigilados'
    ''' </summary>
    ''' <remarks>En tiempo de diseño se da prioridad al fichero de controles en el caso de que exista</remarks>
    Private Sub LoadControlsToSupervise()
        Dim cargarDesdeArchivo As Boolean = False

        dtControls.Clear()

        Dim fichero As String = _securityComponent.ControlsFile
        If Not _DesignTime Then  ' Tpo Ejecución -> no usar archivo de controles
            cargarDesdeArchivo = False
        ElseIf SecurityEnvironment.AdaptFilePath(fichero, _DesignTime) Then
            cargarDesdeArchivo = True
        ElseIf _parentControlAdapter IsNot Nothing AndAlso TypeOf _parentControlAdapter.Control Is System.Windows.Forms.Control Then
            cargarDesdeArchivo = False
        Else
            MessageBox.Show("No es posible mostrar los controles del formulario" + vbCrLf + _
                            "(Para formularios Web en tiempo de diseño se precisa leer los controles desde un archivo. Utilice la propiedad " + _
                            "'FicheroControles' del componente de seguridad. Este fichero se puede cargar en tiempo de ejecución tanto de " + _
                            "forma automática como llamando a RegistrarControlesDeFormulario() en el componente)")
            Me.Close()
        End If

        ' En este combo incluiremos todos los componentes de seguridad (ControlRestriccionesUI) accesibles desde aquí
        If cComponents.SelectedIndex = -1 Then
            cComponents.Items.Add(_securityComponent)
        End If

        If cargarDesdeArchivo Then
            If TypeOf _parentControlAdapter.Control Is System.Windows.Forms.Control Then
                LoadControlsFrom(_parentControlAdapter)
            End If
            LoadControlsAvailableIn(fichero)
            lblAviso.Text = "Se incluyen controles leídos desde: " + fichero
        Else
            LoadControlsFrom(_parentControlAdapter)
        End If
        cControls.Sort(cControls.Columns("IDControl"), ListSortDirection.Ascending)
        cControls.FirstDisplayedScrollingRowIndex = 0

        If cComponents.SelectedIndex = -1 Then
            cComponents.SelectedIndex = 0
        End If
    End Sub

    Private Sub LoadControlsFrom(ByVal control As IControlAdapter, Optional ByVal parent As IControlAdapter = Nothing)
        If Not control Is _parentControlAdapter Then
            AddControl(control, parent)
        End If
        For Each c As IControlAdapter In control.Controls
            LoadControlsFrom(c, control)
        Next
    End Sub

    Private Sub LoadControlsAvailableIn(ByVal file As String)
        ' La identificación del control padre podemos hacerla sin problemas en WinForms, pero en Web no es posible (al menos no lo consigo)
        ' Por ello, en lugar de apoyarnos en el control padre para identificar el formulario, utilizaremos el identificador del componente
        ' de seguridad que está en él embebido
        'Dim idParent As String = Util.GetControlPadreID(_controlSeguridad.ParentControl)
        Dim idParent As String = _securityComponent.ID

        For Each Control As String In _securityComponent.ReadComponentControls(file, idParent)
            If String.IsNullOrEmpty(Control) Then Continue For
            If Control(0) <> "["c Then
                AddControl(Control)
            End If
        Next
    End Sub

    Private Sub AddControl(ByVal adaptControl As IControlAdapter, ByVal parent As IControlAdapter)
        Dim name As String
        Dim pos As Integer
        Dim IDControl As String = adaptControl.Identification(parent, _securityComponent)

        pos = IDControl.LastIndexOf("."c)
        If pos >= 0 Then
            name = IDControl.Substring(pos + 1)
        Else
            name = IDControl
        End If

        Dim r As DataRow = dtControls.NewRow
        r("IDControl") = IDControl
        r("Name") = name
        r("Type") = adaptControl.Control.GetType.ToString
        r("Groups") = ""
        dtControls.Rows.Add(r)

        LocateRestrictedUIComponents(adaptControl.Control)
    End Sub

    Private Sub LocateRestrictedUIComponents(ByVal control As Object)
        If cComponents.SelectedIndex <> -1 Then Exit Sub

        If GetType(UserControl).IsInstanceOfType(control) Then
            Dim t As Type = control.GetType
            For Each f As FieldInfo In t.GetFields(BindingFlags.NonPublic Or BindingFlags.Instance)
                If GetType(ControlRestrictedUI).IsAssignableFrom(f.FieldType) Then
                    Dim c As ControlRestrictedUI = DirectCast(f.GetValue(control), ControlRestrictedUI)
                    cComponents.Items.Add(c)
                    Exit Sub
                End If
            Next
        End If
    End Sub

    Private Sub AddControl(ByVal IDControl As String)
        Dim name As String
        Dim type As String = ""
        Dim pos As Integer

        ' Si hay un archivo de controles éste se procesará después de recorrer los hijos del control padre (salvo que sea un control Web)
        ' Por eso tenemos que comprobar que no se haya añadido ya
        If dtControls.Select("IDControl='" & IDControl & "'").Length > 0 Then Exit Sub

        pos = IDControl.LastIndexOf("."c)
        If pos >= 0 Then
            name = IDControl.Substring(pos + 1)
        Else
            name = IDControl
        End If

        ' Para poder mostrar el tipo tenemos que intentar localizar el control
        Try
            Dim controlAdapt As IControlAdapter
            controlAdapt = SecurityEnvironment.GetAdapter(_securityComponent.ParentControl).FindControl(IDControl)
            If Not controlAdapt.IsNull Then
                type = controlAdapt.Control.GetType.ToString()
                LocateRestrictedUIComponents(controlAdapt.Control)
            End If
        Catch ex As Exception
        End Try

        Dim r As DataRow = dtControls.NewRow
        r("IDControl") = IDControl
        r("Name") = name
        r("Type") = type
        r("Groups") = ""
        dtControls.Rows.Add(r)
    End Sub


    Private Sub cbControlsSelectAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbControlsSelectAll.Click
        For Each r As DataRow In dtControls.Rows
            r("Sel") = cbControlsSelectAll.Checked
        Next
    End Sub

    Private Sub cbControlsSelectAll_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbControlsSelectAll.CheckedChanged
        CheckEnabledRestrictionsButtons()
    End Sub


    Private Sub cControls_CellContentDoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles cControls.CellContentDoubleClick
        Dim controlAdapt As IControlAdapter
        Dim IDControl As String = DirectCast(cControls.Rows(e.RowIndex).Cells("IDControl").Value, String)

        controlAdapt = SecurityEnvironment.GetAdapter(_securityComponent.ParentControl).FindControl(IDControl)
        If Not controlAdapt.IsNull Then
            _findingControl = controlAdapt.Control
            FindControl(controlAdapt)
            _findingControl = Nothing
        End If
    End Sub

    Private Sub FindControl(ByVal controlAdapt As IControlAdapter)
        Dim visibilityState As Boolean
        Dim cursorBak As Cursor = Me.Cursor
        visibilityState = controlAdapt.Visible
        Me.Cursor = Cursors.WaitCursor
        Try
            For i As Integer = 1 To 15
                controlAdapt.Visible = Not controlAdapt.Visible
                Application.DoEvents()
                System.Threading.Thread.Sleep(100)
            Next
            controlAdapt.Visible = visibilityState
        Finally
            Me.Cursor = cursorBak
        End Try
    End Sub

    Private Sub OnBeforeApplyingRestriction(ByVal adaptControl As IControlAdapter, ByVal tipo As RestrictedUI.ControlRestrictedUI.TChange, ByRef allowChange As Boolean)
        If adaptControl.Control Is _findingControl Then
            allowChange = True
        End If
    End Sub

#End Region

#Region "Groups"
    Private Sub LoadGroups(ByVal groups As Group())
        Dim included As Boolean

        If groups Is Nothing Then Exit Sub

        For Each G As Group In groups

            For Each ctrl As DataRow In dtControls.Rows
                Dim idControl As String = CType(ctrl("IDControl"), String).ToUpper

                included = False
                For Each ctrlGrupo As String In G.Controls
                    If ctrlGrupo.ToUpper = idControl Then
                        included = True
                        Exit For
                    End If
                Next
                ctrl("Sel") = included
            Next
            AddGroup(G.Name)
        Next
        dtGroups.AcceptChanges()
    End Sub

    Private Sub AddGroup(Optional ByVal nameGroup As String = "")
        Dim IDgroup As Integer
        Dim r As DataRow
        Dim updateGroup As Boolean = False
        Dim groups() As String
        Dim index As Integer

        If dtGroups.Rows.Count = 0 Then
            IDgroup = 0
        Else
            IDgroup = CType(dtGroups.Rows(dtGroups.Rows.Count - 1)("ID"), Integer) + 1
        End If

        If String.IsNullOrEmpty(nameGroup) Then
            nameGroup = InputBox("Introduzca un nombre para el nuevo grupo", "Creación de grupos", "Grupo " + IDgroup.ToString)

            'Verificar si el nombre ya existe. Si es así advertir de que esta operación actualizará el grupo con la nueva selección de controles
            For Each r In dtGroups.Rows
                If CType(r("Name"), String) = nameGroup Then
                    If MsgBox("Esta operación actualizará el grupo con la nueva selección de controles.", MsgBoxStyle.OkCancel Or MsgBoxStyle.Exclamation, "Creación de grupos") = MsgBoxResult.Cancel Then
                        Exit Sub
                    End If
                    updateGroup = True
                    IDgroup = CType(r("ID"), Integer)
                    Exit For
                End If
            Next
        End If

        _updatingGroups = True
        Try
            If Not updateGroup Then
                r = dtGroups.NewRow
                r("ID") = IDgroup
                r("Name") = nameGroup
                dtGroups.Rows.Add(r)
                dtGroups.AcceptChanges()
            End If


            For Each ctrl As DataRow In dtControls.Rows
                groups = Util.ConvierteEnArrayStr(ctrl("Groups").ToString)
                index = Array.IndexOf(groups, IDgroup.ToString)
                If CType(ctrl("Sel"), Boolean) Then
                    If index < 0 Then
                        ctrl("Groups") = ctrl("Groups").ToString + ", " + IDgroup.ToString
                    End If

                Else  ' Si el grupo existía y el control ha sido ahora deseleccionado eliminar la referencia al grupo para este control
                    If updateGroup And index >= 0 Then
                        groups(index) = ""
                        ctrl("Groups") = Util.ConvertToString(groups)
                    End If
                End If
            Next
            dtControls.AcceptChanges()

        Finally
            _updatingGroups = False
        End Try

    End Sub

    Private Sub DeleteGroup()
        Dim IDgroup As String
        Dim r As DataRow
        Dim groups() As String
        Dim index As Integer

        If cGroups.CurrentRow Is Nothing Then Exit Sub

        r = DirectCast(cGroups.CurrentRow.DataBoundItem, DataRowView).Row
        IDgroup = r("ID").ToString

        OnGroupChanged(r("Name").ToString, "")

        r.Delete()
        dtGroups.AcceptChanges()

        For Each ctrl As DataRow In dtControls.Rows
            groups = Util.ConvierteEnArrayStr(ctrl("Groups").ToString)
            index = Array.IndexOf(groups, IDgroup)
            If index >= 0 Then
                groups(index) = ""
                ctrl("Groups") = Util.ConvertToString(groups)
            End If
        Next

    End Sub

    Private Sub ShowGroup(ByVal IDgroup As String)
        Dim groups() As String
        Dim show As Boolean
        Dim firstControlInGroup As Boolean = Nothing

        cControls.ClearSelection()
        For Each ctrl As DataGridViewRow In cControls.Rows
            groups = Util.ConvierteEnArrayStr(ctrl.Cells("Groups").Value.ToString)
            show = Array.IndexOf(groups, IDgroup) >= 0
            ctrl.Cells("Sel").Value = show
            If show And Not firstControlInGroup Then
                firstControlInGroup = True
                ctrl.Visible = True
                cControls.CurrentCell = ctrl.Cells("Sel")
            End If
        Next

        cControls.CurrentCell = Nothing
        For Each ctrl As DataGridViewRow In cControls.Rows
            show = CType(ctrl.Cells("Sel").Value, Boolean)
            ctrl.Selected = show
            If cbShowOnlySelectedGroup.Checked Then
                ctrl.Visible = show
            End If
        Next

    End Sub

    Private Function GetControlsGroup(ByVal IDgroup As String) As String()
        Dim groups() As String
        Dim included As Boolean
        Dim list As New List(Of String)

        For Each ctrl As DataGridViewRow In cControls.Rows
            groups = Util.ConvierteEnArrayStr(DirectCast(ctrl.Cells("Groups").Value, String))
            included = Array.IndexOf(groups, IDgroup) >= 0
            If included Then
                list.Add(DirectCast(ctrl.Cells("IDControl").Value, String))
            End If
        Next
        Return list.ToArray

    End Function

    Private Sub cGroups_RowValidating(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellCancelEventArgs) Handles cGroups.RowValidating
        Dim row As DataRow
        Dim oldName, newName As String
        Try
            row = DirectCast(cGroups.Rows(e.RowIndex).DataBoundItem, DataRowView).Row
        Catch ex As Exception
            Exit Sub
        End Try

        If row.HasVersion(DataRowVersion.Proposed) Then
            oldName = row("Name", DataRowVersion.Current).ToString
            newName = row("Name", DataRowVersion.Proposed).ToString

            If oldName <> newName Then
                OnGroupChanged(oldName, newName)
            End If

        End If
    End Sub


    Private Sub cGroups_SelectionChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cGroups.SelectionChanged
        Dim r As DataRow
        Dim IDGroup As String

        If _updatingGroups Or cGroups.CurrentRow Is Nothing Then
            cbShowOnlySelectedGroup.Checked = False
            cbShowOnlySelectedGroup.Enabled = False
        Else
            cbShowOnlySelectedGroup.Enabled = True
            r = DirectCast(cGroups.CurrentRow.DataBoundItem, DataRowView).Row
            IDGroup = r("ID").ToString
            ShowGroup(IDGroup)
        End If
    End Sub

    Private Sub btnAddGroup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddGroup.Click
        AddGroup()
    End Sub

    Private Sub btnDeleteGroup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDeleteGroup.Click
        DeleteGroup()
    End Sub

    Private Sub cbShowOnlySelectedGroup_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbShowOnlySelectedGroup.CheckedChanged
        If Not cbShowOnlySelectedGroup.Checked Then
            ShowAllControls()
        End If

        If cGroups.CurrentRow IsNot Nothing Then
            cGroups_SelectionChanged(Nothing, Nothing)
        End If
    End Sub

    Private Sub ShowAllControls()
        For Each ctrl As DataGridViewRow In cControls.Rows
            ctrl.Visible = True
        Next
    End Sub


    Private Sub OnGroupChanged(ByVal oldName As String, Optional ByVal newName As String = "")
        Dim groupName As String
        If String.IsNullOrEmpty(oldName) Then Exit Sub

        oldName = "$" + oldName.ToUpper

        For r As Integer = 0 To dtRestrictions.Rows.Count - 1
            Dim row As DataRow = dtRestrictions.Rows(r)
            groupName = DirectCast(row("IDControl"), String).ToUpper
            If groupName = oldName Then
                If String.IsNullOrEmpty(newName) Then
                    row.Delete()
                Else
                    row("IDControl") = "$" + newName
                End If
            End If
        Next

        dtRestrictions.AcceptChanges()
    End Sub

#End Region

#Region "Roles"
    Private Sub AddRol(ByVal R As Rol, ByVal particular As Boolean)
        Dim row As DataRow = dtRoles.NewRow
        row("Sel") = False
        row("ID") = R.ID
        row("Alias") = R.Alias
        row("Name") = R.Name
        row("Particular") = particular
        dtRoles.Rows.Add(row)
    End Sub

    Private Sub cbRolesSelectAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbRolesSelectAll.Click
        For Each r As DataRow In dtRoles.Rows
            r("Sel") = cbRolesSelectAll.Checked
        Next
    End Sub

    Private Sub cbRolesSelectAll_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbRolesSelectAll.CheckedChanged
        cbFilterOnSelectedRol.Enabled = cbRolesSelectAll.Checked
        cbFilterOnSelectedRol_CheckStateChanged(Nothing, Nothing)
    End Sub

    Private Sub OnRolChanged(ByVal oldAlias As String, ByVal newAlias As String, Optional ByVal oldID As String = "", Optional ByVal newID As String = "")
        Dim roles() As String
        Dim changeTo As String

        If Not String.IsNullOrEmpty(newAlias) Then
            changeTo = newAlias
        Else
            changeTo = newID
        End If

        For r As Integer = 0 To dtRestrictions.Rows.Count - 1
            Dim row As DataRow = dtRestrictions.Rows(r)
            roles = Util.ConvierteEnArrayStr(row("Roles").ToString)
            For i As Integer = 0 To roles.Length - 1
                If roles(i) = oldAlias OrElse roles(i) = oldID Then
                    roles(i) = changeTo
                    If changeTo = "" AndAlso roles.Length = 1 Then
                        row.Delete()
                    Else
                        row("Roles") = Util.ConvertToString(roles)
                    End If

                    Exit For
                End If
            Next
        Next

        dtRestrictions.AcceptChanges()
    End Sub

    Private Sub cRoles_RowValidating(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellCancelEventArgs) Handles cRoles.RowValidating
        Dim row As DataRow
        Try
            row = DirectCast(cRoles.Rows(e.RowIndex).DataBoundItem, DataRowView).Row
        Catch ex As Exception
            Exit Sub
        End Try

        Dim id As Object = row("ID")
        If id Is System.DBNull.Value Then
            MsgBox("Debe especificarse un ID de rol", MsgBoxStyle.Exclamation)
            e.Cancel = True
        Else

            Dim oldAlias, nuevoAlias As String
            Dim oldID, nuevoID As String

            If row.HasVersion(DataRowVersion.Current) And row.HasVersion(DataRowVersion.Proposed) Then
                oldAlias = row("Alias", DataRowVersion.Current).ToString
                nuevoAlias = row("Alias", DataRowVersion.Proposed).ToString
                oldID = row("ID", DataRowVersion.Current).ToString
                nuevoID = row("ID", DataRowVersion.Proposed).ToString

                If (oldAlias <> "" And oldAlias = nuevoAlias) Then
                    Exit Sub
                ElseIf oldAlias <> nuevoAlias Or oldID <> nuevoID Then
                    OnRolChanged(oldAlias, nuevoAlias, oldID, nuevoID)
                End If
            End If

        End If
    End Sub

    Private Sub cRoles_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cRoles.SelectionChanged
        cbFilterOnSelectedRol_CheckStateChanged(Nothing, Nothing)
    End Sub

    Private Sub btnDeleteRol_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDeleteRol.Click
        Dim row As DataRow
        Dim r As DataGridViewRow
        Dim changes As Boolean = False
        If cRoles.Rows.Count = 1 Then Exit Sub

        For i As Integer = cRoles.Rows.Count - 1 To 0 Step -1
            r = cRoles.Rows(i)
            If r.DataBoundItem Is Nothing Then Continue For
            row = DirectCast(r.DataBoundItem, DataRowView).Row
            If DirectCast(row("Sel"), Boolean) Then
                OnRolChanged(row("Alias").ToString, "", row("ID").ToString)
                row.Delete()
                changes = True
            End If
        Next
        If changes Then
            dtRoles.AcceptChanges()
        End If
    End Sub

    Private Function GetCommonRoles() As List(Of Rol)
        Dim commonRoles As New List(Of Rol)
        Dim rol As Rol
        For Each r As DataRow In dtRoles.Rows
            If Not DirectCast(r("Particular"), Boolean) Then
                rol.ID = DirectCast(r("ID"), Integer)
                If r("Alias") Is DBNull.Value Then rol.Alias = "" Else rol.Alias = DirectCast(r("Alias"), String)
                If r("Name") Is DBNull.Value Then rol.Name = "" Else rol.Name = DirectCast(r("Name"), String)
                commonRoles.Add(rol)
            End If
        Next
        Return commonRoles
    End Function

    Private Function GetParticularRoles() As List(Of Rol)
        Dim particularRoles As New List(Of Rol)
        Dim rol As Rol
        For Each r As DataRow In dtRoles.Rows
            If DirectCast(r("Particular"), Boolean) Then
                rol.ID = DirectCast(r("ID"), Integer)
                rol.Alias = DirectCast(r("Alias"), String)
                rol.Name = DirectCast(r("Name"), String)
                particularRoles.Add(rol)
            End If
        Next
        Return particularRoles
    End Function

    Private Sub cRoles_DataError(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles cRoles.DataError
        MsgBox("Dato introducido incorrecto: " + e.Exception.Message)
    End Sub

#End Region

#Region "States"

    Private Sub AddState(ByVal E As State, ByVal particular As Boolean)
        Dim row As DataRow = dtStates.NewRow
        row("Sel") = False
        row("ID") = E.ID
        row("Name") = E.Name
        row("Particular") = particular
        dtStates.Rows.Add(row)
    End Sub

    Private Sub cbStatesSelectAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbStatesSelectAll.Click
        For Each r As DataRow In dtStates.Rows
            r("Sel") = cbStatesSelectAll.Checked
        Next
    End Sub

    Private Sub OnStateChanged(ByVal oldID As String, Optional ByVal newID As String = "")
        Dim states() As String

        For r As Integer = 0 To dtRestrictions.Rows.Count - 1
            Dim row As DataRow = dtRestrictions.Rows(r)
            states = Util.ConvierteEnArrayStr(row("States").ToString)
            For i As Integer = 0 To states.Length - 1
                If states(i) = oldID Then
                    states(i) = newID
                    If String.IsNullOrEmpty(newID) AndAlso states.Length = 1 Then
                        row.Delete()
                    Else
                        row("States") = Util.ConvertToString(states)
                    End If

                    Exit For
                End If
            Next
        Next

        dtRestrictions.AcceptChanges()
    End Sub

    Private Sub cStates_RowValidating(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellCancelEventArgs) Handles cStates.RowValidating
        Dim row As DataRow
        Try
            row = DirectCast(cStates.Rows(e.RowIndex).DataBoundItem, DataRowView).Row
        Catch ex As Exception
            Exit Sub
        End Try

        If row("ID") Is System.DBNull.Value Then
            MsgBox("Debe especificarse un ID de estado", MsgBoxStyle.Exclamation)
            e.Cancel = True
        Else

            Dim oldID, nuevoID As String
            If row.HasVersion(DataRowVersion.Current) And row.HasVersion(DataRowVersion.Proposed) Then
                oldID = row("ID", DataRowVersion.Current).ToString
                nuevoID = row("ID", DataRowVersion.Proposed).ToString

                If oldID <> nuevoID Then
                    OnStateChanged(oldID, nuevoID)
                End If
            End If

        End If
    End Sub


    Private Sub btnDeleteState_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDeleteState.Click
        Dim row As DataRow
        Dim r As DataGridViewRow
        Dim changes As Boolean = False
        If cStates.Rows.Count = 1 Then Exit Sub

        For i As Integer = cStates.Rows.Count - 1 To 0 Step -1
            r = cStates.Rows(i)
            If r.DataBoundItem Is Nothing Then Continue For
            row = DirectCast(r.DataBoundItem, DataRowView).Row
            If DirectCast(row("Sel"), Boolean) Then
                OnStateChanged(row("ID").ToString)
                row.Delete()
                changes = True
            End If
        Next
        If changes Then
            dtStates.AcceptChanges()
        End If

    End Sub

    Private Sub cStates_DataError(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles cStates.DataError
        MsgBox("Dato introducido incorrecto: " + e.Exception.Message)
    End Sub


    Private Function GetCommonStates() As List(Of State)
        Dim state As State
        Dim commonStates As New List(Of State)

        For Each e As DataRow In dtStates.Rows
            If Not DirectCast(e("Particular"), Boolean) Then
                state.ID = DirectCast(e("ID"), Integer)
                state.Name = DirectCast(e("Name"), String)
                commonStates.Add(state)
            End If
        Next
        Return commonStates
    End Function

    Private Function GetParticularStates() As List(Of State)
        Dim state As State
        Dim particularStates As New List(Of State)

        For Each e As DataRow In dtStates.Rows
            If DirectCast(e("Particular"), Boolean) Then
                state.ID = DirectCast(e("ID"), Integer)
                state.Name = DirectCast(e("Name"), String)
                particularStates.Add(state)
            End If
        Next
        Return particularStates
    End Function

#End Region

#Region "Restrictions"
    Private Sub LoadRestrictions(ByVal defSecurity As UIRestrictions, _
                                      Optional ByVal commonRoles As List(Of Rol) = Nothing, _
                                      Optional ByVal particularRoles As Rol() = Nothing)
        Dim typeRestriction As String

        Try
            SecurityEnvironment.CommonRolesAUX = commonRoles
            SecurityEnvironment.ParticularRolesAUX = particularRoles

            typeRestriction = "Authorize"
            For Each v As RestrictionOnControl In defSecurity.Authorizations
                AddRestriction(v, typeRestriction)
            Next
            typeRestriction = "Prohibite"
            For Each v As RestrictionOnControl In defSecurity.Prohibitions
                AddRestriction(v, typeRestriction)
            Next

        Finally
            SecurityEnvironment.CommonRolesAUX = Nothing
            SecurityEnvironment.ParticularRolesAUX = Nothing
        End Try

        dtRestrictions.AcceptChanges()
        If cRestrictions.Rows.Count > 0 Then
            cRestrictions.FirstDisplayedScrollingRowIndex = 0
        End If
    End Sub

    Private Sub AddRestriction(ByVal v As RestrictionOnControl, ByVal tipoPermiso As String)
        Dim r As DataRow

        If v.IDControl = "" Then Exit Sub

        Try
            r = dtRestrictions.NewRow
            r("Sel") = False
            r("Roles") = SecurityEnvironment.RolesToStrUsingAlias(v.roles, _securityComponent.ID)
            r("States") = Util.ConvertToString(v.states)
            r("Restriction") = tipoPermiso
            r("Visible") = v.Visible
            r("Enabled") = v.Enabled
            r("IDControl") = v.IDControl

            Dim tipoControl As String
            If v.IDControl(0) = "$"c Then
                tipoControl = "<GRUPO CONTROLES>"
            Else
                Dim ctrlAdapt As IControlAdapter = _parentControlAdapter.FindControl(v.IDControl)
                If ctrlAdapt.IsNull Then
                    tipoControl = "<Desconocido>"
                Else
                    tipoControl = ctrlAdapt.Control.GetType.ToString
                End If
            End If

            r("Type") = tipoControl
            dtRestrictions.Rows.Add(r)

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub

    Private Sub btnDeleteRestriction_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDeleteRestriction.Click
        For i As Integer = cRestrictions.Rows.Count - 1 To 0 Step -1
            If DirectCast(cRestrictions.Rows(i).Cells("Sel").Value, Boolean) Then
                DirectCast(cRestrictions.Rows(i).DataBoundItem, DataRowView).Row.Delete()
            End If
        Next

        dtRestrictions.AcceptChanges()
    End Sub

    Private Sub btnDuplicateRestriction_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDuplicateRestriction.Click
        If cRestrictions.CurrentRow Is Nothing Then Exit Sub

        Dim row As DataRow = dtRestrictions.NewRow
        row.ItemArray = DirectCast(cRestrictions.CurrentRow.DataBoundItem, DataRowView).Row.ItemArray
        dtRestrictions.Rows.InsertAt(row, cRestrictions.CurrentRow.Index)
        dtRestrictions.AcceptChanges()
    End Sub

    Private Sub cbRestrictionsSelectAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbRestrictionsSelectAll.Click
        For Each r As DataRow In dtRestrictions.Rows
            r("Sel") = cbRestrictionsSelectAll.Checked
        Next
    End Sub

    Private Function GetDefinedRestrictions(ByVal useAliasRoles As Boolean, Optional ByVal particularRoles As Rol() = Nothing, _
                                          Optional ByVal commonRoles As List(Of Rol) = Nothing) As String()

        Dim visible, enabled, permisoPositivo As Boolean
        Dim roles, states As String
        Dim defSecurity As New UIRestrictions(_securityComponent.ID)
        Dim vc As RestrictionOnControl
        Dim lAuthorizationsRC As New List(Of RestrictionOnControl)
        Dim lProhibitionRC As New List(Of RestrictionOnControl)
        Dim lGroups As New List(Of String)
        Dim restrictionsDefined As String()

        Try
            SecurityEnvironment.CommonRolesAUX = commonRoles
            SecurityEnvironment.ParticularRolesAUX = particularRoles

            ' 1- Guardamos la definición de los grupos, si los hay
            For Each row As DataRow In dtGroups.Rows
                lGroups.Add("$" + CType(row("Name"), String) + "= " + Util.ConvertToString(GetControlsGroup(CType(row("ID"), String))))
            Next


            ' 2- Guardamos las líneas de permisos, ya sea positivos (permitir) o negativos (impedir)
            For Each p As DataRow In dtRestrictions.Select("", "Restriction, Roles")
                visible = DirectCast(p("Visible"), Boolean)
                enabled = DirectCast(p("Enabled"), Boolean)

                permisoPositivo = True
                If DirectCast(p("Restriction"), String) <> "Authorize" Then permisoPositivo = False

                If p("Roles") Is DBNull.Value Then
                    roles = "0"
                Else
                    roles = DirectCast(p("Roles"), String)
                    If roles.Trim = "" Then roles = "0"
                End If

                states = DirectCast(p("States"), String)

                If visible Or enabled Then
                    vc = New RestrictionOnControl( _
                                        DirectCast(p("IDControl"), String), _
                                        visible, _
                                        enabled, _
                                        SecurityEnvironment.GetRolesID(roles, _securityComponent.ID), _
                                        Util.ConvertToArrayInt(states))
                    If permisoPositivo Then
                        lAuthorizationsRC.Add(vc)
                    Else
                        lProhibitionRC.Add(vc)
                    End If

                End If
            Next

            defSecurity.Authorizations = lAuthorizationsRC
            defSecurity.Prohibitions = lProhibitionRC

            Dim aux As String() = defSecurity.AuthorizationsToArrayString(useAliasRoles)

            restrictionsDefined = lGroups.ToArray
            ReDim Preserve restrictionsDefined(restrictionsDefined.Length + aux.Length - 1)
            aux.CopyTo(restrictionsDefined, lGroups.Count)

        Finally
            SecurityEnvironment.CommonRolesAUX = Nothing
            SecurityEnvironment.ParticularRolesAUX = Nothing
        End Try

        Return restrictionsDefined

    End Function


    ' Nuevos permisos

    Private Sub btnAuthorize_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAuthorize.Click
        AddNewRestriction(False)
    End Sub

    Private Sub btnProhibit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnProhibit.Click
        AddNewRestriction(True)
    End Sub

    Private Sub CheckEnabledRestrictionsButtons()
        Dim enable As Boolean = (cVisibility.Checked Or cEnabled.Checked) And cbControlsSelectAll.Checked
        btnProhibit.Enabled = enable
        btnAuthorize.Enabled = enable
    End Sub

    Private Sub cVisibility_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cVisibility.CheckedChanged
        CheckEnabledRestrictionsButtons()
    End Sub

    Private Sub cEnabled_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cEnabled.CheckedChanged
        CheckEnabledRestrictionsButtons()
    End Sub

    Private Sub EnsureVisible(ByVal lRows As List(Of DataRow))
        If lRows Is Nothing OrElse lRows.Count = 0 Then Exit Sub

        cRestrictions.ClearSelection()
        Dim n As Integer = lRows.Count - 1
        For i As Integer = n To 0 Step -1
            For Each r As DataGridViewRow In cRestrictions.Rows
                If DirectCast(r.DataBoundItem, DataRowView).Row Is lRows(i) Then
                    If i = n And Not r.Displayed Then
                        cRestrictions.CurrentCell = r.Cells(0)
                    End If
                    r.Selected = True
                    Exit For
                End If
            Next
        Next
        _selecRestrictionsMng.CheckSelectedRows(True)

    End Sub

    Private Sub AddNewRestriction(ByVal prohibit As Boolean)
        Dim states, roles As String
        Dim r As DataRow
        Dim groupName As String = ""
        Dim groupSelected As Boolean = False
        Dim control, type As String

        If Not cRestrictions.Visible Then
            If _txtRestrictionsChanged Then
                UpdateRestrictionsGrid(txtRestrictions.Lines)
            End If
        End If

        ' Al cargar cPermisos detectaremos si está seleccionado un grupo y en ese caso añadiremos no la lista de controles sino el propio grupo
        ' Un grupo estará seleccionado si el check "Mostrar todos los controles" está desmarcado y todas las filas están con el check seleccionado, esto es, no se está queriendo seleccionar un subconjunto de los elementos del grupo 

        If cbShowOnlySelectedGroup.Checked AndAlso dtGroups.Rows.Count > 0 Then
            groupSelected = True
            For Each ctrl As DataGridViewRow In cControls.Rows
                If ctrl.Visible AndAlso Not CType(ctrl.Cells("Sel").Value, Boolean) Then
                    groupSelected = False
                    Exit For
                End If
            Next
            If groupSelected Then
                r = DirectCast(cGroups.CurrentRow.DataBoundItem, DataRowView).Row
                groupName = r("Name").ToString
            End If
        End If

        Dim lRows As New List(Of DataRow)
        ' Aunque tratemos un grupo habrá al menos un control asociado. Nos apoyaremos en esta misma 
        ' estructura, aunque saldremos de la misma tras añadir el registro del control
        For Each ctrl As DataRow In dtControls.Rows
            If CType(ctrl("Sel"), Boolean) OrElse groupSelected Then
                states = convertToString(dtStates.Rows, "ID")
                roles = convertToString(dtRoles.Rows, "Alias", "ID")
                If roles = "" Then roles = "0"
                If groupSelected Then
                    control = "$" + groupName
                    type = "<GRUPO CONTROLES>"
                Else
                    control = CType(ctrl("IDControl"), String)
                    type = CType(ctrl("Type"), String)
                End If

                r = dtRestrictions.NewRow
                r("Sel") = False
                r("Roles") = roles
                r("States") = states
                r("Restriction") = IIf(prohibit, "Prohibite", "Authorize")
                r("Visible") = cVisibility.Checked
                r("Enabled") = cEnabled.Checked
                r("IDControl") = control
                r("Type") = type
                dtRestrictions.Rows.Add(r)

                lRows.Add(r)
                If groupSelected Then Exit For
            End If
        Next

        EnsureVisible(lRows)
        lRows.Clear()

        If Not cRestrictions.Visible Then
            btnRestrictionsTxt_MouseClick(Nothing, Nothing)
        End If
    End Sub

    ' Modo de presentación de los permisos
    Private Sub UpdateRestrictionsGrid(ByVal restrictions As String())
        Dim defSecurity As UIRestrictions

        Try
            SecurityEnvironment.CommonRolesAUX = GetCommonRoles()
            SecurityEnvironment.ParticularRolesAUX = GetParticularRoles.ToArray
            defSecurity = New UIRestrictions(restrictions, _securityComponent.ID, Nothing, Nothing)  ' ControlPadre a Nothing porque no queremos buscar los IAdaptdoresControles ni traducir los Grupos
        Finally
            SecurityEnvironment.CommonRolesAUX = Nothing
            SecurityEnvironment.ParticularRolesAUX = Nothing
        End Try

        dtRestrictions.Clear()

        LoadRestrictions(defSecurity, GetCommonRoles(), GetParticularRoles().ToArray)
        cbFilterOnSelectedRol_CheckStateChanged(Nothing, Nothing)
        _txtRestrictionsChanged = False
    End Sub

    Private Sub ShowRestrictionsGrid(ByVal show As Boolean)
        cRestrictions.Visible = show
        txtRestrictions.Visible = Not show

        cbFilterOnSelectedRol.Visible = show

        btnDuplicateRestriction.Visible = show
        btnDeleteRestriction.Visible = show
    End Sub

    Private Sub btnRestrictionsGrid_MouseClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles btnRestrictionsGrid.MouseClick
        If _txtRestrictionsChanged Then
            UpdateRestrictionsGrid(txtRestrictions.Lines)
        End If
        ShowRestrictionsGrid(True)
    End Sub

    Private Sub btnRestrictionsTxt_MouseClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles btnRestrictionsTxt.MouseClick
        Dim groups As Group() = Nothing
        Dim restrictions As String() = Nothing

        SecurityEnvironment.GetAuthorizationsAndGroups(GetDefinedRestrictions(True, GetParticularRoles.ToArray, GetCommonRoles), restrictions, groups)
        txtRestrictions.Lines = restrictions
        ShowRestrictionsGrid(False)
    End Sub

    Private Sub txtPermisos_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtRestrictions.TextChanged
        _txtRestrictionsChanged = True
    End Sub

    Private Sub cRestrictions_CellFormatting(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles cRestrictions.CellFormatting
        Dim row As DataGridViewRow = cRestrictions.Rows(e.RowIndex)
        If DirectCast(row.Cells("Restriction").Value, String) = "Authorize" Then
            row.DefaultCellStyle.ForeColor = Color.Green
        Else
            row.DefaultCellStyle.ForeColor = Color.Red
        End If
    End Sub


    Private Sub ShowRestrictionsOnRol(ByVal IDRol As Integer, ByVal AliasRol As String)
        Dim roles() As String
        Dim show As Boolean
        Dim firstRestrictionRol As Boolean = Nothing

        cRestrictions.CurrentCell = Nothing
        AliasRol = AliasRol.ToUpper
        For Each p As DataGridViewRow In cRestrictions.Rows
            roles = Util.ConvierteEnArrayStr(p.Cells("Roles").Value.ToString)
            For i As Integer = 0 To roles.Length - 1
                roles(i) = roles(i).ToUpper
            Next

            show = Array.IndexOf(roles, IDRol.ToString) >= 0
            If Not show Then
                show = Array.IndexOf(roles, AliasRol) >= 0
            End If
            p.Visible = show
            If show And Not firstRestrictionRol Then
                firstRestrictionRol = True
                cRestrictions.CurrentCell = p.Cells("Sel")
            End If
        Next
    End Sub

    Private Sub ShowAllRestrictions()
        For Each p As DataGridViewRow In cRestrictions.Rows
            p.Visible = True
        Next
    End Sub

    Private Sub cbFilterOnSelectedRol_CheckStateChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbFilterOnSelectedRol.CheckStateChanged
        If cbFilterOnSelectedRol.Enabled And cbFilterOnSelectedRol.Checked Then
            If cRoles.CurrentRow IsNot Nothing AndAlso Not cRoles.CurrentRow.Cells("ID").Value Is DBNull.Value Then
                Dim r As DataRow = DirectCast(cRoles.CurrentRow.DataBoundItem, DataRowView).Row
                ShowRestrictionsOnRol(DirectCast(r("ID"), Integer), r("Alias").ToString)
            End If
        Else
            ShowAllRestrictions()
        End If

    End Sub


#End Region


#Region "Aceptar / Cancelar"

    Private Sub ApplyChanges()
        Dim restrictionsDef As String()
        Dim restrictions As String() = Nothing
        Dim groups As Group() = Nothing
        Dim particularRoles As List(Of Rol) = GetParticularRoles()

        SecurityEnvironment.CommonRoles = GetCommonRoles()
        SecurityEnvironment.CommonStates = GetCommonStates()

        If _txtRestrictionsChanged Then
            UpdateRestrictionsGrid(txtRestrictions.Lines)
        End If

        restrictionsDef = GetDefinedRestrictions(False, particularRoles.ToArray)  ' En la definición de seguridad no utilizaremos alias para que no sea necesario disponer de las traducciones
        SecurityEnvironment.GetAuthorizationsAndGroups(restrictionsDef, restrictions, groups)

        Dim sec As New ComponentSecurity
        sec.Groups = groups
        sec.Restrictions = restrictions
        sec.Roles = particularRoles.ToArray
        sec.States = GetParticularStates.ToArray
        SecurityEnvironment.AddSecurityDefinition(_securityComponent.ID, sec, False)

        If cbSaveOnApplying.Checked Then
            SaveConfiguration(sec)
        End If
    End Sub

    Private Sub btnApply_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnApply.Click
        ApplyChanges()
        _componentsSecurityBak.Clear()
    End Sub


    Private Sub btnAccept_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAccept.Click
        ApplyChanges()
        _componentsSecurityBak.Clear()
        Me.Close()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        RestoreBackupSecurity()
        Me.Close()
    End Sub

#End Region

#Region "Gestión de archivo de configuración"
    Private Sub btnSelectFile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSelectFile.Click
        Dim dlResult As DialogResult
        OpenFileDialog1.FileName = ConfigFile
        dlResult = Me.OpenFileDialog1.ShowDialog()
        If dlResult = DialogResult.OK Then
            ConfigFile = OpenFileDialog1.FileName
        End If
    End Sub

    Private Sub btnLoad_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLoad.Click
        Dim idSecurityComp As String = ""
        Dim savePriority As Boolean = False
        If cbOnlyActualComponent.Checked Then
            idSecurityComp = _securityComponent.ID
        End If
        If Not _backupAvailable Then
            savePriority = True
        End If
        BackupSecurity()
        SecurityEnvironment.LoadFromWithCancelInMind(_configFile, SecurityEnvironment.OptFileConfig.None, idSecurityComp, savePriority)

        _InitialRestrictionsDefinition = SecurityEnvironment.GetRestrictionsDefinition(_securityComponent.ID)

        LoadSecurity()
        gbSecuritySource.Text = SOURCE_FILE

        If Not cRestrictions.Visible Then
            btnRestrictionsTxt_MouseClick(Nothing, Nothing)
        End If
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        SaveConfiguration()
        MsgBox("Definición de seguridad grabada en el fichero seleccionado", MsgBoxStyle.Information)
    End Sub

    Private Sub BackupSecurity()
        Dim sec, secBak As ComponentSecurity

        If _backupAvailable Then Exit Sub

        _componentsSecurityBak.Clear()
        For Each idForm As String In SecurityEnvironment.ComponentsSecurity.Keys
            sec = SecurityEnvironment.ComponentsSecurity(idForm)
            secBak = New ComponentSecurity
            secBak.States = sec.States
            secBak.Roles = sec.Roles
            secBak.Groups = sec.Groups
            secBak.Restrictions = sec.Restrictions
            _componentsSecurityBak.Add(idForm, secBak)
        Next
        _backupAvailable = True
    End Sub

    Private Sub RestoreBackupSecurity()
        Dim sec, secBak As ComponentSecurity

        If Not _backupAvailable Then Exit Sub

        For Each idForm As String In _componentsSecurityBak.Keys
            secBak = _componentsSecurityBak(idForm)
            sec = New ComponentSecurity
            sec.States = secBak.States
            sec.Roles = secBak.Roles
            sec.Groups = secBak.Groups
            sec.Restrictions = secBak.Restrictions
            SecurityEnvironment.AddSecurityDefinition(idForm, sec, True)
        Next
    End Sub

    Private Sub SaveConfiguration(Optional ByVal sec As ComponentSecurity = Nothing)
        Dim commonRoles As List(Of Rol) = GetCommonRoles()
        Dim particularRoles As List(Of Rol) = GetParticularRoles()
        Dim restrictions As String() = Nothing
        Dim groups As Group() = Nothing

        SecurityEnvironment.GetAuthorizationsAndGroups(GetDefinedRestrictions(True, particularRoles.ToArray, commonRoles), restrictions, groups)  ' Usaremos Alias para que el archivo sea más descriptivo. En el archivo lo normal será que también vengan los roles con sus ID y sus alias
        sec = New ComponentSecurity
        sec.Groups = groups
        sec.Restrictions = restrictions
        sec.Roles = particularRoles.ToArray
        sec.States = GetParticularStates.ToArray

        SecurityEnvironment.SaveConfiguration(ConfigFile, Not cbOnlyActualComponent.Checked, _securityComponent.ID, sec, commonRoles, GetCommonStates)
    End Sub

#End Region

#Region "Interacción con aplicación Host: escucha/modificación de Estado / Roles"

    Public Property HostState() As Integer
        Get
            Return _host.State(_securityComponent.ID, _securityComponent.InstanceID)
        End Get
        Set(ByVal value As Integer)
            _host.State(_securityComponent.ID, _securityComponent.InstanceID) = value
            NotifyPropertyChanged("HostState")
        End Set
    End Property

    Public Property UserRolesStr() As String
        Get
            Dim roles As Integer() = _host.UserRoles(_securityComponent.ID, _securityComponent.InstanceID)
            Try
                SecurityEnvironment.CommonRolesAUX = GetCommonRoles()
                SecurityEnvironment.ParticularRolesAUX = GetParticularRoles.ToArray
                Return SecurityEnvironment.RolesToStrUsingAlias(roles, _securityComponent.ID)
            Finally
                SecurityEnvironment.CommonRolesAUX = Nothing
                SecurityEnvironment.ParticularRolesAUX = Nothing
            End Try
        End Get
        Set(ByVal value As String)
            Try
                SecurityEnvironment.CommonRolesAUX = GetCommonRoles()
                SecurityEnvironment.ParticularRolesAUX = GetParticularRoles.ToArray
                Dim roles As Integer() = SecurityEnvironment.GetRolesID(value, _securityComponent.ID)
                _host.UserRoles(_securityComponent.ID, _securityComponent.InstanceID) = roles
                NotifyPropertyChanged("UserRolesStr")
            Finally
                SecurityEnvironment.CommonRolesAUX = Nothing
                SecurityEnvironment.ParticularRolesAUX = Nothing
            End Try
        End Set
    End Property

    Private Sub OnStateChanged(ByVal _ID As String, ByVal _instanceID As String, ByVal nuevoEstado As Integer)
        NotifyPropertyChanged("HostState")
    End Sub

    Private Sub OnRolesChanged(ByVal _ID As String, ByVal _instanceID As String)
        NotifyPropertyChanged("UserRolesStr")
    End Sub

#End Region

#Region "Varios"

    Private Sub btnResize_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnResize.Click
        Dim bmp As Bitmap
        If Me.ToolTip1.GetToolTip(btnResize) = REDUCE_WINDOW Then
            _sizeForm = Me.Size
            _posPanelSituationHost = cHostSituation.Location
            _posForm = Me.Location

            Me.Size = New Size(cHostSituation.Width + 20, cHostSituation.Height + 45)
            Me.Left = Me.Left + cHostSituation.Left
            cHostSituation.Left = 0
            gbRestrictionsDefinition.Visible = False
            bmp = My.Resources.FullScreen
            bmp.MakeTransparent()
            Me.ToolTip1.SetToolTip(btnResize, RESTORE_WINDOW)
        Else
            gbRestrictionsDefinition.Visible = True
            cHostSituation.Location = _posPanelSituationHost
            Me.Location = _posForm
            Me.Size = _sizeForm
            bmp = My.Resources.CanvasScale
            Me.ToolTip1.SetToolTip(btnResize, REDUCE_WINDOW)
        End If
        bmp.MakeTransparent()
        btnResize.Image = bmp
    End Sub

    Private Sub FrmRestrictionsUIDefinition_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        OPTSaveOnApplying = cbSaveOnApplying.Checked
        OPTOnlyActualComponent = cbOnlyActualComponent.Checked
    End Sub

    Private Function convertToString(ByVal Rows As DataRowCollection, ByVal field As String, Optional ByVal optionalField As String = Nothing) As String
        Dim cad As String = ""
        Dim sep As String = ""
        Dim value As String = ""

        For Each R As DataRow In Rows
            If CType(R("Sel"), Boolean) Then
                If R(field) IsNot DBNull.Value AndAlso R(field).ToString.Trim <> "" Then
                    value = R(field).ToString
                ElseIf Not String.IsNullOrEmpty(optionalField) Then
                    value = R(optionalField).ToString
                End If
                cad += sep + value
                sep = ", "
            End If
        Next
        Return cad
    End Function

#End Region

#Region "Implementación de INotifyPropertyChanged"
    Private Sub NotifyPropertyChanged(ByVal info As String)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(info))
    End Sub

    Public Event PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
#End Region

End Class





