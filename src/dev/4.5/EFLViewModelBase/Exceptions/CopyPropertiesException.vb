Public Class CopyPropertiesException
    Inherits Exception

    Sub New(message As String)
        MyBase.New(message)
    End Sub

    Sub New(message As String, innerException As Exception)
        MyBase.New(message, innerException)
    End Sub

End Class
