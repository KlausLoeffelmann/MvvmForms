Imports System.Windows.Forms

Public Class IsDirtyChangedEventArgs
    Inherits EventArgs

    Sub New()
        MyBase.New()
    End Sub

    Sub New(CausingControl As Control)
        Me.CausingControl = CausingControl
    End Sub

    Public Property CausingControl As Control

End Class
