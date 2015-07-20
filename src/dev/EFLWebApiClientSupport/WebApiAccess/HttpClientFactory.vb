Imports System.Net.Http

''' <summary>
''' Klasse, die einen wiederverwendbaren HTTPClient anbietet.
''' </summary>
Public Class HttpClientFactory

    Private Shared mySingletonHttpClient As HttpClient

    Public Shared Function GetHttpClient(Optional options As Object = Nothing) As HttpClient
        If mySingletonHttpClient Is Nothing Then
            mySingletonHttpClient = New HttpClient()
        End If
        Return mySingletonHttpClient
    End Function

    Private Async Sub DemoAsCustomizedSerializerUsage()
        Dim client As New HttpClient
        Dim url = ""
        Dim results = Await client.GetAsync(url)
        Dim t = results.Content.ReadAsAsync(Of String)({New CustomJSonMediaTypeFormatter})
    End Sub
End Class
