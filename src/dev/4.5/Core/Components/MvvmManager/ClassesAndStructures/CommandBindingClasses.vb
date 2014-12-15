<Serializable>
Public Class BindingCommand
    Inherits AttributeControlledComparableBase

    Sub New()
        MyBase.New()
    End Sub

    Sub New(commandName As String)
        Me.CommandName = commandName
    End Sub

    <DisplayIndicator(1), EqualityIndicator>
    Property CommandName As String

    Public Function Clone() As BindingCommand
        Return DirectCast(MyBase.MemberwiseClone, BindingCommand)
    End Function

End Class
