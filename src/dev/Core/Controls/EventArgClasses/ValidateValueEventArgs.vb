Imports System.ComponentModel

''' <summary>
''' Stellt Daten zur Verfügung, wenn das ValueValidate-Ereignis von NullableValueBase-abgeleiteten Steuerelementen ausgelöst wird.
''' </summary>
''' <typeparam name="NullableType"></typeparam>
''' <remarks></remarks>
Public Class NullableValueValidationEventArgs(Of NullableType)

    Private myValidationFailedUIMessage As String

    ''' <summary>
    ''' Erstellt eine Instanz dieser Klasse und bestimmt den Wert, den es zu validieren gilt.
    ''' </summary>
    ''' <param name="value"></param>
    ''' <remarks></remarks>
    Sub New(ByVal value As NullableType)
        MyBase.New()
        Me.Value = value
    End Sub

    ''' <summary>
    ''' Erstellt eine Instanz dieser Klasse und bestimmt den Wert, den es zu validieren gilt.
    ''' </summary>
    ''' <param name="value"></param>
    ''' <param name="validationFailedUIMessage"></param>
    ''' <remarks></remarks>
    Sub New(value As NullableType, validationFailedUIMessage As String)
        MyBase.new()
        Me.ValidationFailedUIMessage = validationFailedUIMessage
    End Sub

    ''' <summary>
    ''' Bestimmt oder ermittelt den Wert, der validiert werden soll.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Property Value As NullableType

    ''' <summary>
    ''' Bestimmt oder ermittelt einen Text, der für die Benutzeroberflächenanzeige bestimmt ist, 
    ''' und der aussagt, warum die Validierung fehlgeschlagen ist.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Property ValidationFailedUIMessage As String
        Get
            Return myValidationFailedUIMessage
        End Get
        Set(value As String)
            myValidationFailedUIMessage = value
        End Set
    End Property

End Class
