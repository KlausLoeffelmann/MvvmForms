'*****************************************************************************************
'                                          ExtenderProviderPropertyStore
'                                          =============================
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
Imports System.Windows.Forms

Public Class ExtenderProviderPropertyStoreItem(Of T As New)

    Sub New()
        MyBase.New()
    End Sub

    Sub New(data As T)
        Me.Data = data
    End Sub

    Property Control As Control
    Property Data As T
End Class

Public Class ExtenderProviderPropertyStore(Of T As New)
    Inherits KeyedCollection(Of Control, ExtenderProviderPropertyStoreItem(Of T))

    Public Event InsertedItem(sender As Object, e As ExtenderProviderPropertyStoreCollectionChangedEventArgs)

    Protected Overrides Sub InsertItem(index As Integer, item As ExtenderProviderPropertyStoreItem(Of T))
        MyBase.InsertItem(index, item)
        RaiseEvent InsertedItem(Me, New ExtenderProviderPropertyStoreCollectionChangedEventArgs With {.Item = item})
    End Sub

    Protected Overrides Sub RemoveItem(index As Integer)
        MyBase.RemoveItem(index)
    End Sub

    Protected Overrides Sub SetItem(index As Integer, item As ExtenderProviderPropertyStoreItem(Of T))
        MyBase.SetItem(index, item)
    End Sub

    Protected Overrides Function GetKeyForItem(item As ExtenderProviderPropertyStoreItem(Of T)) As Control
        Return item.Control
    End Function

    Public Function GetPropertyStoreItem(ctrl As Control) As T
        If Me.Contains(ctrl) Then
            Return Me(ctrl).Data
        Else
            Dim tItem = New T
            Dim myStoreItem As New ExtenderProviderPropertyStoreItem(Of T) With {.Control = ctrl,
                                                                                .Data = tItem}
            Me.Add(myStoreItem)
            Return tItem
        End If
    End Function
End Class
