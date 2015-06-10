'*****************************************************************************************
'                                  ViewToViewmodelAssignments.vb
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

Imports System.Windows.Forms
Imports System.Collections.ObjectModel
Imports System.ComponentModel

''' <summary>
''' Verwaltet eine Datenstruktur, die das Nachschlagen eines View-Types auf Basis eines ViewModel-Typs ermöglicht.
''' </summary>
''' <remarks></remarks>
Public Class ViewToViewmodelAssignments
    Inherits KeyedCollection(Of Type, ViewToViewmodelAssignment)

    Protected Overrides Function GetKeyForItem(item As ViewToViewmodelAssignment) As Type
        Return item.ViewModelType
    End Function
End Class

''' <summary>
''' Repräsentiert eine Zuordnungseinheit ViewModel --> View und ermöglicht das Nachschlagen einer View auf Basis eines ViewModels.
''' </summary>
''' <remarks></remarks>
Public Class ViewToViewmodelAssignment

    Sub New(viewType As Type, viewModelType As Type)
        CheckTypesWithExceptions(viewType, viewModelType)
        Me.ViewType = viewType
        Me.ViewModelType = viewModelType
    End Sub

    Public Shared Sub CheckTypesWithExceptions(viewType As Type, viewModelType As Type)
        If Not (viewType.IsAssignableFrom(GetType(ContainerControl))) Then
            Throw New ArgumentException("Only types derived from ContainerControl could act as Views under Windows Forms. Choose another type, like Typeof(UserControl) or Typeof(Form) (Gettype(type) in Visual Basic).")
        End If

        If Not (viewModelType.IsAssignableFrom(GetType(INotifyPropertyChanged))) Then
            Throw New ArgumentException("Only types implementing the IMvvmViewModel interdace could act as ViewsModels under Windows Forms. Choose a type that implements this interface and pass it with TypeOf (or Gettype in Visual Basic).")
        End If
    End Sub

    Public Property ViewType As Type
    Public Property ViewModelType As Type

End Class
