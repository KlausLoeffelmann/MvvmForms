Imports System.Runtime.InteropServices
Imports System.Windows.Forms

Public Class AnimationFormBase
    Inherits Form

    'Private mWindow As Window
    Private Const WM_HOTKEY As Integer = &H312
    Private Const WM_MOUSEMOVE As Int32 = &H200

    Private Const AW_HIDE = &H10000
    Private Const AW_ACTIVATE = &H20000
    Private Const AW_HOR_POSITIVE = 1
    Private Const AW_HOR_NEGATIVE = 2
    Private Const AW_VER_POSITIVE = 4
    Private Const AW_VER_NEGATIVE = 8
    Private Const AW_CENTER = 16
    Private Const AW_SLIDE = &H40000
    Private Const AW_BLEND = &H80000

    Private myFadeInFlags As Integer
    Private myFadeOutFlags As Integer
    Private myTransitionEffect As TransitionEffects

    Private myHotKeyList As New Dictionary(Of Short, HotKeyType)
    Private myHotKeyGuidList As New Dictionary(Of Short, Guid)

    Private Declare Function RegisterHotKey Lib "user32" ( _
    ByVal Hwnd As IntPtr, _
    ByVal ID As Integer, _
    ByVal Modifiers As Integer, _
    ByVal Key As Integer) As Integer

    Private Declare Function UnregisterHotKey Lib "user32" ( _
        ByVal Hwnd As IntPtr, _
        ByVal ID As Integer) _
    As Integer

    Private Declare Function GlobalAddAtom Lib "kernel32" Alias "GlobalAddAtomA" ( _
        ByVal IDString As String) _
    As Short

    Private Declare Function GlobalDeleteAtom Lib "kernel32" ( _
        ByVal Atom As Short) _
    As Short

    <DllImport("user32.dll", CharSet:=CharSet.Auto)>
    Private Shared Function AnimateWindow(hwand As IntPtr, dwTime As Integer, dwFlags As Integer) As Integer
    End Function

    <DllImport("SHELL32", CallingConvention:=CallingConvention.StdCall)> _
    Private Shared Function SHAppBarMessage(dwMessage As Integer, ByRef pData As APPBARDATA) As UInteger
    End Function

    <StructLayout(LayoutKind.Sequential)> _
    Public Structure RECT
        Public left As Integer
        Public top As Integer
        Public right As Integer
        Public bottom As Integer
    End Structure

    <StructLayout(LayoutKind.Sequential)> _
    Public Structure APPBARDATA
        Public cbSize As Integer
        Public hWnd As IntPtr
        Public uCallbackMessage As Integer
        Public uEdge As Integer
        Public rc As RECT
        Public lParam As IntPtr
    End Structure

    Public Enum ABMsg As Integer
        ABM_NEW = 0
        ABM_REMOVE = 1
        ABM_QUERYPOS = 2
        ABM_SETPOS = 3
        ABM_GETSTATE = 4
        ABM_GETTASKBARPOS = 5
        ABM_ACTIVATE = 6
        ABM_GETAUTOHIDEBAR = 7
        ABM_SETAUTOHIDEBAR = 8
        ABM_WINDOWPOSCHANGED = 9
        ABM_SETSTATE = 10
    End Enum

    Public Enum ABEdge As Integer
        ABE_LEFT = 0
        ABE_TOP
        ABE_RIGHT
        ABE_BOTTOM
    End Enum

    Public Enum ABState As Integer
        ABS_MANUAL = 0
        ABS_AUTOHIDE = 1
        ABS_ALWAYSONTOP = 2
        ABS_AUTOHIDEANDONTOP = 3
    End Enum

    Public Enum TaskBarEdge As Integer
        Bottom
        Top
        Left
        Right
    End Enum

    Public Shared Sub GetTaskBarInfo(ByRef taskBarEdge As TaskBarEdge, ByRef height As Integer, ByRef autoHide As Boolean)
        Dim abd As New APPBARDATA()

        height = 0
        taskBarEdge = taskBarEdge.Bottom
        autoHide = False

        Dim ret As UInteger = SHAppBarMessage(CInt(ABMsg.ABM_GETTASKBARPOS), abd)
        Select Case abd.uEdge
            Case CInt(ABEdge.ABE_BOTTOM)
                taskBarEdge = taskBarEdge.Bottom
                height = abd.rc.bottom - abd.rc.top
                Exit Select
            Case CInt(ABEdge.ABE_TOP)
                taskBarEdge = taskBarEdge.Top
                height = abd.rc.bottom
                Exit Select
            Case CInt(ABEdge.ABE_LEFT)
                taskBarEdge = taskBarEdge.Left
                height = abd.rc.right - abd.rc.left
                Exit Select
            Case CInt(ABEdge.ABE_RIGHT)
                taskBarEdge = taskBarEdge.Right
                height = abd.rc.right - abd.rc.left
                Exit Select

        End Select

        abd = New APPBARDATA()
        Dim uState As UInteger = SHAppBarMessage(CInt(ABMsg.ABM_GETSTATE), abd)
        Select Case uState
            Case CInt(ABState.ABS_ALWAYSONTOP)
                autoHide = False
                Exit Select
            Case CInt(ABState.ABS_AUTOHIDE)
                autoHide = True
                Exit Select
            Case CInt(ABState.ABS_AUTOHIDEANDONTOP)
                autoHide = True
                Exit Select
            Case CInt(ABState.ABS_MANUAL)
                autoHide = False
                Exit Select
        End Select
    End Sub


    ''' <summary>
    ''' Diesem Event wird immer die zugewiesene HotKeyID übergeben wenn eine HotKey Kombination gedrückt wurde.
    ''' </summary>
    Public Event HotKeyPressed(ByVal sender As Object, ByVal e As HotKeyPressEventArgs)

    Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.TransitionEffect = TransitionEffects.Circle
        Me.Speed = 500
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.AllowClosing = True
        Me.AllowVisible = False
    End Sub

    Protected Overrides Sub SetVisibleCore(value As Boolean)
        'If it shouldn't be visible ...
        If value And (Not AllowVisible) AndAlso Not Me.DesignMode Then
            '...don't make it visible.
            Return
        End If

        'Animate window based on what to do.
        If value Then
            AnimateWindow(Me.Handle, Me.Speed, myFadeInFlags)
        Else
            AnimateWindow(Me.Handle, Me.Speed, myFadeOutFlags)
        End If
        MyBase.SetVisibleCore(value)
    End Sub

    Protected Overrides Sub OnClosing(e As System.ComponentModel.CancelEventArgs)
        MyBase.OnClosing(e)
        If Not Me.AllowClosing Then
            e.Cancel = True
            Return
        End If

        If Not e.Cancel Then
            AnimateWindow(Me.Handle, Me.Speed, myFadeOutFlags)
        End If
    End Sub

    ''' <summary>
    ''' Diese Funktion fügt einen Hotkey hinzu und registriert ihn auch sofort
    ''' </summary>
    ''' <param name="KeyCode">Den KeyCode für die Taste</param>
    ''' <param name="Modifiers">Die Zusatztasten wie z.B. Strg oder Alt, diese können auch mit OR kombiniert werden</param>
    ''' <param name="HotKeyID">Die ID die der Hotkey bekommen soll um diesen zu identifizieren</param>
    Public Sub AddHotKey(ByVal KeyCode As Keys, ByVal Modifiers As HotKeyModifiers, ByVal HotKeyID As HotKeyType, Optional ByVal ProjGuid As Guid = Nothing)

        'Wird durch Remove ersetzt
        'If myHotKeyList.ContainsValue(HotKeyID) Then
        '    myHotKeyList.Remove((From i In myHotKeyList Where i.Value = HotKeyID Select i.Key).FirstOrDefault)
        'End If


        'Einmal den HotKeyType als identifizierer nehmen und einmal die ProjGuid
        Dim ID As Short
        If HotKeyID <> HotKeyType.ProjectShortCut Then
            ID = GlobalAddAtom(HotKeyID.ToString)
        Else
            ID = GlobalAddAtom(ProjGuid.ToString)
        End If

        If Not myHotKeyList.ContainsKey(ID) Then
            'In das Dictionary für die HotKeys eintragen
            myHotKeyList.Add(ID, HotKeyID)
        End If


        'Falls es ein ProjectHotKey ist zusätzlich in das Dictionary für die ProjectGuid eintragen
        If HotKeyID = HotKeyType.ProjectShortCut Then
            If ProjGuid <> Nothing Then
                myHotKeyGuidList.Add(ID, ProjGuid)
            Else
                If Debugger.IsAttached Then
                    'Wieso ist projGuid nothign?
                    Stop
                End If
            End If
        End If

        RegisterHotKey(Me.Handle, ID, Modifiers, KeyCode)
    End Sub

    ''' <summary>
    ''' Diese Funktion entfernt einen Hotkey und deregistriert ihn auch sofort
    ''' </summary>
    ''' <param name="HotKeyID">Gibt die HotkeyID an welche entfernt werden soll</param>
    Public Sub RemoveHotKey(ByVal HotKeyID As HotKeyType, Optional ByVal ProjGuid As Guid = Nothing)
        'If mHotKeyIDList.ContainsKey(HotKeyID) = False Then Exit Sub

        'Aus dem HotKeyType Dictionary löschen
        Dim ID As Short = (From i In myHotKeyList Where i.Value = HotKeyID).FirstOrDefault.Key
        myHotKeyList.Remove(ID)


        'Falls es ein ProjectHotKey ist die Guid aus dem ProjGuid Dictionary löschen
        'Die ID ist diesselbe
        If ProjGuid <> Nothing Then
            myHotKeyGuidList.Remove(ID)
        End If

        UnregisterHotKey(Me.Handle, CInt(ID))
        GlobalDeleteAtom(ID)
    End Sub

    'Alle ProjectHotKeys löschen
    Public Sub RemoveAllProjectHotKeys()
        For Each item In myHotKeyGuidList
            UnregisterHotKey(Me.Handle, CInt(item.Key))
            GlobalDeleteAtom(item.Key)
        Next

        myHotKeyGuidList.Clear()
    End Sub

    Protected Overridable Sub OnHotkeyPressed(ByVal e As HotKeyPressEventArgs)
        RaiseEvent HotKeyPressed(Me, e)
    End Sub

    Public Function MakeHotKeysFromSettings(ByVal hotkeystr As String, ByVal hotkeyt As HotKeyType) As Boolean
        Try

            Dim stringarray = hotkeystr.Split(";"c)

            '0 = Buchstabe, danach folgen die HotkeyModifiers
            '1 = hotKeyModifiers
            Dim Key = Keys.Parse(GetType(System.Windows.Forms.Keys), stringarray(0), False)
            Select Case stringarray(1)
                Case "MOD_ALT"
                    Me.AddHotKey(CType(Key, Keys), HotKeyModifiers.MOD_ALT, hotkeyt)
                Case "MOD_ALTMOD_SHIFT"
                    Me.AddHotKey(CType(Key, Keys), HotKeyModifiers.MOD_ALT Or HotKeyModifiers.MOD_SHIFT, hotkeyt)
                Case "MOD_ALTMOD_SHIFTMOD_CTRL"
                    Me.AddHotKey(CType(Key, Keys), HotKeyModifiers.MOD_ALT Or HotKeyModifiers.MOD_SHIFT Or HotKeyModifiers.MOD_CONTROL, hotkeyt)
                Case "MOD_SHIFT"
                    Me.AddHotKey(CType(Key, Keys), HotKeyModifiers.MOD_SHIFT, hotkeyt)
                Case "MOD_SHIFTMOD_CTRL"
                    Me.AddHotKey(CType(Key, Keys), HotKeyModifiers.MOD_SHIFT Or HotKeyModifiers.MOD_CONTROL, hotkeyt)
                Case "MOD_CTRL"
                    Me.AddHotKey(CType(Key, Keys), HotKeyModifiers.MOD_CONTROL, hotkeyt)
                Case "MOD_ALTMOD_CTRL"
                    Me.AddHotKey(CType(Key, Keys), HotKeyModifiers.MOD_ALT Or HotKeyModifiers.MOD_CONTROL, hotkeyt)
            End Select

            Return True
        Catch
            Return False
        End Try
    End Function

    Protected Overrides Sub WndProc(ByRef m As System.Windows.Forms.Message)
        If m.Msg = WM_HOTKEY Then

            Dim hktyp = myHotKeyList(CShort(m.WParam.ToInt32))

            'Falls es ein Buchungshotkey ist nur den HotKEyType mitgeben
            Dim hkeArgs As HotKeyPressEventArgs
            If hktyp <> HotKeyType.ProjectShortCut Then
                hkeArgs = New HotKeyPressEventArgs(hktyp)
            Else
                'Falls es ein ProjectHotKey ist auch die ProjectGUID mitliefern
                hkeArgs = New HotKeyPressEventArgs(hktyp, myHotKeyGuidList(CShort(m.WParam.ToInt32)))
            End If
            OnHotkeyPressed(hkeArgs)
            Return
        End If

        MyBase.WndProc(m)
    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If

            'Hotkeys wieder entfernen
            Try
                'Versuchen, die Hotkeys zu entfernen.
                RemoveAllProjectHotKeys()
            Catch ex As Exception
            End Try
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub


    Public Property AllowVisible As Boolean
    Public Property AllowClosing As Boolean
    Public Property Speed As Integer

    Public Property TransitionEffect As TransitionEffects
        Get
            Return myTransitionEffect
        End Get
        Set(value As TransitionEffects)
            myTransitionEffect = value
            Select Case value
                Case TransitionEffects.Blend
                    myFadeInFlags = AW_ACTIVATE Or AW_BLEND
                    myFadeOutFlags = AW_HIDE Or AW_BLEND
                Case TransitionEffects.Circle
                    myFadeInFlags = AW_ACTIVATE Or AW_CENTER
                    myFadeOutFlags = AW_HIDE Or AW_CENTER
                Case TransitionEffects.HorizontalSlideRight
                    myFadeInFlags = AW_ACTIVATE Or AW_SLIDE Or AW_HOR_POSITIVE
                    myFadeOutFlags = AW_HIDE Or AW_SLIDE Or AW_HOR_NEGATIVE
                Case TransitionEffects.HorizontalSlideLeft
                    myFadeInFlags = AW_ACTIVATE Or AW_SLIDE Or AW_HOR_NEGATIVE
                    myFadeOutFlags = AW_HIDE Or AW_SLIDE Or AW_HOR_POSITIVE
                Case TransitionEffects.VerticalSlideUp
                    myFadeInFlags = AW_ACTIVATE Or AW_SLIDE Or AW_VER_NEGATIVE
                    myFadeOutFlags = AW_HIDE Or AW_SLIDE Or AW_VER_POSITIVE
                Case TransitionEffects.VerticalSlideDown
                    myFadeInFlags = AW_ACTIVATE Or AW_SLIDE Or AW_VER_POSITIVE
                    myFadeOutFlags = AW_HIDE Or AW_SLIDE Or AW_VER_NEGATIVE
            End Select
        End Set
    End Property

End Class

Public Enum TransitionEffects
    VerticalSlideUp
    VerticalSlideDown
    HorizontalSlideLeft
    HorizontalSlideRight
    Blend
    Circle
End Enum

Public Enum HotKeyType As Integer
    StartStopClocking
    ShowWindow
    Pause
    ProjectShortCut
    ShowOptions
    HideWindow
End Enum

Public Enum HotKeyModifiers As Integer
    MOD_ALT = 1
    MOD_CONTROL = 2
    MOD_SHIFT = 4
    MOD_EMPTY = 0
End Enum

Public Class HotKeyPressEventArgs
    Inherits EventArgs

    Sub New(ByVal hotKey As HotKeyType)
        Me.Hotkey = hotKey
    End Sub

    Sub New(ByVal hotKey As HotKeyType, ByVal projguid As Guid)
        Me.Hotkey = hotKey
        Me.ProjGuid = projguid
    End Sub

    Property Hotkey As HotKeyType
    Property ProjGuid As Guid
End Class
