Imports System.Globalization
Imports System.Threading.Tasks
Imports System.Windows.Forms
Public Class SimpleCalculator

    Private Declare Function HideCaret Lib "user32" (ByVal wHandle As Int32) As Int32
    Private Declare Function ShowCaret Lib "user32" (ByVal wHandle As Int32) As Int32

    Public Shared Sub HideCaret(tb As TextBox)
        HideCaret(tb.Handle.ToInt32)
    End Sub

    Public Shared Sub ShowCaret(tb As TextBox)
        ShowCaret(tb.Handle.ToInt32)
    End Sub


    Public Const CO_PLUS_OPERATOR As String = "+"
    Public Const CO_MINUS_OPERATOR As String = "-"
    Public Const CO_DIVIDE_OPERATOR As String = "/"
    Public Const CO_MULTIPLY_OPERATOR As String = "*"
    Public Const CO_EQUAL_OPERATOR As String = "="

    Public Event SetResult As EventHandler(Of CalculatorSetResultEventArgs)

    Private myLastResult As Decimal = 0    ' Last (temp) Result
    Private myCurrentOperand As String
    Private myDecimalSeparator As String
    Private myCalcEngine As New NullableNumValueFormatterEngine(0)
    Private myFormular As String
    Private myUseBuildInCalcResultWindow As Boolean = True


    Public Property UseBuildInCalcResultWindow As Boolean
        Get
            Return myUseBuildInCalcResultWindow
        End Get
        Set(value As Boolean)
            myUseBuildInCalcResultWindow = value
        End Set
    End Property

    Public Sub SetInitialValue(val As Decimal)
        SetResultInternal(New CalculatorSetResultEventArgs(CalculatorAction.Init, val))
    End Sub


    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        btnPlus.KeyVal = CO_PLUS_OPERATOR
        btnMinus.KeyVal = CO_MINUS_OPERATOR
        btnDiv.KeyVal = CO_DIVIDE_OPERATOR
        btnMulti.KeyVal = CO_MULTIPLY_OPERATOR
        btnCalc.KeyVal = CO_EQUAL_OPERATOR
        myCalcEngine.IsFormularAllowed = True
        myDecimalSeparator = Globalization.CultureInfo.CurrentUICulture.NumberFormat.NumberDecimalSeparator
        btnDecimalSeparator.Text = myDecimalSeparator
    End Sub

    Protected Overrides Sub OnLoad(e As EventArgs)
        MyBase.OnLoad(e)
        SetResultInternal(New CalculatorSetResultEventArgs(CalculatorAction.Init, myLastResult))
        SplitContainer1.Panel1Collapsed = Not myUseBuildInCalcResultWindow
    End Sub

    Private Sub btnDigit_Click(sender As Object, e As EventArgs) Handles btn0.Click, btn1.Click, btn2.Click, btn3.Click, btn4.Click, btn5.Click, btn6.Click, btn7.Click, btn8.Click, btn9.Click
        Dim digit = DirectCast(sender, CalculatorButton).KeyVal

        'Debug.WriteLine("Ziffer: {0}", digit)

        Dim curr As Decimal = 0
        If Decimal.TryParse(myCurrentOperand & digit, curr) Then
            myCurrentOperand = (myCurrentOperand & digit).TrimStart("0"c)
            SetResultInternal(New CalculatorSetResultEventArgs(CalculatorAction.Operand, curr))
        End If
    End Sub

    Public Sub btnCE_Click(sender As Object, e As EventArgs) Handles btnCE.Click
        ' Clear entry
        SetResultInternal(New CalculatorSetResultEventArgs(CalculatorAction.ClearEntry))
    End Sub

    public Sub btnC_Click(sender As Object, e As EventArgs) Handles btnC.Click
        ' Clear All
        SetResultInternal(New CalculatorSetResultEventArgs(CalculatorAction.ClearAll))
    End Sub

    Private Sub btnDel_Click(sender As Object, e As EventArgs) Handles btnDel.Click
        ' erase last char
        ' this can be also done in an other Textbox (e.g. when used in a Popup from a nullablenumvalue
        Dim curr As Decimal = 0
        Dim len = myCurrentOperand.Length
        If len > 0 Then

            If Decimal.TryParse(myCurrentOperand.Substring(0, len - 1), curr) Then
                myCurrentOperand = myCurrentOperand.Substring(0, len - 1)
            ElseIf len = 1 Then
                myCurrentOperand = "0"
            End If
        Else
            Return
        End If
        SetResultInternal(New CalculatorSetResultEventArgs(CalculatorAction.EraseLastChar, curr))
    End Sub

    Private Sub btnDecimalSeparator_Click(sender As Object, e As EventArgs) Handles btnDecimalSeparator.Click
        If myCurrentOperand.Contains(myDecimalSeparator) Then Return        ' only one is allowed / changing DE<>EN  (which may create multiple separators) at runtime is not supportet

        Dim newVal = myCurrentOperand

        If newVal.Trim.Length = 0 Then
            newVal = "0"      'Zero length means 0.
        End If
        myCurrentOperand &= myDecimalSeparator
        calcResult.Text = myCurrentOperand
        SetResultInternal(New CalculatorSetResultEventArgs(CalculatorAction.DecimalSeparator))
    End Sub

    Private Sub btnOperator_Click(sender As Object, e As EventArgs) Handles btnDiv.Click, btnMulti.Click, btnPlus.Click, btnMinus.Click, btnCalc.Click
        Dim op = DirectCast(sender, CalculatorButton).KeyVal
        If op Is Nothing Then Return

        SetResultInternal(New CalculatorSetResultEventArgs(CalculatorAction.Operator, op))
    End Sub

    Protected Sub OnSetResult(e As CalculatorSetResultEventArgs)
        Debug.WriteLine("OnSetResult '{0}', Operator= '{1}', Value = '{2}'", e.Action, e.Operator, e.Value)
        RaiseEvent SetResult(Me, e)
    End Sub

    Private Sub SetResultInternal(e As CalculatorSetResultEventArgs)
        If e.Action = CalculatorAction.Init OrElse e.Action = CalculatorAction.ClearAll Then
            myLastResult = e.Value
            calcResult.Text = e.Value.ToString
            e.Input = e.Value.ToString
            myFormular = e.Value.ToString
            myCurrentOperand = ""
        End If
        If e.Action = CalculatorAction.ClearEntry Then
            myCurrentOperand = "0"
            calcResult.Text = myCurrentOperand
            e.Input = myCurrentOperand
        ElseIf e.Action = CalculatorAction.Operand Then
            'Debug.WriteLine("Operand: {0}", e.Value.ToString)
            calcResult.Text = e.Value.ToString
            e.Input = e.Value.ToString
        ElseIf e.Action = CalculatorAction.IntermediaryResult OrElse
            e.Action = CalculatorAction.FinalResult OrElse
            e.Action = CalculatorAction.EraseLastChar OrElse
            e.Action = CalculatorAction.ToggleSign Then

            calcResult.Text = e.Value.ToString
            e.Input = e.Value.ToString
        ElseIf e.Action = CalculatorAction.DecimalSeparator Then
            e.Input = myCurrentOperand
        ElseIf e.Action = CalculatorAction.Operator Then

            Try
                If String.IsNullOrWhiteSpace(myCurrentOperand) Then
                    ' operand is missing
                    ' maybe due change of the operator
                    ' + and then changing to - for example
                Else
                    ' calculate
                    Dim res As Decimal? = Nothing
                    If myFormular = myLastResult.ToString Then
                        ' no operator
                        ' so replace value
                        myFormular = myCurrentOperand
                        res = myCalcEngine.ConvertToValue(myFormular)
                    Else
                        ' regular calculation
                        res = myCalcEngine.ConvertToValue(myFormular & " " & myCurrentOperand)
                    End If
                    If res.HasValue Then
                        myLastResult = res.Value
                    Else
                        myLastResult = 0
                    End If
                End If


                'myFormular &= myCalcEngine.ConvertToDisplay 
                Dim op2append = ""
                If e.Operator <> CO_EQUAL_OPERATOR Then
                    op2append = " " & e.Operator
                End If
                myFormular = myLastResult.ToString & op2append
                myCurrentOperand = ""
                e.Input = ""
            Catch ex As Exception
                Beep()
            End Try
        End If
        OnSetResult(e)
        If e.Action = CalculatorAction.Operator Then
            Dim a = CalculatorAction.IntermediaryResult
            If e.Operator = CO_EQUAL_OPERATOR Then a = CalculatorAction.FinalResult

            SetResultInternal(New CalculatorSetResultEventArgs(a, myLastResult))
        End If
    End Sub

    Public Sub btnToggleSign_Click(sender As Object, e As EventArgs) Handles btnToggleSign.Click
        Dim d As Decimal
        If String.IsNullOrWhiteSpace(myCurrentOperand) Then
            'Debug.WriteLine("Toggle CurrentOperand=null, LastVal:" & myLastResult)
            ' there is no given (new) operand
            ' so it has to be the last result
            Dim newVal = myLastResult * -1

            SetResultInternal(New CalculatorSetResultEventArgs(CalculatorAction.ToggleSign, newVal))
            myLastResult = newVal

        ElseIf Decimal.TryParse(myCurrentOperand, d) Then
            'Debug.WriteLine("Toggle CurrentOperand:" & myCurrentOperand)
            Dim newVal = d * -1
            SetResultInternal(New CalculatorSetResultEventArgs(CalculatorAction.ToggleSign, newVal))
            myCurrentOperand = newVal.ToString

        End If
    End Sub

    Private Sub calcResult_KeyPress(sender As Object, e As KeyPressEventArgs) Handles calcResult.KeyPress
        Dim numberFormatInfo As NumberFormatInfo = System.Globalization.CultureInfo.CurrentCulture.NumberFormat
        Dim decimalSeparator As String = numberFormatInfo.NumberDecimalSeparator
        Dim groupSeparator As String = numberFormatInfo.NumberGroupSeparator
        Dim negativeSign As String = numberFormatInfo.NegativeSign

        Dim keyInput As String = e.KeyChar.ToString()

        If [Char].IsDigit(e.KeyChar) Then
            ' Digits are OK
        ElseIf keyInput.Equals(decimalSeparator) Then
            ' Decimal separator is OK
        ElseIf e.KeyChar = vbBack Then
            ' Backspace key is OK
            '    else if ((ModifierKeys & (Keys.Control | Keys.Alt)) != 0)
            '    {
            '     // Let the edit control handle control and alt key combinations
            '    }
        ElseIf keyInput = "+" OrElse keyInput = "-" OrElse keyInput = "*" OrElse keyInput = "/" Then        ' OrElse keyInput.Equals(negativeSign) Then
            'ElseIf Char.  Then        ' OrElse keyInput.Equals(negativeSign) Then
            SetResultInternal(New CalculatorSetResultEventArgs(CalculatorAction.Operator, keyInput))
        Else
            ' Consume this invalid key and beep.
            e.Handled = True
        End If
    End Sub

    Private Sub calcResult_KeyDown(sender As Object, e As KeyEventArgs) Handles calcResult.KeyDown
        'Console.WriteLine("{0} - kd {1}, kc {2}", e.Modifiers, e.KeyData, e.KeyCode)
        'e.KeyCode = Keys.Oemplus OrElse e.KeyCode = Keys.OemMinus OrElse
        If e.KeyData = Keys.Multiply OrElse e.KeyData = Keys.Divide OrElse e.KeyCode = Keys.Add OrElse e.KeyCode = Keys.Subtract Then
            e.SuppressKeyPress = True
        End If
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True
        End If
        If e.KeyCode = Keys.NumPad0 OrElse e.KeyCode = Keys.D0 Then
            If calcResult.Text.StartsWith("0") Then e.SuppressKeyPress = True         ' 000 wollen wir nicht -> eine null reicht
        End If
    End Sub

    Public Sub ForceFinalCalculation()
        SetResultInternal(New CalculatorSetResultEventArgs(CalculatorAction.Operator, CO_EQUAL_OPERATOR))
    End Sub

    Protected Overrides Function ProcessCmdKey(ByRef msg As Message, keyData As Keys) As Boolean
        'Console.WriteLine(" ProcessCmdKey {0}, {1}", msg, keyData)
        ProcessKeyDown(keyData)

        Return MyBase.ProcessCmdKey(msg, keyData)
    End Function

    Public Function ProcessKeyDown(keyData As Keys) As Boolean
        'Console.WriteLine("ProcessKeyDown {0}", keyData)
        Dim btn As CalculatorButton = Nothing
        Select Case keyData
            Case Keys.D0, Keys.NumPad0
                btn = btn0
            Case Keys.D1, Keys.NumPad1
                btn = btn1
            Case Keys.D2, Keys.NumPad2
                btn = btn2
            Case Keys.D3, Keys.NumPad3
                btn = btn3
            Case Keys.D4, Keys.NumPad4
                btn = btn4
            Case Keys.D5, Keys.NumPad5
                btn = btn5
            Case Keys.D6, Keys.NumPad6
                btn = btn6
            Case Keys.D7, Keys.NumPad7
                btn = btn7
            Case Keys.D8, Keys.NumPad8
                btn = btn8
            Case Keys.D9, Keys.NumPad9
                btn = btn9
            Case Keys.Add, Keys.Oemplus
                btn = btnPlus
            Case Keys.Subtract, Keys.OemMinus
                btn = btnMinus
            Case Keys.Multiply
                btn = btnMulti
            Case Keys.Divide
                btn = btnDiv
            Case Keys.Enter
                btn = btnCalc
            Case Keys.Delete, Keys.Back
                btn = btnDel
            Case Keys.Oemcomma, Keys.Decimal
                btn = btnDecimalSeparator
            Case Else
        End Select

        Dim clickhandler As EventHandler = Nothing
        Select Case keyData
            Case Keys.D0, Keys.NumPad0,
             Keys.D1, Keys.NumPad1,
             Keys.D2, Keys.NumPad2,
             Keys.D3, Keys.NumPad3,
             Keys.D4, Keys.NumPad4,
             Keys.D5, Keys.NumPad5,
             Keys.D6, Keys.NumPad6,
             Keys.D7, Keys.NumPad7,
             Keys.D8, Keys.NumPad8,
             Keys.D9, Keys.NumPad9
                clickhandler = AddressOf btnDigit_Click
            Case Keys.Add, Keys.Oemplus,
            Keys.Subtract, Keys.OemMinus,
            Keys.Multiply,
            Keys.Divide,
            Keys.Enter
                clickhandler = AddressOf btnOperator_Click
            Case Keys.Delete, Keys.Back
                clickhandler = AddressOf btnDel_Click
            Case Keys.Oemcomma, Keys.Decimal
                clickhandler = AddressOf btnDecimalSeparator_Click
            Case Else
        End Select


        'Debug.WriteLine("Keydata : {0}, btn {1}", keyData, If(btn Is Nothing, "", btn.Name))

        If btn IsNot Nothing AndAlso clickhandler IsNot Nothing Then
            btn.SimulateUIClick()
            clickhandler(btn, EventArgs.Empty)
        End If
        Return True
    End Function

