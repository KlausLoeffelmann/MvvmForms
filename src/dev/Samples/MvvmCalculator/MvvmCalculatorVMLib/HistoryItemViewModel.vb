Imports ActiveDevelop.MvvmBaseLib
Imports ActiveDevelop.MvvmBaseLib.FormulaEvaluator
Imports ActiveDevelop.MvvmBaseLib.Mvvm

Public Class HistoryItemViewModel
    Inherits MvvmViewModelBase

    Private myFormula As FormulaEvaluator

    Public Sub New(formula As FormulaEvaluator)
        OriginalFormula = formula
    End Sub

    Public Property Result As String
        Get
            Return myFormula.Result.ToString
        End Get
        Set(value As String)
            Throw New NotImplementedException("Setting this value is not supported.")
        End Set
    End Property

    Public Property Formula As String
        Get
            Return myFormula.MathExpression
        End Get
        Set(value As String)
            Throw New NotImplementedException("Setting this value is not supported.")
        End Set
    End Property

    Public Property OriginalFormula As FormulaEvaluator
        Get
            Return myFormula
        End Get
        Set(value As FormulaEvaluator)
            If SetProperty(myFormula, value) Then
                OnPropertyChanged(NameOf(Formula))
                OnPropertyChanged(NameOf(Result))
            End If
        End Set
    End Property

End Class
