Imports System.Windows.Input
Imports ActiveDevelop.EntitiesFormsLib.ViewModelBase

''' <summary>
''' Stellt die Command-Funktionalität für bindbare Commands in einem MVVM-ViewModel zur Verfügung.
''' </summary>
''' <remarks></remarks>
<MvvmSystemElement>
Public Class MvvmCommandBase
    Inherits BindableBase
    Implements ICommand

    Private myOnExecute As System.Action(Of Object)
    Private myCanExecute As System.Func(Of Object, Boolean)
    Private myCanExecuteState As Boolean = False
    Private myUseDefaultCanExecuteLogic As Boolean = False

    Public Event CanExecuteChanged(sender As Object, e As EventArgs) Implements ICommand.CanExecuteChanged

    Sub New(onExecute As Action(Of Object))

        If onExecute Is Nothing Then
            Throw New ArgumentException("The onExecute delegate passed to this instance must no be null (nothing in VB)!")
        End If
        myOnExecute = onExecute
        myCanExecute = Function(param As Object) As Boolean
                           Return CanExecuteState
                       End Function
        myUseDefaultCanExecuteLogic = True

    End Sub

    Sub New(ByVal onExecute As Action(Of Object),
            ByVal onCanExecute As Func(Of Object, Boolean))
        myOnExecute = onExecute
        myCanExecute = onCanExecute
    End Sub

    Public Overridable Function CanExecute(parameter As Object) As Boolean Implements ICommand.CanExecute
        Dim tmpOldExecuteState = CanExecuteState
        myCanExecuteState = If(myCanExecute Is Nothing, myOnExecute IsNot Nothing, myCanExecute(parameter))
        If Not (myCanExecuteState And tmpOldExecuteState) Then
            OnCanExecuteChanged(EventArgs.Empty)
        End If
        Return CanExecuteState
    End Function

    Public Overridable Sub Execute(parameter As Object) Implements ICommand.Execute
        If myOnExecute IsNot Nothing Then
            myOnExecute(parameter)
        End If
    End Sub

    Public Property CanExecuteState As Boolean
        Get
            Return myCanExecuteState
        End Get
        Set(value As Boolean)
            If Not myUseDefaultCanExecuteLogic Then
                Throw New ArgumentException("Don't use the setter with custom CanExecute logic.")
            End If

            If Not Object.Equals(value, myCanExecuteState) Then
                MyBase.SetProperty(myCanExecuteState, value)
                OnCanExecuteChanged(EventArgs.Empty)
            End If
        End Set
    End Property

    Protected Overridable Sub OnCanExecuteChanged(e As EventArgs)
        RaiseEvent CanExecuteChanged(Me, e)
    End Sub

    Public Property CanExecuteProc As Func(Of Object, Boolean)
    Public Property ExecuteProc As Action(Of Object)

End Class
