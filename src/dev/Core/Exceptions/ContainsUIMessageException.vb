''' <summary>
''' Stellt Funktionalitäten für Ausnahme-Behandlungen bereit, die mit Fehlermeldung der Benutzeroberflächen verknüpft werden sollen.
''' </summary>
''' <remarks></remarks>
Public Class ContainsUIMessageException
    Inherits Exception

    ''' <summary>
    ''' Erstellt eine neue Instanz dieser Ausnahmeklasse und bestimmt Ausnahmetext und Hinweistext für den Anwender.
    ''' </summary>
    ''' <param name="Message">Nachricht, die die Fehlermeldung dieser Ausnahme beschreibt.</param>
    ''' <param name="UIMessage">Hinweistext für den Anwender, der diese Fehlermeldung beschreibt.</param>
    ''' <remarks></remarks>
    Sub New(ByVal Message As String, ByVal UIMessage As String)
        MyBase.New(Message)
        Me.UIMessage = UIMessage
    End Sub

    ''' <summary>
    ''' Erstellt eine neue Instanz dieser Ausnahmeklasse und bestimmt Ausnahmetext, Hinweistext für den Anwender und eine innere Ausnahme, die zu dieser Ausnahme geführt hat.
    ''' </summary>
    ''' <param name="Message">Nachricht, die die Fehlermeldung dieser Ausnahme beschreibt.</param>
    ''' <param name="UIMessage">Hinweistext für den Anwender, der diese Fehlermeldung beschreibt.</param>
    ''' <param name="innerException">Innere Ausnahme, die zu dieser Ausnahme geführt hat.</param>
    ''' <remarks></remarks>
    Sub New(ByVal Message As String, ByVal UIMessage As String, ByVal innerException As Exception)
        MyBase.New(Message, innerException)
        Me.UIMessage = UIMessage
    End Sub

    ''' <summary>
    ''' Für den Benutzer bzw. das Front-End aufbereiteter Hinweistext beim Auftreten dieser Ausnahme.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property UIMessage As String

    ''' <summary>
    ''' Kurzname der Ausnahme, um ihn beispielsweise darunter als eindeutigen Namen in einer Datenquelle zur Sprach-Lokalisierung nachzuschlagen.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property MessageShortName As String

    ''' <summary>
    ''' Guid dieser Instanz, um ihn beispielsweise darunter in einer Datenquelle zur Sprach-Lokalisierung nachzuschlagen.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property MessageGuid As Guid
End Class
