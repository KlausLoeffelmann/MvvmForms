Imports System.Net.Http
Imports System.Runtime.CompilerServices
Imports System.Threading.Tasks

''' <summary>
''' WARNING! Prototype - use at own risk! Provides simplified access to Web Api-Functions.
''' </summary>
Public Class WebApiAccess

    ''' <summary>
    ''' Creates a new instance of this class and provides the Base URL for the Web Api. 
    ''' The default prefix is automatically set to '/api'.
    ''' </summary>
    ''' <param name="baseAddress"></param>
    Sub New(baseAddress As String)
        Me.BaseAddress = baseAddress
    End Sub

    ''' <summary>
    ''' Creates a new instance of this class and provides the Base URL for the Web Api, and allows 
    ''' to specify a custom API prefix.
    ''' </summary>
    Sub New(baseAddress As String, apiPreFix As String)
        Me.BaseAddress = baseAddress
        Me.ApiPreFix = apiPreFix
    End Sub

    Public Async Function CallMethodAsync(methodUrl As String) As Task
        'Wir könnten an dieser Stelle auch New HttpClient verwenden, aber wiederverwenden ist effektiver!
        'We could do 'New HttpClient' at this point, but reusage of the HttpClient is more effective.
        Dim client = HttpClientFactory.GetHttpClient

#If DEBUG Then
        Dim sw = Stopwatch.StartNew
#End If

        'URL setzte sich aus Typnamen, Key und optionalen Parametern zusammen.
        'URL composes from Typename, Key and optional Parameters.
        Dim url = Me.BaseAddress & Me.ApiPreFix & "/" & methodUrl

        'Web API aufrufen!
        'Let's call the Web API!
        Dim results = Await client.PostAsync(Of String)(url, "", New CustomJSonMediaTypeFormatter)

#If DEBUG Then
        sw.Stop()
        Debug.WriteLine("WEBAPIACCESS: Calling method in" &
                            sw.ElapsedMilliseconds.ToString("#,###") & " ms.")
#End If


        Dim returnString As String
        Try
            returnString = Await results.Content.ReadAsStringAsync
#If DEBUG Then
            Debug.WriteLine("WEBAPIACCESS: Result Calling Method:" & returnString)
#End If

        Catch ex As Exception

#If DEBUG Then
            Debug.WriteLine("WEBAPIACCESS: ERROR READING METHOD RETURN STATUS:" & ex.Message)
#Else
                Throw ex
#End If

        End Try
    End Function

    Public Async Function CreateAsync(Of t)(data As t, Optional params As String = "") As Task(Of t)
        'Wir könnten an dieser Stelle auch New HttpClient verwenden, aber wiederverwenden ist effektiver!
        'We could do 'New HttpClient' at this point, but reusage of the HttpClient is more effective.
        Dim client = HttpClientFactory.GetHttpClient

#If DEBUG Then
        Dim sw = Stopwatch.StartNew
#End If

        'URL setzte sich aus Typnamen, Key und optionalen Parametern zusammen.
        'URL composes from Typename, Key and optional Parameters.
        Dim url = Me.BaseAddress & Me.ApiPreFix & "/" & GetType(t).Name &
            If(String.IsNullOrEmpty(params), "", "/" & params)

        'Web API aufrufen!
        'Let's call the Web API!
        Dim results = Await client.PostAsync(Of t)(url, data, New CustomJSonMediaTypeFormatter())

        Dim returnValue As t = Nothing
        Dim caughtException As Boolean

        Try
            returnValue = Await results.Content.ReadAsAsync(Of t)

#If DEBUG Then
            sw.Stop()
            Debug.WriteLine("WEBAPIACCESS: posting (creating) content in" &
                            sw.ElapsedMilliseconds.ToString("#,###") & " ms.")
#End If

        Catch ex As Exception
            caughtException = True
        End Try

        If caughtException Then
            Try
                Dim returnString = Await results.Content.ReadAsStringAsync

#If DEBUG Then
                Debug.WriteLine("WEBAPIACCESS: ERROR READING RETURN KEY:" & returnString)
