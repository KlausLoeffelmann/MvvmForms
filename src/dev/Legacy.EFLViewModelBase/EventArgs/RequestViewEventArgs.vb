Public Class RequestViewEventArgs
    Inherits EventArgs

    Public Sub New(viewModel As IMvvmViewModel)
        Me.ViewModel = viewModel
    End Sub

    Public Property ViewModel As IMvvmViewModel
    Public Property DialogResult As MvvmMessageBoxReturnValue

End Class
