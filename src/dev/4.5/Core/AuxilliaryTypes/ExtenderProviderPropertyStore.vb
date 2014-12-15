Imports System.ComponentModel
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
