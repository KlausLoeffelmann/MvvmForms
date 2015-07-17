Public Class NullableStringValueFormatterEngine
    Inherits NullableValueFormatterEngineBase(Of StringValue)

    Sub New(ByVal value As StringValue?)
        MyBase.New(value)
    End Sub

    Sub New(ByVal value As StringValue?, ByVal FormatString As String)
        MyBase.New(value, FormatString)
    End Sub

    Sub New(ByVal value As StringValue?, ByVal FormatString As String, ByVal NullValueString As String)
        MyBase.New(value, FormatString, NullValueString)
    End Sub

    Public Overrides Function ConvertToDisplay() As String
        If Me.Value.HasValue Then
            Return Value.Value
        Else
            Return Me.NullValueString
        End If
    End Function

    Public Overrides Function ConvertToValue(ByVal value As String) As StringValue?
        If IgnoreWhiteSpace Then
            If String.IsNullOrWhiteSpace(value) Then
                Return Nothing
            Else
                Return value
            End If
        Else
            If String.IsNullOrEmpty(value) Then
                Return Nothing
            Else
                Return value
            End If
        End If
    End Function

    Public Overrides Function InitializeEditedValue() As String
        If Value.HasValue Then
            Return Value.Value
        Else
            Return ""
        End If
    End Function

    Public Overrides Function Validate(ByVal text As String) As System.Exception
        'Validieren bei einem einfachen String kann niemals in die Hose gehen.
        Return Nothing
    End Function

    ''' <summary>
    ''' Definiert, ob vorhandene WhiteSpaces ignoriert werden oder als reguläre Eingabe gelten.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>Wenn ein String nur White Spaces enthält, aber keine anderen Zeichen, würde er als 
    ''' Null/Nothing bewertet werden, ansonsten nicht.</remarks>
    Public Property IgnoreWhiteSpace As Boolean
End Class
