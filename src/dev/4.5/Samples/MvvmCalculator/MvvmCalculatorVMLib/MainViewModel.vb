Imports ActiveDevelop.MvvmBaseLib.Mvvm
Imports ActiveDevelop.MvvmBaseLib.FormularParser

Public Class MainViewModel
    Inherits MvvmBase

    Private myFormular As String
    Private myCurrentFormular As String
    Private myResult As String
    Private myCalcCommand As RelayCommand
    Private myFormulaEval As FormulaEvaluator

    Sub New()
        myCalcCommand = New RelayCommand(AddressOf CalcCommandProc,
                                         AddressOf CanExecuteCalcCommand)
    End Sub

    Public Property CurrentFormular As String
        Get
            Return myCurrentFormular
        End Get
        Set(value As String)
            SetProperty(myCurrentFormular, value)
        End Set
    End Property

    Public Property EnteredFormular As String
        Get
            Return myFormular
        End Get
        Set(value As String)
            SetProperty(myFormular, value)
        End Set
    End Property

    Public Property Result As String
        Get
            Return myResult
        End Get
        Set(value As String)
            SetProperty(myResult, value)
        End Set
    End Property

    Public Property CalcCommand As RelayCommand
        Get
            Return myCalcCommand
        End Get
        Set(value As RelayCommand)
            SetProperty(myCalcCommand, value)
        End Set
    End Property

    Private Sub CalcCommandProc(param As Object)
        myFormulaEval = New FormulaEvaluator(EnteredFormular)
        Try
            Result = myFormulaEval.Result.ToString
        Catch ex As Exception
            'Todo: Error Handling
        End Try
    End Sub

    Private Function CanExecuteCalcCommand() As Boolean
        Return True
    End Function

End Class
