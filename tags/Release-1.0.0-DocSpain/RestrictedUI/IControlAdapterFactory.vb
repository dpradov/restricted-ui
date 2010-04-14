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
''' Define una factoría externa de adaptadores de control (<see cref="IControlAdapter"/>). 
''' </summary>
''' <remarks>
''' Éstas serán consultadas antes que la factoría base (interna) del componente (<see cref="SecurityEnvironment.BaseFactory"/> a la hora
''' de localizar el adaptador a utilizar.
''' </remarks>
Public Interface IControlAdapterFactory

    ''' <summary>
    ''' Devuelve el adaptador más idóneo correspondiente al control que se le pasa, según la factoría.
    ''' Si la factoría no tiene ningún adaptador adecuado para ese control devolverá el adaptador 
    ''' especial (<see cref="NullControlAdapter "/>)
    ''' </summary>
    ''' <param name="control">Objeto para el que se desea buscar un adaptador</param>
    ''' <param name="parent ">Objeto padre de aquel indicado en <paramref name="control"/>. 
    ''' </param>
    ''' <returns></returns>
    ''' <remarks>El parámetro <paramref name="padre"/> es requerido por algunos pocos adaptadores, 
    ''' principalmente los correspondientes a aquellos controles que no disponen de ningún evento 
    ''' que permita controlar la modificación de las propiedades a supervisar, y tiene por tanto que
    ''' recurrirse a los eventos de su objeto padre. En estos casos, si el control tampoco permite 
    ''' acceder a su objeto padre deberá facilitarse. Como ejemplo pueden verse los adaptadores 
    ''' AdapterWeb_DataControlField o AdapterWinForms_DataGridViewColumn</remarks>
    Function GetAdapter(ByVal control As Object, Optional ByVal parent As Object = Nothing) As IControlAdapter
End Interface
