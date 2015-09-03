Public Interface IMvvmCommandLineContainer
    Property CommandLine As IMvvmCommandLine
End Interface

Public Interface IMvvmCommandLine
    Function WriteAsync(text As String) As Task
    Function WriteLineAsync(text As String) As Task
    Function ReadLineAsync() As Task(Of String)


End Interface
