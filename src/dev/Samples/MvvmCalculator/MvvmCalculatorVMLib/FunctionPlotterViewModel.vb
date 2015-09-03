Imports ActiveDevelop.MvvmBaseLib
Imports ActiveDevelop.MvvmBaseLib.Mvvm
Imports ActiveDevelop.MvvmBaseLib.FormulaEvaluator

Public Class FunctionPlotterViewModel
    Inherits BindableBase

    Private myRenderSize As Size
    Private myPointsToPlot As List(Of Point)
    Private myStartRange As Double?
    Private myEndRange As Double?
    Private myScale As Size
    Private myOffset As Size
    Private myFunction As String

    Private myValueCount As Integer = 50

    Private myCalculateCommand As RelayCommand = New RelayCommand(
        AddressOf CalculateCommandProc,
        Function() True)
    Private myErrorText As String

    Public Property RenderSize As Size
        Get
            Return myRenderSize
        End Get
        Set(value As Size)
            SetProperty(myRenderSize, value)
        End Set
    End Property

    Public Property PointsToPlot As List(Of Point)
        Get
            Return myPointsToPlot
        End Get
        Set(value As List(Of Point))
            SetProperty(myPointsToPlot, value)
        End Set
    End Property

    Public Property StartRange As Double?
        Get
            Return myStartRange
        End Get
        Set(value As Double?)
            SetProperty(myStartRange, value)
        End Set
    End Property

    Public Property EndRange As Double?
        Get
            Return myEndRange
        End Get
        Set(value As Double?)
            SetProperty(myEndRange, value)
        End Set
    End Property

    Public Property Scale As Size
        Get
            Return myScale
        End Get
        Set(value As Size)
            SetProperty(myScale, value)
        End Set
    End Property

    Public Property Offset As Size
        Get
            Return myOffset
        End Get
        Set(value As Size)
            SetProperty(myOffset, value)
        End Set
    End Property

    Public Property [Function] As String
        Get
            Return myFunction
        End Get
        Set(value As String)
            SetProperty(myFunction, value)
        End Set
    End Property

    Public Property CalculateCommand As RelayCommand
        Get
            Return myCalculateCommand
        End Get
        Set(value As RelayCommand)
            SetProperty(myCalculateCommand, value)
        End Set
    End Property

    Public Property ErrorText As String
        Get
            Return myErrorText
        End Get
        Set(value As String)
            SetProperty(myErrorText, value)
        End Set
    End Property

    Private Sub CalculateCommandProc(param As Object)
        If String.IsNullOrEmpty([Function]) Then
            ErrorText = "Please, enter a function!"
        End If

        If Not StartRange.HasValue Or Not EndRange.HasValue Then
            ErrorText = "Please, enter the full range."
        End If


        Dim formulaEval = New FormulaEvaluator([Function])
        Dim xvar As Double

        formulaEval.Functions.Add(New FormulaEvaluatorFunction("xvar", Function() xvar, 0))

        Dim stepScale As Double = ((EndRange.Value - StartRange.Value) / myValueCount)
        xvar = StartRange.Value
        Dim yVar As Double

        Dim resultList As New List(Of Point)

        For i = 0 To myValueCount - 1
            Try
                yVar = formulaEval.Result
            Catch ex As Exception
            End Try

            resultList.Add(New Point() With {.X = xvar, .y = yVar})

            xvar = xvar + stepScale
        Next

        Me.PointsToPlot = resultList
    End Sub

End Class

Public Structure Point
    Public Property X As Double
    Public Property y As Double
End Structure

Public Structure Size
    Public Property Width As Double
    Public Property Height As Double
End Structure