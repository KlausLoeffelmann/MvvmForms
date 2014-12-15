Imports System.Windows.Data

Public Class BooleanToBooleanExConverter
    Implements IValueConverter

    Private Shared preDefinedInstance As BooleanToBooleanExConverter

    Public Shared Function GetInstance() As BooleanToBooleanExConverter
        If preDefinedInstance Is Nothing Then
            preDefinedInstance = New BooleanToBooleanExConverter
        End If
        Return preDefinedInstance
    End Function

    Public Function Convert(value As Object, targetType As Type, parameter As Object, culture As Globalization.CultureInfo) As Object Implements IValueConverter.Convert
        If Not (TypeOf value Is Boolean Or TypeOf value Is Boolean?) AndAlso value IsNot Nothing Then
            Throw New TypeMismatchException("An instance of type Boolean or Boolean? was expected when using the BooleanToBooleanExConverter, but type '" & value.GetType.ToString & "' has been passed. This Converter is being used implicitely, when using NullableValueCheckBox Controls.")
        End If
        If value Is Nothing Then
            Return Nothing
        Else
            Return New BooleanEx(value)
        End If
    End Function

    Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, culture As Globalization.CultureInfo) As Object Implements IValueConverter.ConvertBack
        If Not (TypeOf value Is BooleanEx) AndAlso value IsNot Nothing Then
            Throw New TypeMismatchException("An instance of type BooleanEx was expected when using the ConvertBack method of the BooleanToBooleanExConverter, but type '" & value.GetType.ToString & "' has been provided. This converter is being used implicitely, when using NullableValueCheckBox Controls.")
        End If
        Return DirectCast(value, BooleanEx).ToBoolean
    End Function
End Class
