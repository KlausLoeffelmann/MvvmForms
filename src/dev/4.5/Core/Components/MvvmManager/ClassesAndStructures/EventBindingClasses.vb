Imports System.ComponentModel.Design.Serialization

<Serializable, DesignerSerializer(GetType(PropertyBindingsCodeDomSerializer), GetType(CodeDomSerializer))>
Public Class EventBindings
    Inherits List(Of EventBindingItem)

    Sub New()
        MyBase.New()
    End Sub

    Sub New(enumList As IEnumerable(Of EventBindingItem))
        For Each enumItem In enumList
            Me.Add(enumItem)
        Next
    End Sub

    Public Function ToObservableItemList() As ObservableBindingList(Of EventBindingItem)
        Return New ObservableBindingList(Of EventBindingItem)(Me)
    End Function

    Public Overloads Sub Add(controlEvent As BindingEvent, viewModelCommand As BindingCommand, controlCanExecuteProperty As BindingProperty)

        Dim propBindItem As New EventBindingItem() With
                    {.ControlEvent = controlEvent,
                     .ViewModelCommand = viewModelCommand,
                     .ControlCanExecuteProperty = controlCanExecuteProperty}
        Me.Add(propBindItem)
    End Sub


    Public Function Clone() As EventBindings
        Dim retList As New EventBindings
        Me.ForEach(Sub(item)
                       retList.Add(item.Clone)
                   End Sub)
        Return retList
    End Function
End Class

<Serializable>
Public Class EventBindingItem
    Public Property ControlEvent As BindingEvent
    Public Property ControlCanExecuteProperty As BindingProperty
    Public Property ViewModelCommand As BindingCommand

    Public Function Clone() As EventBindingItem
        Dim copy = DirectCast(MyBase.MemberwiseClone, EventBindingItem)
        'Diese hier müssen extra gecloned werden, da es Referenztypen sind.
        copy.ControlEvent = copy.ControlEvent.Clone
        copy.ViewModelCommand = copy.ViewModelCommand.Clone
        Return copy
    End Function

End Class

<Serializable>
Public Class BindingEvent
    Inherits AttributeControlledComparableBase

    Sub New()
        MyBase.New()
    End Sub

    Sub New(eventName As String)
        Me.EventName = eventName
    End Sub

    <DisplayIndicator(1), EqualityIndicator>
    Property EventName As String

    Public Function Clone() As BindingEvent
        Return DirectCast(MyBase.MemberwiseClone, BindingEvent)
    End Function

End Class


