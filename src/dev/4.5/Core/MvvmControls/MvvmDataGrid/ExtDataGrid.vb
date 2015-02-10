Imports System.Windows.Controls

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

    Protected Overrides Sub OnExecutedDelete(e As Windows.Input.ExecutedRoutedEventArgs)
        Dim oldItems = New List(Of Object)()
        Dim deletedItems = New List(Of Object)()
        Dim currentItems = New List(Of Object)()

        For Each item In MyBase.ItemsSource
            oldItems.Add(item)
        Next

        MyBase.OnExecutedDelete(e)

        For Each item In MyBase.ItemsSource
            currentItems.Add(item)
        Next

        'Diff machen
        For Each oldItem In oldItems
            If Not currentItems.Contains(oldItem) Then
                'Wurde geloescht
                deletedItems.Add(oldItem)
            End If
        Next

        If deletedItems.Count > 0 Then
            'Event werfen, Items wurden geloescht
            OnItemsDeleted(deletedItems)
        End If

    End Sub
End Class
