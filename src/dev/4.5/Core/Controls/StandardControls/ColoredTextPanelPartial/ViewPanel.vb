Imports System.Windows.Forms
Imports System.Drawing

Partial Public Class ColoredTextPanel
    Private Class ViewPanel
        Inherits Panel

        Private myBitmap As Bitmap
        Private myCaret As ViewPanelCaret = New ViewPanelCaret(Me, Me.Font, Me.DesignMode)
        Private myLineElementList As New List(Of LineElement)
        Private mySelectionStart As Point = New Point(-1, -1)
        Private mySelectionEnd As Point = New Point(-1, -1)
        Private WithEvents myContextMenu As System.Windows.Forms.ContextMenuStrip
        Private WithEvents myKopierenToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Private WithEvents myAllesLöschenToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem

        Private myMaxLineCount As Integer
        Public Property MaxLineCount() As Integer
            Get
                Return myMaxLineCount
            End Get
            Set(ByVal value As Integer)
                myMaxLineCount = value
                ReDim myPaintLineElementArray(myMaxLineCount)
            End Set
        End Property

        Private myMaxElementCount As Integer
        Public Property MaxElementCount() As Integer
            Get
                Return myMaxElementCount
            End Get
            Set(ByVal value As Integer)
                myMaxElementCount = value
            End Set
        End Property


        Private myPaintLineElementArray() As LineElement


        ''' <summary>
        ''' Konstruktor
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()
            AddHandler SizeChanged, AddressOf ViewSizeChanged
            'AddHandler MouseDown, AddressOf ViewPanelMouseDown
            'AddHandler MouseMove, AddressOf ViewPanelMouseMove
            'AddHandler MouseUp, AddressOf ViewPanelMouseUp
            Dim b As New Bitmap(5000, 50)
            SetStyle(ControlStyles.OptimizedDoubleBuffer, True)

            AddHandler myCaret.ActivityChanged, AddressOf CaretBlink
            AddHandler myCaret.VisibiltyChanged, AddressOf CaretBlink
            AddHandler myCaret.FontChanged, AddressOf CaretFontChanged
            AddHandler myCaret.PositionChanged, AddressOf CaretPosChanged


            Me.myContextMenu = New System.Windows.Forms.ContextMenuStrip()
            Me.myKopierenToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
            Me.myAllesLöschenToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem

            '
            'ContextMenu
            '
            Me.myContextMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.myKopierenToolStripMenuItem, Me.myAllesLöschenToolStripMenuItem})
            Me.myContextMenu.Name = "ContextMenuStrip1"
            Me.myContextMenu.Size = New System.Drawing.Size(149, 48)
            '
            'KopierenToolStripMenuItem
            '
            Me.myKopierenToolStripMenuItem.Name = "KopierenToolStripMenuItem"
            Me.myKopierenToolStripMenuItem.Size = New System.Drawing.Size(148, 22)
            Me.myKopierenToolStripMenuItem.Text = "Alles kopieren"
            '
            'AllessLöschenToolStripMenuItem
            '
            Me.myAllesLöschenToolStripMenuItem.Name = "AllesLöschenToolStripMenuItem"
            Me.myAllesLöschenToolStripMenuItem.Size = New System.Drawing.Size(148, 22)
            Me.myAllesLöschenToolStripMenuItem.Text = "Alles löschen"

            Me.ContextMenuStrip = myContextMenu

            AddHandler myKopierenToolStripMenuItem.Click, AddressOf ContextMenuKopieren
            AddHandler myAllesLöschenToolStripMenuItem.Click, AddressOf ContextMenuAllesLoeschen

        End Sub

        Private Sub ContextMenuKopieren(ByVal sender As Object, ByVal e As EventArgs)
            CopyToClipboard()
        End Sub

        Private Sub ContextMenuAllesLoeschen(ByVal sender As Object, ByVal e As EventArgs)
            Dim parent As ColoredTextPanel = DirectCast(Me.Parent, ColoredTextPanel)
            parent.Clear()
        End Sub

        Public Sub CopyToClipboard()
            Dim text As String = "<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.01 Transitional//EN"" " & _
       """http://www.w3.org/TR/html4/loose.dtd""> " & _
"<html>" & _
"<head>" & _
"<title></title>" & _
"</head>" & _
"<body>" & _
"<p>"
            Dim parent As ColoredTextPanel = DirectCast(Me.Parent, ColoredTextPanel)
            Dim useIndex As Integer = 0
            SyncLock myLineElementList
                For Each lineElement As LineElement In myLineElementList
                    useIndex += 1
                    If useIndex >= myLineElementList.Count Then
                        useIndex = myLineElementList.Count - 1
                    End If
                    For Each cte As ColorTextElement In lineElement.Elements
                        text += "<font face=""" & cte.Font.Name & """ color=""#" & _
                                Hex(cte.Color.R) & _
                                Hex(cte.Color.G) & _
                                Hex(cte.Color.B) & _
                                """ size=""" & cte.Font.Height & "pt""" & _
                                ">" & cte.Message & "</font>"
                    Next
                Next
            End SyncLock
            text += "</p></body></html>"
            'Console.WriteLine("Copy to Clipboard :'{0}'", text)
            'Clipboard.SetText(text, TextDataFormat.Html)







            '        Clipboard.SetText("Version:0.9" & vbCrLf & _
            '"StartHTML:71" & vbCrLf & _
            '"EndHTML:170" & vbCrLf & _
            '"StartFragment:140" & vbCrLf & _
            '"EndFragment:160" & vbCrLf & _
            '"StartSelection:140" & vbCrLf & _
            '"EndSelection:160" & vbCrLf & _
            '"<!DOCTYPE>" & vbCrLf & _
            '"<HTML>" & vbCrLf & _
            '"<HEAD>" & vbCrLf & _
            '"<TITLE>The HTML Clipboard</TITLE>" & vbCrLf & _
            '"<BASE HREF=""""http://sample/specs""""> " & vbCrLf & _
            '"</HEAD>" & vbCrLf & _
            '"<BODY>" & vbCrLf & _
            '"<!--StartFragment -->" & vbCrLf & _
            '"<P>The Fragment</P>" & vbCrLf & _
            '"<!--EndFragment -->" & vbCrLf & _
            '"</BODY>" & vbCrLf & _
            '"</HTML>" & vbCrLf _
            ', TextDataFormat.Html)
            Dim tmp As String = "Version:0.9" & vbCrLf & "StartHTML:-1" & vbCrLf & "EndHTML:-1" & vbCrLf & "StartFragment:@@@start@@@" & vbCrLf & "EndFragment:@@@end@@@" & vbCrLf
            Dim len As Integer = tmp.Length
            len -= "@@@start@@@".Length - len.ToString.Length
            tmp = tmp.Replace("@@@start@@@", len.ToString)

            Dim eUTF8 As System.Text.Encoding = New System.Text.UTF8Encoding()
            Dim eUNI As System.Text.Encoding = New System.Text.UnicodeEncoding
            text = eUTF8.GetString(System.Text.Encoding.Convert(eUNI, eUTF8, eUNI.GetBytes(text)))

            tmp += "<!DOCTYPE>" & vbCrLf & "<HTML>" & vbCrLf & "<BODY>" & vbCrLf & text.Normalize & vbCrLf & "</HTML>"
            len = tmp.Length
            len -= "@@@end@@@".Length - len.ToString.Length
            tmp = tmp.Replace("@@@end@@@", len.ToString)
            Clipboard.SetText(tmp, TextDataFormat.Html)
        End Sub

        'Private Property SelectionStart() As Point
        '    Get
        '        Return mySelectionStart
        '    End Get
        '    Set(ByVal value As Point)
        '        Debug.WriteLine("Selection Start=" + value.ToString)
        '        mySelectionStart = value
        '    End Set
        'End Property

        'Private Property SelectionEnd() As Point
        '    Get
        '        Return mySelectionEnd
        '    End Get
        '    Set(ByVal value As Point)
        '        Debug.WriteLine("Selection End=" + value.ToString)
        '        mySelectionEnd = value
        '    End Set
        'End Property

        'Private Function TransformToScreen(ByVal p As Point) As Point
        '    Dim p As ColoredTextBox
        '    p = DirectCast(Parent, ColoredTextBox)
        '    Return New Point(p.X - p.myHScrollBar.Value, p.Y - p.myVScrollBar.Value)
        'End Function

        Private Sub CaretBlink(ByVal sender As Object, ByVal e As EventArgs)
            'Dim p As Point = TransformToScreen(myCaret.Position)
            'Invalidate(New Region(New Rectangle(p.X, p.Y, myCaret.Size.Width, myCaret.Size.Height)))
            ViewRedraw(New Rectangle(myCaret.Position.X, myCaret.Position.Y, myCaret.Size.Width, myCaret.Size.Height))
        End Sub

        Private Sub CaretFontChanged(ByVal sender As Object, ByVal e As ViewPanelCaret.ViewPanelCaretFontChangedArgs)
            'Dim p As Point
            If e.OldFont IsNot Nothing Then
                'p = TransformToScreen(myCaret.Position)
                'Invalidate(New Region(New Rectangle(p.X, p.Y, myCaret.Size.Width, e.OldFont.Height)))
                ViewRedraw(New Rectangle(myCaret.Position.X, myCaret.Position.Y, myCaret.Size.Width, e.OldFont.Height))
            End If
            'p = TransformToScreen(myCaret.Position)
            'Invalidate(New Region(New Rectangle(p.X, p.Y, myCaret.Size.Width, e.NewFont.Height)))
            ViewRedraw(New Rectangle(myCaret.Position.X, myCaret.Position.Y, myCaret.Size.Width, e.NewFont.Height))
        End Sub

        Private Sub CaretPosChanged(ByVal sender As Object, ByVal e As ViewPanelCaret.ViewPanelCharetPositionChangedArgs)
            'Dim p As Point
            'p = TransformToScreen(e.OldPos)
            'Invalidate(New Region(New Rectangle(p.X, p.Y, myCaret.Size.Width, myCaret.Size.Width)))
            ViewRedraw(New Rectangle(e.OldPos.X, e.OldPos.Y, myCaret.Size.Width, myCaret.Size.Height))
            'p = TransformToScreen(e.NewPos)
            'Invalidate(New Region(New Rectangle(p.X, p.Y, myCaret.Size.Width, myCaret.Size.Width)))
            ViewRedraw(New Rectangle(e.NewPos.X, e.NewPos.Y, myCaret.Size.Width, myCaret.Size.Height))
        End Sub

        Public ReadOnly Property LineElementList() As List(Of LineElement)
            Get
                Return myLineElementList
            End Get
            'Set(ByVal value As List(Of LineElement))
            '    myLineElementList = value
            'End Set
        End Property

        Public ReadOnly Property PanelCaret() As ViewPanelCaret
            Get
                Return myCaret
            End Get
        End Property



        ''' <summary>
        ''' Den Text per OnPaint ausgeben 
        ''' </summary>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
            If IsDisposed Then
                Return
            End If

            Dim parent As ColoredTextPanel = DirectCast(Me.Parent, ColoredTextPanel)

            '           Dim g As Graphics = Graphics.FromImage(myBitmap)
            Dim g As Graphics = e.Graphics
            g.Clear(BackColor)
            Dim index As Integer = 0

            Dim lineIndex As Integer = 0
            Dim lineCount As Integer = 0
            SyncLock (myLineElementList)
                myLineElementList.CopyTo(myPaintLineElementArray)
                lineCount = myLineElementList.Count
            End SyncLock

            Dim lineElement As LineElement
            For lineIndex = 0 To lineCount - 1
                lineElement = myPaintLineElementArray(lineIndex)
                Dim useIndex As Integer = index + 1
                If useIndex >= lineCount Then
                    useIndex = lineCount - 1
                End If
                If myPaintLineElementArray(useIndex).Y >= parent.myVScrollBar.Value + e.ClipRectangle.Top AndAlso lineElement.Y < parent.myVScrollBar.Value + e.ClipRectangle.Bottom Then
                    For Each cte As ColorTextElement In lineElement.Elements
                        ' nur wenn es nicht zu hoch ist
                        If cte.Position.Y + cte.Measure.Height > parent.myVScrollBar.Value + e.ClipRectangle.Top Then
                            ' nur wenn es nicht zu tief ist
                            If cte.Position.Y < parent.myVScrollBar.Value + e.ClipRectangle.Bottom Then
                                'nur wenn es nicht zu weit links ist
                                If cte.Position.X + cte.Measure.Width > parent.myHScrollBar.Value + e.ClipRectangle.Left Then
                                    'nur wenn es nicht zu weit rechts ist
                                    If cte.Position.X < parent.myHScrollBar.Value + e.ClipRectangle.Right Then
                                        ' Text anzeigen
                                        ' Position ermitteln
                                        'Dim p As Point = TransformToScreen(cte.Position)
                                        Dim x As Integer = cte.Position.X - parent.myHScrollBar.Value
                                        Dim y As Integer = cte.Position.Y - parent.myVScrollBar.Value


                                        'Dim selectionColor As Color = Color.Blue
                                        'If cte.Position.Y >= SelectionStart.Y AndAlso cte.Position.Y <= mySelectionEnd.Y Then
                                        '    'Console.WriteLine("CTE in Selektion y={0}", cte.Position.Y)
                                        '    Dim rect As Rectangle
                                        '    If cte.Position.Y = SelectionStart.Y AndAlso cte.Position.Y = SelectionEnd.Y Then
                                        '        rect = New Rectangle(SelectionStart.X, cte.Position.Y, SelectionEnd.X - SelectionStart.X, cte.Measure.Height)
                                        '    ElseIf cte.Position.Y = SelectionStart.Y Then
                                        '        rect = New Rectangle(SelectionStart.X, cte.Position.Y, cte.Measure.Width - SelectionStart.X, cte.Measure.Height)
                                        '    ElseIf cte.Position.Y = SelectionEnd.Y Then
                                        '        rect = New Rectangle(cte.Position, New Size(SelectionEnd.X - cte.Position.X, cte.Measure.Height))
                                        '    Else
                                        '        rect = New Rectangle(cte.Position, cte.Measure)
                                        '    End If
                                        '    g.FillRectangle(New SolidBrush(selectionColor), rect)
                                        'End If

                                        TextRenderer.DrawText(g, cte.Message, cte.Font, New Point(x, y), cte.Color, TextFormatFlags.NoPadding)
                                        'For Each i As Integer In cte.CharPositions
                                        '    g.DrawLine(Pens.Blue, x + i, y + cte.Font.Height + 2, x + i, y + cte.Font.Height + 4)
                                        'Next
                                    End If
                                End If
                            End If
                        End If
                    Next
                End If
                index += 1
            Next

            If myCaret IsNot Nothing AndAlso myCaret.Visible AndAlso myCaret.Active Then
                'Console.WriteLine("1:" + (myCaret.Position.Y >= parent.myVScrollBar.Value).ToString)
                'Console.WriteLine("2:" + (myCaret.Position.Y < parent.myVScrollBar.Value + Height).ToString)
                'Console.WriteLine("3:" + (myCaret.Position.X >= parent.myHScrollBar.Value).ToString)
                'Console.WriteLine("4:" + (myCaret.Position.X < parent.myHScrollBar.Value + Width).ToString)
                If myCaret.Position.Y >= parent.myVScrollBar.Value AndAlso myCaret.Position.Y < parent.myVScrollBar.Value + Height _
                AndAlso myCaret.Position.X >= parent.myHScrollBar.Value AndAlso myCaret.Position.X < parent.myHScrollBar.Value + Width Then
                    'draw Cursor
                    'Console.WriteLine("Active: {0}", myCaret.Active)
                    g.FillRectangle(New SolidBrush(myCaret.Color), New Rectangle(myCaret.Position.X - parent.myHScrollBar.Value, myCaret.Position.Y - parent.myVScrollBar.Value, myCaret.Size.Width, myCaret.Size.Height))
                End If
            End If

            '            e.Graphics.DrawImageUnscaled(myBitmap, New Point(0, 0))

        End Sub

        ''' <summary>
        ''' OnPaintBackground soll nichts machen
        ''' </summary>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Overrides Sub OnPaintBackground(ByVal e As System.Windows.Forms.PaintEventArgs)
            'MyBase.OnPaintBackground(e)
        End Sub

        ''' <summary>
        ''' Bei einer Größenänderung des ViewPanels muß der Inhalt neu gezeichnet werden
        ''' Ebenso müssen die Scrollbars angepasst werden
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Private Sub ViewSizeChanged(ByVal sender As Object, ByVal e As EventArgs)
            Try
                If myBitmap IsNot Nothing Then
                    myBitmap.Dispose()
                    'GC.Collect()
                End If
                myBitmap = New Bitmap(Width, Height)
                ' Scrollbars anpassen
                Dim parent As ColoredTextPanel = DirectCast(Me.Parent, ColoredTextPanel)
                parent.ViewHeight = parent.ViewHeight
                parent.ViewWidth = parent.ViewWidth

                Refresh()
            Catch ex As Exception
            End Try
        End Sub

        Public Sub ViewRedraw(ByVal rect As Rectangle)
            If Me.IsDisposed Then
                Return
            End If

            If Not Me.Visible Then
                Return
            End If

            Dim parent As ColoredTextPanel = DirectCast(Me.Parent, ColoredTextPanel)
            'Console.WriteLine("a: {0}", rect.Bottom >= parent.myVScrollBar.Value)
            'Console.WriteLine("b: {0}", rect.Top <= parent.myVScrollBar.Value + Height)
            'Console.WriteLine("c: {0}", rect.Left >= parent.myHScrollBar.Value)
            'Console.WriteLine("d: {0}", rect.Right <= parent.myHScrollBar.Value + Width)
            If rect.Bottom >= parent.myVScrollBar.Value AndAlso rect.Top <= parent.myVScrollBar.Value + Height _
                    AndAlso rect.Left >= parent.myHScrollBar.Value AndAlso rect.Right <= parent.myHScrollBar.Value + Width Then
                Invalidate(New Rectangle(rect.Left - parent.myHScrollBar.Value, rect.Top - parent.myVScrollBar.Value, rect.Width, rect.Height))
            End If
        End Sub

        'Public Sub ViewPanelMouseDown(ByVal sender As Object, ByVal e As MouseEventArgs)
        '    ViewPanelMouseHandler(True, False, False, e)
        'End Sub

        'Public Sub ViewPanelMouseMove(ByVal sender As Object, ByVal e As MouseEventArgs)
        '    ViewPanelMouseHandler(False, True, False, e)
        'End Sub

        'Public Sub ViewPanelMouseUp(ByVal sender As Object, ByVal e As MouseEventArgs)
        '    ViewPanelMouseHandler(False, False, True, e)
        'End Sub

        'Public Sub ViewPanelMouseHandler(ByVal mouseDown As Boolean, ByVal mouseMove As Boolean, ByVal mouseUp As Boolean, ByVal e As MouseEventArgs)
        '    Return
        '    Dim parent As ColoredTextBox = DirectCast(Me.Parent, ColoredTextBox)
        '    Dim index As Integer = 0
        '    Dim x As Integer = -1
        '    Dim y As Integer = -1
        '    Dim colorTextElement As ColorTextElement = Nothing

        '    For Each lineElement As LineElement In myLineElementList
        '        Dim useIndex As Integer = index + 1
        '        If useIndex >= myLineElementList.Count Then
        '            useIndex = myLineElementList.Count - 1
        '        End If
        '        If myLineElementList(useIndex).Y >= parent.myVScrollBar.Value + e.Location.Y Then
        '            For Each cte As ColorTextElement In lineElement.Elements
        '                If cte.Position.Y <= parent.myVScrollBar.Value + e.Location.Y Then
        '                    If cte.Position.X <= parent.myHScrollBar.Value + e.Location.X Then
        '                        If cte.Position.X + cte.Measure.Width > parent.myHScrollBar.Value + e.Location.X Then
        '                            ' Text anzeigen
        '                            ' Position ermitteln
        '                            y = cte.Position.Y

        '                            Dim i As Integer = 0
        '                            Dim tmp As Integer = 0
        '                            ' das richtige Zeichen finden
        '                            While cte.Position.X + tmp < parent.myHScrollBar.Value + e.Location.X
        '                                x = tmp
        '                                tmp = cte.CharPositions(i)
        '                                'Console.WriteLine("X={0},MouseX={1},i={2}", tmp + cte.Position.X, parent.myHScrollBar.Value + e.Location.X, i)
        '                                i += 1
        '                                If i > cte.Message.Length Then
        '                                    x = tmp
        '                                    Exit While
        '                                End If
        '                            End While
        '                            colorTextElement = cte
        '                            Exit For
        '                        End If
        '                    End If
        '                End If
        '            Next
        '        End If
        '        index += 1
        '        If (y <> -1 And x <> -1) Then
        '            Exit For
        '        End If
        '    Next
        '    If colorTextElement IsNot Nothing Then
        '        If (e.Button = Windows.Forms.MouseButtons.Left AndAlso mouseDown) Then
        '            myCaret.Font = colorTextElement.Font
        '            myCaret.Position = New Point(x, y)
        '            myCaret.Visible = True
        '            SelectionStart = myCaret.Position
        '            SelectionEnd = myCaret.Position
        '            Refresh()
        '        End If
        '        If (e.Button = Windows.Forms.MouseButtons.Left AndAlso mouseMove) Then
        '            If y <= myCaret.Position.Y Then
        '                If x > myCaret.Position.X AndAlso y = myCaret.Position.Y Then
        '                    SelectionStart = myCaret.Position
        '                    SelectionEnd = New Point(x, y)
        '                Else
        '                    SelectionStart = New Point(x, y)
        '                    SelectionEnd = myCaret.Position
        '                End If
        '            Else
        '                SelectionStart = myCaret.Position
        '                SelectionEnd = New Point(x, y)
        '            End If
        '            Refresh()
        '        End If
        '    End If
        'End Sub

    End Class


End Class
