Imports System.Drawing
Imports System.Windows.Forms
Imports System.ComponentModel
Imports System.Threading.Tasks

<Designer("ActiveDevelop.EntitiesFormsLib.TextBoxBasedControlDesigner"), ToolboxItem(False)>
Public Class TextBoxPopup
    Inherits TextBoxButtonBase(Of ComboButton)

    Private myUndoText As String
    Private myMinimumPopupSize As Drawing.Size
    Private myCommited As Boolean
    Private myClosePopupInternallyInProgress As Boolean

    Public Event PopupCreated(ByVal sender As Object, ByVal e As EventArgs)
    Public Event PopupOpening(ByVal sender As Object, ByVal e As PopupOpeningEventArgs)
    Public Event PopupOpened(ByVal sender As Object, ByVal e As EventArgs)
    Public Event PopupClosing(ByVal sender As Object, ByVal e As PopupClosingEventArgs)
    Public Event PopupClosed(ByVal sender As Object, ByVal e As EventArgs)
    Public Event BeginOpenPopup(sender As Object, e As EventArgs)

    Private myPopupControl As ResizablePopup
    Private myIsPopupOpen As Boolean
    Private myIsPopupResizable As Boolean
    Private myIsOpening As Boolean
    Private myLastPopupSize As Drawing.Size

    Private Shared DEFAULTMAXHEIGHT As Integer = 400
    Private Shared DEFAULTMINHEIGHT As Integer = 80
    Private Shared DEFAULTMAXWIDTH As Integer = 640
    Private Shared DEFAULTMINWIDTH As Integer = 50
    Private Shared DEFAULTWIDTH As Integer = 200
    Private Shared DEFAULTHEIGHT As Integer = 100

    Sub New()
        MyBase.New()

        MyBase.SetButtonBehaviour(ButtonBehaviour.Radio)
        Me.FocusSelectionBehaviour = GetDefaultFocusSelectionBehaviour()

        'Key-Ereignisse aus der TextBox hochbubbeln lassen.
        AddHandler Me.TextBoxPart.KeyPress, Sub(sender As Object, e As KeyPressEventArgs)
                                                OnKeyPress(e)
                                            End Sub

        AddHandler Me.TextBoxPart.KeyDown, Sub(sender As Object, e As KeyEventArgs)
                                               OnKeyDown(e)
                                           End Sub

        AddHandler Me.TextBoxPart.KeyUp, Sub(sender As Object, e As KeyEventArgs)
                                             OnKeyUp(e)
                                         End Sub

        AddHandler Me.TextBoxPart.TextChanged,
            Sub(sender As Object, e As EventArgs)
                myCommited = False
                OnTextChanged(e)
            End Sub

        AddHandler Me.TextBoxPart.Click, Sub(sender As Object, e As EventArgs)
                                             OnClick(e)
                                         End Sub

        AddHandler Me.TextBoxPart.DoubleClick, Sub(sender As Object, e As EventArgs)
                                                   OnDoubleClick(e)
                                               End Sub

        AddHandler Me.TextBoxPart.MouseDoubleClick, Sub(sender As Object, e As MouseEventArgs)
                                                        OnMouseDoubleClick(e)
                                                    End Sub

    End Sub

    Protected Overridable Sub OnPopupOpening(ByVal e As PopupOpeningEventArgs)
        RaiseEvent PopupOpening(Me, e)
    End Sub

    Protected Overrides Function GetAdditionalButton(buttonCount As Integer) As Tuple(Of MultiPurposeButtonBase, Boolean)
        Return MyBase.GetAdditionalButton(buttonCount)
    End Function

    Protected Overridable Sub OnPopupOpened(ByVal e As EventArgs)
        RaiseEvent PopupOpened(Me, e)
    End Sub

    Protected Overridable Sub OnPopupClosing(ByVal e As PopupClosingEventArgs)
        RaiseEvent PopupClosing(Me, e)
    End Sub

    Protected Overridable Sub OnPopupClosed(ByVal e As EventArgs)
        RaiseEvent PopupClosed(Me, e)
    End Sub

    Private Sub PopupClosedHandler(ByVal sender As Object,
                               ByVal e As ToolStripDropDownClosedEventArgs)
        IsPopupOpen = False
        If Not (e.CloseReason = PopupClosingReason.AppClicked And
                MouseOverButton) Then
            Me.SnapOutButton()
        End If
        OnPopupClosed(New PopupClosingEventArgs(CType(e.CloseReason, PopupClosingReason)))
    End Sub

    Private Sub CreatePopupControlOnDemand()

        Dim tmpPopupContent = GetPopupContent()
        If Me.IsPopupAutoSize Then
            Me.PopupSize = tmpPopupContent.Size
        End If

        myPopupControl = New ResizablePopup() With
            {.MinimumSize = MinimumPopupSize,
             .MaximumSize = MaximumPopupSize,
             .ShowStatusBar = False,
             .IsResizable = IsPopupResizable,
             .Size = PopupSize,
             .PopupContentControl = tmpPopupContent
            }

        System.Windows.WeakEventManager(Of ResizablePopup, EventArgs).AddHandler(
                myPopupControl, "SizeChanged", AddressOf PopupSizedChangedHandler)

        System.Windows.WeakEventManager(Of ResizablePopup, PopupOpeningEventArgs).AddHandler(
            Me.PopupControl, "PopupOpening", Sub(sender As Object, e As PopupOpeningEventArgs)
                                                 If e.Cancel Then
                                                     OnPopupOpening(e)
                                                     Return
                                                 End If

                                                 myIsOpening = True
                                                 OnPopupOpening(e)
                                             End Sub)
        System.Windows.WeakEventManager(Of ResizablePopup, EventArgs).AddHandler(
            Me.PopupControl, "PopupOpened", Sub(sender As Object, e As EventArgs)
                                                myIsOpening = False
                                                OnPopupOpened(e)
                                            End Sub)

        System.Windows.WeakEventManager(Of ResizablePopup, PopupClosingEventArgs).AddHandler(
            Me.PopupControl, "PopupClosing", Sub(sender As Object, e As PopupClosingEventArgs)
                                                 OnPopupClosing(e)
                                                 ClosePopupInternally(e)
                                             End Sub)

        System.Windows.WeakEventManager(Of ResizablePopup, EventArgs).AddHandler(
            Me.PopupControl, "PopupClosed", Sub(sender As Object, e As EventArgs)
                                                OnPopupClosed(e)
                                            End Sub)

        System.Windows.WeakEventManager(Of ResizablePopup, PopupCloseRequestedEventArgs).AddHandler(
            Me.PopupControl, "PopupCloseRequested",
            Sub(sender As Object, e As PopupCloseRequestedEventArgs)
#If DEBUG Then
                TraceEx.TraceInformation("TRACING: TextBoxPopup. PopupControl.PopupCloseRequested handler.")
#End If

                Dim e2 As New PopupClosingEventArgs(PopupClosingReason.Keyboard,
                                                    e.KeyCode)

                If Not e2.Cancel Then
                    ClosePopupInternally(e2)
                End If
            End Sub)

    End Sub

    Protected Sub DestroyPopup()
        If myPopupControl IsNot Nothing Then
            myPopupControl.PopupContentControl.Dispose()
            myPopupControl.PopupContentControl = Nothing
            myPopupControl.Dispose()
            myPopupControl = Nothing
        End If
    End Sub

    Protected Overrides Sub OnGotFocus(ByVal e As System.EventArgs)
        MyBase.OnGotFocus(e)
        If FocusSelectionBehaviour = FocusSelectionBehaviours.PreSelectInput Then
            Me.TextBoxPart.SelectAll()
        ElseIf FocusSelectionBehaviour = FocusSelectionBehaviours.PlaceCaretAtEnd Then
            Me.TextBoxPart.SelectionStart = Me.Text.ToString.Length
            Me.TextBoxPart.SelectionLength = 0
        Else
            Me.TextBoxPart.SelectionStart = 0
            Me.TextBoxPart.SelectionLength = 0
        End If
    End Sub

    Protected Overrides Sub OnEnter(ByVal e As System.EventArgs)
        MyBase.OnEnter(e)
    End Sub

    Protected Overrides Sub OnLeave(e As System.EventArgs)
        MyBase.OnLeave(e)
        CommitOnLeave()
    End Sub

    Protected Overridable Sub CommitOnLeave()
        If Not myCommited Then
            Commit(True)
        End If
    End Sub

    Protected Overridable Function GetPopupContent() As Control
        Return New SimplePopupContent
    End Function

    Protected Overrides Sub OnButtonAction(ByVal e As ButtonActionEventArgs)
        MyBase.OnButtonAction(e)
    End Sub

    Protected Overrides Sub OnButtonPressedStateChange(ByVal e As PressedStateChangedEventArgs)
        MyBase.OnButtonPressedStateChange(e)
        If Me.ButtonPressedState Then

            'Im Design-Modus sollten wir ein Popup niemals öffnen.
            If Me.DesignMode Then Return
            OnBeginOpenPopup(EventArgs.Empty)
        Else
            If Me.PopupControl.IsOpen Then
                If e.PressedStateChangedReason = PressedStateChangedReason.PropertySetter Then
                    Return
                Else
                    ClosePopup(New PopupClosingEventArgs(PopupClosingReason.PopupOpenerClicked))
                End If
            End If
        End If
    End Sub

    ''' <summary>
    ''' Löst das BeforePopupOpen-Ereignis aus, das benötigt wird, um Dinge zu behandeln, kurz bevor das Öffnen des Popups beginnt.
    ''' </summary>
    ''' <param name="e"></param>
    ''' <remarks>Gibt überschreibenden Steuerelementen die Möglichkeit, beispielsweise ein IsDirty-Handling für das Formular zu implementieren, 
    ''' da das Öffnen des Popups unter Umständen mit werteverändernden Ereignissen innerhalb des Popup-Controls einher geht.</remarks>
    Protected Overridable Sub OnBeginOpenPopup(e As EventArgs)
        OpenPopupInternally()
        RaiseEvent BeginOpenPopup(Me, e)
    End Sub

    Private Sub OpenPopupInternally()
        If myPopupControl Is Nothing Then
            CreatePopupControlOnDemand()
            OnPopupCreated()
        End If

        myPopupControl.MaximumSize = Me.MaximumPopupSize
        myPopupControl.MinimumSize = Me.MinimumPopupSize
        myPopupControl.Margin = Padding.Empty
        myPopupControl.Padding = Padding.Empty
        myPopupControl.OpenPopup(Me)
        IsPopupOpen = myPopupControl.IsOpen
    End Sub

    Protected Overridable Sub OnPopupCreated()
        RaiseEvent PopupCreated(Me, EventArgs.Empty)
    End Sub

    Public Sub OpenPopup()
        If Not IsPopupOpen Then
            OnBeginOpenPopup(EventArgs.Empty)
        End If
    End Sub

    Public Sub ClosePopup()
        ClosePopupInternally(New PopupClosingEventArgs(PopupClosingReason.CloseCalled))
    End Sub

    Public Sub ClosePopup(cEventArgs As PopupClosingEventArgs)
        ClosePopupInternally(cEventArgs)
    End Sub

    Public Overridable Sub Undo()
        Me.Text = myUndoText
    End Sub

    Public Overridable Sub Commit(lateCommit As Boolean)
        myUndoText = Me.Text
        myCommited = True
    End Sub

    Protected Overridable Sub ClosePopupInternally(ByVal ClosingReason As PopupClosingEventArgs)

        If Not IsPopupOpen Then
