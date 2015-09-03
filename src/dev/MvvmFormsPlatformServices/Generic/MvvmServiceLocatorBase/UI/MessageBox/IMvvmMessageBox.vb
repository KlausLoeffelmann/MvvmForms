Public Interface IMvvmMessageBox

    Function ShowMessageBox(ByVal text As String,
                            Optional ByVal caption As String = Nothing,
                            Optional ByVal buttons As MvvmMessageBoxButtons = MvvmMessageBoxButtons.OK,
                            Optional ByVal defaultButton As MvvmMessageBoxDefaultButton = MvvmMessageBoxDefaultButton.Button1,
                            Optional ByVal icon As MvvmMessageBoxIcon = MvvmMessageBoxIcon.None) As MvvmDialogResult

    Function ShowMessageBoxAsync(ByVal text As String,
                            Optional ByVal caption As String = Nothing,
                            Optional ByVal buttons As MvvmMessageBoxButtons = MvvmMessageBoxButtons.OK,
                            Optional ByVal defaultButton As MvvmMessageBoxDefaultButton = MvvmMessageBoxDefaultButton.Button1,
                            Optional ByVal icon As MvvmMessageBoxIcon = MvvmMessageBoxIcon.None) As Task(Of MvvmDialogResult)

    Function ShowDialogAsync(Of t As INotifyPropertyChanged)(ByVal viewModel As t,
                            Optional ByVal dialogTitel As String = Nothing,
                            Optional buttons As MvvmMessageBoxButtons = MvvmMessageBoxButtons.OKCancel,
                            Optional defaultButton As MvvmMessageBoxDefaultButton = MvvmMessageBoxDefaultButton.Button2,
                            Optional ByVal validationCallbackAsync As Func(Of Task(Of Boolean)) = Nothing) As Task(Of MvvmDialogResult)
    Function ShowDialog(Of t As INotifyPropertyChanged)(ByVal viewModel As t,
                            Optional ByVal dialogTitel As String = Nothing,
                            Optional buttons As MvvmMessageBoxButtons = MvvmMessageBoxButtons.OKCancel,
                            Optional defaultButton As MvvmMessageBoxDefaultButton = MvvmMessageBoxDefaultButton.Button2,
                            Optional ByVal validationCallbackAsync As Func(Of Task(Of Boolean)) = Nothing) As MvvmDialogResult

End Interface
