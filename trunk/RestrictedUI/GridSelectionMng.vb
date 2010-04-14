Option Strict On

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
''' Ofrece un tratamiento de selección de filas en objetos DataGridView (con checkbox) similar al que ofrece Gmail en sus contactos:
''' La columna con el control checkbox se marcará al seleccionar la fila. Es posible usar hacer clic en la propia
''' columna del checkbox sin perder las marcas añadidas hasta el momento, bien por selección bien por otros clic en
''' dicha columna
''' </summary>
''' <remarks></remarks>
Public Class GridSelectionMng
    Private _cGrid As DataGridView
    Private _cbSelectAll As CheckBox
    Private _colIndex As Integer

    Sub New(ByVal dataGrid As DataGridView, ByVal colIndex As Integer, Optional ByVal checkboxAll As CheckBox = Nothing)
        If dataGrid Is Nothing Then Exit Sub

        _cGrid = dataGrid
        _cbSelectAll = checkboxAll
        _colIndex = colIndex

        AddHandler _cGrid.CellClick, AddressOf cGrid_CellClick
        AddHandler _cGrid.SelectionChanged, AddressOf cGrid_SelectionChanged
        AddHandler _cGrid.CellValueChanged, AddressOf cGrid_CellValueChanged
        AddHandler _cGrid.CurrentCellDirtyStateChanged, AddressOf cGrid_CurrentCellDirtyStateChanged
        AddHandler _cGrid.CellPainting, AddressOf cGrid_CellPainting

        If _cbSelectAll IsNot Nothing Then
            AddHandler _cbSelectAll.Click, AddressOf cbSelectAll_Click
        End If
    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()

        If _cGrid IsNot Nothing Then
            RemoveHandler _cGrid.CellClick, AddressOf cGrid_CellClick
            RemoveHandler _cGrid.SelectionChanged, AddressOf cGrid_SelectionChanged
            RemoveHandler _cGrid.CellValueChanged, AddressOf cGrid_CellValueChanged
            RemoveHandler _cGrid.CurrentCellDirtyStateChanged, AddressOf cGrid_CurrentCellDirtyStateChanged
            RemoveHandler _cGrid.CellPainting, AddressOf cGrid_CellPainting
        End If
        If _cbSelectAll IsNot Nothing Then
            RemoveHandler _cbSelectAll.Click, AddressOf cbSelectAll_Click
        End If
    End Sub

    Public Property ColorCheckedRow() As Brush
        Get
            Return _colorCheckedRow
        End Get
        Set(ByVal value As Brush)
            _colorCheckedRow = value
        End Set
    End Property
    Private _colorCheckedRow As Brush = Brushes.LightSalmon

    ''' <summary>
    ''' Fuerza que se resalten las filas seleccionadas 
    ''' </summary>
    ''' <param name="uncheckNotSelected">Si es <b>true</b> tan sólo se resaltarán las que estén actualmente seleccionadas, el resto que
    ''' pudieran tener el check pero no estar seleccionadas se desmarcarán. Si es <b>false</b> las seleccionadas se añadirán a las que
    ''' ya estén marcadas</param>
    ''' <remarks>Se ofrece este método porque la selección de filas sólo se traduce en el marcado de las mismas, con el consiguiente
    ''' resaltado de éstas, cuando el foco se encuentra en el control.</remarks>
    Public Sub CheckSelectedRows(ByVal uncheckNotSelected As Boolean)
        If uncheckNotSelected Then
            CheckOnlySelectedRows()
        Else
            CheckSelectedRows()
        End If
        VerifyExistenceRowsChecked()
    End Sub

    Private Sub cGrid_CellClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs)
        If My.Computer.Keyboard.ShiftKeyDown Then Exit Sub ' Si Shift pulsado -> salir pues ya se habrá gestionado desde SelectionChanged

        If e.ColumnIndex <> 0 Then
            If Not (My.Computer.Keyboard.CtrlKeyDown Or My.Computer.Keyboard.ShiftKeyDown) Then
                CheckOnlySelectedRows()
            Else
                _cGrid.Rows(e.RowIndex).Cells(_colIndex).Value = Not CType(_cGrid.Rows(e.RowIndex).Cells(_colIndex).Value, Boolean)
                _cGrid.InvalidateRow(e.RowIndex)
            End If
        End If
        VerifyExistenceRowsChecked()
    End Sub

    Private Sub cGrid_SelectionChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If _cGrid.Focused Then
            If Control.MouseButtons = Windows.Forms.MouseButtons.Left Then
                If My.Computer.Keyboard.ShiftKeyDown Then
                    CheckSelectedRows() ' Shift+Left Click -> Añadimos a las ya marcadas las filas seleccionadas
                End If

            ElseIf Control.MouseButtons = Windows.Forms.MouseButtons.None Then
                If Not (My.Computer.Keyboard.CtrlKeyDown Or My.Computer.Keyboard.ShiftKeyDown) Then
                    CheckOnlySelectedRows()  ' Selección modificada, no usado el ratón, y Shift y Control no pulsados -> marcamos exclusivamente los que estén seleccionados
                Else
                    CheckSelectedRows()  ' Sólo teclado. Shift o Control pulsados -> se añadirán a las ya marcadas las filas seleccionadas
                End If
            End If
        End If
        VerifyExistenceRowsChecked()
    End Sub

    Private Sub cGrid_CellValueChanged(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs)
        If e.ColumnIndex = 0 Then
            _cGrid.InvalidateRow(e.RowIndex)
            VerifyExistenceRowsChecked()
        End If
    End Sub

    Private Sub cGrid_CurrentCellDirtyStateChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If _cGrid.CurrentCell.ColumnIndex <> 0 Or _cGrid.CurrentRow.IsNewRow Then Exit Sub
        If _cGrid.IsCurrentCellDirty Then
            _cGrid.CommitEdit(DataGridViewDataErrorContexts.Commit)
        End If

    End Sub

    Private Sub cGrid_CellPainting(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellPaintingEventArgs)
        If e.RowIndex < 0 Then Exit Sub
        If _cGrid.Focused And _cGrid.CurrentCell IsNot Nothing AndAlso (Not _cGrid.CurrentCell.ReadOnly And _cGrid.Rows(e.RowIndex).Cells(e.ColumnIndex) Is _cGrid.CurrentCell) Then
            e.PaintBackground(e.CellBounds, True)

        ElseIf CType(_cGrid.Rows(e.RowIndex).Cells(_colIndex).Value, Boolean) Then
            Dim gridBrush As New SolidBrush(_cGrid.GridColor)
            Dim gridLinePen As New Pen(gridBrush)

            e.Graphics.FillRectangle(_colorCheckedRow, e.CellBounds)

            ' Draw the grid lines (only the right and bottom lines. DataGridView takes care of the others).
            e.Graphics.DrawLine(gridLinePen, e.CellBounds.Left, _
                e.CellBounds.Bottom - 1, e.CellBounds.Right - 1, _
                e.CellBounds.Bottom - 1)
            e.Graphics.DrawLine(gridLinePen, e.CellBounds.Right - 1, _
                e.CellBounds.Top, e.CellBounds.Right - 1, _
                e.CellBounds.Bottom)
        Else
            e.PaintBackground(e.CellBounds, False)
        End If
        e.PaintContent(e.CellBounds)
        e.Handled = True
    End Sub

    Private Sub cbSelectAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        _cGrid.Invalidate()
    End Sub

    Private Function VerifyExistenceRowsChecked(Optional ByVal verifyDataBoundItem As Boolean = False) As Boolean
        Dim hayFilasMarcadas As Boolean = False
        For Each r As DataGridViewRow In _cGrid.Rows
            Try
                If verifyDataBoundItem Then
                    If r.DataBoundItem Is Nothing Then Continue For
                End If
                If CType(r.Cells(_colIndex).Value, Boolean) Then
                    hayFilasMarcadas = True
                    Exit For
                End If
            Catch ex As Exception
            End Try
        Next
        If _cbSelectAll IsNot Nothing Then
            _cbSelectAll.Checked = hayFilasMarcadas
        End If
        Return hayFilasMarcadas
    End Function

    Private Sub CheckSelectedRows()
        If _cGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect Then
            For Each r As DataGridViewRow In _cGrid.SelectedRows
                r.Cells(_colIndex).Value = True
            Next

        Else
            For Each c As DataGridViewCell In _cGrid.SelectedCells
                _cGrid.Rows(c.RowIndex).Cells(_colIndex).Value = True
            Next
        End If

    End Sub

    Private Sub CheckOnlySelectedRows()
        If _cGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect Then
            For Each r As DataGridViewRow In _cGrid.Rows
                If CType(r.Cells(_colIndex).Value, Boolean) <> r.Selected Then
                    r.Cells(_colIndex).Value = r.Selected
                End If
            Next

        Else
            Dim selected As Boolean
            For Each r As DataGridViewRow In _cGrid.Rows
                If r.IsNewRow Then Continue For
                selected = False
                For Each c As DataGridViewCell In r.Cells
                    If _cGrid.SelectedCells.Contains(c) Then
                        selected = True
                        Exit For
                    End If
                Next
                r.Cells(_colIndex).Value = selected
            Next
        End If

    End Sub

End Class
