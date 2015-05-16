'*****************************************************************************************
'                                StringValueToStringConverter.vb
'                    =======================================================
'
'          Part of MvvmForms - The Component Library for bringing the Model-View-Viewmodel
'                              pattern to Data Centric Windows Forms Apps in an easy,
'                              feasible and XAML-compatible way.
'
'                    Copyright -2015 by Klaus Loeffelmann
'
'    This program is free software; you can redistribute it and/or modify
'    it under the terms of the GNU General Public License as published by
'    the Free Software Foundation; either version 2 of the License, or
'    (at your option) any later version.
'
'    This program is distributed in the hope that it will be useful,
'    but WITHOUT ANY WARRANTY; without even the implied warranty Of
'    MERCHANTABILITY Or FITNESS FOR A PARTICULAR PURPOSE.  See the
'    GNU General Public License For more details.
'
'    You should have received a copy of the GNU General Public License along
'    with this program; if not, write to the Free Software Foundation, Inc.,
'    51 Franklin Street, Fifth Floor, Boston, MA 02110-1301 USA.
'
'    MvvmForms is dual licenced. A permissive licence can be obtained - CONTACT INFO:
'
'                       ActiveDevelop
'                       Bremer Str. 4
'                       Lippstadt, DE-59555
'                       Germany
'                       email: mvvmforms at activedevelop . de. 
'*****************************************************************************************

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

