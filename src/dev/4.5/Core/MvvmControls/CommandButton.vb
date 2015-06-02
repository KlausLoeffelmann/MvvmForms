Imports System.Windows.Input
Imports System.Windows.Forms

''' <summary>
''' Buttonableitung, welcher eine bindbare Command-Property zur Verfügung stellt
''' </summary>
''' <remarks></remarks>
Public Class CommandButton
    Inherits Button

    Private myCommand As ICommand

    ''' <summary>
    ''' Bindbarer Command vom Typ ICommand 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Command As ICommand
        Get
            Return myCommand
        End Get
        Set(ByVal value As ICommand)
            myCommand = value
            OnCommandChanged()
            If myCommand IsNot Nothing Then
                Windows.WeakEventManager(Of ICommand, EventArgs).AddHandler(
                        myCommand, "CanExecuteChanged", AddressOf OnCommandCanExecuteChanged)

            End If
        End Set
    End Property

    Private myCommandParameter As Object
    Public Property CommandParameter() As Object
        Get
            Return myCommandParameter
        End Get
        Set(ByVal value As Object)
            myCommandParameter = value
        End Set
    End Property

    ''' <summary>
    ''' Hier wird der Command NACH Click-Event aufgerufen
    ''' </summary>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Overrides Sub OnClick(e As EventArgs)
        MyBase.OnClick(e)
        If myCommand IsNot Nothing AndAlso myCommand.CanExecute(Nothing) Then
            Me.OnBeforeCommandExecution(EventArgs.Empty)
            myCommand.Execute(myCommandParameter)
            Me.OnAfterCommandExecution(EventArgs.Empty)
        End If
    End Sub

    Protected Overridable Sub OnCommandChanged()
        OnCommandCanExecuteChanged(Me, EventArgs.Empty)
    End Sub

    Private Sub OnCommandCanExecuteChanged(sender As Object, e As EventArgs)
        If myCommand IsNot Nothing Then
            Me.Enabled = myCommand.CanExecute(Nothing)
        End If
    End Sub

    ''' <summary>
    ''' Wird aufgerufen, vor der Ausführung des Commands
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Public Event BeforeCommandExecution(ByVal sender As Object, ByVal e As EventArgs)

    ''' <summary>
    ''' Führt das BeforeCommandExecution-Event aus
    ''' </summary>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Overridable Sub OnBeforeCommandExecution(e As EventArgs)
        RaiseEvent BeforeCommandExecution(Me, e)
    End Sub

    ''' <summary>
    ''' Wird aufgerufen, sobald der gebundene Command aufgerufen wurde
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Public Event AfterCommandExecution(ByVal sender As Object, ByVal e As EventArgs)

    ''' <summary>
    ''' Führt das AfterCommandExecution-Event aus
    ''' </summary>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Overridable Sub OnAfterCommandExecution(e As EventArgs)
        RaiseEvent AfterCommandExecution(Me, e)
    End Sub
End Class
