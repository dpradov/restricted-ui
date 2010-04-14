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

    ' La seguridad a aplicar puede depender no sólo del tipo de formulario o control (donde esté incrustado el componente
    ' de seguridad, sino de la instancia concreta de ese formulario o control. Vamos a distinguir cada instancia en 
    ' el formulario Form1. Por defecto serán "00"
    Public _InstanceID As String = "00"

    ' En la clase Host (IHost) se depende del ID del componente de seguridad y de la instancia concreta de éste. Con esta 
    ' clase auxiliar podemos interactuar a modo de prueba desde UI. Permite enlazar controles de usuario
    ' para el estado y los roles de la aplicación, asumiendo que éstos se refieren al ID del componente
    ' de seguridad y a la instancia concreta que los crea (lo que se establece en la creación de este objeto Host_Aux)
    Private _hostAux As Host_Aux

    ' Campos internos a Form1
    Private dtControls As DataTable
    Private ds As New DataSet
    Private aa As New Entity

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        ' Todos los controles del componente de seguridad los hemos inicializado en tiempo de diseño y sus
        ' propiedades las cargará por tanto el diseñador (ver Form1.Designer.vb)
        ControlRestrictedUIWinForms1.InstanceID = _InstanceID
        lblInstance.Text = _InstanceID

        ' Para poder enlazar en este formulario a modo de ejemplo controles que muestren y modifiquen el estado
        ' y los roles correspondientes a este ID e instancia.
        _hostAux = New Host_Aux(MainForm._host, ControlRestrictedUIWinForms1.ID, _InstanceID)


        CreateDataTableControls()

        'aa.a = "10"
        'aa.b = 10
        'Me.EntidadBindingSource.DataSource = aa
        Me.EntidadBindingSource.DataSource = ds
        Me.EntidadBindingSource.DataMember = "Controls"


#If DEBUG Then
        ' Para mantener actualizado el fichero de controles y así tener la posibilidad de considerarlos
        ' al editar la seguridad en tiempo de diseño
        ControlRestrictedUIWinForms1.RegisterControls()
