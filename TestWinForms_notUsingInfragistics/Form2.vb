Public Class Form2

    Private Sub Form2_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ControlRestrictedUIWinForms1.InstanceID = "00"
#If DEBUG Then
        ControlRestrictedUIWinForms1.RegisterControls()
#End If
    End Sub

    Private Sub btnChangeState_N_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnChangeState_N.Click
        Dim dt1 As DateTime
        Dim dt2 As DateTime
        Dim diff As TimeSpan
        Dim r As New Random()

        dt1 = Now
        For i As Integer = 1 To 500
            MainForm._host.State(ControlRestrictedUIWinForms1.ID, ControlRestrictedUIWinForms1.InstanceID) = r.Next(4, 6)
            MainForm._host.State(ControlRestrictedUIWinForms1.ID, ControlRestrictedUIWinForms1.InstanceID) = r.Next(3)
        Next
        dt2 = Now
        diff = dt2 - dt1
        MsgBox("1000 state changes in " + diff.TotalMilliseconds.ToString() + " ms")
    End Sub
End Class