Imports System.Runtime.CompilerServices

Public Module TraceEx

    Public Property IncludeDateAndTime As Boolean = True

    Public Sub TraceInformation(info As String)
        If IncludeDateAndTime Then
            info = Now.ToString("hh:mm:ss.ffff") & ": " & info
        End If
        Trace.TraceInformation(info)
    End Sub

    Public Sub TraceError(info As String)
        If IncludeDateAndTime Then
            info = Now.ToString("hh:mm:ss.ffff") & ": " & info
        End If
        Trace.TraceError(info)
    End Sub

End Module
