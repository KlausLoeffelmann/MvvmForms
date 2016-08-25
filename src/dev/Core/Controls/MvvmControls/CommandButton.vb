Imports System.Windows.Input
Imports System.Windows.Forms
Imports System.Drawing
Imports System.ComponentModel

''' <summary>
''' Buttonableitung, welcher eine bindbare Command-Property zur Verfügung stellt
''' </summary>
''' <remarks></remarks>
<ToolboxItem(True), ToolboxBitmap(GetType(Button))>
Public Class CommandButton
    Inherits Button

    Private myCommand As ICommand
    Private myCommandParameter As Object
    Private myImitateTabByPageKeys As Boolean

    Private Const DEFAULT_IMITATE_TAB_BY_PAGE_KEYS = False


    ''' <summary>
    ''' Fired before execution of the actual command.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Public Event BeforeCommandExecution(ByVal sender As Object, ByVal e As EventArgs)

    Sub New()
        MyBase.New
        ImitateTabByPageKeys = NullableControlManager.GetInstance.GetDefaultImitateTabByPageKeys(Me, DEFAULT_IMITATE_TAB_BY_PAGE_KEYS)
    End Sub

    ''' <summary>
    ''' Bindable Command of Type ICommand 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Command As ICommand
        Get
            Return myCommand
        End Get
        Set(ByVal value As ICommand)
            If Not Object.Equals(value, myCommand) Then
                If myCommand IsNot Nothing Then
                    System.Windows.WeakEventManager(Of ICommand, EventArgs).RemoveHandler(
                        myCommand, "CanExecuteChanged", AddressOf OnCommandCanExecuteChanged)
                End If
                myCommand = value
                If myCommand IsNot Nothing Then
                    System.Windows.WeakEventManager(Of ICommand, EventArgs).AddHandler(
                        myCommand, "CanExecuteChanged", AddressOf OnCommandCanExecuteChanged)

                End If
                OnCommandChanged()
            End If
        End Set
    End Property

    ''' <summary>
    ''' Returns or sets if the user can cycle between entry fields with Page up and Page down in addition to Tab and Shift+Tab.
    ''' </summary>
    ''' <returns></returns>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
     Description("Returns or sets if the user can cycle between entry fields with Page up and Page down in addition to Tab and Shift+Tab."),
     Category("Behavior"),
     EditorBrowsable(EditorBrowsableState.Always),
     Browsable(True), DefaultValue(False)>
    Public Property ImitateTabByPageKeys As Boolean
        Get
            Return myImitateTabByPageKeys
        End Get
        Set(value As Boolean)
            If Not Object.Equals(myImitateTabByPageKeys, value) Then
                myImitateTabByPageKeys = value
            End If
        End Set
    End Property


    Protected Overrides Sub OnKeyDown(e As System.Windows.Forms.KeyEventArgs)
        MyBase.OnKeyDown(e)
        If ImitateTabByPageKeys Then
            If e.KeyCode = Keys.Next Then
                SendKeys.SendWait("{TAB}")
                e.SuppressKeyPress = True
            ElseIf e.KeyCode = Keys.PageUp Then
                SendKeys.SendWait("+{TAB}")
                e.SuppressKeyPress = True
            End If
        End If
    End Sub


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
