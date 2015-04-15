Imports System.ComponentModel
Imports System.Collections.ObjectModel
Imports System.Threading
Imports System.Runtime.CompilerServices
Imports ActiveDevelop.MvvmBaseLib

<Serializable, MvvmSystemElement>
Public Class ObservableBindingList(Of T As New)
    Inherits ObservableCollection(Of T)
    Implements IBindingList
    Implements IDisposable
    Implements ICancelAddNew

    Private mySyncRoot As New Object
    Private mySyncContext As SynchronizationContext = SynchronizationContext.Current

    Public Event ListChanged(sender As Object, e As ListChangedEventArgs) Implements IBindingList.ListChanged
    Public Event AddingNew(sender As Object, e As AddingNewEventArgs)

    Sub New()
        'If Debugger.IsAttached Then
        '    Debugger.Break()
        'End If
        AddHandler Me.CollectionChanged, AddressOf InnerCollectionChanged
    End Sub

    Sub New(observalueCollection As ObservableCollection(Of T))
        For Each ocItem In observalueCollection
            Me.Add(ocItem)
        Next
        AddHandler Me.CollectionChanged, AddressOf InnerCollectionChanged
    End Sub

    Sub New(listItems As IEnumerable(Of T))
        Dim tmpList = listItems.ToList
        For Each ocItem In tmpList
            Me.Add(ocItem)
        Next
        tmpList = Nothing
        AddHandler Me.CollectionChanged, AddressOf InnerCollectionChanged
    End Sub

    Private Sub ItemPropertyChangedHandlerProc(sender As Object, e As PropertyChangedEventArgs)
        Dim itemIndex = Me.IndexOf(DirectCast(sender, T))
        Dim listChangeEventArgs As New ListChangedEventArgs(ListChangedType.ItemChanged, itemIndex, itemIndex)
        OnListChanged(listChangeEventArgs)
    End Sub

    Protected Overrides Sub ClearItems()
        For Each element In Me
            Dim pcItem = TryCast(element, INotifyPropertyChanged)
            If pcItem IsNot Nothing Then
                RemoveHandler pcItem.PropertyChanged, AddressOf ItemPropertyChangedHandlerProc
            End If
        Next
        MyBase.ClearItems()
    End Sub

    Protected Overrides Sub InsertItem(index As Integer, item As T)
        MyBase.InsertItem(index, item)
        Dim pcItem = TryCast(item, INotifyPropertyChanged)
        If pcItem IsNot Nothing Then
            AddHandler pcItem.PropertyChanged, AddressOf ItemPropertyChangedHandlerProc
        End If
    End Sub

    Protected Overrides Sub RemoveItem(index As Integer)
        Dim pcItem = TryCast(Me(index), INotifyPropertyChanged)
        If pcItem IsNot Nothing Then
            RemoveHandler pcItem.PropertyChanged, AddressOf ItemPropertyChangedHandlerProc
        End If
        MyBase.RemoveItem(index)
    End Sub

    Protected Overrides Sub SetItem(index As Integer, item As T)
        Dim pcItem = TryCast(Me(index), INotifyPropertyChanged)
        If pcItem IsNot Nothing Then
            RemoveHandler pcItem.PropertyChanged, AddressOf ItemPropertyChangedHandlerProc
        End If
        MyBase.SetItem(index, item)
        pcItem = TryCast(Me(index), INotifyPropertyChanged)
        If pcItem IsNot Nothing Then
            AddHandler pcItem.PropertyChanged, AddressOf ItemPropertyChangedHandlerProc
        End If
    End Sub

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
        'mySyncContext.Send(Sub()
        '                   End Sub, Nothing)
    End Sub

    Public Sub AddIndex([property] As PropertyDescriptor) Implements IBindingList.AddIndex
        Throw New NotImplementedException("AddIndex is not suppoerted.")
    End Sub

    Protected Overridable Function AddNew() As Object Implements IBindingList.AddNew
        Dim tmpTInstance As New T
        Dim e As New AddingNewEventArgs(tmpTInstance)
        OnAddingNew(e)

        Dim tInstance = TryCast(e.NewObject, INotifyEditState(Of T))
        If tInstance IsNot Nothing Then
            tInstance.BeginAdding(Me)
        End If

        Me.Add(DirectCast(e.NewObject, T))
        Return e.NewObject
    End Function

    Protected Overridable Sub OnAddingNew(e As AddingNewEventArgs)
        RaiseEvent AddingNew(Me, e)
    End Sub

    Protected Overridable Sub CancelNew(itemIndex As Integer) Implements ICancelAddNew.CancelNew
        Dim tInstance = TryCast(Me(itemIndex), INotifyEditState(Of T))
        If tInstance IsNot Nothing Then
            tInstance.CancelNew(Me)
        End If
    End Sub

    Protected Overridable Sub EndNew(itemIndex As Integer) Implements ICancelAddNew.EndNew
        Dim tInstance = TryCast(Me(itemIndex), INotifyEditState(Of T))
        If tInstance IsNot Nothing Then
            tInstance.EndNew(Me)
        End If
    End Sub

    Protected Overridable ReadOnly Property AllowEdit As Boolean Implements IBindingList.AllowEdit
        Get
            Return True
        End Get
    End Property

    Protected Overridable ReadOnly Property AllowNew As Boolean Implements IBindingList.AllowNew
        Get
            Return True
        End Get
    End Property

    Protected Overridable ReadOnly Property AllowRemove As Boolean Implements IBindingList.AllowRemove
        Get
            Return True
        End Get
    End Property

    Protected Overridable Sub ApplySort([property] As PropertyDescriptor, direction As ListSortDirection) Implements IBindingList.ApplySort
        Throw New NotImplementedException("ApplySort is not supported.")
    End Sub

    Protected Overridable Function Find([property] As PropertyDescriptor, key As Object) As Integer Implements IBindingList.Find
        Throw New NotImplementedException("Find is not supported.")
    End Function

    Protected Overridable ReadOnly Property IsSorted As Boolean Implements IBindingList.IsSorted
        Get
            Return False
        End Get
    End Property

    Protected Overridable Sub RemoveIndex([property] As PropertyDescriptor) Implements IBindingList.RemoveIndex
        Throw New NotImplementedException("RemoveIndex is not supported.")
    End Sub

    Protected Overridable Sub RemoveSort() Implements IBindingList.RemoveSort
        Throw New NotImplementedException("RemoveSort is not supported.")
    End Sub

    Protected Overridable ReadOnly Property SortDirection As ListSortDirection Implements IBindingList.SortDirection
        Get
            Return Nothing
        End Get
    End Property

    Protected Overridable ReadOnly Property SortProperty As PropertyDescriptor Implements IBindingList.SortProperty
        Get
            Return Nothing
        End Get
    End Property

    Protected Overridable ReadOnly Property SupportsChangeNotification As Boolean Implements IBindingList.SupportsChangeNotification
        Get
            Return True
        End Get
    End Property

    Protected Overridable ReadOnly Property SupportsSearching As Boolean Implements IBindingList.SupportsSearching
        Get
            Return False
        End Get
    End Property

    Protected Overridable ReadOnly Property SupportsSorting As Boolean Implements IBindingList.SupportsSorting
        Get
            Return False
        End Get
    End Property

#Region "IDisposable Support"
    Private disposedValue As Boolean ' To detect redundant calls

    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                RemoveHandler Me.CollectionChanged, AddressOf InnerCollectionChanged
            End If
        End If

        Me.disposedValue = True
    End Sub

    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region

End Class

Public Module ObservableBindinglistExtender

    <Extension>
    Public Function ToObservableBindingList(Of T As New)(list As IEnumerable(Of T)) As ObservableBindingList(Of T)
        Return New ObservableBindingList(Of T)(list)
    End Function

End Module
