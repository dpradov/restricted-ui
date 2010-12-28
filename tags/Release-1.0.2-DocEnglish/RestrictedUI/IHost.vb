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
''' It indicates that the object can act as intermediary in the necessary communication between the Host and 
''' the Restricted Interface library (<see cref="RestrictedUI"/>), enabling it to view at any time the application status and the role or 
''' roles that may have the user. It also reports through events of changes to these values.
''' </summary>
''' <remarks> 
''' <para>It also offers a method to receive possible error messages detected in the security library.</para>
''' <para>These data (state and roles) may be dependent on the form or user control from which we are asking (type and specific instance).
''' </para>
''' <para>This approach allows, for example, keep different instances of a particular window open, each one in a state or a different 
''' processing stage and therefore with different security requirements.</para>
''' <para>While it is more common that the user role or roles should be uniform throughout the application, they are also allowed 
''' to depend on where you place the security component and the specific instance. This will give more leeway when configuring the
''' security.</para>
''' </remarks>
Public Interface IHost

    ' NOTE:
    ' Although in the IHost interface the properties State and UserRoles have not been set as ReadOnly, the reading of these properties is 
    ' the most important thing. The change in state or roles will be used (if allowed) to test security from the maintenance form. 
    ' If it is not necessary, the implementation of the modification (set) can be left empty.


    ''' <summary>
    ''' Returns the current state of the application, on the form or container on wich the security component (<see cref="ControlRestrictedUI "/>)
    ''' is embedded. The security component is identified with the parameter <paramref name="ID"/> and the particular instance: <paramref name="instanceID"/>
    ''' </summary>
    ''' <remarks>
    ''' This approach allows, for example, keep different instances of a particular window open, each one in a state or a different 
    ''' processing stage and therefore with different security requirements.
    '''</remarks>
    ''' <param name="ID">Identifier of the security component</param> 
    ''' <param name="instanceID">Identifier of the instance of the security component</param> 
    Property State(ByVal ID As String, ByVal instanceID As String) As Integer

    ''' <summary>
    ''' Returns the roles of the application user, on the form or container where the security component (<see cref="ControlRestrictedUI "/>),
    ''' identified with the parameter <paramref name="ID"/> and the particular instance <paramref name="instanceID"/>, is embedded.
    ''' </summary>
    ''' <remarks>
    ''' While it is more common that the user role or roles should be uniform throughout the application, they are also allowed to 
    ''' depend on where you place the security component and the specific instance. 
    ''' This will give more leeway when configuring the security.
    '''</remarks>
    ''' <param name="ID">Identifier of the security component</param> 
    ''' <param name="instanceID">Identifier of the instance of the security component</param> 
    Property UserRoles(ByVal ID As String, ByVal instanceID As String) As Integer()


    ''' <summary>
    ''' It signals a change in the current state of the application, a change that could affect all security components, 
    ''' only to a specific component, even just a specific instance of one component.
    ''' </summary>
    ''' <remarks>If the parameter <paramref name="ID"/> is Nothing then it should be adressed by all components. It may also be restricted to some
    ''' instances using <paramref name="instanceID"/></remarks> 
    ''' <param name="ID">Identifier of the security component (referring in turn to a certain form or container)</param> 
    ''' <param name="instanceID">Identifier of the instance of the security component</param> 
    Event StateChanged(ByVal ID As String, ByVal instanceID As String, ByVal nuevoEstado As Integer)

    ''' <summary>
    ''' It signals a change on the roles of the application user, a change that could affect all security components, 
    ''' only to a specific component, even just a specific instance of one component.
    ''' </summary>
    ''' <remarks>If the parameter <paramref name="ID"/> is Nothing then it should be adressed by all components. It may also be restricted to some
    ''' instances using <paramref name="instanceID"/></remarks> 
    ''' <param name="ID">Identifier of the security component (referring in turn to a certain form or container)</param> 
    ''' <param name="instanceID">Identifier of the instance of the security component</param> 
    Event RolesChanged(ByVal ID As String, ByVal instanceID As String)

    ''' <summary>
    ''' It receives error o incidence messages generated by the security library
    ''' </summary>
    ''' <remarks>Some of the errors or incidences may be safely ignored by the application if they are related to a not found control
    ''' and we know that this control is created dinamically
    ''' </remarks>
    Sub ShowError(ByVal [error] As String)
End Interface

