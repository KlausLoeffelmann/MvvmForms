'*****************************************************************************************
'                                          BindEventArgs
'                                          ==============
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

Imports System.Windows.Forms
Imports System.ComponentModel

Public Class ValueAssigningEventArgs
    Inherits CancelEventArgs

    Sub New()
        MyBase.New()
    End Sub

    Sub New(cancel As Boolean)
        MyBase.New(cancel)
    End Sub

    Property Control As Object
    Property ViewModelObject As Object
    Property ControlPropertyName As String
    Property ViewModelPropertyName As String
    Property Value As Object
    Property Target As Targets

End Class

Public Class ValueAssignedEventArgs
    Inherits EventArgs

    Private myValue As Object

    Sub New()
        MyBase.New()
    End Sub

    Sub New(e As ValueAssigningEventArgs)
        Me.Control = e.Control
        Me.ViewModelObject = e.ViewModelObject
        Me.ControlPropertyName = e.ControlPropertyName
        Me.ViewModelPropertyName = e.ViewModelPropertyName
        Me.Value = e.Value
        Me.Target = e.Target
    End Sub

    Property Control As Object
    Property ViewModelObject As Object
    Property ControlPropertyName As String
    Property ViewModelPropertyName As String
    Property Value As Object
        Get
            Return myvalue
        End Get
        Private Set(value As Object)
            myValue = value
        End Set
    End Property
    Property Target As Targets

End Class

Public Enum Targets
    Control
    ViewModel
End Enum
