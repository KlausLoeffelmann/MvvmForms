Imports System.Windows.Data

Public Class UmsatzToFarbeConverter
    Implements IValueConverter
    Public Function Convert(value As Object, targetType As Type, parameter As Object, culture As Globalization.CultureInfo) As Object Implements IValueConverter.Convert
        If DirectCast(value, Double) < 0 Then
            Return System.Windows.Media.Colors.Red
        Else
            Return System.Windows.Media.Colors.Green
        End If

    End Function

    Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, culture As Globalization.CultureInfo) As Object Implements IValueConverter.ConvertBack
        Throw New NotImplementedException("Not implemented.")
    End Function
End Class