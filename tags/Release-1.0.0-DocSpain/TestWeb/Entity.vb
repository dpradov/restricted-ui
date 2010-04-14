Public Class Entity
    Private _a As String
    Public Property a() As String
        Get
            Return _a
        End Get
        Set(ByVal value As String)
            _a = value
        End Set
    End Property

    Private _b As Integer
    Public Property b() As Integer
        Get
            Return _b
        End Get
        Set(ByVal value As Integer)
            _b = value
        End Set
    End Property

    Public Function getDatos() As DataSet
        Dim ds As New DataSet
        Dim t As DataTable
        t = New DataTable("Controles")
        t.Columns.Add("Rol", GetType(Int32))
        t.Columns.Add("ID", GetType(String))
        t.Columns.Add("IDPadre", GetType(String))
        t.Columns.Add("IDControl", GetType(String))
        t.Columns.Add("Nombre", GetType(String))
        t.Columns.Add("Visible", GetType(Boolean))
        t.Columns.Add("Enabled", GetType(Boolean))
        t.Columns.Add("Estados_", GetType(String))
        t.Columns.Add("Estados", GetType(String))
        t.Columns.Add("Numero", GetType(String))

        Dim r As DataRow = t.NewRow
        r("Rol") = 0
        r("ID") = "ID"
        r("IDControl") = "IDControl"
        r("Nombre") = "Nombre"
        t.Rows.Add(r)

        ds.Tables.Add(t)
        Return ds
    End Function

End Class
