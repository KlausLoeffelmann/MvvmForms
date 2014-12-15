Imports System.ComponentModel

<TypeConverter(GetType(GridPanelGridInfoTypeConverter)), Serializable()>
Public Structure GridPanelGridInfo

    Sub New(row As Integer, Column As Integer, rowSpan As Integer, columnSpan As Integer)
        Me.Row = row
        Me.Column = Column
        Me.RowSpan = rowSpan
        Me.ColumnSpan = columnSpan
    End Sub

    Public Property Row As Integer
    Public Property Column As Integer
    Public Property RowSpan As Integer
    Public Property ColumnSpan As Integer
    Public Property IsCaptionCell As Boolean

    Public Overrides Function ToString() As String
        Return Row & "; " & Column & "; " & RowSpan & "; " & ColumnSpan & "; " & IsCaptionCell
    End Function
End Structure

Public Class GridPanelGridInfoTypeConverter
    Inherits TypeConverter

    Public Overrides Function CanConvertFrom(context As System.ComponentModel.ITypeDescriptorContext, sourceType As System.Type) As Boolean
        If sourceType.Equals(GetType(String)) Then
            Return True
        Else
            Return MyBase.CanConvertFrom(context, sourceType)
        End If
    End Function

    Public Overrides Function ConvertTo(context As System.ComponentModel.ITypeDescriptorContext, culture As System.Globalization.CultureInfo, value As Object, destinationType As System.Type) As Object
        If destinationType.Equals(GetType(String)) Then
            Return value.ToString
        Else
            Return MyBase.ConvertTo(context, culture, value, destinationType)
        End If
    End Function

    Public Overrides Function CanConvertTo(context As System.ComponentModel.ITypeDescriptorContext, destinationType As System.Type) As Boolean
        If destinationType.Equals(GetType(String)) Then
            Return True
        Else
            Return MyBase.CanConvertTo(context, destinationType)
        End If
    End Function

    Public Overrides Function ConvertFrom(context As System.ComponentModel.ITypeDescriptorContext, culture As System.Globalization.CultureInfo, value As Object) As Object
        If (TypeOf value Is String) Then
            Dim valuesAsStrings() = CType(value, String).Split(";"c)
            Dim tmp = New GridPanelGridInfo With {.Row = Integer.Parse(valuesAsStrings(0)),
                                      .Column = Integer.Parse(valuesAsStrings(1)),
                                      .RowSpan = Integer.Parse(valuesAsStrings(2)),
                                      .ColumnSpan = Integer.Parse(valuesAsStrings(3))}
            If valuesAsStrings.Length = 5 Then
                tmp.IsCaptionCell = Boolean.Parse(valuesAsStrings(4))
            End If
            Return tmp
        Else
            Return MyBase.ConvertFrom(context, culture, value)
        End If
    End Function

    Public Overrides Function GetPropertiesSupported(context As System.ComponentModel.ITypeDescriptorContext) As Boolean
        Return True
    End Function

    Public Overrides Function GetProperties(context As System.ComponentModel.ITypeDescriptorContext, value As Object, attributes() As System.Attribute) As System.ComponentModel.PropertyDescriptorCollection
        Return TypeDescriptor.GetProperties(value)
    End Function
End Class
