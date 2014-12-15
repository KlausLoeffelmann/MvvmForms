Imports System.Net.Http.Formatting
Imports Newtonsoft.Json

Public Class CustomJSonMediaTypeFormatter
    Inherits JsonMediaTypeFormatter

    Public Overrides Function CreateJsonSerializer() As JsonSerializer
        Dim ser = MyBase.CreateJsonSerializer()
        ser.ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        Return ser
    End Function

End Class