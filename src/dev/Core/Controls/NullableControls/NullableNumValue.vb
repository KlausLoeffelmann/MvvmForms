Imports System.ComponentModel
Imports System.Windows.Forms

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

    Private Const DEFAULT_MAX_VALUE_EXCEEDED_MESSAGE As String = "Der eingegebenene Wert überschreitet das Maximum!"
    Private Const DEFAULT_MIN_VALUE_EXCEEDED_MESSAGE As String = "Der eingegebene Wert unterschreitet das Minimum!"

    Private ReadOnly DEFAULT_MIN_VALUE As Decimal? = 0
    Private ReadOnly DEFAULT_MAX_VALUE As Decimal? = Nothing

    Private ReadOnly DEFAULT_INCREMENT As Decimal? = 1

    Private myIncrement As Decimal?
    Private myMinValue As Decimal?
    Private myMaxValue As Decimal?

    Sub New()
        MyBase.New()
        Me.MinValue = NullableControlManager.GetInstance.GetDefaultMinValue(Me, DEFAULT_MIN_VALUE)
        Me.MaxValue = NullableControlManager.GetInstance.GetDefaultMaxValue(Me, DEFAULT_MAX_VALUE)

        Me.Increment = NullableControlManager.GetInstance.GetDefaultIncrement(Me, DEFAULT_INCREMENT)
        Me.MaxValueExceededMessage = NullableControlManager.GetInstance.GetDefaultMaxValueExceededMessage(Me, DEFAULT_MAX_VALUE_EXCEEDED_MESSAGE)
        Me.MinValueExceededMessage = NullableControlManager.GetInstance.GetDefaultMinValueExceededMessage(Me, DEFAULT_MIN_VALUE_EXCEEDED_MESSAGE)

        AddHandler Me.ReadOnlyChanged, AddressOf myReadOnlyChanged

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

    Protected Overrides Function ProcessCmdKey(ByRef msg As Message, keyData As Keys) As Boolean

        'Console.WriteLine("keyData= {0}, (Keys.Control And Keys.R) = {1}", keyData, (Keys.Control Or Keys.R))
        If (DropDownCalculatorTrigger = CalculatorActivationTrigger.Strg_R AndAlso keyData = (Keys.Control Or Keys.R)) OrElse
            (DropDownCalculatorTrigger = CalculatorActivationTrigger.Cursor_UpOrDown AndAlso (keyData = Keys.Down OrElse keyData = Keys.Up)) Then

            If DropDownCalculatorMode = CalculatorType.Simple Then
                ToggleCalculator()
                Return True
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
                Me.ValueControl.AllowNonNumericKeys = value
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
        ElseIf e.Action = CalculatorAction.IntermediaryResult OrElse e.Action = CalculatorAction.FinalResult Then
            Me.Value = e.Value
            If e.Action = CalculatorAction.FinalResult Then
                Dim calcWin = DirectCast(myCalculatorPopup.PopupContentControl, SimpleCalculator)
                Try
                    RemoveHandler calcWin.SetResult, AddressOf SetResult
                    ToggleCalculator()
                Finally
                    RemoveHandler calcWin.SetResult, AddressOf SetResult
                End Try
                Me.TextBoxPart.SelectionLength = 0
                Me.TextBoxPart.SelectionStart = Me.TextBoxPart.Text.Length

            End If
        End If
    End Sub

    Private Sub PopupClosing(sender As Object, e As PopupClosingEventArgs)
        'If e.PopupCloseReason = PopupClosingReason.AppFocusChanged OrElse e.PopupCloseReason = PopupClosingReason.AppClicked Then
        '    ' den Taschenrechner lassen wir offen
        '    e.Cancel = True
        '    Return
        'End If
        Dim calcWin = DirectCast(myCalculatorPopup.PopupContentControl, SimpleCalculator)
        Try
            calcWin.ForceFinalCalculation()
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
        Dim e2 As New PopupClosingEventArgs(PopupClosingReason.Keyboard,
                                                    e.KeyCode)

        If Not e2.Cancel Then
            myCalculatorPopup.ClosePopupInternally(e2)
        End If

    End Sub

    Private Function FindNullableValuePrimalUpDownControl() As UpDownButton
        Dim x = (From item In Me.ValueControl.Controls
                 Let titem = TryCast(item, UpDownButton)
                 Where titem IsNot Nothing
                 Select titem).FirstOrDefault
        Return x
    End Function
End Class

Public Enum CalculatorType
    ''' <summary>
    ''' Es steht kein Taschenrechner zur Verfügung
    ''' </summary>
    None
    ''' <summary>
    ''' Ein einfacher Taschenrechner mit den Grundrechenarten
    ''' </summary>
    Simple
End Enum

Public Enum CalculatorActivationTrigger
    ''' <summary>
    ''' Keine automatische anzeige des Taschenrechners möglich
    ''' </summary>
    None
    ''' <summary>
    ''' Anzeige des Taschenrechners über den Hotkey Strg-R (Strg C geht ja leider nicht)
    ''' </summary>
    Strg_R
    ''' <summary>
    ''' Anzeige des Taschenrechners über die Cursor Up oder Down-Taste
    ''' </summary>
    Cursor_UpOrDown

End Enum