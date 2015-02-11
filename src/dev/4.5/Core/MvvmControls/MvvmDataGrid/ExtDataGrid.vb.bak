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
End Class
