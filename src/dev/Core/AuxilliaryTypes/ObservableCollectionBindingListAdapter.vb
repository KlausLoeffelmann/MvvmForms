Imports System.Collections.Specialized
Imports System.ComponentModel
Imports System.Windows

Public Class ObservableCollectionBindingListAdapter
    Implements IBindingList

    Public Event ListChanged As ListChangedEventHandler Implements IBindingList.ListChanged

    Private myAssignedCollection As INotifyCollectionChanged

    Sub New()
        MyBase.New
    End Sub

    Sub New(observalueCollection As INotifyCollectionChanged)
        Me.DataSource = observalueCollection
    End Sub

    Public Property DataSource As INotifyCollectionChanged
        Get
            Return myAssignedCollection
        End Get
        Set(value As INotifyCollectionChanged)
            If Not Object.Equals(myAssignedCollection, value) Then
                If myAssignedCollection IsNot Nothing Then
                    WeakEventManager(Of INotifyCollectionChanged, NotifyCollectionChangedEventArgs).RemoveHandler(
                            myAssignedCollection, NameOf(INotifyCollectionChanged.CollectionChanged),
                            AddressOf InnerCollectionChanged)
                End If

                If value IsNot Nothing Then
                    myAssignedCollection = value

                    WeakEventManager(Of INotifyCollectionChanged, NotifyCollectionChangedEventArgs).AddHandler(
                            myAssignedCollection, NameOf(INotifyCollectionChanged.CollectionChanged),
                            AddressOf InnerCollectionChanged)
                Else
                    myAssignedCollection = Nothing
                End If
            End If
        End Set
    End Property

    Private Sub InnerCollectionChanged(sender As Object, e As System.Collections.Specialized.NotifyCollectionChangedEventArgs)

        Dim oldStartingIndex = e.OldStartingIndex
        Dim newStartingIndex = e.NewStartingIndex

        Dim changeType As ListChangedType

        If e.Action = Specialized.NotifyCollectionChangedAction.Add Then
            changeType = ListChangedType.ItemAdded
        ElseIf e.Action = Specialized.NotifyCollectionChangedAction.Move Then
            changeType = ListChangedType.ItemMoved
        ElseIf e.Action = Specialized.NotifyCollectionChangedAction.Remove Then
            newStartingIndex = 0
            changeType = ListChangedType.ItemDeleted
        ElseIf e.Action = Specialized.NotifyCollectionChangedAction.Replace Then
            changeType = ListChangedType.ItemChanged
        ElseIf e.Action = Specialized.NotifyCollectionChangedAction.Reset Then
            changeType = ListChangedType.Reset
        End If

        Dim listChangeEventArgs As New ListChangedEventArgs(changeType, newStartingIndex, oldStartingIndex)
        OnListChanged(listChangeEventArgs)
    End Sub

    Protected Overridable Sub OnListChanged(e As ListChangedEventArgs)
        RaiseEvent ListChanged(Me, e)
    End Sub

    Public ReadOnly Property AllowEdit As Boolean Implements IBindingList.AllowEdit
        Get
            Return False
        End Get
    End Property

    Public ReadOnly Property AllowNew As Boolean Implements IBindingList.AllowNew
        Get
            Return False
        End Get
    End Property

    Public ReadOnly Property AllowRemove As Boolean Implements IBindingList.AllowRemove
        Get
            Return False
        End Get
    End Property

    Public ReadOnly Property Count As Integer Implements ICollection.Count
        Get
            Return DirectCast(myAssignedCollection, ICollection).Count
        End Get
    End Property

    Public ReadOnly Property IsFixedSize As Boolean Implements IList.IsFixedSize
        Get
            Return False
        End Get
    End Property

    Public ReadOnly Property IsReadOnly As Boolean Implements IList.IsReadOnly
        Get
            Return True
        End Get
    End Property

    Public ReadOnly Property IsSorted As Boolean Implements IBindingList.IsSorted
        Get
            Return False
        End Get
    End Property

    Public ReadOnly Property IsSynchronized As Boolean Implements ICollection.IsSynchronized
        Get
            Return False
        End Get
    End Property

    Default Public Property Item(index As Integer) As Object Implements IList.Item
        Get
            Return DirectCast(myAssignedCollection, ICollection)(index)
        End Get
        Set(value As Object)
            Throw New NotImplementedException()
        End Set
    End Property

    Public ReadOnly Property SortDirection As ListSortDirection Implements IBindingList.SortDirection
        Get
            Throw New NotImplementedException()
        End Get
    End Property

    Public ReadOnly Property SortProperty As PropertyDescriptor Implements IBindingList.SortProperty
        Get
            Throw New NotImplementedException()
        End Get
    End Property

    Public ReadOnly Property SupportsChangeNotification As Boolean Implements IBindingList.SupportsChangeNotification
        Get
            Return True
        End Get
    End Property

    Public ReadOnly Property SupportsSearching As Boolean Implements IBindingList.SupportsSearching
        Get
            Return False
        End Get
    End Property

    Public ReadOnly Property SupportsSorting As Boolean Implements IBindingList.SupportsSorting
        Get
            Return False
        End Get
    End Property

    Public ReadOnly Property SyncRoot As Object Implements ICollection.SyncRoot
        Get
            Throw New NotImplementedException()
        End Get
    End Property

    Public Sub AddIndex([property] As PropertyDescriptor) Implements IBindingList.AddIndex
        Throw New NotImplementedException()
    End Sub

    Public Sub ApplySort([property] As PropertyDescriptor, direction As ListSortDirection) Implements IBindingList.ApplySort
        Throw New NotImplementedException()
    End Sub

    Public Sub Clear() Implements IList.Clear
        Throw New NotImplementedException()
    End Sub

    Public Sub CopyTo(array As Array, index As Integer) Implements ICollection.CopyTo
        Throw New NotImplementedException()
    End Sub

    Public Sub Insert(index As Integer, value As Object) Implements IList.Insert
        Throw New NotImplementedException()
    End Sub

    Public Sub Remove(value As Object) Implements IList.Remove
        Throw New NotImplementedException()
    End Sub

    Public Sub RemoveAt(index As Integer) Implements IList.RemoveAt
        Throw New NotImplementedException()
    End Sub

    Public Sub RemoveIndex([property] As PropertyDescriptor) Implements IBindingList.RemoveIndex
        Throw New NotImplementedException()
    End Sub

    Public Sub RemoveSort() Implements IBindingList.RemoveSort
        Throw New NotImplementedException()
    End Sub

    Public Function Add(value As Object) As Integer Implements IList.Add
        Throw New NotImplementedException()
    End Function

    Public Function AddNew() As Object Implements IBindingList.AddNew
        Throw New NotImplementedException()
    End Function

    Public Function Contains(value As Object) As Boolean Implements IList.Contains
        Throw New NotImplementedException()
    End Function

    Public Function Find([property] As PropertyDescriptor, key As Object) As Integer Implements IBindingList.Find
        Throw New NotImplementedException()
    End Function

    Public Function GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
        Return DirectCast(myAssignedCollection, IEnumerable).GetEnumerator
    End Function

    Public Function IndexOf(value As Object) As Integer Implements IList.IndexOf
        Throw New NotImplementedException()
    End Function
End Class
