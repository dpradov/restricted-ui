Imports RestrictedUI
Imports RestrictedWinFormsUI_Infragistics
Imports RestrictedWinFormsUI

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


Public Class MainForm
    Private nextInstanceForm1 As Integer = 0

    Public Shared _host As New Host

    Private Sub btnForm1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnForm1.Click
        Dim f As New Form1

        ' La seguridad a aplicar puede variar no sólo del tipo de formulario o control (donde esté incrustado el componente
        ' de seguridad, sino de la instancia concreta de ese formulario o control. Vamos a distinguir cada instancia en 
        ' el formulario Form1
        f._InstanceID = nextInstanceForm1.ToString("00")
        f.Show()
        nextInstanceForm1 += 1
    End Sub

    Private Sub btnForm2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnForm2.Click
        Dim f As New Form2
        f.Show()
    End Sub

    Private Sub FormPpal_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ' Configuración inicial de la librería de seguridad:

        ' Establecer el objeto IHost que permitirá conocer el estado y roles de la aplicación
        SecurityEnvironment.Host = _host

        ' No queremos que se actualicen los ficheros de controles automáticamente al inicializarse los
        ' componentes de seguridad. Lo actualizaremos cuando tengamos todos los controles creados (algunos
        ' los construiremos dinámicamente: columnas de un datagrid)
        SecurityEnvironment.AutomaticUpdateOfControlsFile = False

        ' Como incluimos controles Infragistics y éstos no se contemplan en la factoría interna que viene
        ' junto a la librería SeguridadWinForms.dll registraremos la factoría que sí los gestiona
        SecurityEnvironment.AddFactory(AdapterInfragisticsWinForms_Factory.getInstance)

        ' Algunos adaptadores, como éste, pueden permitir controlar el estado de habilitado o no con la propiedad ReadOnly
        ' en lugar de Enabled (esta última sería la por defecto)
        AdapterWinForms_DataGridView.UseReadOnly = True
        AdapterInfragisticsWinForms_UltraGrid.UseReadOnly = True

        ' Al margen de que tengamos definida alguna seguridad de manera embebida en algún componente, 
        ' utilizaremos la definida en un archivo. (También podríamos haber leído la seguridad desde un
        ' Stream facilitando un System.IO.StreamReader, o directamente desde una cadena con EntornoSeguridad.LoadFromString
        SecurityEnvironment.LoadFrom("TestWinForms\Security.txt")

        ' Podríamos modificar la combinación de teclas con las que activaremos el formulario de mantenimiento
        ' de la seguridad. Por ejemplo, sin quisiéramos establecer CTR-Shift-F5 utilizaríamos:
        ' EntornoSeguridad.HotKey = New HotKey(Keys.F5, Keys.Control Or Keys.Shift, True)    ' Nota: el último parámetro es opcional y por defecto a False, esto es, la combinación se crea deshabilitada 
        ' Dejaremos de momento la establecida por defecto: CTR-?  (CTR más Shift más tecla de ?)
        ' Como inicialmente la combinación está deshabilitada la habilitaremos:
        SecurityEnvironment.AllowedHotKey = True

    End Sub
End Class