End Class


Public Class CalculatorSetResultEventArgs

    Private myAction As CalculatorAction
    Private myValue As Decimal
    Private myOperator As String
    Private myInput As String

    Public Sub New(action As CalculatorAction)
        myAction = action
        myValue = 0
        myOperator = Nothing
    End Sub

    Public Sub New(action As CalculatorAction, value As Decimal)
        If action = CalculatorAction.Operator Then Throw New ArgumentException("wrong ctor")
        myAction = action
        myValue = value
        myOperator = Nothing
    End Sub

    Public Sub New(action As CalculatorAction, [operator] As String)
        If action <> CalculatorAction.Operator Then Throw New ArgumentException("wrong ctor")
        myAction = action
        myValue = 0
        myOperator = [operator]
    End Sub


    Public ReadOnly Property Action As CalculatorAction
        Get
            Return myAction
        End Get
    End Property

    Public ReadOnly Property [Operator] As String
        Get
            Return myOperator
        End Get
    End Property

    Public ReadOnly Property Value As Decimal
        Get
            Return myValue
        End Get
    End Property

    Public Property Input As String
        Get
            Return myInput
        End Get
        Set(value As String)
            myInput = value
        End Set
    End Property
End Class

Public Enum CalculatorAction
    Init
    Operand
    [Operator]
    IntermediaryResult
    FinalResult         ' Betätigung von =
    ClearAll
    ClearEntry
    EraseLastChar
    ToggleSign
    DecimalSeparator
