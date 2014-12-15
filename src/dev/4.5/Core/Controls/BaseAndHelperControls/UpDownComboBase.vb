Imports System.Windows.Forms
Imports System.Drawing
Imports System.Windows.Forms.VisualStyles
Imports System.Windows.Forms.VisualStyles.VisualStyleElement
Imports System.Windows.Forms.VisualStyles.VisualStyleElement.Page
Imports System.ComponentModel

Public MustInherit Class MultiPurposeButtonBase
    Inherits Control

    Private myCaptured As UpDownComboButtonIDs
    Private myDoubleClickFired As Boolean
    Private myMouseOver As UpDownComboButtonIDs
    Private myPushed As UpDownComboButtonIDs
    Private myTimer As Timer
    Private myTimerInterval As Integer
    Private myButtonType As UpDownComboButtonType
    Private myButtonBehaviour As ButtonBehaviour
    Private myPressedState As Boolean
    Private myPressedStateChangedByCode As Boolean

    Public Event UpDownCombo(ByVal sender As Object, ByVal e As UpDownComboEventArgs)
    Public Event QueryValidationCancelled(ByVal sender As Object, ByVal e As CancelEventArgs)
    Public Event RequestFocus(ByVal sender As Object, ByVal e As RequestFocusEventArgs)
    Public Event PressedStateChanged(ByVal sender As Object, ByVal e As PressedStateChangedEventArgs)
    Public Event SimpleButtonClicked(sender As Object, e As EventArgs)

    Public Sub New()
        MyBase.SetStyle((ControlStyles.FixedHeight Or (ControlStyles.FixedWidth Or ControlStyles.Opaque)), True)
        MyBase.SetStyle(ControlStyles.Selectable, False)
        MyBase.SetStyle(ControlStyles.ResizeRedraw, True)

        InitializeProperties()
    End Sub

    Public Property PressedState As Boolean
        Get
            Return myPressedState
        End Get
        Set(ByVal value As Boolean)
            If value <> myPressedState Then
                myPressedState = value
                OnPressedStateChange(New PressedStateChangedEventArgs(myPressedStateChangedByCode))
                myPressedStateChangedByCode = False
            End If
        End Set
    End Property

    Protected Overridable Sub OnPressedStateChange(ByVal e As PressedStateChangedEventArgs)
        RaiseEvent PressedStateChanged(Me, e)
    End Sub

    Protected Overridable Sub InitializeProperties()
        myButtonType = GetButtonType()
    End Sub

    Protected MustOverride Function GetButtonType() As UpDownComboButtonType

    Protected Overrides Sub OnMouseDown(ByVal e As MouseEventArgs)
        Dim ce As New CancelEventArgs
        OnQueryValidationCancelled(ce)

        'Wir versuchen, zu fokussieren. Wenn das fehl schlägt,
        'wird der Rest der MouseDown-Behandlung gar nicht erst mehr ausgeführt.
        Dim rfe As New RequestFocusEventArgs
        OnRequestFocus(rfe)
        If Not rfe.Succeeded Then
            MyBase.OnMouseDown(e)
            Return
        End If

        If ((e.Clicks = 2) AndAlso (e.Button = MouseButtons.Left)) Then
            Me.myDoubleClickFired = True
            Return
        End If
        If (Not ce.Cancel AndAlso (e.Button = MouseButtons.Left)) Then
            Me.BeginButtonPress(e)
        End If
        MyBase.OnMouseDown(e)
    End Sub

    Private Sub BeginButtonPress(ByVal e As MouseEventArgs)
        If myButtonType = UpDownComboButtonType.SpinUpDownCombined Then
            Dim num As Integer = (MyBase.Size.Height \ 2)
            If (e.Y < num) Then
                Me.myPushed = UpDownComboButtonIDs.Up
                Me.myCaptured = Me.myPushed
                MyBase.Invalidate()
            Else
                Me.myPushed = UpDownComboButtonIDs.Down
                Me.myCaptured = Me.myPushed
                MyBase.Invalidate()
            End If
            MyBase.Capture = True
            Me.OnUpDownCombo(New UpDownComboEventArgs(myPushed))
            Me.StartTimer()
        Else
            Me.myPushed = UpDownComboButtonIDs.Combined
            PressedState = Not PressedState
            Me.myCaptured = Me.myPushed
            MyBase.Capture = True
            MyBase.Invalidate()
            Me.OnUpDownCombo(New UpDownComboEventArgs(myPushed))
        End If
    End Sub

    Private Sub EndButtonPress()
        If Me.myButtonType = UpDownComboButtonType.SimpleButton Then
            PressedState = Not PressedState
        End If
        Me.myPushed = UpDownComboButtonIDs.None
        Me.myCaptured = UpDownComboButtonIDs.None
        If myButtonType = UpDownComboButtonType.SpinUpDownCombined Then
            Me.StopTimer()
        End If
        MyBase.Capture = False
        MyBase.Invalidate()
        If Me.myButtonType = UpDownComboButtonType.SimpleButton Then
            OnSimpleButtonClicked(EventArgs.Empty)
        End If

    End Sub

    Protected Overridable Sub OnSimpleButtonClicked(e As EventArgs)
        RaiseEvent SimpleButtonClicked(Me, e)
    End Sub

    Protected Overridable Sub OnRequestFocus(ByVal e As RequestFocusEventArgs)
        RaiseEvent RequestFocus(Me, e)
    End Sub

    Protected Overrides Sub OnMouseLeave(ByVal e As EventArgs)
        Me.myMouseOver = UpDownComboButtonIDs.None
        MyBase.Invalidate()
        MyBase.OnMouseLeave(e)
    End Sub

    Protected Overrides Sub OnMouseMove(ByVal e As MouseEventArgs)
        If myButtonType = UpDownComboButtonType.SpinUpDownCombined Then
            OnSpinUpDownMouseMove(e)
        Else
            OnSingleMouseMove(e)
        End If
    End Sub

    Private Sub OnSpinUpDownMouseMove(ByVal e As MouseEventArgs)
        If MyBase.Capture Then
            Dim rectangle As Rectangle = MyBase.ClientRectangle

            rectangle.Height = (rectangle.Height \ 2)
            If (Me.myCaptured = UpDownComboButtonIDs.Down) Then
                rectangle.Y = (rectangle.Y + rectangle.Height)
            End If
            If rectangle.Contains(e.X, e.Y) Then
                If (Me.myPushed <> Me.myCaptured) Then
                    Me.StartTimer()
                    Me.myPushed = Me.myCaptured
                    MyBase.Invalidate()
                End If
            ElseIf (Me.myPushed <> UpDownComboButtonIDs.None) Then
                Me.StopTimer()
                Me.myPushed = UpDownComboButtonIDs.None
                MyBase.Invalidate()
            End If
        End If
        Dim clientRectangle As Rectangle = MyBase.ClientRectangle
        Dim rectangle3 As Rectangle = MyBase.ClientRectangle
        clientRectangle.Height = (clientRectangle.Height \ 2)
        rectangle3.Y = (rectangle3.Y + (rectangle3.Height \ 2))
        If clientRectangle.Contains(e.X, e.Y) Then
            Me.myMouseOver = UpDownComboButtonIDs.Up
            MyBase.Invalidate()
        ElseIf rectangle3.Contains(e.X, e.Y) Then
            Me.myMouseOver = UpDownComboButtonIDs.Down
            MyBase.Invalidate()
        End If
        MyBase.OnMouseMove(e)
    End Sub

    Private Sub OnSingleMouseMove(ByVal e As MouseEventArgs)
        Dim clientRectangle As Rectangle = MyBase.ClientRectangle
        If clientRectangle.Contains(e.X, e.Y) Then
            Me.myMouseOver = UpDownComboButtonIDs.Combined
            MyBase.Invalidate()
        End If
        MyBase.OnMouseMove(e)
    End Sub

    Protected Overridable Sub OnQueryValidationCancelled(ByVal e As CancelEventArgs)
        RaiseEvent QueryValidationCancelled(Me, e)
    End Sub

    Public ReadOnly Property MouseOverButton As Boolean
        Get
            Return Me.ClientRectangle.Contains(Me.PointToClient(MousePosition))
        End Get
    End Property

    Protected Overrides Sub OnMouseUp(ByVal e As MouseEventArgs)
        Dim ce As New CancelEventArgs
        OnQueryValidationCancelled(ce)
        If (Not ce.Cancel AndAlso (e.Button = MouseButtons.Left)) Then
            Me.EndButtonPress()
        End If
        Dim p As New Point(e.X, e.Y)
        p = MyBase.PointToScreen(p)

        If (e.Button = MouseButtons.Left) Then
            If Not ce.Cancel Then
                If Me.myDoubleClickFired Then
                    Me.myDoubleClickFired = False
                End If
            End If
            Me.myDoubleClickFired = False
        End If
        MyBase.OnMouseUp(e)
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        If myButtonType = UpDownComboButtonType.SpinUpDownCombined Then
            PaintSpinButton(e)
        ElseIf myButtonType = UpDownComboButtonType.Combo Then
            PaintComboButton(e)
        Else
            PaintSimpleButton(e)
        End If
        MyBase.OnPaint(e)
    End Sub

    Private Sub PaintSpinButton(ByVal e As PaintEventArgs)
        Dim height As Integer = CInt(Math.Round(MyBase.ClientSize.Height / 2)) + 1
        If Application.RenderWithVisualStyles Then
            Dim renderer As New VisualStyleRenderer(If((Me.myMouseOver = UpDownComboButtonIDs.Up), Spin.Up.Hot, Spin.Up.Normal))
            If Not MyBase.Enabled Then
                renderer.SetParameters(Spin.Up.Disabled)
            ElseIf (Me.myPushed = UpDownComboButtonIDs.Up) Then
                renderer.SetParameters(Spin.Up.Pressed)
            End If
            renderer.DrawBackground(e.Graphics, New Rectangle(0, 0, &H10, height))
            If Not MyBase.Enabled Then
                renderer.SetParameters(Spin.Down.Disabled)
            ElseIf (Me.myPushed = UpDownComboButtonIDs.Down) Then
                renderer.SetParameters(Spin.Down.Pressed)
            Else
                renderer.SetParameters(If((Me.myMouseOver = UpDownComboButtonIDs.Down), Spin.Down.Hot, Spin.Down.Normal))
            End If
            renderer.DrawBackground(e.Graphics, New Rectangle(0, height, &H10, height))
        Else
            ControlPaint.DrawScrollButton(e.Graphics,
                                          New Rectangle(0, 0, &H10, height), ScrollButton.Up,
                                            If((Me.myPushed = UpDownComboButtonIDs.Up), ButtonState.Pushed,
                                                If(MyBase.Enabled, ButtonState.Normal, ButtonState.Inactive)))
            ControlPaint.DrawScrollButton(e.Graphics,
                                          New Rectangle(0, height, &H10, height), ScrollButton.Down,
                                            If((Me.myPushed = UpDownComboButtonIDs.Down), ButtonState.Pushed,
                                                If(MyBase.Enabled, ButtonState.Normal, ButtonState.Inactive)))
        End If

        If (height <> ((MyBase.ClientSize.Height + 1) / 2)) Then
            Using pen As Pen = New Pen(Me.Parent.BackColor)
                Dim clientRectangle As Rectangle = MyBase.ClientRectangle
                e.Graphics.DrawLine(pen, clientRectangle.Left, (clientRectangle.Bottom - 1), (clientRectangle.Right - 1), (clientRectangle.Bottom - 1))
            End Using
        End If
    End Sub

    Private Sub PaintComboButton(ByVal e As PaintEventArgs)
        Dim height As Integer = MyBase.ClientSize.Height
        If Application.RenderWithVisualStyles Then
            Dim renderer As New VisualStyleRenderer(If((Me.myMouseOver = UpDownComboButtonIDs.Combined),
                                                       VisualStyleElement.ComboBox.DropDownButton.Hot,
                                                       VisualStyleElement.ComboBox.DropDownButton.Normal))
            If Not MyBase.Enabled Then
                renderer.SetParameters(VisualStyleElement.ComboBox.DropDownButton.Disabled)
            ElseIf (Me.myPushed = UpDownComboButtonIDs.Combined Or PressedState) Then
                renderer.SetParameters(VisualStyleElement.ComboBox.DropDownButton.Pressed)
            End If
            renderer.DrawBackground(e.Graphics, New Rectangle(0, 0, &H10, height))
        Else
            ControlPaint.DrawComboButton(e.Graphics,
                                          New Rectangle(0, 0, &H10, height),
                                            If((Me.myPushed = UpDownComboButtonIDs.Combined Or PressedState), ButtonState.Pushed,
                                                If(MyBase.Enabled, ButtonState.Normal, ButtonState.Inactive)))
        End If
    End Sub

    Private Sub PaintSimpleButton(e As PaintEventArgs)
        Dim height As Integer = MyBase.ClientSize.Height
        ControlPaint.DrawButton(e.Graphics,
                                          New Rectangle(0, 0, &H10, height),
                                            If((Me.myPushed = UpDownComboButtonIDs.Combined Or PressedState), ButtonState.Pushed,
                                                If(MyBase.Enabled, ButtonState.Normal, ButtonState.Inactive)))

        If Me.ButtonImage IsNot Nothing Then
            DrawButtonImage(e.Graphics, Me.ButtonImage)
        End If
    End Sub

    Protected Overridable Sub DrawButtonImage(g As Graphics, Image As Image)
        Dim height As Integer = MyBase.ClientSize.Height
        If MyBase.Enabled Then
            g.DrawImage(Image, New Rectangle(1, 1, 14, height - 2))
        Else
            ControlPaint.DrawImageDisabled(g, Image, 1, 1, Color.Transparent)
        End If
    End Sub

    Protected Overridable Sub OnUpDownCombo(ByVal upevent As UpDownComboEventArgs)
        RaiseEvent UpDownCombo(Me, upevent)
    End Sub

    Protected Sub StartTimer()
        If (Me.myTimer Is Nothing) Then
            Me.myTimer = New Timer
            AddHandler myTimer.Tick, New EventHandler(AddressOf Me.TimerHandler)
        End If
        Me.myTimerInterval = 500
        Me.myTimer.Interval = Me.myTimerInterval
        Me.myTimer.Start()
    End Sub

    Protected Sub StopTimer()
        If (Not Me.myTimer Is Nothing) Then
            Me.myTimer.Stop()
            Me.myTimer.Dispose()
            Me.myTimer = Nothing
        End If
    End Sub

    Private Sub TimerHandler(ByVal source As Object, ByVal args As EventArgs)
        If Not MyBase.Capture Then
            Me.EndButtonPress()
        Else
            Me.OnUpDownCombo(New UpDownComboEventArgs(myPushed))
            Me.myTimerInterval = (Me.myTimerInterval * 7)
            Me.myTimerInterval = (Me.myTimerInterval \ 10)
            If (Me.myTimerInterval < 1) Then
                Me.myTimerInterval = 1
            End If
            Me.myTimer.Interval = Me.myTimerInterval
        End If
    End Sub

    Private Function IsComboOrSimpleButton() As Boolean
        If (myButtonType And UpDownComboButtonType.Combo) = UpDownComboButtonType.Combo Then
            Return True
        End If

        If (myButtonType And UpDownComboButtonType.SimpleButton) = UpDownComboButtonType.SimpleButton Then
            Return True
        End If
        Return False
    End Function

    Public Sub SnapInButton()
        myPressedStateChangedByCode = True
        PressedState = True
        Me.Invalidate()
    End Sub

    Public Sub SnapOutButton()
        myPressedStateChangedByCode = True
        PressedState = False
        Me.Invalidate()
    End Sub

    Property ButtonBehaviour As ButtonBehaviour
    Property ButtonImage As Image
End Class

Public Class UpDownComboEventArgs
    Inherits EventArgs

    Sub New(ByVal UpDownComboButtonID As UpDownComboButtonIDs)
        Me.UpDownComboButtonID = UpDownComboButtonID
    End Sub

    Property UpDownComboButtonID As UpDownComboButtonIDs
End Class

Public Enum UpDownComboButtonType
    Combo
    SpinUpDownCombined
    SimpleButton
End Enum

Public Enum UpDownComboButtonIDs As Integer
    None = 0
    Up = 1
    Down = 2
    Combined = 4
End Enum

Public Enum ButtonBehaviour
    Standard
    Radio
End Enum




