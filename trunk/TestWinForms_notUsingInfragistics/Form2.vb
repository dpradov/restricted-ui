Public Class Form2

    Private Sub Form2_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
#If DEBUG Then
        ControlSeguridadWinForms1.RegisterControls()
#End If
    End Sub

End Class