Imports System.ComponentModel
Imports System.Collections.ObjectModel
Imports ActiveDevelop.EntitiesFormsLib.ViewModelBase

''' <summary>
''' Definiert eine Schnittstelle für Elemente, die mithilfe einer VisibilityAwareItemCollection als Datenquelle die Sichtbarkeit ihrer 
''' grafischen Repräsentation in einem IBindingList-Steuerelement (DataGridView, ListView) durch setzen ihrer IsVisible Eigenschaft selber steuern können.
''' </summary>
''' <remarks></remarks>
Public Interface IVisibilityAwareItem
    Event IsVisibleChanged(sender As Object, e As EventArgs)
    Property IsVisible As Boolean
End Interface

''' <summary>
''' Stellt eine Auflistung zur Verfügung, mit deren Hilfe Items, die die IVisibilityAwareItem-Schnittstelle implementieren, 
''' durch Ihre IsVisible-Eigenschaft die Sichtbarkeit eines Elementes später in einem IBindingList-kompatibelen Steuerelement steuern können.
''' </summary>
''' <typeparam name="T"></typeparam>
''' <remarks>Wenn eine Auflistung mit Elementen erstellt wird, die IVisibilityAwareItem und damit die IsVisible-Eigenschaft 
''' implementieren, und diese Elemente später einem Listen-Steuerelement als Datenquellen dienen sollen, können die Items 
''' selbst durch Setzen ihrer IsVisible-Eigenschaft dafür sorgen, ob ihre grafische Repräsentation in dem Liste-
''' Steuerelement zu sehen ist. WICHTIG: Dazu muss allerdings nicht die Auflistung selber als Datenquelle an die BindingSource 
''' des Listen-Steuerelementes gebunden werden, sondern ihre FilteredItems-Eigenschaft.</remarks>
<MvvmSystemElement>
Public Class VisibilityAwareItemCollection(Of t As {New, INotifyPropertyChanged, IVisibilityAwareItem})
    Inherits BindableBase
    Implements IBindingList

    Public Event ListChanged(sender As Object, e As ListChangedEventArgs) Implements IBindingList.ListChanged

    Private myOriginalCollection As VisibilityAwareItemCollectionInternal(Of t)

    Sub New()
        myOriginalCollection = New VisibilityAwareItemCollectionInternal(Of t)
        AddHandler myOriginalCollection.FilteredItems.ListChanged, Sub(o, e)
                                                                       RaiseEvent ListChanged(o, e)
                                                                   End Sub
    End Sub

    Public Sub CopyTo(array As Array, index As Integer) Implements ICollection.CopyTo
        myOriginalCollection.CopyTo(CType(array, t()), index)
    End Sub

    Public ReadOnly Property Count As Integer Implements ICollection.Count
        Get
            Return myOriginalCollection.FilteredItems.Count
        End Get
    End Property

    Public ReadOnly Property IsSynchronized As Boolean Implements ICollection.IsSynchronized
        Get
            Return DirectCast(myOriginalCollection.FilteredItems, ICollection).IsSynchronized
        End Get
    End Property

    Public ReadOnly Property SyncRoot As Object Implements ICollection.SyncRoot
        Get
            Return DirectCast(myOriginalCollection.FilteredItems, ICollection).SyncRoot
        End Get
    End Property

    Public Function GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
        Return DirectCast(myOriginalCollection.FilteredItems, ICollection).GetEnumerator
    End Function

    Public Function Add(value As Object) As Integer Implements IList.Add
        myOriginalCollection.Add(DirectCast(value, t))
    End Function

    Public Sub Clear() Implements IList.Clear
        myOriginalCollection.Clear()
    End Sub

    Public Function Contains(value As Object) As Boolean Implements IList.Contains
        Return myOriginalCollection.Contains(DirectCast(value, t))
    End Function

    Public Function IndexOf(value As Object) As Integer Implements IList.IndexOf
        Return myOriginalCollection.IndexOf(DirectCast(value, t))
    End Function

    Public Sub Insert(index As Integer, value As Object) Implements IList.Insert
        myOriginalCollection.Insert(index, DirectCast(value, t))
    End Sub

    Public ReadOnly Property IsFixedSize As Boolean Implements IList.IsFixedSize
        Get
            Return False
        End Get
    End Property

    Public ReadOnly Property IsReadOnly As Boolean Implements IList.IsReadOnly
        Get
            Return False
        End Get
    End Property

    Private Property ItemInternal(index As Integer) As Object Implements IList.Item
        Get
            Return myOriginalCollection.FilteredItems(index)
        End Get
        Set(value As Object)
            'TODO: Objekt in der OriginalListe finden und dort ebenfalls setzen.
            Throw New NotImplementedException("Items can't be set directly.")
        End Set
    End Property

    Default Public Property Item(index As Integer) As t
        Get
            Return DirectCast(ItemInternal(index), t)
        End Get
        Set(value As t)
            ItemInternal(index) = value
        End Set
    End Property

    Public Property OriginalItem(index As Integer) As t
        Get
            Return myOriginalCollection(index)
        End Get
        Set(value As t)
            myOriginalCollection(index) = value
        End Set
    End Property

    Public Sub Remove(value As Object) Implements IList.Remove
        myOriginalCollection.Remove(DirectCast(value, t))
    End Sub

    Public Sub RemoveAt(index As Integer) Implements IList.RemoveAt
        myOriginalCollection.RemoveAt(index)
    End Sub

    Public Sub AddIndex([property] As PropertyDescriptor) Implements IBindingList.AddIndex
        Throw New NotImplementedException("Items can't be set directly.")
    End Sub

    Public Function AddNew() As Object Implements IBindingList.AddNew
        Return DirectCast(myOriginalCollection, IBindingList).AddNew
    End Function

    Public ReadOnly Property AllowEdit As Boolean Implements IBindingList.AllowEdit
        Get
            Return DirectCast(myOriginalCollection, IBindingList).AllowEdit
        End Get
    End Property

    Public ReadOnly Property AllowNew As Boolean Implements IBindingList.AllowNew
        Get
            Return DirectCast(myOriginalCollection, IBindingList).AllowNew
        End Get
    End Property

    Public ReadOnly Property AllowRemove As Boolean Implements IBindingList.AllowRemove
        Get
            Return DirectCast(myOriginalCollection, IBindingList).AllowRemove
        End Get
    End Property

    Public Sub ApplySort([property] As PropertyDescriptor, direction As ListSortDirection) Implements IBindingList.ApplySort
        Throw New NotImplementedException("Not implemented.")
    End Sub

    Public Function Find([property] As PropertyDescriptor, key As Object) As Integer Implements IBindingList.Find
        Throw New NotImplementedException("Not implemented.")
    End Function

    Public ReadOnly Property IsSorted As Boolean Implements IBindingList.IsSorted
        Get
            Return False
        End Get
    End Property

    Public Sub RemoveIndex([property] As PropertyDescriptor) Implements IBindingList.RemoveIndex
        Throw New NotImplementedException("Not implemented.")
    End Sub

    Public Sub RemoveSort() Implements IBindingList.RemoveSort
        DirectCast(myOriginalCollection, IBindingList).RemoveSort()
    End Sub

    Public ReadOnly Property SortDirection As ListSortDirection Implements IBindingList.SortDirection
        Get
            Return DirectCast(myOriginalCollection, IBindingList).SortDirection
        End Get
    End Property

    Public ReadOnly Property SortProperty As PropertyDescriptor Implements IBindingList.SortProperty
        Get
            Return DirectCast(myOriginalCollection, IBindingList).SortProperty
        End Get
    End Property

    Public ReadOnly Property SupportsChangeNotification As Boolean Implements IBindingList.SupportsChangeNotification
        Get
            Return DirectCast(myOriginalCollection, IBindingList).SupportsChangeNotification
        End Get
    End Property

    Public ReadOnly Property SupportsSearching As Boolean Implements IBindingList.SupportsSearching
        Get
            Return DirectCast(myOriginalCollection, IBindingList).SupportsSearching
        End Get
    End Property

    Public ReadOnly Property SupportsSorting As Boolean Implements IBindingList.SupportsSorting
        Get
            Return DirectCast(myOriginalCollection, IBindingList).SupportsSorting
        End Get
    End Property
End Class

<MvvmSystemElement>
Public Class VisibilityAwareItemCollectionInternal(Of T As {New, INotifyPropertyChanged, IVisibilityAwareItem})
    Inherits ObservableBindingList(Of T)

    Private myFilteredCollection As New ObservableBindingList(Of T)

    Friend Sub New()
        MyBase.New()
    End Sub

    Protected Overrides Sub ClearItems()
        MyBase.ClearItems()
        For Each pcItem In myFilteredCollection
            RemoveHandler pcItem.IsVisibleChanged, AddressOf ItemPropertyChangedEventHandlerProc
        Next
        myFilteredCollection.Clear()
    End Sub

    Protected Overrides Sub InsertItem(index As Integer, item As T)
        If item.IsVisible Then
            InsertItemInFilteredCollection(index, item)
        End If
        AddHandler item.IsVisibleChanged, AddressOf ItemPropertyChangedEventHandlerProc
        MyBase.InsertItem(index, item)
    End Sub

    Private Sub InsertItemInFilteredCollection(index As Integer, item As T)
        If index > -1 AndAlso Me.Count > 0 Then
            If item.IsVisible Then
                Dim tmpItem = FindNextVisibleItem(index)
                If tmpItem Is Nothing Then
                    myFilteredCollection.Add(item)
                Else
                    Dim tmpIndex = myFilteredCollection.IndexOf(tmpItem)
                    If tmpIndex > -1 Then
                        myFilteredCollection.Insert(tmpIndex, item)
                    Else
                        myFilteredCollection.Add(item)
                    End If
                End If
            End If
        Else
            myFilteredCollection.Add(item)
        End If
    End Sub

    Protected Overrides Sub RemoveItem(index As Integer)
        Dim tmpIndex = myFilteredCollection.IndexOf(Me(index))
        If tmpIndex >= 0 Then
            myFilteredCollection.RemoveAt(tmpIndex)
        End If
        RemoveHandler Me(index).IsVisibleChanged, AddressOf ItemPropertyChangedEventHandlerProc
        MyBase.RemoveItem(index)
    End Sub

    Private Sub ItemPropertyChangedEventHandlerProc(sender As Object, e As EventArgs)
        Dim item = DirectCast(sender, T)
        If Not item.IsVisible Then
            'Erase from Internal Collection
            myFilteredCollection.RemoveAt(myFilteredCollection.IndexOf(item))
        Else
            'Add to Internal Collection
            InsertItemInFilteredCollection(Me.IndexOf(item), item)
        End If
    End Sub

    Protected Overrides Sub SetItem(index As Integer, item As T)
        Dim oldItem = Me(index)
        RemoveHandler oldItem.IsVisibleChanged, AddressOf ItemPropertyChangedEventHandlerProc
        Dim tmpIndex = myFilteredCollection.IndexOf(oldItem)
        If tmpIndex > -1 Then
            myFilteredCollection.RemoveAt(tmpIndex)
        End If
        If item.IsVisible Then
            InsertItemInFilteredCollection(index, item)
        End If
        AddHandler item.IsVisibleChanged, AddressOf ItemPropertyChangedEventHandlerProc
        MyBase.SetItem(index, item)
    End Sub

    Private Function FindNextVisibleItem(fromIndex As Integer) As T
        For i As Integer = fromIndex + 1 To Me.Count - 1
            If Me(i).IsVisible Then
                Return Me(i)
            End If
        Next
        Return Nothing
    End Function

    <MvvmViewModelInclude>
    Public Property FilteredItems As ObservableBindingList(Of T)
        Get
            Return myFilteredCollection
        End Get
        Set(value As ObservableBindingList(Of T))
            Throw New NotImplementedException("Here, we don't possibly come.")
        End Set
    End Property
End Class
