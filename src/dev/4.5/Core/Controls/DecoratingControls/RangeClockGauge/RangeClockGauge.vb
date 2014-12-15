Imports System.Drawing
Imports System.Drawing.Drawing2D

Public Class RangeClockGauge
    Inherits TransparentControlBase

    Private myStartTime As Date?
    Private myEndtime As Date?
    Private myHasStartTopButton As Boolean
    Private myCanPause As Boolean

    Private Shared mySharedTimer As System.Timers.Timer
    Dim myCurrentTime As Date?

    Public Event StartTimeChanged(sender As Object, e As EventArgs)
    Public Event EndTimeChanged(sender As Object, e As EventArgs)
    Public Event DurationChanged(sender As Object, e As EventArgs)
    Public Event CurrentTimeChanged(sender As Object, e As EventArgs)

    Protected Overrides Sub OnPaint(e As System.Windows.Forms.PaintEventArgs)
        MyBase.OnPaint(e)
        DrawClockInternally(e.Graphics)
    End Sub

    Public Sub DrawClockInternally(ByVal g As Graphics)

        Dim TimeToAngle = Function(timeValue As Date) As Double
                              Dim lineAngle As Double = timeValue.Hour
                              lineAngle *= 30.0F
                              lineAngle += (StartTime.Value.Minute / 2.0F)
                              lineAngle -= 90
                              Return lineAngle
                          End Function

        Dim alarmTime As Date? = Nothing
        Dim drawDuration As Boolean

        Dim lineStartPoint As PolarCoordinate
        Dim lineEndPoint As PolarCoordinate

        If StartTime.HasValue AndAlso Not EndTime.HasValue Then
            alarmTime = StartTime
        ElseIf Not StartTime.HasValue AndAlso EndTime.HasValue Then
            alarmTime = EndTime
        ElseIf StartTime.HasValue AndAlso EndTime.HasValue Then
            drawDuration = True
        End If

        'Create Brushes and Pens
        Dim locPenWidth As Single = 3
        Dim locHalfPenWidth As Single = locPenWidth / 2
        Dim locCenter As PointF
        Dim locSize As SizeF

        Dim clockDimension As RectangleF = New RectangleF( _
            locHalfPenWidth, locHalfPenWidth, _
            (ClientSize.Width - 1) - locPenWidth, _
            (ClientSize.Height - 1) - locPenWidth)

        'Find the center
        locCenter.X = ClientSize.Width / 2.0F
        locCenter.Y = ClientSize.Height / 2.0F

        'Size without the clock's border
        locSize.Width = clockDimension.Width / 2 - 5
        locSize.Height = clockDimension.Height / 2 - 5

        'Enable AntiAlias
        g.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias

        'g.Clear(Color.Transparent)

        'Paint outer circle of the clock.
        g.DrawEllipse(New Pen(Color.Black, locPenWidth), clockDimension)

        If drawDuration Then
            Dim durationPath As New GraphicsPath
            Dim markerColor As Color = DurationMarkerColor

            If Duration.Value >= New TimeSpan(12, 0, 0) Then
                markerColor = DurationMarkerOverflowColor
            End If

            Dim linesAngle = CSng(TimeToAngle(StartTime.Value))
            lineStartPoint = New PolarCoordinate(locCenter,
                                                 locSize.Width * DurationMarkerWidthRatio,
                                                 locSize.Height * DurationMarkerWidthRatio, linesAngle)
            lineEndPoint = New PolarCoordinate(locCenter,
                                               locSize.Width - DurationMarkerMargin,
                                               locSize.Height - DurationMarkerMargin,
                                               linesAngle)

            durationPath.AddLine(lineStartPoint.Cartesian, lineEndPoint.Cartesian)

            Dim endAngle = CSng(TimeToAngle(EndTime.Value))

            Dim reducedClockDimension = clockDimension
            reducedClockDimension.Inflate(-DurationMarkerMargin, -DurationMarkerMargin)
            durationPath.AddArc(reducedClockDimension, linesAngle, endAngle - linesAngle)

            'durationPath.AddArc(clockDimension, linesAngle, endAngle - linesAngle)
            Dim deltaWidth = locSize.Width - locSize.Width * DurationMarkerWidthRatio
            Dim deltaHeight = locSize.Height - locSize.Height * DurationMarkerWidthRatio

            reducedClockDimension.Inflate(-deltaWidth, -deltaHeight)
            durationPath.AddArc(reducedClockDimension, endAngle, -(endAngle - linesAngle))

            lineStartPoint = New PolarCoordinate(locCenter, locSize.Width * DurationMarkerWidthRatio,
                                                 locSize.Height * DurationMarkerWidthRatio, endAngle)
            lineEndPoint = New PolarCoordinate(locCenter, locSize.Width - DurationMarkerMargin,
                                               locSize.Height - DurationMarkerMargin, endAngle)
            'durationPath.AddLine(lineStartPoint.Cartesian, lineEndPoint.Cartesian)
            durationPath.CloseFigure()

            Dim durationBrush As New SolidBrush(markerColor)
            g.FillPath(durationBrush, durationPath)
        End If

        'Paint the clock's hands if ShowTime is provided.
        If CurrentTime.HasValue Then
            'Parameter for hour hand
            Dim locHourHandAngle As Single = CSng(TimeToAngle(CurrentTime.Value))

            'Parameter for the minute hand
            Dim locMinuteHandAngle As Single = CurrentTime.Value.Minute
            locMinuteHandAngle *= 6
            locMinuteHandAngle -= 90

            'Parameter for the second hand
            Dim locSecondHandAngle As Single = CurrentTime.Value.Second
            locSecondHandAngle *= 6
            locSecondHandAngle -= 90

            'First draw the hour hand...
            lineStartPoint = New PolarCoordinate(locCenter, 0, 0, locHourHandAngle)
            lineEndPoint = New PolarCoordinate(locCenter, locSize.Width / 2, locSize.Height / 2, locHourHandAngle)
            g.DrawLine(New Pen(Color.Blue, 5), lineStartPoint.Cartesian.X, _
                                               lineStartPoint.Cartesian.Y, _
                                               lineEndPoint.Cartesian.X, _
                                               lineEndPoint.Cartesian.Y)

            '... then the minute hand ...
            lineStartPoint = New PolarCoordinate(locCenter, 0, 0, locMinuteHandAngle)
            lineEndPoint = New PolarCoordinate(locCenter, locSize.Width - 25, locSize.Height - 25, locMinuteHandAngle)
            g.DrawLine(New Pen(Color.Blue, 3), lineStartPoint.Cartesian.X, _
                                               lineStartPoint.Cartesian.Y, _
                                               lineEndPoint.Cartesian.X, _
                                               lineEndPoint.Cartesian.Y)

            '... then the second hand ...
            lineStartPoint = New PolarCoordinate(locCenter, 0, 0, locSecondHandAngle)
            lineEndPoint = New PolarCoordinate(locCenter, locSize.Width - 20, locSize.Height - 20, locSecondHandAngle)
            g.DrawLine(New Pen(Color.Black, 1), lineStartPoint.Cartesian.X, _
                                               lineStartPoint.Cartesian.Y, _
                                               lineEndPoint.Cartesian.X, _
                                               lineEndPoint.Cartesian.Y)

        End If

        'Paint the clock's face on demand.
        If ClockFaceVisible Then
            Dim locOffset As Single
            For locAngle As Single = 0 To 359 Step 30
                If locAngle = 0 Or locAngle = 90 Or locAngle = 180 Or locAngle = 270 Then
                    locOffset = 20
                Else
                    locOffset = 10
                End If

                Dim locPcStart As New PolarCoordinate(locCenter, locSize.Width - 5, locSize.Height - 5, locAngle)
                Dim locPcEnd As New PolarCoordinate(locCenter, locSize.Width - locOffset, _
                                    locSize.Height - locOffset, locAngle)
                g.DrawLine(Pens.Black, locPcStart.Cartesian.X, _
                                      locPcStart.Cartesian.Y, _
                                      locPcEnd.Cartesian.X, _
                                      locPcEnd.Cartesian.Y)
            Next
        End If

        If alarmTime.HasValue Then
            'Parameter for the alarm tag
            Dim alarmTagAngle As Single
            alarmTagAngle = alarmTime.Value.Hour
            If alarmTagAngle > 12 Then
                alarmTagAngle -= 12
            End If
            alarmTagAngle *= 30.0F
            alarmTagAngle += (alarmTime.Value.Minute / 2.0F)
            alarmTagAngle -= 90

            '... and if there is an alarm time to tag, draw that as well!
            lineStartPoint = New PolarCoordinate(locCenter, locSize.Width / 4 * 3, locSize.Height / 4 * 3, alarmTagAngle)
            lineEndPoint = New PolarCoordinate(locCenter, locSize.Width - 5, locSize.Height - 5, alarmTagAngle)
            g.DrawLine(New Pen(Color.Red, 5), lineStartPoint.Cartesian.X, _
                                               lineStartPoint.Cartesian.Y, _
                                               lineEndPoint.Cartesian.X, _
                                               lineEndPoint.Cartesian.Y)

        End If

        'New in this version: the MessageText
        'Todo: Draw via DrawString.
        If Not String.IsNullOrEmpty(Me.Text) Then
            'lblAlarmMessage.Text = MessageText
            'lblAlarmMessage.Visible = True
        Else
            'lblAlarmMessage.Visible = False
        End If
    End Sub

    Public Property StartTime As Date?
        Get
            Return myStartTime
        End Get
        Set(value As Date?)
            If value <> myStartTime OrElse
                (Not value.HasValue AndAlso myStartTime.HasValue) OrElse
                    (value.HasValue AndAlso Not myStartTime.HasValue) Then
                myStartTime = value
                OnStartTimeChanged(EventArgs.Empty)
                OnDurationChanged(EventArgs.Empty)
            End If
        End Set
    End Property

    Public ReadOnly Property Duration As TimeSpan?
        Get
            Return EndTime - StartTime
        End Get
    End Property

    Public Property DurationMarkerMargin As Integer = 3
    Public Property DurationMarkerWidthRatio As Single = 0.6
    Public Property DurationMarkerColor As Color = Color.LawnGreen
    Public Property DurationMarkerOverflowColor As Color = Color.Red

    Public Property EndTime As Date?
        Get
            Return myEndtime
        End Get
        Set(value As Date?)
            If value <> myEndtime OrElse
                (Not value.HasValue AndAlso myEndtime.HasValue) OrElse
                    (value.HasValue AndAlso Not myEndtime.HasValue) Then
                myEndtime = value
                OnEndTimeChanged(EventArgs.Empty)
                OnDurationChanged(EventArgs.Empty)
            End If
        End Set
    End Property

    Public Property HasStartStopButton As Boolean
        Get
            Return myHasStartTopButton
        End Get
        Set(value As Boolean)
            If value <> myHasStartTopButton Then
                myCanPause = value
                Me.InvalidateEx()
            End If
        End Set
    End Property

    Public Property CanPause As Boolean
        Get
            Return myCanPause
        End Get
        Set(value As Boolean)
            If value <> myCanPause Then
                myCanPause = value
                Me.InvalidateEx()
            End If
        End Set
    End Property

    Public Property ShowClockFace As Boolean
    Public Property HideClockFaceSizeThreshold As Integer = 65

    Public ReadOnly Property ClockFaceVisible As Boolean
        Get
            Return ShowClockFace AndAlso
                Me.ClientSize.Width > HideClockFaceSizeThreshold AndAlso
                Me.ClientSize.Height > HideClockFaceSizeThreshold
        End Get
    End Property

    Public Property CurrentTime As Date?
        Get
            Return myCurrentTime
        End Get

        Set(value As Date?)
            If value <> myEndtime OrElse
                (Not value.HasValue AndAlso myCurrentTime.HasValue) OrElse
                    (value.HasValue AndAlso Not myCurrentTime.HasValue) Then
                myCurrentTime = value
                OnCurrentTimeChanged(EventArgs.Empty)
            End If
        End Set
    End Property

    Protected Overridable Sub OnStartTimeChanged(e As EventArgs)
        RaiseEvent StartTimeChanged(Me, e)
    End Sub

    Protected Overridable Sub OnEndTimeChanged(e As EventArgs)
        RaiseEvent EndTimeChanged(Me, e)
    End Sub

    Protected Overridable Sub OnDurationChanged(e As EventArgs)
        RaiseEvent DurationChanged(Me, e)
    End Sub

    Protected Overridable Sub OnCurrentTimeChanged(e As EventArgs)
        RaiseEvent CurrentTimeChanged(Me, e)
    End Sub

End Class
