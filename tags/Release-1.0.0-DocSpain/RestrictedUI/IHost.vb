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
''' Indica que el objeto puede actuar como intermediario en la comunicaci�n necesaria entre la aplicaci�n Host y
''' la librer�a de Interface Restringida (<see cref="RestrictedUI"/>), permitiendo a �sta consultar en cualquier
''' momento el estado de la aplicaci�n y el rol o roles que pueda tener el usuario de la misma. Informa tambi�n 
''' mediante eventos de cambios en estos valores.
''' </summary>
''' <remarks> 
''' <para>Ofrece adem�s un m�todo para recibir posibles mensajes de error detectados desde la librer�a de seguridad.</para>
''' <para>Estos datos (estado y roles) podr�n ser dependientes del formulario o control de usuario desde 
''' el que se pregunte (tipo e instancia concreta).</para>
''' <para>Este enfoque permite, por ejemplo, mantener abiertas diferentes instancias de una determinada
''' ventana, cada una de ellas en un estado o fase de tramitaci�n diferente y por tanto con requerimientos
''' de seguridad distintos.</para>
''' <para>Aunque es m�s com�n que el rol o roles del usuario sean uniformes por toda la aplicaci�n se permite que puedan
''' tambi�n depender de donde se ubique el componente de seguridad y de la instancia concreta. De esta manera se
''' da m�s margen de maniobra a la hora de configurar la seguridad.</para>
''' </remarks>
Public Interface IHost

    ' NOTA:
    ' Aunque tanto el estado actual como los roles de usuario no se han establecido como propiedades ReadOnly la lectura de estas propiedades
    ' es lo realmente importante. La modificaci�n del estado o roles se usar� (si se permite) para testear la seguridad, desde el formulario de
    ' definici�n.
    ' Si no se desea, la implementaci�n de la parte de modificaci�n (Set) puede dejarse vac�a, como se prefiera 

    ''' <summary>
    ''' Devuelve el estado actual de la aplicaci�n para el formulario o contenedor en el que est�
    ''' incrustado el componente de seguridad (<see cref="ControlRestrictedUI "/>) identificado con el par�metro
    ''' <paramref name="ID"/> y la instancia concreta <paramref name="instanceID"/>
    ''' </summary>
    ''' <remarks>
    ''' Este enfoque permite, por ejemplo, mantener abiertas diferentes instancias de una determinada ventana,
    ''' cada una de ellas en un estado o fase de tramitaci�n diferente y por tanto con requerimientos de
    ''' seguridad distintos.
    '''</remarks>
    ''' <param name="ID">Identificador del componente de seguridad</param> 
    ''' <param name="instanceID">Identificador de la instancia del componente de seguridad</param> 
    Property State(ByVal ID As String, ByVal instanceID As String) As Integer

    ''' <summary>
    ''' Devuelve el rol o roles del usuario de la aplicaci�n para el formulario o contenedor en el que est�
    ''' incrustado el componente de seguridad (<see cref="ControlRestrictedUI "/>) identificado con el par�metro
    ''' <paramref name="ID"/> y la instancia concreta <paramref name="instanceID"/>
    ''' </summary>
    ''' <remarks>
    ''' Aunque es m�s com�n que el rol o roles del usuario sean uniformes por toda la aplicaci�n se permite que puedan
    ''' depender de donde se ubique el componente de seguridad y de la instancia concreta. De esta manera se
    ''' da m�s margen de maniobra a la hora de configurar la seguridad.
    '''</remarks>
    ''' <param name="ID">Identificador del componente de seguridad</param> 
    ''' <param name="instanceID">Identificador de la instancia del componente de seguridad</param> 
    Property UserRoles(ByVal ID As String, ByVal instanceID As String) As Integer()


    ''' <summary>
    ''' Se�ala un cambio en el estado actual de la aplicaci�n, cambio que podr�a afectar a todos los componentes de seguridad, s�lo
    ''' a uno concreto e incluso s�lo a una instancia concreta de uno.
    ''' </summary>
    ''' <remarks>Si el ID del componente es Nothing deber� ser atendido por todos los componentes. Podr� tambi�n restringirse
    ''' a determinadas instancias mediante <paramref name="instanceID"/></remarks> 
    ''' <param name="ID">Identificador del componente de seguridad (que referencia a su vez a un determinado formulario o contenedor)</param> 
    ''' <param name="instanceID">Identificador de la instancia del componente de seguridad</param> 
    Event StateChanged(ByVal ID As String, ByVal instanceID As String, ByVal nuevoEstado As Integer)

    ''' <summary>
    ''' Se�ala un cambio en el rol o roles que ostenta el usuario de la aplicaci�n, cambio que podr� afectar a todos los 
    ''' componentes de seguridad, s�lo a uno concreto e incluso s�lo a una instancia concreta de uno.
    ''' </summary>
    ''' <remarks>Si el ID del componente es Nothing deber� ser� atendido por todos los componentes. Podr� tambi�n restringirse 
    ''' a determinadas instancias mediante <paramref name="instanceID"/></remarks> 
    ''' <param name="ID">Identificador del componente de seguridad (que referencia a su vez a un determinado formulario o contenedor)</param> 
    ''' <param name="instanceID">Identificador de la instancia del componente de seguridad</param> 
    Event RolesChanged(ByVal ID As String, ByVal instanceID As String)

    ''' <summary>
    ''' Recibe errores o incidencias generados por la librer�a de seguridad
    ''' </summary>
    ''' <remarks>Algunas errores o incidencias podr�n ser ignorados tranquilamente por la aplicaci�n si se�alan que no se ha
    ''' encontrado un determinado control y se sabe que �ste es creado din�micamente</remarks>
    Sub ShowError(ByVal [error] As String)
End Interface

