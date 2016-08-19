Imports System.Windows.Forms
Imports System.ComponentModel
Imports System.Drawing
Imports System.Windows.Forms.VisualStyles
Imports System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox
Imports System.Runtime.CompilerServices
Imports System.Drawing.Drawing2D
Imports System.Globalization

<Designer("ActiveDevelop.EntitiesFormsLib.TextBoxBasedControlDesigner")>
Public MustInherit Class NullableValueBase(Of NullableType As {Structure, IComparable},
                                                ControlType As {Control, New,
                                                INullableValuePrimalControl,
                                                ITextBoxBasedControl})
    Inherits ContainerControl
    Implements INullableValueEditor, INullableValueDataBinding,
               ITextBoxBasedControl, IRequestAdditionalSnapBaselineOffset,
               IPermissionManageableUIContentElement

    Private myEditedValueIsInitialized As Boolean
    Private myValueChangedInternally As Boolean
    Private myFailedValidationMessage As String
    Private myLastFailedValidationException As ContainsUIMessageException
    Private mySuppresValueChangedOnTextPartTextChange As Boolean
    Private myRequestedSize As Size
    Private myReverseTextOverflowBehaviour As Boolean
    Private myIsManageable As Boolean
    Private myGroupName As String = NullableControlManager.GetInstance.GetDefaultGroupName(Me, "Default")
    Private myBackColorBrush As SolidBrush
    Private myValueValidationState As ValueValidationStateStore(Of NullableType)
    Private myBorderstyleHasBeenSetForInstance As Boolean
    Private myEmulatedIsFocusedForUnitTesting As Boolean
    Private mySurpressQueryIsDirty As Boolean
    Private myForceValueChangeCauseToUser As Boolean

    'Wird benötigt, wenn wir den kompletten Zyklus
    'LostFocus --> GotFocus einmalig emulierend durchlaufen müssen,
    'weil die Value-Eigenschaft fokussiert abgefragt wurde
    '(z.B. durch Toolbar-Button ausgelöst).
    'Private myBlindFocusHandling As Boolean

    Public Event IsDirtyChanged(ByVal sender As Object, ByVal e As IsDirtyChangedEventArgs) Implements INullableValueDataBinding.IsDirtyChanged
    Public Event ValueChanging(ByVal sender As Object, ByVal e As ValueChangingEventArgs(Of NullableType?))
    Public Event ValueChanged(ByVal sender As Object, ByVal e As ValueChangedEventArgs) Implements INullableValueDataBinding.ValueChanged
    Public Event RequestValidationFailedReaction(ByVal sender As Object, ByVal e As RequestValidationFailedReactionEventArgs) Implements INullableValueDataBinding.RequestValidationFailedReaction
    Public Event ValueValidating(ByVal sender As Object, ByVal e As NullableValueValidationEventArgs(Of NullableType?))
    Public Event ValueValidated(ByVal sender As Object, ByVal e As NullableValueValidationEventArgs(Of NullableType?))
    Public Event ReadOnlyChanged(sender As Object, e As EventArgs)
    Public Event ValueValidationStateChanged(sender As Object, e As EventArgs)
    Public Event NullValueColorChanged(sender As Object, e As EventArgs)
    Public Event IsValueNullChanged(sender As Object, e As EventArgs)
    Public Event ExceptionBalloonDurationChanged(sender As Object, e As EventArgs)
    Public Event MaxLengthChanged(sender As Object, e As EventArgs)
    Public Event ReverseTextOverflowBehaviourChanged(sender As Object, e As EventArgs)
    Public Event TextAlignChanged(sender As Object, e As EventArgs)

    Private myFormatString As String                            ' Der Format-String, der den String für das Anzeigen des Wertes nach Verlassen des Feldes vorgibt.
    Private myNullValueString As String                         ' Die Zeichenfolge, die beim Verlieren des Fokus angezeigt wird, wenn eine Null-Eingabe erfolgte.

    Private myValueControl As ControlType                       ' Das Steuerelement, dass die eigentliche Werteeingabe durchführt.
    Private myValue As Nullable(Of NullableType)                ' Der Wert, den dieses Steuerelement bearbeitet
    Private myEditedValue As String                             ' Die ursprüngliche Tastatureingabe, die sich aus dem Wert ergibt.

    Private myOriginalValue As Nullable(Of NullableType)        ' Der Ausgabgswert für den Rückgängig-Vorgang (typisiert)
    Private myOriginalEditedValue As String                     ' Der Ausgangswert für den Rückgängig-Vorgang (als Zeichenkette, wie eingegeben)
    Private myFormatterEngine As INullableValueFormatterEngine  ' Die Formatter-Klasse für den entsprechenden Typ, der in abgeleiteten Versionen dieser Klasse verarbeitet wird.
    Private myIsLoading As HistoricalBoolean                    ' Zeigt an, dass sich diese Klasse gerade im "Lade"-Modus befindet, in der Update-Ereignisse ausgeblockt sind.
    Private myIsDirty As Boolean                                ' Zeigt an, dass sich der Value-Wert seit der letzten (ersten) Zuweisung geändert hat, und ein Datensatz aktualisiert werden muss.
    Private myBeepOnFailedValidation As Boolean                 ' Bestimmt, dass ein Warnton bei einer fehlgeschlagenen Validierung erfolgen soll.
    Private myFocusColor As Color                               ' Bestimmt die Farbe, die im Bedarfsfall vorselektiert werden soll, wenn das Steuerelement den Fokus erhält.
    Private myErrorColor As Color                               ' Bestimmt die Farbe, die beim genehmigter Fehlvalidierung dem betroffenen Steuerelement zugewiesen werden soll.
    Private myNullValueColor As Color?                          ' Bestimmt die Farbe, die im Bedarfsfall ForeColor zugewiesen werden soll, wenn Value Null ist und das Steuerelement NICHT den Focus hat. Standardmäßig ForeColor.
    Private myOriginalBackcolor As Color                        ' Zwischenspeicher für die Farbe beim Wechsel der Farben durch das Fokussieren.
    Private myOnFocusColor As Boolean                           ' Bestimmt, *ob* das Steuerelement mit FocusColor eingefärbt werden soll, wenn das Steuerelement den Fokus erhält.
    Private myFocusSelectionBehaviour As FocusSelectionBehaviours  ' Bestimmt die Verhaltensweise des Vorselektierens des Steuerelementtextes, wenn es den Fokus erhält. 

    Private mySuspendHandleIsDirty As Boolean                   ' Dirty nicht setzen beim Umformatieren des Wertes durch LostFocus/Enter/Leave/GotFocus.
    Private myDatafieldDescription As String
    Private myNullValueMessage As String
    Private myBorderstyle As BorderStyle
    Private myIsFocused As Boolean
    Private myDoesLostFocusPrecedeValidate As Boolean           ' Ermöglicht es Validating herauszufinden, 

    Protected ReadOnly DEFAULT_FOCUS_COLOR As Color = Color.Yellow
    Protected ReadOnly DEFAULT_ERROR_COLOR As Color = Color.Red

    Protected Const CONTROLDEFAULTWIDTH As Integer = 120
    Protected Const DEFAULT_NULL_VALUE_STRING = "* - - -*"
    Protected Shared ReadOnly DEFAULT_DATE_FORMAT_STRING As String = CultureInfo.CurrentUICulture.DateTimeFormat.ShortDatePattern
    Protected Const DEFAULT_FOCUS_SELECTION_BEHAVIOUR As FocusSelectionBehaviours = FocusSelectionBehaviours.PreSelectInput
    Protected Const DEFAULT_ON_FOCUS_COLOR As Boolean = True
    Protected Const DEFAULT_BEEP_ON_FAILED_VALIDATION As Boolean = False
    Protected Const DEFAULT_IMITATE_TAB_BY_PAGE_KEYS = False

    Private Const WM_KEYDOWN = &H100

    'Shapepoints für den Balloon.
    Private myShapePointTypes As Byte() = {CByte(PathPointType.Start),
                                           CByte(PathPointType.Line),
                                           CByte(PathPointType.Line),
                                           CByte(PathPointType.Line),
                                           CByte(PathPointType.Line),
                                           CByte(PathPointType.Line),
                                           CByte(PathPointType.Line),
                                           CByte(PathPointType.Line Or PathPointType.CloseSubpath)}
    Private myExceptionBalloonDuration As Integer
    Private myImitateTabByPageKeys As Boolean

    Public Sub New()
        MyBase.New()

        myFormatterEngine = GetDefaultFormatterEngine()
        SetInitialDefaultBorderstyleOnDemand()
        myValueControl = New ControlType()
        myValueControl.AutoSize = False
        myValueControl.TabStop = False

        InitializeProperties()

        If Me.IsMultiLineControl Then
            myValueControl.TextBoxPart.Multiline = True
            myValueControl.TextBoxPart.ScrollBars = ScrollBars.Vertical
            myValueControl.TextBoxPart.AcceptsReturn = True
            SetStyle(ControlStyles.FixedHeight, False)
        Else
            'Eigentlich redundant.
            myValueControl.TextBoxPart.Multiline = False
            SetStyle(ControlStyles.FixedHeight, True)
        End If

        myNullValueString = GetDefaultNullValueString()

        myBeepOnFailedValidation = NullableControlManager.GetInstance.GetDefaultBeepOnFailedValidation(Me, DEFAULT_BEEP_ON_FAILED_VALIDATION)
        myOnFocusColor = NullableControlManager.GetInstance.GetDefaultOnFocusColor(Me, DEFAULT_ON_FOCUS_COLOR)
        myFocusColor = NullableControlManager.GetInstance.GetDefaultFocusColor(Me, DEFAULT_FOCUS_COLOR)
        myErrorColor = NullableControlManager.GetInstance.GetDefaultErrorColor(Me, DEFAULT_ERROR_COLOR)
        myFormatString = NullableControlManager.GetInstance.GetDefaultFormatString(Me, GetDefaultFormatString)
        myFocusSelectionBehaviour = NullableControlManager.GetInstance.GetDefaultFocusSelectionBehaviour(Me, DEFAULT_FOCUS_SELECTION_BEHAVIOUR)
        ExceptionBalloonDuration = NullableControlManager.GetInstance.GetDefaultExceptionBalloonDuration(Me, 5000)
        ImitateTabByPageKeys = NullableControlManager.GetInstance.GetDefaultImitateTabByPageKeys(Me, DEFAULT_IMITATE_TAB_BY_PAGE_KEYS)

        SetStyle(ControlStyles.ResizeRedraw, True)
        SetStyle(ControlStyles.UseTextForAccessibility, True)
        SetStyle(ControlStyles.StandardClick, False)
        SetStyle(ControlStyles.ContainerControl, True)
        SetStyle(ControlStyles.SupportsTransparentBackColor, True)
        SetStyle(ControlStyles.OptimizedDoubleBuffer, True)

        Me.Controls.Add(myValueControl)

        AddHandler DirectCast(myValueControl, ITextBoxBasedControl).TextBoxPart.Resize, AddressOf OnTextBoxResize

        'Wirering up the event which blocks alpha keys when no Formula is allowed.
        AddHandler Me.TextBoxPart.KeyDown, AddressOf TextBoxPartKeyPressHandler

        'Verhalten geändert: Hier wurde aus ungeklärten Gründen schon direkt beim Ändern der TextBox das ValueChanging-Ereignis getriggert.
        'Dieser Workflow ist falsch. Wir haben das Verhalten dahin gehend geändert, dass nun die Text-Eigenschaft geändert wird,
        'die das TextChanged von Control auslöst. Dadurch kann das bisherige Verhalten emuliert werden.
        AddHandler DirectCast(myValueControl, ITextBoxBasedControl).TextBoxPart.TextChanged,
            Sub(sender As Object, e As EventArgs)
                Me.Text = DirectCast(myValueControl, ITextBoxBasedControl).TextBoxPart.Text
                'If mySuppresValueChangedOnTextPartTextChange Then
                '    mySuppresValueChangedOnTextPartTextChange = False
                '    Return
                'End If
                'If Not myValueChangedByPropertySetter Then
                '    OnValueChanged(ValueChangedEventArgs.PredefinedWithUser)
                'Else
                '    myValueChangedByPropertySetter = False
                'End If
            End Sub
        InitializeValue()
    End Sub


    Private Sub TextBoxPartKeyPressHandler(sender As Object, e As KeyEventArgs)
        If ImitateTabByPageKeys Then
            If e.KeyCode = Keys.Next Then
                SendKeys.SendWait("{TAB}")
                e.SuppressKeyPress = True
            ElseIf e.KeyCode = Keys.PageUp Then
                SendKeys.SendWait("+{TAB}")
                e.SuppressKeyPress = True
            End If
        End If
    End Sub

    Protected MustOverride Sub InitializeProperties()

    ''' <summary>
    ''' Behandelt das Verhalten des Setzens des Borderstyles beim Instanziieren des Controls.
    ''' </summary>
    ''' <remarks>Da das Control *vor* dem Aufruf von DefaultSize von Control(Base) 
    ''' keine Gelegenheit im Basiskonstruktor bekommt, den Borderstyle zu setzen, 
    ''' müssen wir beim ersten Aufruf von DefaultSize UND im Konstruktor 
    ''' den Borderstyle setzen. Das darf natürlich nur dann passieren, 
    ''' wenn der Borderstyle noch nie initial gesetzt wurde - dafür 
    ''' dient das entsprechende Flag. Dieses Verhalten sollte, wenn überhaupt, 
    ''' nur mit Bedacht überschrieben werden!</remarks>
    Protected Overridable Sub SetInitialDefaultBorderstyleOnDemand()
        If myBorderstyleHasBeenSetForInstance Then Return
        Me.Borderstyle = DefaultBorderStyle()
        myBorderstyleHasBeenSetForInstance = True
    End Sub

    ''' <summary>
    ''' Liefert den Default-Borderstyle-Wert zurück.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>Überschreiben Sie diese Methode, wenn Sie das BorderStyle-Verhalten 
    ''' beim Instanziieren des Steuerelementes beeinflussen wollen.</remarks>
    Protected Overridable Function DefaultBorderStyle() As System.Windows.Forms.BorderStyle
        Return System.Windows.Forms.BorderStyle.FixedSingle
    End Function


#Region "Layout-and Style-Handling"

    ''' <summary>
    ''' Ermittelt in abgeleiteten Klassen, ob es sich um ein mehrzeiliges Steuerelement handelt, bei dem 
    ''' die Größenänderung dann in alle Richtungen im Designer ansonsten nur in X-Richtung möglich ist.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>Bei einzeiligen Steuerelementen kann die Höhe des Steuerelementes nicht verändert werden, sondern richtet sich nach der 
    ''' Fontgröße. Bei der Standardeinstellung für Fonts beträgt die Höhe des einzeiligen Steuerelementes 20 Pixel.</remarks>
    Protected MustOverride Function IsMultiLineControl() As Boolean Implements INullableValueEditor.IsMultilineControl

    Protected Overrides ReadOnly Property DefaultSize As Size
        Get
            'DefaultSize kann aufgerufen werden, noch bevor
            'der Konstruktor durch ist. Deswegen ist dieser
            'Aufruf hier notwendig.
            SetInitialDefaultBorderstyleOnDemand()

            If Me.IsMultiLineControl Then
                Return MyBase.DefaultSize
            Else
                Return New Size(CONTROLDEFAULTWIDTH,
                                Me.PreferredHeight)
            End If
        End Get
    End Property

    <Browsable(False), EditorBrowsable(EditorBrowsableState.Advanced),
     DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public ReadOnly Property PreferredHeight As Integer
        Get
            Dim fontHeight As Integer = MyBase.Font.Height
            If (Me.Borderstyle <> BorderStyle.None) Then
                Return (fontHeight + ((SystemInformation.BorderSize.Height * 4) + 3))
            End If
            Return (fontHeight + 3)
        End Get
    End Property

    Protected Overrides Sub OnHandleCreated(e As EventArgs)
        MyBase.OnHandleCreated(e)
        PositionControls()
    End Sub

    Property Borderstyle As BorderStyle
        Set(ByVal value As BorderStyle)
            If (Me.Borderstyle <> value) Then
                myBorderstyle = value
                MyBase.RecreateHandle()
                PositionControls()
            End If
        End Set
        Get
            Return myBorderstyle
        End Get
    End Property

    Protected Overrides Sub [Select](directed As Boolean, forward As Boolean)
        MyBase.[Select](directed, directed)
        Me.TextBoxPart.Focus()
    End Sub

    Protected Overrides ReadOnly Property CreateParams As System.Windows.Forms.CreateParams
        Get
            Dim tmpParams As CreateParams = MyBase.CreateParams
            tmpParams.Style = (tmpParams.Style And -8388609)
            If Not Application.RenderWithVisualStyles Then
                Select Case Me.Borderstyle
                    Case BorderStyle.FixedSingle
                        tmpParams.Style = (tmpParams.Style Or &H800000)
                        Return tmpParams
                    Case BorderStyle.Fixed3D
                        tmpParams.ExStyle = (tmpParams.ExStyle Or &H200)
                        Return tmpParams
                End Select
            End If
            Return tmpParams
        End Get
    End Property

    Protected Overrides Sub OnFontChanged(e As System.EventArgs)
        MyBase.OnFontChanged(e)
        Me.SetBoundsCore(Me.Location.X, Me.Location.Y, Me.Width, Me.Height, BoundsSpecified.Size)
        PositionControls()
    End Sub

    Protected Overrides Sub OnLayout(ByVal e As System.Windows.Forms.LayoutEventArgs)
        PositionControls()
        MyBase.OnLayout(e)
    End Sub

    Protected Sub OnTextBoxResize(ByVal sender As Object, ByVal e As System.EventArgs)
        Me.PositionControls()
    End Sub

    Public Overrides Function GetPreferredSize(proposedSize As Size) As Size
        myRequestedSize = New Size(Width, Height)
        If Not Me.IsMultiLineControl Then
            Height = Me.PreferredHeight
        End If
        Return MyBase.GetPreferredSize(myRequestedSize)
    End Function

    Protected Overrides Sub SetBoundsCore(x As Integer, y As Integer, width As Integer, height As Integer, specified As System.Windows.Forms.BoundsSpecified)
        If specified = BoundsSpecified.Size Then
            myRequestedSize = New Size(width, height)
            If Not Me.IsMultiLineControl Then
                height = Me.PreferredHeight
            End If
        End If
        MyBase.SetBoundsCore(x, y, width, height, specified)
    End Sub

    Protected Overridable Sub PositionControls()
        'Noch kein Control da, dann gibt's nix zu layouten.
        If myValueControl Is Nothing Then
            Return
        End If

        Dim ctrlArea As Rectangle = Rectangle.Empty
        Dim tmpRec As New Rectangle(Point.Empty, MyBase.ClientSize)
        Dim width As Integer = tmpRec.Width
        Dim borderStyle As BorderStyle = Me.Borderstyle
        If Application.RenderWithVisualStyles Then
            Dim offset As Integer = If((Me.Borderstyle = BorderStyle.None), 0, 2)
            tmpRec.Inflate(-offset, -offset)
        Else
            Dim offset As Integer = 0
            If Me.Borderstyle = System.Windows.Forms.BorderStyle.FixedSingle Then
                offset = 1
            ElseIf Me.Borderstyle = System.Windows.Forms.BorderStyle.Fixed3D Then
                offset = 0
            Else
                offset = 1
            End If
            tmpRec.Inflate(-offset, -offset)
        End If
        ctrlArea = tmpRec
        myValueControl.Bounds = ctrlArea
    End Sub

    Protected Overrides Sub OnPaintBackground(e As System.Windows.Forms.PaintEventArgs)
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        If myBackColorBrush Is Nothing Then
            If BackColor = Color.Transparent Then
                myBackColorBrush = New SolidBrush(Color.White)
            Else
                myBackColorBrush = New SolidBrush(BackColor)
            End If
        End If

        If Application.RenderWithVisualStyles Then
        End If

        Dim rec = New Rectangle(1, 1, Me.ClientSize.Width - 1, Me.ClientSize.Height - 1)
        e.Graphics.FillRectangle(myBackColorBrush, rec)

        Dim bounds As Rectangle = myValueControl.Bounds
        If Application.RenderWithVisualStyles Then
            If (Me.Borderstyle = BorderStyle.None) Then
                GoTo SkipToEnd
            End If
            Dim clientRectangle As Rectangle = MyBase.ClientRectangle
            Dim clipRectangle As Rectangle = e.ClipRectangle
            Dim renderer As New VisualStyleRenderer(TextEdit.Normal)
            Dim width As Integer = 1
            Dim surface1 As New Rectangle(clientRectangle.Left, clientRectangle.Top, width, clientRectangle.Height)
            Dim surface2 As New Rectangle(clientRectangle.Left, clientRectangle.Top, clientRectangle.Width, width)
            Dim surface3 As New Rectangle((clientRectangle.Right - width), clientRectangle.Top, width, clientRectangle.Height)
            Dim surface4 As New Rectangle(clientRectangle.Left, (clientRectangle.Bottom - width), clientRectangle.Width, width)
            surface1.Intersect(clipRectangle)
            surface2.Intersect(clipRectangle)
            surface3.Intersect(clipRectangle)
            surface4.Intersect(clipRectangle)
            renderer.DrawBackground(e.Graphics, clientRectangle, surface1)
            renderer.DrawBackground(e.Graphics, clientRectangle, surface2)
            renderer.DrawBackground(e.Graphics, clientRectangle, surface3)
            renderer.DrawBackground(e.Graphics, clientRectangle, surface4)
            Using pen As Pen = New Pen(Me.BackColor)
                Dim rect As Rectangle = bounds
                rect.X -= 1
                rect.Y -= 1
                rect.Width += 1
                rect.Height += 1
                e.Graphics.DrawRectangle(pen, rect)
                GoTo SkipToEnd
            End Using
        End If
        Using pen2 As Pen = New Pen(Me.BackColor, If(MyBase.Enabled, CSng(2), CSng(1)))
            Dim tmpSurface As Rectangle = bounds
            tmpSurface.Inflate(1, 1)
            If Not MyBase.Enabled Then
                tmpSurface.X -= 1
                tmpSurface.Y -= 1
                tmpSurface.Width += 1
                tmpSurface.Height += 1
            End If
            e.Graphics.DrawRectangle(pen2, tmpSurface)
        End Using
SkipToEnd:
        'If ((Not MyBase.Enabled AndAlso (Me.Borderstyle <> Borderstyle.None)) AndAlso Not myValueControl.ShouldSerializeBackColor) Then
        'bounds.Inflate(1, 1)
        'ControlPaint.DrawBorder(e.Graphics, bounds, SystemColors.Control, ButtonBorderStyle.Solid)
        'End If
    End Sub
#End Region

#Region "Focus-Handling"

    Protected Overrides Sub OnBackColorChanged(ByVal e As System.EventArgs)
        MyBase.OnBackColorChanged(e)
        myBackColorBrush = Nothing
        myOriginalBackcolor = Me.BackColor
        If Not myIsFocused Then
            'Wenn Hintergrund transparent ist, dass ändern wir den Hintergrund auf weiß.
            'Durchsichtig geht nicht bei einer Textbox-Orientierten Anwendung.
            Me.myValueControl.TextBoxPart.BackColor = If(Me.BackColor = Color.Transparent, Color.White, Me.BackColor)
        End If
    End Sub

    Protected Overrides Sub OnEnter(ByVal e As System.EventArgs)
        MyBase.OnEnter(e)

        If Me.DesignMode Then
            Return
        End If

        myIsFocused = True
        'If Not Me.TextBoxPart.Focused Then
        '    Me.TextBoxPart.Focus()
        'End If

        myDoesLostFocusPrecedeValidate = True
        mySuspendHandleIsDirty = True
        OnSetEditedValue()
        mySuspendHandleIsDirty = False
        If OnFocusColor Then
            myValueControl.TextBoxPart.BackColor = FocusColor
        End If

        If myNullValueColor.HasValue Then
            Me.TextBoxPart.ForeColor = ForeColor
        End If

        If FocusSelectionBehaviour = FocusSelectionBehaviours.PreSelectInput Then
            myValueControl.TextBoxPart.SelectAll()
        ElseIf FocusSelectionBehaviour = FocusSelectionBehaviours.PlaceCaretAtEnd Then
            myValueControl.TextBoxPart.SelectionStart = myValueControl.Value.ToString.Length
            myValueControl.TextBoxPart.SelectionLength = 0
        Else
            myValueControl.TextBoxPart.SelectionStart = 0
            myValueControl.TextBoxPart.SelectionLength = 0
        End If
    End Sub

    Protected Overrides Sub OnGotFocus(ByVal e As System.EventArgs)
        If Not myDoesLostFocusPrecedeValidate Then
            myDoesLostFocusPrecedeValidate = True
        End If
        MyBase.OnGotFocus(e)
    End Sub

    ''' <summary>
    ''' Setzt beim Fokussieren des Elements die editierbare Repräsentation des Wertes in die Textbox.
    ''' </summary>
    ''' <remarks></remarks>
    Protected Overridable Sub OnSetEditedValue()
        mySuppresValueChangedOnTextPartTextChange = True
        myValueControl.Value = myEditedValue
    End Sub

    Protected Overrides Sub OnLeave(ByVal e As System.EventArgs)
        MyBase.OnLeave(e)
        If Me.DesignMode Then
            Return
        End If

        myIsFocused = False
        If OnFocusColor Then
            If myOriginalBackcolor = Color.Transparent Then
                myValueControl.TextBoxPart.BackColor = Color.White
            Else
                myValueControl.TextBoxPart.BackColor = myOriginalBackcolor
            End If
        End If

        If Me.AutoValidateOnLeaving Then
            Dim ce As New CancelEventArgs
            myEditedValue = myValueControl.Value.ToString
            ValidateInternal(ce)
            If ce.Cancel Then
                Me.Focus()
                Return
            End If
            OnValidated(EventArgs.Empty)
        End If

        If myNullValueColor.HasValue Then
            If Not Me.Value.HasValue Then
                Me.TextBoxPart.ForeColor = NullValueColor
            End If
        End If

    End Sub

    Protected Overrides Sub OnValidating(ByVal e As System.ComponentModel.CancelEventArgs)
        MyBase.OnValidating(e)
        If Me.DesignMode Then
            Return
        End If

        If e.Cancel Then
            Return
        End If

        'Sicherstellen, dass hier nichts Schlimmes passiert, wenn OnValidating 
        'durch ein Formular aufgerufen wird, weil ein Button beispielsweise 
        'CausesValidation auf True gesetzt hat.
        If Not myDoesLostFocusPrecedeValidate Then
            Exit Sub
        End If
        myEditedValue = myValueControl.Value.ToString
        ValidateInternal(e)
    End Sub

    'WICHTIG: Sollte diese Verhaltensweise an dieser Stelle geändert werden müssen,
    'wäre es wichtig zu schauen, ob die ähnlich implementierte Verhaltensweise auch
    'bei NullableValueRelationPopup angepasst werden muss.
    Private Sub ValidateInternal(ByVal e As CancelEventArgs)
        'ValidateInput überprüft, ob die Eingabe i.O. geht, und liefert 
        'bei einem Fehler eine Exception zurück, die letzten Endes
        'auch den Fehlertext ergibt.
        myLastFailedValidationException = ValidateInput(myValueControl.Value.ToString)

        If myLastFailedValidationException IsNot Nothing Then

            'Bei Bedarf, kann hier ein Warnton ausgegeben werden.
            If myBeepOnFailedValidation Then
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

    Protected Sub SetFailedValidation(ByVal Message As String) Implements INullableValueEditor.SetFailedValidation
        myFailedValidationMessage = Message
        Me.ValueControl.TextBoxPart.BackColor = Me.ErrorColor
    End Sub

    Protected Sub ClearFailedValidation() Implements INullableValueEditor.ClearFailedValidation
        myFailedValidationMessage = Nothing
        If myOriginalBackcolor = Color.Transparent Then
            myValueControl.TextBoxPart.BackColor = Color.White
        Else
            myValueControl.TextBoxPart.BackColor = myOriginalBackcolor
        End If
    End Sub

    Protected Sub OnRequestValidationFailedReaction(ByVal e As RequestValidationFailedReactionEventArgs)
        RaiseEvent RequestValidationFailedReaction(Me, e)
    End Sub

    Protected Function TryGetValue() As Tuple(Of NullableType?, Exception)
        If Me.Focused Then
            Dim valRes = ValidateInput(myValueControl.Value.ToString)
            If valRes IsNot Nothing Then
                Return New Tuple(Of NullableType?, Exception)(Nothing, valRes)
            Else
                myFormatterEngine.FormatString = FormatString
                myFormatterEngine.ConvertToValue(myValueControl.Value.ToString)
                Return New Tuple(Of NullableType?, Exception)(ChangeValuetypeInternally(FormatterEngine.Value), Nothing)
            End If
        Else
            Return New Tuple(Of NullableType?, Exception)(Me.Value, Nothing)
        End If
    End Function

    Public Overrides ReadOnly Property Focused As Boolean
        Get
            Return myIsFocused Or myEmulatedIsFocusedForUnitTesting
        End Get
    End Property

    ''' <summary>
    ''' Infrastructure. Don't use directly, it's only meant for internal Unit-Test purposes. 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
     EditorBrowsable(EditorBrowsableState.Never),
     Browsable(False)>
    Public Property EmulatedIsFocusedForUnitTesting As Boolean
        Get
            Return myEmulatedIsFocusedForUnitTesting
        End Get
        Set(value As Boolean)
            myEmulatedIsFocusedForUnitTesting = value
        End Set
    End Property

    Protected Overridable Function ValidateInput(ByVal text As String) As ContainsUIMessageException

        If (myValueControl.Value Is Nothing OrElse String.IsNullOrWhiteSpace(myValueControl.Value.ToString)) And
                    Not String.IsNullOrWhiteSpace(Me.NullValueMessage) Then
            Return New ContainsUIMessageException(Me.NullValueMessage, Me.NullValueMessage)
        End If

        Try
            If Me.myIsFocused Then
                FormatterEngine.ConvertToValue(myValueControl.Value.ToString)
            Else
                FormatterEngine.ConvertToValue(myEditedValue)
            End If
            Dim tmpValue = ChangeValuetypeInternally(FormatterEngine.Value)
            Dim nullValEvArgs = New NullableValueValidationEventArgs(Of NullableType?)(tmpValue)

            OnValueValidating(nullValEvArgs)

            If Not String.IsNullOrWhiteSpace(nullValEvArgs.ValidationFailedUIMessage) Then
                Return New ContainsUIMessageException("Fehler bei der Validierung der Eingabe in 'ValidateInput' für Control '." & Me.Name & "'.",
                                                      nullValEvArgs.ValidationFailedUIMessage)
            End If
            FormatterEngine.Value = nullValEvArgs.Value
            Return Nothing
        Catch ex As Exception
            Return GetFailedValidationException(ex)
        End Try
    End Function

    Protected Overridable Function GetFailedValidationException(ex As Exception) As ContainsUIMessageException
        Return New ContainsUIMessageException("Fehler bei der Validierung der Eingabe in 'ValidateInput' für Control '." & Me.Name & "'.",
                                      "Beim Überprüfen der Eingabe wurde ein Fehler entdeckt. Bitte überprüfen Sie Ihre Eingabe.",
                                      ex)
    End Function

    Protected Overridable Function ValidateContent() As ContainsUIMessageException Implements INullableValueEditor.ValidateContent
        If Me.myIsFocused Then
            Return ValidateInput(Me.myValueControl.Value.ToString)
        Else
            Return ValidateInput(Me.myEditedValue)
        End If
    End Function

    Protected Overrides Sub OnValidated(ByVal e As System.EventArgs)
        MyBase.OnValidated(e)
        If Me.DesignMode Then
            Return
        End If

        Try
            If Not myDoesLostFocusPrecedeValidate Then
                Return
            End If
        Finally
            myDoesLostFocusPrecedeValidate = False
        End Try

        'Hier können immer noch Fehler auftreten, wenn die In-Time-Validierung overruled wurde! 
        Try
            myEditedValue = myValueControl.Value.ToString
            FormatterEngine.ConvertToValue(myEditedValue)
            myValueChangedInternally = True
            ValueAsObject = ChangeValuetypeInternally(FormatterEngine.Value)
            Dim nullValEvArgs = New NullableValueValidationEventArgs(Of NullableType?)(DirectCast(ValueAsObject, NullableType?))
            OnValueValidated(nullValEvArgs)
            myValueChangedInternally = False
            mySuspendHandleIsDirty = True
            If Not Me.Focused Then
                myValueControl.Value = FormatterEngine.ConvertToDisplay
            Else
                myDoesLostFocusPrecedeValidate = True
            End If

            mySuspendHandleIsDirty = False
            ClearFailedValidation()
        Catch ex As Exception
            SetFailedValidation(ex.Message)
        End Try
    End Sub

    ''' <summary>
    ''' Löst das ValueValidating-Ereignis aus.
    ''' </summary>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Overridable Sub OnValueValidating(ByVal e As NullableValueValidationEventArgs(Of NullableType?))
        Dim newValueValidationState = ValueValidationStateStore(Of NullableType).GetPredefinedValidatingInstance
        newValueValidationState.RaiseValueValidationStateChangedEventUnconditionally = True
        newValueValidationState.ValidationEventArgs = e
        Me.ValueValidationState = newValueValidationState
        RaiseEvent ValueValidating(Me, e)
    End Sub

    ''' <summary>
    ''' Löst das ValueValidated-Ereignis aus.
    ''' </summary>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Overridable Sub OnValueValidated(ByVal e As NullableValueValidationEventArgs(Of NullableType?))
        Dim newValueValidationState = ValueValidationStateStore(Of NullableType).GetPredefinedValidatedInstance
        newValueValidationState.RaiseValueValidationStateChangedEventUnconditionally = True
        newValueValidationState.ValidationEventArgs = e
        Me.ValueValidationState = newValueValidationState
        RaiseEvent ValueValidated(Me, e)
    End Sub

    ''' <summary>
    ''' Methode, in der ein Typ durch Mehrfachverwendung einer Formatter-Engine gewechselt werden kann.
    ''' </summary>
    ''' <param name="Value">Wert, dessen Typ geändert werden kann oder soll.</param>
    ''' <returns>Wert als neuen Typ.</returns>
    ''' <remarks>Mithilfe dieser Methode, die in abgeleiteten Klassen überschrieben werden kann, 
    ''' lassen sich Formatter-Engines mehrfach verwenden, obwohl sie gleiche Typen 
    ''' für unterschiedliche Eingabefeldtypen bedienen. Bei einem Integer-Eingabefeld kann 
    ''' auf diese Weise zum Beispiel die Decimal-Formatter-Engine (NullableNumValueFormatterEngine) für die Formeleingabe verwendet werden; 
    ''' der eigentliche Datentyp wird durch Überschreiben dieser Methode von Decimal in Integer geändert.</remarks>
    Protected Overridable Function ChangeValuetypeInternally(ByVal Value As Object) As NullableType?
        Return DirectCast(Value, NullableType?)
    End Function

    ''' <summary>
    ''' Methode, in der ein Typ durch Mehrfachverwendung einer Formatter-Engine gewechselt werden kann.
    ''' </summary>
    ''' <param name="Value">Wert, dessen Typ in den Ausgangstyp geändert werden soll.</param>
    ''' <returns>Wert als neuen Typ.</returns>
    ''' <remarks>Mithilfe dieser Methode, die in abgeleiteten Klassen überschrieben werden kann, 
    ''' lassen sich Formatter-Engines mehrfach verwenden, obwohl sie gleiche Typen 
    ''' für unterschiedliche Eingabefeldtypen bedienen. Dabei bildet diese Methode das Gegenstück 
    ''' zur Methode ChangeValueTypeInternally, bei der zum Beispiel bei einem Integer-Eingabefeld 
    ''' auf diese Weise die Decimal-Formatter-Engine (NullableNumValueFormatterEngine) für die Formeleingabe verwendet werden kann; 
    ''' der eigentliche Datentyp wird durch Überschreiben dieser Methode von Decimal in Integer geändert. 
    ''' Durch das Überschreiben dieser Methode in abgeleiteten Klassen wird der Datentypen für die Anzeige wieder 
    ''' vom konvertierten Datentyp (Integer) in den Ausgangsdatentyp (Decimal) geändert.</remarks>
    Protected Overridable Function RechangeValueTypeInternally(ByVal value As NullableType?) As Object
        Return value
    End Function

    ''' <summary>
    ''' Bestimmt oder Ermittelt die Verhaltensweise des Vorselektierens des Steuerelementtextes, wenn es den Fokus erhält. 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
     Description("Bestimmt oder Ermittelt die Verhaltensweise des Vorselektierens des Steuerelementtextes, wenn es den Fokus erhält."),
     Category("Verhalten"),
     EditorBrowsable(EditorBrowsableState.Always),
     Browsable(True)>
    Public Property FocusSelectionBehaviour As FocusSelectionBehaviours
        Get
            Return myFocusSelectionBehaviour
        End Get
        Set(ByVal value As FocusSelectionBehaviours)
            myFocusSelectionBehaviour = value
        End Set
    End Property

    Public Function ShouldSerializeFocusSelectionBehaviour() As Boolean
        Return Not (Me.FocusSelectionBehaviour = NullableControlManager.GetInstance.GetDefaultFocusSelectionBehaviour(Me, DEFAULT_FOCUS_SELECTION_BEHAVIOUR))
    End Function

    Public Sub ResetFocusSelectionBehaviour()
        Me.FocusSelectionBehaviour = NullableControlManager.GetInstance.GetDefaultFocusSelectionBehaviour(Me, DEFAULT_FOCUS_SELECTION_BEHAVIOUR)
    End Sub

    ''' <summary>
    ''' Bestimmt oder ermittelt, ob ein Warnton bei einer fehlgeschlagenen Validierung ausgegeben werden soll.  
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
     Description("Bestimmt oder ermittelt, ob ein Warnton bei einer fehlgeschlagenen Validierung ausgegeben werden soll."),
     Category("Verhalten"),
     EditorBrowsable(EditorBrowsableState.Always),
     Browsable(True)>
    Public Property BeepOnFailedValidation As Boolean
        Get
            Return myBeepOnFailedValidation
        End Get
        Set(ByVal value As Boolean)
            myBeepOnFailedValidation = value
        End Set
    End Property

    Public Function ShouldSerializeBeepOnFailedValidation() As Boolean
        Return Not (BeepOnFailedValidation = NullableControlManager.GetInstance.GetDefaultBeepOnFailedValidation(Me, DEFAULT_BEEP_ON_FAILED_VALIDATION))
    End Function

    ''' <summary>
    ''' Bestimmt oder ermittelt, ob das Steuerelement mit FocusColor eingefärbt werden soll, wenn das Steuerelement den Fokus erhält. 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
     Description("Bestimmt oder ermittelt, ob das Steuerelement mit FocusColor eingefärbt werden soll, wenn das Steuerelement den Fokus erhält."),
     Category("Verhalten"),
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
        Return Not (OnFocusColor = NullableControlManager.GetInstance.GetDefaultOnFocusColor(Me, DEFAULT_ON_FOCUS_COLOR))
    End Function

    ''' <summary>
    ''' Bestimmt oder ermittelt die Farbe, die das Steuerelement bei einer erlaubten Fehlvalidierung bekommt. 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
     Description("Bestimmt oder ermittelt die Farbe, die das Steuerelement bei einer erlaubten Fehlvalidierung bekommt."),
     Category("Verhalten"),
     EditorBrowsable(EditorBrowsableState.Always),
     Browsable(True)>
    Public Property ErrorColor As Color
        Get
            Return myErrorColor
        End Get
        Set(ByVal value As Color)
            myErrorColor = value
        End Set
    End Property

    Public Function ShouldSerializeErrorColor() As Boolean
        Return Not (ErrorColor = NullableControlManager.GetInstance.GetDefaultErrorColor(Me, DEFAULT_ERROR_COLOR))
    End Function

    ''' <summary>
    ''' Bestimmt oder ermittelt die Farbe, die im Bedarfsfall vorselektiert werden soll, wenn das Steuerelement den Fokus erhält. 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
     Description("Bestimmt oder ermittelt die Farbe, die im Bedarfsfall vorselektiert werden soll, wenn das Steuerelement den Fokus erhält."),
     Category("Verhalten"),
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
        Return Not FocusColor = NullableControlManager.GetInstance.GetDefaultFocusColor(Me, DEFAULT_FOCUS_COLOR)
    End Function

    ''' <summary>
    ''' Bestimmt oder ermittelt, ob das Steuerelement automatisch validiert werden soll, 
    ''' wenn ein anderes Steuerelement selektiert wird, das aber keinen GotFocus auslöst (Toolbar-Button, z.B.).
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
     Description("Bestimmt oder ermittelt, ob das Steuerelement automatisch validiert werden soll, wenn ein anderes Steuerelement selektiert wird, das aber keinen GotFocus auslöst (Toolbar-Button, z.B.)."),
     Category("Verhalten"),
     EditorBrowsable(EditorBrowsableState.Always),
     Browsable(True), DefaultValue(False)>
    Public Property AutoValidateOnLeaving As Boolean

#End Region

#Region "Value Handling"

    <Browsable(False), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public Overrides Property Text As String
        Get
            Return MyBase.Text
        End Get
        Set(ByVal value As String)
            MyBase.Text = value
        End Set
    End Property

    'Überschreiben wir, um bessere Infos in den Tooltips beim Debuggen zu erhalten.
    Public Overrides Function ToString() As String
        Return Me.GetType.Name & " - Name: " & If(String.IsNullOrWhiteSpace(Me.Name), "- - -", Me.Name) &
            "; Wert: " & If(Me.Value IsNot Nothing, Me.Value.ToString, Me.NullValueString)
    End Function

    ''' <summary>
    ''' Bestimmt oder ermittelt den Wert, den dieses Steuerelement repräsentiert.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
     Description("Bestimmt oder ermittelt den Wert, den dieses Steuerelement repräsentiert."),
     Category("Verhalten"),
     EditorBrowsable(EditorBrowsableState.Advanced),
     Browsable(True)>
    Public Property Value() As NullableType?
        Get
            If Not Me.DesignMode Then
                If Focused Then
                    Dim ret = TryGetValue()
                    If ret.Item2 IsNot Nothing Then
                        'Nur triggern - Gibt keinen Fokus zu halten, weil es kein Validate gab.
                        OnRequestValidationFailedReaction(New RequestValidationFailedReactionEventArgs(ValidationFailedActions.ForceKeepFocus, ""))
                        'TODO: Richtige Exception (neuer Typ) verwenden!
                        Throw New ArgumentOutOfRangeException(myFailedValidationMessage)
                    End If
                    Return ret.Item1
                End If
            End If
            Return myValue
        End Get

        Set(ByVal value As NullableType?)

            'Alter und neuer Wert nothing, es ändert sich nix.
            If Not value.HasValue And Not myValue.HasValue Then
                Me.IsLoading.Value = True
                UpdateValue()
                Me.IsLoading.Value = False
                Return
            End If

            'Alter und neuer Wert derselbe, es ändert sich nix.
            If value.HasValue And myValue.HasValue Then
                If value.Value.CompareTo(myValue.Value) = 0 Then
                    Me.IsLoading.Value = True
                    UpdateValue()
                    Me.IsLoading.Value = False
                    Return
                End If
            End If

            Me.IsLoading.Value = True
            Dim e As New ValueChangingEventArgs(Of NullableType?)(value)
            OnValueChanging(e)
            Dim tmpValueChanged = CompareValue(myValue, e.NewValue)
            myValue = e.NewValue
            UpdateValue()

            Dim evArgs As New ValueChangedEventArgs()
            If myValueChangedInternally Or myForceValueChangeCauseToUser Then
                evArgs.ValueChangedCause = ValueChangedCauses.User
            Else
                evArgs.ValueChangedCause = ValueChangedCauses.PropertySetter
            End If

            If Not tmpValueChanged.HasValue Then
                OnValueChanged(evArgs)
            ElseIf tmpValueChanged.Value <> 0 Then
                OnValueChanged(evArgs)
            End If
            OnInitializeEditedValue()
            Me.IsLoading.Value = False
        End Set
    End Property

    ''' <summary>
    ''' Wird von ableitenden Klassen verwendet, zum Setzen eines Wertes, wie 
    ''' wenn der Anwender den Wert verändert hätte, er also zum IsDirty führt,
    ''' was beim reinen Setzen der Value-Eigenschaft nicht der Fall wäre. 
    ''' </summary>
    ''' <param name="value"></param>
    ''' <remarks></remarks>
    Protected Sub SetValuePreserveOriginalValue(value As NullableType?)
        Dim tmp = Me.myOriginalValue
        Me.Value = value
        myOriginalValue = tmp
    End Sub

    ''' <summary>
    ''' Wird von abgeleiteten Klassen dann aufgerufen, wenn sie den Value verändern, und anzeigen wollen, 
    ''' dass Value durch den User von außen und nicht durch den Property Setter verändert wurde.
    ''' </summary>
    ''' <remarks>Dieser Aufruf muss mit EndChangeValue Internally abgeschlossen werden.</remarks>
    Protected Sub BeginForceValueChangeCauseToUser()
        If myForceValueChangeCauseToUser Then
            Throw New ArgumentException("Calling BeginForceValueChangeCauseToUser twice is not possible. You probably missed calling EndValueChangeCauseToUser.")
        End If
        myForceValueChangeCauseToUser = True
    End Sub

    ''' <summary>
    ''' Wird von abgeleiteten Klassen dann aufgerufen, nachdem sie den Value verändern, und anzeigen wollen, 
    ''' dass Value durch den User von außen und nicht durch den Property Setter verändert wurde.
    ''' </summary>
    ''' <remarks>Diesem Aufruf muss EndChangeValueInternally voran gehen.</remarks>
    Protected Sub EndValueChangeCauseToUser()
        If Not myForceValueChangeCauseToUser Then
            Throw New ArgumentException("Call BeginForceValueChangeCauseToUser before ending it.")
        End If
        myForceValueChangeCauseToUser = False
    End Sub

    Public Property ValueValidationState As ValueValidationStateStore(Of NullableType)
        Get
            Return myValueValidationState
        End Get
        Set(value As ValueValidationStateStore(Of NullableType))
            If Not Object.Equals(value, myValueValidationState) OrElse
                (value IsNot Nothing AndAlso value.RaiseValueValidationStateChangedEventUnconditionally) Then
                myValueValidationState = value
                OnValueValidationStateChanged(EventArgs.Empty)
            End If
        End Set
    End Property

    Protected Overridable Sub OnValueValidationStateChanged(e As EventArgs)
        RaiseEvent ValueValidationStateChanged(Me, e)
    End Sub

    Public Function CompareValue(ByVal v1 As NullableType?, ByVal v2 As NullableType?) As Integer?
        If (Not v1.HasValue) AndAlso (Not v2.HasValue) Then
            Return 0
        End If
        If (Not v1.HasValue) OrElse (Not v2.HasValue) Then
            Return Nothing
        End If
        Return v1.Value.CompareTo(v2.Value)
    End Function

    ''' <summary>
    ''' Ermittelt den letzten validierten Wert, den dieses Steuerelement repräsentiert; löst keine Neuvalidierung aus, wenn das Steuerelement den Fokus besitzt.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
     Description("Ermittelt den letzten validierten Wert, den dieses Steuerelement repräsentiert; löst keine Neuvalidierung beim Auslesen aus, wenn das Steuerelement den Fokus besitzt."),
     Category("Verhalten"),
     EditorBrowsable(EditorBrowsableState.Advanced),
     Browsable(False)>
    Public ReadOnly Property LastCommittedValue As NullableType?
        Get
            Return myValue
        End Get
    End Property

    ''' <summary>
    ''' Löst das ValueChanging-Ereignis aus.
    ''' </summary>
    ''' <param name="e">Die Ereignisparameter, die den alten und den neuen Wert enthalten.</param>
    ''' <remarks>Überschreiben Sie diese Methode in abgeleiteten Klassen, um den Wert, in den die Value-Eigenschaft 
    ''' geändert werden soll, beeinflussen zu können.</remarks>
    Protected Overridable Sub OnValueChanging(ByVal e As ValueChangingEventArgs(Of NullableType?))
        RaiseEvent ValueChanging(Me, e)
    End Sub

    ''' <summary>
    ''' Löst das ValueChanged-Ereignis aus.
    ''' </summary>
    ''' <param name="e">Leere Ereignisparameter.</param>
    ''' <remarks>Überschreiben Sie diese Methode in abgeleiteten Klassen, um den Zeitpunkt 
    ''' zu bestimmen, zu dem die Value-Eigenschaft geändert wurde.</remarks>
    Protected Overridable Sub OnValueChanged(ByVal e As ValueChangedEventArgs)
        HandleNullValueColorChanged()
        RaiseEvent ValueChanged(Me, e)
    End Sub

    Protected Overridable Sub InitializeValue()
        IsLoading.Value = True
        UpdateValue()
        IsLoading.Value = False
    End Sub

    ''' <summary>
    ''' Wird aufgerufen, sobald der Value-Eigenschaft ein neuer Wert zugewiesen wird, und sorgt dafür, 
    ''' dass die entsprechende Aktualisierung im Steuerelement stattfindet.
    ''' </summary>
    ''' <remarks>Berücksichtigt die unterschiedlicher Darstellung 
    ''' des Wertes bei Fokussierung/Nicht-Fokussierung des Steuerelements.</remarks>
    Protected Overridable Sub UpdateValue()
        FormatterEngine.Value = RechangeValueTypeInternally(myValue)
        FormatterEngine.NullValueString = Me.NullValueString
        FormatterEngine.FormatString = Me.FormatString
        myEditedValue = OnGetEditedValue()
        If myValueControl IsNot Nothing Then
            If Me.Focused Then
                myValueControl.Value = myEditedValue
            Else
                myValueControl.Value = FormatterEngine.ConvertToDisplay
            End If
        End If
    End Sub

    ''' <summary>
    ''' Ermittelt, ob die Value-Eigenschaft gegenwärtig Null ist.
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property IsValueNull As Boolean
        Get
            Return Not Me.Value.HasValue
        End Get
    End Property

    Protected Overridable Sub OnIsValueNullChanged(e As EventArgs)
        RaiseEvent IsValueNullChanged(Me, e)
    End Sub

    ''' <summary>
    ''' Ermittelt die initiale editierbaren Repräsentation des Wertes, wenn die Value-Eigenschaft zugewiesen wird.
    ''' </summary>
    ''' <returns>Zeichenkette, die der editierbaren Repräsentation des Wertes entspricht.</returns>
    ''' <remarks></remarks>
    Protected Overridable Function OnGetEditedValue() As String
        If myValueChangedInternally Then
            Return myEditedValue
        End If
        Return FormatterEngine.InitializeEditedValue
    End Function

    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
     EditorBrowsable(EditorBrowsableState.Advanced),
     Browsable(False)>
    Private Property ValueAsObject() As Object Implements INullableValueEditor.Value
        Get
            Return Me.Value
        End Get
        Set(ByVal value As Object)
            If value Is Nothing Then
                Me.Value = Nothing
            Else
                Me.Value = CTypeDynamic(Of NullableType)(value)
            End If
        End Set
    End Property

    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
     EditorBrowsable(EditorBrowsableState.Advanced),
     Browsable(False)>
    Public Property OriginalValue() As Nullable(Of NullableType)
        Get
            Return myOriginalValue
        End Get
        Set(ByVal value As Nullable(Of NullableType))
            myOriginalValue = value
        End Set
    End Property

    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
     EditorBrowsable(EditorBrowsableState.Advanced),
     Browsable(False)>
    Public Property OriginalValueAsObject() As Object Implements INullableValueEditor.OriginalValue
        Get
            Return OriginalValue
        End Get
        Set(ByVal value As Object)
            OriginalValue = DirectCast(value, Nullable(Of NullableType))
        End Set
    End Property
#End Region

#Region "Formatting-Handling"

    Protected MustOverride Function GetDefaultFormatterEngine() As INullableValueFormatterEngine

    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
     EditorBrowsable(EditorBrowsableState.Advanced),
     Browsable(False)>
    Public Property FormatterEngine() As INullableValueFormatterEngine Implements INullableValueEditor.FormatterEngine
        Get
            Return myFormatterEngine
        End Get
        Set(ByVal value As INullableValueFormatterEngine)
            myFormatterEngine = value
        End Set
    End Property

    ''' <summary>
    ''' Bestimmt oder ermittelt die Format-Zeichenfolge, die die Formatierung für das Anzeigen des Wertes nach Verlassen des Feldes vorgibt.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
     Description("Bestimmt oder ermittelt die Format-Zeichenfolge, die die Formatierung für das Anzeigen des Wertes nach Verlassen des Feldes vorgibt."),
     Category("Verhalten"),
     EditorBrowsable(EditorBrowsableState.Advanced),
     Browsable(False)>
    Public Overridable Property FormatString() As String Implements INullableValueEditor.FormatString
        Get
            Return myFormatString
        End Get
        Set(ByVal value As String)
            myFormatString = value
        End Set
    End Property

    ''' <summary>
    ''' Liefert in abgeleiteten Klassen den Standardwert für die die Format-Zeichenfolge, die die Formatierung für das Anzeigen des Wertes nach Verlassen des Feldes vorgibt.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected MustOverride Function GetDefaultFormatString() As String

    Public Function ShouldSerializeFormatString() As Boolean
        Return Not (Me.NullValueString = GetDefaultFormatString())
    End Function

    ''' <summary>
    ''' Bestimmt oder ermittelt die Zeichenfolge, die beim Verlassen des Steuerelements angezeigt wird, wenn eine Null-Eingabe erfolgte.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
     Description("Bestimmt oder ermittelt die Zeichenfolge, die beim Verlassen des Steuerelements angezeigt wird, wenn eine Null-Eingabe erfolgte."),
     Category("Verhalten"),
     EditorBrowsable(EditorBrowsableState.Always),
     Browsable(True)>
    Public Property NullValueString() As String Implements INullableValueEditor.NullValueString
        Get
            Return myNullValueString
        End Get
        Set(ByVal value As String)
            myNullValueString = value
            UpdateValue()
        End Set
    End Property

    ''' <summary>
    ''' Liefert in abgeleiteten Klassen den Standardwert für die Zeichenfolge, die beim Verlassen des Steuerelements angezeigt wird, wenn eine Null-Eingabe erfolgte.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected MustOverride Function GetDefaultNullValueString() As String

    Public Function ShouldSerializeNullValueString() As Boolean
        Return Not (Me.NullValueString = GetDefaultNullValueString())
    End Function

    ''' <summary>
    ''' Bestimmt oder ermittelt die Farbe, mit der der Inhalt des Steuerelementes angezeigt werden soll, wenn es den Wert 'null' widerspiegelt.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
     Description("Bestimmt oder ermittelt die Farbe, mit der der Inhalt des Steuerelementes angezeigt werden soll, wenn es den Wert 'null' widerspiegelt."),
     Category("Verhalten"),
     EditorBrowsable(EditorBrowsableState.Always),
     Browsable(True)>
    Public Property NullValueColor As Color
        Get
            If Not myNullValueColor.HasValue Then
                Return ForeColor
            Else
                Return myNullValueColor.Value
            End If
        End Get
        Set(value As Color)
            If Not Object.Equals(value, NullValueColor) Then
                If value.Equals(ForeColor) Then
                    myNullValueColor = Nothing
                Else
                    myNullValueColor = value
                End If
                'RaiseEvent
                OnNullValueColorChanged(EventArgs.Empty)
                HandleNullValueColorChanged()
            End If
        End Set
    End Property

    Protected Overridable Sub OnNullValueColorChanged(e As EventArgs)
        RaiseEvent NullValueColorChanged(Me, e)
    End Sub

    Private Function ShouldSerializeNullValueColor() As Boolean
        Return myNullValueColor.HasValue
    End Function

    Private Sub ResetNullValueColor()
        myNullValueColor = Nothing
    End Sub

    Private Sub HandleNullValueColorChanged()
        'Nur wenn ich nicht fokussiert bin...
        If Not Me.Focused Then
            If Not Me.Value.HasValue Then
                Me.TextBoxPart.ForeColor = Me.NullValueColor
            Else
                Me.TextBoxPart.ForeColor = Me.ForeColor
            End If
        End If
    End Sub

#End Region

#Region "IsDirty-Handling"

    ''' <summary>
    ''' Wird aufgerufen, wenn der bearbeitete Wert neu initialisiert wird (durch den Setter der Value-Eigenschaft)
    ''' </summary>
    ''' <remarks></remarks>
    Protected Overridable Sub OnInitializeEditedValue()
        myEditedValue = myFormatterEngine.InitializeEditedValue
        myOriginalEditedValue = myEditedValue
        myOriginalValue = myValue
    End Sub

    Protected Overrides Sub OnTextChanged(ByVal e As System.EventArgs)
        MyBase.OnTextChanged(e)
        If Not IsLoading.Value Then
            HandleIsDirty()
        End If
    End Sub

    Protected Overrides Sub OnForeColorChanged(e As EventArgs)
        MyBase.OnForeColorChanged(e)
        Me.TextBoxPart.ForeColor = Me.ForeColor
    End Sub

    Protected Overridable Sub TagDirtyState()
        If Not IsDirty Then
            IsDirty = CheckIsDirty()
        End If
    End Sub

    Protected Overridable Sub HandleIsDirty()

        'Verhalten geändert 8.11.2011: Nur wenn das Feld nicht fokussiert ist, reagieren wir nicht auf das Zurückändern
        'in den NullValue-String. Ansonsten käme kein IsDirtyChanged, wenn (beispielsweise durch Delete oder Paste)
        'direkt in den NullValueString geändert würde.
        If Not Me.Focused Then
            If Me.Text = Me.NullValueString Then
                Return
            End If
        End If

        'Wenn GotFocus/Enter/LostFocus/Leave ein Umformatieren notwendig macht, dann ignorieren
        If mySuspendHandleIsDirty Then Return

        If Me.Text <> myOriginalEditedValue Then
            TagDirtyState()
        End If
    End Sub

    Protected Overridable Sub OnIsDirtyChanged(ByVal e As IsDirtyChangedEventArgs)
        RaiseEvent IsDirtyChanged(Me, e)
    End Sub

    ''' <summary>
    ''' Ermittelt ob sich der Value-Wert seit der letzten (ersten) Zuweisung geändert hat, und ein Datensatz deswegen aktualisiert werden muss.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
     EditorBrowsable(EditorBrowsableState.Advanced),
     Browsable(False)>
    Public Overridable Property IsDirty() As Boolean Implements INullableValueDataBinding.IsDirty
        Get
            Return myIsDirty
        End Get

        Protected Set(value As Boolean)
            If value <> myIsDirty Then
                myIsDirty = value
                OnIsDirtyChanged(New IsDirtyChangedEventArgs(Me))
            End If
        End Set
    End Property

    Protected Overridable Function CheckIsDirty() As Boolean

        Dim tmpValue As NullableType? = Nothing
        Try
            'Parsen des Wertes; hier können komplett unvollständige Werte eingegeben werden, deswegen der Try/Catch/Block.
            tmpValue = Value
        Catch ex As Exception
            'ASSUMPTION:
            'If that was has been entered by the user so far cannot be converted
            'in a valid value, we can assume that the input is dirty.
            Return True
        End Try

        'Damit der folgende Test funktionieren kann, muss OriginalValue in 
        'OnInitializeEditedValue gesetzt werden, damit der Ausgangswert 
        'mit dem eigentlichen Wert verglichen werden kann.
        If OriginalValue.HasValue And tmpValue.HasValue Then
            'Neuer Wert ist nicht null und alter Wert auch nicht --> Dirty, wenn sich die Werte unterscheiden.
            Return OriginalValue.Value.CompareTo(tmpValue.Value) <> 0
        ElseIf Not OriginalValue.HasValue AndAlso Not tmpValue.HasValue Then
            'Ursprungswert war null, neuer Wert ist null --> NOT dirty.
            Return False
        ElseIf Not OriginalValue.HasValue AndAlso tmpValue.HasValue Then
            'Ursprungswert war null, neuer Wert war null --> dirty.
            Return True
        ElseIf OriginalValue.HasValue AndAlso Not tmpValue.HasValue Then
            'Ursprungswert war nicht null, neuer Wert ist null --> dirty.
            Return True
        Else
            'alles andere --> nicht dirty. (Hier dürften wir nie hinkommen.)
            Return False
        End If

    End Function

    ''' <summary>
    ''' Setzt den Status zurück, dass dieses Feld vom Anwender geändert wurde, und sein Value in der Datenquelle aktualisiert werden sollte.
    ''' </summary>
    ''' <remarks>Diese Methode wird von der Infrastruktur verwendet, und sollte nicht direkt angewendet werden.</remarks>
    Public Sub ResetIsDirty() Implements INullableValueDataBinding.ResetIsDirty
        myIsDirty = False
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
            If myIsLoading Is Nothing Then
                myIsLoading = New HistoricalBoolean With {.Value = False}
            End If
            Return myIsLoading
        End Get
        Set(ByVal value As HistoricalBoolean)
            If value Is Nothing Then
                myIsLoading = New HistoricalBoolean With {.Value = False}
            Else
                myIsLoading.Value = value.Value
            End If
        End Set
    End Property
#End Region

#Region "Databinding-Handling"
    ''' <summary>
    ''' Bestimmt oder Ermittelt den Datenquellen-Feldnamen des Feldes, mit dem dieses Steuerelement verknüpft werden soll.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
     Description("Bestimmt oder Ermittelt den Datenquellen-Feldnamen des Feldes, mit dem dieses Steuerelement verknüpft werden soll."),
     Category("Verhalten"),
     EditorBrowsable(EditorBrowsableState.Always),
     Browsable(True),
     TypeConverter(GetType(DatafieldNameConverter))>
    Public Property DatafieldName As String Implements INullableValueDataBinding.DatafieldName

    Public Function ShouldSerializeDatafieldName() As Boolean
        Return Not String.IsNullOrEmpty(DatafieldName)
    End Function

    Public Sub ResetDatafieldName()
        DatafieldName = Nothing
    End Sub

    ''' <summary>
    ''' Bestimmt oder ermittelt die FormToBusinessClassManager-Komponente, die die Verwaltung dieses NullableValue-Controls übernimmt.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
     Description("Bestimmt oder ermittelt die FormToBusinessClassManager-Komponente, die die Verwaltung dieses NullableValue-Controls übernimmt."),
     Category("Verhalten"),
     EditorBrowsable(EditorBrowsableState.Always),
     Browsable(True)>
    Public Property AssignedManagerComponent As FormToBusinessClassManager Implements INullableValueControl.AssignedManagerControl

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
     Category("Verhalten"),
     EditorBrowsable(EditorBrowsableState.Always),
     Browsable(True)>
    Public Property DatafieldDescription As String Implements INullableValueDataBinding.DatafieldDescription
        Get
            Return myDatafieldDescription
        End Get
        Set(ByVal value As String)
            myDatafieldDescription = value
        End Set
    End Property

    Public Function ShouldSerializeDatafieldDescription() As Boolean
        Return Not String.IsNullOrEmpty(DatafieldDescription)
    End Function

    Public Sub ResetDatafieldDescription()
        DatafieldDescription = Nothing
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
     Category("Verhalten"),
     EditorBrowsable(EditorBrowsableState.Always),
     Browsable(True)>
    Public Property NullValueMessage As String Implements INullableValueDataBinding.NullValueMessage
        Get
            Return myNullValueMessage
        End Get
        Set(ByVal value As String)
            myNullValueMessage = value
        End Set
    End Property

    Public Function ShouldSerializeNullValueMessage() As Boolean
        Return Not String.IsNullOrEmpty(NullValueMessage)
    End Function

    Public Sub ResetNullValueMessage()
        NullValueMessage = Nothing
    End Sub
#End Region

#Region "Other Properties (Keyboard, Readonly, Security)"

    ''' <summary>
    ''' Returns or sets a flag which determines that the use can cycle between entry fields with Page up and Page down rather than Tab and Shift+Tab.
    ''' </summary>
    ''' <returns></returns>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
     Description("Returns or sets if the user can cycle between entry fields with Page up and Page down in addition to Tab and Shift+Tab."),
     Category("Behaviour"),
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

    ''' <summary>
    ''' Definiert, ob Daten im Steuerelement nur dargestellt (true) oder auch verändert werden können.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
     Description("Definiert, ob Daten im Steuerelement nur dargestellt (true) oder auch verändert werden können."),
     Category("Verhalten"),
     EditorBrowsable(EditorBrowsableState.Always),
     Browsable(True), DefaultValue(False)>
    Public Overridable Property [ReadOnly] As Boolean
        Get
            Return Me.TextBoxPart.ReadOnly
        End Get
        Set(ByVal value As Boolean)
            If value <> Me.TextBoxPart.ReadOnly Then
                Me.TextBoxPart.ReadOnly = value
                OnReadOnlyChanged(EventArgs.Empty)
            End If
        End Set
    End Property

    Protected Overridable Sub OnReadOnlyChanged(e As EventArgs)
        RaiseEvent ReadOnlyChanged(Me, e)
    End Sub

    ''' <summary>
    ''' Bestimmt oder ermittelt das Zeichen, das bei eingeschaltete Obfuskierung 
    ''' anstelle des wirklichen Inhaltes angezeigt werden sollen.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
     Description("Bestimmt oder ermittelt das Zeichen, das bei eingeschalteter Obfuskierung anstelle des wirklichen Inhaltes angezeigt werden sollen."),
     Category("Verhalten"),
     EditorBrowsable(EditorBrowsableState.Always),
     Browsable(True), DefaultValue(Char.MinValue)>
    Overridable Property ObfuscationChar As Char?

    ''' <summary>
    ''' Bestimmt oder ermittelt, ob es sich bei einem Eingabefeld um ein Key-Feld handelt oder nicht.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
     Description("Bestimmt oder ermittelt, ob es sich bei einem Eingabefeld um ein Key-Feld handelt oder nicht."),
     Category("Verhalten"),
     EditorBrowsable(EditorBrowsableState.Always),
     Browsable(True), DefaultValue(False)>
    Overridable Property IsKeyField As Boolean Implements IKeyFieldProvider.IsKeyField

    ''' <summary>
    ''' Bestimmt die Dauer in Millisekunden, die ein Baloontip im Falle einer Fehlermeldung angezeigt wird.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
     Description("Bestimmt oder ermittelt die Dauer in Millisekunden, die ein Baloontip im Falle einer Fehlermeldung angezeigt wird."),
     Category("Verhalten"),
     EditorBrowsable(EditorBrowsableState.Always),
     Browsable(True), DefaultValue(5000)>
    Public Property ExceptionBalloonDuration As Integer Implements INullableValueControl.ExceptionBalloonDuration
        Get
            Return myExceptionBalloonDuration
        End Get
        Set(value As Integer)
            If Not Object.Equals(value, myExceptionBalloonDuration) Then
                myExceptionBalloonDuration = value
                OnExceptionBalloonDurationChanged(EventArgs.Empty)
            End If
        End Set
    End Property

    Protected Overridable Sub OnExceptionBalloonDurationChanged(e As EventArgs)
        RaiseEvent ExceptionBalloonDurationChanged(Me, e)
    End Sub

    Protected ReadOnly Property ValueControl As ControlType
        Get
            Return myValueControl
        End Get
    End Property

    ''' <summary>
    ''' Bestimmt oder ermittelt die maximale Anzahl an Zeichen/Ziffern, die in dieses Feld eingegeben werden können.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
     Description("Bestimmt oder ermittelt die maximale Anzahl an Zeichen/Ziffern, die in dieses Feld eingegeben werden können."),
     Category("Verhalten"),
     EditorBrowsable(EditorBrowsableState.Always),
     Browsable(True), DefaultValue(0)>
    Public Property MaxLength As Integer
        Get
            Return Me.TextBoxPart.MaxLength
        End Get
        Set(value As Integer)
            If Not Object.Equals(Me.TextBoxPart.MaxLength, value) Then
                Me.TextBoxPart.MaxLength = value
                OnMaxLengthChanged(EventArgs.Empty)
            End If
        End Set
    End Property

    Protected Overridable Sub OnMaxLengthChanged(e As EventArgs)
        RaiseEvent MaxLengthChanged(Me, e)
    End Sub

    ''' <summary>
    ''' Bestimmt oder ermittelt, ob bei einem Überlauf in der TextBox der vordere oder der hintere Teiltext angezeigt wird.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
     Description("Bestimmt oder ermittelt, ob bei einem Überlauf in der TextBox der vordere oder der hintere Teiltext angezeigt wird."),
     Category("Verhalten"),
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
                        Me.TextAlign = HorizontalAlignment.Right
                    Else
                        Me.TextBoxPart.RightToLeft = System.Windows.Forms.RightToLeft.No
                        Me.TextAlign = HorizontalAlignment.Left
                    End If
                End If
                OnReverseTextOverflowBehaviourChanged(EventArgs.Empty)
            End If
        End Set
    End Property

    Protected Overridable Sub OnReverseTextOverflowBehaviourChanged(e As EventArgs)
        RaiseEvent ReverseTextOverflowBehaviourChanged(Me, e)
    End Sub

    ''' <summary>
    ''' Ermittelt oder bestimmt, wie der Text innerhalb des Steuerelementes ausgerichtet wird.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
     Description("Ermittelt oder bestimmt, wie der Text innerhalb des Steuerelementes ausgerichtet wird."),
     Category("Verhalten"),
     EditorBrowsable(EditorBrowsableState.Always),
     Browsable(True), DefaultValue(HorizontalAlignment.Left)>
    Public Property TextAlign As HorizontalAlignment
        Get
            Return Me.TextBoxPart.TextAlign
        End Get
        Set(value As HorizontalAlignment)
            If Not Object.Equals(Me.TextBoxPart.TextAlign, value) Then
                If Me.TextAlign = HorizontalAlignment.Right AndAlso ReverseTextOverflowBehaviour Then
                    ReverseTextOverflowBehaviour = False
                End If
                Me.TextBoxPart.TextAlign = value
                OnTextAlignChanged(EventArgs.Empty)
            End If
        End Set
    End Property

    Protected Overridable Sub OnTextAlignChanged(e As EventArgs)
        RaiseEvent TextAlignChanged(Me, e)
    End Sub

