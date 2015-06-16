Imports ActiveDevelop.MvvmBaseLib.Mvvm
Imports ActiveDevelop.MvvmBaseLib.FormulaEvaluator

Public Class MainViewModel
    Inherits MvvmBase

    Private myFormular As String
    Private myCurrentFormular As String
    Private myResult As String
    Private myCalcCommand As RelayCommand
    Private myFormulaEval As FormulaEvaluator
    Private myFormulas As New ObservableCollection(Of FormulaEvaluator)
    Private myErrorText As String
    Private mySelectedFormula As FormulaEvaluator

    ''' <summary>
    ''' Initializes a new instance of this ViewModel.
    ''' </summary>
    Sub New()
        myCalcCommand = New RelayCommand(AddressOf CalcCommandProc,
                                         AddressOf CanExecuteCalcCommand)
    End Sub

    ''' <summary>
    ''' Represents the current math term which was calculated at last.
    ''' </summary>
    ''' <returns></returns>
    Public Property CurrentFormular As String
        Get
            Return myCurrentFormular
        End Get
        Set(value As String)
            SetProperty(myCurrentFormular, value)
        End Set
    End Property

    ''' <summary>
    ''' Represents the math term as it is currently in the editable field.
    ''' </summary>
    ''' <returns></returns>
    Public Property EnteredFormula As String
        Get
            Return myFormular
        End Get
        Set(value As String)
            SetProperty(myFormular, value)
        End Set
    End Property

    Public Property Formulas As ObservableCollection(Of FormulaEvaluator)
        Get
            Return myFormulas
        End Get
        Set(value As ObservableCollection(Of FormulaEvaluator))
            SetProperty(myFormulas, value)
        End Set
    End Property

    Public Property SelectedFormula As FormulaEvaluator
        Get
            Return mySelectedFormula
        End Get
        Set(value As FormulaEvaluator)
            If SetProperty(mySelectedFormula, value) Then
                EnteredFormula = value.MathExpression
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

    Public Property ErrorText As String
        Get
            Return myErrorText
        End Get
        Set(value As String)
            If SetProperty(myErrorText, value) Then
                Result = ErrorText
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
            ErrorText = Nothing
            Me.Formulas.Add(myFormulaEval)
        Catch ex As Exception
            ErrorText = ex.Message
        End Try
    End Sub

    ''' <summary>
    ''' Method, which finds out, if the command can be executed at the moment.
    ''' </summary>
    ''' <returns></returns>
    Private Function CanExecuteCalcCommand() As Boolean
        Return True
    End Function

End Class
