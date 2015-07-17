Public Class ValidationFailedEventArgs
    Inherits EventArgs

    Sub New()
        MyBase.new()
    End Sub

    Sub New(ByVal originalSource As INullableValueDataBinding,
            ByVal originalEventArgs As RequestValidationFailedReactionEventArgs)
        Me.OriginalSource = originalSource
        Me.OriginalEventArgs = originalEventArgs
    End Sub

    Property OriginalSource As INullableValueDataBinding
    Property OriginalEventArgs As RequestValidationFailedReactionEventArgs
End Class
