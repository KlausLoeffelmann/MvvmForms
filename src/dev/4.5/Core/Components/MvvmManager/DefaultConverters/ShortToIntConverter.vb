Imports System.Windows.Data

Public Class ShortToIntConverter
    Implements IValueConverter

    Private Shared preDefinedInstance As ShortToIntConverter

    Public Shared Function GetInstance() As ShortToIntConverter
        If preDefinedInstance Is Nothing Then
            preDefinedInstance = New ShortToIntConverter
        End If
        Return preDefinedInstance
    End Function

    Public Function Convert(value As Object, targetType As Type, parameter As Object, culture As Globalization.CultureInfo) As Object Implements IValueConverter.Convert
        If Not (TypeOf value Is Short Or TypeOf value Is Short?) AndAlso value IsNot Nothing Then
            Throw New TypeMismatchException("An instance of type Short or Short? was expected when using the ShortToIntConverter, but type '" & value.GetType.ToString & "' has been passed.")
        End If
        If value Is Nothing Then
            Return 0
        Else
            Return CInt(value)
        End If
    End Function

    Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, culture As Globalization.CultureInfo) As Object Implements IValueConverter.ConvertBack
        If Not (TypeOf value Is Integer) AndAlso value IsNot Nothing Then
            Throw New TypeMismatchException("An instance of type Integer was expected when using the ShortToIntConverter, but type '" & value.GetType.ToString & "' has been passed.")
        End If
        If value Is Nothing Then
            Return CShort(0)
        Else
            Return CShort(value)
        End If
    End Function

End Class
