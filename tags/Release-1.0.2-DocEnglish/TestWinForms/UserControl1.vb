Public Class UserControl1

    Private Sub UserControl1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
#If DEBUG Then
        ControlSeguridadWinForms1.RegisterControls()
#End If
    End Sub
End Class
