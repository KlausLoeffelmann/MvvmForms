Public Class PressedStateChangedEventArgs
    Inherits EventArgs

    Sub New()
        MyBase.New()
    End Sub

    Sub New(changedByCode As Boolean)
        If changedByCode Then
            PressedStateChangedReason = EntitiesFormsLib.PressedStateChangedReason.PropertySetter
        Else
            PressedStateChangedReason = EntitiesFormsLib.PressedStateChangedReason.User
        End If
    End Sub

    Public Property PressedStateChangedReason As PressedStateChangedReason

End Class

Public Enum PressedStateChangedReason
    Undefined
    User
    PropertySetter
End Enum
