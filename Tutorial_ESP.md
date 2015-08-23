
<br />

---

# 1. Introducción #
Una parte delicada de la implementación de la seguridad en aplicaciones, especialmente corporativas, consiste en determinar qué funcionalidad debe estar o no accesible al usuario en función de su rol, o qué elementos deben o no mostrarse.

Lo habitual es incluir en el código la lógica necesaria para ocultar o deshabilitar determinadas opciones, botones, etc, dependiendo del tipo de usuario que accede a la aplicación. Esto puede complicarse cuando aparte del tipo de usuario también debe tenerse en cuenta el estado en que se encuentra la aplicación, por ejemplo el estado de tramitación de una entidad.

Es bastante usual que esta política deba cambiar con cierta frecuencia, de la misma forma que deben evolucionar los requerimientos de las aplicaciones. Modificaciones en la estructura organizativa de la empresa, cambios en los protocolos de gestión de las unidades implicadas o simplemente la identificación de carencias tras el uso de la aplicación, entre otras causas, llevan a tener que hacer adaptaciones a esta lógica ligada a la interface.

En respuesta a esta problemática cabe también la opción de centralizar la definición de esta política, en un repositorio común a múltiples aplicaciones. En base a esta definición, soportada por un modelo de datos determinado, es posible definir una librería que utilicen las aplicaciones, y que haga posible modificar la política de seguridad sin necesidad de recompilar y redistribuir las aplicaciones. La idea principal es la siguiente: el programador desarrolla como si sólo hubiera un tipo de usuario, el administrador, con permisos para hacer cualquier cosa. Es la librería la que controla si debe impedirse un intento de hacer visible o de habilitar determinados controles.

Este enfoque es el que se usa actualmente en la empresa en la que trabajo. Aun ofreciendo gran flexibilidad en su concepción, la implementación concreta de la misma presenta desde mi punto de vista una serie de limitaciones y dificultades concretas que he intentado corregir con esta otra librería/arquitectura:

<br />
## Características principales ##
Por un lado la librería / arquitectura que aquí se explica no obliga a centralizar la definición de esta seguridad, pero sí la contempla como un caso más; por otro lado para asegurar el cumplimiento de estas restricciones no es necesario obligar al programador a utilizar métodos ni propiedades específicas de la librería, que verifiquen si el cambio se permite o no, ni tiene que ser controlado por código a añadir; simplemente cualquier intento de hacer visible o habilitar cualquier elemento de interface supervisado que no sea admitido por la política de seguridad es interceptado e impedido. Por defecto sólo se supervisa el intento de hacer visible o habilitar, pero es posible forzar también la supervisión de la 'desactivación' de esas propiedades, esto es, del intento de hacer invisible o desabilitar esos controles.

Se permite definir la política de restricciones no sólo en base a roles sino a estados de la aplicación, tanto en aplicaciones WinForms como Web; y para simplificar la definición de esta política se ofrece un formulario de mantenimiento, disponible tanto en tiempo de diseño como de ejecución.

Notas:
  * No es objetivo de esta librería controlar la autenticación del usuario. Se asume que ésta es realizada correctamente y que se conoce, con garantías, el rol o roles que ostenta el usuario.
  * El nombre de la librería, RestrictedUI, hace referencia al hecho de que ciertos elementos de la interface estarán restringidos según la seguridad establecida ('área restringida')

<br /><br />


---

# 2. Usando el código #

Paso a describir a continuación con más detalle esta librería:

