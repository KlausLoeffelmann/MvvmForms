Imports System.ComponentModel
Imports System.Windows.Forms
Imports ActiveDevelop.IoC.Generic


Public Class WinFormsMvvmMessageBox
    Implements IMvvmMessageBox

    Public Function ShowDialog(Of t As INotifyPropertyChanged)(viewModel As t,
                                                               Optional dialogTitel As String = Nothing,
                                                               Optional buttons As MvvmMessageBoxButtons = MvvmMessageBoxButtons.OKCancel,
                                                               Optional defaultButton As MvvmMessageBoxDefaultButton = MvvmMessageBoxDefaultButton.Button2,
                                                               Optional validationCallbackAsync As Func(Of Task(Of Boolean)) = Nothing) As MvvmDialogResult Implements IMvvmMessageBox.ShowDialog

        Dim modelForm = WinFormsMvvmPlatformServiceLocator.ViewToDialogResolver(viewModel)
        Dim winFormsDr = modelForm.ShowDialog()

        If Not String.IsNullOrEmpty(dialogTitel) Then
            modelForm.Text = dialogTitel
        End If

        Dim dialogResult As MvvmDialogResult

        Select Case winFormsDr
            Case System.Windows.Forms.DialogResult.Abort
                dialogResult = MvvmDialogResult.Abort
            Case System.Windows.Forms.DialogResult.Cancel
                dialogResult = MvvmDialogResult.Cancel
            Case System.Windows.Forms.DialogResult.Ignore
                dialogResult = MvvmDialogResult.Ignore
            Case System.Windows.Forms.DialogResult.No
                dialogResult = MvvmDialogResult.No
            Case System.Windows.Forms.DialogResult.None
                dialogResult = MvvmDialogResult.None
            Case System.Windows.Forms.DialogResult.OK
                dialogResult = MvvmDialogResult.OK
            Case System.Windows.Forms.DialogResult.Retry
                dialogResult = MvvmDialogResult.Retry
            Case System.Windows.Forms.DialogResult.Yes
                dialogResult = MvvmDialogResult.Yes
        End Select

        Return winFormsDr

    End Function

    Public Async Function ShowDialogAsync(Of t As INotifyPropertyChanged)(
                                                    viewModel As t,
                                                    Optional dialogTitel As String = Nothing,
                                                    Optional buttons As MvvmMessageBoxButtons = MvvmMessageBoxButtons.OKCancel,
                                                    Optional defaultButton As MvvmMessageBoxDefaultButton = MvvmMessageBoxDefaultButton.Button2,
                                                    Optional validationCallbackAsync As Func(Of Task(Of Boolean)) = Nothing) As Task(Of MvvmDialogResult) Implements IMvvmMessageBox.ShowDialogAsync

        Return Await Task.FromResult(ShowDialog(viewModel,
                                                dialogTitel,
                                                buttons,
                                                defaultButton,
                                                validationCallbackAsync))

    End Function

    Public Function ShowMessageBox(text As String, Optional caption As String = Nothing,
                                   Optional buttons As MvvmMessageBoxButtons = MvvmMessageBoxButtons.OK,
                                   Optional defaultButton As MvvmMessageBoxDefaultButton = MvvmMessageBoxDefaultButton.Button1,
                                   Optional icon As MvvmMessageBoxIcon = MvvmMessageBoxIcon.None) As MvvmDialogResult Implements IMvvmMessageBox.ShowMessageBox

        Dim winFormsMsgButtons As MessageBoxButtons
        Select Case buttons
            Case MvvmMessageBoxButtons.OK
                winFormsMsgButtons = MessageBoxButtons.OK
            Case MvvmMessageBoxButtons.OKCancel
                winFormsMsgButtons = MessageBoxButtons.OKCancel
            Case MvvmMessageBoxButtons.YesNo
                winFormsMsgButtons = MessageBoxButtons.YesNo
            Case MvvmMessageBoxButtons.YesNoCancel
                winFormsMsgButtons = MessageBoxButtons.YesNoCancel
            Case Else
                Throw New NotSupportedException("MvvmForms does not support any other MessageBoxButtons combination.")
        End Select

        Dim winFormsMsgIcon As MessageBoxIcon

        Select Case icon
            Case MvvmMessageBoxIcon.Error
                winFormsMsgIcon = MessageBoxIcon.Error
            Case MvvmMessageBoxIcon.Information
                winFormsMsgIcon = MessageBoxIcon.Information
            Case MvvmMessageBoxIcon.None
                winFormsMsgIcon = MessageBoxIcon.None
            Case MvvmMessageBoxIcon.Warning
                winFormsMsgIcon = MessageBoxIcon.Warning
            Case Else
                Throw New NotSupportedException("MvvmForms does not support any other MessageBoxIcon combination.")
        End Select

        Dim winFormsDefaultButton As MessageBoxDefaultButton

        Select Case defaultButton
            Case MvvmMessageBoxDefaultButton.Button1
                defaultButton = MvvmMessageBoxDefaultButton.Button1
            Case MvvmMessageBoxDefaultButton.Button2
                defaultButton = MvvmMessageBoxDefaultButton.Button2
            Case MvvmMessageBoxDefaultButton.Button3
                defaultButton = MvvmMessageBoxDefaultButton.Button3
        End Select

        Dim winFormsDr = MessageBox.Show(text, caption, winFormsMsgButtons, winFormsMsgIcon, winFormsDefaultButton)

        Dim dialogResult As MvvmDialogResult

        Select Case winFormsDr
            Case System.Windows.Forms.DialogResult.Abort
                dialogResult = MvvmDialogResult.Abort
            Case System.Windows.Forms.DialogResult.Cancel
                dialogResult = MvvmDialogResult.Cancel
            Case System.Windows.Forms.DialogResult.Ignore
                dialogResult = MvvmDialogResult.Ignore
            Case System.Windows.Forms.DialogResult.No
                dialogResult = MvvmDialogResult.No
            Case System.Windows.Forms.DialogResult.None
                dialogResult = MvvmDialogResult.None
            Case System.Windows.Forms.DialogResult.OK
                dialogResult = MvvmDialogResult.OK
            Case System.Windows.Forms.DialogResult.Retry
                dialogResult = MvvmDialogResult.Retry
            Case System.Windows.Forms.DialogResult.Yes
                dialogResult = MvvmDialogResult.Yes
        End Select

        Return winFormsDr

    End Function

    Public Async Function ShowMessageBoxAsync(text As String,
                                        Optional caption As String = Nothing,
                                        Optional buttons As MvvmMessageBoxButtons = MvvmMessageBoxButtons.OK,
                                        Optional defaultButton As MvvmMessageBoxDefaultButton = MvvmMessageBoxDefaultButton.Button1,
                                        Optional icon As MvvmMessageBoxIcon = MvvmMessageBoxIcon.None) As Task(Of MvvmDialogResult) Implements IMvvmMessageBox.ShowMessageBoxAsync

        Return Await Task.FromResult(ShowMessageBox(text, caption,
                                    buttons, defaultButton, icon))

    End Function
End Class
