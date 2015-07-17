''' <summary>
''' Wird ausgelöst, wenn eine View aus einem ViewModel für ein MvvmViewSelectorContainer 
''' abgeleitet werden soll, das erforderliche MvvmViewAttribut über der ViewModel-Klasse aber 
''' nicht angegeben wurde.
''' </summary>
''' <remarks></remarks>
Class MissingViewAttributeException
    Inherits Exception

    Sub New(message As String)
        MyBase.New(message)
    End Sub

    Sub New(message As String, innerException As Exception)
        MyBase.New(message, innerException)
    End Sub

End Class
