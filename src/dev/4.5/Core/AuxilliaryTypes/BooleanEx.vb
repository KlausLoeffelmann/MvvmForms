''' <summary>
''' Stellt einen Boolean-Datentypen mit erweiterten Konvertierungsmöglichkeiten in andere und von anderen Datentypen zur Verfügung.
''' </summary>
''' <remarks></remarks>
Public Structure BooleanEx

    Private myValue As Boolean
    Private d() As Integer

    Sub New(ByVal value As Object)
        If value Is Nothing Then
            myValue = False
            Return
        End If

        Select Case value.GetType()

            Case GetType(BooleanEx)
                myValue = DirectCast(value, BooleanEx).myValue

            Case GetType(Boolean)
                myValue = CBool(value)

            Case GetType(Byte), GetType(Integer), GetType(Short),
                GetType(Double), GetType(Single), GetType(Decimal)
                myValue = 1 = CInt(value)

            Case GetType(String)
                Dim tmpValue = DirectCast(value, String)
                myValue = FromStringInternal(tmpValue)
        End Select

    End Sub

    Sub New(ByVal value As Boolean)
        myValue = value
    End Sub

    Sub New(ByVal value As Double)
        myValue = 1 = value
    End Sub

    Sub New(ByVal value As Single)
        myValue = 1 = value
    End Sub

    Sub New(ByVal value As Decimal)
        myValue = 1 = value
    End Sub

    Sub New(ByVal value As Long)
        myValue = 1 = value
    End Sub

    Sub New(ByVal value As Integer)
        myValue = 1 = value
    End Sub

    Sub New(ByVal value As Short)
        myValue = 1 = value
    End Sub

    Sub New(ByVal value As Byte)
        myValue = 1 = value
    End Sub

    Sub New(ByVal value As String)
        myValue = FromStringInternal(value)
    End Sub

    Private Function FromStringInternal(ByVal value As String) As Boolean
        If String.IsNullOrWhiteSpace(value) Then
            Return False
        End If

        value = value.ToUpper
        Select Case value

            Case "TRUE", "WAHR", "T", "W", "YES", "Y", "JA", "J", "1", "-1", "+"
                Return True
            Case Else
                Return False
        End Select
    End Function

    Public Overrides Function ToString() As String
        Return myValue.ToString
    End Function

    Public Function ToInt32() As Integer
        Return If(myValue, 1, 0)
    End Function

    Public Function ToInt16() As Integer
        Return If(myValue, 1, 0)
    End Function

    Public Function ToIntByte() As Integer
        Return If(myValue, 1, 0)
    End Function

    Public Function ToDouble() As Double
        Return If(myValue, 1, 0)
    End Function

    Public Function ToSingle() As Single
        Return If(myValue, 1, 0)
    End Function

    Public Function ToDecimal() As Decimal
        Return If(myValue, 1, 0)
    End Function

    Public Function ToBoolean() As Boolean
        Return myValue
    End Function

    Public Shared Widening Operator CType(ByVal value As Boolean) As BooleanEx
        Return New BooleanEx(value)
    End Operator

    Public Shared Widening Operator CType(ByVal value As BooleanEx) As Boolean
        Return value.myValue
    End Operator

    Public Shared Widening Operator CType(ByVal value As Integer) As BooleanEx
        Return New BooleanEx(value)
    End Operator

    Public Shared Widening Operator CType(ByVal value As BooleanEx) As Integer
        Return value.ToInt32
    End Operator

    Public Shared Widening Operator CType(ByVal value As String) As BooleanEx
        Return New BooleanEx(value)
    End Operator

    Public Shared Widening Operator CType(ByVal value As BooleanEx) As String
        Return value.ToString
    End Operator

    Public Shared Operator =(value1 As BooleanEx, value2 As Boolean) As Boolean
        Return value1.myValue = value2
    End Operator

    Public Shared Operator <>(value1 As BooleanEx, value2 As Boolean) As Boolean
        Return value1.myValue <> value2
    End Operator

End Structure
