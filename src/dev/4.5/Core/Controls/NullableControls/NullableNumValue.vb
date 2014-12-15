Imports System.ComponentModel
Imports System.Windows.Forms

''' <summary>
''' Steuerelement zur Erfassung von numerischen Werten, das überdies Null-Werte verarbeitet, Textformeln berechnen kann, 
''' eine vereinheitlichende Value-Eigenschaft bietet, seinen Inhalt anpassbar formatiert, 
''' Funktionen für Rechteverwaltung zur Verfügung stellt und von einer 
''' <see cref="FormToBusinessClassManager">FormToBusinessClassManager-Komponente</see> verwaltet werden kann.
''' </summary>
<Designer("ActiveDevelop.EntitiesFormsLib.TextBoxBasedControlDesigner")>
Public Class NullableNumValue
    Inherits NullableValueBase(Of Decimal, NullableValuePrimalUpDownControl)

    Private myDecimalPlaces As Integer
    Private myHasThousandsSeperator As Boolean
    Private myCurrencySymbolString As String
    Private myCurrencySymbolUpFront As Boolean
    Private myAllowFormular As Boolean
    Private myLeadingZeros As Integer

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

    Private Sub myReadOnlyChanged(sender As Object, e As EventArgs)
        MyBase.ValueControl.Enabled = Not Me.ReadOnly
    End Sub

    Protected Overrides Sub InitializeProperties()
        DecimalPlaces = -1
        HasThousandsSeperator = True
        CurrencySymbolString = ""
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
            myAllowFormular = value
            DirectCast(Me.FormatterEngine, NullableNumValueFormatterEngine).IsFormularAllowed = value
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
End Class
