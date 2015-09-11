
Imports System.Collections.ObjectModel
Imports System.Runtime.CompilerServices

Friend Module ObservaleCollectionExtender
    <Extension>
    Public Sub AddRange(Of T)(collection As ObservableCollection(Of T), range As IEnumerable(Of T))
        For Each item In range
            collection.Add(item)
        Next
    End Sub

    <Extension>
    Public Sub AddRange(Of T)(collection As ObservableCollection(Of T), range As IEnumerable(Of T), index As Integer)
        For Each item In range
            collection.Insert(index, item)
        Next
    End Sub
End Module