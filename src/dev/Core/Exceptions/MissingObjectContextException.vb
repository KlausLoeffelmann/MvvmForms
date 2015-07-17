''' <summary>
''' Wird ausgelöst, wenn im FormsToBusinessClass-Manager an einem Entitätsobjekt ein 
''' Context-Wechsel durchgeführt werden müsste, aber ein entsprechender ObjectContext nicht zu finden war.
''' </summary>
''' <remarks></remarks>
Public Class MissingObjectContextException
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