#End Region

#Region "Designer-Unterstützung"

    ''' <summary>
    ''' Infrastruktur - dient nur zur Designer-Unterstützung.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
     Description("Infrastruktur. Dient nur zur Designerunterstützung."),
     Category("Verhalten"),
     EditorBrowsable(EditorBrowsableState.Advanced),
     Browsable(False)>
    Protected ReadOnly Property TextBoxPart As System.Windows.Forms.TextBox Implements ITextBoxBasedControl.TextBoxPart
        Get
            Return myValueControl.TextBoxPart
        End Get
    End Property


    ''' <summary>
    ''' Bestimmt, dass 1 Pixel mehr an Snapline-Offset für die Baseline-Snapline-Designer-Funktionalität benötigt wird.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function AdditionalSnapBaselineOffset() As Integer Implements IRequestAdditionalSnapBaselineOffset.AdditionalSnapBaselineOffset
        Return 1
    End Function

#End Region

    ''' <summary>
    ''' Bestimmt oder ermittelt eine eindeutige GUID für das Steuerelement, um beispielsweise Rechte-Mappings in Datenbanken aufzubauen.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>Dass diese Eigenschaft UIGuid und nicht PermissionGuid lautet, hat historische Gründe. Diese Eigenschaft implementiert 
    ''' IPermissionManageableUIContentElement.PermissionGuid.</remarks>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
     Description("Bestimmt oder ermittelt eine eindeutige GUID für das Steuerelement, um beispielsweise Rechte-Mappings in Datenbanken aufzubauen."),
     Category("Sicherheit"),
     EditorBrowsable(EditorBrowsableState.Always),
     Browsable(True)>
    Public Property UIGuid As System.Guid Implements IPermissionManageableUIContentElement.IdentificationGuid

    ''' <summary>
    ''' Bestimmt oder ermittelt, in welcher Form eine Komponente auf Grund von Rechten oder Lizenzausbaustufen verwendet werden darf.
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
    ''' Definiert, um welche Art Element es sich bei dieser Komponente handelt.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property ElementType As PermissionManageableUIElementType Implements IPermissionManageableUIContentElement.ElementType
        Get
            Return PermissionManageableUIElementType.Content
        End Get
    End Property

    ''' <summary>
    ''' Bestimmt oder ermittelt, ob dieses Steuerelement rechtemäßig von einem externen Controler verwaltet werden kann oder nicht.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
     Description("Bestimmt oder ermittelt, ob dieses Steuerelement rechtemäßig von einem externen Controler verwaltet werden kann oder nicht."),
     Category("Sicherheit"),
     EditorBrowsable(EditorBrowsableState.Always),
     Browsable(True), DefaultValue(False)>
    Public Property IsManageable As Boolean Implements IPermissionManageableComponent.IsManageable
        Get
            Return myIsManageable
        End Get
        Set(ByVal value As Boolean)
            myIsManageable = value
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
     Category("Verhalten"),
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
    ''' Bestimmt oder ermittelt einen Gruppierungsnamen, um eine Möglichkeit zur Verfügung zu stellen, zentral eine Reihe von Steuerelementen zu steuern.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
     Description("Bestimmt oder ermittelt einen Gruppierungsnamen, um eine Möglichkeit zur Verfügung zu stellen, zentral eine Reihe von Steuerelementen zu steuern."),
     Category("Verhalten"),
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

End Class
