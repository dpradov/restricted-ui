Public Partial Class WebUserControl1
    Inherits System.Web.UI.UserControl

    Friend WithEvents ControlSeguridadWeb1 As RestrictedWebUI.ControlRestrictedUIWeb

    Private Sub InitializeComponent()
        Me.ControlSeguridadWeb1 = New RestrictedWebUI.ControlRestrictedUIWeb
        CType(Me.ControlSeguridadWeb1, System.ComponentModel.ISupportInitialize).BeginInit()
        '
        'ControlSeguridadWeb1
        '
        Me.ControlSeguridadWeb1.ConfigFile = "PruebaWeb\Seguridad.txt"
        Me.ControlSeguridadWeb1.ControlsFile = "PruebaWeb\Controles.txt"
        Me.ControlSeguridadWeb1.ID = "WebUserControl1"
        Me.ControlSeguridadWeb1.InstanceID = "00"
        Me.ControlSeguridadWeb1.ParentControl = Me
        Me.ControlSeguridadWeb1.Paused = False
        Me.ControlSeguridadWeb1.RestrictionsDefinition = New String() {"$GrupoPequeño= Button1, TextBox1", "-0/Button1,E"}
        CType(Me.ControlSeguridadWeb1, System.ComponentModel.ISupportInitialize).EndInit()

    End Sub

    Protected Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs)
        InitializeComponent()
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs)
#If DEBUG Then
        ControlSeguridadWeb1.RegisterControls()
#End If
    End Sub
End Class