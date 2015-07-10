''' <summary>
''' Bestimmt für ein ViewModel die standardmäßig für Windows Forms zu verwendende View.
''' </summary>
''' <remarks></remarks>
Public NotInheritable Class MvvmViewModelAttribute
    Inherits Attribute

    ''' <summary>
    ''' Erstellt eine neue Instanz dieser Attribut-Klasse, die eine Klasse als ViewModel kennzeichnet.
    ''' </summary>
    ''' <remarks></remarks>
    Sub New()
        MyBase.New()
    End Sub

End Class
