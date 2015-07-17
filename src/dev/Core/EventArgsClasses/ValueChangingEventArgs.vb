''' <summary>
''' Parameter, mit denen Informationen zum ValueChanging-Ereignis übermittelt und zurückgegeben werden.
''' </summary>
''' <typeparam name="NullableType"></typeparam>
''' <remarks></remarks>
Public Class ValueChangingEventArgs(Of NullableType)
    Inherits EventArgs

    Sub New(ByVal originalValue As NullableType)
        Me.OriginalValue = originalValue
        Me.NewValue = originalValue
    End Sub

    ''' <summary>
    ''' Der Wert, der zum neuen Wert für die Value-Eigenschaft erklärt werden soll.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Property OriginalValue As NullableType

    ''' <summary>
    ''' Der Wert, der abweichend vom eingegebenen Wert für die Value-Eigenschaft gelten soll.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>Mit NewValue kann der Wert, der eigentlich vom Anwender eingegeben wurde, durch einen frei wählbaren ersetzt werden.</remarks>
    Property NewValue As NullableType
End Class

''' <summary>
''' Wird ausgelöst, wenn zwei Typen unterschiedlich sind, es aber nicht sein dürfen.
''' </summary>
''' <remarks></remarks>
Public Class TypeMismatchException
    Inherits SystemException

    Sub New()
        MyBase.New()
    End Sub

    Sub New(ByVal Message As String)
        MyBase.New(Message)
    End Sub

    Sub New(ByVal Message As String, ByVal innerException As Exception)
        MyBase.New(Message, innerException)
    End Sub
End Class

