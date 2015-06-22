Imports System.Windows.Forms
Imports System.Drawing
Imports System.Runtime.InteropServices

Imports System.Windows.Forms.VisualStyles
Imports System.Windows.Forms.VisualStyles.VisualStyleElement
Imports System.Windows.Forms.VisualStyles.VisualStyleElement.Page
Imports System.ComponentModel
Imports System.Drawing.Drawing2D

<ToolboxItem(False)>
Public Class TooltipEx
    Inherits System.Windows.Forms.Form
    Implements IMessageFilter

    Private Const WM_NCHITTEST = &H84
    Private Const WM_GETMINMAXINFO As Integer = &H24

    Private Const WS_EX_TOOLWINDOW = &H80
    Private Const WS_EX_NOACTIVATE As Integer = &H8000000
    Private Const WS_EX_TOPMOST = 8
    Private Const WS_EX_TRANSPARENT = &H20
    Private Const WS_EX_LAYERED = &H80000

    Private Const HTBOTTOM = 15
    Private Const HTBOTTOMRIGHT = 17

    Private Const GRIPHEIGHT = 16
    Private Const STATUSBARHEIGHT = 22
    Private Const LEFTSTATUSTEXTOFFSET = 22

    Public Const WM_LBUTTONDOWN = &H201
    Public Const WM_NCLBUTTONDOWN = &HA1
    Public Const WM_KEYDOWN = &H100

    Dim myDeleteButtonImageLocationArea As Rectangle
    Dim myIsVisible As Boolean

    Public Event CloseButtonClick(ByVal sender As Object, ByVal e As EventArgs)
    Public Event ActionButtonClick(ByVal sender As Object, ByVal e As EventArgs)
    Public Event TooltipOpening(ByVal sender As Object, ByVal e As TooltipOpeningEventArgs)
    Public Event TooltipOpened(ByVal sender As Object, ByVal e As EventArgs)
    Public Event TooltipClosed(ByVal sender As Object, ByVal e As EventArgs)
    Public Event TooltipClosing(ByVal sender As Object, ByVal e As TooltipClosingEventArgs)

    'TODO: Requestreason einbauen, falls notwendig, für die Unterscheidung.
    Public Event TooltipCloseRequested(ByVal sender As Object, ByVal E As PopupCloseRequestedEventArgs)

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

    Private myTooltipContentControl As Control
    Private myIsResizable As Boolean
    Private myAutoSize As Boolean
    Private myOriginalHostingControlSize As Size
    Private myDeleteButtonImageLocation As Point
    Private myMouseOverDeleteButton As Boolean
    Private myRejectFocus As Boolean
    Private myLastShownReferringControl As Control

    Sub New()
        MyBase.New()

        Me.TransparencyKey = Color.White
        Me.BackColor = Color.White
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.TopMost = True

        Me.SetStyle(ControlStyles.ResizeRedraw, True)
        Me.SetStyle(ControlStyles.OptimizedDoubleBuffer, True)
    End Sub


    Public Sub ShowTooltip(ByVal location As Point)
        Me.Location = location
        MyBase.Show()
    End Sub

    Public Sub CloseTooltip()
        CloseTooltipInternally(New TooltipClosingEventArgs(TooltipClosingReason.CloseCalled))
    End Sub

    Friend Sub CloseTooltipInternally(ByVal ClosingReason As TooltipClosingEventArgs)
        If Not ClosingReason.TooltipClosingReason = TooltipClosingReason.SuppressEvent Then
            OnTooltipClosing(ClosingReason)
        End If
        If Not Me.IsOpen Then Return
        If Not ClosingReason.Cancel Then
            Hide()
            OnTooltipClosed(EventArgs.Empty)
        End If
    End Sub

    Private Shadows Sub Show(ByVal location As Point)
        Me.Location = location
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
            RaiseEvent TooltipCloseRequested(Me, New PopupCloseRequestedEventArgs(PopupCloseRequestReasons.KeyboardCancel))
            Return True
        End If

        'Return für offenes Popup behandeln
        If m.Msg = WM_KEYDOWN And m.WParam.ToInt32 = 13 And Me.IsOpen Then
            RaiseEvent TooltipCloseRequested(Me, New PopupCloseRequestedEventArgs(PopupCloseRequestReasons.KeyboardCommit))
            Return True
        End If

        'TAB für offenes Popup behandeln
        If m.Msg = WM_KEYDOWN And m.WParam.ToInt32 = Keys.Tab And Me.IsOpen Then
            RaiseEvent TooltipCloseRequested(Me, New PopupCloseRequestedEventArgs(PopupCloseRequestReasons.KeyboardCommit))
            Return False
        End If

        Dim pe As TooltipClosingEventArgs = Nothing
        If (myIsVisible AndAlso (Form.ActiveForm Is Nothing OrElse Form.ActiveForm.Equals(Me))) Then
            pe = New TooltipClosingEventArgs(TooltipClosingReason.AppFocusChanged)
        End If

        If (m.Msg = WM_LBUTTONDOWN Or m.Msg = WM_NCLBUTTONDOWN) And Not IsHandleInThisControlHierachy(m.HWnd) Then
            pe = New TooltipClosingEventArgs(TooltipClosingReason.AppClicked)
        End If

        If pe IsNot Nothing Then
            OnTooltipClosing(pe)
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

    Protected Overrides Sub WndProc(ByRef m As System.Windows.Forms.Message)
        MyBase.WndProc(m)
    End Sub

    Public Property TooltipContentControl As Control
        Get
            Return myTooltipContentControl
        End Get

        Set(ByVal value As Control)
            If myTooltipContentControl Is value Then Return
            myTooltipContentControl = value
            myOriginalHostingControlSize = myTooltipContentControl.Size

            'Wenn das Steuerlement ein Fomular ist, dann dafür sorgen, 
            'dass es kein TopLevel sondern hostbares Formular ist.
            If GetType(Form).IsAssignableFrom(myTooltipContentControl.GetType()) Then
                DirectCast(myTooltipContentControl, Form).TopLevel = False
                myTooltipContentControl.Dock = DockStyle.Fill
            End If
            Me.Controls.Add(Me.TooltipContentControl)
        End Set
    End Property

    Public ReadOnly Property IsOpen As Boolean
        Get
            Return myIsVisible
        End Get
    End Property

    Protected Overridable Sub OnTooltipOpening(ByVal e As TooltipOpeningEventArgs)
        RaiseEvent TooltipOpening(Me, e)
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

    Private Sub OnTooltipOpened(ByVal e As EventArgs)
        RaiseEvent TooltipOpened(Me, e)
    End Sub

    Protected Overridable Sub OnTooltipClosing(ByVal e As TooltipClosingEventArgs)
        RaiseEvent TooltipClosing(Me, e)
    End Sub

    Protected Overridable Sub OnTooltipClosed(ByVal e As EventArgs)
        RaiseEvent TooltipClosed(Me, e)
        myLastShownReferringControl = Nothing
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        MyBase.OnPaint(e)
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias
        FillRoundedRectangle(Brushes.LightGoldenrodYellow, e.Graphics, 0, 0, Me.ClientRectangle.Width - 1, Me.ClientRectangle.Height - 1, 10)
        DrawRoundedRectangle(Pens.Black, e.Graphics, 0, 0, Me.ClientRectangle.Width - 1, Me.ClientRectangle.Height - 1, 10)
    End Sub

    Private Sub DrawRoundedRectangle(ByVal p As Pen, ByVal g As Graphics,
                                ByVal X As Single, ByVal Y As Single,
                                ByVal Width As Single, ByVal Height As Single,
                                ByVal Radius As Single)

        Dim gp = New GraphicsPath()

        gp.AddLine(X + Radius, Y, X + Width - (Radius * 2), Y)                  ' Linie
        gp.AddArc(X + Width - (Radius * 2), Y, Radius * 2, Radius * 2, 270, 90) ' Ecke
        gp.AddLine(X + Width, Y + Radius, X + Width, Y + Height - (Radius * 2)) ' Line
        gp.AddArc(X + Width - (Radius * 2), Y + Height - (Radius * 2), Radius * 2, Radius * 2, 0, 90) ' Ecke
        gp.AddLine(X + Width - (Radius * 2), Y + Height, X + Radius, Y + Height) ' Linie
        gp.AddArc(X, Y + Height - (Radius * 2), Radius * 2, Radius * 2, 90, 90) ' Ecke
        gp.AddLine(X, Y + Height - (Radius * 2), X, Y + Radius) ' Line
        gp.AddArc(X, Y, Radius * 2, Radius * 2, 180, 90) ' Ecke
        gp.CloseFigure()

        g.DrawPath(p, gp)
        gp.Dispose()
    End Sub

    Private Sub FillRoundedRectangle(ByVal b As Brush, ByVal g As Graphics,
                            ByVal X As Single, ByVal Y As Single,
                            ByVal Width As Single, ByVal Height As Single,
                            ByVal Radius As Single)

        Dim gp = New GraphicsPath()

        gp.AddLine(X + Radius, Y, X + Width - (Radius * 2), Y)                  ' Linie
        gp.AddArc(X + Width - (Radius * 2), Y, Radius * 2, Radius * 2, 270, 90) ' Ecke
        gp.AddLine(X + Width, Y + Radius, X + Width, Y + Height - (Radius * 2)) ' Line
        gp.AddArc(X + Width - (Radius * 2), Y + Height - (Radius * 2), Radius * 2, Radius * 2, 0, 90) ' Ecke
        gp.AddLine(X + Width - (Radius * 2), Y + Height, X + Radius, Y + Height) ' Linie
        gp.AddArc(X, Y + Height - (Radius * 2), Radius * 2, Radius * 2, 90, 90) ' Ecke
        gp.AddLine(X, Y + Height - (Radius * 2), X, Y + Radius) ' Line
        gp.AddArc(X, Y, Radius * 2, Radius * 2, 180, 90) ' Ecke
        gp.CloseFigure()

        g.FillPath(b, gp)
        gp.Dispose()
    End Sub

End Class

Public Class TooltipOpeningEventArgs
    Inherits CancelEventArgs

    Sub New(ByVal cancel As Boolean, ByVal PreferredNewSize As Size)
        MyBase.Cancel = cancel
        Me.PreferredNewSize = PreferredNewSize
    End Sub

    Property PreferredNewSize As Size
    Property Unresizable As Boolean
End Class
