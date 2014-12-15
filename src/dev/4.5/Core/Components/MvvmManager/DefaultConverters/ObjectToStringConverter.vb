Imports System.Windows.Data

Public Class ObjectToStringConverter
    Implements IValueConverter

    Private Shared preDefinedInstance As ObjectToStringConverter

    Public Shared Function GetInstance() As ObjectToStringConverter
        If preDefinedInstance Is Nothing Then
            preDefinedInstance = New ObjectToStringConverter
        End If
        Return preDefinedInstance
    End Function

    Public Function Convert(value As Object, targetType As Type, parameter As Object, culture As Globalization.CultureInfo) As Object Implements IValueConverter.Convert
        If value IsNot Nothing Then
            Return value.ToString
        End If
        Return Nothing
    End Function

    Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, culture As Globalization.CultureInfo) As Object Implements IValueConverter.ConvertBack
        Return value
    End Function
End Class
