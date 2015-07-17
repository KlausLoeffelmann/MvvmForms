Imports System.Windows.Forms
Imports System.ComponentModel

Public Class TooltipClosingEventArgs
    Inherits CancelEventArgs

    Sub New(ByVal closeReason As TooltipClosingReason)
        Me.TooltipClosingReason = closeReason
        Me.KeyData = Keys.None
    End Sub

    Sub New(ByVal closeReason As TooltipClosingReason, ByVal keyData As Keys)
        Me.TooltipClosingReason = closeReason
        Me.KeyData = keyData
    End Sub

    Property TooltipClosingReason As TooltipClosingReason
    Property KeyData As Keys
End Class

Public Enum TooltipClosingReason
    AppClicked = 0
    AppFocusChanged = 1
    CloseCalled = 2
    ContentClicked = 3
    Keyboard = 4

    'Infrastructure only.
    SuppressEvent = 256
End Enum
