Imports System.Windows.Forms
Imports System.Drawing.Imaging
Imports System.ComponentModel
Imports System.Drawing

''' <summary>
''' Stellt ein Control bereit mit dem Text farbig ausgegeben werden kann
''' </summary>
''' <remarks></remarks>
Partial Public Class ColoredTextPanel
    Inherits Panel

    Private Delegate Sub DelegateWriteLine(ByVal msg As String)
    Private Delegate Sub DelegateDoWrite(ByVal writeStruc As WriterStructure)
    Private Delegate Sub DelegateNoParam()
    Private Delegate Sub SetViewWidthOrHeight(ByVal newVal As Integer)

    Private myHScrollBar As New System.Windows.Forms.HScrollBar With {.Name = "myHScrollBar"}
    Private myVScrollBar As New System.Windows.Forms.VScrollBar With {.Name = "myVScrollBar"}
    Private myViewPanel As New ViewPanel
    Private myViewHeight As Integer = 0
    Private myViewWidth As Integer = 0
    Private myCurrentDrawFont As Font = Me.Font
    Private myCurrentDrawColor As Color = Color.Black
    Private myLastDrawPoint As New Point(0, 0)
    Private myLastLineElementIndex As Integer = 0
    Private myAutoScrollToEnd As Boolean = False
    Private myElementManager As ColoredTextElementManager
    Private myLineManager As LineElementManager
    Private myWriterManager As New WriterStructureManager(1000)
    Private myWriterQueueLock As New Object
    Private myWriterQueue As New Queue(Of WriterStructure)(1000)
    Private myWriterThread As Threading.Thread
    Private myWriterControl As New Control
    Private myWriterControlgraphics As Graphics

    Public Delegate Sub CurrentDrawColorChangedHandler(ByVal sender As Object, ByVal e As EventArgs)
    Public Event CurrentDrawColorChanged As CurrentDrawColorChangedHandler

    Public Delegate Sub CurrentDrawFontChangedHandler(ByVal sender As Object, ByVal e As EventArgs)
    Public Event CurrentDrawFontChanged As CurrentDrawFontChangedHandler

    Private Shared myDefaultColoredElementCapacity As Integer = 5000
    ''' <summary>
    ''' liefert zurück oder legt fest, wieviel ColoredElemente in einer neuen Instanz von ColoredTextBox verwendet werden sollen.
    ''' </summary>
    ''' <value>Anzahl ALLER in der ColoredTextBox nutzbaren ColoredElemente</value>
    ''' <returns></returns>
    ''' <remarks>siehe auch DefaultLineCapacity.
    ''' sind keine freien Elemente mehr vorhanden, wird die erste Zeile der ColoredTextBox gelöscht und die so gewonnenen ColoredElemente wiederverwendet.</remarks>
    Public Shared Property DefaultColoredElementCapacity() As Integer
        Get
            Return myDefaultColoredElementCapacity
        End Get
        Set(ByVal value As Integer)
            myDefaultColoredElementCapacity = value
        End Set
    End Property

    Private Shared myDefaultLineCapacity As Integer = 1000
    ''' <summary>
    ''' liefert zurück oder legt fest, wieviele Zeilen in der ColoredTextBox verwendet werden können, ohne eine Zeile löschen zu müssen.
    ''' </summary>
    ''' <value>Anzahl der Zeilen, nach denen die ersten Zeilen wieder gelöscht werden</value>
    ''' <returns></returns>
    ''' <remarks>siehe auch DefaultColoredElementCapacity.
    ''' sind keine freien Zeilen mehr vorhanden, wird die erste Zeile gelöscht.</remarks>
    Public Shared Property DefaultLineCapacity() As Integer
        Get
            Return myDefaultLineCapacity
        End Get
        Set(ByVal value As Integer)
            myDefaultLineCapacity = value
        End Set
    End Property


    ''' <summary>
    ''' Initialisiert die Instanz.
    ''' für ColoredElementCapacity und LineCapacity werden die jeweiligen statischen DefaultXXX Werte verwendet.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
        Init(myDefaultColoredElementCapacity, myDefaultLineCapacity)
    End Sub

    ''' <summary>
    ''' Initialisiert die Instanz.
    ''' Die ColoredElementCapacity und LineCapacity können hier explizit angegeben werden (siehe im Gegensatz auch die jeweiligen statischen DefaultXXX Werte).
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New(ByVal coloredElementCounter As Integer, ByVal lineCounter As Integer)
        Init(coloredElementCounter, lineCounter)
    End Sub

    Private Sub Init(ByVal coloredElementCapacity As Integer, ByVal lineCapacity As Integer)
        Controls.Add(myHScrollBar)
        Controls.Add(myVScrollBar)
        Controls.Add(myViewPanel)
        myHScrollBar.Dock = DockStyle.Bottom
        myVScrollBar.Dock = DockStyle.Right
        myViewPanel.Dock = DockStyle.Fill

        myHScrollBar.Visible = False
        AddHandler myHScrollBar.Scroll, AddressOf ViewScroll

        myVScrollBar.Visible = False
        AddHandler myVScrollBar.Scroll, AddressOf ViewScroll

        myViewHeight = myViewPanel.Height
        myViewWidth = myViewPanel.Width

        myElementManager = New ColoredTextElementManager(coloredElementCapacity)
        myLineManager = New LineElementManager(lineCapacity)
        'myWriterThread.Start()
        myViewPanel.MaxElementCount = coloredElementCapacity
        myViewPanel.MaxLineCount = lineCapacity
    End Sub

    Protected Overrides Sub OnHandleCreated(e As System.EventArgs)
        MyBase.OnHandleCreated(e)
        If Not Me.DesignMode Then
            myWriterThread = New Threading.Thread(AddressOf WriterThreadRun) With {.IsBackground = True, .Name = "ColoredTextBoxWriter-Thread"}
            myWriterThread.Start()
        End If
    End Sub

    ''' <summary>
    ''' Ruft den aktuellen Font für die WriteLine Methode ab oder liegt diesen fest
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Category("WriteLine-Eigenschaften"), Description("Legt den Font für die WriteLine-Methode fest")> _
    Public Property CurrentDrawFont() As Font
        Get
            Return myCurrentDrawFont
        End Get
        Set(ByVal value As Font)
            myCurrentDrawFont = value
            RaiseEvent CurrentDrawColorChanged(Me, New EventArgs())
        End Set
    End Property

    ''' <summary>
    ''' Legt die aktuelle Farbe für die WriteLine Methode ab oder liegt diese fest
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Category("WriteLine-Eigenschaften"), Description("Legt die Farbe für die WriteLine-Methode fest")> _
    Public Property CurrentDrawColor() As Color
        Get
            Return myCurrentDrawColor
        End Get
        Set(ByVal value As Color)
            myCurrentDrawColor = value
            RaiseEvent CurrentDrawColorChanged(Me, New EventArgs())
        End Set
    End Property

    ''' <summary>
    ''' Gibt eine Textzeile in der per CurrentDrawColor festgelegten Farbe und mit der per CurrentDrawFont festgelegten Font aus
    ''' </summary>
    ''' <param name="msg"></param>
    ''' <remarks></remarks>
    Public Sub WriteLine(ByVal msg As String)
        Dim msgList = msg.Split(CChar(vbCr))
        If msgList.Count > 1 Then
            For Each singleLine In msgList
                WriteLine(singleLine.Replace(vbLf, ""))
            Next
            Return
        End If
        Dim writerEl = myWriterManager.GetFreeObject
        writerEl.Msg = msg
        writerEl.Font = CurrentDrawFont
        writerEl.Color = CurrentDrawColor

        SyncLock myWriterQueueLock
            myWriterQueue.Enqueue(writerEl)
        End SyncLock

    End Sub

    ''' <summary>
    ''' Ruft die Weite des Views ab oder legt diese fest
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Property ViewWidth() As Integer
        Get
            Return myViewWidth
        End Get
        Set(ByVal value As Integer)
            SetViewWidth(value)
        End Set
    End Property

    Private Sub SetViewWidth(ByVal newVal As Integer)
        If InvokeRequired Then
            Dim d As SetViewWidthOrHeight = AddressOf SetViewWidth
            Invoke(d, newVal)
            Return
        End If
        myViewWidth = newVal
        'HScrollBar anzeigen ? Hierzu die Höhe von VScrollbar beachten
        If ((myVScrollBar.Visible = False AndAlso myViewWidth > myViewPanel.Width) Or (myVScrollBar.Visible AndAlso myViewWidth > myViewPanel.Width - myVScrollBar.Width)) Then
            ' HScrollbar muss angezeigt werden 
            myHScrollBar.Visible = True
            myHScrollBar.Maximum = myViewWidth
            If myVScrollBar.Visible Then
                myHScrollBar.Maximum += myVScrollBar.Width
            End If
        Else
            ' HScrollbar ist nicht notwendig
            myHScrollBar.Value = 0
            myHScrollBar.Visible = False
        End If
        myHScrollBar.LargeChange = myViewPanel.Width
    End Sub

    ''' <summary>
    ''' Ruft die Höhe des Views ab oder legt diese fest
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Property ViewHeight() As Integer
        Get
            Return myViewHeight
        End Get
        Set(ByVal value As Integer)
            SetViewHeigth(value)
        End Set
    End Property

    Private Sub SetViewHeigth(ByVal newVal As Integer)
        If InvokeRequired Then
            Dim d As SetViewWidthOrHeight = AddressOf SetViewHeigth
            Invoke(d, newVal)
            Return
        End If
        myViewHeight = newVal
        'VScrollBar anzeigen ? Hierzu die Höhe von HScrollbar beachten
        If ((myHScrollBar.Visible = False AndAlso myViewHeight > myViewPanel.Height) Or (myHScrollBar.Visible AndAlso myViewHeight > myViewPanel.Height - myHScrollBar.Height)) Then
            ' VScrollbar anzeigen
            myVScrollBar.Visible = True
            myVScrollBar.Maximum = myViewHeight
            If myHScrollBar.Visible Then
                myVScrollBar.Maximum += myHScrollBar.Height
            End If
        Else
            ' VScrollbar nicht nötig
            myVScrollBar.Value = 0
            myVScrollBar.Visible = False
        End If
        myVScrollBar.LargeChange = myViewPanel.Height
        '            Console.WriteLine("value {0}, max {1}, Large {2}", myVScrollBar.Value, myVScrollBar.Maximum, myVScrollBar.LargeChange)
    End Sub

    ''' <summary>
    ''' Löscht alle Einträge der Textbox
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Clear()
        If InvokeRequired Then
            Dim d As DelegateNoParam = AddressOf Clear
            Invoke(d)
            Return
        End If
        SyncLock myViewPanel.LineElementList
            For Each line In myViewPanel.LineElementList
                line.Recycle()
            Next
            myViewPanel.LineElementList.Clear()
            myLastLineElementIndex = 0
            ' Point auf 0,0 setzen
            myLastDrawPoint.X -= myLastDrawPoint.X
            myLastDrawPoint.Y -= myLastDrawPoint.Y
            ViewWidth = 0
            ViewHeight = 0
        End SyncLock
        myViewPanel.Refresh()
    End Sub

    ''' <summary>
    ''' ViewScroll wird aufgrufen wenn vertikal oder horizontal gescrollt wird
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ViewScroll(ByVal sender As Object, ByVal e As ScrollEventArgs)
        'Debug.WriteLine("ViewScroll ausgelöst durch " + DirectCast(sender, Control).Name + " old/new Vals" + e.OldValue.ToString + "/" + e.NewValue.ToString)
        myViewPanel.Refresh()
    End Sub

    ''' <summary>
    ''' Mit AutoScrollToEnd wird festgelegt, ob Automatisch bei einem WriteLine der View zum Ende gescrollt werden soll
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Category("Behavior"), Description("Steuert, ob bei einem WriteLine automatisch zu der ausgegebenen Zeile gesprungen werden soll")> _
    Public Property AutoScrollToEnd() As Boolean
        Get
            Return myAutoScrollToEnd
        End Get
        Set(ByVal value As Boolean)
            myAutoScrollToEnd = value
        End Set
    End Property

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

    Private myDeleteFirstElementOnNoFreeElement As Boolean = True

    ''' <summary>
    ''' Liefert oder legt fest, ob bei "Platzproblemen" (siehe Remark) die erste Zeile gelöscht werden soll.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>
    ''' Falls kein Element mehr frei ist, wird die jeweils erste Zeile der Ausgabe gelöscht
    ''' Dieses kann der Fall sein, falls der LineManager keine freien Zeilen mehr hat
    ''' oder der ColoredElementManager keine freien Einträge mehr liefern kann
    ''' </remarks>
    <Category("Behavior"),
     Description("Steuert, ob bei 'Speicherplatzproblemen(siehe Remark)' automatisch die ersten Zeilen gelöscht werden sollen.")>
    Public Property DeleteFirstElementOnNoFreeElement() As Boolean
        Get
            Return myDeleteFirstElementOnNoFreeElement
        End Get
        Set(ByVal value As Boolean)
            myDeleteFirstElementOnNoFreeElement = value
        End Set
    End Property

    Private Sub WriterThreadRun()
        myWriterControlgraphics = Graphics.FromHwnd(myWriterControl.Handle)
        Try
            Dim wait As Boolean = False
            Dim newViewWidth As Integer = -1
            Dim newViewHeight As Integer = -1
            Dim curViewWidth As Integer = -1
            Dim curViewHeight As Integer = -1
            Dim newVScrollbarVal As Integer = -1
            While True
                wait = False
                ' Aktuelle View-Groesse merken
                curViewHeight = ViewHeight
                curViewWidth = ViewWidth
                newViewWidth = curViewWidth
                newViewHeight = curViewHeight
                newVScrollbarVal = -1

                SyncLock myWriterQueueLock
                    If myWriterQueue.Count > 0 Then
                        Dim writeEl = myWriterQueue.Dequeue
                        Try
                            doWrite(writeEl, newViewWidth, newViewHeight, newVScrollbarVal)
                        Catch ex As Exception
                            Exit While
                        End Try
                    Else
                        wait = True
                    End If
                End SyncLock
                If newViewHeight <> curViewHeight Then
                    ViewHeight = newViewHeight
                End If
                If newViewWidth <> curViewWidth Then
                    ViewWidth = newViewWidth
                End If
                If newVScrollbarVal <> -1 Then
                    Try
                        SetScrollbarValue(myVScrollBar, newVScrollbarVal)
                    Catch ex As Exception
                        Exit While
                    End Try
                End If
                If wait Then
                    'GC.Collect()
                    Threading.Thread.Sleep(100)
                End If
            End While
        Finally
            myWriterControlgraphics.Dispose()
        End Try
    End Sub

    Private Sub doWrite(ByVal writeEl As WriterStructure, ByRef curViewWidth As Integer, ByRef curViewHeight As Integer, ByRef newVScrollBarVal As Integer)

        'If InvokeRequired Then
        '    Dim d As DelegateDoWrite = AddressOf doWrite
        '    Invoke(d, writeEl)
        '    Return
        'End If
        Dim msg = writeEl.Msg
        Dim font = writeEl.Font
        Dim color = writeEl.Color


        ' Textgrösse ermitteln
        Dim textSize As SizeF = TextRenderer.MeasureText(myWriterControlgraphics, msg, font, New Size(5000, 100), TextFormatFlags.NoPadding)

        Dim newColorTextElement As ColorTextElement = Nothing
        Try
            newColorTextElement = myElementManager.GetFreeObject()
        Catch nfo As NoFreeObjectException
            If DeleteFirstElementOnNoFreeElement Then
                SyncLock myViewPanel.LineElementList
                    ' nichts mehr frei -> also 1ste Zeile löschen
                    RecycleFirstRow()
                    newColorTextElement = myElementManager.GetFreeObject()
                End SyncLock
            Else
                Throw nfo
            End If
        End Try
        newColorTextElement.Init(myLastDrawPoint, font, color, msg)
        Dim lineElement As LineElement = Nothing
        SyncLock myViewPanel.LineElementList
            If myLastLineElementIndex = myViewPanel.LineElementList.Count Then
                ' es gibt noch kein Element
                ' eins anlegen
                Try
                    lineElement = myLineManager.GetFreeObject()
                Catch nfo As NoFreeObjectException
                    If DeleteFirstElementOnNoFreeElement Then
                        RecycleFirstRow()
                        lineElement = myLineManager.GetFreeObject()
                    Else
                        Throw nfo
                    End If
                End Try
                lineElement.Init(myLastDrawPoint.Y, New List(Of ColorTextElement))
                myViewPanel.LineElementList.Add(lineElement)
            ElseIf myLastLineElementIndex < myViewPanel.LineElementList.Count Then
                lineElement = myViewPanel.LineElementList(myLastLineElementIndex)
            End If
            lineElement.Elements.Add(newColorTextElement)
            newColorTextElement.Measure = TextRenderer.MeasureText(myWriterControlgraphics, newColorTextElement.Message, newColorTextElement.Font, New Size(100, 100), TextFormatFlags.NoPadding)
            For i As Integer = 0 To newColorTextElement.Message.Length
                newColorTextElement.CharPositions.Add(TextRenderer.MeasureText(myWriterControlgraphics, newColorTextElement.Message.Substring(0, i), newColorTextElement.Font, New Size(100, 100), TextFormatFlags.NoPadding).Width)
            Next

            myLastLineElementIndex += 1
        End SyncLock

        Dim textWidth As Double = textSize.Width
        Dim textHeight As Double = textSize.Height
        If textHeight = 0 Then
            textHeight = font.Height
        End If
        myLastDrawPoint = New Point(0, myLastDrawPoint.Y + CInt(textHeight))
        Dim tmpX As Integer = newColorTextElement.Position.X + CInt(textWidth)

        'View erweitern !?
        If tmpX > curViewWidth Then
            curViewWidth = tmpX
        End If
        If myLastDrawPoint.Y > curViewHeight Then
            curViewHeight = myLastDrawPoint.Y
        End If

        'Ans Ende scrollen ?
        If AutoScrollToEnd AndAlso myVScrollBar.Visible Then
            Dim y As Integer = 0
            If (myHScrollBar.Visible) Then
                y = myHScrollBar.Height
            End If
            newVScrollBarVal = curViewHeight + y - myVScrollBar.LargeChange
            'Debug.WriteLine("doWrite - > newVScrollbarPos =" + newVScrollBarVal.ToString)
        End If
        writeEl.Recycle()
    End Sub

    Private Delegate Sub SetScrollbarValueDelegate(ByVal sbar As ScrollBar, ByVal val As Integer)

    Private Sub SetScrollbarValue(ByVal sbar As ScrollBar, ByVal val As Integer)
        If InvokeRequired Then
            Dim d As SetScrollbarValueDelegate = AddressOf SetScrollbarValue
            Invoke(d, sbar, val)
            Return
        End If
        sbar.Value = val
        myViewPanel.Refresh()
    End Sub

    Private Sub RecycleFirstRow()
        SyncLock myViewPanel.LineElementList
            ' nichts mehr frei -> also 1ste Zeile löschen
            Dim firstRowHeight As Integer = 0
            For Each cte As ColorTextElement In myViewPanel.LineElementList(0).Elements
                If cte.Font.Height > firstRowHeight Then
                    firstRowHeight = cte.Font.Height
                End If
            Next
            For Each row As LineElement In myViewPanel.LineElementList
                For Each cte As ColorTextElement In row.Elements
                    cte.Position = New Point(cte.Position.X, cte.Position.Y - firstRowHeight)
                Next
                row.Y -= firstRowHeight
            Next
            myViewPanel.LineElementList(0).Recycle()
            myViewPanel.LineElementList.RemoveAt(0)
            myLastLineElementIndex -= 1
            myLastDrawPoint = New Point(myLastDrawPoint.X, myLastDrawPoint.Y - firstRowHeight)
        End SyncLock
    End Sub

    Public Sub CopyToClipboard()
        myViewPanel.CopyToClipboard()
    End Sub

End Class
