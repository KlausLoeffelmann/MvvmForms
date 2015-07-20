Imports System.Windows.Data
Imports System.Windows

Public Class BooleanToBoldConverter
    Implements IValueConverter
    Public Function Convert(value As Object, targetType As Type, parameter As Object, culture As Globalization.CultureInfo) As Object Implements IValueConverter.Convert
        If DirectCast(value, Boolean) Then

            Return New System.Windows.Media.FontFamily("Segoe UI Black")
        Else
            Return New System.Windows.Media.FontFamily("Segoe UI")
        End If

    End Function

    Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, culture As Globalization.CultureInfo) As Object Implements IValueConverter.ConvertBack
        Throw New NotImplementedException("Not implemented.")
    End Function
End Class
