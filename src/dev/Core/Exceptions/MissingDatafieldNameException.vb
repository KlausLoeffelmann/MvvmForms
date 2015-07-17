''' <summary>
''' Wird ausgelöst, wenn bei einem zugewiesenen NullableValueControl an einen 
''' FormToBusinessclass-Manager die DataField-Eigenschaft nicht gesetzt wurde.
''' </summary>
''' <remarks></remarks>
Public Class MissingDatafieldNameException
    Inherits SystemException

    Sub New()
        MyBase.New()
    End Sub

    Sub New(ByVal Message As String)
        MyBase.New(Message)
    End Sub

    Sub New(ByVal Message As String, ByVal innerException As Exception)
        MyBase.New(Message, innerException)
    End Sub
End Class
