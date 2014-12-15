Public Class NullableNumValueFormatterEngine
    Inherits NullableValueFormatterEngineBase(Of Decimal)

    Private myHandleAsInteger As Boolean

    Sub New(ByVal value As Decimal?)
        MyBase.New(value)
        IsFormularAllowed = True
    End Sub

    Sub New(ByVal value As Decimal?, ByVal FormatString As String)
        MyBase.New(value, FormatString)
        IsFormularAllowed = True
    End Sub

    Sub New(ByVal value As Decimal?, ByVal FormatString As String, ByVal NullValueString As String)
        MyBase.New(value, FormatString, NullValueString)
        IsFormularAllowed = True
    End Sub

    Public Overrides Function ConvertToDisplay() As String
        If Me.Value.HasValue Then
            Dim valueAsString = Value.Value.ToString(Me.FormatString)
            If Not String.IsNullOrEmpty(CurrencySymbolString) Then
                If CurrencySymbolUpFront Then
                    valueAsString = CurrencySymbolString & valueAsString
                Else
                    valueAsString &= CurrencySymbolString
                End If
            End If
            Return valueAsString
        Else
            Return Me.NullValueString
        End If
    End Function

    Public Overrides Function ConvertToValue(ByVal value As String) As Decimal?
        If String.IsNullOrEmpty(value) Then
            Return Nothing
        End If
        If IsFormularAllowed Then
            Dim fp As New FormulaEvaluator(value)
            Dim dblDouble = CDec(fp.Result)
            If HandleAsInteger Then
                Return Decimal.Truncate(dblDouble)
            End If
            Return dblDouble
        Else
            If HandleAsInteger Then
                Return Decimal.Truncate(CDec(value))
            End If
            Return Decimal.Parse(value)
        End If
    End Function

    Public Overrides Function InitializeEditedValue() As String
        Return Me.Value.ToString
    End Function

    Public Overrides Function Validate(ByVal value As String) As Exception
        Try
            If String.IsNullOrEmpty(value) Then
                Return Nothing
            End If
            If IsFormularAllowed Then
                Dim fp As New FormulaEvaluator(value)
                Dim tmpdbl = fp.Result
            Else
                Double.Parse(value)
            End If
            Return Nothing
        Catch ex As Exception
            Return ex
        End Try
    End Function

    Public Property IsFormularAllowed As Boolean
    Public Property CurrencySymbolUpFront As Boolean
    Public Property CurrencySymbolString As String
    Public Property HandleAsInteger As Boolean
End Class
