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
''' It provides a treatment in the row selection of DataGridView objects (with checkbox) similar to that provided in Gmail contacts:
''' The column with the checkbox control will be marked by selecting the row.
''' You may click the checkbox column itself without losing the added marks so far, either by row selection or by clicking in that 
''' checkbox column
''' </summary>
''' <remarks></remarks>
Public Class GridSelectionMng
    Implements IDisposable

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

#Region " IDisposable Support "

    ' Keep track of when the object is disposed.
    Protected disposed As Boolean = False

    ' This method disposes the base object's resources.
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        Try
            If Not Me.disposed Then
                If disposing Then
                    CleanUp()
                End If
                ' Code to dispose the unmanaged resources held by the class
            End If
            Me.disposed = True
        Finally

        End Try
    End Sub


    ' Do not change or add Overridable to these methods.
    ' Put cleanup code in Dispose(ByVal disposing As Boolean).
    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
        MyBase.Finalize()
    End Sub
#End Region

    Private Sub CleanUp()
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

    ''' <summary>It forces selected rows to be checked and highlighted</summary>
    ''' <param name="uncheckNotSelected">If it's <b>true</b> then it will be highlighted only those that are currently selected, 
    ''' the rest that could have the check but are not selected will uncheck. 
    ''' If it is <b>false</b> those selected will be added to the already marked
    ''' </param>
    ''' <remarks>
    ''' We offer this method because the row selection makes rows being marked, therefore highlighting the latter,
    ''' only when the focus is on control.
    ''' </remarks>
    Public Sub CheckSelectedRows(ByVal uncheckNotSelected As Boolean)
        If uncheckNotSelected Then
            CheckOnlySelectedRows()
        Else
            CheckSelectedRows()
        End If
        VerifyExistenceRowsChecked()
    End Sub

    Private Sub cGrid_CellClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs)
        If My.Computer.Keyboard.ShiftKeyDown Then Exit Sub ' If Shift pressed -> Exit because it will already be managed from SelectionChanged

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
                    CheckSelectedRows() ' Shift+Left Click -> We add selected rows to those already marked
                End If

            ElseIf Control.MouseButtons = Windows.Forms.MouseButtons.None Then
                If Not (My.Computer.Keyboard.CtrlKeyDown Or My.Computer.Keyboard.ShiftKeyDown) Then
                    CheckOnlySelectedRows()  ' Selection changed, mouse not used, and Shift and Control not pressed -> mark only those that are selected
                Else
                    CheckSelectedRows()  ' Only keyboard. Shift or Control pressed -> We add selected rows to those already marked
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
