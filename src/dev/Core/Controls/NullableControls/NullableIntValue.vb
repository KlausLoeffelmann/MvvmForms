Imports System.ComponentModel
Imports System.Windows.Forms

''' <summary>
''' Allows editing of integer values, which can be retrieved/set over the Value property, 
''' and which can handle Nothing (null in CSharp) on top.
''' </summary>
<Description("Allows interactive editing of integer values which can be retrieved/set over the Value property," &
             "and which can handle Nothing (null in CSharp) on top."),
 Designer("ActiveDevelop.EntitiesFormsLib.TextBoxBasedControlDesigner")>
Public Class NullableIntValue
    Inherits NullableValueBase(Of Integer, NullableValuePrimalUpDownControl)

    Private myDecimalPlaces As Integer
    Private myHasThousandsSeperator As Boolean
    Private myCurrencySymbolString As String
    Private myCurrencySymbolUpFront As Boolean
    Private myAllowFormular As Boolean

    Private Const DEFAULT_MAX_VALUE_EXCEEDED_MESSAGE As String = "Der eingegebenene Wert überschreitet das Maximum!"
    Private Const DEFAULT_MIN_VALUE_EXCEEDED_MESSAGE As String = "Der eingegebene Wert unterschreitet das Minimum!"

    Private ReadOnly DEFAULT_MIN_VALUE As Decimal? = 0
    Private ReadOnly DEFAULT_MAX_VALUE As Decimal? = Nothing

    Private ReadOnly DEFAULT_ALLOW_FORMULAR As Boolean = True


    Private ReadOnly DEFAULT_INCREMENT As Decimal? = 1
    Dim myIncrement As Integer?
    Dim myMaxValue As Integer?
    Dim myMinValue As Integer?

    Sub New()
        MyBase.New()
        Me.MinValue = CType(NullableControlManager.GetInstance.GetDefaultMinValue(Me, DEFAULT_MIN_VALUE), Integer?)
        Me.MaxValue = CType(NullableControlManager.GetInstance.GetDefaultMaxValue(Me, DEFAULT_MAX_VALUE), Integer?)

        Me.Increment = CInt(NullableControlManager.GetInstance.GetDefaultIncrement(Me, DEFAULT_INCREMENT))
        Me.MaxValueExceededMessage = NullableControlManager.GetInstance.GetDefaultMaxValueExceededMessage(Me, DEFAULT_MAX_VALUE_EXCEEDED_MESSAGE)
        Me.MinValueExceededMessage = NullableControlManager.GetInstance.GetDefaultMinValueExceededMessage(Me, DEFAULT_MIN_VALUE_EXCEEDED_MESSAGE)

        Me.AllowFormular = NullableControlManager.GetInstance.GetDefaultAllowFormular(Me, DEFAULT_ALLOW_FORMULAR)

        AddHandler Me.ReadOnlyChanged, AddressOf myReadOnlyChanged

        'Wirering up the event which blocks alpha keys when no Formula is allowed.
        AddHandler Me.TextBoxPart.KeyPress, AddressOf TextBoxPartKeyPressHandler

        AddHandler Me.ValueControl.ButtonAction,
            Sub(sender As Object, e As ButtonActionEventArgs)
                If Increment.HasValue Then
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
                        If Value.HasValue Then
                            If e.ButtonType = UpDownComboButtonIDs.Down Then
                                If MinValue.HasValue Then
                                    If Value - Increment >= MinValue Then
                                        MyBase.SetValuePreserveOriginalValue(Value - Increment)
                                        MyBase.TagDirtyState()
                                    Else
                                        Beep()
                                    End If
                                Else
                                    MyBase.SetValuePreserveOriginalValue(Value - Increment)
                                    MyBase.TagDirtyState()
                                End If
                            Else
                                If MaxValue.HasValue Then
                                    If Value + Increment <= MaxValue Then
                                        MyBase.SetValuePreserveOriginalValue(Value + Increment)
                                        MyBase.TagDirtyState()
                                    Else
                                        Beep()
                                    End If
                                Else
                                    MyBase.SetValuePreserveOriginalValue(Value + Increment)
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
                    Finally
                        'BeginChangeValueInternally abschließen.
                        MyBase.EndValueChangeCauseToUser()
                    End Try
                End If
            End Sub
    End Sub

    'Handels the TextBoxPartKeyPress event and prevent letters when AllowFormular is false.
    Private Sub TextBoxPartKeyPressHandler(sender As Object, e As KeyPressEventArgs)
        If Not AllowFormular Then
            If Char.IsNumber(e.KeyChar) Or
               Char.IsControl(e.KeyChar) Or
               e.KeyChar.Equals("-"c) Then
            Else
                e.Handled = True
            End If
        End If
    End Sub

    Private Sub myReadOnlyChanged(sender As Object, e As EventArgs)
        MyBase.ValueControl.Enabled = Not Me.ReadOnly
    End Sub

    Protected Overrides Sub InitializeProperties()
        HasThousandsSeperator = True
        AllowFormular = True
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
        If HasThousandsSeperator Then
            ret = "#,##0"
        Else
            ret = "###0"
        End If
        Return ret
    End Function

    Private Function CreateFormatString() As String
        Return GetDefaultFormatString()
    End Function

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
    Property Increment As Integer?
        Get
            Return myIncrement
        End Get
        Set(value As Integer?)
            myIncrement = value
            If value.HasValue Then
                Me.ValueControl.HideButtons = False
            Else
                Me.ValueControl.HideButtons = True
            End If
        End Set
    End Property

    Private Function ShouldSerializeIncrement() As Boolean
        Return CBool(Increment <> NullableControlManager.GetInstance.GetDefaultIncrement(Me, DEFAULT_INCREMENT))
    End Function

    Private Sub ResetIncrement()
        Increment = CType(NullableControlManager.GetInstance.GetDefaultIncrement(Me, DEFAULT_INCREMENT), Integer?)
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
            myAllowFormular = value
            DirectCast(Me.FormatterEngine, NullableNumValueFormatterEngine).IsFormularAllowed = value
        End Set
    End Property

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
    Public Property MaxValue() As Integer?
        Get
            Return myMaxValue
        End Get
        Set(ByVal value As Integer?)
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
    Public Property MinValue() As Integer?
        Get
            Return myMinValue
        End Get
        Set(ByVal value As Integer?)
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
    ''' Behandelt die Beachtung Min-/und Max-Eigenschaften bei der Wertevalidierung.
    ''' </summary>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Overrides Sub OnValueValidating(e As NullableValueValidationEventArgs(Of Integer?))
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
        Return New ContainsUIMessageException(ex.Message, "Der eingegebene Text konnte nicht in einen Ganzzahlenwert umgewandelt werden.", ex)
    End Function

    Protected Overrides Function ChangeValuetypeInternally(ByVal ValueAsObject As Object) As Integer?
        Return CTypeDynamic(Of Integer?)(ValueAsObject)
    End Function

    Protected Overrides Function RechangeValueTypeInternally(ByVal ValueAsObject As Integer?) As Object
        Return CTypeDynamic(Of Decimal?)(ValueAsObject)
    End Function

End Class
