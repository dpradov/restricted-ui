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
''' Defines an external factory of control adapters (<see cref="IControlAdapter"/>). 
''' </summary>
''' <remarks>
''' These external factories will be consulted before the base factory (internal) of the security component 
''' (<see cref="SecurityEnvironment.BaseFactory"/>) when it comes to locating the adapter to be used.
''' </remarks>
Public Interface IControlAdapterFactory

    ''' <summary>
    ''' Returns the most suitable adapter for the control that is being passed, according to the factory. 
    ''' If the factory has no proper adapter for this control, it will return the special adapter <see cref="NullControlAdapter "/>.
    ''' </summary>
    ''' <param name="control">The object for which you want to find an adapter</param>
    ''' <param name="parent ">Parent object of that shown in <paramref name="control"/>.</param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Parameter <paramref name="padre"/> is required for a few adapters, mainly those related to the controls with 
    ''' no event that make possible to control the change in the properties to monitor, and therefore have to resort 
    ''' to the events of its parent object.<br/>
    ''' In these cases, if the control does not allow access to its parent object, you must provide this parent object.
    ''' As an example you can see the adapters AdapterWinForms_DataGridViewColumn or AdapterWeb_DataControlField
    ''' </remarks>
    Function GetAdapter(ByVal control As Object, Optional ByVal parent As Object = Nothing) As IControlAdapter
End Interface
