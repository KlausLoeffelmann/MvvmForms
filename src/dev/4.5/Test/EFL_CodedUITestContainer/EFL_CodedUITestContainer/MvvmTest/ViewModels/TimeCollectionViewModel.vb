Imports ActiveDevelop.EntitiesFormsLib
Imports System.Windows.Input
Imports ActiveDevelop.EntitiesFormsLib.ViewModelBase

<MvvmViewAttribute("TimeCollectionView")>
Public Class TimeCollectionViewModel
    Inherits MvvmViewModelBase

    Private myTimeCollectionItems As TimeCollectItems
    Private myExternalTimeCollectionItem As TimeCollectItem
    Private mySelectedItem As TimeCollectItem

    Protected Overrides Sub InitializeViewModel()

        myTimeCollectionItems = New TimeCollectItems()

        AddTimeCollectionItemCommand = New MvvmCommandBase(AddressOf AddTimeCollectionItem)
        DeleteTimeCollectionItemCommand = New MvvmCommandBase(AddressOf DeleteTimeCollectionItem)
        EditTimeCollectionItemCommand = New MvvmCommandBase(Sub(obj) EditTimeCollectionItem(obj))
    End Sub

    Private Sub AddTimeCollectionItem(param As Object)
        TimeCollectionItems.Add(New TimeCollectItem() With {.StartTime = Now.Add(New TimeSpan(12, 0, 0)),
                                                            .EndTime = Now.Add(New TimeSpan(18, 0, 0)),
                                                            .ActivityDescription = "Ich tat etwas um " & Now.TimeOfDay.ToString & " am " & Now.Date.ToString()})
    End Sub

    Private Sub EditTimeCollectionItem(notUsed As Object)

        'Dieses Item MUSS zu diesem Zeitpunkt "etwas" sein, 
        'da der Command sonst nicht hätte aufgerufen werden können.
        Dim timeCollectItem = Me.SelectedItem

        'Wir klonen das Objekt, da wir nicht möchten, dass es sich direkt in der Liste ändert.
        Dim clone = timeCollectItem.DeepClone(Of TimeCollectItem)()

        'Eine neue ViewModel-Instanz, für das Detail-editieren...
        Dim vm As New AddEditTimeCollectionItemViewModel
        '...der wir das zu editierende Item zuordnen...
        'vm.TimeCollectItem = clone

        '...und dann auf Basis des ViewModels den entsprechenden Dialog aufrufen.
        Dim retValue = ShowViewByViewModelModal(vm)

        If retValue = MvvmMessageBoxReturnValue.OK Then
            timeCollectItem.CopyPropertiesFrom(vm.TimeCollectItem)
        End If
    End Sub

    Private Sub DeleteTimeCollectionItem(param As Object)
        TimeCollectionItems.Remove(SelectedItem)
    End Sub

    Private Sub OnSelectedItemChange(selectedItem As TimeCollectItem)
        DeleteTimeCollectionItemCommand.CanExecuteState = selectedItem IsNot Nothing
        EditTimeCollectionItemCommand.CanExecuteState = DeleteTimeCollectionItemCommand.CanExecuteState

        Try
            Debug.Print(selectedItem.ToString)
        Catch ex As Exception
        End Try
    End Sub

    Public Property SelectedItem As TimeCollectItem
        Get
            Return mySelectedItem
        End Get
        Set(value As TimeCollectItem)
            If Not Object.Equals(value, mySelectedItem) Then
                MyBase.SetProperty(mySelectedItem, value)
                OnSelectedItemChange(mySelectedItem)
            End If
        End Set
    End Property

    Public Property TimeCollectionItems As TimeCollectItems
        Get
            Return myTimeCollectionItems
        End Get
        Set(value As TimeCollectItems)
            MyBase.SetProperty(myTimeCollectionItems, value)
        End Set
    End Property

    Public Property AddTimeCollectionItemCommand As MvvmCommandBase
    Public Property DeleteTimeCollectionItemCommand As MvvmCommandBase
    Public Property EditTimeCollectionItemCommand As MvvmCommandBase

End Class
