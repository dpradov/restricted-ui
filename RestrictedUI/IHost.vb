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
''' Indica que el objeto puede actuar como intermediario en la comunicación necesaria entre la aplicación Host y
''' la librería de Interface Restringida (<see cref="RestrictedUI"/>), permitiendo a ésta consultar en cualquier
''' momento el estado de la aplicación y el rol o roles que pueda tener el usuario de la misma. Informa también 
''' mediante eventos de cambios en estos valores.
''' </summary>
''' <remarks> 
''' <para>Ofrece además un método para recibir posibles mensajes de error detectados desde la librería de seguridad.</para>
''' <para>Estos datos (estado y roles) podrán ser dependientes del formulario o control de usuario desde 
''' el que se pregunte (tipo e instancia concreta).</para>
''' <para>Este enfoque permite, por ejemplo, mantener abiertas diferentes instancias de una determinada
''' ventana, cada una de ellas en un estado o fase de tramitación diferente y por tanto con requerimientos
''' de seguridad distintos.</para>
''' <para>Aunque es más común que el rol o roles del usuario sean uniformes por toda la aplicación se permite que puedan
''' también depender de donde se ubique el componente de seguridad y de la instancia concreta. De esta manera se
''' da más margen de maniobra a la hora de configurar la seguridad.</para>
''' </remarks>
Public Interface IHost

    ' NOTA:
    ' Aunque tanto el estado actual como los roles de usuario no se han establecido como propiedades ReadOnly la lectura de estas propiedades
    ' es lo realmente importante. La modificación del estado o roles se usará (si se permite) para testear la seguridad, desde el formulario de
    ' definición.
    ' Si no se desea, la implementación de la parte de modificación (Set) puede dejarse vacía, como se prefiera 

    ''' <summary>
    ''' Devuelve el estado actual de la aplicación para el formulario o contenedor en el que está
    ''' incrustado el componente de seguridad (<see cref="ControlRestrictedUI "/>) identificado con el parámetro
    ''' <paramref name="ID"/> y la instancia concreta <paramref name="instanceID"/>
    ''' </summary>
    ''' <remarks>
    ''' Este enfoque permite, por ejemplo, mantener abiertas diferentes instancias de una determinada ventana,
    ''' cada una de ellas en un estado o fase de tramitación diferente y por tanto con requerimientos de
    ''' seguridad distintos.
    '''</remarks>
    ''' <param name="ID">Identificador del componente de seguridad</param> 
    ''' <param name="instanceID">Identificador de la instancia del componente de seguridad</param> 
    Property State(ByVal ID As String, ByVal instanceID As String) As Integer

    ''' <summary>
    ''' Devuelve el rol o roles del usuario de la aplicación para el formulario o contenedor en el que está
    ''' incrustado el componente de seguridad (<see cref="ControlRestrictedUI "/>) identificado con el parámetro
    ''' <paramref name="ID"/> y la instancia concreta <paramref name="instanceID"/>
    ''' </summary>
    ''' <remarks>
    ''' Aunque es más común que el rol o roles del usuario sean uniformes por toda la aplicación se permite que puedan
    ''' depender de donde se ubique el componente de seguridad y de la instancia concreta. De esta manera se
    ''' da más margen de maniobra a la hora de configurar la seguridad.
    '''</remarks>
    ''' <param name="ID">Identificador del componente de seguridad</param> 
    ''' <param name="instanceID">Identificador de la instancia del componente de seguridad</param> 
    Property UserRoles(ByVal ID As String, ByVal instanceID As String) As Integer()


    ''' <summary>
    ''' Señala un cambio en el estado actual de la aplicación, cambio que podría afectar a todos los componentes de seguridad, sólo
    ''' a uno concreto e incluso sólo a una instancia concreta de uno.
    ''' </summary>
    ''' <remarks>Si el ID del componente es Nothing deberá ser atendido por todos los componentes. Podrá también restringirse
    ''' a determinadas instancias mediante <paramref name="instanceID"/></remarks> 
    ''' <param name="ID">Identificador del componente de seguridad (que referencia a su vez a un determinado formulario o contenedor)</param> 
    ''' <param name="instanceID">Identificador de la instancia del componente de seguridad</param> 
    Event StateChanged(ByVal ID As String, ByVal instanceID As String, ByVal nuevoEstado As Integer)

    ''' <summary>
    ''' Señala un cambio en el rol o roles que ostenta el usuario de la aplicación, cambio que podrá afectar a todos los 
    ''' componentes de seguridad, sólo a uno concreto e incluso sólo a una instancia concreta de uno.
    ''' </summary>
    ''' <remarks>Si el ID del componente es Nothing deberá será atendido por todos los componentes. Podrá también restringirse 
    ''' a determinadas instancias mediante <paramref name="instanceID"/></remarks> 
    ''' <param name="ID">Identificador del componente de seguridad (que referencia a su vez a un determinado formulario o contenedor)</param> 
    ''' <param name="instanceID">Identificador de la instancia del componente de seguridad</param> 
    Event RolesChanged(ByVal ID As String, ByVal instanceID As String)

    ''' <summary>
    ''' Recibe errores o incidencias generados por la librería de seguridad
    ''' </summary>
    ''' <remarks>Algunas errores o incidencias podrán ser ignorados tranquilamente por la aplicación si señalan que no se ha
    ''' encontrado un determinado control y se sabe que éste es creado dinámicamente</remarks>
    Sub ShowError(ByVal [error] As String)
End Interface

