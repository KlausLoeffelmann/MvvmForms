Imports System.Windows.Forms

Public Class PopupCloseRequestedEventArgs
    Inherits EventArgs

    Sub New(ByVal closeRequestReason As PopupCloseRequestReasons)
        Me.CloseRequestReason = closeRequestReason
    End Sub

    Property CloseRequestReason As PopupCloseRequestReasons
    Property KeyCode As Keys

End Class

Public Enum PopupCloseRequestReasons
    KeyboardCommit
    KeyboardCancel
    PopupFormRequest
End Enum
