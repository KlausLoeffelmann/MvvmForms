Imports ActiveDevelop.EntitiesFormsLib
Imports System.Windows.Forms
Imports System.Threading

<TestClass()>
Public Class SimpleCalcTests

    ''' <summary>
    ''' Testet, ob Zahleneingabe ohne vorherige Operatorangabe auch den neuen Wert übernimmt und nicht konkateniert (Init=42, Eingabe 666+ ->darf nicht zu 42666 werden)
    ''' </summary>
    <TestMethod()>
    Public Sub StartWithValue_EnterValue_Test()
        ' HACK wegen der Exception:
        'Exception thrown 'System.InvalidOperationException' in Microsoft.VisualStudio.TestPlatform.TestExecutor.Core.dll
        'Additional Information: The cancelation of the test run Is Not possible as there Is no test run which Is in progress.
        ' daher nutzen wir ein 
        Dim are As New AutoResetEvent(False)

        Dim res As Decimal = 0
        Dim calc As New SimpleCalculator
        AddHandler calc.SetResult, Sub(s, e)
                                       If e.Action = CalculatorAction.FinalResult Then
                                           res = e.Value
                                           are.Set()
                                       End If
                                   End Sub
        calc.SetInitialValue(42)
        calc.ProcessKeyDown(Keys.D6)
        calc.ProcessKeyDown(Keys.D6)
        calc.ProcessKeyDown(Keys.D6)
        calc.ProcessKeyDown(Keys.Add)
        calc.ForceFinalCalculation()

        are.WaitOne()
        Assert.AreNotEqual(42666D, res)
        Assert.AreEqual(666D, res)
    End Sub


    ''' <summary>
    ''' Testet die Berechnung mit einem Initalwert (und Decimalwerten)
    ''' </summary>
    <TestMethod()>
    Public Sub StartWithValue_Calc_Test()
        Dim res As Decimal = 0
        Dim calc As New SimpleCalculator
        AddHandler calc.SetResult, Sub(s, e)
                                       If e.Action = CalculatorAction.FinalResult Then
                                           res = e.Value
                                       End If
                                   End Sub
        calc.SetInitialValue(42.43D)
        calc.ProcessKeyDown(Keys.Multiply)
        calc.ProcessKeyDown(Keys.D3)
        calc.ForceFinalCalculation()

        Assert.AreEqual(127.29D, res)
    End Sub


    ''' <summary>
    ''' ToggleSign bei Initialwert
    ''' </summary>
    <TestMethod()>
    Public Sub ToggleSignWithInitalvalue_Test()
        Dim res As Decimal = 0
        Dim calc As New SimpleCalculator
        AddHandler calc.SetResult, Sub(s, e)
                                       If e.Action = CalculatorAction.ToggleSign Then
                                           res = e.Value
                                       End If
                                   End Sub
        calc.SetInitialValue(42.43D)
        calc.btnToggleSign_Click(Nothing, EventArgs.Empty)
        calc.ForceFinalCalculation()

        Assert.AreEqual(-42.43D, res)
    End Sub

    ''' <summary>
    ''' ToggleSign bei Berechnungsergebnis
    ''' </summary>
    <TestMethod()>
    Public Sub ToggleSignWithCalcresult_Test()
        Dim res As Decimal = 0
        Dim calc As New SimpleCalculator
        AddHandler calc.SetResult, Sub(s, e)
                                       If e.Action = CalculatorAction.ToggleSign Then
                                           res = e.Value
                                       End If
                                   End Sub
        calc.SetInitialValue(7)
        calc.ProcessKeyDown(Keys.Multiply)
        calc.ProcessKeyDown(Keys.D3)
        calc.ProcessKeyDown(Keys.Add)
        calc.btnToggleSign_Click(Nothing, EventArgs.Empty)
        calc.ForceFinalCalculation()

        Assert.AreEqual(-21D, res)
    End Sub

    ''' <summary>
    ''' ToggleSign bei Operator "7 - (-3)"
    ''' </summary>
    <TestMethod()>
    Public Sub ToggleSignWithOperator_Test()
        Dim res As Decimal = 0
        Dim calc As New SimpleCalculator
        AddHandler calc.SetResult, Sub(s, e)
                                       If e.Action = CalculatorAction.ToggleSign Then
                                           Assert.AreEqual(-3D, e.Value)
                                       End If

                                       If e.Action = CalculatorAction.FinalResult Then
                                           res = e.Value
                                       End If
                                   End Sub
        calc.SetInitialValue(7)
        calc.ProcessKeyDown(Keys.Subtract)
        calc.ProcessKeyDown(Keys.D3)
        calc.btnToggleSign_Click(Nothing, EventArgs.Empty)
        calc.ForceFinalCalculation()

        Assert.AreEqual(10D, res)
    End Sub


    ''' <summary>
    ''' Verändern von Operatoren (=Korrektur)
    ''' </summary>
    <TestMethod()>
    Public Sub ChangeOperatorWithInitialValue_Test()
        Dim res As Decimal = 0
        Dim calc As New SimpleCalculator
        AddHandler calc.SetResult, Sub(s, e)
                                       If e.Action = CalculatorAction.FinalResult Then
                                           res = e.Value
                                       End If
                                   End Sub
        calc.SetInitialValue(7)
        calc.ProcessKeyDown(Keys.Multiply)
        calc.ProcessKeyDown(Keys.D3)
        calc.ForceFinalCalculation()

        Assert.AreEqual(21D, res)
    End Sub


    ''' <summary>
    ''' Verändern von Operatoren (=Korrektur)
    ''' </summary>
    <TestMethod()>
    Public Sub ChangeOperator_Test()
        Dim res As Decimal = 0
        Dim calc As New SimpleCalculator
        AddHandler calc.SetResult, Sub(s, e)
                                       If e.Action = CalculatorAction.FinalResult Then
                                           res = e.Value
                                       End If
                                   End Sub
        calc.SetInitialValue(0)
        calc.ProcessKeyDown(Keys.D7)
        calc.ProcessKeyDown(Keys.Multiply)
        calc.ProcessKeyDown(Keys.Add)
        calc.ProcessKeyDown(Keys.D3)
        calc.ForceFinalCalculation()

        Assert.AreEqual(10D, res)
    End Sub

    ''' <summary>
    ''' Testet die Eingabe von , als erstes Zeichen
    ''' </summary>
    <TestMethod()>
    Public Sub StartWithdecimal_Test()
        Dim res As Decimal = 0
        Dim ir As Decimal = 0
        Dim calc As New SimpleCalculator
        AddHandler calc.SetResult, Sub(s, e)
                                       If e.Action = CalculatorAction.FinalResult Then
                                           res = e.Value
                                       End If
                                       If e.Action = CalculatorAction.Operand Then
                                           Assert.AreEqual(ir, e.Value)
                                       End If
                                   End Sub
        calc.SetInitialValue(0)
        SendDecimalSeperatorKey(calc)
        ir = 0.4D
        calc.ProcessKeyDown(Keys.D4)
        ir = 0.43D
        calc.ProcessKeyDown(Keys.D3)
        calc.ProcessKeyDown(Keys.Delete)
        ir = 0.42D
        calc.ProcessKeyDown(Keys.D2)
        calc.ForceFinalCalculation()

        Assert.AreEqual(0.42D, res)
    End Sub

    ''' <summary>
    ''' Rechenkette
    ''' </summary>
    <TestMethod()>
    Public Sub ComplexCalculation_Test()
        Dim ir As Decimal = 0
        Dim res As Decimal = 0
        Dim calc As New SimpleCalculator
        AddHandler calc.SetResult, Sub(s, e)
                                       If e.Action = CalculatorAction.IntermediaryResult Then
                                           Assert.AreEqual(ir, e.Value)
                                       End If
                                       If e.Action = CalculatorAction.FinalResult Then
                                           res = e.Value
                                       End If
                                   End Sub
        calc.SetInitialValue(31.2D)
        ir = 31.2D
        calc.ProcessKeyDown(Keys.Multiply)
        calc.ProcessKeyDown(Keys.D7)
        SendDecimalSeperatorKey(calc)
        calc.ProcessKeyDown(Keys.D3)
        ir = 227.76D
        calc.ProcessKeyDown(Keys.Subtract)
        calc.ProcessKeyDown(Keys.D2)
        calc.ProcessKeyDown(Keys.D1)
        calc.ProcessKeyDown(Keys.D3)
        SendDecimalSeperatorKey(calc)
        calc.ProcessKeyDown(Keys.D3)
        calc.ProcessKeyDown(Keys.D4)
        calc.ProcessKeyDown(Keys.D5)
        calc.ProcessKeyDown(Keys.D6)
        ir = 14.4144D
        calc.ProcessKeyDown(Keys.Divide)
        calc.ProcessKeyDown(Keys.D2)
        calc.ForceFinalCalculation()

        Assert.AreEqual(7.2072D, res)
    End Sub

    ''' <summary>
    ''' Testet das Löschen von Zeichen
    ''' </summary>
    <TestMethod()>
    Public Sub EraseKey_Test()
        Dim res As Decimal = 0
        Dim calc As New SimpleCalculator
        AddHandler calc.SetResult, Sub(s, e)
                                       If e.Action = CalculatorAction.FinalResult Then
                                           res = e.Value
                                       End If
                                       If e.Action = CalculatorAction.EraseLastChar Then
                                           Assert.AreEqual("4", e.Input)
                                       End If
                                   End Sub
        calc.SetInitialValue(0)
        calc.ProcessKeyDown(Keys.D4)
        calc.ProcessKeyDown(Keys.D3)
        calc.ProcessKeyDown(Keys.Delete)
        calc.ProcessKeyDown(Keys.D2)
        calc.ForceFinalCalculation()

        Assert.AreEqual(42D, res)
    End Sub


    ''' <summary>
    ''' Testet, ob die Löschetaste bei berechneten Werten ohne funktion ist
    ''' </summary>
    <TestMethod()>
    Public Sub EraseKey_OncalculatedValues_Test()
        Dim res As Decimal = 0
        Dim calc As New SimpleCalculator
        AddHandler calc.SetResult, Sub(s, e)
                                       If e.Action = CalculatorAction.FinalResult Then
                                           res = e.Value
                                       End If
                                       If e.Action = CalculatorAction.EraseLastChar Then
                                           Assert.Fail("CalculatorAction.EraseLastChar called")
                                       End If
                                   End Sub
        calc.SetInitialValue(13)
        calc.ProcessKeyDown(Keys.Multiply)
        calc.ProcessKeyDown(Keys.D3)
        calc.ProcessKeyDown(Keys.Add)
        calc.ProcessKeyDown(Keys.D1)
        calc.ProcessKeyDown(Keys.Add)
        calc.ProcessKeyDown(Keys.Delete)
        calc.ForceFinalCalculation()

        Assert.AreEqual(40D, res)
    End Sub



    ''' <summary>
    ''' Testet, ob die Tase "C" = ClearAll funktioniert
    ''' </summary>
    <TestMethod()>
    Public Sub ClearAll_Test()
        Dim res As Decimal = 0
        Dim calc As New SimpleCalculator
        AddHandler calc.SetResult, Sub(s, e)
                                       If e.Action = CalculatorAction.FinalResult Then
                                           res = e.Value
                                       End If
                                   End Sub
        calc.SetInitialValue(13)
        calc.ProcessKeyDown(Keys.Subtract)
        calc.ProcessKeyDown(Keys.D3)
        calc.ProcessKeyDown(Keys.Add)
        calc.btnC_Click(Nothing, EventArgs.Empty)
        calc.ProcessKeyDown(Keys.D1)
        calc.ProcessKeyDown(Keys.D0)
        calc.ProcessKeyDown(Keys.D0)
        calc.ForceFinalCalculation()

        Assert.AreEqual(100D, res)
    End Sub

    ''' <summary>
    ''' Testet, ob die Tase "CE" = ClearEntry funktioniert
    ''' </summary>
    <TestMethod()>
    Public Sub ClearEntry_Test()
        Dim res As Decimal = 0
        Dim calc As New SimpleCalculator
        AddHandler calc.SetResult, Sub(s, e)
                                       If e.Action = CalculatorAction.FinalResult Then
                                           res = e.Value
                                       End If
                                   End Sub
        calc.SetInitialValue(169)
        calc.ProcessKeyDown(Keys.Divide)
        calc.ProcessKeyDown(Keys.D1)
        calc.ProcessKeyDown(Keys.D0)
        calc.ProcessKeyDown(Keys.D0)
        calc.btnCE_Click(Nothing, EventArgs.Empty)
        calc.ProcessKeyDown(Keys.D1)
        calc.ProcessKeyDown(Keys.D3)

        calc.ForceFinalCalculation()

        Assert.AreEqual(13D, res)
    End Sub

    Private Sub SendDecimalSeperatorKey(calc As SimpleCalculator)
        calc.ProcessKeyDown(Keys.Decimal)
    End Sub
End Class
