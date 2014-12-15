''' <summary>
''' Implementiert 
''' </summary>
''' <typeparam name="t"></typeparam>
''' <remarks></remarks>
Public Interface INotifyEditState(Of t)

    Sub BeginAdding(parent As IList(Of t))
    Sub CancelNew(parent As IList(Of t))
    Sub EndNew(parent As IList(Of t))

    Property IsLoading As Boolean

End Interface

