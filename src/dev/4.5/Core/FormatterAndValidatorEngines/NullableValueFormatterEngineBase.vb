Imports System.Windows.Forms
Imports System.Text

Public MustInherit Class NullableValueFormatterEngineBase(Of NullableValue As Structure)
    Implements INullableValueFormatterEngine

    Private myValue As NullableValue?
    Private myFormatString As String
    Private myNullValueString As String

    Public Sub New(ByVal Value As NullableValue?)
        myValue = Value
    End Sub

    Public Sub New(ByVal value As NullableValue?, ByVal FormatString As String)
        myFormatString = FormatString
        myValue = value
    End Sub

    Public Sub New(ByVal value As NullableValue?, ByVal FormatString As String, ByVal NullValueString As String)
        myFormatString = FormatString
        myNullValueString = NullValueString
        myValue = value
    End Sub

    Public MustOverride Function ConvertToDisplay() As String Implements INullableValueFormatterEngine.ConvertToDisplay

    Private Sub ConvertToValueInternal(ByVal text As String) Implements INullableValueFormatterEngine.ConvertToValue
        Me.Value = ConvertToValue(text)
    End Sub

    Public MustOverride Function ConvertToValue(ByVal value As String) As NullableValue?
    Public MustOverride Function Validate(ByVal text As String) As Exception Implements INullableValueFormatterEngine.Validate
    Public MustOverride Function InitializeEditedValue() As String Implements INullableValueFormatterEngine.InitializeEditedValue

    Public Property FormatString() As String Implements INullableValueFormatterEngine.FormatString
        Get
            Return myFormatString
        End Get
        Set(ByVal value As String)
            myFormatString = value
        End Set
    End Property

    Public Property NullValueString() As String Implements INullableValueFormatterEngine.NullValueString
        Get
            Return myNullValueString
        End Get
        Set(ByVal value As String)
            myNullValueString = value
        End Set
    End Property

    Private Property ValueInternal() As Object Implements INullableValueFormatterEngine.Value
        Get
            Return Me.Value
        End Get
        Set(ByVal value As Object)
            'TODO: Case testing!!!
            Me.Value = CType(CTypeDynamic(value, GetType(NullableValue?)), NullableValue?)
        End Set
    End Property

    Public Property Value() As NullableValue?
        Get
            Return myValue
        End Get
        Set(ByVal value As NullableValue?)
            myValue = value
        End Set
    End Property
End Class
