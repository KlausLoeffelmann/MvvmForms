Imports System.ComponentModel

Public Interface INotifyAttachOrDetach(Of T As INotifyAttachOrDetach(Of T))
    Sub NotifyAttached()
    Sub NotifyDetached()
    Property CorrelatingNode As LinkedListNode(Of T)
    ReadOnly Property HasPredecessor As Boolean
    Property ParentLinkedList As ChangeAwareLinkedList(Of T)
End Interface

Public Class ChangeAwareLinkedList(Of t As INotifyAttachOrDetach(Of t))

    Private myElements As New LinkedList(Of t)

    Public Sub Add(newItem As t)
        Dim tmpNewNode = New LinkedListNode(Of t)(newItem)
        newItem.CorrelatingNode = tmpNewNode
        myElements.AddLast(tmpNewNode)
        newItem.ParentLinkedList = Me
        newItem.NotifyAttached()
    End Sub

    Public Sub AddAfter(oldItem As t, newItem As t)
        Dim tmpNewNode = New LinkedListNode(Of t)(newItem)
        newItem.CorrelatingNode = tmpNewNode
        myElements.AddAfter(oldItem.CorrelatingNode, tmpNewNode)
        newItem.ParentLinkedList = Me
        newItem.NotifyAttached()
    End Sub

    Public Sub AddBefore(oldItem As t, newItem As t)
        Dim tmpNewNode = New LinkedListNode(Of t)(newItem)
        newItem.CorrelatingNode = tmpNewNode
        myElements.AddBefore(oldItem.CorrelatingNode, tmpNewNode)
        newItem.ParentLinkedList = Me
        newItem.NotifyAttached()
    End Sub

    Public Sub Remove(oldItem As t)
        myElements.Remove(oldItem.CorrelatingNode)
        oldItem.ParentLinkedList = Nothing
        oldItem.NotifyDetached()
    End Sub

    Public Sub RemoveAllFromInclusive(oldItem As t)
        If oldItem.CorrelatingNode.Previous Is Nothing Then
            RemoveAll()
        Else
            Dim currentItem = oldItem.CorrelatingNode.Previous.Value
            Do While currentItem.CorrelatingNode.Next IsNot Nothing
                Dim nodeToRemove = currentItem.CorrelatingNode.Next.Value
                myElements.Remove(currentItem.CorrelatingNode.Next)
                nodeToRemove.CorrelatingNode = Nothing
                nodeToRemove.ParentLinkedList = Nothing
                nodeToRemove.NotifyDetached()
            Loop
        End If
    End Sub

    Public Sub RemoveAll()
        Do
            Dim firstNode = myElements.First
            If firstNode Is Nothing Then
                Exit Do
            End If
            Dim t = firstNode.Value
            If t Is Nothing Then
                Exit Do
            End If
            Remove(t)
        Loop
    End Sub
End Class
