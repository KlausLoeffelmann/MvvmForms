Public Class ValidationIssues
    Inherits List(Of ValidationIssue)

End Class

Public Class ValidationIssue
    Property ControlCausedFailedValidation As INullableValueControl
    Property ValidationException As Exception
    Property Message As String
End Class
