'*****************************************************************************************
'                                    ShortToIntConverter.vb
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
