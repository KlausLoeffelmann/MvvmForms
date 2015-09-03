Imports ActiveDevelop.IoC.Generic
Imports Windows.UI.Popups

Public Class UwpMvvmMessageBox
    Implements IMvvmMessageBox

    Public Function ShowDialog(Of t As INotifyPropertyChanged)(viewModel As t,
                                                               Optional dialogTitel As String = Nothing,
                                                               Optional buttons As MvvmMessageBoxButtons = MvvmMessageBoxButtons.OKCancel,
                                                               Optional defaultButton As MvvmMessageBoxDefaultButton = MvvmMessageBoxDefaultButton.Button2,
                                                               Optional validationCallbackAsync As Func(Of Task(Of Boolean)) = Nothing) As MvvmDialogResult Implements IMvvmMessageBox.ShowDialog
        Throw New NotImplementedException()
    End Function

    Public Async Function ShowDialogAsync(Of t As INotifyPropertyChanged)(
                                                    viewModel As t,
                                                    Optional dialogTitel As String = Nothing,
                                                    Optional buttons As MvvmMessageBoxButtons = MvvmMessageBoxButtons.OKCancel,
                                                    Optional defaultButton As MvvmMessageBoxDefaultButton = MvvmMessageBoxDefaultButton.Button2,
                                                    Optional validationCallbackAsync As Func(Of Task(Of Boolean)) = Nothing) As Task(Of MvvmDialogResult) Implements IMvvmMessageBox.ShowDialogAsync

        Dim contentDialog = UwpMvvmPlatformServiceLocator.ViewToDialogResolver(viewModel)
        contentDialog.DataContext = viewModel
        Select Case buttons
            Case MvvmMessageBoxButtons.OK
                contentDialog.PrimaryButtonText = "Ok"
                contentDialog.SecondaryButtonCommand = Nothing
            Case MvvmMessageBoxButtons.OKCancel
                contentDialog.PrimaryButtonText = "Ok"
                contentDialog.SecondaryButtonText = "Cancel"
            Case MvvmMessageBoxButtons.YesNo
                contentDialog.PrimaryButtonText = "Yes"
                contentDialog.SecondaryButtonText = "No"
            Case MvvmMessageBoxButtons.YesNoCancel
                Throw New NotSupportedException("Dialogs with 3 Buttons are not supported in UWAs!")
        End Select

        Dim dr = Await contentDialog.ShowAsync
        Select Case dr
            Case ContentDialogResult.Primary
                If contentDialog.PrimaryButtonText = "OK" Then
                    Return MvvmDialogResult.OK
                Else
                    Return MvvmDialogResult.Yes
                End If
            Case ContentDialogResult.Secondary
                If contentDialog.PrimaryButtonText = "OK" Then
                    Return MvvmDialogResult.Cancel
                Else
                    Return MvvmDialogResult.No
                End If
        End Select
        Return MvvmDialogResult.None
    End Function

    Public Function ShowMessageBox(text As String, Optional caption As String = Nothing,
                                   Optional buttons As MvvmMessageBoxButtons = MvvmMessageBoxButtons.OK,
                                   Optional defaultButton As MvvmMessageBoxDefaultButton = MvvmMessageBoxDefaultButton.Button1,
                                   Optional icon As MvvmMessageBoxIcon = MvvmMessageBoxIcon.None) As MvvmDialogResult Implements IMvvmMessageBox.ShowMessageBox
        Throw New NotImplementedException()
    End Function

    Public Async Function ShowMessageBoxAsync(text As String,
                                        Optional caption As String = Nothing,
                                        Optional buttons As MvvmMessageBoxButtons = MvvmMessageBoxButtons.OK,
                                        Optional defaultButton As MvvmMessageBoxDefaultButton = MvvmMessageBoxDefaultButton.Button1,
                                        Optional icon As MvvmMessageBoxIcon = MvvmMessageBoxIcon.None) As Task(Of MvvmDialogResult) Implements IMvvmMessageBox.ShowMessageBoxAsync

        Dim dr As MvvmDialogResult = MvvmDialogResult.None

        Dim mesDialog = New MessageDialog(text)
        If String.IsNullOrEmpty(caption) Then
            mesDialog.Title = caption
        End If

        If buttons = MvvmMessageBoxButtons.OK Or buttons = MvvmMessageBoxButtons.OKCancel Then
            mesDialog.Commands.Add(New UICommand("OK", Sub() dr = MvvmDialogResult.OK))
        End If
        If buttons = MvvmMessageBoxButtons.OKCancel Then
            mesDialog.Commands.Add(New UICommand("Cancel", Sub() dr = MvvmDialogResult.Cancel))
        End If

        If buttons = MvvmMessageBoxButtons.YesNo Or buttons = MvvmMessageBoxButtons.YesNoCancel Then
            mesDialog.Commands.Add(New UICommand("Yes", Sub() dr = MvvmDialogResult.Yes))

            mesDialog.Commands.Add(New UICommand("No", Sub() dr = MvvmDialogResult.No))
        End If

        If buttons = MvvmMessageBoxButtons.YesNoCancel Then
            mesDialog.Commands.Add(New UICommand("Cancel", Sub() dr = MvvmDialogResult.Cancel))
        End If

        Await mesDialog.ShowAsync
        Return dr
    End Function
End Class
