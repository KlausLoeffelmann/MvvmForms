
Public Class ValueChangedEventArgs
    Inherits EventArgs

    Sub New()
        Me.ValueChangedCause = ValueChangedCauses.Undefined
    End Sub

    Sub New(ByVal valueChangedCause As ValueChangedCauses)
        Me.ValueChangedCause = valueChangedCause
    End Sub

    Property ValueChangedCause As ValueChangedCauses

    Public Overrides Function ToString() As String
        Return "Value changed cause: " & Me.ValueChangedCause.ToString
    End Function

    Public Shared Shadows Function Empty() As ValueChangedEventArgs
        Return New ValueChangedEventArgs
    End Function

    Public Shared Function PredefinedWithPropertySetter() As ValueChangedEventArgs
        Return New ValueChangedEventArgs(ValueChangedCauses.PropertySetter)
    End Function

    Public Shared Function PredefinedWithUser() As ValueChangedEventArgs
        Return New ValueChangedEventArgs(ValueChangedCauses.User)
    End Function

End Class

Public Enum ValueChangedCauses
    Undefined
    User
    PropertySetter
End Enum
