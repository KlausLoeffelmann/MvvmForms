Imports System.ComponentModel

<Obsolete("Conceptional. Don't use!")>
Public MustInherit Class PropertyAssignmentAttributeBase
    Inherits Attribute
End Class

<Obsolete("Conceptional. Don't use!")>
Public Class HighPriorityAssignmentAttribute
    Inherits PropertyAssignmentAttributeBase
End Class

<Obsolete("Conceptional. Don't use!")>
Public Class LowPriorityAssignmentAttribute
    Inherits PropertyAssignmentAttributeBase
End Class

''' <summary>
''' Idea: This is converter, which takes care of indirectly binding a ViewModel to the DataContext on first assignment. 
''' This way, a copy of the Model is created, the properties of the original are copied in here in a certain order.
''' Unwanted backfiring of the initial binding process (VM->ListBox DataSource, ListBox DataSource->SelectedItem, 
''' SelectedItem->VM=null) could thus be prevented.
''' </summary>
''' <typeparam name="t"></typeparam>
''' <remarks></remarks>
<Obsolete("Conceptional. Don't use!")>
Public Class DataContextAssigner(Of t As {New, INotifyPropertyChanged})

    Private myClonedInstance As t
    Private myOriginalInstance As t
    Private myBurned As Boolean

    Sub New(ByRef originalInstance As t)
        Dim tmpOriginalInstance = originalInstance
        myClonedInstance = CloneHighAndNoPriorityInternal(originalInstance)
        originalInstance = myClonedInstance
        myOriginalInstance = tmpOriginalInstance
    End Sub

    Private Function CloneHighAndNoPriorityInternal(instance As t) As t
        Dim returnType = New t

        Return returnType
    End Function

    Private Sub AssignLowPriorityInternal(original As t, target As t)

    End Sub

    Public ReadOnly Property DataContext As Object
        Get
            If myBurned Then
                Throw New ObjectDisposedException("Object is not really disposed, but has the burned status set to true, thus can't be reused.")
            End If
            Return myClonedInstance
        End Get
    End Property

    Public Sub Complete()
        AssignLowPriorityInternal(myOriginalInstance, myClonedInstance)
        myBurned = True
    End Sub

    Public ReadOnly Property IsBurned As Boolean
        Get
            Return myBurned
        End Get
    End Property

End Class
