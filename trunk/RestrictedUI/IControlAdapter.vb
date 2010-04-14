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
''' Indica que el objeto puede envolver a un 'control' aislando las particularidades del mismo
''' y permitiendo así su uso en el entorno de seguridad.
''' </summary>
''' <remarks>
''' <para>Envuelve un 'control' con el objetivo de ofrecer de manera homogénea el acceso a sus propiedades 
''' Enabled y Visible, así como a sus 'controles' hijo, permitiendo además ofrecer la supervisión
''' del cambio de esas propiedades, dando así opción a impedir dichos cambios en base a un esquema
''' de seguridad.</para>
''' <para>Lo que el adaptador envuelva no tiene necesariamente que ser un objeto de una clase determinada
''' puede ser cualquier elemento que el adaptador 'entienda', sobre el cual se pueda e interese controlar 
''' su visibilidad y/o si está o no habilitado, y que un adaptador 'padre' haya reconocido como hijo.</para>
''' <example>Por ejemplo sería planteable considerar como control a supervisar los elementos de un control
''' tipo ListBox. Cómo se implemente las propiedades Visible y Enabled (o sólo una de ellas) dependerá
''' de cada control y podría pasar (en este ejemplo) por eliminar (salvando previamente) o añadir uno
''' de los items. Algo así se ha realizado con el control TreeView (desde AdapterWinForms_TreeView
''' y AdapterWinForms_TreeNode) </example>  
''' 
''' <para>Las propiedades realmente controladas no tienen por qué ser exactamente 'Visible' y 'Enabled'; es
''' responsabilidad del adaptador de control ofrecer esa interface y actuar sobre las propiedades
''' que tenga el control (por ejemplo, algunos controles no ofrecen Enabled pero sí ReadOnly)</para>
''' </remarks>
Public Interface IControlAdapter
    ReadOnly Property Identification(Optional ByVal parent As IControlAdapter = Nothing, Optional ByVal security As ControlRestrictedUI = Nothing) As String

    ''' <summary>
    ''' Obtiene o establece un valor que determina la visibilidad del control asociado (envuelto por este adaptador)
    ''' </summary>
    ''' <remarks>
    ''' La propiedad realmente controlada no tienen por qué ser exactamente 'Visible'; es
    ''' responsabilidad del adaptador de control ofrecer esa interface y actuar sobre las propiedades
    ''' que tenga el control (por ejemplo, algunos controles no ofrecen Enabled pero sí ReadOnly)
    ''' </remarks>
    Property Visible() As Boolean

    ''' <summary>
    ''' Obtiene o establece un valor que habilita o desabilita el control asociado (envuelto por este adaptador)
    ''' </summary>
    ''' <remarks>
    ''' La propiedad realmente controlada no tienen por qué ser exactamente 'Enabled'; es
    ''' responsabilidad del adaptador de control ofrecer esa interface y actuar sobre las propiedades
    ''' que tenga el control (por ejemplo, algunos controles no ofrecen Enabled pero sí ReadOnly)
    ''' </remarks>
    Property Enabled() As Boolean

    ''' <summary>
    ''' Fuerza la supervisión de la propiedad Visible del control asociado (envuelto por este adaptador) por parte 
    ''' del componente facilitado como parámetro, de manera que éste pueda impedir la visibilidad del control
    ''' dependiendo de su configuración de seguridad.
    ''' </summary>
    Sub SuperviseVisible(ByVal compSecurity As ControlRestrictedUI)


    ''' <summary>
    ''' Fuerza la supervisión de la propiedad Enabled del control asociado (envuelto por este adaptador) por parte 
    ''' del componente facilitado como parámetro, de manera que éste pueda impedir que el control sea habilitado
    ''' dependiendo de su configuración de seguridad.
    ''' </summary>
    Sub SuperviseEnabled(ByVal compSecurity As ControlRestrictedUI)


    ''' <summary>
    ''' Finaliza la supervisión de las propiedades Enabled y Visible que pudiera habese iniciado, de manera que 
    ''' cualquier modificación sobre dichas propiedades en el control asociado (envuelto por este adaptador) no
    ''' puedan ser impedidas por un componente de seguridad.
    ''' </summary>
    Sub FinalizeSupervision()

    ''' <summary>
    ''' Devuelve los 'controles' hijo del control asociado, envueltos éstos a su vez en objetos IControlAdapter
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>Lo que los adaptadores envuelvan no tienen necesariamente que ser objetos Control, pueden ser cualquier
    ''' elemento que para este adaptador sea susceptible de ser considerado como un hijo de este control, como un detalle
    ''' o componente de este control que pueda interesar (y sea posible) controlar desde el punto de vista de su visibilidad
    ''' y/o habilitación</remarks>
    Function Controls() As IList(Of IControlAdapter)

    ''' <summary>
    ''' Delvuelve el 'control' (envuelto en un objeto IControlAdapter) especificado a través de una identificación
    ''' completa (<paramref name="id"/>), accesible por el 'control' asociado a este adaptador.
    ''' </summary>
    ''' <param name="id">Identificación del 'control' a buscar. Deberá incluir la identificación de todos los controles
    ''' padre del control a buscar (visibles desde el control en el que se ejecuta este método).
    ''' Por ejemplo, si el adaptador sobre el que se ejecuta FindControl corresponde a un formulario con un control 
    ''' groupbox denominado "GroupBox1" que tiene a su vez una etiqueta denominada "Label1", para localizar la etiqueta la
    ''' identificación deberá ser: "GroupBox1.Label1". Si la búsqueda se realiza sobre el control groupbox la identificación
    ''' de esa etiqueta será simplemente "Label1"
    ''' </param>
    ''' <returns></returns>
    ''' <remarks>
    ''' <para>El parámetro <paramref name="id"/> deberá incluir la identificación de todos los controles
    ''' padre del control a buscar (visibles desde el control en el que se ejecuta este método).
    ''' Por ejemplo, si el adaptador sobre el que se ejecuta FindControl corresponde a un formulario con un control 
    ''' groupbox denominado <c>GroupBox1</c> que tiene a su vez una etiqueta denominada <c>Label1</c>, para localizar la etiqueta la
    ''' identificación deberá ser: <c>GroupBox1.Label1</c>. Si la búsqueda se realiza sobre el control groupbox la identificación
    ''' de esa etiqueta será simplemente <c>Label1</c></para>
    ''' 
    ''' <para>La búsqueda del control no debería tener en cuenta mayúsculas y minúsculas</para>
    ''' </remarks>
    Function FindControl(ByVal id As String) As IControlAdapter

    ''' <summary>
    ''' Indica si el adaptador es el caso especial 'nulo', esto es, que no envuelve ningún control
    ''' </summary>
    ''' <remarks>(Según el patrón 'Special Case' descrito por Martin Fowler: http://martinfowler.com/eaaCatalog/specialCase.html)</remarks>
    ReadOnly Property IsNull() As Boolean

    ''' <summary>
    ''' Obtiene el 'control' envuelto por este adaptador
    ''' </summary>
    ReadOnly Property Control() As Object
End Interface
