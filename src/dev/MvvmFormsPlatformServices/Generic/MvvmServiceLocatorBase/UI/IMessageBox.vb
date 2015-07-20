Public Enum MvvmMessageBoxResult
    None
    Ok
    Cancel
    Yes
    No
End Enum

Public Interface IMvvmMessageBox
    Function ShowAsync(message As String, titel As String) As Task(Of MvvmMessageBoxResult)
End Interface
