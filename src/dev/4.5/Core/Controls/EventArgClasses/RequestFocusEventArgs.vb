''' <summary>
''' Stellt Daten für das RequestFocus-Ereignis zur Verfügung, die darüber Aufschluss geben, ob das Fokussieren zum Erfolg führte oder nicht.
''' </summary>
''' <remarks></remarks>
Public Class RequestFocusEventArgs
    Inherits EventArgs

    Sub New()
        Succeeded = False
    End Sub

    Sub New(succeeded As Boolean)
        Me.Succeeded = succeeded
    End Sub

    Property Succeeded As Boolean

End Class
