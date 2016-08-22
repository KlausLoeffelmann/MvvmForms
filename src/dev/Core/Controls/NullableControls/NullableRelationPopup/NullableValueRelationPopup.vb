Imports System.ComponentModel
Imports System.Drawing
Imports System.Windows.Forms
Imports System.Windows.Forms.VisualStyles
Imports System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox
Imports System.Reflection
Imports System.Runtime.InteropServices

Imports System.Threading
Imports System.Threading.Tasks

Imports ActiveDevelop.EntitiesFormsLib.ObjectAnalyser

''' <summary>
''' Steuerelement zur aufklappbaren Darstellung von Elementen in DataGridView-Listen, das überdies Null-Werte verarbeitet, 
''' Such- und Auto-Complete-Funktionalitäten sowie eine vereinheitlichende Value-Eigenschaft bietet, 
''' Funktionen für Rechteverwaltung zur Verfügung stellt und von einer 
''' <see cref="FormToBusinessClassManager">FormToBusinessClassManager-Komponente</see> verwaltet werden kann.
''' </summary>
<ToolboxItem(True)>
Public Class NullableValueRelationPopup
    Inherits TextBoxPopup
    Implements ITextBoxBasedControl, INullableValueDataBinding,
                INullableValueRelationBinding, IPermissionManageableUIContentElement,
                ISupportInitialize

    Private Const WM_KEYFIRST = &H100
    Private Const WM_KEYLAST = &H108

    Private Const STATUSBAROFFSETHEIGHT = 41
    Private Const SCROLLBAROFFSETHEIGHT = 4
    Private Const SCROLLBAROFFSETWIDTH = 3
    Private Const SCROLLBARWIDTH = 17

    'General Infrastructure Fields:
    Private myDontRefocus As Boolean
    Private myLastFailedValidationException As ContainsUIMessageException
    Private myBackColorBrush As SolidBrush
    Private myValueBeforeValueChanged As Object
    Private myIsDirty As Boolean
    Private myOldSelectedValue As Object
    Private mySupressSelectedValueChange As Boolean

    Private myIsInitializing As Boolean
    Private myIgnoreNextValueReset As Boolean
    Private myRaiseIsDirtyOnPreserveInputTextChange As Boolean
    Private myIsHandleDestroyed As Boolean

    Private myDataGridForm As DataGridViewPopupContent
    Private myCurrentRowIndex As Integer
    Private myOpenOnFirstKeyStroke As Boolean
    Private myIgnoreNextSelectedValueChange As Boolean
    Private myUndoValue As Object
    Private myTextDiff As String
    Private myIsBinding As Boolean
    Private myChangedByValueSetAccessor As Boolean
    Private myAddButton As SimpleButton
    Private myOldBackColor As Color
    Private myHandleCreated As Boolean

    'Class Members for ASync Entry Search
    Private myCurrentAutoCompleteListAndSchemaInfo As Tuple(Of List(Of String), List(Of AutoCompleteLookupItem))
    Private myOldCurrentAutoCompleteListAndSchemaInfo As List(Of String)
    Private myTextBoxDeferrer As TextBoxDeferrer
    Private myFilterTask As FilterBindingViewTask
    Private myStopFilteringBindingView As Boolean
    Private myFilterTaskIsRunning As Boolean
    Private myFilterBindingViewInProgress As Boolean

    'Property Backing Fields:
    Private myReverseTextOverflowBehaviour As Boolean
    Private myHasAddButton As Boolean
    Private myValueText As String
    Private myPreserveInput As Boolean
    Private mySearchColumnHeaderFont As Font
    Private mySearchColumnBackgroundColor As Color = DEFAULT_SEARCH_COLUMN_BACKGROUND_COLOR
    Private mySearchColumnHeaderBackgroundColor As Color = DEFAULT_SEARCH_COLUMN_HEADER_BACKGROUND_COLOR

    Private myDataSource As Object
    Private myGroupName As String = NullableControlManager.GetInstance.GetDefaultGroupName(Me, "Default")
    Private myOnUnassignableValueAction As UnassignableValueAction
    Private myFocusColor As Color
    Private myOnFocusColor As Boolean
    Private mySearchable As Boolean
    Private mySearchPattern As String
    Private myDisplayMember As String
    Private myPreferredVisibleColumnsOnOpen As Integer
    Private myPreferredVisibleRowsOnOpen As Integer
    Private myValue As Object
    Private myMultiSelect As Boolean
    Private myDataFieldname As String
    Private myDeferredTextChangeDelay As Integer = 300
    Private myImitateTabByPageKeys As Boolean

    'Default Values
    Private ReadOnly DEFAULT_SEARCH_COLUMN_BACKGROUND_COLOR As Color = Drawing.Color.FromArgb(255, 255, 224)
    Private ReadOnly DEFAULT_SEARCH_COLUMN_HEADER_BACKGROUND_COLOR As Color = Drawing.Color.FromArgb(238, 232, 170)
    Private ReadOnly DEFAULT_SEARCH_COLUMN_HEADER_FONT_STYLE As FontStyle = FontStyle.Bold

    Private Const DEFAULT_BEEP_ON_FAILED_VALIDATION As Boolean = False
    Private Const DEFAULT_ON_FOCUS_COLOR As Boolean = True
    Private ReadOnly DEFAULT_FOCUS_COLOR As Color = Color.Yellow
    Private ReadOnly DEFAULT_ERROR_COLOR As Color = Color.Red
    Private Const DEFAULT_FOCUS_SELECTION_BEHAVIOUR As FocusSelectionBehaviours = FocusSelectionBehaviours.PreSelectInput
    Private Const DEFAULT_IMITATE_TAB_BY_PAGE_KEYS = False

    ''' <summary>
    ''' Raised, if ThrowLayoutException is set to false and there was an exception on auto-layouting the BindableDataGrid.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Public Event GridLayoutException(sender As Object, e As EventArgs)

    ''' <summary>
    ''' Wird ausgelöst, wenn der Anwender den Text ändert, der zur Suche des Eintrages in der Liste 
    ''' oder zur Textbestimmung bei PreserveInput = true verwendet wird.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Public Event ValueTextChanged(sender As Object, e As EventArgs)

    ''' <summary>
    ''' Wird ausgelöst, wenn der Anwender zur Laufzeit auf den AddButton klickt, der sich mit der HasAddButton-Eigenschaft einschalten lässt.
    ''' </summary>
    ''' <param name="sender">Steuerelement, das das Ereignis ausgelöst hat.</param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Public Event AddButtonClick(sender As Object, e As EventArgs)

    ''' <summary>
    ''' Wird ausgelöst, wenn sich der Wert im Steuerelement geändert hat, um einen einbindenden Formular oder 
    ''' User Control die Möglichkeit zu geben, den Benutzer zu informieren, dass er Änderungen speichern muss.
    ''' </summary>
    ''' <param name="sender">Steuerelement, das das Ereignis ausgelöst hat.</param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Public Event IsDirtyChanged(ByVal sender As Object, ByVal e As IsDirtyChangedEventArgs) Implements INullableValueDataBinding.IsDirtyChanged

    ''' <summary>
    ''' Wird ausgelöst, wenn sich die Datenquelle geändert hat, die zum Befüllen des NullableValueRelationPopup Controls mit den entsprechenden Einträgen dient.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Public Event DataSourceChanged(sender As Object, e As EventArgs)

    ''' <summary>
    ''' Wird ausgelöst, nachdem sich der Wert der Value-Eigenschaft geändert hat.
    ''' </summary>
    ''' <param name="sender">Objekt, dass das Ereignis ausgelöst hat.</param>
    ''' <param name="e">Ereignisparameter vom Typ ValueChangedEventArgs, mit denen der Grund für die Werteänderung ermittelt werden kann.</param>
    ''' <remarks></remarks>
    Public Event ValueChanged(ByVal sender As Object, ByVal e As ValueChangedEventArgs) Implements INullableValueDataBinding.ValueChanged

    ''' <summary>
    ''' Wird ausgelöst, wenn das Steuerelement dem Entwickler die Möglichkeit gibt, die Schema-Informationen des angezeigten 
    ''' Grids anzupassen.
    ''' </summary>
    ''' <param name="sender">Steuerelement, das das Ereignis ausgelöst hat.</param>
    ''' <param name="e">EventArgs-Parameter, mit denen festgelegt werden kann, welche Spalten angezeigt werden sollen.</param>
    ''' <remarks></remarks>
    Public Event GetColumnSchema(ByVal sender As Object, ByVal e As GetColumnSchemaEventArgs)

    ''' <summary>
    ''' Wird ausgelöst, wenn der Wert der Eigenschaft DeferredTextChangeDelay geändert wurde.
    ''' Grids anzupassen.
    ''' </summary>
    ''' <param name="sender">Steuerelement, das das Ereignis ausgelöst hat.</param>
    ''' <param name="e">EventArgs-Parameter, mit denen festgelegt werden kann, welche Spalten angezeigt werden sollen.</param>
    ''' <remarks></remarks>
    Public Event DeferredTextChangeDelayChanged(sender As Object, e As EventArgs)

    ''' <summary>
    ''' Wird ausgelöst, wenn bei der Zuweisung der Value-Eigenschaft ein Wert erkannt wurde, der es nicht ermöglich, den entsprechenden Eintrag in der Liste zu selektieren.
    ''' </summary>
    ''' <param name="sender">Objekt, dass das Ereignis ausgelöst hat.</param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Public Event UnassignableValueDetected(ByVal sender As Object, ByVal e As UnassignableValueDetectedEventArgs)

    'Der ist nur zur Interface-Richtlinien-Erfüllung da und wird hier nie benutzt, da es keinen Zustand gibt,
    'bei dem dieses Steuerelement den Focus behalten könnte.
    'TODO: Überprüfen, ob es nicht doch Fälle gibt, bei dem der Fokus erzwungener Maßen erhalten bleiben soll.
    Public Event RequestValidationFailedReaction(ByVal sender As Object, ByVal e As RequestValidationFailedReactionEventArgs) Implements INullableValueDataBinding.RequestValidationFailedReaction

    ''' <summary>
    ''' Wird ausgelöst, wenn sich der selektierte Wert in der Liste geändert hat.
    ''' </summary>
    ''' <param name="sender">Objekt, dass das Ereignis ausgelöst hat.</param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Public Event SelectedValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Implements INullableValueRelationBinding.SelectedValueChanged

    Private Class FilterBindingViewTask
        Property FilterTask As Task
        Property AssignedCancellationTokenSource As CancellationTokenSource
    End Class

    Sub New()
        MyBase.New()

        SetStyle(ControlStyles.ResizeRedraw, True)

        NullValueString = NullableControlManager.GetInstance.GetDefaultNullValueString(Me, "* - - - *")
        BeepOnFailedValidation = NullableControlManager.GetInstance.GetDefaultBeepOnFailedValidation(Me, DEFAULT_BEEP_ON_FAILED_VALIDATION)
        OnFocusColor = NullableControlManager.GetInstance.GetDefaultOnFocusColor(Me, DEFAULT_ON_FOCUS_COLOR)
        FocusColor = NullableControlManager.GetInstance.GetDefaultFocusColor(Me, DEFAULT_FOCUS_COLOR)
        FocusSelectionBehaviour = NullableControlManager.GetInstance.GetDefaultFocusSelectionBehaviour(Me, DEFAULT_FOCUS_SELECTION_BEHAVIOUR)
        ExceptionBalloonDuration = NullableControlManager.GetInstance.GetDefaultExceptionBalloonDuration(Me, 5000)
        ImitateTabByPageKeys = NullableControlManager.GetInstance.GetDefaultImitateTabByPageKeys(Me, DEFAULT_IMITATE_TAB_BY_PAGE_KEYS)

        Me.TextBoxPart.ReadOnly = True
        Me.PreferredVisibleColumnsOnOpen = 0
        Me.AutoResizeColumnsOnOpen = True
        Me.IsPopupAutoSize = False
        Me.IsPopupResizable = True
        Me.FocusColor = GetDefaultFocusColor()
        Me.OnFocusColor = GetDefaultOnFocusColor()

        myDataGridForm = DirectCast(MyBase.HostingControl, DataGridViewPopupContent)
        Me.OnUnassignableValueAction = UnassignableValueAction.TryHandleUnassignableValueDetectedEvent

        Me.MultiSelect = False
        Me.PopupControl.ShowStatusBar = True
        Me.PopupControl.MinimumSize = New Size(200, 160)
        Me.PopupControl.ValueTextAlignment = StringAlignment.Center

        System.Windows.WeakEventManager(Of TextBox, EventArgs).AddHandler(
                Me.TextBoxPart, "TextChanged",
            Sub(sender As Object, e As EventArgs)
                'If Me.TextBoxPart.Text = Me.NullValueString Then
                '    Me.PopupControl.ValueText = Nothing
                'Else

                '***** Magges Änderung.
                If Me.PopupControl IsNot Nothing Then
                    Me.PopupControl.ValueText = Me.TextBoxPart.Text
                End If
                'End If

                'IsDirty-Handling für PreserveInput=True und RaiseIsDirtyOnPreserveInputTextChange=true
                If Not myTextBoxDeferrer.IgnoreNextTextChange Then
                    If RaiseIsDirtyOnPreserveInputTextChange Then
                        If PreserveInput Then
                            If Not IsDirty Then
                                IsDirty = True
                            End If
                        End If
                    End If
                End If

                'Wenn TextChange im TextBoxPart nicht gerade durch die Auswahl 
                'durch einen Eintrag in der Liste ausgelöst wurde, dann
                'Value zurücksetzen, denn wurde entweder durch TAB
                'durch Mausklick oder durch RETURN dieser Wert gerade erst gesetzt.
                'Umgekehrt muss er aber bei einer manuellen EIngabe gelöscht werden,
                'da er sonst ein falsches Ergebnis liefern würde,
                'wenn mit PreserveInput=true auch neue Eingaben erlaubt wären.
                If Not myIgnoreNextValueReset Then
                    myValue = Nothing
                End If
                OnTextChanged(e)
            End Sub)

        'SetColumnSchema hochleiten von BindableDataGridView
        System.Windows.WeakEventManager(Of BindableDataGridView, GetColumnSchemaEventArgs).AddHandler(
            Me.myDataGridForm.BindableDataGridView, "GetColumnSchema",
            Sub(sender As Object, e As GetColumnSchemaEventArgs)
                OnGetColumnSchema(e)
            End Sub)

        'ResetButton behandeln: Hier setzen wir den Wert des Controls auf Null.
        System.Windows.WeakEventManager(Of ResizablePopup, EventArgs).AddHandler(
            Me.PopupControl, "ResetButtonClick",
            Sub(sender As Object, e As EventArgs)
                If Me.PreserveInput Then
                    'Wir nehmen in diesem Fall Early-Commit an, da der Benutzer vermutlich eher
                    'am Selektieren von Null als am Löschen der Eingabe interessiert ist.
                    If Me.ValueText IsNot Nothing Then
                        IsDirty = True
                    End If
                End If
                Me.myDataGridForm.BindableDataGridView.ClearSelection()
                myTextBoxDeferrer.IgnoreNextTextChange = True
                If Me.PreserveInput Then
                    Me.TextBoxPart.Text = ""
                End If
                Me.ClosePopup(New PopupClosingEventArgs(PopupClosingReason.ResetButtonClicked))
            End Sub)

        'CellClick führt zum Popup schließen, da Werteaufwahl stattgefunden hat.
        'Wir binden weak an der Stelle, da die Quellen für das Event möglicherweise
        'weit außerhalb des Forms liegen können, und verhindern könnten,
        'dass das NullableValueRealtionPopup entsorgt wird.
        System.Windows.WeakEventManager(Of BindableDataGridView, DataGridViewCellEventArgs).AddHandler(
                Me.myDataGridForm.BindableDataGridView, "CellClick",
                    Sub(sender As Object, e As DataGridViewCellEventArgs)
                        Me.ClosePopup(New PopupClosingEventArgs(PopupClosingReason.ContentClicked) With {.Caused = PopupCloseCause.ExternalByUser})
                        myTextBoxDeferrer.IgnoreNextTextChange = True
                    End Sub)

        'CellClick führt zum Popup schließen, da Werteaufwahl stattgefunden hat.
        'Wir binden weak an der Stelle, da die Quellen für das Event möglicherweise
        'weit außerhalb des Forms liegen können, und verhindern könnten,
        'dass das NullableValueRealtionPopup entsorgt wird.
        System.Windows.WeakEventManager(Of BindableDataGridView, UnassignableValueDetectedEventArgs).AddHandler(
            Me.myDataGridForm.BindableDataGridView,
            "UnassignableValueDetected",
            Sub(sender As Object, e As UnassignableValueDetectedEventArgs)
                OnUnassignableValueDetected(e)
            End Sub)

        'SelectedValueChanged im BindableDataGridView löst SelectedValueChanged aus.
        'Wir binden weak an der Stelle, da die Quellen für das ValueChange möglicherweise
        'weit außerhalb des Forms liegen können, und verhindern könnten,
        'dass das NullableValueRealtionPopup entsorgt wird.
        System.Windows.WeakEventManager(Of BindableDataGridView, EventArgs).AddHandler(
            Me.myDataGridForm.BindableDataGridView,
            "SelectedValueChanged",
                        Sub(sender As Object, e As EventArgs)

                            If Me.myIsBinding Or mySupressSelectedValueChange Then Return

                            'Damit Tastatursteuerung funktionieren kann!
                            If myIgnoreNextSelectedValueChange Then
                                myIgnoreNextSelectedValueChange = False
                                Return
                            End If

                            If Not Object.Equals(Me.myDataGridForm.BindableDataGridView.Value, myOldSelectedValue) Then
                                myOldSelectedValue = Me.myDataGridForm.BindableDataGridView.Value
                                OnSelectedValueChanged(e)
                            End If
                        End Sub)

        System.Windows.WeakEventManager(Of BindableDataGridView, BindableDataGridLayoutExceptionEventArgs).AddHandler(
            Me.myDataGridForm.BindableDataGridView,
            NameOf(BindableDataGridView.BindableGridLayoutException),
                        Sub(sender As Object, e As BindableDataGridLayoutExceptionEventArgs)
                            RaiseEvent GridLayoutException(Me, e)
                        End Sub)

        System.Windows.WeakEventManager(Of ResizablePopup, EventArgs).AddHandler(
            Me.PopupControl, "ValueTextChanged",
            Sub(sender, e)
                OnValueTextChanged(EventArgs.Empty)
            End Sub)

        System.Windows.WeakEventManager(Of TextBox, KeyEventArgs).AddHandler(
            Me.TextBoxPart, NameOf(TextBox.KeyDown), AddressOf TextBoxPartKeyPressHandler)
    End Sub

    'Handles the TextBoxPart KeyPress Event for the ImitateTabByPageKeys Property.
    Private Sub TextBoxPartKeyPressHandler(sender As Object, e As KeyEventArgs)
        If ImitateTabByPageKeys Then
            If Not Me.PopupControl.IsOpen Then
                If e.KeyCode = Keys.Next Then
                    SendKeys.SendWait("{TAB}")
                    e.SuppressKeyPress = True
                ElseIf e.KeyCode = Keys.PageUp Then
                    SendKeys.SendWait("+{TAB}")
                    e.SuppressKeyPress = True
                End If
            End If
        End If
    End Sub

    Protected Overrides Sub OnHandleCreated(e As EventArgs)
        MyBase.OnHandleCreated(e)
        myTextBoxDeferrer = New TextBoxDeferrer(Me.TextBoxPart)
        AddHandler myTextBoxDeferrer.DeferralTextChanged, AddressOf OnDeferralTextChanged
    End Sub

    Protected Overrides Sub OnHandleDestroyed(e As EventArgs)
        myIsHandleDestroyed = True
        MyBase.DestroyPopup()
        MyBase.OnHandleDestroyed(e)
        RemoveHandler myTextBoxDeferrer.DeferralTextChanged, AddressOf OnDeferralTextChanged
        myTextBoxDeferrer.Dispose()
    End Sub

    Protected Overrides Function DefaultBorderStyle() As BorderStyle
        Return Borderstyle.FixedSingle
    End Function

    Protected Overridable Sub OnUnassignableValueDetected(ByVal e As UnassignableValueDetectedEventArgs)
        RaiseEvent UnassignableValueDetected(Me, e)
    End Sub

    Protected Overrides Function GetAdditionalButton(buttonCount As Integer) As Tuple(Of MultiPurposeButtonBase, Boolean)
        If buttonCount = 0 Then
            myAddButton = New SimpleButton
            AddHandler myAddButton.SimpleButtonClicked, Sub(sender As Object, e As EventArgs)
                                                            OnAddButtonClick(EventArgs.Empty)
                                                        End Sub

            AddHandler myAddButton.RequestFocus, Sub(sender As Object, e As RequestFocusEventArgs)
                                                     e.Succeeded = Me.TextBoxPart.Focus
                                                 End Sub

            myAddButton.Visible = False
            myAddButton.ButtonImage = My.Resources._70_row_add_16
            Return New Tuple(Of MultiPurposeButtonBase, Boolean)(myAddButton, False)
        End If
        Return MyBase.GetAdditionalButton(buttonCount)
    End Function

    Protected Overridable Sub OnAddButtonClick(e As EventArgs)
        RaiseEvent AddButtonClick(Me, e)
    End Sub

    Protected Overrides Sub OnPaintBackground(e As System.Windows.Forms.PaintEventArgs)
    End Sub

    Protected Overrides Sub OnBackColorChanged(e As System.EventArgs)
        MyBase.OnBackColorChanged(e)
        myBackColorBrush = Nothing
    End Sub

    Protected Overrides Sub OnPaint(e As System.Windows.Forms.PaintEventArgs)
        MyBase.OnPaint(e)
        If myBackColorBrush Is Nothing Then
            If BackColor = Color.Transparent Then
                myBackColorBrush = New SolidBrush(Color.White)
            Else
                myBackColorBrush = New SolidBrush(BackColor)
            End If
        End If

        Dim rec = New Rectangle(1, 1, Me.ClientSize.Width - 2, Me.ClientSize.Height - 2)
        e.Graphics.FillRectangle(myBackColorBrush, rec)

    End Sub

    'Müssen wir behandeln, um beim initialen Anzeigen des Popups nicht schon IsDirty falsch auszulösen,
    'weil es SelectionChange im PopupContainer auslöst.
    Protected Overrides Sub OnBeginOpenPopup(ByVal e As System.EventArgs)
        Me.IsLoading.Value = True
        MyBase.OnBeginOpenPopup(e)
        Me.IsLoading.Value = False
    End Sub

    ''' <summary>
    ''' Handelt das Ändern des Wertes im Control.
    ''' </summary>
    ''' <param name="lateCommit">Flag, das anzeigt, ob das Commiten des Wertes früh (durch Übernahme aus der Liste) 
    ''' oder spät und impliziet (durch FocusLost) erfolgte.</param>
    ''' <remarks></remarks>
    Private Sub HandleSelectedValueChanged(lateCommit As Boolean)
        If Not myIsHandleDestroyed Then

            If myDataGridForm.BindableDataGridView.Value Is Nothing Then
                If Me.Focused Or Me.TextBoxPart.Focused Then
                    If Me.PreserveInput Then
                        Me.PopupControl.ValueText = Me.TextBoxPart.Text
                    Else
                        Me.PopupControl.ValueText = String.Empty
                    End If
                Else
                    'Eigentlich sollten wir hier nie hinkommen, da dieser Part von
                    'CommitOnLeave abgefangen wird. Für etwaige Sonderfälle lassen wir
                    'den Code aber wie er ist.
                    If Me.PreserveInput Then
                        Me.PopupControl.ValueText = Me.TextBoxPart.Text
                    Else
                        Me.PopupControl.ValueText = Me.NullValueString
                    End If
                End If
            Else
                If Not String.IsNullOrWhiteSpace(Me.DisplayMember) Then
                    Me.PopupControl.ValueText =
                        ObjectAnalyser.ObjectToString(Me.ValueBase, Me.DisplayMember)
                Else
                    Me.PopupControl.ValueText = Value.ToString
                End If
            End If

            'ValueChanged nur auslösen, wenn sich Value wirklich geändert hat.
            If Not Object.Equals(myValueBeforeValueChanged, Me.Value) Then
                If myChangedByValueSetAccessor Then
                    OnValueChangedInternal(ValueChangedEventArgs.PredefinedWithPropertySetter)
                Else
                    OnValueChangedInternal(ValueChangedEventArgs.PredefinedWithUser)
                End If
            End If

            If myTextBoxDeferrer Is Nothing Then
                myTextBoxDeferrer = New TextBoxDeferrer(Me.TextBoxPart)
                myTextBoxDeferrer.DeferredTextChangeDelay = DeferredTextChangeDelay
            End If
            myTextBoxDeferrer.NoDeferOnNextTextChange = True

            'TextChanged für TextBoxPart unterdrücken, da dies
            'myValue wieder auf nothing setzen würde, was wir brauchen
            'um bei PreserveInput nur dann den Wert der Liste zu haben
            'wenn er gezielt mit Return ausgewählt wurde.
            myIgnoreNextValueReset = True
            Me.TextBoxPart.Text = Me.PopupControl.ValueText
            myIgnoreNextValueReset = False
            Me.TextBoxPart.SelectAll()

            If Not lateCommit Then
                If Not myChangedByValueSetAccessor Then
                    Me.TextBoxPart.Focus()
                End If
            End If

            If myDataGridForm.BindableDataGridView.SelectedRows.Count > 0 Then
                myCurrentRowIndex = myDataGridForm.BindableDataGridView.SelectedRows(0).Index
            Else
                myCurrentRowIndex = -1
            End If

            'Brauchen wir, um festzustellen, ob sich Value seit dem letzten Mal wirklich geändert hat.
            myValueBeforeValueChanged = Me.Value
        End If
    End Sub

    Protected Sub OnGetColumnSchema(ByVal e As GetColumnSchemaEventArgs)
        RaiseEvent GetColumnSchema(Me, e)
    End Sub

    Private Sub OnValueChangedInternal(ByVal e As ValueChangedEventArgs)
        If e.ValueChangedCause = ValueChangedCauses.User Then
            If Not Me.IsDirty Then
                IsDirty = True
            End If
        End If
        OnValueChanged(e)
    End Sub

    Protected Overridable Sub OnValueChanged(ByVal e As ValueChangedEventArgs)
        RaiseEvent ValueChanged(Me, e)
    End Sub

    Protected Overridable Sub OnSelectedValueChanged(ByVal e As EventArgs)
        If Me.InvokeRequired Then
            Me.Invoke(Sub()
                          RaiseEvent SelectedValueChanged(Me, e)
                      End Sub)
        Else
            RaiseEvent SelectedValueChanged(Me, e)
        End If
    End Sub

    Protected Overrides Sub OnPopupCreated()
        MyBase.OnPopupCreated()
    End Sub

    Protected Overrides Sub OnPopupOpening(ByVal e As PopupOpeningEventArgs)
        e.PreferredNewSize = New Size(GetPreferredWidthInternal, GetPreferredHeightInternal)
        MyBase.OnPopupOpening(e)
    End Sub

    Protected Overrides Sub OnPopupOpened(ByVal e As EventArgs)
        'HACK: Dieses Handling benötigen wir, da eine aufgeklappte Instanz des Popups "verloren" gehen kamm.
        Dim tmpValueOld = Me.BindableDataGridView.RestoreValue
        If tmpValueOld IsNot Nothing Then
            Me.BindableDataGridView.Value = tmpValueOld
            If BindableDataGridView.SelectedRows.Count > 0 Then
                myCurrentRowIndex = Me.BindableDataGridView.SelectedRows(0).Index
            End If
        End If
        BindableDataGridView.SelectValue(Me.ValueBase)
        MyBase.OnPopupOpened(e)
    End Sub

    Protected Overrides Sub OnPopupClosing(e As PopupClosingEventArgs)
        MyBase.OnPopupClosing(e)

        If e.PopupCloseReason = PopupClosingReason.Keyboard AndAlso (e.KeyData = Keys.Escape) Then
            Return
        End If

        If e.PopupCloseReason = PopupClosingReason.Keyboard AndAlso (e.KeyData = Keys.Return Or e.KeyData = Keys.Tab) OrElse
            e.PopupCloseReason = PopupClosingReason.ContentClicked Then
            If Me.PreserveInput Then
                If e.PopupCloseReason = PopupClosingReason.Keyboard AndAlso e.KeyData = Keys.Tab Then
                    TraceEx.TraceInformation("TRACING: OnPopupClosing, Keys.TAB")
                    Return
                End If
            End If
            myDataGridForm.BindableDataGridView.CommitValuesInternally()
            myValue = myDataGridForm.BindableDataGridView.Value
            If Not Object.Equals(Me.Value, myUndoValue) Then
                If Not Me.IsDirty Then
                    IsDirty = True
                    Return
                End If
            End If
        End If

        'Zurücksetzen auf Null.
        If e.PopupCloseReason = PopupClosingReason.ResetButtonClicked Then
            If Me.PreserveInput Then
                myValue = Nothing
            End If
            myDataGridForm.BindableDataGridView.CommitValuesInternally()
            If myUndoValue IsNot Nothing AndAlso Me.Value Is Nothing Then
                If Not Me.IsDirty Then
                    IsDirty = True
                    Return
                End If
            End If
        End If

    End Sub

    Protected Overrides Sub ClosePopupInternally(ClosingReason As PopupClosingEventArgs)
        If PreserveInput Then
            If ClosingReason.PopupCloseReason = PopupClosingReason.Keyboard AndAlso ClosingReason.KeyData = Keys.Tab Then
                Me.Undo()
                MyBase.ClosePopupInternally(ClosingReason)
            Else
                MyBase.ClosePopupInternally(ClosingReason)
            End If
        Else
            MyBase.ClosePopupInternally(ClosingReason)
        End If
    End Sub

    Protected Overrides Sub DecideUndoCommit(closingReason As PopupClosingEventArgs)
        'Wenn der Inhalt erhalten bleiben soll, dann müssen wir das Commit-Verhalten durch TAB ändern.
        If Me.PreserveInput Then
            'In diesem Fall löst die TAB-Taste schon an anderer Stelle ein UNDO aus, und stellt die ValueText-Eigenschaft
            'mit der entsprechenden Texteingabe her. Deswegen finden in der Basisklasse WEDER ein Commit NOCH ein Undo statt.
            If (closingReason.PopupCloseReason = PopupClosingReason.Keyboard And closingReason.KeyData = Keys.Tab) Then
                Return
            End If
        End If
        'Ansonsten wird anhand von closingReason bestimmt, ob Commit oder Undo gemacht wird.
        MyBase.DecideUndoCommit(closingReason)
    End Sub

    Private Async Function FilterBindingView() As Task

        If myFilterBindingViewInProgress Then
            myFilterTask.AssignedCancellationTokenSource.Cancel()
            Try
                Await myFilterTask.FilterTask
            Catch ex As Exception
                'Stop
            End Try
        End If

        myFilterTask = New FilterBindingViewTask
        myFilterTask.AssignedCancellationTokenSource = New CancellationTokenSource
        myFilterTask.FilterTask = Task.Factory.StartNew(
                        Sub()
                            FilterBindingViewAsync(myFilterTask.AssignedCancellationTokenSource.Token)
                        End Sub, myFilterTask.AssignedCancellationTokenSource.Token)
    End Function

    Private Class SyncLockedArrayList
        Inherits ArrayList

        Public Overrides Function Add(value As Object) As Integer
            SyncLock Me
                Return MyBase.Add(value)
            End SyncLock
        End Function
    End Class

    Private Sub FilterBindingViewAsync(ct As CancellationToken)
        Try
            myFilterBindingViewInProgress = True

            If myIsInitializing Then Return

            ct.ThrowIfCancellationRequested()

            If String.IsNullOrEmpty(Me.Text) Then
                Me.Invoke(Sub()
                              myDataGridForm.BindableDataGridView.DataSource = myDataSource
                          End Sub)
                Return
            End If

            Dim tempDataSource = New SyncLockedArrayList
            Dim tmpAutoCompleteList = rebuildAutoCompleteList()
            Dim arbitraryItem As Object = Nothing

            'Keine Datasource, keine AutoCompleteList
            If tmpAutoCompleteList Is Nothing Then
                myCurrentAutoCompleteListAndSchemaInfo = Nothing
                Exit Sub
            End If

            'Dim test = "Ga" 'reth+Clarke;Löffelmann+Lippstadt;Dortmund"
            Dim AndList = Me.Text.ToUpper.Split(Me.SearchKeywordAndChar)
            'Dim AndList = test.ToUpper.Split(Me.SearchKeywordAndChar)

            myCurrentAutoCompleteListAndSchemaInfo = tmpAutoCompleteList
            If myOldCurrentAutoCompleteListAndSchemaInfo Is Nothing OrElse
            (Not myCurrentAutoCompleteListAndSchemaInfo.Item1.SequenceEqual(myOldCurrentAutoCompleteListAndSchemaInfo)) Then
                myOldCurrentAutoCompleteListAndSchemaInfo = myCurrentAutoCompleteListAndSchemaInfo.Item1
                UpdateColumnHeadersAndBackgroundColor()
            End If

            Dim itemTemp As AutoCompleteLookupItem = Nothing

            If myCurrentAutoCompleteListAndSchemaInfo.Item2 IsNot Nothing AndAlso myCurrentAutoCompleteListAndSchemaInfo.Item2.Count > 0 Then
                arbitraryItem = myCurrentAutoCompleteListAndSchemaInfo.Item2(0).Row
            End If

            Dim filterLambda = Sub(item As AutoCompleteLookupItem)
                                   itemTemp = item
                                   Dim textToSearch = itemTemp.SearchText.ToUpper

                                   Dim FoundFlagAnd = True
                                   Dim foundFlagOr = False
                                   For Each andItem In AndList
                                       foundFlagOr = False
                                       For Each orItem In andItem.Split(Me.SearchKeywordOrChar)
                                           If textToSearch.IndexOf(orItem.ToUpper) > -1 Then
                                               foundFlagOr = foundFlagOr Or True
                                           End If
                                           If ct.IsCancellationRequested Then Exit For
                                       Next
                                       FoundFlagAnd = FoundFlagAnd And foundFlagOr
                                   Next

                                   If FoundFlagAnd Then
                                       tempDataSource.Add(item.Row)
                                   End If
                                   ct.ThrowIfCancellationRequested()
                               End Sub

            If Me.ParallelizeFiltering Then
                Parallel.ForEach(myCurrentAutoCompleteListAndSchemaInfo.Item2, filterLambda)
            Else
                For Each item In myCurrentAutoCompleteListAndSchemaInfo.Item2
                    filterLambda(item)
                Next
            End If

            mySupressSelectedValueChange = True
            If tempDataSource.Count = 0 Then
                'Wir dürfen an dieser Stelle die Datasource nicht löschen,
                'da sonst die Schemainfo ebenfalls verschwindet.

                'Aber: Die DataSource muss mindestens ein Element haben - warum auch immer!
                tempDataSource.Add(arbitraryItem)
                myDataGridForm.Invoke(Sub()
                                          myDataGridForm.BindableDataGridView.DataSource = tempDataSource
                                          myDataGridForm.BindableDataGridView.ClearRows()
                                      End Sub)

            Else
                myDataGridForm.Invoke(Sub()
                                          myDataGridForm.BindableDataGridView.DataSource = tempDataSource
                                      End Sub)
            End If

            mySupressSelectedValueChange = False
            If myDataGridForm.BindableDataGridView.Rows.Count > 0 Then
                Dim firstRow = myDataGridForm.BindableDataGridView.Rows(0)
                myDataGridForm.BindableDataGridView.SelectRow(firstRow)
            End If

            If Not Object.Equals(myOldSelectedValue, BindableDataGridView.Value) Then
                myOldSelectedValue = Me.myDataGridForm.BindableDataGridView.Value
                OnSelectedValueChanged(EventArgs.Empty)
            End If
        Catch ex As Exception

        Finally
            myFilterBindingViewInProgress = False
        End Try
    End Sub

    Protected Overrides Sub OnFontChanged(e As System.EventArgs)
        MyBase.OnFontChanged(e)
        Me.SetBoundsCore(Me.Location.X, Me.Location.Y, Me.Width, Me.Height, BoundsSpecified.Size)
        PositionControls()
    End Sub

    Protected Overrides Sub SetBoundsCore(x As Integer, y As Integer, width As Integer, height As Integer, specified As System.Windows.Forms.BoundsSpecified)
        If specified = BoundsSpecified.Size Then
            height = Me.PreferredHeight
        End If
        MyBase.SetBoundsCore(x, y, width, height, specified)
    End Sub

    Private Sub ResetBindingView()
        myDataGridForm.BindableDataGridView.ResetDatasource(myDataSource)
    End Sub

    Protected Overrides Sub OnMouseWheel(e As MouseEventArgs)
        MyBase.OnMouseWheel(e)
        If Me.IsPopupOpen Then
            Try
                Me.BindableDataGridView.FirstDisplayedScrollingRowIndex = Me.BindableDataGridView.FirstDisplayedScrollingRowIndex - (e.Delta \ 40)
            Catch ex As Exception
            End Try
        End If
    End Sub

    Protected Overrides Sub OnKeyDown(ByVal e As System.Windows.Forms.KeyEventArgs)
        MyBase.OnKeyDown(e)
        HandleKeyDownInternally(e)
    End Sub

    Private Sub HandleKeyDownInternally(Optional e As KeyEventArgs = Nothing)
        If Me.IsPopupOpen Then

            If e.KeyData = Keys.Down Then
                'Wenn die Basisklasse das schon behandelt hat,
                'ist damit das Popup geöffnet worden und wir sind durch an dieser Stelle.
                If e.Handled Then
                    EnsureOneRowIsSelected(False)
                    Return
                End If

                e.Handled = True

                If EnsureOneRowIsSelected(True) Then
                    Dim nextVisibleRow = Me.BindableDataGridView.FindNextVisibleRow
                    If nextVisibleRow Is Nothing Then Return

                    Dim selRow = Me.BindableDataGridView.SelectedRows(0)
                    'Den ersten SelectedValueChanged ignorieren, wenn die Zeile selektiert war.
                    myIgnoreNextSelectedValueChange = selRow.Selected
                    selRow.Selected = False

                    ''Den zweiten SelectedValueChanged ignorieren
                    'myIgnoreNextSelectedValueChange = True
                    Me.BindableDataGridView.SelectRow(nextVisibleRow)
                    e.Handled = True
                End If

            ElseIf e.KeyData = Keys.Up Then

                e.Handled = True

                If EnsureOneRowIsSelected(True) Then
                    Dim previousSelectedRow = Me.BindableDataGridView.FindPreviousVisibleRow
                    If previousSelectedRow IsNot Nothing Then
                        Dim selRow = Me.BindableDataGridView.SelectedRows(0)
                        'Den ersten SelectedValueChanged ignorieren
                        myIgnoreNextSelectedValueChange = selRow.Selected
                        selRow.Selected = False

                        ''Den zweiten SelectedValueChanged ignorieren
                        'myIgnoreNextSelectedValueChange = True
                        Me.BindableDataGridView.SelectRow(previousSelectedRow)
                    End If
                End If

            ElseIf e.KeyData = Keys.PageDown Then
                If EnsureOneRowIsSelected(True) Then
                    Dim visableRows = Me.BindableDataGridView.DisplayedRowCount(False)
                    For c = 1 To visableRows
                        Dim nextVisibleRow = Me.BindableDataGridView.FindNextVisibleRow
                        If nextVisibleRow Is Nothing Then Return

                        Dim selRow = Me.BindableDataGridView.SelectedRows(0)
                        'Den ersten SelectedValueChanged ignorieren
                        myIgnoreNextSelectedValueChange = True
                        selRow.Selected = False

                        'Den zweiten SelectedValueChanged ignorieren
                        myIgnoreNextSelectedValueChange = True
                        Me.BindableDataGridView.SelectRow(nextVisibleRow)
                    Next
                    e.Handled = True
                End If
            End If
            myTextDiff = Me.Text
        End If
    End Sub

    Private Function EnsureOneRowIsSelected(ignoreNextSelectedValueChange As Boolean) As Boolean
        If Me.BindableDataGridView.SelectedRows.Count = 0 Then
            For Each rowItem As DataGridViewRow In Me.BindableDataGridView.Rows
                If rowItem.Visible Then
                    myIgnoreNextSelectedValueChange = ignoreNextSelectedValueChange
                    Me.BindableDataGridView.SelectRow(rowItem)
                    Exit For
                End If
            Next
            Return False
        End If
        Return True
    End Function

    Private Function GetPreferredWidthInternal() As Integer
        Dim tGrid = myDataGridForm.BindableDataGridView
        If Not myDataGridForm.BindableDataGridView.IsHandleCreated Then
            IsLoading.Value = True
            'CreateControl ruft SelctionChange aus, was eigentlich zum IsDirty=true führt.
            'Darf hier aber nicht passieren, da es das initial-SelectionChange ist.
            myDataGridForm.CreateControl()
            IsLoading.Value = False
        End If

        If tGrid.VisibleColumnCount = 0 Then
            Return Me.PopupSize.Width
        End If

        If Me.PreferredVisibleColumnsOnOpen = 0 Then
            Return Me.PopupSize.Width
        End If

        Dim maxColumns = Me.PreferredVisibleColumnsOnOpen
        If maxColumns > tGrid.VisibleColumnCount Then
            maxColumns = tGrid.VisibleColumnCount
        End If

        Dim currentWidth = SCROLLBAROFFSETWIDTH
        Dim totalWidth = 0
        If Me.AutoResizeColumnsOnOpen Then

            For Each itemColoumn As DataGridViewColumn In tGrid.Columns
                If itemColoumn.Visible Then
                    tGrid.AutoResizeColumn(itemColoumn.Index)
                End If
            Next
            tGrid.Update()
        End If

        Dim gridCount = 0
        For Each itemColoumn As DataGridViewColumn In tGrid.Columns
            If itemColoumn.Visible Then
                totalWidth += itemColoumn.Width + itemColoumn.DividerWidth
                gridCount += 1
                If gridCount = maxColumns Then
                    currentWidth = totalWidth
                End If
                If itemColoumn.FillWeight > 0 And itemColoumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill Then
                    totalWidth = Me.PopupSize.Width
                    currentWidth = totalWidth
                    Exit For
                End If
            End If
        Next

        If currentWidth < Me.MinimumPopupSize.Width Then
            currentWidth = Me.MinimumPopupSize.Width
        End If

        If currentWidth > Me.MaximumPopupSize.Width Then
            currentWidth = Me.MaximumPopupSize.Width
        End If

        'Wenn alle angezeigten Spalten schmaler als die Gesamtbreite sind, 
        'dann wird die erste Palte um die fehlenden Pixel vergrößert
        Dim newCurrentWidth = currentWidth - SCROLLBAROFFSETWIDTH
        If tGrid.RowCount > Me.PreferredVisibleRowsOnOpen Then
            newCurrentWidth -= SCROLLBARWIDTH
        End If

        If totalWidth < newCurrentWidth Then
            Dim newAdditionalWidth = (newCurrentWidth - totalWidth)

            For Each itemColoumn As DataGridViewColumn In tGrid.Columns
                If itemColoumn.Visible Then
                    itemColoumn.Width += CInt(newAdditionalWidth)
                    Exit For
                End If
            Next
        End If

        ''Wenn die PopUpBox größer als das Control, und eine Schrollbar angezeigt wird muss das PopUp
        ''um die Breite der Scrollbar vergrößert werden
        If currentWidth > Me.MinimumPopupSize.Width AndAlso tGrid.RowCount > Me.PreferredVisibleRowsOnOpen Then
            currentWidth += SCROLLBARWIDTH
        End If

        Return currentWidth
    End Function

    Private Function GetPreferredHeightInternal() As Integer
        Dim tGrid = myDataGridForm.BindableDataGridView

        If tGrid.RowCount = 0 Then
            Return Me.PopupSize.Height
        End If

        If Me.PreferredVisibleRowsOnOpen = 0 Then
            Return Me.PopupSize.Height
        End If

        Dim maxRows = Me.myPreferredVisibleRowsOnOpen

        Dim currentHeight = 0
        If tGrid.ColumnHeadersVisible Then
            'currentHeight = tGrid.ColumnHeadersHeight
        End If

        If maxRows > tGrid.Rows.Count Then
            maxRows = tGrid.Rows.Count
        End If

        For gridCount = 0 To maxRows - 1
            currentHeight += tGrid.Rows(gridCount).Height + tGrid.Rows(gridCount).DividerHeight
        Next

        currentHeight += STATUSBAROFFSETHEIGHT
        If Me.PreferredVisibleRowsOnOpen < tGrid.Rows.Count Then
            currentHeight += SCROLLBAROFFSETHEIGHT
        End If

        If currentHeight < Me.MinimumPopupSize.Height Then
            currentHeight = Me.MinimumPopupSize.Height
        End If

        If currentHeight > Me.MaximumPopupSize.Height Then
            currentHeight = Me.MaximumPopupSize.Height
        End If

        If tGrid.VisibleColumnCount > Me.PreferredVisibleColumnsOnOpen Then
            currentHeight += SCROLLBARWIDTH
        End If

        Return currentHeight + SCROLLBAROFFSETHEIGHT
    End Function

    Protected Overrides Function GetPopupContent() As System.Windows.Forms.Control
        If myDataGridForm Is Nothing Then
            myDataGridForm = New DataGridViewPopupContent
        End If
        Return myDataGridForm
    End Function

    Protected Overrides Sub OnEnter(ByVal e As System.EventArgs)
        MyBase.OnEnter(e)
        If Me.OnFocusColor Then
            myOldBackColor = Me.TextBoxPart.BackColor
            Me.TextBoxPart.BackColor = Me.FocusColor
        End If

        If Not String.IsNullOrEmpty(Me.NullValueString) Then
            If Me.TextBoxPart.Text = Me.NullValueString Then
                myTextBoxDeferrer.IgnoreNextTextChange = True
                Me.TextBoxPart.Text = ""
            End If
        End If
        Me.TextBoxPart.SelectAll()
        myUndoValue = Value
    End Sub

    Protected Overrides Sub OnGotFocus(ByVal e As System.EventArgs)
        MyBase.OnGotFocus(e)
    End Sub

    Protected Overrides Sub [Select](directed As Boolean, forward As Boolean)
        MyBase.[Select](directed, directed)
        Me.TextBoxPart.Focus()
    End Sub

    Protected Overrides Sub OnTextChanged(ByVal e As System.EventArgs)
        MyBase.OnTextChanged(e)
    End Sub

    Protected Overridable Async Sub OnDeferralTextChanged(sender As Object, e As EventArgs)
        If Me.DesignMode Then
            Return
        End If

        Try
            'Flag, dass TextChange übergangen werden soll für ein einziges Mal.

            'Wenn TextChange durch SetAccessor von Value gesetzt wurde,
            'ebenfalls Sonderbehandlung. Dann darf das Menü nicht aufklappen.
            If myChangedByValueSetAccessor Then
                'HACK: Das könnte an dieser Stelle schon redundant sein:
                myTextDiff = Me.Text
                Return
            End If

            If String.IsNullOrWhiteSpace(Me.TextBoxPart.Text) Then
                Me.BindableDataGridView.ClearSelection()
                If myTextDiff <> Me.Text Then
                    If Not Me.IsPopupOpen Then
                        Me.OpenPopup()
                    End If
                    Await FilterBindingView()
                End If
                myTextDiff = ""
            End If

            'Wenn der Ausgangstext ein anderer ist, als der veränderte,
            'müssen wir Filtern.
            If myTextDiff <> Me.Text Then
                If Not Me.IsPopupOpen Then
                    Me.OpenPopup()
                End If
                Await FilterBindingView()
            End If
        Catch ex As Exception
            TraceEx.TraceError("OnDeferralTextChange caused an " & ex.GetType.ToString & " in EntityFormsLib. Message: " & ex.Message)
        End Try
    End Sub

    Protected Overrides Sub OnLostFocus(ByVal e As System.EventArgs)
        MyBase.OnLostFocus(e)
    End Sub

    Protected Overrides Sub CommitOnLeave()
        'Wenn wir das nicht abfangen, wird bei OnLeave auch wieder in jedem
        'Fall ein Commit durchgeführt, und das muss verhindert werden.
        If Me.PreserveInput Then
            CommitOnLeaveForPreserveInput()
        Else
            MyBase.CommitOnLeave()
        End If
    End Sub

    ''' <summary>
    ''' Wird bei PreserveInput=True dann aufgerufen, wenn ein LostFocus erfolgt, und der Wert spät commited werden muss.
    ''' </summary>
    ''' <remarks>Dieser Methodenaufruf wird wichtig, wenn ein early-commit durch direkte Werteauswahl stattgefunden hat,
    ''' der Anwender danach das Eingabefeld gelöscht und damit die Werteauswahl deselektiert hat.</remarks>
    Protected Overridable Sub CommitOnLeaveForPreserveInput()
        MyBase.Commit(True)
        myTextBoxDeferrer.IgnoreNextTextChange = True

        If myDataGridForm.BindableDataGridView.Value Is Nothing Then
            Me.PopupControl.ValueText = Me.TextBoxPart.Text
        End If

        'ValueChanged nur auslösen, wenn sich Value wirklich geändert hat.
        If Not Object.Equals(myValueBeforeValueChanged, Me.Value) Then
            If myChangedByValueSetAccessor Then
                OnValueChangedInternal(ValueChangedEventArgs.PredefinedWithPropertySetter)
            Else
                OnValueChangedInternal(ValueChangedEventArgs.PredefinedWithUser)
            End If
        End If

        If myDataGridForm.BindableDataGridView.SelectedRows.Count > 0 Then
            myCurrentRowIndex = myDataGridForm.BindableDataGridView.SelectedRows(0).Index
        Else
            myCurrentRowIndex = -1
        End If

        'Brauchen wir, um festzustellen, ob sich Value seit dem letzten Mal wirklich geändert hat.
        myValueBeforeValueChanged = Me.Value

        ResetBindingView()
        myUndoValue = Value
        myTextBoxDeferrer.IgnoreNextTextChange = False
    End Sub

    Protected Overrides Sub OnLeave(ByVal e As System.EventArgs)

        If Me.PopupControl Is Nothing Then
            Return
        End If

        MyBase.OnLeave(e)

        If String.IsNullOrEmpty(Me.TextBoxPart.Text) Then
            myTextBoxDeferrer.IgnoreNextTextChange = True
            If Me.PopupControl IsNot Nothing Then
                Me.PopupControl.ValueText = Nothing
            End If
        End If

        If Me.OnFocusColor Then
            Me.TextBoxPart.BackColor = myOldBackColor
        End If
    End Sub

    Public Overrides Sub Undo()
        If Me.PreserveInput Then
            'Text soll bestehen bleiben
            Me.PopupControl.ValueText = Me.TextBoxPart.Text
        Else
            ResetBindingView()
            Value = myUndoValue
        End If
    End Sub

    ''' <summary>
    ''' Wird aufgerufen, wenn der Wert im Control entweder durch FocusLost oder 
    ''' durch Auswahl des Wertes in der Liste final bestimmt wird.
    ''' </summary>
    ''' <param name="lateCommit">Flag, das anzeigt, ob das Commiten des Wertes früh (durch Übernahme aus der Liste) 
    ''' oder spät und impliziet (durch FocusLost) erfolgte.</param>
    ''' <remarks></remarks>
    Public Overrides Sub Commit(lateCommit As Boolean)
        MyBase.Commit(lateCommit)
        myTextBoxDeferrer.IgnoreNextTextChange = True
        HandleSelectedValueChanged(lateCommit)
        ResetBindingView()
        myUndoValue = Value
        myTextBoxDeferrer.IgnoreNextTextChange = False
    End Sub

    Protected Overrides Sub OnValidating(ByVal e As System.ComponentModel.CancelEventArgs)
        MyBase.OnValidating(e)
        If Me.DesignMode Then
            Return
        End If

        If e.Cancel Then
            Return
        End If

        ValidateInternal(e)
    End Sub

    'WICHTIG: Sollte diese Verhaltensweise an dieser Stelle geändert werden müssen,
    'wäre es wichtig zu schauen, ob die ähnlich implementierte Verhaltensweise auch
    'bei NullableValueBase angepasst werden muss.
    Private Sub ValidateInternal(ByVal e As CancelEventArgs)

        myLastFailedValidationException = Nothing

        'ValidateInput überprüft, ob die Eingabe i.O. geht, und liefert 
        'bei einem Fehler eine Exception zurück, die letzten Endes
        'auch den Fehlertext ergibt.
        If (Me.Value Is Nothing OrElse String.IsNullOrWhiteSpace(Me.Value.ToString)) And
            Not String.IsNullOrWhiteSpace(Me.NullValueMessage) Then
            myLastFailedValidationException = New ContainsUIMessageException(Me.NullValueMessage, Me.NullValueMessage)
        End If

        If myLastFailedValidationException IsNot Nothing Then

            'Bei Bedarf, kann hier ein Warnton ausgegeben werden.
            If BeepOnFailedValidation Then
                Beep()
            End If

            Dim vfe = New RequestValidationFailedReactionEventArgs(ValidationFailedActions.ForceKeepFocus)
            vfe.BallonMessage = myLastFailedValidationException.UIMessage
            vfe.CausingException = myLastFailedValidationException

            NullableValueExtender.ValidationTooltipHandler(Me, vfe)

            'Rausfinden, wie wir uns Verhalten sollen, weil das Validieren fehlgeschlagen ist.
            '(kann ja nicht immer sein, dass der Anwender im Feld gefangen bleibt, zum Beispiel bei Cancel des Forms, wenn sich 
            'das durch CauseValidation nicht regeln lässt).
            OnRequestValidationFailedReaction(vfe)

            'Focusverhalten überprüfen
            If (vfe.ValidationFailedReaction And ValidationFailedActions.ForceKeepFocus) = ValidationFailedActions.ForceKeepFocus Then
                e.Cancel = True
            Else
                e.Cancel = False
            End If
        End If
    End Sub

    Protected Sub OnRequestValidationFailedReaction(ByVal e As RequestValidationFailedReactionEventArgs)
        RaiseEvent RequestValidationFailedReaction(Me, e)
    End Sub

    'Refaktoriert: Lieferte früher die nur Lookup-Items zurück. Da wir an dieser Stelle die Schema-Infos
    'benötigen, liefert diese Methode jetzt auch die Namen der Methoden und damit Felder zurück, in denen gesucht wird (erster Tuple-Wert).
    Private Function rebuildAutoCompleteList() As Tuple(Of List(Of String), List(Of AutoCompleteLookupItem))

        'Control wird noch initialisiert - wir machen nix.
        If myIsInitializing Then
            Return Nothing
        End If

        'Keine Datenquelle, wir können nicht suchen.
        If myDataSource Is Nothing Then
            Return Nothing
        End If

        'Kein SearchPattern, Suche geht auch dann nicht.
        If String.IsNullOrEmpty(SearchPattern) Then
            Return Nothing
        End If

        Dim tmpAutoCompleteLookupList As List(Of AutoCompleteLookupItem)

        tmpAutoCompleteLookupList = New List(Of AutoCompleteLookupItem)(DirectCast(myDataSource, ICollection).Count)
        Dim ow As ObjectWrapper = Nothing
        For Each item In DirectCast(Me.DataSource, ICollection)
            If ow Is Nothing Then
                ow = ObjectAnalyser.ObjectToFunctionWrapper(item, Me.SearchPattern)
                ow.SchemaControlDisabled = True
            End If
            ow.DataBoundItem = item
            Dim tmpItem = New AutoCompleteLookupItem With {.Row = item,
                                                           .SearchText = ow.ToString}

            tmpAutoCompleteLookupList.Add(tmpItem)
        Next

        If ow Is Nothing Then
            Return Nothing
        End If

        Dim retSchemaInfoList = (From item In ow.LambdaMethodWrappers Select item.MethodOrPropertyname).ToList

        Return New Tuple(Of List(Of String), List(Of AutoCompleteLookupItem))(retSchemaInfoList, tmpAutoCompleteLookupList)
    End Function

    Private Sub UpdateColumnHeadersAndBackgroundColor()
        If myIsInitializing Then Return

        If myCurrentAutoCompleteListAndSchemaInfo IsNot Nothing Then

            Dim highlightedHeaderStyle As New DataGridViewCellStyle(
                        Me.BindableDataGridView.DefaultCellStyle) With {.Font = Me.SearchColumnHeaderFont,
                                                                        .BackColor = Me.SearchColumnHeaderBackgroundColor}

            Dim highlightedColumnStyle As New DataGridViewCellStyle(Me.BindableDataGridView.DefaultCellStyle) With {.BackColor = Me.SearchColumnBackgroundColor}

            'Dim highlightedCellStyle=New DataGridViewCellStyle(New.BindableDataGridView.Defaul)
            For Each dgvColumn As DataGridViewColumn In Me.BindableDataGridView.Columns
                If myCurrentAutoCompleteListAndSchemaInfo.Item1.Contains(dgvColumn.DataPropertyName) Then
                    dgvColumn.DefaultCellStyle.ApplyStyle(highlightedColumnStyle)
                    dgvColumn.HeaderCell.Style = highlightedHeaderStyle
                Else
                    dgvColumn.DefaultCellStyle.ApplyStyle(Me.BindableDataGridView.DefaultCellStyle)
                    dgvColumn.HeaderCell.Style = BindableDataGridView.ColumnHeadersDefaultCellStyle
                End If
            Next
        End If
    End Sub

    'Wir müssen dieses Pattern implementieren, da das Setzen zuvieler Eigenschaften
    'nach dem Setzen der DataSource zur ständigen Neugenerierung der Suchliste 
    'führen würde, was einen erheblichen Performanceeinbruch zur Folge haben könnte.

    'BeginInit in ISupportInitialize sorgt dafür, dass entsprechender Designer-Code
    'generiert wird, der EndInit aufruft, wenn ALLE Eigenschften des Controls gesetzt wurden.
    Public Sub BeginInit() Implements System.ComponentModel.ISupportInitialize.BeginInit
        myIsInitializing = True
    End Sub

    Public Sub EndInit() Implements System.ComponentModel.ISupportInitialize.EndInit
        If myIsInitializing Then
            myIsInitializing = False
            myCurrentAutoCompleteListAndSchemaInfo = rebuildAutoCompleteList()
            myOldCurrentAutoCompleteListAndSchemaInfo = Nothing
            UpdateColumnHeadersAndBackgroundColor()
        End If
    End Sub

    Public Overrides Function ToString() As String
        Return Me.GetType.Name & " - Name: " & If(String.IsNullOrWhiteSpace(Me.Name), "- - -", Me.Name) &
            "; Wert: " & If(Me.Value IsNot Nothing, Me.Value.ToString, Me.NullValueString)
    End Function

    ''' <summary>
    ''' Bestimmt oder ermittelt die Zeichenfolge im Eingabebereich des Popup-Controls.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
     Description("Bestimmt oder ermittelt die Zeichenfolge im Eingabebereich des Popup-Controls."),
     Category("Darstellung"),
     EditorBrowsable(EditorBrowsableState.Never),
     Browsable(False)>
    Public Overrides Property Text As String
        Get
            Return MyBase.Text
        End Get
        Set(ByVal value As String)
            myTextBoxDeferrer.IgnoreNextTextChange = True
            myTextDiff = value
            MyBase.Text = value
        End Set
    End Property

    ''' <summary>
    ''' Bestimmt oder ermittelt, was passieren soll, wenn Value ein Wert zugewiesen wird, der in der Liste keinem Eintrag entspricht.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DefaultValue(GetType(UnassignableValueAction), "TryHandleUnassignableValueDetectedEvent"),
     Description("Bestimmt oder ermittelt, was passieren soll, wenn Value ein Wert zugewiesen wird, der in der Liste keinem Eintrag entspricht."),
     Category("Behavior"),
     EditorBrowsable(EditorBrowsableState.Always),
     Browsable(True)>
    Public Property OnUnassignableValueAction As UnassignableValueAction
        Get
            Return myOnUnassignableValueAction
        End Get
        Set(ByVal value As UnassignableValueAction)
            myOnUnassignableValueAction = value
            If BindableDataGridView IsNot Nothing Then
                BindableDataGridView.OnUnassignableValueAction = myOnUnassignableValueAction
            End If
        End Set
    End Property

    ''' <summary>
    ''' Bestimmt oder ermittelt, ob das Steuerelement mit FocusColor eingefärbt werden soll, wenn das Steuerelement den Fokus erhält. 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
     Description("Bestimmt oder ermittelt, ob das Steuerelement mit FocusColor eingefärbt werden soll, wenn das Steuerelement den Fokus erhält."),
     Category("Behavior"),
     EditorBrowsable(EditorBrowsableState.Always),
     Browsable(True)>
    Public Property OnFocusColor As Boolean
        Get
            Return myOnFocusColor
        End Get
        Set(ByVal value As Boolean)
            myOnFocusColor = value
        End Set
    End Property

    Public Function ShouldSerializeOnFocusColor() As Boolean
        Return Not (OnFocusColor = GetDefaultOnFocusColor())
    End Function

    Protected Overridable Function GetDefaultOnFocusColor() As Boolean
        Return True
    End Function

    ''' <summary>
    ''' Bestimmt oder ermittelt die Farbe, die im Bedarfsfall vorselektiert werden soll, wenn das Steuerelement den Fokus erhält. 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
     Description("Bestimmt oder ermittelt die Farbe, die im Bedarfsfall vorselektiert werden soll, wenn das Steuerelement den Fokus erhält."),
     Category("Behavior"),
     EditorBrowsable(EditorBrowsableState.Always),
     Browsable(True)>
    Public Property FocusColor As Color
        Get
            Return myFocusColor
        End Get
        Set(ByVal value As Color)
            myFocusColor = value
        End Set
    End Property

    Public Function ShouldSerializeFocusColor() As Boolean
        Return Not FocusColor = GetDefaultFocusColor()
    End Function

    Protected Overridable Function GetDefaultFocusColor() As Color
        Return Color.Yellow
    End Function

    ''' <summary>
    ''' Bestimmt oder ermittelt die Zeichenfolge, die beim Verlassen des Steuerelements angezeigt wird, wenn eine Null-Eingabe erfolgte.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
     Description("Bestimmt oder ermittelt die Zeichenfolge, die beim Verlassen des Steuerelements angezeigt wird, wenn eine Null-Eingabe erfolgte."),
     Category("Behavior"),
     EditorBrowsable(EditorBrowsableState.Always),
     Browsable(True)>
    Public Property NullValueString As String

    ''' <summary>
    ''' Bestimmt oder ermittelt den Namen der Eigenschaft, deren Inhalt die Value-Eigenschaft 
    ''' beim Auswählen eines Elements der Liste zurückliefert.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks><para>Ist diese Eigenschaft nicht gesetzt, wird das komplette Listenelement zurückgeliefert. 
    ''' Falls diese Eigenschaft gesetzt ist, versucht das Steuerelement den Inhalt der entsprechenden Eigenschaft 
    ''' zu ermitteln, und liefert diesen über die Value-Eigenschaft zurück.
    ''' </para>
    ''' <para>Wichtig: Die Eigenschaft, die verwendet werden soll, sollte auf jedenfall eindeutige (also keine doppelten) 
    ''' Werte in der Liste zurückliefern, da beim Zuweisen sonst u.U. falsche Elemente in der Liste selektiert werden.</para>
    ''' </remarks>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
     Description("Bestimmt oder ermittelt den Namen der Eigenschaft, deren Inhalt die Value-Eigenschaft beim Auswählen eines Elements der Liste zurückliefert."),
     Category("Behavior"),
     EditorBrowsable(EditorBrowsableState.Always),
     Browsable(True)>
    Public Property ValueMember As String Implements INullableValueRelationBinding.ValueMember
        Get
            Return Me.myDataGridForm.BindableDataGridView.ValueMember
        End Get
        Set(ByVal value As String)
            If value <> Me.myDataGridForm.BindableDataGridView.ValueMember Then
                Me.myDataGridForm.BindableDataGridView.ValueMember = value
            End If
        End Set
    End Property

    ''' <summary>
    ''' Bestimmt oder ermittelt den in der TextBox dieses Steuerelementes angezeigten Wert, 
    ''' nachdem ein Elemente in der Liste ausgewählt wurde.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks><para>Der DisplayMember wird aus mindestens zwei Teilen als Zeichenfolge angegeben werden, 
    ''' die durch Komma getrennt werden. Der erste Teil - in Anführungszeichen - definiert die Formatierung, 
    ''' der zweite Teil, welche Eigenschaften der Datenquelle angezeigt werden sollen. 
    ''' Eigenschaftennamen werden in geschwifte Klammern gesetzt. Das ist wichtig, um zu einem späteren Zeitpunkt 
    ''' hier noch um Formelauswertungsfunktioanlitäten zu erweitern.
    ''' </para>
    ''' <para>Beispiel: Kundennummer 6-stellig und Betrag mit zwei Nachkomma, Tausendertrennzeichen und Euro-Symbol darstellen:</para>
    ''' <code>
    ''' Me.DisplayMember = """{0:000000}: {1:#,##0.00} €"", {Kundennummer},{Betrag}"
    ''' </code>
    ''' <para>Beispiel: Kundennummer 6-stellig, mit anschließendem Doppelpüunkt, Leerzeichen,
    ''' dann Nachname, Komma und Vorname:</para>
    ''' <code>
    ''' Me.DisplayMember = """{0:000000}: {1}, {2}"", {Kundennummer},{Nachname},{Vorname}"
    ''' </code>
    ''' </remarks>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
     Description("Bestimmt oder Ermittelt den ausgeschriebenen/lolkalisierten Namen des Feldes, mit dem dieses Steuerelement verknüpft werden soll." & vbNewLine &
        "Zum Beispiel: " & """{0:000000}: {1}, {2}"", {Kundennummer},{Nachname},{Vorname}"),
     Category("Behavior"),
     EditorBrowsable(EditorBrowsableState.Always),
     Browsable(True)>
    Public Property DisplayMember As String
        Get
            Return myDisplayMember
        End Get
        Set(value As String)
            If value Is Nothing AndAlso myDisplayMember Is Nothing Then
                Return
            End If

            myDisplayMember = value
            If mySearchPattern Is Nothing Then
                'DisplayMember hat SearchPattern implizit geändert:
                myCurrentAutoCompleteListAndSchemaInfo = rebuildAutoCompleteList()
                myOldCurrentAutoCompleteListAndSchemaInfo = Nothing
                UpdateColumnHeadersAndBackgroundColor()
            End If
        End Set
    End Property

    ''' <summary>
    ''' Bestimmt oder ermittelt die Eigenschaften, in denen in der Liste nach Begriffen gesucht werden soll.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>
    ''' <para>SearchPattern wird wie DisplayMember parametrisiert und erbt leitet seine Einstellungen solange von 
    ''' Displaymember ab, bis eigene Einstellungen für SearchPattern definiert wurden.
    ''' </para>
    ''' <para>Der SearchPattern-String wird aus mindestens zwei Teilen als Zeichenfolge angegeben werden, 
    ''' die durch Komma getrennt werden. Der erste Teil - in Anführungszeichen - definiert die Formatierung, 
    ''' der zweite Teil, welche Eigenschaften der Datenquelle angezeigt werden sollen. 
    ''' Eigenschaftennamen werden in geschwifte Klammern gesetzt. Das ist wichtig, um zu einem späteren Zeitpunkt 
    ''' hier noch um Formelauswertungsfunktioanlitäten zu erweitern.
    ''' </para>
    ''' <para>Beispiel: Kundennummer 6-stellig und Betrag mit zwei Nachkomma, Tausendertrennzeichen und Euro-Symbol darstellen:</para>
    ''' <code>
    ''' Me.SearchPattern = """{0:000000}: {1:#,##0.00} €"", {Kundennummer},{Betrag}"
    ''' </code>
    ''' <para>Beispiel: Kundennummer 6-stellig, mit anschließendem Doppelpüunkt, Leerzeichen,
    ''' dann Nachname, Komma und Vorname:</para>
    ''' <code>
    ''' Me.SearchPattern = """{0:000000}: {1}, {2}"", {Kundennummer},{Nachname},{Vorname}"
    ''' </code>
    ''' </remarks>
    Public Property SearchPattern As String
        Get
            If String.IsNullOrEmpty(mySearchPattern) Then
                Return DisplayMember
            Else
                Return mySearchPattern
            End If
        End Get
        Set(ByVal value As String)
            If value = DisplayMember Then
                mySearchPattern = Nothing
            Else
                If value <> mySearchPattern Then
                    mySearchPattern = value
                End If
            End If
            myCurrentAutoCompleteListAndSchemaInfo = rebuildAutoCompleteList()
            myOldCurrentAutoCompleteListAndSchemaInfo = Nothing
            UpdateColumnHeadersAndBackgroundColor()
        End Set
    End Property

    Function ShouldSerializeSearchPattern() As Boolean
        Return Not String.IsNullOrEmpty(mySearchPattern)
    End Function

    Sub ResetSearchPattern()
        mySearchPattern = Nothing
    End Sub

    ''' <summary>
    ''' Bestimmt oder Ermittelt den ausgeschriebenen/lolkalisierten Namen des Feldes, mit dem dieses Steuerelement verknüpft werden soll.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>Wenn ein bindbares Feld in einem Tabellenschema beispielsweise den Namen ZipCity trägt, könnte 
    ''' diese Eigenschaft "Postleitzahl/Ort" lauten. Diese Eigenschaft spiegelt also die Bezeichnung des 
    ''' Datenfeldnamens in echter Sprache wider, sodass sie verständlich für den Endanwender wird. Bei automatisierter 
    ''' Validierung einer Eingabemaske kann dann in der UI mit diesen Namen Bezug auf das entsprechende 
    ''' Datenfeld genommen werden. Die Bezeichnung einer Datenfeldes in der UI beispielsweise durch ein Label sollte 
    ''' daher genauso lauten, wie diese Eigenschaft. Diese Eigenschaft sollte bei Verwendung von automatischer 
    ''' Validierung auch auf jedenfall gesetzt sein, weil anderenfalls eine Ausnahme ausgelöst werden kann.</remarks>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
     Description("Bestimmt oder Ermittelt den ausgeschriebenen/lolkalisierten Namen des Feldes, mit dem dieses Steuerelement verknüpft werden soll."),
     Category("Behavior"),
     EditorBrowsable(EditorBrowsableState.Always),
     Browsable(True)>
    Public Property DatafieldDescription As String Implements INullableValueDataBinding.DatafieldDescription

    Public Function ShouldSerializeDatafieldDescription() As Boolean
        Return Not String.IsNullOrEmpty(DatafieldDescription)
    End Function

    Public Sub ResetDatafieldDescription()
        DatafieldDescription = Nothing
    End Sub

    ''' <summary>
    ''' Bestimmt oder Ermittelt den Datenquellen-Feldnamen des Feldes, mit dem dieses Steuerelement verknüpft werden soll.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
     Description("Bestimmt oder Ermittelt den Datenquellen-Feldnamen des Feldes, mit dem dieses Steuerelement verknüpft werden soll."),
     Category("Behavior"),
     EditorBrowsable(EditorBrowsableState.Always),
     Browsable(True)>
    Public Property DatafieldName As String Implements INullableValueDataBinding.DatafieldName
        Get
            Return myDataFieldname
        End Get
        Set(ByVal value As String)
            If value <> myDataFieldname Then
                myDataFieldname = value
                MultiSelect = False
            End If
        End Set
    End Property

    Public Function ShouldSerializeDatafieldName() As Boolean
        Return Not String.IsNullOrEmpty(DatafieldName)
    End Function

    Public Sub ResetDatafieldName()
        DatafieldName = Nothing
    End Sub

    ''' <summary>
    ''' Bestimmt oder Ermittelt den Text der ausgegeben werden soll, wenn der Anwender versucht ein Feld zu verlassen, 
    ''' dass keine Eingaben enthält. Ist der Text nicht definiert, kann das Feld verlassen werden.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>Mit dieser Eigenschaft bestimmen Sie einerseits, ob ein Eingabefeld Null-Werte enthalten darf oder nicht, und 
    ''' andererseits wird gleichzeitig festgelegt, welche Fehlermeldung für den Benutzer ausgegeben werden soll, wenn er 
    ''' versucht, ein Eingabefeld zu verlassen, das keine Null-Werte akzeptiert, er aber keinen Wert eingegeben hat.</remarks>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
     Description("Bestimmt oder Ermittelt den Text der ausgegeben werden soll, wenn der Anwender versucht ein Feld zu verlassen, dass keine Eingaben enthält."),
     Category("Behavior"),
     EditorBrowsable(EditorBrowsableState.Always),
     Browsable(True)>
    Public Property NullValueMessage As String Implements INullableValueDataBinding.NullValueMessage

    Public Function ShouldSerializeNullValueMessage() As Boolean
        Return Not String.IsNullOrEmpty(NullValueMessage)
    End Function

    Public Sub ResetNullValueMessage()
        NullValueMessage = Nothing
    End Sub

    Public Overrides Property MinimumPopupSize As System.Drawing.Size
        Get
            If Me.Width > 0 Then
                If MyBase.MinimumPopupSize.Width < Me.Width Then
                    Return New Size(Me.Width, MyBase.MinimumPopupSize.Height)
                Else
                    Return MyBase.MinimumPopupSize
                End If
            End If
        End Get

        Set(ByVal value As System.Drawing.Size)
            MyBase.MinimumPopupSize = value
        End Set
    End Property

    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
     Description("Bestimmt oder ermittelt, ob Mehrfachselektierungen in der Liste zulässig sind oder nicht."),
     Category("Behavior"),
     EditorBrowsable(EditorBrowsableState.Always),
     Browsable(True)>
    Public Property MultiSelect As Boolean
        Get
            Return BindableDataGridView.MultiSelect
        End Get
        Set(ByVal value As Boolean)
            'TODO: In diesem Fall müssten wir ValueMember zurücksetzen; Value müsste eine List(Of ) zurückliefern?
            BindableDataGridView.MultiSelect = value
        End Set
    End Property

    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
     Description("Bestimmt oder ermittelt, wieviele Spalten beim Öffnen des Popups der enthaltenen Tabelle nach Möglichkeit im sichtbaren Ausschnitt angezeigt werden."),
     Category("Behavior"),
     EditorBrowsable(EditorBrowsableState.Always),
     Browsable(True), DefaultValue(0)>
    Public Property PreferredVisibleColumnsOnOpen As Integer
        Get
            Return myPreferredVisibleColumnsOnOpen
        End Get
        Set(ByVal value As Integer)
            If value < 0 Then
                Throw New ArgumentException("Wert darf nicht kleiner als 0 sein!")
            End If
            myPreferredVisibleColumnsOnOpen = value
        End Set
    End Property

    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
     Description("Bestimmt oder ermittelt, wieviele Zeilen beim Öffnen des Popups der enthaltenen Tabelle nach Möglichkeit im sichtbaren Ausschnitt angezeigt werden."),
     Category("Behavior"),
     EditorBrowsable(EditorBrowsableState.Always),
     Browsable(True), DefaultValue(0)>
    Public Property PreferredVisibleRowsOnOpen As Integer
        Get
            Return myPreferredVisibleRowsOnOpen
        End Get
        Set(ByVal value As Integer)
            If value < 0 Then
                Throw New ArgumentException("Wert darf nicht kleiner als 0 sein!")
            End If
            myPreferredVisibleRowsOnOpen = value
        End Set
    End Property

    ''' <summary>
    ''' Bestimmt oder ermittelt, ob die Spaltenbreiten vor dem Öffnen 
    ''' an die Inhalte der Überschriften/Zellen angepasst werden sollen.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
     Description("Bestimmt oder ermittelt, ob die Spaltenbreiten vor dem Öffnen an die Inhalte der Überschriften/Zellen angepasst werden sollen."),
     Category("Behavior"),
     EditorBrowsable(EditorBrowsableState.Always),
     Browsable(True), DefaultValue(True)>
    Public Property AutoResizeColumnsOnOpen As Boolean

    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
     Description("Bestimmt oder ermittelt, wie der Text in der Statuszeile des geöffneten Popups ausgerichtet werden soll."),
     Category("Behavior"),
     EditorBrowsable(EditorBrowsableState.Always),
     Browsable(True), DefaultValue(System.Drawing.StringAlignment.Center)>
    Public Property ValueTextAlignment As System.Drawing.StringAlignment
        Get
            Return Me.PopupControl.ValueTextAlignment
        End Get
        Set(ByVal value As System.Drawing.StringAlignment)
            Me.PopupControl.ValueTextAlignment = value
        End Set
    End Property

    ''' <summary>
    ''' Bestimmt oder ermittelt, nach welcher Zeitspanne der verzögerte TextChange-Event eine Hintergrundsuche in den Elementen für die AutoComplete-List auslösen soll.
    ''' </summary>
    ''' <returns></returns>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
     Description("Bestimmt oder ermittelt, nach welcher Zeitspanne der verzögerte TextChange-Event eine Hintergrundsuche in den Elementen für die AutoComplete-List auslösen soll."),
     Category("Behavior"),
     EditorBrowsable(EditorBrowsableState.Always),
     Browsable(True), DefaultValue(300)>
    Public Property DeferredTextChangeDelay As Integer
        Get
            Return myDeferredTextChangeDelay
        End Get
        Set(value As Integer)
            If Not Object.Equals(myDeferredTextChangeDelay, value) Then
                myDeferredTextChangeDelay = value
                OnDeferredTextChangeDelayChanged()
            End If
        End Set
    End Property

    Protected Overridable Sub OnDeferredTextChangeDelayChanged()
        RaiseEvent DeferredTextChangeDelayChanged(Me, EventArgs.Empty)
    End Sub

    <AttributeProvider(GetType(IListSource))>
    Public Property DataSource As Object Implements INullableValueRelationBinding.Datasource
        Get
            Return myDataSource
        End Get
        Set(ByVal value As Object)
            'If Debugger.IsAttached Then
            '    Debugger.Break()
            'End If

            If Object.Equals(value, myDataSource) Then
                Return
            End If
            myIsBinding = True
            myDataGridForm.BindableDataGridView.DataSource = value
            myDataSource = value
            Me.Value = Nothing
            myIsBinding = False
            myCurrentAutoCompleteListAndSchemaInfo = rebuildAutoCompleteList()
            myOldCurrentAutoCompleteListAndSchemaInfo = Nothing
            UpdateColumnHeadersAndBackgroundColor()
            OnDataSourceChanged(EventArgs.Empty)
        End Set
    End Property

    Protected Overridable Sub OnDataSourceChanged(e As EventArgs)
        RaiseEvent DataSourceChanged(Me, e)
    End Sub

    <Browsable(False)>
    Public ReadOnly Property BindableDataGridView As BindableDataGridView
        Get
            Return myDataGridForm.BindableDataGridView
        End Get
    End Property

    <Browsable(False), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public Property Value As Object Implements INullableValueDataBinding.Value
        Get
            If Me.PreserveInput Then
                Return myValue
            End If
            Return Me.myDataGridForm.BindableDataGridView.Value
        End Get

        Set(ByVal value As Object)
            myChangedByValueSetAccessor = True

            Try
                myDataGridForm.BindableDataGridView.Value = value
                myValue = value
            Catch ex As Exception
                Throw New UnassignableValueException("Value '" & value.ToString & "' could not assigned to control '" & Me.Name & "'." &
                                                  vbNewLine & "See InnerException for further information.",
                                                  ex, Me)
            End Try
            HandleSelectedValueChanged(False)
            myChangedByValueSetAccessor = False
        End Set
    End Property

    <Browsable(False), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public ReadOnly Property SelectedValue As Object
        Get
            Return Me.BindableDataGridView.Value
        End Get
    End Property

    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
     Description("Bestimmt oder ermittelt, ob der Anwender zur Laufzeit in der Lage sein soll, in der Popup-Liste zu suchen."),
     Category("Behavior"),
     EditorBrowsable(EditorBrowsableState.Always),
     Browsable(True), DefaultValue(False)>
    Public Property Searchable As Boolean
        Get
            Return mySearchable
        End Get
        Set(ByVal value As Boolean)
            If mySearchable <> value Then
                mySearchable = value
                Me.TextBoxPart.ReadOnly = Not value
                If value Then
                    rebuildAutoCompleteList()
                End If
            End If
        End Set
    End Property

    ''' <summary>
    ''' Ursprünglicher Wert der gesamten, selektierten Datenzeile auf dem die Value-Eigenschaft, gesteuert durch ValueMember, basiert.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
     Category("Sicherheit"),
     EditorBrowsable(EditorBrowsableState.Advanced),
     Browsable(False)>
    Public Property ValueBase As Object
        Get
            Return myDataGridForm.BindableDataGridView.ValueBase
        End Get
        Set(ByVal value As Object)
            myDataGridForm.BindableDataGridView.ValueBase = value
        End Set
    End Property

    <Browsable(False)>
    Public Property IsDirty As Boolean Implements INullableValueDataBinding.IsDirty
        Get
            Return myIsDirty
        End Get
        Private Set(value As Boolean)
            If value <> myIsDirty Then
                myIsDirty = value
                OnIsDirtyChanged(New IsDirtyChangedEventArgs(Me))
            End If
        End Set
    End Property

    Protected Overridable Sub OnIsDirtyChanged(e As IsDirtyChangedEventArgs)
        RaiseEvent IsDirtyChanged(Me, e)
    End Sub

    ''' <summary>
    ''' Bestimmt oder ermittelt, ob die Maske, die das Steuerelement enthält, gerade die Steuerelemente mit Daten befüllt, oder nicht.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>Das Setzen dieser Eigenschaft kann nur über die Schnittstelle (in der Regel von der Maskensteuerung) vorgenommen werden.</remarks>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
     EditorBrowsable(EditorBrowsableState.Advanced),
     Browsable(False)>
    Public Property IsLoading As HistoricalBoolean Implements INullableValueDataBinding.IsLoading
        Get
            Return myDataGridForm.BindableDataGridView.IsLoading
        End Get
        Set(ByVal value As HistoricalBoolean)
            myDataGridForm.BindableDataGridView.IsLoading = value
        End Set
    End Property

    Public Sub ResetIsDirty() Implements INullableValueDataBinding.ResetIsDirty
        myIsDirty = False
        myUndoValue = Nothing
    End Sub

    ''' <summary>
    ''' Bestimmt oder ermittelt die FormToBusinessClassManager-Komponente, die die Verwaltung dieses NullableValue-Controls übernimmt.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
     Description("Bestimmt oder ermittelt die FormToBusinessClassManager-Komponente, die die Verwaltung dieses NullableValue-Controls übernimmt."),
     Category("Behavior"),
     EditorBrowsable(EditorBrowsableState.Always),
     Browsable(True)>
    Public Property AssignedManagerComponent As FormToBusinessClassManager Implements INullableValueControl.AssignedManagerControl

    ''' <summary>
    ''' Bestimmt oder ermittelt eine eindeutige GUID für das Steuerelement, 
    ''' um beispielsweise Rechte-Mappings in Datenbanken aufzubauen.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
     Description("Bestimmt oder ermittelt eine eindeutige GUID für das Steuerelement, um beispielsweise Rechte-Mappings in Datenbanken aufzubauen."),
     Category("Sicherheit"),
     EditorBrowsable(EditorBrowsableState.Always),
     Browsable(True)>
    Public Property UIGuid As System.Guid Implements IPermissionManageableUIContentElement.IdentificationGuid

    ''' <summary>
    ''' Bestimmt oder ermittelt, in welcher Form eine Komponente auf Grund von 
    ''' Rechten oder Lizenzausbaustufen verwendet werden darf.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
     Description("Bestimmt oder ermittelt, in welcher Form eine Komponente auf Grund von Rechten oder Lizenzausbaustufen verwendet werden darf."),
     Category("Sicherheit"),
     EditorBrowsable(EditorBrowsableState.Always),
     Browsable(True), DefaultValue(GetType(ContentPresentPermissions), "ContentPresentPermissions.Normal")>
    Public Property ContentPresentPermission As ContentPresentPermissions Implements IPermissionManageableUIContentElement.ContentPresentPermission

    ''' <summary>
    ''' Bestimmt oder ermittelt einen Grund, weswegen ein Benutzer nur eingeschränkten Zugriff auf das Steuerelement hat.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
     Description("Bestimmt oder ermittelt einen Grund, weswegen ein Benutzer nur eingeschränkten Zugriff auf das Steuerelement hat."),
     Category("Sicherheit"),
     EditorBrowsable(EditorBrowsableState.Always),
     Browsable(True)>
    Public Property PermissionReason As String Implements IPermissionManageableUIContentElement.PermissionReason

    ''' <summary>
    ''' Bestimmt oder ermittelt, ob das Steuerelement in der Rechteverwaltung eingeschlossen werden soll.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
     Description("Bestimmt oder ermittelt, ob das Steuerelement in der Rechteverwaltung eingeschlossen werden soll."),
     Category("Sicherheit"),
     EditorBrowsable(EditorBrowsableState.Always),
     Browsable(True), DefaultValue(False)>
    Public Property IsManageable As Boolean Implements IPermissionManageableUIContentElement.IsManageable

    <Browsable(False)>
    Public ReadOnly Property ElementType As PermissionManageableUIElementType Implements IPermissionManageableUIContentElement.ElementType
        Get
            Return PermissionManageableUIElementType.Content
        End Get
    End Property

    ''' <summary>
    ''' Bestimmt oder ermittelt, ob bei der Zuweisung der Value-Eigenschaft Typgleichheit zwischen 
    ''' ValueMember (entsprechende Eigenschaft des Item in der Liste) und gesetztem Wert herschen muss, 
    ''' oder das Steuerelement versucht, eine impliziete Typkonvertierung durchzuführen.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>
    ''' Durch eine EnforceValueMemberTypeSafety-Eigenschaft beim BindableDataGrid sowie beim NullableValueRelationPopup-Steuerelement 
    ''' soll durch Setzen dieser Eigenschaft auf false (Standard ist true) die Typsicherheit dabei aufgerufen werden. 
    ''' Stellt das Control fest, dass bei der Zuweisung der Value-Eigenschaft ein anderer Typ als beim entsprechenden ValueMember vorliegt, 
    ''' versucht es bei EnforceValueMemberTypeSafety=false selbständig eine Konvertierung in den Typ vorzunehmen. 
    ''' Falls das nicht gelingt, wird eine ensprechende TypeMismatchException ausgelöst. 
    ''' Bei EnforceValueMemberTypeSafety=true wird in diesem Fall per se eine TypeMismatchException ausgelöst.
    ''' </remarks>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
     Description("Bestimmt oder ermittelt, ob bei der Zuweisung der Value-Eigenschaft Typengleichheit zwischen ValueMember (entsprechende Eigenschaft des Item in der Liste) und gesetztem Wert herschen muss."),
     Category("Behavior"),
     EditorBrowsable(EditorBrowsableState.Always),
     Browsable(True), DefaultValue(True)>
    Public Property EnforceValueMemberTypeSafety As Boolean
        Get
            Return Me.BindableDataGridView.EnforceValueMemberTypeSafety
        End Get
        Set(value As Boolean)
            Me.BindableDataGridView.EnforceValueMemberTypeSafety = value
        End Set
    End Property

    ''' <summary>
    ''' Bestimmt oder ermittelt einen Prioritätsindex, der bestimmt, in welcher Reihenfolge 
    ''' das Steuerelement vom FormsToBusinessClass-Manager verarbeitet wird (Höhere Nummer, frühere Verarbeitung.)
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
     Description("Bestimmt oder ermittelt einen Prioritätsindex, der bestimmt, in welcher Reihenfolge das Steuerelement vom FormsToBusinessClass-Manager verarbeitet wird (Höhere Nummer, frühere Verarbeitung.)"),
     Category("Behavior"),
     EditorBrowsable(EditorBrowsableState.Advanced),
     Browsable(True), DefaultValue(0)>
    Public Property ProcessingPriority As Integer Implements IAssignableFormToBusinessClassManager.ProcessingPriority

    ''' <summary>
    ''' Bestimmt oder ermittelt, ob eine <see cref="EntitiesFormsLib.FormToBusinessClassManager">FormToBusinessClassManager</see>-Instanz 
    ''' diese Komponente verarbeiten soll, wenn seine AutoUpdateFields-Eigenschaft auf ProcessSelected gesetzt wurde.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
     Description("Bestimmt oder ermittelt, ob eine FormToBusinessClassManager-Instanz diese Komponente verarbeiten soll, wenn seine AutoUpdateFields-Eigenschaft auf ProcessSelected gesetzt wurde."),
     Category("Sicherheit"),
     EditorBrowsable(EditorBrowsableState.Always),
     Browsable(True), DefaultValue(False)>
    Public Property SelectedForProcessing As Boolean Implements IAssignableFormToBusinessClassManager.SelectedForProcessing

    ''' <summary>
    ''' Bestimmt oder ermittelt, ob ein Warnton bei einer fehlgeschlagenen Validierung ausgegeben werden soll.  
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
     Description("Bestimmt oder ermittelt, ob ein Warnton bei einer fehlgeschlagenen Validierung ausgegeben werden soll."),
     Category("Behavior"),
     EditorBrowsable(EditorBrowsableState.Always),
     Browsable(True), DefaultValue(False)>
    Public Property BeepOnFailedValidation As Boolean

    ''' <summary>
    ''' Bestimmt oder ermittelt einen Gruppierungsnamen, um eine Möglichkeit zur Verfügung zu stellen, zentral eine Reihe von Steuerelementen zu steuern.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
     Description("Bestimmt oder ermittelt einen Gruppierungsnamen, um eine Möglichkeit zur Verfügung zu stellen, zentral eine Reihe von Steuerelementen zu steuern."),
     Category("Behavior"),
     EditorBrowsable(EditorBrowsableState.Always),
     Browsable(True), DefaultValue("Default")>
    Public Property GroupName As String Implements IAssignableFormToBusinessClassManager.GroupName
        Get
            Return myGroupName
        End Get
        Set(value As String)
            myGroupName = value
        End Set
    End Property

    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
     Description("Bestimmt oder ermittelt die Dauer in Millisekunden, die ein Baloontip im Falle einer Fehlermeldung angezeigt wird."),
     Category("Behavior"),
     EditorBrowsable(EditorBrowsableState.Always),
     Browsable(True), DefaultValue(5000)>
    Public Property ExceptionBalloonDuration As Integer Implements INullableValueControl.ExceptionBalloonDuration

    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
     Description("Bestimmt oder ermittelt das Zeichen, mit dem Suchbegriffe so verknüpft werden, dass alle Suchbegriffe für einen Zeilentreffer zutreffen müssen."),
     Category("Behavior"),
     EditorBrowsable(EditorBrowsableState.Always),
     Browsable(True), DefaultValue("+"c)>
    Public Property SearchKeywordAndChar As Char = "+"c

    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
     Description("Bestimmt oder ermittelt das Zeichen, mit dem Suchbegriffe so verknüpft werden, dass einer der Suchbegriffe für einen Zeilentreffer zutreffen muss."),
     Category("Behavior"),
     EditorBrowsable(EditorBrowsableState.Always),
     Browsable(True), DefaultValue(";"c)>
    Public Property SearchKeywordOrChar As Char = ";"c

    ''' <summary>
    ''' Bestimmt oder ermittelt, ob es sich bei einem Eingabefeld um ein Key-Feld handelt oder nicht.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
     Description("Bestimmt oder ermittelt, ob es sich bei einem Eingabefeld um ein Key-Feld handelt oder nicht."),
     Category("Behavior"),
     EditorBrowsable(EditorBrowsableState.Always),
     Browsable(True), DefaultValue(False)>
    Public Property IsKeyField As Boolean Implements IKeyFieldProvider.IsKeyField

    ''' <summary>
    ''' Bestimmt oder ermittelt, ob die Spalten, nach denen bei eingeschalteter Suche gesucht werden kann, 
    ''' mit der SearchColumnBackgroundColor eingefärbt werden sollen (True) oder nicht.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
     Description("Bestimmt oder ermittelt, ob die Spalten, nach denen bei eingeschalteter Suche gesucht werden kann, mit der SearchColumnBackgroundColor eingefärbt werden sollen (True) oder nicht."),
     Category("Behavior"),
     EditorBrowsable(EditorBrowsableState.Always),
     Browsable(True), DefaultValue(True)>
    Public Property ColorSearchColumn As Boolean = True

    ''' <summary>
    ''' Bestimmt oder ermittelt den Font, mit dem die Titel der Spalten, nach denen bei eingeschalteter Suche 
    ''' gesucht werden kann, ausgezeichnet werden soll.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
     Description("Bestimmt oder ermittelt den Font, mit dem die Titel der Spalten, nach denen bei eingeschalteter Suche gesucht werden kann, ausgezeichnet werden soll."),
     Category("Behavior"),
     EditorBrowsable(EditorBrowsableState.Always),
     Browsable(True)>
    Public Property SearchColumnHeaderFont As Font
        Get
            If Me.DesignMode Then
                Return mySearchColumnHeaderFont
            End If

            If mySearchColumnHeaderFont Is Nothing Then
                If Me.ParentForm IsNot Nothing Then
                    Return New Font(Me.ParentForm.Font, DEFAULT_SEARCH_COLUMN_HEADER_FONT_STYLE)
                End If
            End If
            Return mySearchColumnHeaderFont
        End Get

        Set(value As Font)
            If Me.DesignMode Then
                mySearchColumnHeaderFont = value
                Return
            End If

            If Me.Parent IsNot Nothing Then
                If Object.Equals(value, Me.Parent.Font) Then
                    If value.Style = DEFAULT_SEARCH_COLUMN_HEADER_FONT_STYLE Then
                        mySearchColumnHeaderFont = Nothing
                        UpdateColumnHeadersAndBackgroundColor()
                        Return
                    End If
                End If
            End If

            mySearchColumnHeaderFont = value
            UpdateColumnHeadersAndBackgroundColor()
        End Set
    End Property

    Public Function ShouldSerializeSearchColumnHeaderFont() As Boolean
        Return (mySearchColumnHeaderFont IsNot Nothing)
    End Function

    Public Sub ResetSearchColumnHeaderFont()
        mySearchColumnHeaderFont = Nothing
        UpdateColumnHeadersAndBackgroundColor()
    End Sub

    ''' <summary>
    ''' Bestimmt oder ermittelt die Farbe, mit dem der Hintergrund der Spalten, nach denen bei eingeschalteter Suche 
    ''' gesucht werden kann, ausgezeichnet werden soll.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
     Description("Bestimmt oder ermittelt die Farbe, mit dem die Titel der Spalten, nach denen bei eingeschalteter Suche gesucht werden kann, ausgezeichnet werden soll."),
     Category("Behavior"),
     EditorBrowsable(EditorBrowsableState.Always),
     Browsable(True)>
    Public Property SearchColumnBackgroundColor As Color
        Get
            Return mySearchColumnBackgroundColor
        End Get
        Set(value As Color)
            mySearchColumnBackgroundColor = value
            UpdateColumnHeadersAndBackgroundColor()
        End Set
    End Property

    Function ShouldSerializeSearchColumnBackgroundColor() As Boolean
        Return Not mySearchColumnBackgroundColor.Equals(DEFAULT_SEARCH_COLUMN_BACKGROUND_COLOR)
    End Function

    Sub ResetSearchColumnBackgroundColor()
        mySearchColumnBackgroundColor = DEFAULT_SEARCH_COLUMN_BACKGROUND_COLOR
        UpdateColumnHeadersAndBackgroundColor()
    End Sub

    ''' <summary>
    ''' Bestimmt oder ermittelt die Farbe, mit dem der Hintergrund der Spaltenköpfe, nach denen bei eingeschalteter Suche 
    ''' gesucht werden kann, ausgezeichnet werden soll.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
     Description("Bestimmt oder ermittelt die Farbe, mit dem die Titel der Spaltenköpfe, nach denen bei eingeschalteter Suche gesucht werden kann, ausgezeichnet werden soll."),
     Category("Behavior"),
     EditorBrowsable(EditorBrowsableState.Always),
     Browsable(True)>
    Public Property SearchColumnHeaderBackgroundColor As Color
        Get
            Return mySearchColumnHeaderBackgroundColor
        End Get
        Set(value As Color)
            mySearchColumnHeaderBackgroundColor = value
            UpdateColumnHeadersAndBackgroundColor()
        End Set
    End Property

    Function ShouldSerializeSearchColumnHeaderBackgroundColor() As Boolean
        Return Not mySearchColumnHeaderBackgroundColor.Equals(DEFAULT_SEARCH_COLUMN_HEADER_BACKGROUND_COLOR)
    End Function

    Sub ResetSearchColumnHeaderBackgroundColor()
        mySearchColumnHeaderBackgroundColor = DEFAULT_SEARCH_COLUMN_HEADER_BACKGROUND_COLOR
        UpdateColumnHeadersAndBackgroundColor()
    End Sub

    ''' <summary>
    ''' Bestimmt oder ermittelt, ob das RelationalPopup über einen Hinzufüge-Button verfügt.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
     Description("Bestimmt oder ermittelt, ob das RelationalPopup über einen Hinzufüge-Button verfügt."),
     Category("Behavior"),
     EditorBrowsable(EditorBrowsableState.Always),
     Browsable(True), DefaultValue(False)>
    Public Property HasAddButton As Boolean
        Get
            Return myHasAddButton
        End Get
        Set(value As Boolean)
            If value <> myHasAddButton Then
                myHasAddButton = value
                myAddButton.Visible = myHasAddButton
                OnHasAddButtonChanged(EventArgs.Empty)
            End If
        End Set
    End Property

    ''' <summary>
    ''' Bestimmt oder ermittelt, ob das RelationalPopup seine Texteingabe behält, wenn ein entsprechender Wert nicht in der Lookup-Tabelle gefunden wurde.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>Wenn PreserveInput ausgeschaltet ist, können im Eingabebereich nur Werte zu sehen sein, die einer Auswahl in der Liste entsprechen. 
    ''' Die Formatierung des ausgewählten Wertes der Liste wird dabei durch die DisplayPattern-Eigenschaft definiert. Werte aus der Liste können dabei 
    ''' mit TAB oder mit Return ausgewählt werden, wobei TAB auch gleichzeitig zum nächsten Feld springt (Fokusänderung). Neueingaben im Eingabebereich sind nur 
    ''' möglich, wenn PreserveInput auf True gesetzt wurde. PreselectFirstMatchWithPreserveInput steuert dann das TAB-Auswahl-Verhalten gefundener Werte aufgrund der 
    ''' Eingabe in der Ergebnisliste: Ist diese Eigenschaft gesetzt, findet eine Selektierung des ersten gefundenen Wertes statt; TAB übernimmt in diesem Fall 
    ''' NICHT den Wert als Auswahl; dieses passiert nur mit RETURN (wichtig: das gilt grundsätzlich nur für PreserveInput=True!). Ist diese Eigenschaft nicht 
    ''' gesetzt, muss der Anwender einen Eintrag zunächst mit KeyUp oder KeyDown aus der Liste selektieren. TAB übernimmt in diesem Fall wie bei PreserveInput=false 
    ''' bei einem Control-Fokus-Wechsel ebenfalls den Wert aus der Liste.</remarks>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
     Description("Bestimmt oder ermittelt, ob das RelationalPopup seine Texteingabe behält, wenn ein entsprechender Wert nicht in der Lookup-Tabelle gefunden wurde."),
     Category("Behavior"),
     EditorBrowsable(EditorBrowsableState.Always),
     Browsable(True), DefaultValue(False)>
    Public Property PreserveInput As Boolean
        Get
            Return myPreserveInput
        End Get
        Set(value As Boolean)
            myPreserveInput = value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the maximum number of characters that can be manually entered into the Textbox-Part of this control. NOTE: This also limits the search text length, when PreserveInput is not used!
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
     Description("Gets or sets the maximum number of characters that can be manually entered into the Textbox-Part of this control. NOTE: This also limits the search text length, when PreserveInput is not used!"),
     Category("Behavior"),
     EditorBrowsable(EditorBrowsableState.Always),
     Browsable(True), DefaultValue(0)>
    Public Property MaxLength As Integer
        Get
            Return TextBoxPart.MaxLength
        End Get
        Set(value As Integer)
            TextBoxPart.MaxLength = value
        End Set
    End Property

    ''' <summary>
    ''' Bestimmt oder ermittelt, ob IsDirty ausgelöst werden soll, 
    ''' wenn Preserve-Input gesetzt ist, und der Anwender bereits ein einzelnes Zeichen im Eingabefeld geändert hat.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>Ein Sonderfall bei der Behandlung von Eingaben mit PreserveInput ist, dass das Control nicht bestimmen kann,
    ''' ob eine Eingabe eines Zeichens Teil einer Neuerfassung eines Wertes ist (ValueText) wird ausgewertet als de-facto-Value oder 
    ''' ob die Eingabe zur Bestimmung eines Wertes aus der Liste ist. Beim letzten ist das IsDirty-Auslösen bereits bei 
    ''' der Eingabe von Zeichen NICHT erwünscht, beim ersten schon. Mithilfe dieser Eigenschaft kann die Verhaltensweise gesteuert werden. 
    ''' Wenn die Fälle im Zusammenhang anders behandelt werden müssen, sollte die IsDirty-Eigenschaft entsprechend von 
    ''' außen gesetzt werden, damit die angeschlossene Verarbeitungskette richtig mit diesen Grenzfällen umgehen kann. 
    ''' Diese Eigenschaft hat Opt-In-Charakteristik.</remarks>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
     Description("Bestimmt oder ermittelt, ob IsDirty ausgelöst werden soll, wenn Preserve-Input gesetzt ist, und der Anwender bereits ein einzelnes Zeichen im Eingabefeld geändert hat."),
     Category("Behavior"),
     EditorBrowsable(EditorBrowsableState.Always),
     Browsable(True), DefaultValue(False)>
    Public Property RaiseIsDirtyOnPreserveInputTextChange As Boolean
        Get
            Return myRaiseIsDirtyOnPreserveInputTextChange
        End Get
        Set(value As Boolean)
            myRaiseIsDirtyOnPreserveInputTextChange = value
        End Set
    End Property

    ''' <summary>
    ''' Bestimmt oder ermittelt, ob der erste gefundene Eintrag vorselektiert werden soll, wenn PreserveInput eingeschaltet ist, und damit auch Neueingaben in diesem Control erfasst werden können.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>Wenn PreserveInput ausgeschaltet ist, können im Eingabebereich nur Werte zu sehen sein, die einer Auswahl in der Liste entsprechen. 
    ''' Die Formatierung des ausgewählten Wertes der Liste wird dabei durch die DisplayPattern-Eigenschaft definiert. Werte aus der Liste können dabei 
    ''' mit TAB oder mit Return ausgewählt werden, wobei TAB auch gleichzeitig zum nächsten Feld springt (Fokusänderung). Neueingaben im Eingabebereich sind nur 
    ''' möglich, wenn PreserveInput auf True gesetzt wurde. PreselectFirstMatchWithPreserveInput steuert dann das TAB-Auswahl-Verhalten gefundener Werte aufgrund der 
    ''' Eingabe in der Ergebnisliste: Ist diese Eigenschaft gesetzt, findet eine Selektierung des ersten gefundenen Wertes statt; TAB übernimmt in diesem Fall 
    ''' NICHT den Wert als Auswahl; dieses passiert nur mit RETURN (WICHTIG: das gilt grundsätzlich nur für PreserveInput=True!). Ist diese Eigenschaft nicht 
    ''' gesetzt, muss der Anwender einen Eintrag zunächst mit KeyUp oder KeyDown aus der Liste selektieren. TAB übernimmt in diesem Fall wie bei PreserveInput=false 
    ''' bei einem Control-Fokus-Wechsel ebenfalls den Wert aus der Liste.</remarks>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
     Description("Bestimmt oder ermittelt, ob das RelationalPopup seine Texteingabe behält, wenn ein entsprechender Wert nicht in der Lookup-Tabelle gefunden wurde."),
     Category("Behavior"),
     EditorBrowsable(EditorBrowsableState.Always),
     Browsable(True), DefaultValue(False)>
    Public Property PreselectFirstMatchWithPreserveInput As Boolean

    ''' <summary>
    ''' Ermittelt, ob ein gefundener Eintrag in der Liste aufgrund der Eingabe im Textfeld direkt vorselektiert werden soll.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private ReadOnly Property SuppressPreselectFirstMatch As Boolean
        Get
            Return PreserveInput And (Not PreselectFirstMatchWithPreserveInput)
        End Get
    End Property

    ''' <summary>
    ''' Bestimmt oder ermittelt, ob bei einem Überlauf in der TextBox der vordere oder der hintere Teiltext angezeigt wird.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
     Description("Bestimmt oder ermittelt, ob bei einem Überlauf in der TextBox der vordere oder der hintere Teiltext angezeigt wird."),
     Category("Behavior"),
     EditorBrowsable(EditorBrowsableState.Always),
     Browsable(True), DefaultValue(False)>
    Public Property ReverseTextOverflowBehaviour As Boolean
        Get
            Return myReverseTextOverflowBehaviour
        End Get
        Set(value As Boolean)
            If value Xor myReverseTextOverflowBehaviour Then
                myReverseTextOverflowBehaviour = value
                If value Then
                    If Me.RightToLeft <> System.Windows.Forms.RightToLeft.Yes Then
                        Me.TextBoxPart.RightToLeft = System.Windows.Forms.RightToLeft.Yes
                        Me.TextBoxPart.TextAlign = HorizontalAlignment.Right
                    Else
                        Me.TextBoxPart.RightToLeft = System.Windows.Forms.RightToLeft.Yes
                        Me.TextBoxPart.TextAlign = HorizontalAlignment.Right
                    End If
                End If
            End If
        End Set
    End Property

    ''' <summary>
    ''' Returns or sets a flag which determines that the use can cycle between entry fields with Page up and Page down rather than Tab and Shift+Tab.
    ''' </summary>
    ''' <returns></returns>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
     Description("Returns or sets if the user can cycle between entry fields with Page up and Page down in addition to Tab and Shift+Tab."),
     Category("Behavior"),
     EditorBrowsable(EditorBrowsableState.Always),
     Browsable(True), DefaultValue(False)>
    Public Property ImitateTabByPageKeys As Boolean
        Get
            Return myImitateTabByPageKeys
        End Get
        Set(value As Boolean)
            If Not Object.Equals(myImitateTabByPageKeys, value) Then
                myImitateTabByPageKeys = value
            End If
        End Set
    End Property

    Protected Overridable Sub OnHasAddButtonChanged(e As EventArgs)
        MyBase.PositionControls()
    End Sub

    ''' <summary>
    ''' Bestimmt oder ermittelt, ob der Filtervorgang bei der Eingabe von Suchbegriffen für eine große Anzahl von Elementen (>5000) parallelisiert werden soll.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
     Description("Bestimmt oder ermittelt, ob der Filtervorgang bei der Eingabe von Suchbegriffen für eine große Anzahl von Elementen (>5000) parallelisiert werden soll."),
     Category("Behavior"),
     EditorBrowsable(EditorBrowsableState.Always),
     Browsable(True), DefaultValue(False)>
    Public Property ParallelizeFiltering As Boolean

    ''' <summary>
    ''' Bestimmt oder ermittelt wenn PreserveInput auf True gesetzt ist, welcher neue Wert (reiner Text) im Eingabebereich angezeigt wird, der nicht einem Wert der Liste entspricht.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>Diese Eigenschaft wird nur programmatisch gesetzt und steht im Designer nicht zur Verfügung.</remarks>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
     Description("Bestimmt oder ermittelt wenn PreserveInput auf True gesetzt ist, welcher neue Wert (reiner Text) im Eingabebereich angezeigt wird, der nicht einem Wert der Liste entspricht."),
     Category("Behavior"),
     EditorBrowsable(EditorBrowsableState.Always),
     Browsable(False), DefaultValue("")>
    Public Property ValueText As String
        Get
            Return Me.PopupControl.ValueText
        End Get
        Set(value As String)
            Me.PopupControl.ValueText = value
            myTextBoxDeferrer.IgnoreNextTextChange = True
            Me.TextBoxPart.Text = value
        End Set
    End Property

    Protected Overridable Sub OnValueTextChanged(e As EventArgs)
        RaiseEvent ValueTextChanged(Me, e)
    End Sub

    Public Property AutoSizeColumnsMode As DataGridViewAutoSizeColumnsMode
        Get
            Return BindableDataGridView.AutoSizeColumnsMode
        End Get
        Set(value As DataGridViewAutoSizeColumnsMode)
            BindableDataGridView.AutoSizeColumnsMode = value
        End Set
    End Property

    Public Property AutoSizeRowsMode As DataGridViewAutoSizeRowsMode
        Get
            Return BindableDataGridView.AutoSizeRowsMode
        End Get
        Set(value As DataGridViewAutoSizeRowsMode)
            BindableDataGridView.AutoSizeRowsMode = value
        End Set
    End Property

    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
     EditorBrowsable(EditorBrowsableState.Advanced),
     Browsable(False)>
    Public Property ThrowLayoutException As Boolean
        Get
            If myDataGridForm IsNot Nothing Then
                Return myDataGridForm.BindableDataGridView.ThrowLayoutException
            Else
                Return False
            End If
        End Get
        Set(value As Boolean)
            If myDataGridForm IsNot Nothing Then
                myDataGridForm.BindableDataGridView.ThrowLayoutException = value
            End If
        End Set
    End Property
End Class

Public Class AutoCompleteLookupItem
    Property SearchText As String
    Property Row As Object
End Class

<StructLayout(LayoutKind.Sequential)>
Public Structure NativeMessage
    Public handle As IntPtr
    Public msg As UInteger
    Public wParam As IntPtr
    Public lParam As IntPtr
    Public time As UInteger
    Public p As System.Drawing.Point
End Structure
