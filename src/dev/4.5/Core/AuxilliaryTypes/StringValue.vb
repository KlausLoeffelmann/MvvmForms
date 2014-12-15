Imports System.Windows.Forms
Imports System.Text

''' <summary>
''' String, der als ValueType ausgelegt ist, um der Verarbeitung als Nullable zu genügen.
''' </summary>
''' <remarks></remarks>
Public Structure StringValue
    Implements IComparable

    Private myValue As String

    Public Sub New(ByVal value As String)
            myValue = value
    End Sub

    Public Property Value() As String
        Get
            If myValue Is Nothing Then
                Return ""
            End If
            Return myValue.ToString
        End Get
        Set(ByVal value As String)
            myValue = value
        End Set
    End Property

    Public ReadOnly Property IsEmpty() As Boolean
        Get
            If Value.Length = 0 Then
                Return True
            End If
            Return False
        End Get
    End Property

    Public Overrides Function ToString() As String
        Return Value
    End Function

    Public Shared Operator +(ByVal value1 As StringValue, ByVal value2 As String) As StringValue
        Return New StringValue(value1.Value & value2)
    End Operator

    Public Shared Operator +(ByVal value1 As StringValue, ByVal value2 As StringValue) As StringValue
        Return New StringValue(value1.Value & value2.Value)
    End Operator

    Public Shared Widening Operator CType(ByVal value1 As String) As StringValue
        Return New StringValue(value1)
    End Operator

    Public Shared Widening Operator CType(ByVal value As StringValue) As String
        Return value.Value
    End Operator

    Public Function CompareTo(ByVal obj As Object) As Integer Implements System.IComparable.CompareTo
        If obj Is Nothing AndAlso myValue Is Nothing Then Return 0
        If obj Is Nothing Then Throw New NullReferenceException("The object to compare with must not be null!")
        Return Value.CompareTo(obj.ToString)
    End Function


End Structure