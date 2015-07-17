Imports System.Windows.Forms
Imports System.ComponentModel

Public Class PopupClosingEventArgs
    Inherits CancelEventArgs

    Sub New(ByVal closeReason As PopupClosingReason)
        Me.PopupCloseReason = closeReason
        Me.KeyData = Keys.None
    End Sub

    Sub New(ByVal closeReason As PopupClosingReason, ByVal keyData As Keys)
        Me.PopupCloseReason = closeReason
        Me.KeyData = keyData
    End Sub

    Property PopupCloseReason As PopupClosingReason
    Property KeyData As Keys
    Property Caused As PopupCloseCause
End Class

Public Enum PopupCloseCause
    ExternalByUser = 0
    InternalByComponent = 1
End Enum

Public Enum PopupClosingReason

    ''' <summary>
    ''' Es wurde auf einen anderen Teil der Oberfläche geklickt, was zum Schließen des Popup führte.
    ''' </summary>
    ''' <remarks></remarks>
    AppClicked = 0

    ''' <summary>
    ''' Die Anwendung wurde in den Hintergrund gestellt, weil zu einer anderen Anwendung gewechselt wurde.
    ''' </summary>
    ''' <remarks></remarks>
    AppFocusChanged = 1

    ''' <summary>
    ''' Die Popup-Close-Methode wurde aufgerufen.
    ''' </summary>
    ''' <remarks></remarks>
    CloseCalled = 2

    ''' <summary>
    ''' Es wurde in den geöffneten Content des Popups geklickt.
    ''' </summary>
    ''' <remarks></remarks>
    ContentClicked = 3

    ''' <summary>
    ''' Popup wurde durch eine entsprechende Keyboard-Funktionalität geschlossen.
    ''' </summary>
    ''' <remarks></remarks>
    Keyboard = 4

    ''' <summary>
    ''' Popup wurde mit dem DropDown-Button geschlossen.
    ''' </summary>
    ''' <remarks></remarks>
    PopupOpenerClicked = 5

    ''' <summary>
    ''' Ein Reset-Button innerhalb des geöffneten Popups wurde geklickt, was das Popup anwies, sich zu schließen.
    ''' </summary>
    ''' <remarks></remarks>
    ResetButtonClicked = 6

    ''' <summary>
    ''' Ein anderer Button des Popups in der gleichen Ebene wie der Popup-Opener/Closer-Button wurde angeklickt.
    ''' </summary>
    ''' <remarks></remarks>
    MiscPopupButtonClicked = 7

    'Infrastructure only.
    SuppressEvent = 256
End Enum