End Enum

Public Class CalculatorButton
    Inherits CheckBox

    Public Sub New()
        MyBase.New
        Me.Appearance = Appearance.Button
        Me.AutoSize = False
        Me.AutoCheck = False
        Me.TextAlign = Drawing.ContentAlignment.MiddleCenter
        SetStyle(ControlStyles.Selectable, False)
    End Sub

    Public Property KeyVal As String

    Protected Overrides Sub OnMouseDown(mevent As MouseEventArgs)
        If mevent.Button = MouseButtons.Left Then
            Me.Checked = True
        End If
        MyBase.OnMouseDown(mevent)
    End Sub

    Protected Overrides Sub OnClick(e As EventArgs)
        MyBase.OnClick(e)
    End Sub

    Protected Overrides Sub OnMouseUp(mevent As MouseEventArgs)
        MyBase.OnMouseUp(mevent)
        If mevent.Button = MouseButtons.Left Then
            Me.Checked = False
        End If
        MyBase.OnMouseUp(mevent)
    End Sub


    Public Sub SimulateUIClick()
        Me.Checked = True
        Dim t = Task.Delay(200)
        t.ContinueWith(Sub()
                           Me.Checked = False
                       End Sub, TaskScheduler.FromCurrentSynchronizationContext)
    End Sub

End Class