#If DEBUG Then
            TraceEx.TraceInformation("TRACING: ClosePopupInternally. Canceled. Popup is closed or has just been closed.")
#End If
            Return
        End If

#If DEBUG Then
        TraceEx.TraceInformation("TRACING: ClosePopupInternally. ClosingReason:" & ClosingReason.PopupCloseReason.ToString & "; KeysData" & ClosingReason.KeyData.ToString)
#End If
        Try
            'Rekursionen vermeiden.
            If myClosePopupInternallyInProgress Then Return
            myClosePopupInternallyInProgress = True
            If IsPopupOpen Then
#If DEBUG Then
                TraceEx.TraceInformation("TRACING: ClosePopupInternally. Canceled. Popup is closed or has just been closed.")
#End If
                myPopupControl.ClosePopupInternally(ClosingReason)
            End If

            DecideUndoCommit(ClosingReason)

        Finally
            myClosePopupInternallyInProgress = False
            Me.SnapOutButton()
            IsPopupOpen = False

#If DEBUG Then
            TraceEx.TraceInformation("TRACING: ClosePopupInternally. Snapping out Button. PopupOpen is false.")
#End If
        End Try


    End Sub

    Protected Overridable Sub DecideUndoCommit(closingReason As PopupClosingEventArgs)
        If (closingReason.PopupCloseReason = PopupClosingReason.Keyboard And closingReason.KeyData = Keys.Escape) OrElse
    closingReason.PopupCloseReason = PopupClosingReason.PopupOpenerClicked Then