## 2.1. Entidades principales ##
<br />
![http://restricted-ui.googlecode.com/svn/wiki/images/modelo1.gif](http://restricted-ui.googlecode.com/svn/wiki/images/modelo1.gif)

<br /><br />

![http://restricted-ui.googlecode.com/svn/wiki/images/modelo2.gif](http://restricted-ui.googlecode.com/svn/wiki/images/modelo2.gif)

<br />
Las clases principales y con las que se interactuará serán fundamentalmente: **`ControlRestrictedUI`** (a través de alguna de los componentes concretos que se ofrecen, bien para WinForms bien para Web), **`SecurityEnvironment`** e **`IHost`**. La primera contiene la mayor parte de la lógica de negocio, y es la responsable de asegurar la seguridad UI en el control en el que se encuentra, apoyándose en otras clases. La segunda, `SecurityEnvironment`, es un objeto Singleton que permite configurar aspectos comunes a todos los componentes y sobre todo hace posible el establecimiento masivo de la seguridad, para todos o parte de los componentes (necesario para la carga de las restricciones desde un repositorio centralizado). La tercera, `IHost`, es la interface que debe ofrecer la aplicación que haga uso de esta librería y que permitirá saber el estado de la aplicación y el rol o roles del usuario.

La política de restricciones que se aplican en un componente `ControlRestrictedUI` se almacenan bajo la forma de un objeto `UIRestrictions` y éste a su vez contiene una lista de restricciones (permisos y prohibiciones), que no son más que estructuras `RestrictionOnControl`. Más abajo se describen con un poco más de detalle estas dos entidades y con ellas cómo se define la política de seguridad.

En el caso de que se necesite trabajar con controles que no estén ya contemplados o se necesite gestionar alguno de ellos de una manera diferente, se habrán de crear los adaptadores correspondientes (`IControlAdapter`) y también la factoría que los genere (`IControlAdapterFactory`). Lo único que habrá que hacer en estos casos es preparar un nueva librería que incluya los nuevos adaptadores necesarios así como la factoría correspondiente. Este factoría se añadiría al objeto `SecurityEnvironment` como se explica en el ejemplo que se describe más adelante. Para crear estos adaptadores y factorías puede basarse en los ya disponibles.


Las clases y métodos más destacables son:

<br />
### `ControlRestrictedUI` ###
Componente base de la librería de Interface Restringida (!RestrictedUI): permite restringir la visibilidad y estado de habilitación de los controles incluidos en un formulario o control de usuario en base a una definición de seguridad establecida, en la que intervienen el estado actual de la aplicación en dicho formulario o contenedor así como el rol o roles que tenga el usuario de la aplicación.

  * **`ChangeAllowed`** Es el método que evalúa la política de restricciones y decide si permite o no un determinado cambio sobre un control. Comprueba si el cambio sobre la propiedad Visible o Enabled (o la correspondiente del control) es válido atendiendo a la definición de seguridad, a los roles del usuario y al estado actual de la aplicación.

  * **`VerifyChange`** - Comprueba si el cambio sobre la propiedad indicada (`TChange`) es válido atendiendo a la definición de seguridad, a los roles del usuario y al estado actual de la aplicación. Si el cambio no es válido se deshará, esto es, se establecerá nuevamente a False
> Los adaptadores de control llaman a este método del componente de seguridad en respuesta al intento de modificación de las propiedades supervisadas.

  * **`ID, InstanceID`** - Identificador del componente y de la instancia concreta. A  partir del `ID` podrá leerse/actualizarse la seguridad desde un archivo, para establecerla a nivel de entorno. Con estos dos atributos el objeto `IHost` podrá indicar el estado y roles que aplican, distinguiendo no sólo por pantalla sino por instancia concreta.

  * **`RestrictionsDefinition`** - Configuración de los bloqueos y permisos que se establecerán.

  * **`ConfigFile`** - Obtiene o establece la ruta hacia un fichero de configuración, a utilizar fundamentalmente en tiempo de diseño.
> El fichero de configuración hace posible ofrecer en tiempo de diseño y durante la definición de la seguridad en el formulario `FrmRestrictionsUIDefinition` la relación de roles y estados a utilizar, así como de factorías de adaptadores adicionales, para así 'descubrir' nuevos controles en tiempo de diseño.
> Las propias definiciones de restricciones de seguridad, de todos o sólo algunos componentes pueden estar contenidas en este archivo. Estas restricciones pueden cargarse en tiempo de ejecución mediante el método `LoadFrom()` así como cargarse y grabarse a voluntad desde el formulario `FrmRestrictionsUIDefinition`, en tiempo de diseño o de ejecución.

  * **`ControlsFile`** - Nombre del archivo sobre el que podrá escribirse la relación los controles contenidos en el formulario o control de usuario controlado por este componente.
> Este archivo hace posible ofrecer en tiempo de diseño desde el formulario `FrmRestrictionsUIDefinition` controles que se crearán dinámicamente. Para aplicaciones Web debe utilizarse necesariamente para poder configurar la seguridad en tiempo de diseño con la ayuda de ese formulario.

  * **`RegisterControls`** - Fuerza el registro o actualización de los controles existentes en el formulario o contenedor en donde está incrustado este componente `ControlRestrictedUI`.

  * **`BeforeApplyingRestriction`** - Ocurre justo antes de llegar a permitir o impedir el cambio de la propiedad `Visible` o `Enabled`, para dar la opción de permitir o no el cambio en base a una lógica más compleja

  * **`SuperviseDeactivation`** - Establece si la restricciones se deberán aplicar también en la 'desactivación' de los controles, supervisando el intento de ocultarlos o desabilitarlos, y no solo en la 'activación'.


<br />
### `ControlRestrictedUIWeb` ###
Adaptación, para aplicaciones Web, del componente `ControlRestrictedUI`.


<br />
### `ControlRestrictedUIWinForms` ###
Adaptación, para aplicaciones WinForms, del componente `ControlRestrictedUI`.


<br />
### `IControlAdapter` ###
Envuelve un 'control' con el objetivo de ofrecer de manera homogénea el acceso a sus propiedades `Enabled` y `Visible`, así como a sus 'controles' hijo, permitiendo además ofrecer la supervisión del cambio de esas propiedades, dando así opción a impedir dichos cambios en base a un esquema de seguridad.

Lo que el adaptador envuelva no tiene necesariamente que ser un objeto de una clase determinada puede ser cualquier elemento que el adaptador 'entienda', sobre el cual se pueda e interese controlar su visibilidad y/o si está o no habilitado, y que un adaptador 'padre' haya reconocido como hijo.
Las propiedades realmente controladas no tienen por qué ser exactamente `Visible` y `Enabled`; es responsabilidad del adaptador de control ofrecer esa interface y actuar sobre las propiedades que tenga el control (por ejemplo, algunos controles no ofrecen `Enabled` pero sí `ReadOnly`)

Los controles que ofrece el formulario de mantenimiento de la seguridad (`FrmRestrictionsUIDefinition`) así como los controles donde busca el componente `ControlRestrictedUI` son todos aquellos ofrecidos como 'hijos' por el control en donde esté incrustado el componente. El adaptador de este control padre irá preguntando a los adaptadores de sus controles hijo, lo que irá haciendo descubrir nuevos controles. Así, si se cuenta con un adaptador que entienda de controles DataGridView, por ejemplo, es posible ofrecer cada una de las columnas como un hijo a supervisar. De esta manera, el número de controles a supervisar dependerá de los adaptadores con los que se trabaje, y esto dependerá a su vez de las factorías que se hayan podido incorporar:


<br />
### `IControlAdapterFactory` ###
Define una factoría externa de adaptadores de control. Éstas serán consultadas antes que la factoría base (interna) a la hora de localizar el adaptador a utilizar.
Sólo define el siguiente método GetAdapter

Devuelve el adaptador más idóneo correspondiente al control que se le pasa, según la factoría. Si la factoría no tiene ningún adaptador adecuado para ese control devolverá el adaptador especial `NullControlAdapter`


<br />
### `UIRestrictions` ###
Contiene todas las restricciones de interface (UI) (permisos y prohibiciones) que definen la seguridad para un componente `ControlRestrictedUI`, y que afectan normalmente por tanto a un formulario o control de usuario.

Todas las restricciones se aplican a nivel de controles individuales, teniendo presente que las prohibiciones tendrán prioridad sobre los permisos, esto es, primero se aplicarán los permisos y luego se restringirán en base a las prohibiciones:

  * Prohibiciones: sólo se impedirán las modificaciones de las propiedades `Visible` / `Enabled` en las situaciones aquí señaladas.

  * Permisos: sólo se autorizarán las modificaciones de las propiedades `Visible` / `Enabled` en las situaciones aquí señaladas

Esta clase guarda restricciones individuales (ver `RestrictionOnControl`) y detemina si se deben considerar en lógica positiva (permisos) o negativa (prohibiciones). Es la encargada de serializar y deserializar estos permisos.


<br />
### `RestrictionOnControl` ###
Define los elementos que configuran una restricción concreta a supervisar. Esta restricción sólo tendrá pleno sentido interpretada conjuntamente con el resto de restricciones incluidas en una definición de seguridad definida en un objeto `UIRestrictions` y gestionada por un componente `ControlRestrictedUI`.


Los elementos que configuran una restricción individual son:
  * Control a supervisar (a través de un adaptador)
  * Propiedades a controlar (`Visible` y/o `Enabled`)
  * Contexto de la aplicación para el que se define la restricción:
    * Rol o roles del usuario de la aplicación
    * Estado o estados de la aplicación

Estos elementos podrán ser aplicados en lógica positiva (permisos) o negativa (prohibiciones). Esta interpretación no la ofrece esta estructura sino la clase `UIRestrictions` dependiendo de que esta restricción haya sido incluida en una línea de permisos o prohibiciones.

  * Si la restricción consiste en un permiso indicará que las propiedades supervisadas del control sólo las podrán 'activar' (hacer visible o habilitar) los roles indicados y únicamente cuando la aplicación esté en los estados señalados.
  * Si la restricción consiste en una prohibición indicará que las propiedades supervisadas del control no podrán ser 'activadas' por los roles indicados cuando la aplicación esté en los estados señalados. Para cualquier combinación de roles/estado distinta sí será posible la activación.
  * Si no se aporta ningún rol (por defecto se asume rol = 0) entonces aplicará a todos los roles: a todos se les permitirá o impedirá (según) en los estados señalados
  * Si no se aporta ningún estado entonces la restricción aplicará a los roles que correspondan con independencia del estado en que esté la aplicación.
  * Si un control no tiene ningún elemento de restricción asociado (ni positivo ni negativo) entonces no será supervisado, y cualquier rol y en cualquier estado podrá activar sus propiedades `Visible` y `Enabled`.

Las propiedades realmente controladas en el control no tienen por qué ser exactamente '`Visible`' y '`Enabled`' (como se indica en `IControlAdapter`)
Por defecto sólo se supervisa y tal vez se impida (según la política definida) la 'activación' de esas propiedades, esto es, el intento de hacer visible o habilitar el control. No se impide el ocultar o deshabilitar un control.
Mediante la propiedad `SuperviseDeactivation` (del componente `ControlRestrictedUI`) es posible modificar ese comportamiento general y monitorizar también la 'desactivación' de los controles, pudiendo por tanto impedirla atendiendo a la definición de la política de seguridad.


<br />
### `IHost` ###
Indica que el objeto puede actuar como intermediario en la comunicación necesaria entre la aplicación Host y la librería de Interface Restringida permitiendo a ésta consultar en cualquier momento el estado de la aplicación y el rol o roles que pueda tener el usuario de la misma. Informa también mediante eventos de cambios en estos valores.

Estos datos (estado y roles) podrán ser dependientes del formulario o control de usuario desde el que se pregunte (tipo e instancia concreta).

Este enfoque permite, por ejemplo, mantener abiertas diferentes instancias de una determinada ventana, cada una de ellas en un estado o fase de tramitación diferente y por tanto con requerimientos de seguridad distintos.

Aunque es más común que el rol o roles del usuario sean uniformes por toda la aplicación se permite que puedan también depender de donde se ubique el componente de seguridad y de la instancia concreta. De esta manera se da más margen de maniobra a la hora de configurar la seguridad.


<br />
### `SecurityEnvironment` ###
Clase Singleton (vía Shared) que mantiene aspectos generales a todo el esquema de seguridad.

  * **`CommonStates, CommonRoles`** - Permiten establecer los estados y roles comunes, que se ofrecen en todos los componentes de seguridad

  * **`ComponentsSecurity`** - Devuelve el objeto diccionario en el que se guarda la definición de seguridad (política de restricciones de interface) de los diferentes componentes, según pueda haberse establecido a partir de la carga de un fichero de configuración o de una cadena de texto.
> La seguridad aquí contenida no tiene por qué ser igual a la embebida en los distintos componentes. La que aplique finalmente en el componente depende de una propiedad del mismo (`ProrityEmbeddedSecurity`) Métodos como `LoadFrom` o `LoadFromString`, por ejemplo, forzarán la prioridad a la seguridad del entorno sobre la embebida, para un componente, cuando en el fichero o cadena (p.ej) aparezca referenciado ese componente, aunque sea con un definición vacía.

  * **`BaseFactory, AditionalFactories`** - Obtiene o establece la factoría de adaptadores que se considera base, así como otras adicionales. A la hora de buscar el mejor adaptador para un control se comenzará buscando en las adicionales y si no se encuentra ninguno, finalmente en factoria base.

  * **`Host`** - Recupera o establece el objeto que implementa la interface `IHost` y a través de la cual el entorno y el resto de componentes de seguridad puede interaccionar con la aplicación Host a efectos de conocer el estado y roles actuales que deben considerarse

  * **`AddFactoria`** - Agrega la factoría de adaptadores de control indicada. Será consultada antes que la factoría interna

  * **`GetAdapter`** - Devuelve el adaptador (`IControlAdapter`) correspondiente al control indicado, en base a las factorías registradas en el Entorno.

  * **`LoadFrom, LoadFromString`** - Permiten cargar la política de seguridad, a nivel de entorno, a partir de un fichero, un stream o una cadena de texto.


<br /><br />

---

## 2.2. Ejemplo de uso ##

Es necesario hacer referencia a la DLL RestrictedUI.dll así como RestrictedWinFormsUI.dll o RestrictedWebUI.dll, según la aplicación. Si incluimos en la aplicación algún tipo de control especial que queramos tratar de manera personalizada entonces añadiremos también una referencia a la DLL correspondiente, que tendrá implementada una factoría de adaptadores para ese control especial. En el siguiente ejemplo se hace uso de una librería que interpreta los controles UltraGrid y UltraTree de Infragistics ([NetAdventage](http://www.infragistics.com/dotnet/netadvantage.aspx)).

Al arranque de la aplicación hacemos la siguiente inicialización, donde la única línea realmente importante es la primera:
<br />

```
' Configuración inicial de la librería de seguridad:'
'-----------------------------------------'

' Establecer el objeto IHost que permitirá conocer el estado y roles de la aplicación'
 SecurityEnvironment.Host = _host

' No queremos que se actualicen los ficheros de controles automáticamente al inicializarse los'
' componentes de seguridad. Lo actualizaremos cuando tengamos todos los controles creados (algunos'
' los construiremos dinámicamente: columnas de un datagrid)'
 SecurityEnvironment.AutomaticUpdateOfControlsFile = False

' Como incluimos controles Infragistics y éstos no se contemplan en la factoría interna que viene'
' junto a la librería SeguridadWinForms.dll registraremos la factoría que sí los gestiona'
 SecurityEnvironment.AddFactory(AdapterInfragisticsWinForms_Factory.getInstance)

' Algunos adaptadores, como éste, pueden permitir controlar el estado de habilitado o no con la propiedad ReadOnly'
' en lugar de Enabled (esta última sería la por defecto)'
 AdapterWinForms_DataGridView.UseReadOnly = True
 AdapterInfragisticsWinForms_UltraGrid.UseReadOnly = True

' Al margen de que tengamos definida alguna seguridad de manera embebida en algún componente, '
' utilizaremos la definida en un archivo. (También podríamos haber leído la seguridad desde un'
' Stream facilitando un System.IO.StreamReader, o directamente desde una cadena con EntornoSeguridad.LoadFromString'
 SecurityEnvironment.LoadFrom("TestWinForms\Security.txt")
```

> (Más adelante se muestra un ejemplo de fichero de configuración que puede ser cargado mediante `LoadFrom`)

<br />
El objeto _host sería de la clase Host, que implementaría la interface `IHost`, por ejemplo de esta forma (TestForm):_

```
Public Class Host
    Implements IHost

    Public Event StateChanged(ByVal ID As String, ByVal instanceID As String, ByVal newState As Integer) Implements RestrictedUI.IHost.StateChanged
    Public Event RolesChanged(ByVal ID As String, ByVal instanceID As String) Implements RestrictedUI.IHost.RolesChanged

    Public Sub ShowError(ByVal [error] As String) Implements IHost.ShowError
        ' Hay controles que se crean dinámicamente y es normal que no se localicen en el evento HandleCreated (caso de WinForms)'
        If [error].Contains("Control no localizado") Then
            If [error].Contains("cControles.") Then
                Exit Sub
            End If
        End If

        MessageBox.Show([error])
    End Sub

    ' En esta implementación, el estado podrá ser diferente según el control e instancia que pregunte'
    Public Property State(ByVal ID As String, ByVal instanceID As String) As Integer Implements RestrictedUI.IHost.State
        Get
            Dim nState As Integer = 0
            _State.TryGetValue(ID + instanceID, nState)
            Return nState
        End Get
        Set(ByVal value As Integer)
            _State.Remove(ID + instanceID)
            _State.Add(ID + instanceID, value)
            RaiseEvent StateChanged(ID, instanceID, value)
        End Set
    End Property
    Private _State As New Dictionary(Of String, Integer)

    ' En esta implementación, los roles que devolvamos serán los mismos con independencia del formulario o de la instancia'
    Public Property UserRoles(ByVal ID As String, ByVal instanceID As String) As Integer() Implements RestrictedUI.IHost.UserRoles
        Get
            Return _userRoles
        End Get
        Set(ByVal value As Integer())
            _userRoles = value
            RaiseEvent RolesChanged(ID, instanceID)
        End Set
    End Property
    Private _userRoles As Integer() = New Integer(1) {10, 11}

End Class
```

Aunque en la interface `IHost` las propiedades `State` y `UserRoles` no se han establecido como ReadOnly la lectura de estas propiedades es lo realmente importante. La modificación del estado o roles se usará (si se permite) para testear la seguridad desde el formulario de mantenimiento. Si no es necesario, la implementación de la parte de modificación (set) puede dejarse vacía.

<br />
Por cada formulario o control de usuario en el que queramos aplicar una política de restricciones añadiremos un componente `ControlRestrictedUI`. La configuración de este componente se hará normalmente en tiempo de diseño. Como ejemplo (viéndolo a través del código generado por el Designer):

```
'ControlRestrictedUIWinForms1'

Me.ControlRestrictedUIWinForms1.ConfigFile = "TestWinForms\Security.txt"
Me.ControlRestrictedUIWinForms1.ControlsFile = "TestWinForms\Controls.txt"
Me.ControlRestrictedUIWinForms1.ID = "Form1"
Me.ControlRestrictedUIWinForms1.InstanceID = "00"
Me.ControlRestrictedUIWinForms1.ParentControl = Me
Me.ControlRestrictedUIWinForms1.Paused = False
Me.ControlRestrictedUIWinForms1.RestrictionsDefinition = New String() {
"$Group 0= GroupBox1.CheckBox1, GroupBox1.TextBox2",
"$Group 2= TextBox",
"+0/GroupBox1.CheckBox1,E,2,7", "+99,33/Combo,V"}
```

> En este ejemplo se está configurando la política de seguridad de ese formulario de manera que el control `CheckBox1` incluido dentro de `GroupBox1` sólo estará habilitado cuando el estado de la aplicación sea 2 o 7, con independencia del rol del usuario. Y que el control `Combo` sólo lo podrán ver los roles 99 y 33.
> Nota: En las restricciones definidas directamente sobre el componente no se usan alias de roles, para no depender de ninguna tabla de traducción.

<br />
Aparte de esa configuración, podemos añadir el siguiente código al inicio de ese formulario o contenedor, por ejemplo en el evento Load:

```
'Si vamos a querer que la aplicación permita determinar un estado y rol o roles'
'independiente para cada instancia de una pantalla, porque podamos abrir varias'
'simultáneamente y éstas pueda tener vidas independientes, entonces tendremos que'
'identificar cada instancia:'
'ControlRestrictedUIWinForms1.InstanceID = _InstanceID

'Si hemos creado dinámicamente controles entonces podemos actualizar el fichero de'
' controles (si estamos desarrollando querremos que en modo diseño se nos permita' 
'establecer la seguridad sobre controles no disponibles más que en tiempo de 'ejecución):'
#If DEBUG Then
  ' Para mantener actualizado el fichero de controles y así tener la posibilidad de considerarlos'
  ' al editar la seguridad en tiempo de diseño'
   ControlRestrictedUIWinForms1.RegisterControls()
#End If

'En cualquier caso, si hemos creado controles dinámicamente, no habrán sido localizados '
'al inicializarse la seguridad. Por ello forzaremos a que se reconsidere la seguridad:'
 ControlRestrictedUIWinForms1.ReinitializeSecurity()
```

<br />
Es posible establecer la seguridad directamente a nivel de entorno, para todos o sólo una selección de componentes mediante una cadena de texto, contenida por ejemplo en un fichero, similar a la siguiente:

```
[Factories]
; Las rutas relativas se expresarán de manera relativa a la carpeta de la solución (.sln). Esta ruta se utilizará para localizar las DLL en tiempo de diseño
; Se supondrá que la DLL se encuentra en la misma carpeta que el ejecutable, por lo que en tpo. de ejecución se ignorará la ruta y se utilizará únicamente el nombre del fichero
; Nota: es posible utilizar también rutas absolutas.
TestWinForms\bin\Debug\RestrictedWinFormsUI_Infragistics.dll, RestrictedWinFormsUI_Infragistics.AdapterInfragisticsWinForms_Factory

[CommonRoles]
99,Administrator,Adm
10,Director,Dtor
20,Consultant,Cons

[CommonStates]
0,Initial
1,Pending Validating
2,Validated

;=======================================================
[SECURITYCONTROL=Form1]
[Roles]
30,Auxiliar,Aux

[States]
3,Special State

[Groups]
Group Disable Buttons= gbCommands.btnDisableEnabled, gbCommands.btnDisableVisible
Group Enabled Buttons= gbCommands.btnEnableEnabled, gbCommands.btnEnableVisible, gbCommands.btnEnableVisible_N
Group Trees= TreeView1, UltraTree1

[Restrictions]
#Yes=TextBox,$Group Trees
#No=GroupBox1.CheckBox1
+123/TextBox,V,2
+Adm/$Group Trees,E/MenuStrip1.EditarToolStripMenuItem.CortarToolStripMenuItem,V
+Dtor/GroupBox1.CheckBox1,V
-0/MenuStrip1.ArchivoToolStripMenuItem.NuevoToolStripMenuItem,E/ToolStrip1.ToolStripComboBox1,E/ToolStrip1.ToolStripSplitButton1,E
-Aux/cControles.Name,V,1,2,3/$Group Enabled Buttons,V
-Cons/combo,V/$Group Disable Buttons,E

;=======================================================
[SECURITYCONTROL=Form2_Sub1]
[Roles]

[States]

[Groups]
Grupo 0= CheckBox1, TextBox1
Grupo 1 - nuevo grupo= Button1, TextBox1

[Restrictions]
+0/$Grupo 0,E
-0/CheckBox1,E/Button1,E

```

<br />
A través de esta funcionalidad es posible almacenar restricciones de seguridad en un repositorio centralizado, por ejemplo una BD relacional, para luego recuperarlas y aplicarlas al inicio del la aplicación. De esta manara podemos modificar las restricciones sin necesidad de recompilar las aplicaciones.

En la definición a cargar en el Entorno, por ejemplo desde un fichero, pueden utilizarse alias de roles para definir las restricciones, pues se da por hecho que éstos estarán definidos en el mismo fichero. De cualquier manera al aplicarse al componente de seguridad se traducen en los códigos correspondientes.

Hay que tener en cuenta que el uso de alias es totalmente opcional, como lo es el definir listas de estados y de roles. No son más que una ayuda para facilitar la definición de las restricciones. Lo único importante es que los códigos de los roles y de los estados los entienda la aplicacion a través del objeto que implemente la interface IHost.

Al llamar a `SecurityEnvironment.LoadFrom` o `LoadFormString`, las restricciones definidas en el fichero o cadena de texto tendrán prioridad sobre la embebida en los componentes. Si en ese texto no hay ningún apartado relativo a un determinado componente se seguirá aplicando la seguridad embebida en ese componente. Si se quisiera eliminar todas las restricciones de un componente bastaría con incluirle un apartado a ese componente dejándolo vacío:

```
;=======================================================
[SECURITYCONTROL=Form3]

;=======================================================
[SECURITYCONTROL=Form4]
[Restrictions]
-0/CheckBox1,E/Button1,E
```


<br /><br />
### Mantenimiento de la política de seguridad: `FrmRestrictionsUIDefinition` ###

Tanto la configuración embebida directamente en el componente como a través de archivos de configuración similares al indicado, son generables mediante el formulario de mantenimiento incorporado, y fácilmente modificables a mano si es necesario.
Este formulario de mantenimiento está accesible tanto en tiempo de diseño como de ejecución. En tiempo de diseño se puede acceder a él pulsando en el botón ... correspondiente a la propiedad `RestrictionsDefinition` de un componente `ControlRestrictedUI`; en tiempo de ejecución mediante el método `ShowConfigurationSecurityForm` y también directamente pulsando una combinación de teclas habilitada para ello mediante `SecurityEnvironment.AllowedHotKey` (por defecto desactivado)

![http://restricted-ui.googlecode.com/svn/wiki/images/Spanish/frmRestrictionsUIDefinition.gif](http://restricted-ui.googlecode.com/svn/wiki/images/Spanish/frmRestrictionsUIDefinition.gif)

Básicamente lo que podemos hacer es seleccionar (checkbox) uno o varios controles, la propiedad o propiedades que queremos controlar (Visible, Enabled) y opcionalmente uno o varios roles así como uno o varios estados. Al pulsar en el botón Permitir añadiremos una nueva restricción (positiva) relacionada con esa selección. Al pulsar en Impedir añadiremos una restricción negativa.
Podemos definir grupos de controles, con un nombre asociado, y establecer restricciones directamente al grupo. Para añadir o actualizar un grupo existente marcaremos una serie de controles y pulsaremos Añadir. Nos preguntará por un nombre para el grupo. Si ya existe lo reemplazará por la nueva selección, en caso contrario lo añadirá.
Al seleccionar uno u otro grupo se irán seleccionando en la tabla de controles los elementos correspondientes al grupo. Cuando esté marcada la casilla 'Mostrar sólo Grupo seleccionado' entonces al pulsar en Permitir o Impedir, la restricción se asociará directamente al grupo correspondiente.

Tenemos la opción de salvar las restricciones establecidas en un archivo de configuración, añadiéndose a las restricciones que puedan ya existir relativas a otros componentes. Este archivo será inicialmente el que se haya establecido (si alguno) para el componente.
Podemos leer la configuración de seguridad del componente desde el que hemos lanzado este formulario, o todos los que pueda haber incluidos en el archivo.

Contamos para mayor facilidad con dos formas de mostrar los permisos: tabular o de texto, ambas editables.
<br>
<img src='http://restricted-ui.googlecode.com/svn/wiki/images/Spanish/frmRestrictionsUIDefinition_text.gif' />

Desde esta vista de texto es posible indicar la relación de controles para los que se precisa una tratamiento explícito de la monitorización de la 'desactivación':<br>
aquellos para los que el intento de ocultar o desabilitar debe ser o no supervisado (#Yes=... or #No=...), y por lo tanto podrá o no ser impedido.<br>
<br>
<br>
<br>
Si usamos este formulario en tiempo de ejecución podremos también hacer doble click en un control para hacerlo resaltar (parpadeará varias veces) Tendremos también la posibilidad de modificar (si el objeto que implemente la interface <code>IHost</code> lo permite) el estado y el rol o roles asociados a este componente e instancia, para así testear la seguridad. Para mayor comodidad podemos reducir este formulario para que sólo ofrezca estos controles:<br>
<br>
<img src='http://restricted-ui.googlecode.com/svn/wiki/images/Spanish/frmRestrictionsUIDefinition_reduced.gif' />


<br /><br />
<hr />
<h2>2.3. Proyectos de test</h2>

Junto a la solución se incluyen tres proyectos, dos sobre WinForms y sobre Web: TestWeb, TestWinForms y TestWinForms_notUsingInfragistics.<br>
Los dos proyectos en WinForms son equivalentes con la única diferencia de que el último no hace uso de ningún control (<a href='http://www.infragistics.com/dotnet/netadvantage.aspx'>NetAdventage</a>) de Infragistics, librería de controles utilizada en mi empresa y que he incluido para demostrar el uso de factorías adicionales para tratar controles especiales.<br>
<br>
Los ejemplos en WinForms incluyen un formulario (Form1) con el cual es posible jugar con todas las características de esta librería, incluyendo la posibilidad de pausar la seguridad, forzar la visibilidad o el enabled de un control, o aplicar una lógica más elaborada con la ayuda del evento <code>BeforeApplyingRestriction</code>. El formulario Form1 en TestWinForms es el siguiente:<br>
<br>
<img src='http://restricted-ui.googlecode.com/svn/wiki/images/Spanish/TestForm1.gif' />

<br /><br />
El formulario Form2 permite mostrar el tratamiento de la seguridad sobre controles de usuario: las pestañas 1 y 2 tienen incrustado un control de usuario sobre el que se ha aplicado una serie de restricciones que hacen que sólo sea editable el cuadro de texto. Las pestañas 3 y 4 incluyen otro control de usuario cuya política de seguridad oculta un nodo del TreeView. Aparte de esa restricción específica de este segundo control de usuario, el componente de seguridad de este formulario Form2 aplica una restricción sobre la instancia del control de usuario de la pestaña 4 de manera que el segundo radiobutton esté deshabilitado.<br>
<br>
<img src='http://restricted-ui.googlecode.com/svn/wiki/images/Spanish/TestForm2.gif' />


Tanto en Form1 como en Form2 está habilitada como tecla que muestra el formulario de mantenimiento de la seguridad en tiempo de ejecución la combinación de teclas CTR-ALT-End<br>
<br>
<br>
<br /><br />
<hr />
<h1>3. Points of Interest</h1>
<ul><li>Del desarrollo de este código me ha resultado muy interesante descubrir las posibilidades que ofrece este tipo de "inyección" de código, que permite controlar o influir en la aplicación Host sin necesidad de hacer cambios en la misma (salvo naturalmente la configuración normal de la librería). Este enfoque se ha empleado para actuar (impidiendo según una política de seguridad) sobre un par de propiedades habituales de los controles, Visible y Enabled, pero podría utilizarse para muchos otros usos.</li></ul>

<ul><li>El patrón Indirección, a través de la interface <code>IControlAdapter</code>, ha sido fundamental para conseguir este objetivo, y algo muy potente. Su uso con el patrón Factory ha sido también muy conveniente.<br>
</li></ul><blockquote>El uso de estos dos patrones han hecho posible la extensibilidad de esta arquitectura, permitiendo incorporar nuevas librerías que ofrezcan un tratamiento personalizado de determinados controles.</blockquote>

<ul><li>Aunque sea algo aparentemente menor me parece interesante señalar el uso del patrón Caso Especial, tal y como describe Martin Fowler en <a href='http://martinfowler.com/eaaCatalog/specialCase.html'>P of EAA: Special Case</a> y las ventajas claras que repercute en el código. Este patrón está siendo utilizado mediante el objeto <code>NullControlAdapter</code>.</li></ul>

<ul><li>Durante el desarrollo del componente <code>ControlRestrictedUI</code> tuve dificultades con el hecho de que las propiedades establecidas en tiempo de diseño fueran inicializadas por el Designer en un orden alfabético. La inicialización de la seguridad en un componente se realiza al establecer la propiedad <code>RestrictionsDefinition</code>, pero en ese momento, en la inicialización del mismo por parte del Designer otras propiedades necesarias no estaban establecidas. La solución a este problema la ofrece la interface <code>ISupportInitialize</code> (<a href='http://en.csharp-online.net/Design-Time_Integration%E2%80%94Batch_Initialization'>Design-Time Integration—Batch Initialization</a>)</li></ul>

<ul><li>También ha resultado muy cómodo y limpio el uso de la interface <code>INotifyPropertyChanged</code> para asegurar el correcto enlace de datos bidireccional (databindings) entre controles de usuario y entidades de negocio (<a href='http://www.claassen.net/geek/blog/2007/07/generic-asynchronous.html'>A generic asynchronous INotifyPropertyChanged helper</a>))</li></ul>

<ul><li>Una dificultad importante de este desarrollo ha estado relacionada con el trabajo del componente en tiempo de diseño. Especialmente problemática resultó la instanciación dinámica de objetos <code>IControlAdapterFactory</code> en tiempo de diseño, lo que provocaba excepciones InvalidCastException.<br>
</li></ul><blockquote>Entre otros, el siguiente artículo me ayudó a comprender el problema y el por qué de las excepciones InvalidCastException que estaba obteniendo al instanciar dinámicamente los objetos que implementaban la interface <code>IControlAdapterFactory</code>: <a href='http://www.yoda.arachsys.com/csharp/plugin.html'>Plug-ins and cast exceptions</a>
Pude comprobar posteriormente que al estar instanciando la librería para cargar la factoría auxiliar mediante <code>Assembly.LoadFrom</code> en tiempo de diseño, las estaba cargando en el dominio de aplicación por defecto del proceso asociado al IDE, con lo que estas DLL no era liberadas más que al cerrar el IDE, llegando por tanto a tener por tanto múltiples definiciones aparentemente iguales en memoria.<br>
El problema lo resolví cargando la DLL dentro un dominio independiente, aislado, de aplicación, utilizando <code>AppDomain.LoadFrom</code> en lugar de <code>Assembly.LoadFrom</code>, y descargando el dominio una vez utilizada la factoría.</blockquote>

<ul><li>Para documentar las clases y métodos me he apoyado en los comentarios al código y he utilizado <a href='http://docproject.codeplex.com/'>DocProject</a>, herramienta construida sobre <a href='http://sandcastle.codeplex.com/'>Sandcastle - Documentation Compiler for Managed Class Libraries</a>. Es una combinación extremedamente cómoda y potente. En concreto me he basado en la versión 1.11.0 Release Candidate de DocProject y de Sandcastle May 2008 Release.