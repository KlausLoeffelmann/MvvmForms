Imports ActiveDevelop.IoC.Generic
Imports Windows.UI.Popups

Public Class MvvmMessageBox
    Implements IMvvmMessageBox

    Public Async Function ShowAsync(message As String,
                                    titel As String) As Task(Of MvvmMessageBoxResult) Implements IMvvmMessageBox.ShowAsync
        Dim md As New MessageDialog(message, titel)
        Dim result = MvvmMessageBoxResult.None
        md.Commands.Add(New UICommand("OK", New UICommandInvokedHandler(Sub() result = MvvmMessageBoxResult.Ok)))
        md.Commands.Add(New UICommand("Cancel", New UICommandInvokedHandler(Sub() result = MvvmMessageBoxResult.Cancel)))
        Await md.ShowAsync()
        Return result
    End Function
End Class

