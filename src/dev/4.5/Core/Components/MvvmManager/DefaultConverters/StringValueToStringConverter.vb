Imports System.Windows.Data

Public Class StringValueToStringConverter
    Implements IValueConverter

    Private Shared preDefinedInstance As StringValueToStringConverter

    Public Shared Function GetInstance() As StringValueToStringConverter
        If preDefinedInstance Is Nothing Then
            preDefinedInstance = New StringValueToStringConverter
        End If
        Return preDefinedInstance
    End Function

    Public Function Convert(value As Object, targetType As Type, parameter As Object, culture As Globalization.CultureInfo) As Object Implements IValueConverter.Convert
        If Not (TypeOf value Is String) AndAlso value IsNot Nothing Then
            Throw New TypeMismatchException("An instance of type String was expected when using the StringToStringValueConverter, but type '" & value.GetType.ToString & "' has been passed. This Converter is being used implicitely, when using NullableTextValue Controls.")
        End If
        If value Is Nothing Then
            Return Nothing
        Else
            Return New StringValue(DirectCast(value, String))
        End If
    End Function

    Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, culture As Globalization.CultureInfo) As Object Implements IValueConverter.ConvertBack
        If Not (TypeOf value Is StringValue) AndAlso value IsNot Nothing Then
            Throw New TypeMismatchException("An instance of type StringValue was expected when using the ConvertBack method of the StringToStringValueConverter, but type '" & value.GetType.ToString & "' has been passed. This Converter is being used implicitely, when using NullableTextValue Controls.")
        End If
        Return value.ToString
    End Function
End Class

