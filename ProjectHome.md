**Restricted UI** (Restricted User Interface)
> How to control the user interface using a policy established in a declaratively way, based on the user roles and the application status.

# Introduction #
A delicate part in the implementation of applications's security, specially corporate applications, is to determine which functionalities must or not be accessible to the user depending on his role, or which elements should or should not be shown.

It is usual to include in the code logic to hide or disable certain options, buttons, etc, depending on the user accessing the application. This can get complicated when apart from the type of user should also be considered the state of the application, such as the processing status of an entity.

Also, it is quite usual that this policy should be changed with some frequency, in the same way that should must evolve the application requirements. Due to changes in the organizational structure of the company, modifications in management protocols or simply the identification of gaps after the use of the application, among other things, would be necessary to make adjustments to this logic related to the interface.

In response to this problem it is also possible to centralize the definition of this policy, in a common repository for multiple applications. Based on this definition, supported by a particular data model, you can define a library used by the applications that makes it possible to modify the security policy without having to recompile and redeploy applications. The main idea is that the programmer develops as if there were only one type of user, administrator, with permission to do anything. It is the library that controls whether an attempt should be prevented from making visible certain controls or enable them.

This approach is currently used in the company where I work. While offering great flexibility in design, it's concrete implementation presents, from my point of view, a number of constraints and practical difficulties that I have tried to correct with this other library / framework:

<br />
# Key Features #
On the one hand the library / architecture described here does not require to centralize the definition of this security, but it's seen as a case; secondly, to ensure compliance with these interface restrictions it is not necessary to require the programmer to use specific methods or properties of the library, to check if the change is allowed or not, nor has to be controlled by any code; simply any attempt to make visible or enabled any interface element supervised not allowed by the security policy is intercepted and prevented. By default, it is only supervised the attempt of making visible or enabled the control, but it can be configured to monitor also the 'deactivation' of that properties, that is, the attempt to make invisible or disabled the control.

It is allowed to define restrictions policy not only on the basis of roles but also on the application states, in both WinForms and Web applications; and to simplify the definition of this policy a form of maintenace is provided, available both at design time and runtime.

Notes:
  * It isn't objective of this library to control user authentication. It is assumed that this is done correctly and it is known, with guarantees, the role or roles that the user holds.
  * The name of the library, RestrictedUI refers to the fact that certain elements of the interface will be restricted according to established security ('Restricted area')

<br />


---


**RestrictedUI**
> Librería para limitar la interface de usuario en base a una política de seguridad

Cómo controlar la interface de usuario mediante una política establecida de manera declarativa, en base al rol o roles del usuario y al estado de la aplicación.

# Introducción #
Una parte delicada de la implementación de la seguridad en aplicaciones, especialmente corporativas, consiste en determinar qué funcionalidad debe estar o no accesible al usuario en función de su rol, o qué elementos deben o no mostrarse.

Lo habitual es incluir en el código la lógica necesaria para ocultar o deshabilitar determinadas opciones, botones, etc, dependiendo del tipo de usuario que accede a la aplicación. Esto puede complicarse cuando aparte del tipo de usuario también debe tenerse en cuenta el estado en que se encuentra la aplicación, por ejemplo el estado de tramitación de una entidad.
Es bastante usual que esta política deba cambiar con cierta frecuencia, de la misma forma que deben evolucionar los requerimientos de las aplicaciones. Modificaciones en la estructura organizativa de la empresa, cambios en los protocolos de gestión de las unidades implicadas o simplemente la identificación de carencias tras el uso de la aplicación, entre otras causas, llevan a tener que hacer adaptaciones a esta lógica ligada a la interface.

En respuesta a esta problemática cabe también la opción de centralizar la definición de esta política, en un repositorio común a múltiples aplicaciones. En base a esta definición, soportada por un modelo de datos determinado, es posible definir una librería que utilicen las aplicaciones, y que haga posible modificar la política de seguridad sin necesidad de recompilar y redistribuir las aplicaciones. La idea principal es la siguiente: el programador desarrolla como si sólo hubiera un tipo de usuario, el administrador, con permisos para hacer cualquier cosa. Es la librería la que controla si debe impedirse un intento de hacer visible o de habilitar determinados controles.

Este enfoque es el que se usa actualmente en la empresa en la que trabajo. Aun ofreciendo gran flexibilidad en su concepción, la implementación concreta de la misma presenta desde mi punto de vista una serie de limitaciones y dificultades concretas que he intentado corregir con esta otra librería/arquitectura:

# Características principales #
Por un lado la librería / arquitectura que aquí se explica no obliga a centralizar la definición de esta seguridad, pero sí la contempla como un caso más; por otro lado para asegurar el cumplimiento de estas restricciones no es necesario obligar al programador a utilizar métodos ni propiedades específicas de la librería, que verifiquen si el cambio se permite o no, ni tiene que ser controlado por código a añadir; simplemente cualquier intento de hacer visible o habilitar cualquier elemento de interface supervisado que no sea admitido por la política de seguridad es interceptado e impedido. Por defecto sólo se supervisa el intento de hacer visible o habilitar, pero es posible forzar también la supervisión de la 'desactivación' de esas propiedades, esto es, del intento de hacer invisible o desabilitar esos controles.
Se permite definir la política de restricciones no sólo en base a roles sino a estados de la aplicación, tanto en aplicaciones WinForms como Web; y para simplificar la definición de esta política se ofrece un formulario de mantenimiento, disponible tanto en tiempo de diseño como de ejecución.

Notas:
  * No es objetivo de esta librería controlar la autenticación del usuario. Se asume que ésta es realizada correctamente y que se conoce, con garantías, el rol o roles que ostenta el usuario.
  * El nombre de la librería, RestrictedUI, hace referencia al hecho de que ciertos elementos de la interface estarán restringidos según la seguridad establecida ('área restringida')