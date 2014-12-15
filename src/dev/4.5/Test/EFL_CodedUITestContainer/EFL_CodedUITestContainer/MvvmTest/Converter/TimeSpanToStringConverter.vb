Imports System.Windows.Data

Public Class TimeSpanToStringConverter
    Implements IValueConverter

    Public Function Convert(value As Object, targetType As Type, parameter As Object,
                            culture As Globalization.CultureInfo) As Object Implements IValueConverter.Convert
        Dim tSpan = DirectCast(value, TimeSpan)
        Return tSpan.ToString(parameter.ToString)
    End Function

    Public Function ConvertBack(value As Object, targetType As Type, parameter As Object,
                                culture As Globalization.CultureInfo) As Object Implements IValueConverter.ConvertBack
        Dim timeAsString As String = DirectCast(value, String)
        Try
            Return TimeSpan.Parse(timeAsString)
        Catch ex As Exception
            Return Date.Now.TimeOfDay
        End Try
    End Function
End Class