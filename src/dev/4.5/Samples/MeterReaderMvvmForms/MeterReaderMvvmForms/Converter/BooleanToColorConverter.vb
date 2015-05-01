Imports System.Globalization
Imports System.Windows.Data
Imports WpfColor = System.Windows.Media.Colors

Public Class BooleanToColorConverter
    Implements IValueConverter

    Public Function Convert(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.Convert
        Dim valueAsBoolean = CBool(value)
        If valueAsBoolean Then
            Return WpfColor.LightGray
        Else
            Return WpfColor.White
        End If
    End Function

    Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.ConvertBack
        Throw New NotImplementedException()
    End Function
End Class