#End If

            Catch ex As Exception

#If DEBUG Then
                Debug.WriteLine("WEBAPIACCESS: ERROR READING KEY AND ERROR MESSAGE:" & ex.Message)
#Else
                Throw ex
#End If

            End Try
        End If
        Return returnValue
    End Function

    Public Async Function UpdateAsync(Of t)(data As t, Optional params As String = "") As Task(Of t)
        'Wir könnten an dieser Stelle auch New HttpClient verwenden, aber wiederverwenden ist effektiver!
        'We could do 'New HttpClient' at this point, but reusage of the HttpClient is more effective.
        Dim client = HttpClientFactory.GetHttpClient

#If DEBUG Then
        Dim sw = Stopwatch.StartNew
#End If

        'URL setzte sich aus Typnamen, Key und optionalen Parametern zusammen.
        'URL composes from Typename, Key and optional Parameters.
        Dim url = Me.BaseAddress & Me.ApiPreFix & "/" & GetType(t).Name &
            If(String.IsNullOrEmpty(params), "", "/" & params)

        'Web API aufrufen!
        'Let's call the Web API!
        Dim results = Await client.PutAsync(Of t)(url, data, New CustomJSonMediaTypeFormatter())

        Dim returnValue As t = Nothing
        Dim caughtException As Boolean

        Try
            returnValue = Await results.Content.ReadAsAsync(Of t)

#If DEBUG Then
            sw.Stop()
            Debug.WriteLine("WEBAPIACCESS: putting (updating or creating) content in" &
                            sw.ElapsedMilliseconds.ToString("#,###") & " ms.")
#End If

        Catch ex As Exception
            caughtException = True
        End Try

        If caughtException Then
            Try
                Dim returnString = Await results.Content.ReadAsStringAsync

#If DEBUG Then
                Debug.WriteLine("WEBAPIACCESS: ERROR READING RETURN DATA:" & returnString)
#End If

            Catch ex As Exception

#If DEBUG Then
                Debug.WriteLine("WEBAPIACCESS: ERROR READING DATA AND ERROR MESSAGE:" & ex.Message)
#Else
                Throw ex
#End If

            End Try
        End If
        Return returnValue
    End Function

    Public Async Function GetDataAsync(Of t)(<CallerMemberName> Optional method As String = "Get",
                                             Optional category As String = Nothing,
                                             Optional params As String = "") As Task(Of t)
        'Wir könnten an dieser Stelle auch New HttpClient verwenden, aber wiederverwenden ist effektiver!
        'We could do 'New HttpClient' at this point, but reusage of the HttpClient is more effective.
        Dim client = HttpClientFactory.GetHttpClient

        'Hack: that's not safe enough. Only if method's name ends with it, remove it.
        method = method.ToLower.Replace("async", "")

#If DEBUG Then
        Dim sw = Stopwatch.StartNew
#End If

        'URL setzte sich aus Typnamen, Key und optionalen Parametern zusammen.
        'URL composes from Typename, Key and optional Parameters.
        Dim url = Me.BaseAddress & Me.ApiPreFix &
                  If(String.IsNullOrEmpty(category), "", "/" & category) & "/" & method &
                  If(String.IsNullOrEmpty(params), "", "/" & params)

        'Web API aufrufen!
        'Let's call the Web API!
        Dim results = Await client.GetAsync(url)

        Dim returnValue As t = Nothing
        Dim caughtException As Boolean

        Try
            returnValue = Await results.Content.ReadAsAsync(Of t)

#If DEBUG Then
            sw.Stop()
            Debug.WriteLine("WEBAPIACCESS: Reading content in" &
                            sw.ElapsedMilliseconds.ToString("#,###") & " ms.")
#End If

        Catch ex As Exception
            caughtException = True

        End Try

        If caughtException Then
            Try
                Dim returnString = Await results.Content.ReadAsStringAsync

#If DEBUG Then
                Debug.WriteLine("WEBAPIACCESS: ERROR READING TYPE:" & returnString)
#End If

            Catch ex As Exception

