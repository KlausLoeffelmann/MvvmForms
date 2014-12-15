Public Class RequestValidationFailedReactionEventArgs
    Inherits EventArgs

    Sub New()
        MyBase.new()
    End Sub

    Sub New(ByVal validationFailedReaction As ValidationFailedActions)
        Me.ValidationFailedReaction = validationFailedReaction
    End Sub

    Sub New(ByVal validationFailedReaction As ValidationFailedActions,
            ByVal newBallonMessage As String)
        Me.ValidationFailedReaction = validationFailedReaction
        Me.BallonMessage = newBallonMessage
    End Sub

    Property ValidationFailedReaction As ValidationFailedActions
    Property BallonMessage As String
    Property CausingException As ContainsUIMessageException

End Class

<Flags()>
Public Enum ValidationFailedActions
    AllowLooseFocus = 0
    ForceKeepFocus = 1
    ChangeBallonMessage = 2
End Enum

