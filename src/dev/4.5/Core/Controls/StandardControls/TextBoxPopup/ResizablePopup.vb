Imports System.Windows.Forms
Imports System.Drawing
Imports System.Runtime.InteropServices

Imports System.Windows.Forms.VisualStyles
Imports System.Windows.Forms.VisualStyles.VisualStyleElement
Imports System.Windows.Forms.VisualStyles.VisualStyleElement.Page
Imports System.ComponentModel

''' <summary>
''' Infrastruktur-Klasse. Support-Control für alle auf <see cref="TextBoxPopup">TextBoxPopup</see> basierenden Steuerelemente.
''' </summary>
<ToolboxItem(False)>
Public Class ResizablePopup
    Inherits UserControl
    Implements IMessageFilter

    Private Const WM_NCHITTEST = &H84
    Private Const WM_GETMINMAXINFO As Integer = &H24

    Private Const WS_EX_TOOLWINDOW = &H80
    Private Const WS_EX_NOACTIVATE As Integer = &H8000000

    Private Const HTBOTTOM = 15
    Private Const HTBOTTOMRIGHT = 17

    Private Const GRIPHEIGHT = 16
    Private Const STATUSBARHEIGHT = 22
    Private Const LEFTSTATUSTEXTOFFSET = 22

    Public Const WM_LBUTTONDOWN = &H201
    Public Const WM_NCLBUTTONDOWN = &HA1
    Public Const WM_KEYDOWN = &H100
    Public Const WM_TIMER = &H113

    Private myDeleteButtonImageLocationArea As Rectangle
    Private myIsVisible As Boolean
    Private myValueText As String

    ''' <summary>
    ''' Wird ausgelöst, wenn sich die ValueText-Eigenschaft ändert.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Public Event ValueTextChanged(sender As Object, e As EventArgs)

    ''' <summary>
    ''' Wird ausgelöst, wenn der Reset-Button (Null Value), geklickt wird.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Public Event ResetButtonClick(ByVal sender As Object, ByVal e As EventArgs)

    ''' <summary>
    ''' Wird ausgelöst, wenn der Aktions-Button geklickt wird.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Public Event ActionButtonClick(ByVal sender As Object, ByVal e As EventArgs)

    ''' <summary>
    ''' Wird ausgelöst, bevor das Popup geöffnet wird.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Public Event PopupOpening(ByVal sender As Object, ByVal e As PopupOpeningEventArgs)

    ''' <summary>
    ''' Wird ausgelöst, nachdem das Popup geöffnet wurde.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Public Event PopupOpened(ByVal sender As Object, ByVal e As EventArgs)

    ''' <summary>
    ''' Wird ausgelöst, nachdem das Popup geschlossen wurde.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Public Event PopupClosed(ByVal sender As Object, ByVal e As EventArgs)

    ''' <summary>
    ''' Wird ausgelöst, bevor das Popup geschlossen wird.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Public Event PopupClosing(ByVal sender As Object, ByVal e As PopupClosingEventArgs)

    'TODO: Requestreason einbauen, falls notwendig für die Unterscheidung.
    Public Event PopupCloseRequested(ByVal sender As Object, ByVal E As PopupCloseRequestedEventArgs)

    <DllImport("user32")>
    Public Shared Function SetParent(ByVal hWndChild As IntPtr, ByVal hWndNewParent As IntPtr) As Integer
    End Function

    <DllImport("user32.dll")> _
    Public Shared Function SetWindowLong( _
         ByVal hWnd As IntPtr, _
         ByVal nIndex As Integer, _
         ByVal dwNewLong As IntPtr) As Integer
    End Function

    <DllImport("user32.dll", CharSet:=CharSet.Auto, ExactSpelling:=True)> _
    Public Shared Function ShowWindow(ByVal hWnd As IntPtr, ByVal nCmdShow As Integer) As Boolean
    End Function

    Private Declare Function SetForegroundWindow Lib "User32.dll" (ByVal hWnd As IntPtr) As Boolean

    <DllImport("user32.dll")> _
    Public Shared Function GetParent(ByVal hwnd As IntPtr) As IntPtr
    End Function

    Private Structure POINTAPI
        Dim x As Integer
        Dim y As Integer
    End Structure

    Private Structure MINMAXINFO
        Dim ptReserved As POINTAPI
        Dim ptMaxSize As POINTAPI
        Dim ptMaxPosition As POINTAPI
        Dim ptMinTrackSize As POINTAPI
        Dim ptMaxTrackSize As POINTAPI
    End Structure

    Private myPopupContentControl As Control
    Private myIsResizable As Boolean
    Private myAutoSize As Boolean
    Private myOriginalHostingControlSize As Size
    Private myDeleteButtonImageLocation As Point
    Private myMouseOverDeleteButton As Boolean
    Private myRejectFocus As Boolean
    Private myLastShownReferringControl As Control

    Sub New()
        MyBase.New()

        'Nicht wundern: AutoSize ist geshadowed!
        MyBase.AutoSize = False
        Me.AutoSize = True

        Me.BackColor = Color.White
        Me.BorderStyle = Windows.Forms.BorderStyle.FixedSingle

        Me.SetStyle(ControlStyles.ResizeRedraw, True)
        Me.SetStyle(ControlStyles.OptimizedDoubleBuffer, True)
    End Sub

    Protected Overrides ReadOnly Property CreateParams As System.Windows.Forms.CreateParams
        Get
            Dim cp = MyBase.CreateParams
            cp.ExStyle = cp.ExStyle Or WS_EX_NOACTIVATE
            cp.Parent = IntPtr.Zero
            Return cp
        End Get
    End Property

    Public Sub OpenPopup(ByVal referringControl As Control)
        myLastShownReferringControl = referringControl
        Dim e As New PopupOpeningEventArgs(False, Size.Empty)
        OnPopupOpening(e)
        If e.Cancel Then Return
        Show(referringControl)
        OnPopupOpened(EventArgs.Empty)
    End Sub

    Public Sub ClosePopup()
        ClosePopupInternally(New PopupClosingEventArgs(PopupClosingReason.CloseCalled))
    End Sub

    Friend Sub ClosePopupInternally(ByVal ClosingReason As PopupClosingEventArgs)

        'Rekursion vermeiden
#If DEBUG Then
        TraceEx.TraceInformation("TRACING: ResizablePopup: ClosePopupInternally.")
#End If

        If ClosingReason.Caused = PopupCloseCause.ExternalByUser Then
            ClosingReason.Caused = PopupCloseCause.InternalByComponent
            OnPopupClosing(ClosingReason)
        End If

        If Not Me.IsOpen Then Return
        If Not ClosingReason.Cancel Then
            Hide()
            OnPopupClosed(EventArgs.Empty)
        End If
    End Sub

    Private Shadows Sub Show(ByVal referringControl As Control)
        '#If DEBUG Then
        '        Debug.Print(Me.ControlTypeAndNameString & " : Show (private)")
        '#End If

        Me.Location = ResizablePopup.GetPopupLocation(referringControl, Me.Size)
        SetParent(Me.Handle, IntPtr.Zero)
        ShowWindow(Me.Handle, 1)
        SetForegroundWindow(Me.Handle)
        Application.AddMessageFilter(Me)
        myIsVisible = True
    End Sub

    Private Shadows Sub Hide()
        If myIsVisible Then
            ShowWindow(Me.Handle, 0)
            myIsVisible = False
        End If
    End Sub

    Public Function PreFilterMessage(ByRef m As System.Windows.Forms.Message) As Boolean Implements System.Windows.Forms.IMessageFilter.PreFilterMessage

        If Me.IsDisposed Then Return False

        If m.Msg = &H118 Then
            Return False
        End If

        'Nix machen, wenn im Control geklickt wird und es schon auf war.
        If myLastShownReferringControl IsNot Nothing Then
            If (m.Msg = WM_LBUTTONDOWN) And IsHandleInGivenControlHierachy(myLastShownReferringControl, m.HWnd) Then
                If IsOpen Then
                    Return False
                End If
            End If
        End If

        'ESC für offenes Popup behandeln
        If m.Msg = WM_KEYDOWN And m.WParam.ToInt32 = &H1B And Me.IsOpen Then
            RaiseEvent PopupCloseRequested(Me, New PopupCloseRequestedEventArgs(PopupCloseRequestReasons.KeyboardCancel) With {.KeyCode = Keys.Escape})
            Return True
        End If

        'Return für offenes Popup behandeln
        If m.Msg = WM_KEYDOWN And m.WParam.ToInt32 = 13 And Me.IsOpen Then
            RaiseEvent PopupCloseRequested(Me, New PopupCloseRequestedEventArgs(PopupCloseRequestReasons.KeyboardCommit) With {.KeyCode = Keys.Return})
            Return True
        End If

        'TAB für offenes Popup behandeln
        If m.Msg = WM_KEYDOWN And m.WParam.ToInt32 = Keys.Tab And Me.IsOpen Then
#If DEBUG Then
            TraceEx.TraceInformation("TRACING: ResizingPopup. PreFilterMessage, TAB for WM_KEYDOWN with Open Popup detected.")
#End If
            RaiseEvent PopupCloseRequested(Me, New PopupCloseRequestedEventArgs(PopupCloseRequestReasons.KeyboardCommit) With {.KeyCode = Keys.Tab})
            Return False
        End If

        Dim pe As PopupClosingEventArgs = Nothing
        If (myIsVisible AndAlso (Form.ActiveForm Is Nothing OrElse Form.ActiveForm.Equals(Me))) Then
            pe = New PopupClosingEventArgs(PopupClosingReason.AppFocusChanged)
        End If

        If (m.Msg = WM_LBUTTONDOWN Or m.Msg = WM_NCLBUTTONDOWN) And Not IsHandleInThisControlHierachy(m.HWnd) Then
            pe = New PopupClosingEventArgs(PopupClosingReason.AppClicked)
        End If

        If pe IsNot Nothing Then
            If Me.IsOpen Then
                OnPopupClosing(pe)
#If DEBUG Then
                TraceEx.TraceInformation("TRACING: ResizingPopup. PreFilterMessage, Message lead to Popup Closing.")
#End If
            End If
        End If
        Return False
    End Function

    Public Function IsHandleInThisControlHierachy(ByVal hwnd As IntPtr) As Boolean
        Return IsHandleInGivenControlHierachy(Me, hwnd)
    End Function

    Public Function IsHandleInGivenControlHierachy(ByVal ctrl As Control, ByVal hwnd As IntPtr) As Boolean
        If ctrl Is Nothing Then Return False

        If hwnd = ctrl.Handle Then Return True

        Dim currentParentHwnd = hwnd
        Dim parentHwnd = GetParent(currentParentHwnd)
        If parentHwnd = IntPtr.Zero Then Return False

        Do
            parentHwnd = GetParent(currentParentHwnd)
            If parentHwnd = ctrl.Handle Then
                Return True
            ElseIf parentHwnd = IntPtr.Zero Then
                Return False
            Else
                currentParentHwnd = parentHwnd
            End If
        Loop
    End Function

    Private ReadOnly Property BottomGripArea As Rectangle
        Get
            Dim rect = ClientRectangle
            rect.Y = rect.Bottom - 4
            rect.Height = 4
            Return rect
        End Get
    End Property

    Private ReadOnly Property RightGripArea As Rectangle
        Get
            Dim rect = ClientRectangle
            rect.X = rect.Width - 10
            rect.Width = 10
            Return rect
        End Get
    End Property

    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        MyBase.OnPaint(e)
        If Me.IsResizable Then

            'Auf VS-Renderer testen
            If VisualStyleInformation.IsSupportedByOS AndAlso VisualStyleInformation.IsEnabledByUser Then
                Dim renderer As New VisualStyleRenderer(VisualStyleElement.Status.Pane.Normal)
                renderer.DrawBackground(e.Graphics, New Rectangle(
                                                0, Me.ClientSize.Height - GRIPHEIGHT,
                                                Me.ClientSize.Width, GRIPHEIGHT))

                renderer = New VisualStyleRenderer(VisualStyleElement.Status.Gripper.Normal)
                renderer.DrawBackground(e.Graphics, New Rectangle(
                                                Me.ClientSize.Width - GRIPHEIGHT, Me.ClientSize.Height - GRIPHEIGHT,
                                                GRIPHEIGHT, GRIPHEIGHT))
            Else
                ControlPaint.DrawSizeGrip(e.Graphics, Me.BackColor, Me.ClientSize.Width - GRIPHEIGHT, Me.ClientSize.Height - GRIPHEIGHT, GRIPHEIGHT, GRIPHEIGHT)
            End If

            If ShowStatusBar Then
                If myDeleteButtonImageLocation.IsEmpty Then
                    RecalcCurrentButtonPosition()
                End If
                If myMouseOverDeleteButton Then
                    e.Graphics.DrawImage(My.Resources._92_cancel_16_sel, myDeleteButtonImageLocation)
                Else
                    e.Graphics.DrawImage(My.Resources._92_cancel_16, myDeleteButtonImageLocation)
                End If
                'DrawString-Dimensionen ausrechnen.
                Dim drRec As New Rectangle(LEFTSTATUSTEXTOFFSET, Me.Height - STATUSBARHEIGHT + 3,
                                           Me.Width - GRIPHEIGHT - LEFTSTATUSTEXTOFFSET, STATUSBARHEIGHT - 3)

                Using sff As New StringFormat() With {
                                    .Trimming = StringTrimming.EllipsisCharacter,
                                    .LineAlignment = StringAlignment.Center,
                                    .Alignment = Me.ValueTextAlignment}

                    e.Graphics.DrawString(ValueText, Me.Font, New SolidBrush(Me.ForeColor), drRec, sff)
                End Using
            End If
        End If
    End Sub

    Protected Overrides Sub OnMouseMove(ByVal mea As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseMove(mea)

        If myDeleteButtonImageLocationArea.Contains(mea.Location) Then
            If Not myMouseOverDeleteButton Then
                myMouseOverDeleteButton = True
                Me.Invalidate()
            End If
        Else
            If myMouseOverDeleteButton Then
                myMouseOverDeleteButton = False
                Me.Invalidate()
            End If
        End If
    End Sub

    Private Shared Function LowWord(ByVal value As Integer) As Integer
        Return value And &HFFFF
    End Function

    Private Shared Function HighWord(ByVal value As Integer) As Integer
        Return (value >> 16) And &HFFFF
    End Function

    Private Sub RecalcCurrentButtonPosition()
        myDeleteButtonImageLocation = New Point(2, Me.Height - STATUSBARHEIGHT + 4)
        myDeleteButtonImageLocationArea = New Rectangle(myDeleteButtonImageLocation, New Size(17, 17))
    End Sub

    Protected Overrides Sub OnResize(ByVal e As System.EventArgs)
        MyBase.OnResize(e)
        If Me.IsResizable Then
            If myPopupContentControl IsNot Nothing Then
                myPopupContentControl.Size = New Size(Me.Size.Width, Me.Size.Height - STATUSBARHEIGHT)
            End If
            RecalcCurrentButtonPosition()
        End If
    End Sub

    Protected Overrides Sub OnMouseClick(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseClick(e)
        If myMouseOverDeleteButton Then
            OnResetButtonClick()
        End If
    End Sub

    Protected Overridable Sub OnResetButtonClick()
        RaiseEvent ResetButtonClick(Me, EventArgs.Empty)
    End Sub

    Protected Overrides Sub OnMouseLeave(ByVal e As System.EventArgs)
        MyBase.OnMouseLeave(e)
        myMouseOverDeleteButton = False
        Me.Invalidate()
    End Sub

    Protected Overrides Sub WndProc(ByRef m As System.Windows.Forms.Message)
        If IsResizable Then
            If m.Msg = WM_GETMINMAXINFO Then
                Dim mmi As MINMAXINFO = CType(m.GetLParam(GetType(MINMAXINFO)), MINMAXINFO)
                mmi.ptMinTrackSize.x = Me.MinimumSize.Width
                mmi.ptMinTrackSize.y = Me.MinimumSize.Height

                If Not (Me.MaximumSize.Width = 0 And Me.MaximumSize.Height = 0) Then
                    mmi.ptMaxTrackSize.x = Me.MaximumSize.Width
                    mmi.ptMaxTrackSize.y = Me.MaximumSize.Height
                End If
                System.Runtime.InteropServices.Marshal.StructureToPtr(mmi, m.LParam, True)

            ElseIf (m.Msg = WM_NCHITTEST) Then
                Dim x = LowWord(m.LParam.ToInt32)
                Dim y = HighWord(m.LParam.ToInt32)

                Dim cLoc = PointToClient(New Point(x, y))

                If BottomGripArea.Contains(cLoc) And Not RightGripArea.Contains(cLoc) Then
                    m.Result = New IntPtr(HTBOTTOM)
                    Return
                End If

                If RightGripArea.Contains(cLoc) Then
                    m.Result = New IntPtr(HTBOTTOMRIGHT)
                    Return
                End If
            End If
        End If
        MyBase.WndProc(m)
    End Sub

    Private Sub DoLayout()
        If Me.AutoSize Then
            If myPopupContentControl IsNot Nothing Then
                Dim tmpSize = Size.Add(myOriginalHostingControlSize, New Size(1, 1))
                If IsResizable Then
                    tmpSize = Size.Add(tmpSize, New Size(0, 22))
                End If
                Me.Size = tmpSize
            End If
        End If
    End Sub

    Public Property ShowStatusBar As Boolean
    Public Property ValueTextAlignment As System.Drawing.StringAlignment

    Public Property ValueText As String
        Get
            Return myValueText
        End Get
        Set(value As String)
            If Not Object.Equals(myValueText, value) Then
                myValueText = value
                OnValueTextChanged(EventArgs.Empty)
            End If
        End Set
    End Property

    Protected Overridable Sub OnValueTextChanged(e As EventArgs)
        RaiseEvent ValueTextChanged(Me, e)
    End Sub

    Public Shadows Property AutoSize As Boolean
        Get
            Return myAutoSize
        End Get
        Set(ByVal value As Boolean)
            MyBase.AutoSize = False
            myAutoSize = value
        End Set
    End Property

    ''' <summary>
    ''' Bestimmt oder ermittelt, ob sich das Popup-Control in der Größe verändern lässt.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Property IsResizable As Boolean
        Get
            Return myIsResizable
        End Get
        Set(ByVal value As Boolean)
            AutoSize = True
            myIsResizable = value
        End Set
    End Property

    Public Property PopupContentControl As Control
        Get
            Return myPopupContentControl
        End Get
        Set(ByVal value As Control)
            If myPopupContentControl Is value Then Return
            myPopupContentControl = value
            If myPopupContentControl Is Nothing Then Return

            myOriginalHostingControlSize = myPopupContentControl.Size

            'Wenn das Steuerlement ein Fomular ist, dann dafür sorgen, 
            'dass es kein TopLevel sondern hostbares Formular ist.
            If GetType(Form).IsAssignableFrom(myPopupContentControl.GetType()) Then
                DirectCast(myPopupContentControl, Form).TopLevel = False
                myPopupContentControl.Dock = DockStyle.Fill
            End If
            Me.Controls.Add(Me.PopupContentControl)
            DoLayout()
        End Set
    End Property

    Public ReadOnly Property IsOpen As Boolean
        Get
            Return myIsVisible
        End Get
    End Property

    Protected Overridable Sub OnPopupOpening(ByVal e As PopupOpeningEventArgs)
        RaiseEvent PopupOpening(Me, e)
        If e.Cancel Then
            Return
        Else
            If Not e.PreferredNewSize.IsEmpty Then
                Me.Size = e.PreferredNewSize
                If e.Unresizable Then
                    Me.MaximumSize = e.PreferredNewSize
                    Me.MinimumSize = e.PreferredNewSize
                End If
            End If
        End If
    End Sub

    Private Sub OnPopupOpened(ByVal e As EventArgs)
        RaiseEvent PopupOpened(Me, e)
    End Sub

    Protected Overridable Sub OnPopupClosing(ByVal e As PopupClosingEventArgs)
        RaiseEvent PopupClosing(Me, e)
    End Sub

    Protected Overridable Sub OnPopupClosed(ByVal e As EventArgs)
        RaiseEvent PopupClosed(Me, e)
        myLastShownReferringControl = Nothing
    End Sub

    Public Shared Function GetPopupLocation(ByVal referringControl As Control, ByVal popupSize As Size) As Point

        Dim screenCoordinates As Point

        'Versuchen, das Parent zu ermitteln
        If referringControl.Parent Is Nothing Then

            Throw New ArgumentNullException("Control must be assigned to a Form when calculating the screen coordinates for its popup position")
        End If

        screenCoordinates = referringControl.Parent.PointToScreen(referringControl.Location)
        screenCoordinates += New Size(0, referringControl.Height)

        'Referenzpunkt gefunden
        Dim currentScreen = Screen.FromPoint(screenCoordinates)

        'Schauen, ob das Control nach unten Platz hat
        Dim tmpRec = New Rectangle(screenCoordinates, popupSize)

        If currentScreen.WorkingArea.Bottom < tmpRec.Bottom Then
            'ReferencePoint nach oben packen
            screenCoordinates = referringControl.Parent.PointToScreen(referringControl.Location)
            screenCoordinates -= New Size(0, popupSize.Height)
        End If

        'Schauen, ob das Control nach rechts Platz hat
        If currentScreen.WorkingArea.Right < tmpRec.Right Then
            'ReferencePoint nach links packen
            screenCoordinates -= New Size(popupSize.Width - referringControl.Width, 0)
        End If

        Return screenCoordinates
    End Function
End Class

Public Class PopupOpeningEventArgs
    Inherits CancelEventArgs

    Sub New(ByVal cancel As Boolean, ByVal PreferredNewSize As Size)
        MyBase.Cancel = cancel
        Me.PreferredNewSize = PreferredNewSize
    End Sub

    Property PreferredNewSize As Size
    Property Unresizable As Boolean
End Class