#If DEBUG Then
                Debug.WriteLine("WEBAPIACCESS: ERROR READING TYPE AND ERROR MESSAGE:" & ex.Message)
#Else
                Throw ex
#End If

            End Try
        End If
        Return returnValue
    End Function

    Public Async Function GetDataAsync(Of t, keyType)(key As keyType, Optional params As String = "") As Task(Of t)
        'Wir könnten an dieser Stelle auch New HttpClient verwenden, aber wiederverwenden ist effektiver!
        'We could do 'New HttpClient' at this point, but reusage of the HttpClient is more effective.
        Dim client = HttpClientFactory.GetHttpClient

#If DEBUG Then
        Dim sw = Stopwatch.StartNew
#End If

        'URL setzte sich aus Typnamen, Key und optionalen Parametern zusammen.
        'URL composes from Typename, Key and optional Parameters.
        Dim url = Me.BaseAddress & Me.ApiPreFix & "/" & GetType(t).Name & "/" &
            key.ToString & If(String.IsNullOrEmpty(params), "", "/" & params)

        'Web API aufrufen!
        'Let's call the Web API!
        Dim results = Await client.GetAsync(url)

        Dim returnValue As t = Nothing
        Dim caughtException As Boolean

        Try
            returnValue = Await results.Content.ReadAsAsync(Of t)

#If DEBUG Then
            sw.Stop()
            Debug.WriteLine("WEBAPIACCESS: Reading content in" &
                            sw.ElapsedMilliseconds.ToString("#,###") & " ms.")
#End If

        Catch ex As Exception
            caughtException = True
        End Try

        If caughtException Then
            Try
                Dim returnString = Await results.Content.ReadAsStringAsync

#If DEBUG Then
                Debug.WriteLine("WEBAPIACCESS: ERROR READING TYPE:" & returnString)
#End If

            Catch ex As Exception

#If DEBUG Then
                Debug.WriteLine("WEBAPIACCESS: ERROR READING TYPE AND ERROR MESSAGE:" & ex.Message)
#Else
                Throw ex
#End If

            End Try
        End If
        Return returnValue

    End Function

    Public Async Function GetCollectionAsync(Of t)(Optional params As String = "") As Task(Of IEnumerable(Of t))

        'Wir könnten an dieser Stelle auch New HttpClient verwenden, aber wiederverwenden ist effektiver!
        'We could do 'New HttpClient' at this point, but reusage of the HttpClient is more effective.
        Dim client = HttpClientFactory.GetHttpClient

#If DEBUG Then
        Dim sw = Stopwatch.StartNew
#End If

        'URL setzte sich aus Typnamen, Key und optionalen Parametern zusammen.
        'URL composes from Typename, Key and optional Parameters.
        Dim url = Me.BaseAddress & Me.ApiPreFix & "/" & GetType(t).Name & If(String.IsNullOrEmpty(params), "", "/" & params)
        Dim results = Await client.GetAsync(url)

        Dim returnValue As List(Of t) = Nothing
        Dim caughtException As Boolean

        Try
            returnValue = Await results.Content.ReadAsAsync(Of List(Of t))

#If DEBUG Then
            sw.Stop()
            Debug.WriteLine("WEBAPIACCESS: Reading " & url & ", " & returnValue.Count & " Elements, in " &
                            sw.ElapsedMilliseconds.ToString("#,###") & " ms.")
#End If

        Catch ex As Exception
            caughtException = True
        End Try

        If caughtException Then
            Try
                Dim returnString = Await results.Content.ReadAsStringAsync
#If DEBUG Then
                Debug.WriteLine("WEBAPIACCESS: ERROR READING COLLECTION:" & returnString)
#End If
            Catch ex As Exception
#If DEBUG Then
                Debug.WriteLine("WEBAPIACCESS: ERROR READING COLLECTION AND ERROR MESSAGE:" & ex.Message)
#Else
                Throw ex
#End If
            End Try
        End If
        Return returnValue
    End Function

    Public Property BaseAddress As String = ""
    Public Property ApiPreFix As String = "/api"


End Class
