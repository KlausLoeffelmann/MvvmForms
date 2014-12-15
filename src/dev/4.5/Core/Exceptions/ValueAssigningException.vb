''' <summary>
''' Ausnahme, die ausgelöst wird, wenn bei einem NullableValue-Steuerelement versucht wird, einen nicht gültiger Wert zuzuweisen.
''' </summary>
''' <remarks></remarks>
Public Class UnassignableValueException
    Inherits ArgumentOutOfRangeException

    ''' <summary>
    ''' Erstellt eine Instanz dieser Klasse und bestimmt Exception-Text und das Steuerelement, das die Ausnahme ausgelöst hat.
    ''' </summary>
    ''' <param name="param"></param>
    ''' <param name="message"></param>
    ''' <param name="causingControl"></param>
    ''' <remarks></remarks>
    Sub New(param As String, message As String, causingControl As INullableValueDataBinding)
        MyBase.New(param, message)
        Me.CausingControl = causingControl
    End Sub

    ''' <summary>
    ''' Erstellt eine Instanz dieser Klasse und bestimmt Exception-Text und das Steuerelement, das die Ausnahme ausgelöst hat.
    ''' </summary>
    ''' <param name="param"></param>
    ''' <param name="message"></param>
    ''' <param name="causingControl"></param>
    ''' <remarks></remarks>
    Sub New(param As String, actualValue As Object, message As String, causingControl As INullableValueDataBinding)
        MyBase.New(param, actualValue, message)
        Me.CausingControl = causingControl
    End Sub

    ''' <summary>
    ''' Erstellt eine Instanz dieser Klasse und bestimmt Exception-Text und das Steuerelement, das die Ausnahme ausgelöst hat.
    ''' </summary>
    ''' <param name="message">Fehlertext.</param>
    ''' <param name="causingControl">Steuerelement, das die Ausnahme ausgelöst hat.</param>
    ''' <remarks></remarks>
    Sub New(message As String, causingControl As INullableValueDataBinding)
        MyBase.New(message)
        Me.CausingControl = causingControl
    End Sub

    ''' <summary>
    ''' Erstellt eine Instanz dieser Klasse und bestimmt Exception-Text, innerException und das Steuerelement, das die Ausnahme ausgelöst hat.
    ''' </summary>
    ''' <param name="message">Fehlertext.</param>
    ''' <param name="innerException">Die Ausnahme, die zu dieser Ausnahme ursächlich geführt hat.</param>
    ''' <param name="causingControl">Steuerelement, das die Ausnahme ausgelöst hat.</param>
    ''' <remarks></remarks>
    Sub New(message As String, innerException As Exception, causingControl As INullableValueDataBinding)
        MyBase.New(message, innerException)
        Me.CausingControl = causingControl
    End Sub

    ''' <summary>
    ''' Bestimmt oder ermittelt das Steuerelement, das diese Ausnahme ausgelöst hat.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Property CausingControl As INullableValueDataBinding
End Class
