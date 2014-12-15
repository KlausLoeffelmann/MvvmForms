Imports System.Collections.ObjectModel

''' <summary>
''' Stellt die Basis-Funktionalität für ViewModels dar. ViewModels müssen nicht zwingend von dieser Klasse 
''' ergben, es reicht das Einbinden des IMvvmViewModel-Interfaces.
''' </summary>
''' <remarks></remarks>
<MvvmViewModel, MvvmSystemElement>
Public MustInherit Class MvvmViewModelBase
    Inherits BindableBase
    Implements IMvvmViewModel, IMvvmViewModelNotifyBindingProcess

    Private myPendingChangedProperties As New List(Of String)

    ''' <summary>
    ''' Ereignis, das ein ViewModel auslöst, damit ein entsprechender, einem ViewModel zugeordneter Dialog 
    ''' modal dargestellt werden kann.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Public Event RequestModalView(sender As Object, e As RequestViewEventArgs) Implements IMvvmViewModel.RequestModalView

    ''' <summary>
    ''' Ereignis, das ein ViewModel auslöst, damit ein entsprechender, einem ViewModel zugeordneter Dialog 
    ''' dargestellt werden kann (non-modal).
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Public Event RequestView(sender As Object, e As RequestViewEventArgs) Implements IMvvmViewModel.RequestView

    ''' <summary>
    ''' Ereignis, das ein ViewModel auslöst, damit es einen entsprechenden Message-Dialog darstellen kann.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Public Event RequestMessageDialog(sender As Object, e As RequestMessageDialogEventArgs) Implements IMvvmViewModel.RequestMessageDialog

    Protected Sub New()
        MyBase.New()
        InitializeViewModel()
    End Sub

    ''' <summary>
    ''' Wird direkt nach dem Konstruktor aufgerufen, und bietet dem ViewModel die Gelegenheit, seine Initialisierungen durchzuführen.
    ''' </summary>
    ''' <remarks></remarks>
    Protected MustOverride Sub InitializeViewModel() Implements IMvvmViewModel.InitializeViewModel

    Protected Overridable Sub OnRequestModalView(e As RequestViewEventArgs)
        RaiseEvent RequestModalView(Me, e)
    End Sub

    Protected Overridable Sub OnRequestView(e As RequestViewEventArgs)
        RaiseEvent RequestView(Me, e)
    End Sub

    Protected Overridable Sub OnRequestMessageDialog(e As RequestMessageDialogEventArgs)
        RaiseEvent RequestMessageDialog(Me, e)
    End Sub

    Public Sub ShowViewByViewModel(viewModel As IMvvmViewModel)
        Dim e As New RequestViewEventArgs(viewModel)
        OnRequestView(e)
    End Sub

    Public Function ShowViewByViewModelModal(viewModel As IMvvmViewModel) As MvvmMessageBoxReturnValue
        Dim e As New RequestViewEventArgs(viewModel)
        OnRequestModalView(e)
        Return e.DialogResult
    End Function

    Public Function ShowMessageDialog(message As String,
                    Optional messageBoxTitle As String = Nothing,
                    Optional messageBoxEventButtons As MvvMessageBoxEventButtons = MvvMessageBoxEventButtons.OK,
                    Optional messageBoxIcon As MvvmMessageBoxIcon = MvvmMessageBoxIcon.None) As MvvmMessageBoxReturnValue

        Dim mesBoxeArgs As New RequestMessageDialogEventArgs(message, messageBoxTitle,
                                                             messageBoxEventButtons,
                                                             messageBoxIcon)
        OnRequestMessageDialog(mesBoxeArgs)
        Return mesBoxeArgs.MessageBoxReturnValue
    End Function

    Protected Overrides Sub OnPropertyChanged(Optional propertyName As String = Nothing)
        myPendingChangedProperties.Add(propertyName)
        MyBase.OnPropertyChanged(propertyName)
        myPendingChangedProperties.Remove(propertyName)
    End Sub

    Public ReadOnly Property PendingChangedProperties As IEnumerable(Of String)
        Get
            Return myPendingChangedProperties
        End Get
    End Property

    Protected Overridable Sub BeginBinding() Implements IMvvmViewModelNotifyBindingProcess.BeginBinding
        MyBase.SuspendPropertyChanges()
    End Sub

    Protected Overridable Sub EndBinding() Implements IMvvmViewModelNotifyBindingProcess.EndBinding
        MyBase.ResumePropertyChanges()
    End Sub
End Class