#If DEBUG Then
            TraceEx.TraceInformation("TRACING: ClosePopupInternally. Undoing input.")
#End If
            Me.Undo()
        Else
#If DEBUG Then
            TraceEx.TraceInformation("TRACING: ClosePopupInternally. Commiting input.")
#End If
            Me.Commit(False)
        End If
    End Sub

    Protected Overrides Sub OnKeyDown(ByVal e As System.Windows.Forms.KeyEventArgs)
        MyBase.OnKeyDown(e)

        If e.KeyData = Keys.Down Then
            If Not IsPopupOpen Then
                OpenPopup()
                If IsPopupOpen Then
                    e.Handled = True
                End If
            End If
        End If
    End Sub

    Private Sub PopupSizedChangedHandler(ByVal sender As Object,
                               ByVal e As EventArgs)
        myLastPopupSize = myPopupControl.Size
    End Sub

    Protected ReadOnly Property HostingControl As Control
        Get
            If myPopupControl Is Nothing Then
                CreatePopupControlOnDemand()
            End If
            Return myPopupControl.PopupContentControl
        End Get
    End Property

    <Browsable(False)>
    Protected ReadOnly Property PopupControl As ResizablePopup
        Get
            Return myPopupControl
        End Get
    End Property

    <Browsable(False)>
    Public Property IsPopupOpen As Boolean
        Get
            Return myIsPopupOpen
        End Get
        Private Set(ByVal value As Boolean)
            myIsPopupOpen = value
        End Set
    End Property

    <DefaultValue(False)>
    Public Overridable Property IsPopupResizable As Boolean
        Get
            Return myIsPopupResizable
        End Get
        Set(ByVal value As Boolean)
            If myIsPopupResizable <> value Then
                myIsPopupResizable = value
                If Me.PopupControl IsNot Nothing Then
                    Me.PopupControl.IsResizable = value
                End If
            End If
        End Set
    End Property

    <Browsable(False)>
    Public ReadOnly Property IsOpening As Boolean
        Get
            Return myIsOpening
        End Get
    End Property

    <Browsable(False)>
    Public ReadOnly Property LastPopupSize As Size
        Get
            Return myLastPopupSize
        End Get
    End Property

    <Browsable(True)>
    Public Overrides Property Text As String
        Get
            Return Me.TextBoxPart.Text
        End Get
        Set(ByVal value As String)
            Me.TextBoxPart.Text = value
            myUndoText = value
        End Set
    End Property

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

    Public Function ShouldSerializeFocusSelectionBehaviour() As Boolean
        Return Not (Me.FocusSelectionBehaviour = GetDefaultFocusSelectionBehaviour())
    End Function

    Public Sub ResetFocusSelectionBehaviour()
        Me.FocusSelectionBehaviour = GetDefaultFocusSelectionBehaviour()
    End Sub

    Protected Overridable Function GetDefaultFocusSelectionBehaviour() As FocusSelectionBehaviours
        Return FocusSelectionBehaviours.PreSelectInput
    End Function

    'Standardwertvorgaben für Popup-Größe
    Public Shared Property DefaultMinimumPopupSize As Size = New Size(DEFAULTMINWIDTH, DEFAULTMINHEIGHT)
    Public Shared Property DefaultMaximumPopupSize As Size = New Size(DEFAULTMAXWIDTH, DEFAULTMAXHEIGHT)
    Public Shared Property DefaultPopupSize As Size = New Size(DEFAULTWIDTH, DEFAULTHEIGHT)
    Public Shared Property DefaultFocusSelectionBehaviour As FocusSelectionBehaviours = FocusSelectionBehaviours.PreSelectInput

    'Eigenschaften
    Public Overridable Property MinimumPopupSize As Size = DefaultMinimumPopupSize

    Friend Function ShouldSerializeMinimumPopupSize() As Boolean
        If MinimumPopupSize = DefaultMinimumPopupSize Then
            Return False
        Else
            Return True
        End If
    End Function

    Friend Sub ResetMinimumPopupSize()
        Me.MinimumPopupSize = DefaultMinimumPopupSize
    End Sub

    Public Property MaximumPopupSize As Size = DefaultMaximumPopupSize

    Friend Function ShouldSerializeMaximumPopupSize() As Boolean
        If MaximumPopupSize = DefaultMaximumPopupSize Then
            Return False
        Else
            Return True
        End If
    End Function

    Friend Sub ResetMaximumPopupSize()
        Me.MaximumPopupSize = DefaultMaximumPopupSize
    End Sub

    Public Property PopupSize As Size = DefaultPopupSize

    <DefaultValue(True)>
    Public Property IsPopupAutoSize As Boolean = True

    Friend Function ShouldSerializePopupSize() As Boolean
        If PopupSize = DefaultPopupSize Then
            Return False
        Else
            Return True
        End If
    End Function

    Friend Sub ResetPopupSize()
        Me.PopupSize = DefaultPopupSize
    End Sub
End Class

