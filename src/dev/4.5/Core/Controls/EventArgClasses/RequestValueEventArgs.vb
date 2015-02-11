Public Class RequestValueEventArgs
    Inherits EventArgs

    Sub New(ByVal value As Object)
        value = value
    End Sub

    Property Value As Object

End Class
