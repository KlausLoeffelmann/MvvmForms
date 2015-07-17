Imports System.Windows.Forms
Imports System.Drawing
Imports System.ComponentModel

<ToolboxItem(False)>
Public Class MonthCalendarEx
    Inherits MonthCalendar

    Private myDateChangedProc As DateRangeEventHandler
    Private myMouseDownProc As MouseEventHandler
    Private myMouseUpProc As MouseEventHandler
    Private myClickProc As EventHandler
    Private myDateSelectedProc As DateRangeEventHandler

    Sub New()
        MyBase.new()
    End Sub

    Public ReadOnly Property DateChangedProc As DateRangeEventHandler
        Get
            Return myDateChangedProc
        End Get
    End Property

    Public Sub AddDateChangedProcHandler(eh As DateRangeEventHandler)
        If eh Is Nothing Then Return
        myDateChangedProc = DirectCast([Delegate].Combine(myDateChangedProc, eh), DateRangeEventHandler)
        AddHandler Me.DateChanged, AddressOf eh.Invoke
    End Sub

    Public ReadOnly Property DateSelectedProc As DateRangeEventHandler
        Get
            Return myDateSelectedProc
        End Get
    End Property

    Public Sub AddDateSelectedProcHandler(eh As DateRangeEventHandler)
        If eh Is Nothing Then Return
        myDateSelectedProc = DirectCast([Delegate].Combine(myDateSelectedProc, eh), DateRangeEventHandler)
        AddHandler Me.DateSelected, AddressOf eh.Invoke
    End Sub

    Public ReadOnly Property MouseDownProc As MouseEventHandler
        Get
            Return myMouseDownProc
        End Get
    End Property

    Public Sub AddMouseDownProcHandler(eh As MouseEventHandler)
        If eh Is Nothing Then Return
        myMouseDownProc = DirectCast([Delegate].Combine(myMouseDownProc, eh), MouseEventHandler)
        AddHandler Me.MouseDown, AddressOf eh.Invoke
    End Sub

    Public ReadOnly Property MouseUpProc As MouseEventHandler
        Get
            Return myMouseUpProc
        End Get
    End Property

    Public Sub AddMouseUpProcHandler(eh As MouseEventHandler)
        If eh Is Nothing Then Return
        myMouseUpProc = DirectCast([Delegate].Combine(myMouseUpProc, eh), MouseEventHandler)
        AddHandler Me.MouseUp, AddressOf eh.Invoke
    End Sub

    Public ReadOnly Property ClickProc As EventHandler
        Get
            Return myClickProc
        End Get
    End Property

    Public Sub AddClickProcHandler(eh As EventHandler)
        If eh Is Nothing Then Return
        myClickProc = DirectCast([Delegate].Combine(myClickProc, eh), Global.System.EventHandler)
        AddHandler Me.Click, AddressOf eh.Invoke
    End Sub
End Class
