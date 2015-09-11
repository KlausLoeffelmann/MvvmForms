Imports ActiveDevelop.IoC.Generic
Imports ActiveDevelop.MvvmBaseLib
Imports ActiveDevelop.MvvmBaseLib.FormulaEvaluator
Imports ActiveDevelop.MvvmBaseLib.Mvvm

Public Class MainViewModel
    Inherits BindableBase

    Private myFormular As String
    Private myCurrentFormular As String
    Private myResult As String
    Private myFormulaEval As FormulaEvaluator
    Private myHistoryItems As New ObservableCollection(Of HistoryItemViewModel)
    Private myErrorText As String
    Private mySelectedFormula As HistoryItemViewModel
    Private mySelectedFormulaIndex As Integer = -1
    Private myPreviousContent As String

    Private myCalcCommand As RelayCommand
    Private myClearListCommand As RelayCommand
    Private myCallFunctionPlotterCommand As RelayCommand

    Private myPlatformServiceLocator As Func(Of IMvvmPlatformServiceLocator)
    'The lifetimecontroller for all the other instances created by this Viewmodel, which need to persist during its lifetime.
    Private myLifetimeInstanceController As IIoCLifetimeController

    Public Sub New()
    End Sub

    ''' <summary>
    ''' Initializes a new instance of this ViewModel.
    ''' </summary>
    Public Sub New(platformServiceLocator As Func(Of IMvvmPlatformServiceLocator))
        myCalcCommand = New RelayCommand(AddressOf CalcCommandProc,
                                         AddressOf CanExecuteCalcCommand)

        myClearListCommand = New RelayCommand(AddressOf ClearListProc,
                                            Function() True)

        myCallFunctionPlotterCommand = New RelayCommand(AddressOf CallFunctionPlotterProc,
                                                      Function() True)

        myPlatformServiceLocator = platformServiceLocator
        myLifetimeInstanceController = myPlatformServiceLocator().GetLifetimeController

    End Sub


    ''' <summary>
    ''' Represents the math term as it is currently in the editable field.
    ''' </summary>
    ''' <returns></returns>
    Public Property EnteredFormula As String
        Get
            Return myFormular
        End Get
        Set(value As String)
            If SetProperty(myFormular, value) Then
                If String.IsNullOrEmpty(myPreviousContent) = Not String.IsNullOrEmpty(value) Then
                    myPreviousContent = value
                    CalcCommand.RaiseCanExecuteChanged()
                End If
            End If
        End Set
    End Property

    Public Property Formulas As ObservableCollection(Of HistoryItemViewModel)
        Get
            Return myHistoryItems
        End Get
        Set(value As ObservableCollection(Of HistoryItemViewModel))
            SetProperty(myHistoryItems, value)
        End Set
    End Property

    Public Property SelectedFormula As HistoryItemViewModel
        Get
            Return mySelectedFormula
        End Get
        Set(value As HistoryItemViewModel)
            If SetProperty(mySelectedFormula, value) Then
                EnteredFormula = mySelectedFormula?.Formula
                Result = mySelectedFormula?.Result
            End If
        End Set
    End Property

    ''' <summary>
    ''' Represents the result displayed on top of the entry field after the calculation of the math term.
    ''' </summary>
    ''' <returns></returns>
    Public Property Result As String
        Get
            Return myResult
        End Get
        Set(value As String)
            SetProperty(myResult, value)
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the command for starting the calculating process.
    ''' </summary>
    ''' <remarks>
    ''' This is just the property which provides the command, so it can be bound.
    ''' It is not the code which is executed, when the user invokes the actual command!
    ''' </remarks>
    ''' <returns></returns>
    Public Property CalcCommand As RelayCommand
        Get
            Return myCalcCommand
        End Get
        Set(value As RelayCommand)
            SetProperty(myCalcCommand, value)
        End Set
    End Property

    Public Property ClearListCommand As RelayCommand
        Get
            Return myClearListCommand
        End Get
        Set(value As RelayCommand)
            SetProperty(myClearListCommand, value)
        End Set
    End Property

    Public Property CallFunctionPlotterCommand As RelayCommand
        Get
            Return myCallFunctionPlotterCommand
        End Get
        Set(value As RelayCommand)
            SetProperty(myCallFunctionPlotterCommand, value)
        End Set
    End Property

    Public Property ErrorText As String
        Get
            Return myErrorText
        End Get
        Set(value As String)
            If SetProperty(myErrorText, value) Then
                If Not String.IsNullOrEmpty(value) Then
                    Result = ErrorText
                End If
            End If
        End Set
    End Property

    ''' <summary>
    ''' Method, which is executed, when the related command is invoked.
    ''' </summary>
    ''' <param name="param"></param>
    Private Sub CalcCommandProc(param As Object)
        myFormulaEval = New FormulaEvaluator(EnteredFormula)
        Try
            Result = myFormulaEval.Result.ToString
            'This is an error!
            ErrorText = Nothing
            Me.Formulas.Add(New HistoryItemViewModel(myFormulaEval))
        Catch ex As Exception
            ErrorText = ex.Message
        End Try
    End Sub

    ''' <summary>
    ''' Method, which finds out, if the command can be executed at the moment.
    ''' </summary>
    ''' <returns></returns>
    Private Function CanExecuteCalcCommand() As Boolean
        Return Not String.IsNullOrWhiteSpace(EnteredFormula)
    End Function

    Private Async Sub ClearListProc(param As Object)
        Dim dialogResult = Await myLifetimeInstanceController.
            GetMessageBoxService.
                ShowMessageBoxAsync("Are you sure you want to clear the list?",
                                    "Clear list",
                                     MvvmMessageBoxButtons.YesNo,
                                     MvvmMessageBoxDefaultButton.Button2,
                                     MvvmMessageBoxIcon.Question)

        If dialogResult = MvvmDialogResult.Yes Then
            Me.Formulas = New ObservableCollection(Of HistoryItemViewModel)
        End If

    End Sub

    Private Sub CallFunctionPlotterProc(param As Object)
        Dim functionPlotterVm = myLifetimeInstanceController.Resolve(Of FunctionPlotterViewModel)

        myLifetimeInstanceController.GetNavigationService.NavigateTo(functionPlotterVm)
    End Sub

End Class


