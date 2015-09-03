Imports ActiveDevelop.MvvmBaseLib
Imports ActiveDevelop.MvvmBaseLib.Mvvm
Imports ActiveDevelop.MvvmBaseLib.FormulaEvaluator

Public Class FunctionPlotterViewModel
    Inherits BindableBase

    Private myRenderSize As Size?
    Private myPointsToPlot As ObservableCollection(Of Point)
    Private myStartRange As Double?
    Private myEndRange As Double?
    Private myScaling As Size?
    Private myFunction As String

    Private myValueCount As Integer = 50

    Private myCalculateCommand As RelayCommand = New RelayCommand(
        AddressOf CalculateCommandProc,
        Function() True)
    Private myErrorText As String

    Public Property RenderSize As Size?
        Get
            Return myRenderSize
        End Get
        Set(value As Size?)
            If SetProperty(myRenderSize, value) Then
                OnRenderSizedChanged()
            End If
        End Set
    End Property

    Protected Overridable Sub OnRenderSizedChanged()
        Dim minX, maxX As Double
        Dim minY, maxY As Double

        If PointsToPlot IsNot Nothing Then

            For c As Integer = 0 To PointsToPlot.Count - 1
                minX = Math.Min(minX, PointsToPlot(c).X)
                minY = Math.Min(minY, PointsToPlot(c).Y)
                maxX = Math.Max(maxX, PointsToPlot(c).X)
                maxY = Math.Max(maxY, PointsToPlot(c).Y)
            Next
        End If

        minX = Math.Min(minX, -minX)
        minY = Math.Min(minY, -minY)
        maxX = Math.Max(maxX, -maxX)
        maxY = Math.Max(maxY, -maxY)

        If RenderSize.HasValue Then
            Me.Scaling = New Size() With {.Width = (maxX - minX) / RenderSize.Value.Width,
                                        .Height = (maxY - minY) / RenderSize.Value.Height}

        End If
    End Sub

    Public Property PointsToPlot As ObservableCollection(Of Point)
        Get
            Return myPointsToPlot
        End Get
        Set(value As ObservableCollection(Of Point))
            If SetProperty(myPointsToPlot, value) Then
                OnRenderSizedChanged()
            End If
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

    Public Property Scaling As Size?
        Get
            Return myScaling
        End Get
        Set(value As Size?)
            SetProperty(myScaling, value)
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

        Dim stepScale As Double = ((EndRange.Value - StartRange.Value) / myValueCount)
        xvar = StartRange.Value
        Dim yVar As Double

        Dim resultList As New ObservableCollection(Of Point)

        For i = 0 To myValueCount - 1

            formulaEval.X = xvar

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
    Public Property Y As Double
    Public Overrides Function ToString() As String
        Return $"(X:{X}),(Y:{Y})"
    End Function
End Structure

Public Structure Size
    Public Property Width As Double
    Public Property Height As Double

    Public Overrides Function ToString() As String
        Return $"(Width:{Width}),(Height:{Height})"
    End Function
End Structure