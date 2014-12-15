Public Class ValueValidationStateStore(Of NullableType As {Structure, IComparable})

    Private Shared myPredefinedValidatingInstance As ValueValidationStateStore(Of NullableType)
    Private Shared myPredefinedValidatedInstance As ValueValidationStateStore(Of NullableType)

    Public Property EventType As ValidateEventType
    Public Property ValidationEventArgs As NullableValueValidationEventArgs(Of NullableType?)
    Public Property RaiseValueValidationStateChangedEventUnconditionally As Boolean

    Public Shared Function GetPredefinedValidatingInstance() As ValueValidationStateStore(Of NullableType)
        If myPredefinedValidatingInstance Is Nothing Then
            myPredefinedValidatingInstance = New ValueValidationStateStore(Of NullableType) With {.EventType = ValidateEventType.Validating,
                                                                                             .RaiseValueValidationStateChangedEventUnconditionally = True}
        End If
        Return myPredefinedValidatingInstance
    End Function

    Public Shared Function GetPredefinedValidatedInstance() As ValueValidationStateStore(Of NullableType)
        If myPredefinedValidatedInstance Is Nothing Then
            myPredefinedValidatedInstance = New ValueValidationStateStore(Of NullableType) With {.EventType = ValidateEventType.Validated,
                                                                                            .RaiseValueValidationStateChangedEventUnconditionally = True}
        End If
        Return myPredefinedValidatedInstance
    End Function
End Class

Public Enum ValidateEventType
    Validating
    Validated
End Enum