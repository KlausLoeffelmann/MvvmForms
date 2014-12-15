Public Class AfterFormValidatedEventArgs
    Inherits EventArgs

    Sub New()
        MyBase.new()
    End Sub

    Sub New(ByVal validationIssues As ValidationIssues)
        Me.ValidationIssues = validationIssues
    End Sub

    Property ValidationIssues As ValidationIssues
End Class