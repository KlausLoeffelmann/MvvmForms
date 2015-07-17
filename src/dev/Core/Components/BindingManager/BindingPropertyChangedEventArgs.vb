'*****************************************************************************************
'                                          BindingPropertyChangedEventArgs
'                                          ===============================
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
'    CONTACT INFO:
'    Klaus Loeffelmann, C/O ActiveDevelop
'                       Bremer Str. 4
'                       Lippstadt, DE-59555
'                       Germany
'                       email: mvvmforms at activedevelop . de. 
'*****************************************************************************************

Imports System.Windows.Data

''' <summary>
''' Provides data for the BindingPropertyChanged Event.
''' </summary>
''' <remarks></remarks>
Public Class BindingPropertyChangedEventArgs
    Inherits EventArgs

    Sub New()
        MyBase.New()
    End Sub

    Sub New(originalSource As Object)
        Me.OriginalSource = originalSource
    End Sub

    ''' <summary>
    ''' Control, which caused this event.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property OriginalSource As Object

    ''' <summary>
    ''' BindingPath-Name of the property, which caused this event.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property EventProperty As String

    ''' <summary>
    ''' The converter to use for data conversion, if a converter has been spacified, otherwise NULL (Nothing in VB).
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Converter As IValueConverter

End Class
