Imports System.Windows.Forms
Imports System.Text

Public Interface INullableValueFormatterEngine
    Function ConvertToDisplay() As String
    Sub ConvertToValue(ByVal text As String)
    Function InitializeEditedValue() As String
    Function Validate(ByVal text As String) As Exception
    Property FormatString() As String
    Property NullValueString() As String
    Property Value() As Object
End Interface
