Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports MvvmCalculatorVMLib

<TestClass()> Public Class FunctionPlotterTest

    <TestMethod()> Public Sub TestSimpleFunction()
        Dim FunctionPlotterVM As New FunctionPlotterViewModel With
                {
                    .StartRange = -10,
                    .EndRange = 10,
                    .Function = "sin(xvar)",
                    .Scale = New Size With {.Width = 800, .Height = 600}
                }

        FunctionPlotterVM.CalculateCommand.Execute(Nothing)



    End Sub

End Class