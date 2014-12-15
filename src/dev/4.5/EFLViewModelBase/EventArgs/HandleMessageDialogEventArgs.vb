Imports System.ComponentModel


Public Class RequestMessageDialogEventArgs
    Inherits EventArgs

    Public Sub New(messageboxText As String,
                   Optional messageBoxTitle As String = Nothing,
                   Optional messageBoxEventButtons As MvvMessageBoxEventButtons = MvvMessageBoxEventButtons.OK,
                   Optional messageBoxIcon As MvvmMessageBoxIcon = MvvmMessageBoxIcon.None,
                   Optional messageBoxDefaultButton As MvvmMessageBoxDefaultButton = MvvmMessageBoxDefaultButton.Button1)

        Me.MessageBoxText = messageboxText
        Me.MessageBoxTitle = messageBoxTitle
        Me.MessageBoxEventButtons = messageBoxEventButtons
        Me.MessageBoxIcon = messageBoxIcon
        Me.MessageBoxDefaultButton = messageBoxDefaultButton
    End Sub

    Public Property MessageBoxText As String
    Public Property MessageBoxTitle As String
    Public Property MessageBoxEventButtons As MvvMessageBoxEventButtons
    Public Property MessageBoxIcon As MvvmMessageBoxIcon
    Public Property MessageBoxReturnValue As MvvmMessageBoxReturnValue
    Public Property MessageBoxDefaultButton As MvvmMessageBoxDefaultButton

End Class

Public Enum MvvmMessageBoxDefaultButton
    Button1
    Button2
    Button3
End Enum

Public Enum MvvMessageBoxEventButtons
    OK
    OKCancel
    YesNo
    YesNoCancel
End Enum

Public Enum MvvmMessageBoxIcon
    None
    [Error]
    Information
    Warning
    [Stop]
End Enum

Public Enum MvvmMessageBoxReturnValue
    OK
    Cancel
    Yes
    No
End Enum