#End If

        ' Hemos creado dinámicamente controles (columnas de un DataGridView) que queremos controlar
        ' con la librería de seguridad. Forzamos por ello a que se reconsidere la seguridad (antes de llamar
        ' a este método en base a este evento se habrá inicializado la seguridad, pero algunos de los controles 
        ' no existían)
        ControlRestrictedUIWinForms1.ReinitializeSecurity()

        ' Permitimos mostrar y modificar las propiedades AuthorizationsDefinition (del componente de seguridad)
        ' y el estado y roles de la aplicación (para esta pantalla e instancia) enlazándonos con los objetos
        ' componente y Host_Aux. El enlace bidireccional se consigue porque ambos objetos implementan la
        ' interface INotifyPropertyChanged, por lo que cambios a esas propiedades en los objetos se comunican
        ' a los controles en los que se enlazan.
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
        r("Name") = "Nombre1"
        r("Type") = "Tipo1"
        r("Groups") = ""
        dtControls.Rows.Add(r)
        r = dtControls.NewRow
        r("IDControl") = "ID2"
        r("Name") = "Nombre2"
        r("Type") = "Tipo2"
        r("Groups") = ""
        dtControls.Rows.Add(r)

        cControles.DataSource = ds
        cControles.DataMember = "Controls"
    End Sub

    ''' <summary>
    ''' Método para abrir el formulario de mantenimiento de la seguridad en tiempo de ejecución
    ''' </summary>
    Private Sub btnRestrictionsMngt_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRestrictionsMngt.Click
        ControlRestrictedUIWinForms1.ShowConfigurationSecurityForm(MainForm._host)

        ' También podría forzarse la definición que se ofrezca como punto de partida:
        'ControlRestrictedUIWinForms1.ShowConfigurationSecurityForm(MainForm._host, txtRestrictions.Lines)
    End Sub

#Region "Métodos para forzar las propiedades Visible y Enabled y así verificar el funcionamiento de la librería de Seguridad"

    Private Sub btnEnableVisible_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEnableVisible.Click
        Me.combo.Visible = True
        Me.TextBox.Visible = True
        'Me.GroupBox1.Visible = True
        Me.TextBox2.Visible = True
        Me.CheckBox1.Visible = True

        'TreeView1.Visible = True

        Dim adapt As IControlAdapter
        adapt = New AdapterWinForms_TreeView(TreeView1)
        For Each c As IControlAdapter In adapt.Controls
            c.Visible = True
        Next
        adapt.FindControl("Nodo0.Nodo1").Visible = True
        adapt.FindControl("Nodo0.Nodo2").Visible = True
        adapt.FindControl("Nodo3.Nodo4").Visible = True
        adapt.FindControl("Nodo3.Nodo4.Nodo5").Visible = True

        'cControles.Visible = True
        adapt = New AdapterWinForms_DataGridView(cControles)
        For Each c As IControlAdapter In adapt.Controls
            c.Visible = True
        Next

    End Sub

    Private Sub btnEnableEnabled_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEnableEnabled.Click
        'Me.GroupBox1.Enabled = True
        Me.combo.Enabled = True
        Me.TextBox.Enabled = True
        Me.TextBox2.Enabled = True
        Me.CheckBox1.Enabled = True

        'cControles.Enabled = True
        Dim adapt As IControlAdapter
        adapt = New AdapterWinForms_DataGridView(cControles)
        For Each c As IControlAdapter In adapt.Controls
            c.Enabled = True
        Next

    End Sub

    Private Sub btnDisableVisible_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisableVisible.Click
        'Me.GroupBox1.Visible = False
        Me.combo.Visible = False
        Me.TextBox.Visible = False
        Me.TextBox2.Visible = False
        Me.CheckBox1.Visible = False

        'TreeView1.Visible = False

        Dim adapt As IControlAdapter
        adapt = New AdapterWinForms_TreeView(TreeView1)
        For Each c As IControlAdapter In adapt.Controls
            c.Visible = False
        Next
        adapt.FindControl("Nodo0.Nodo1").Visible = False
        adapt.FindControl("Nodo0.Nodo2").Visible = False
        adapt.FindControl("Nodo3.Nodo4").Visible = False
        adapt.FindControl("Nodo3.Nodo4.Nodo5").Visible = False

        'cControles.Visible = False
        adapt = New AdapterWinForms_DataGridView(cControles)
        For Each c As IControlAdapter In adapt.Controls
            c.Visible = False
        Next

    End Sub

    Private Sub btnDisableEnabled_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisableEnabled.Click
        Me.combo.Enabled = False
        Me.TextBox.Enabled = False
        'Me.GroupBox1.Enabled = False
        Me.TextBox2.Enabled = False
        Me.CheckBox1.Enabled = False

        'cControles.Enabled = False
        Dim adapt As IControlAdapter
        adapt = New AdapterWinForms_DataGridView(cControles)
        For Each c As IControlAdapter In adapt.Controls
            c.Enabled = False
        Next

    End Sub

    Private Sub btnEnableVisible_N_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEnableVisible_N.Click
        For i As Integer = 0 To 10
            btnEnableVisible_Click(Nothing, Nothing)
        Next
    End Sub

#End Region

#Region "Prueba de acciones a realizar sobre el componente de seguridad"

    ' Prueba de la propiedad Pause del componente de seguridad. De la descripción de la misma:
    ' Deshabilitar temporalmente (pausar) la seguridad del ControlSeguridad, de manera que los posteriores cambios en las propiedades
    ' supervisadas (Visible y Enabled) sean permitidos.
    ' Al restablecer False (valor inicial) la definición de la seguridad se restablecerá: algunos controles se deshabilitarán u ocultarán
    ' en consecuencia
    Private Sub cbPaused_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbPaused.Click
        ControlRestrictedUIWinForms1.Paused = cbPaused.Checked
    End Sub

    ''' <summary>
    '''  Verificación de la funcionalidad ForzarVisibilidad sobre el control TextBox
    ''' </summary>
    Private Sub btnForceVisibility_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnForceVisibility.Click
        ControlRestrictedUIWinForms1.ForceVisibility(TextBox)
    End Sub

    ''' <summary>
    '''  Verificación de la funcionalidad ForzarEnabled sobre el control TextBox
    ''' </summary>
    Private Sub btnForceEnabled_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnForceEnabled.Click
        ControlRestrictedUIWinForms1.ForceEnabled(TextBox)
    End Sub

    ''' <summary>
    '''  Verificación de la funcionalidad ExcluirControl sobre el control TextBox
    ''' </summary>
    Private Sub btnExcludeControl_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExcludeControl.Click
        ControlRestrictedUIWinForms1.ExcludeControl(TextBox)
    End Sub

    ''' <summary>
    ''' En ocasiones puede interesar forzar que se reconsidere la seguridad, por ejemplo si hemos añadido controles
    ''' dinámicamente
    ''' </summary>
    Private Sub btnReinitialize_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnReinitialize.Click
        ControlRestrictedUIWinForms1.ReinitializeSecurity()
    End Sub


    ''' <summary>
    ''' Activación/desactivación de la lógica adicional de restricción, como ejemplo de uso del evento disponible BeforeApplyingRestriction
    ''' </summary>
    Private Sub cbAditionalLogicOfRestriction_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbAditionalLogicOfRestriction.CheckedChanged
        If cbAditionalLogicOfRestriction.Checked Then
            AddHandler ControlRestrictedUIWinForms1.BeforeApplyingRestriction, AddressOf OnBeforeApplyingRestriction
        Else
            RemoveHandler ControlRestrictedUIWinForms1.BeforeApplyingRestriction, AddressOf OnBeforeApplyingRestriction
        End If
    End Sub

    ''' <summary>
    ''' Ejemplo de uso del evento AntesDeAplicarSeguridad, como opción para permitir o no el cambio en base a una lógica más compleja:
    ''' Sólo permitimos el cambio sobre CheckBox1 si, además de cumplir las restricciones de seguridad impuestas, el contenido de los
    ''' controles TextBox y TextoBox2 es el mismo
    ''' </summary>
    Private Sub OnBeforeApplyingRestriction(ByVal adaptControl As IControlAdapter, ByVal tipo As RestrictedUI.ControlRestrictedUI.TChange, ByRef permitirCambio As Boolean)
        If permitirCambio And adaptControl.Control Is CheckBox1 Then
            If TextBox.Text <> TextBox2.Text Then
                permitirCambio = False
            End If
        End If
    End Sub

    Private Sub cbUseReadOnly_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbUseReadOnly.CheckedChanged

        AdapterWinForms_DataGridView.UseReadOnly = cbUseReadOnly.Checked

        ControlRestrictedUIWinForms1.Paused = True
        If cbUseReadOnly.Checked Then
            cControles.Enabled = True
        Else
            cControles.ReadOnly = True
        End If
        ControlRestrictedUIWinForms1.ReinitializeSecurity()
        ControlRestrictedUIWinForms1.Paused = False
    End Sub

