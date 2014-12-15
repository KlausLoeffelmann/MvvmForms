Imports System.Collections.ObjectModel
Imports System.Runtime.CompilerServices
Imports System.ComponentModel

Public Module BindableBaseExtender

    <Extension>
    Public Function ToObservableCollection(Of t)(list As IEnumerable(Of t)) As ObservableCollection(Of t)
        Return New ObservableCollection(Of t)(list)
    End Function

End Module
