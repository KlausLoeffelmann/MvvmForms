Imports System.Windows.Controls
Imports System.Windows.Input

''' <summary>
''' Ableitung des WPF-DataGrids
''' </summary>
''' <remarks></remarks>
Public Class ExtDataGrid
    Inherits DataGrid

    ''' <summary>
    ''' Wird aufgerufen wenn die Sortierung bereits durchgelaufen ist
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Public Event Sorted(ByVal sender As Object, ByVal e As DataGridSortingEventArgs)

    ''' <summary>
    ''' Wird geworfen wenn Items mittels interner Loeschfunktion vom DataGrid geloescht wurden
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Public Event ItemsDeleted(ByVal sender As Object, ByVal e As ItemsDeletedEventArgs)

    ''' <summary>
    ''' Wird vor interner Loeschfunktion vom DataGrid geworfen
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Public Event ItemsDeleting(ByVal sender As Object, ByVal e As ItemsDeletingEventArgs)

    ''' <summary>
    ''' Wirft NACH der Sortierung das Sorted-Event
    ''' </summary>
    ''' <param name="column"></param>
    ''' <remarks></remarks>
    Protected Overridable Sub OnSorted(column As DataGridColumn)
        RaiseEvent Sorted(Me, New DataGridSortingEventArgs(column))
    End Sub

    Protected Overrides Sub OnSorting(eventArgs As DataGridSortingEventArgs)
        MyBase.OnSorting(eventArgs)

        'Unser Event aufrufen
        OnSorted(eventArgs.Column)
    End Sub

    Protected Overridable Sub OnItemsDeleted(items As IEnumerable)
        RaiseEvent ItemsDeleted(Me, New ItemsDeletedEventArgs() With {.DeletedItems = items})
    End Sub
    Protected Overridable Sub OnItemsDelete(args As ItemsDeletingEventArgs)
        RaiseEvent ItemsDeleting(Me, args)
    End Sub

    Protected Overrides Sub OnPreviewKeyDown(e As KeyEventArgs)

        If MyBase.SelectedItem IsNot Nothing Then
            Dim dgr = DirectCast(MyBase.ItemContainerGenerator.ContainerFromIndex(MyBase.SelectedIndex), DataGridRow)

            If e.Key = Key.Delete AndAlso Not dgr.IsEditing Then
                Dim deletedItems = SelectedItems
                Dim args = New ItemsDeletingEventArgs()

                OnItemsDelete(args)

                e.Handled = args.Cancel
            End If
        End If

        MyBase.OnPreviewKeyDown(e)
    End Sub

    Protected Overrides Sub OnExecutedDelete(e As System.Windows.Input.ExecutedRoutedEventArgs)
        Dim deletedItems As New List(Of Object)()

        For Each item In SelectedItems
            deletedItems.Add(item)
        Next

        MyBase.OnExecutedDelete(e)

        If deletedItems.Count > 0 Then
            'Event werfen, Items wurden geloescht
            OnItemsDeleted(deletedItems)
        End If

    End Sub
End Class