#End Region

#Region "Pruebas sobre los adaptadores UltraGrid, UltraTree, TreeView y Control"

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

        MsgBox("Buscando NodoINEXISTENTE")
        a2 = a.FindControl("NodoINEXISTENTE")
        a2 = a.FindControl("Nodo0")
        a2 = a2.FindControl("Nodo2")
        a2.SuperviseVisible(Me.ControlRestrictedUIWinForms1)
        a2.Visible = False

        ' Comprobamos que realmente se oculta y muestra el control
        MsgBox(a2.Identification)
        a2.Visible = True
        MsgBox(a2.Identification)

        ' Vamos a recorrer los controles de Nodo0, teniendo en cuenta que uno de ellos (Nodo2)
        ' estará oculto, por lo que habrá sido eliminado del TreeView..
        a2.Visible = False
        a2 = a.FindControl("Nodo0")
        For Each elem As IControlAdapter In a2.Controls
            Debug.Print(elem.Identification)
        Next

        a2 = a.FindControl("Nodo0.Nodo2")
        Debug.Print(a2.Identification)
        a2.SuperviseVisible(Me.ControlRestrictedUIWinForms1)
        a2.Visible = True

        ' Buscamos un nodo que cuelga de otro oculto
        MsgBox("Buscamos Nodo3.Nodo4")
        a2 = a.FindControl("Nodo3.Nodo4")
        a2.SuperviseVisible(Me.ControlRestrictedUIWinForms1)
        a2.Visible = False
        MsgBox(a2.Identification)
        MsgBox("Buscamos Nodo3.Nodo4.Nodo5")
        a3 = a.FindControl("Nodo3.Nodo4.Nodo5")
        Debug.Print(a3.Visible.ToString)
        Debug.Print(a3.Identification)
        a3.SuperviseVisible(Me.ControlRestrictedUIWinForms1)
        a3.Visible = False
        MsgBox(a3.Identification)
        a2.Visible = True
        MsgBox(a3.Identification)
        a3.Visible = True
        MsgBox("FIN")
    End Sub

#End Region

End Class
