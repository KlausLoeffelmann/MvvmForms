﻿Imports System.ComponentModel
Imports System.Windows.Forms
Imports ActiveDevelop.EntitiesFormsLib

''' <summary>
''' Allows editing of numerical values (Decimal), which can be retrieved/set over the Value property, 
''' and which can handle Nothing (null in CSharp) on top.
''' </summary>
<Description("Allows interactive editing of numerical values (Decimal) which can be retrieved/set over the Value property," &
             "and which can handle Nothing (null in CSharp) on top."),
 Designer("ActiveDevelop.EntitiesFormsLib.TextBoxBasedControlDesigner")>
Public Class NullableNumValue
    Inherits NullableValueBase(Of Decimal, NullableValuePrimalUpDownControl)

    Private myDecimalPlaces As Integer
    Private myHasThousandsSeperator As Boolean
    Private myCurrencySymbolString As String
    Private myCurrencySymbolUpFront As Boolean
    Private myAllowFormular As Boolean
    Private myLeadingZeros As Integer
    Private myCalculatorPopup As ResizablePopup      ' wir halten das Popup des Calculators
    Private myCalculatorCloseCounter As Integer = 0    ' the Calculator throught the second enter key press


    Private Const DEFAULT_MAX_VALUE_EXCEEDED_MESSAGE As String = "Der eingegebenene Wert überschreitet das Maximum!"
    Private Const DEFAULT_MIN_VALUE_EXCEEDED_MESSAGE As String = "Der eingegebene Wert unterschreitet das Minimum!"
    Private ReadOnly DEFAULT_CALCULATOR_MODE As CalculatorType = CalculatorType.None
    Private ReadOnly DEFAULT_CALCULATOR_TRIGGER As CalculatorActivationTrigger = CalculatorActivationTrigger.None

    Private ReadOnly DEFAULT_MIN_VALUE As Decimal? = 0
    Private ReadOnly DEFAULT_MAX_VALUE As Decimal? = Nothing
    Private ReadOnly DEFAULT_ALLOW_FORMULAR As Boolean = True

    Private ReadOnly DEFAULT_INCREMENT As Decimal? = 1

    Private myIncrement As Decimal?
    Private myMinValue As Decimal?
    Private myMaxValue As Decimal?
    Private myDropDownCalculatorTrigger As CalculatorActivationTrigger

    Sub New()
        MyBase.New()
        Me.MinValue = NullableControlManager.GetInstance.GetDefaultMinValue(Me, DEFAULT_MIN_VALUE)
        Me.MaxValue = NullableControlManager.GetInstance.GetDefaultMaxValue(Me, DEFAULT_MAX_VALUE)

        'TODO: Do we need to take the relationship between those into account?
        Me.DropDownCalculatorMode = NullableControlManager.GetInstance.GetDefaultCalculatorMode(Me, DEFAULT_CALCULATOR_MODE)
        Me.DropDownCalculatorTrigger = NullableControlManager.GetInstance.GetDefaultCalculatorTrigger(Me, DEFAULT_CALCULATOR_TRIGGER)

        Me.AllowFormular = NullableControlManager.GetInstance.GetDefaultAllowFormular(Me, DEFAULT_ALLOW_FORMULAR)

        Me.Increment = NullableControlManager.GetInstance.GetDefaultIncrement(Me, DEFAULT_INCREMENT)
        Me.MaxValueExceededMessage = NullableControlManager.GetInstance.GetDefaultMaxValueExceededMessage(Me, DEFAULT_MAX_VALUE_EXCEEDED_MESSAGE)
        Me.MinValueExceededMessage = NullableControlManager.GetInstance.GetDefaultMinValueExceededMessage(Me, DEFAULT_MIN_VALUE_EXCEEDED_MESSAGE)

        AddHandler Me.ReadOnlyChanged, AddressOf myReadOnlyChanged

        'Wirering up the event which blocks alpha keys when no Formula is allowed.
        AddHandler Me.TextBoxPart.KeyPress, AddressOf TextBoxPartKeyPressHandler

        AddHandler Me.ValueControl.ButtonAction,
            Sub(sender As Object, e As ButtonActionEventArgs)
                Try
                    Dim v = Value
                Catch ex As Exception
                    Beep()
                    Return
                End Try

                Try
                    'Anzeigen, dass Value durch den Benutzer geändert wird, damit beim ValueChange
                    'der korrekte ValueChangeReason übergeben wird.
                    MyBase.BeginForceValueChangeCauseToUser()

                    If Increment.HasValue Then
                        If Value.HasValue Then
                            If e.ButtonType = UpDownComboButtonIDs.Down Then
                                If MinValue.HasValue Then
                                    If Value - Increment >= MinValue Then
                                        MyBase.SetValuePreserveOriginalValue(CType(Value - Increment, Decimal?))
                                        MyBase.TagDirtyState()
                                    Else
                                        Beep()
                                    End If
                                Else
                                    MyBase.SetValuePreserveOriginalValue(CType(Value - Increment, Decimal?))
                                    MyBase.TagDirtyState()
                                End If
                            Else
                                If MaxValue.HasValue Then
                                    If Value + Increment <= MaxValue Then
                                        MyBase.SetValuePreserveOriginalValue(CType(Value + Increment, Decimal?))
                                        MyBase.TagDirtyState()
                                    Else
                                        Beep()
                                    End If
                                Else
                                    MyBase.SetValuePreserveOriginalValue(CType(Value + Increment, Decimal?))
                                    MyBase.TagDirtyState()
                                End If
                            End If

                        Else
                            If e.ButtonType = UpDownComboButtonIDs.Down Then
                                MyBase.SetValuePreserveOriginalValue(If(MaxValue.HasValue, MaxValue, 0))
                            ElseIf e.ButtonType = UpDownComboButtonIDs.Up Then
                                MyBase.SetValuePreserveOriginalValue(If(MinValue.HasValue, MinValue, 0))
                            End If
                            MyBase.TagDirtyState()
                        End If
                    End If
                Finally
                    'BeginChangeValueInternally abschließen.
                    MyBase.EndValueChangeCauseToUser()
                End Try
            End Sub
    End Sub

    'Handels the TextBoxPartKeyPress event and prevent letters when AllowFormular is false.
    Private Sub TextBoxPartKeyPressHandler(sender As Object, e As KeyPressEventArgs)
        If Not AllowFormular Then
            If Char.IsNumber(e.KeyChar) Or
               Char.IsControl(e.KeyChar) Or
               e.KeyChar.Equals("-"c) Or
               e.KeyChar.Equals("e"c) Or
               e.KeyChar.Equals(","c) Or
               e.KeyChar.Equals("."c) Then
            Else
                e.Handled = True
            End If
        End If
    End Sub

    Protected Overrides Function ProcessCmdKey(ByRef msg As Message, keyData As Keys) As Boolean

        'Necessary, because otherwise those characters would be processed/blocked by this, even if the calculator is open.
        'This would lead to e.g. * - / + being swallowed rather than performing an actual arithmetic operation.
        If myCalculatorPopup Is Nothing OrElse Not myCalculatorPopup.IsOpen Then

            Dim toBetriggered As Boolean = False

            toBetriggered = DropDownCalculatorTrigger.HasFlag(CalculatorActivationTrigger.Ctrl_R) And
                            keyData = (Keys.Control Or Keys.R)

            toBetriggered = toBetriggered Or DropDownCalculatorTrigger.HasFlag(CalculatorActivationTrigger.Cursor_UpOrDown) And
                            (keyData = Keys.Down Or keyData = Keys.Up)

            toBetriggered = toBetriggered Or DropDownCalculatorTrigger.HasFlag(CalculatorActivationTrigger.F2) And
                            (keyData = Keys.F2)

            toBetriggered = toBetriggered Or DropDownCalculatorTrigger.HasFlag(CalculatorActivationTrigger.F3) And
                            (keyData = Keys.F3)

            toBetriggered = toBetriggered Or DropDownCalculatorTrigger.HasFlag(CalculatorActivationTrigger.F5) And
                            (keyData = Keys.F5)

            toBetriggered = toBetriggered Or DropDownCalculatorTrigger.HasFlag(CalculatorActivationTrigger.F6) And
                            (keyData = Keys.F6)

            toBetriggered = toBetriggered Or DropDownCalculatorTrigger.HasFlag(CalculatorActivationTrigger.BasicArithmeticKeys) And
                            (keyData = Keys.Multiply Or keyData = Keys.Divide Or keyData = Keys.Add)

            toBetriggered = toBetriggered Or DropDownCalculatorTrigger.HasFlag(CalculatorActivationTrigger.MinusSign) And
                            (keyData = Keys.Subtract)

            toBetriggered = toBetriggered Or DropDownCalculatorTrigger.HasFlag(CalculatorActivationTrigger.Letter_C) And
                            (keyData = Keys.C)


            If toBetriggered Then
                If DropDownCalculatorMode = CalculatorType.Simple Then
                    ToggleCalculator()
                    Return True
                End If
            End If
        End If

        Return MyBase.ProcessCmdKey(msg, keyData)
    End Function

    Private Sub myReadOnlyChanged(sender As Object, e As EventArgs)
        MyBase.ValueControl.Enabled = Not Me.ReadOnly
    End Sub

    Protected Overrides Sub InitializeProperties()
        DecimalPlaces = -1
        HasThousandsSeperator = True
        CurrencySymbolString = ""
        AllowFormular = True
        DropDownCalculatorMode = CalculatorType.None
        DropDownCalculatorTrigger = CalculatorActivationTrigger.None
    End Sub

    Protected Overrides Function IsMultiLineControl() As Boolean
        Return False
    End Function

    Protected Overrides Function GetDefaultFormatterEngine() As INullableValueFormatterEngine
        Dim retTmp = New NullableNumValueFormatterEngine(Me.Value, Me.GetDefaultFormatString, Me.NullValueString)
        Return (retTmp)
    End Function

    Protected Overrides Function GetDefaultNullValueString() As String
        Return NullableControlManager.GetInstance.GetDefaultNullValueString(Me, DEFAULT_NULL_VALUE_STRING)
    End Function

    Protected Overrides Function GetDefaultFormatString() As String
        Dim ret As String
        If LeadingZeros > 0 Then
            ret = New String("0"c, LeadingZeros)
        Else
            If HasThousandsSeperator Then
                ret = "#,##0"
            Else
                ret = "###0"
            End If
        End If

        If DecimalPlaces = -1 Then
            ret &= ".########################"
        ElseIf DecimalPlaces > 0 Then
            ret &= "."
            ret &= New String("0"c, DecimalPlaces)
        End If
        Return ret
    End Function

    Private Function CreateFormatString() As String
        Return GetDefaultFormatString()
    End Function

    ''' <summary>
    ''' Behandelt die Beachtung Min-/und Max-Eigenschaften bei der Wertevalidierung.
    ''' </summary>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Overrides Sub OnValueValidating(e As NullableValueValidationEventArgs(Of Decimal?))
        MyBase.OnValueValidating(e)
        If Me.MaxValue.HasValue AndAlso e.Value.HasValue Then
            If MaxValue.Value < e.Value Then
                e.ValidationFailedUIMessage = MaxValueExceededMessage
                Return
            End If
        End If

        If Me.MinValue.HasValue AndAlso e.Value.HasValue Then
            If MinValue.Value > e.Value.Value Then
                e.ValidationFailedUIMessage = MinValueExceededMessage
            End If
        End If
    End Sub

    Protected Overrides Function GetFailedValidationException(ex As System.Exception) As ContainsUIMessageException
        Return New ContainsUIMessageException(ex.Message, "Der eingegebene Text konnte nicht in einen Fließkommazahlenwert umgewandelt werden.", ex)
    End Function

    ''' <summary>
    ''' Bestimmt oder ermittelt, ob das Währungszeichen voran (true) oder nachgestellt werden soll.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
     Description("Bestimmt oder ermittelt, ob das Währungszeichen voran (true) oder nachgestellt werden soll."),
     Category("Verhalten"),
     EditorBrowsable(EditorBrowsableState.Advanced),
     Browsable(True), DefaultValue(False)>
    Property CurrencySymbolUpFront As Boolean
        Get
            Return myCurrencySymbolUpFront
        End Get
        Set(ByVal value As Boolean)
            If value <> myCurrencySymbolUpFront Then
                myCurrencySymbolUpFront = value
                Me.FormatString = Me.CreateFormatString
                DirectCast(Me.FormatterEngine, NullableNumValueFormatterEngine).CurrencySymbolUpFront = value
                MyBase.UpdateValue()
            End If
        End Set
    End Property

    ''' <summary>
    ''' Bestimmt oder ermittelt das Währungszeichen, das dem Betrag voran- oder nachgestellt werden soll.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
     Description("Bestimmt oder ermittelt das Währungszeichen, das dem Betrag voran- oder nachgestellt werden soll."),
     Category("Verhalten"),
     EditorBrowsable(EditorBrowsableState.Advanced),
     Browsable(True), DefaultValue("")>
    Property CurrencySymbolString As String
        Get
            Return myCurrencySymbolString
        End Get
        Set(ByVal value As String)
            If value <> myCurrencySymbolString Then
                myCurrencySymbolString = value
                Me.FormatString = Me.CreateFormatString
                DirectCast(Me.FormatterEngine, NullableNumValueFormatterEngine).CurrencySymbolString = value
                MyBase.UpdateValue()
            End If
        End Set
    End Property

    ''' <summary>
    ''' Bestimmt oder ermittelt, ob das Tausendertrennzeichen bei Zahlen über 999 (unter -999) angezeigt werden soll.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
     Description("Bestimmt oder ermittelt, ob das Tausendertrennzeichen bei Zahlen über 999 (<-999) angezeigt werden soll."),
     Category("Verhalten"),
     EditorBrowsable(EditorBrowsableState.Advanced),
     Browsable(True), DefaultValue(True)>
    Property HasThousandsSeperator As Boolean
        Get
            Return myHasThousandsSeperator
        End Get
        Set(ByVal value As Boolean)
            myHasThousandsSeperator = value
            Me.FormatString = Me.CreateFormatString
            MyBase.UpdateValue()
        End Set
    End Property

    ''' <summary>
    ''' Bestimmt oder ermittelt die Anzahl der Dezimalstellen, die bei Brüchen angezeigt werden sollen.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
     Description("Bestimmt oder ermittelt die Anzahl der Dezimalstellen, die bei Brüchen angezeigt werden sollen."),
     Category("Verhalten"),
     EditorBrowsable(EditorBrowsableState.Advanced),
     Browsable(True), DefaultValue(-1)>
    Property DecimalPlaces As Integer
        Get
            Return myDecimalPlaces
        End Get
        Set(ByVal value As Integer)
            myDecimalPlaces = value
            Me.FormatString = Me.CreateFormatString
            MyBase.UpdateValue()
        End Set
    End Property

    ''' <summary>
    ''' Bestimmt oder ermittelt, um welchen Wert der angegebene Wert mit den Up-/Down-Schaltflächen erhöht oder verkleinert werden soll.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
     Description("Bestimmt oder ermittelt, um welchen Wert der angegebene Wert mit den Up-/Down-Schaltflächen erhöht oder verkleinert werden soll."),
     Category("Verhalten"),
     EditorBrowsable(EditorBrowsableState.Always),
     Browsable(True)>
    Property Increment As Decimal?
        Get
            Return myIncrement
        End Get
        Set(value As Decimal?)
            myIncrement = value
            If value.HasValue Then
                Me.ValueControl.HideButtons = False
            Else
                Me.ValueControl.HideButtons = True
            End If
        End Set
    End Property

    Private Function ShouldSerializeIncrement() As Boolean

        'Auf Klaus’ Anweisung hin
        'wurden die nachstehenden Zeilen in diese Zeile geändert.

        'If Not Me.Increment.HasValue Then Return False
        'Return CBool(Increment <> NullableControlManager.GetInstance.GetDefaultIncrement(Me, DEFAULT_INCREMENT))
        Return Not Object.Equals(Me.Increment, NullableControlManager.GetInstance.GetDefaultIncrement(Me, DEFAULT_INCREMENT))

    End Function

    Private Sub ResetIncrement()
        Increment = NullableControlManager.GetInstance.GetDefaultIncrement(Me, DEFAULT_INCREMENT)
    End Sub

    ''' <summary>
    ''' Bestimmt oder ermittelt, ob anstelle von Werten auch mathematische, berechbare Ausdrücke eingegeben werden können (Formeln).
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
     Description("Bestimmt oder ermittelt, ob anstelle von Werten auch mathematische, berechbare Ausdrücke eingegeben werden können (Formeln)."),
     Category("Verhalten"),
     EditorBrowsable(EditorBrowsableState.Advanced),
     Browsable(True), DefaultValue(True)>
    Property AllowFormular As Boolean
        Get
            Return myAllowFormular
        End Get
        Set(ByVal value As Boolean)
            If Not Object.Equals(value, myAllowFormular) Then
                myAllowFormular = value
                DirectCast(Me.FormatterEngine, NullableNumValueFormatterEngine).IsFormularAllowed = value
            End If
        End Set
    End Property

    ''' <summary>
    ''' Bestimmt oder ermittelt, ob der formatierte, anzuzeigende Wert mit der entsprechenden Anzahl führender Nullen aufgefüllt wird.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>HINWEIS: Wenn dieser Eigenschaft ein Wert größer Null zugewiesen wird, und damit führende Nullen angezeigt werden, 
    ''' führt das gleichzeitig dazu, dass die <see cref="HasThousandsSeperator">HasThousandsSeperator</see>-Eigenschaft nicht zur Anwendung kommt.</remarks>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
     Description("Bestimmt oder ermittelt, ob der formatierte, anzuzeigende Wert mit der entsprechenden Anzahl führender Nullen aufgefüllt wird."),
     Category("Verhalten"),
     EditorBrowsable(EditorBrowsableState.Advanced),
     Browsable(True), DefaultValue(0)>
    Property LeadingZeros As Integer
        Get
            Return myLeadingZeros
        End Get
        Set(value As Integer)
            myLeadingZeros = value
            Me.FormatString = Me.CreateFormatString
            MyBase.UpdateValue()
        End Set
    End Property

    ''' <summary>
    ''' Bestimmt oder ermittelt den Maximalwert, der in diesem Steuerelement erfasst werden darf.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
     Description("Bestimmt oder ermittelt den Maximalwert, der in diesem Steuerelement erfasst werden darf."),
     Category("Verhalten"),
     EditorBrowsable(EditorBrowsableState.Advanced),
     Browsable(True)>
    Public Property MaxValue() As Decimal?
        Get
            Return myMaxValue
        End Get
        Set(ByVal value As Decimal?)
            Dim needEvent As Boolean = myMaxValue.HasValue <> value.HasValue OrElse (myMaxValue.HasValue AndAlso myMaxValue.Value.CompareTo(value.Value) <> 0)
            If needEvent Then
                myMaxValue = value
                OnMaxValueChanged(EventArgs.Empty)
            End If
        End Set
    End Property

    Public Event MaxValueChanged As EventHandler

    Protected Sub OnMaxValueChanged(e As EventArgs)
        RaiseEvent MaxValueChanged(Me, e)
    End Sub

    ''' <summary>
    ''' Bestimmt oder ermittelt den Minimalwert, der in diesem Steuerelement erfasst werden darf.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
     Description("Bestimmt oder ermittelt den Minimalwert, der in diesem Steuerelement erfasst werden darf."),
     Category("Verhalten"),
     EditorBrowsable(EditorBrowsableState.Advanced),
     Browsable(True)>
    Public Property MinValue() As Decimal?
        Get
            Return myMinValue
        End Get
        Set(ByVal value As Decimal?)
            Dim needEvent As Boolean = myMinValue.HasValue <> value.HasValue OrElse (myMinValue.HasValue AndAlso myMinValue.Value.CompareTo(value.Value) <> 0)
            If needEvent Then
                myMinValue = value
                OnMinValueChanged(EventArgs.Empty)
            End If
        End Set
    End Property

    Public Event MinValueChanged As EventHandler

    Protected Sub OnMinValueChanged(e As EventArgs)
        RaiseEvent MinValueChanged(Me, e)
    End Sub

    ''' <summary>
    ''' Bestimmt oder ermittelt den Fehlertext, der ausgegebenen wird, wenn der Wert des Steuerelementes den Maximalwert überschreitet.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
     Description("Bestimmt oder ermittelt den Fehlertext, der ausgegebenen wird, wenn der Wert des Steuerelementes den Maximalwert überschreitet."),
     Category("Verhalten"),
     EditorBrowsable(EditorBrowsableState.Advanced),
     Browsable(True)>
    Property MaxValueExceededMessage As String

    Private Function ShouldSerializeMaxValueExceededMessage() As Boolean
        Return MaxValueExceededMessage <> NullableControlManager.GetInstance.GetDefaultMaxValueExceededMessage(Me, DEFAULT_MAX_VALUE_EXCEEDED_MESSAGE)
    End Function

    Private Sub ResetMaxValueExceededMessage()
        MaxValueExceededMessage = NullableControlManager.GetInstance.GetDefaultMaxValueExceededMessage(Me, DEFAULT_MAX_VALUE_EXCEEDED_MESSAGE)
    End Sub


    ''' <summary>
    ''' Bestimmt oder ermittelt den Fehlertext, der ausgegebenen wird, wenn der Wert des Steuerelementes den Minimalwert unterschreitet.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
     Description("Bestimmt oder ermittelt den Fehlertext, der ausgegebenen wird, wenn der Wert des Steuerelementes den Minimalwert unterschreitet."),
     Category("Verhalten"),
     EditorBrowsable(EditorBrowsableState.Advanced),
     Browsable(True)>
    Property MinValueExceededMessage As String

    Private Function ShouldSerializeMinValueExceededMessage() As Boolean
        Return MinValueExceededMessage <> NullableControlManager.GetInstance.GetDefaultMinValueExceededMessage(Me, DEFAULT_MIN_VALUE_EXCEEDED_MESSAGE)
    End Function

    Private Sub ResetMinValueExceededMessage()
        MinValueExceededMessage = NullableControlManager.GetInstance.GetDefaultMinValueExceededMessage(Me, DEFAULT_MIN_VALUE_EXCEEDED_MESSAGE)
    End Sub

    ''' <summary>
    ''' Bestimmt ob und welcher Taschenrechner in dem Control angezeigt werden kann
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
     Description("Bestimmt ob und welcher Taschenrechner in dem Control angezeigt werden kann"),
     Category("Verhalten"),
     EditorBrowsable(EditorBrowsableState.Advanced),
     Browsable(True), DefaultValue(CalculatorType.None)>
    Public Property DropDownCalculatorMode As CalculatorType

    ''' <summary>
    ''' Bestimmt wie der Taschenrechner angezeigt werden kann
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
     Description("Bestimmt wie der Taschenrechner angezeigt werden kann"),
     Category("Verhalten"),
     EditorBrowsable(EditorBrowsableState.Advanced),
     Browsable(True), DefaultValue(CalculatorActivationTrigger.None)>
    Public Property DropDownCalculatorTrigger As CalculatorActivationTrigger
        Get
            Return myDropDownCalculatorTrigger
        End Get
        Set(value As CalculatorActivationTrigger)
            If Not Object.Equals(value, myDropDownCalculatorTrigger) Then
                myDropDownCalculatorTrigger = value
                If myDropDownCalculatorTrigger >= CalculatorActivationTrigger.BasicArithmeticKeys Then
                    AllowFormular = False
                End If
            End If
        End Set
    End Property

    ''' <summary>
    ''' Zeigt den Taschenrechner an oder blendet ihn aus, wenn er bereits angezeigt wird
    ''' </summary>
    Public Sub ToggleCalculator()
        Dim calcWin As SimpleCalculator = Nothing
        If myCalculatorPopup Is Nothing Then
            myCalculatorPopup = New ResizablePopup
            calcWin = New SimpleCalculator
            calcWin.UseBuildInCalcResultWindow = False
            myCalculatorPopup.PopupContentControl = calcWin
        End If
        If myCalculatorPopup.IsOpen Then
            ' das RemoveHandler wird  in der Funktion PopupClosing durchgeführt
            myCalculatorPopup.ClosePopup()
        Else
            myCalculatorCloseCounter = 0
            Dim val As Decimal = 0
            ' vor dem verdrahten der Events erst mal gucken, ob ein gültiger Wert als input für den Taschenrechner vorhanden ist
            Try
                Dim tmpVal = Me.Value
                If tmpVal.HasValue Then
                    val = tmpVal.Value
                End If
            Catch aoore As ArgumentOutOfRangeException
                ' es ist nur min/max falsch
                ' den Wert können wir trotzdem übernehmen
                Debug.WriteLine(aoore.Message)
                Try
                    'Debug.WriteLine("Text:" & Me.TextBoxPart.Text)
                    Dim valEngine = New NullableNumValueFormatterEngine(0, "", "")
                    Dim tmpVal = valEngine.ConvertToValue(Me.TextBoxPart.Text)
                    If tmpVal.HasValue Then val = tmpVal.Value
                    'Console.WriteLine("Val: {0}" , val)
                Catch ex As Exception
                    ' scheint doch ein anderes problem zu sein
                    If BeepOnFailedValidation Then
                        Beep()
                    End If

                    Debug.WriteLine(ex.Message)
                    Return
                End Try
            End Try


            AddHandler Me.TextBoxPart.KeyDown, AddressOf ResendAndSupressKeys
            AddHandler Me.TextBoxPart.KeyUp, AddressOf SupressKeys
            calcWin = DirectCast(myCalculatorPopup.PopupContentControl, SimpleCalculator)
            AddHandler calcWin.SetResult, AddressOf SetResult
            AddHandler myCalculatorPopup.PopupClosing, AddressOf PopupClosing
            AddHandler myCalculatorPopup.PopupCloseRequested, AddressOf PopupCloseRequested

            Dim upDown = FindNullableValuePrimalUpDownControl()
            If upDown IsNot Nothing Then
                upDown.Enabled = False
            End If


            'AddHandler Me.TextBoxPart.EnabledChanged, Sub(x, y)
            '                                              Dim a = 5
            '                                          End Sub
            'Me.ValueControl.Enabled = False
            'Me.TextBoxPart.Enabled = True
            'Me.ValueControl.Visible = False

            SimpleCalculator.HideCaret(Me.TextBoxPart)


            calcWin.SetInitialValue(val)
            myCalculatorPopup.OpenPopup(Me)
        End If
    End Sub

    Private Sub ResendAndSupressKeys(sender As Object, e As KeyEventArgs)
        If myCalculatorPopup Is Nothing OrElse Not myCalculatorPopup.IsOpen Then Return     ' das haette nicht passieren sollen
        Dim calc = DirectCast(myCalculatorPopup.PopupContentControl, SimpleCalculator)
        calc.ProcessKeyDown(e.KeyData)
        SupressKeys(sender, e)
    End Sub
    Private Sub SupressKeys(sender As Object, e As KeyEventArgs)
        'Debug.WriteLine("SupressKeys called for KeyCode='{0}', KeyData='{1}'", e.KeyCode, e.KeyData)
        Dim isTab = (e.KeyCode = Keys.Tab)
        Dim isESC = (e.KeyCode = Keys.Escape)
        If Not isESC AndAlso Not isTab Then
            ' Tab + Esc ist für das Popup wichtig
            ' Return behandeln wir selbst
            e.SuppressKeyPress = True
        End If
    End Sub

    Private Sub SetResult(sender As Object, e As CalculatorSetResultEventArgs)
        If e.Action = CalculatorAction.Init OrElse
            e.Action = CalculatorAction.ClearAll OrElse
            e.Action = CalculatorAction.ClearEntry OrElse
            e.Action = CalculatorAction.Operand OrElse
            e.Action = CalculatorAction.EraseLastChar OrElse
            e.Action = CalculatorAction.DecimalSeparator OrElse
            e.Action = CalculatorAction.ToggleSign Then
            Me.TextBoxPart.Text = e.Input
            myCalculatorCloseCounter = 0
        ElseIf e.Action = CalculatorAction.IntermediaryResult OrElse e.Action = CalculatorAction.FinalResult Then
            Dim calcWin As SimpleCalculator = Nothing
            Try
                calcWin = DirectCast(myCalculatorPopup.PopupContentControl, SimpleCalculator)
                Me.TextBoxPart.Text = CStr(e.Value)
                calcWin.Tag = e.Value       ' wird beim Close des Popups als Value zurückgeschrieben

            Catch aoor As ArgumentOutOfRangeException

            End Try
            If e.Action = CalculatorAction.FinalResult Then
                myCalculatorCloseCounter += 1

                If myCalculatorCloseCounter >= 2 Then
                    Try
                        RemoveHandler calcWin.SetResult, AddressOf SetResult
                        ToggleCalculator()
                    Finally
                        RemoveHandler calcWin.SetResult, AddressOf SetResult
                    End Try
                    Me.TextBoxPart.SelectionLength = 0
                    Me.TextBoxPart.SelectionStart = Me.TextBoxPart.Text.Length
                End If
            Else
                myCalculatorCloseCounter = 0
            End If
        End If
    End Sub

    Private Sub PopupClosing(sender As Object, e As PopupClosingEventArgs)

        Dim calcWin = DirectCast(myCalculatorPopup.PopupContentControl, SimpleCalculator)

        Try
            calcWin.ForceFinalCalculation()
            If calcWin.Tag IsNot Nothing Then
                'Me.Value = CDec(calcWin.Tag)
                Me.TextBoxPart.Text = CDec(calcWin.Tag).ToString(Me.FormatString)
            End If


        Catch be As MvvmBindingException
            ' hier ging was schief
            ' Wert ggf. negativ?
        Finally
            RemoveHandler Me.TextBoxPart.KeyDown, AddressOf ResendAndSupressKeys
            RemoveHandler Me.TextBoxPart.KeyUp, AddressOf SupressKeys
            RemoveHandler calcWin.SetResult, AddressOf SetResult
            RemoveHandler myCalculatorPopup.PopupClosing, AddressOf PopupClosing
            RemoveHandler myCalculatorPopup.PopupCloseRequested, AddressOf PopupCloseRequested

            myCalculatorPopup.ClosePopupInternally(e)

            SimpleCalculator.ShowCaret(Me.TextBoxPart)

            Dim upDown = FindNullableValuePrimalUpDownControl()
            If upDown IsNot Nothing Then
                upDown.Enabled = True
            End If
            'Me.TextBoxPart.Enabled = False  ' konsistenten zustand wiederherstellen
            'Me.ValueControl.Enabled = True

            'Me.ValueControl.Visible = True
        End Try
    End Sub

    Private Sub PopupCloseRequested(sender As Object, e As PopupCloseRequestedEventArgs)

        If e.CloseRequestReason = PopupCloseRequestReasons.KeyboardCommit AndAlso e.KeyCode = Keys.Return AndAlso myCalculatorCloseCounter < 1 Then
            Dim calcWin = DirectCast(myCalculatorPopup.PopupContentControl, SimpleCalculator)
            calcWin.ProcessKeyDown(e.KeyCode)
            Return
        End If
        Dim e2 As New PopupClosingEventArgs(PopupClosingReason.Keyboard,
                                                    e.KeyCode)

        If Not e2.Cancel Then
            myCalculatorPopup.ClosePopupInternally(e2)
        End If

    End Sub


    ''' <summary>
    ''' If the calculator is visible/active this property returns true and false otherwise
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property IsCalculatorOpen As Boolean
        Get
            Return myCalculatorPopup IsNot Nothing AndAlso myCalculatorPopup.IsOpen
        End Get
    End Property

    Private Function FindNullableValuePrimalUpDownControl() As UpDownButton
        Dim x = (From item In Me.ValueControl.Controls
                 Let titem = TryCast(item, UpDownButton)
                 Where titem IsNot Nothing
                 Select titem).FirstOrDefault
        Return x
    End Function

    Protected Overrides Sub Dispose(disposing As Boolean)
        MyBase.Dispose(disposing)
        If disposing Then
            RemoveHandler Me.TextBoxPart.KeyPress, AddressOf TextBoxPartKeyPressHandler
        End If
    End Sub

End Class

''' <summary>
''' Controls the type of calculator to be used.
''' </summary>
Public Enum CalculatorType
    ''' <summary>
    ''' Calculator is not used.
    ''' </summary>
    None

    ''' <summary>
    ''' Provides a simple visual calculator with basic arithmetics.
    ''' </summary>
    Simple
