Imports ActiveDevelop.MvvmBaseLib.FormulaEvaluator

<TestClass()>
Public Class FormulaEvalTest

    <TestMethod()>
    Public Sub FormularEval_SimpleTest()

        Dim formEval = New FormulaEvaluator("3+3*3^3")
        Assert.AreEqual(Of Double)(84, formEval.Result)

        formEval = New FormulaEvaluator("(3+3)*3^3")
        Assert.AreEqual(Of Double)(162, formEval.Result)

        formEval = New FormulaEvaluator("((3+3)*3)^3")
        Assert.AreEqual(Of Double)(5832, formEval.Result)

        formEval.MathExpression = ("3+xvar()*2")
        formEval.X = 3
        Assert.AreEqual(Of Double)(9, formEval.Result)

        formEval.X = 4
        Assert.AreEqual(Of Double)(11, formEval.Result)

        Dim formula = "3+xvar()*2"
        formEval.MathExpression = (formula)
        Assert.AreEqual(Of String)(formula & ": 11", formEval.ToString)

        formula = "3*-2"
        formEval.MathExpression = (formula)
        Assert.AreEqual(Of String)(formula & ": -6", formEval.ToString)

    End Sub

End Class
