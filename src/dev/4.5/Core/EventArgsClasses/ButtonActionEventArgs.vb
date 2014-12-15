Public Class ButtonActionEventArgs
    Inherits EventArgs

    Sub New(ByVal buttonType As UpDownComboButtonIDs)
        Me.ButtonType = buttonType
    End Sub

    Public Property ButtonType As UpDownComboButtonIDs

End Class

