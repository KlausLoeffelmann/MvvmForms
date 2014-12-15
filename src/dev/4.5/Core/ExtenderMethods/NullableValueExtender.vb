Imports System.Runtime.CompilerServices
Imports System.Windows.Forms
Imports System.Drawing
Imports System.Drawing.Drawing2D

Module NullableValueExtender

    Private myToolTipShapeCoordinates As New List(Of PointF) From {New PointF(10, 0),
                                                               New PointF(-1, 0),
                                                               New PointF(-1, -1),
                                                               New PointF(10, -1),
                                                               New PointF(10, 20),
                                                               New PointF(0, 10),
                                                               New PointF(10, 10),
                                                               New PointF(10, 0)}

    <Extension()>
    Public Function ControlTypeAndNameString(ByVal ctrl As Control) As String
        Return ctrl.GetType.Name & " (" & ctrl.Name & ")"
    End Function


    <Extension()>
    Public Function NullSafeToString(nullableControl As INullableValueControl) As String
        If nullableControl.Value IsNot Nothing Then

            Return nullableControl.Value.ToString
        Else
            Return nullableControl.NullValueMessage
        End If
    End Function

    <Extension()>
    Public Sub ValidationTooltipHandler(Of nullControlType As {Control, INullableValueControl})(nullableControl As nullControlType,
                                           vfe As RequestValidationFailedReactionEventArgs)

        'Die Fehlermeldung soll in einem Tooltip neben
        'dem Steuerelement für eine gewisse Zeit zu sehen sein.
        Dim tt As New ToolTip()

        'Steuerelement malen wir selbst.
        tt.OwnerDraw = True

        'Soll sich Ausblenden.
        tt.UseFading = True
        tt.UseAnimation = True

        'Dank Mehrzeiligen Lambdas, können wir den Drawing-Handler Code
        'direkt dem Draw-Ereignis übergeben.
        AddHandler tt.Draw, Sub(sender As Object, de As DrawToolTipEventArgs)
                                Dim pen As New Pen(Brushes.Black, 1)
                                'Point-Array clonen und Koordinaten anpassen

                                Dim path As New GraphicsPath()
                                Dim p1, p2 As PointF
                                p1 = PointF.Empty

                                'Die Koordinaten des Rahmen des
                                'Tooltipps stehen in einer List(Of PointF),
                                'die wir mit der ForEach-Methode, und wieder
                                'mit einem mehrzeiligen Lambda durchiterieren.
                                myToolTipShapeCoordinates.ForEach(
                                    Sub(item)

                                        'Koordinaten des Tooltips an
                                        'die des Steuerelements anpassen:
                                        Dim x = item.X
                                        Dim y = item.Y
                                        If x = -1 Then
                                            x = de.Bounds.X + de.Bounds.Width - 1
                                        Else
                                            x += de.Bounds.X
                                        End If

                                        If y = -1 Then
                                            y = de.Bounds.Y + de.Bounds.Height - 1
                                        Else
                                            y += de.Bounds.Y
                                        End If
                                        p1 = p2
                                        p2 = New PointF(x, y)
                                        If p1 <> Point.Empty Then
                                            path.AddLine(p1, p2)
                                        End If
                                    End Sub)

                                'Zum Malen auf AntiAlias stellen: sieht "weicher" aus.
                                de.Graphics.SmoothingMode = SmoothingMode.AntiAlias
                                path.CloseFigure()

                                'Hintergrund, Umrandung und Text des Tooltips malen:
                                de.Graphics.FillPath(New SolidBrush(Color.AliceBlue), path)
                                de.Graphics.DrawPath(Pens.Black, path)
                                de.Graphics.DrawString(de.ToolTipText, New Font(FontFamily.GenericSansSerif,
                                                                               8, FontStyle.Regular),
                                                                           Brushes.Black,
                                                        New RectangleF(de.Bounds.X + 15, de.Bounds.Y + 5,
                                                                       de.Bounds.Width - 15, de.Bounds.Height - 5))
                            End Sub


        'Die ergibt sich nun auf jeden Fall durch die Ereignisparameter.
        Dim ballonMessage As String
        ballonMessage = vfe.BallonMessage

        'Tooltip ausgeben. Das erst triggert den obenstehenden Code,
        'da jetzt erst das Draw-Ereignis des Tooltips ausgelöst wird,
        'dem dieser Code mit AddHandler zugeordnet wurde.
        tt.Show("Die Eingabe hatte einen Fehler:" & vbNewLine & vbNewLine &
                    ballonMessage, nullableControl, nullableControl.Width - 10, nullableControl.Height \ 2, nullableControl.ExceptionBalloonDuration)

    End Sub
End Module