End Enum

''' <summary>
''' Controls how to activate the calculator.
''' </summary>
<Flags>
Public Enum CalculatorActivationTrigger
    ''' <summary>
    ''' Calculator cannot be used.
    ''' </summary>
    None = 0

    ''' <summary>
    ''' Calculator can be shown by Ctrl-R
    ''' </summary>
    Ctrl_R = 1

    ''' <summary>
    ''' Calculator can be shown by Cursor Up or Down
    ''' </summary>
    Cursor_UpOrDown = 2

    ''' <summary>
    ''' Calculator can be shown by F2
    ''' </summary>
    F2 = 4

    ''' <summary>
    ''' Calculator can be shown by F3
    ''' </summary>
    F3 = 8

    ''' <summary>
    ''' Calculator can be shown by F5
    ''' </summary>
    F5 = 16

    ''' <summary>
    ''' Calculator can be shown by F5
    ''' </summary>
    F6 = 32

    ''' <summary>
    ''' Calculator can be shown by * / - + = NOTE: This deactivates AllowFormular Property.
    ''' </summary>
    BasicArithmeticKeys = 64

    ''' <summary>
    ''' Calculator can by the Letter C NOTE: This deactivates AllowFormular Property.
    ''' </summary>
    Letter_C = 128

    ''' <summary>
    ''' Calculator can be shown by Dash
    ''' </summary>
    MinusSign = 256

    ''' <summary>
    ''' Calculator can be activated by Cursor up/down or Ctrl+R
    ''' </summary>
    Subtle = Ctrl_R Or Cursor_UpOrDown

    ''' <summary>
    ''' Calculator can be activated by Cursor up/down, Ctrl+R and F2.
    ''' </summary>
    Normal = Subtle Or CalculatorActivationTrigger.F2

    ''' <summary>
    ''' Calculator can be activated by Cursor up/down, Letter C or = + * / but NOT the Minus sign, so negative values can be entered.
    ''' </summary>
    SemiProminent = Cursor_UpOrDown Or Letter_C Or BasicArithmeticKeys

    ''' <summary>
    ''' Calculator can be activated by Cursor up/down, Letter C or = + - * /.
    ''' </summary>
    Prominent = Cursor_UpOrDown Or Letter_C Or BasicArithmeticKeys Or CalculatorActivationTrigger.MinusSign

End Enum
