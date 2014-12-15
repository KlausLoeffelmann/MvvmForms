Imports System.Windows.Input

Public Class CommandBase
    Implements ICommand

    Private myCommand As Action(Of Object)
    Private myCanExecuteTester As Func(Of Object, Boolean)

    Public Event CanExecuteChanged(sender As Object, e As EventArgs) Implements ICommand.CanExecuteChanged

    Sub New(command As Action(Of Object), canExecuteTester As Func(Of Object, Boolean))
        myCommand = command
        myCanExecuteTester = canExecuteTester
    End Sub

    Private Function CanExecute(parameter As Object) As Boolean Implements ICommand.CanExecute
        If myCanExecuteTester IsNot Nothing Then
            Return myCanExecuteTester.Invoke(parameter)
        End If
        Return False
    End Function

    Public Sub Execute(parameter As Object) Implements ICommand.Execute
        If myCommand IsNot Nothing Then
            myCommand.Invoke(parameter)
        End If
    End Sub

End Class
