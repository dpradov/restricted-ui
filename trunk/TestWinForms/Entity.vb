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
End Class
