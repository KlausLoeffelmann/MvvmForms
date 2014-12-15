Imports System.Windows.Forms
Imports System.Drawing
Imports System.Collections.ObjectModel
Imports System.Threading.Tasks
Imports System.Text

Public Class frmMvvmEventAssignment

    Private myViewModelType As Type = GetType(ContactTest)
    Private myControlToBind As Control = New NullableDateValue With {.Name = "Geburtsdatum"}

    Private myCommandCanExecuteProperties As ObservableCollection(Of BindingProperty)
    Private myControlEvents As ObservableCollection(Of BindingEvent)

    Private myEventBindings As New ObservableBindingList(Of EventBindingItem)

    Property EventBindings As ObservableBindingList(Of EventBindingItem)
        Get
            'Keine Zuordnungen gespeichert, dann Nothing zurückgeben.
            If myEventBindings.Count = 0 Then
                Return Nothing
            Else
                Return myEventBindings
            End If

        End Get
        Set(value As ObservableBindingList(Of EventBindingItem))
            If value Is Nothing Then
                myEventBindings = New ObservableBindingList(Of EventBindingItem)
            Else
                myEventBindings = value
            End If
            EventBindingGrid.GridDataSource.DataSource = myEventBindings
        End Set
    End Property

    Protected Overrides Sub OnLoad(e As EventArgs)
        MyBase.OnLoad(e)

        'If Debugger.IsAttached Then
        '    Debugger.Break()
        'End If

        myViewModelType = If(Me.MvvmManager Is Nothing, Nothing, Me.MvvmManager.DataContextType)
        myControlToBind = Me.ComponentInstance

        If myControlToBind Is Nothing Or myViewModelType Is Nothing Then
            MessageBox.Show("Warning: Null Objects detected, Control oder ViewModel should not be null.",
                            "Null detected", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            myControlToBind = New NullableDateValue With {.Name = "ndvDateOfBirthTesting"}
            myViewModelType = GetType(ContactTest)
        End If

        lblCurrentControl.Text = myControlToBind.Name
        lblCurrentControlType.Text = myControlToBind.GetType.Name
        lblCurrentViewModelType.Text = myViewModelType.Name
        lblCurrentViewModelFullName.Text = myViewModelType.FullName

        Dim initializeTasks = New List(Of Task)

        initializeTasks.Add(InitializeCommandCanExecutePropertiesAsync)
        initializeTasks.Add(InitializeControlEventsAsync)
        Task.WaitAll(initializeTasks.ToArray)

        nvrControlCanExecuteProperty.DataSource = myCommandCanExecuteProperties
        nvrControlEvent.DataSource = myControlEvents
    End Sub

    Private Function InitializeCommandCanExecutePropertiesAsync() As Task

        Dim workerTask = Task.Factory.StartNew(
            Sub()
                myCommandCanExecuteProperties = New ObservableCollection(Of BindingProperty)(
                            (From propItem In myControlToBind.GetType.GetProperties(Reflection.BindingFlags.NonPublic Or
                                                                            Reflection.BindingFlags.Public Or
                                                                            Reflection.BindingFlags.Instance)
                             Where propItem.PropertyType Is GetType(Boolean) AndAlso
                                   propItem.CanWrite
                             Order By propItem.Name Descending
                             Select New BindingProperty With
                             {.PropertyName = propItem.Name,
                              .PropertyType = propItem.PropertyType}))
            End Sub)
        Return workerTask
    End Function

    Private Function InitializeControlEventsAsync() As Task

        Dim workerTask = Task.Factory.StartNew(
            Sub()
                myControlEvents = New ObservableCollection(Of BindingEvent)(
                            (From eventItem In myControlToBind.GetType.GetEvents(Reflection.BindingFlags.NonPublic Or
                                                                            Reflection.BindingFlags.Public Or
                                                                            Reflection.BindingFlags.Instance)
                             Order By eventItem.Name
                             Select New BindingEvent With
                             {.EventName = eventItem.Name}))
            End Sub)
        Return workerTask
    End Function

    Friend Property ComponentInstance As Control
    Friend Property MvvmManager As MvvmManager

End Class
