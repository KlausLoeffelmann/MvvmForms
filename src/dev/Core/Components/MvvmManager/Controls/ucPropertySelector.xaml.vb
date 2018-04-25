'*****************************************************************************************
'                                    ucPropertySelector.vb
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

Imports System.Collections.ObjectModel
Imports System.Windows
Imports System.Windows.Controls
Imports ActiveDevelop.EntitiesFormsLib

Public Class ucPropertySelector
    Friend Shared ReadOnly ItemsSourceProperty As DependencyProperty =
                           DependencyProperty.Register(NameOf(ItemsSource),
                           GetType(ObservableCollection(Of PropertyBindingNodeDefinition)), GetType(ucPropertySelector))

    Public Property ItemsSource As ObservableCollection(Of PropertyBindingNodeDefinition)
        Get
            Return CType(GetValue(ItemsSourceProperty), ObservableCollection(Of PropertyBindingNodeDefinition))
        End Get
        Set(ByVal value As ObservableCollection(Of PropertyBindingNodeDefinition))
            SetValue(ItemsSourceProperty, value)
        End Set
    End Property

    Friend Shared ReadOnly SelectedPropertyProperty As DependencyProperty =
                           DependencyProperty.Register(NameOf(SelectedProperty),
                           GetType(BindingProperty), GetType(ucPropertySelector))

    Public Property SelectedProperty As BindingProperty
        Get
            Return CType(GetValue(SelectedPropertyProperty), BindingProperty)
        End Get
        Set(ByVal value As BindingProperty)
            Dim eq = (SelectedProperty Is value)
            SetValue(SelectedPropertyProperty, value)
            If Not eq Then
                Dim Itm = (From item In Me.ItemsSource Where item.Binding Is value).FirstOrDefault
                If Itm IsNot Nothing Then
                    Dim selitem = TryCast(tvProps.ItemContainerGenerator.ContainerFromItem(Itm), TreeViewItem)
                    If selitem IsNot Nothing Then
                        selitem.IsSelected = True
                    End If
                End If
            End If
        End Set
    End Property


    Public Event SelectedPropertyChanged As EventHandler(Of System.Windows.RoutedPropertyChangedEventArgs(Of Object))

    Private Sub TreeView_SelectedItemChanged(sender As Object, e As RoutedPropertyChangedEventArgs(Of Object))
        If e.NewValue IsNot Nothing Then
            SelectedProperty = DirectCast(e.NewValue, PropertyBindingNodeDefinition).Binding
            Debug.WriteLine($"SelectedProperty: {SelectedProperty.PropertyName}, Typ: {SelectedProperty.PropertyType.Name}")
        Else
            SelectedProperty = Nothing
            Debug.WriteLine($"SelectedProperty: nothing")
        End If
        togglePopup.IsOpen = False
        RaiseEvent SelectedPropertyChanged(Me, e)
    End Sub

    Private Sub toggleButton_Click(sender As Object, e As RoutedEventArgs) Handles toggleButton.Click
        togglePopup.IsOpen = Not togglePopup.IsOpen
    End Sub
End Class
