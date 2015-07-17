Imports System.Drawing
Imports System.Windows.Forms

Partial Public Class ColoredTextPanel
    Private Class ViewPanelCaret

        Public Class ViewPanelCaretFontChangedArgs
            Private myNewFont As Font
            Private myOldFont As Font

            Public Sub New(ByVal oldFont As Font, ByVal newFont As Font)
                myNewFont = newFont
                myOldFont = oldFont
            End Sub

            Public ReadOnly Property NewFont() As Font
                Get
                    Return myNewFont
                End Get
            End Property

            Public ReadOnly Property OldFont() As Font
                Get
                    Return myOldFont
                End Get
            End Property
        End Class

        Public Class ViewPanelCharetPositionChangedArgs
            Private myOldPos As Point
            Private myNewPos As Point
            Public Sub New(ByVal oldPos As Point, ByVal newPos As Point)
                myOldPos = oldPos
                myNewPos = newPos
            End Sub

            Public ReadOnly Property OldPos() As Point
                Get
                    Return myOldPos
                End Get
            End Property

            Public ReadOnly Property NewPos() As Point
                Get
                    Return myNewPos
                End Get
            End Property
        End Class

        Private myPos As Point
        Private myFont As Font
        Private mySize As Size
        Private myColor As Color
        Private myVisible As Boolean = False
        Private myParent As ViewPanel
        Private myBIsActive As Boolean = True
        Private myTimer As New Timer

        Public Event FontChanged(ByVal sender As Object, ByVal e As ViewPanelCaretFontChangedArgs)
        Public Event PositionChanged(ByVal sender As Object, ByVal e As ViewPanelCharetPositionChangedArgs)
        Public Event VisibiltyChanged(ByVal sender As Object, ByVal e As EventArgs)
        Public Event ActivityChanged(ByVal sender As Object, ByVal e As EventArgs)

        Public Sub New(ByVal parent As ViewPanel, ByVal font As Font, ByVal designMode As Boolean)
            myPos = New Point(0, 0)
            myColor = Color.Black
            myParent = parent
            If Not designMode Then
                myTimer.Interval = 500
                AddHandler myTimer.Tick, AddressOf TimerTick
                myTimer.Enabled = True
            End If
        End Sub

        Public Property Font() As Font
            Get
                Return myFont
            End Get
            Set(ByVal value As Font)
                Dim oldFont As Font = myFont
                Dim oldSize As Size = mySize
                myFont = value
                If (myFont.Italic) Then
                    mySize = New Size(3, myFont.Height)
                Else
                    mySize = New Size(1, myFont.Height)
                End If
                RaiseEvent FontChanged(Me, New ViewPanelCaretFontChangedArgs(oldFont, myFont))
                'TODO: Redraw(myPos, oldSize)
                'TODO: Redraw(myPos, mySize)
            End Set
        End Property

        Public ReadOnly Property Size() As Size
            Get
                Return mySize
            End Get
        End Property

        Public Property Position() As Point
            Get
                Return myPos
            End Get
            Set(ByVal value As Point)
                Dim oldPos As Point = myPos
                myPos = value
                RaiseEvent PositionChanged(Me, New ViewPanelCharetPositionChangedArgs(oldPos, myPos))
            End Set
        End Property

        Public Property Color() As Color
            Get
                Return myColor
            End Get
            Set(ByVal value As Color)
                myColor = value
            End Set
        End Property

        Public Property Visible() As Boolean
            Get
                Return myVisible
            End Get
            Set(ByVal value As Boolean)
                myVisible = value
                RaiseEvent VisibiltyChanged(Me, New EventArgs())
                'TODO: Redraw(myPos, mySize)
            End Set
        End Property

        Private Sub TimerTick(ByVal sender As Object, ByVal e As EventArgs)
            SyncLock (Me)
                myBIsActive = Not myBIsActive
            End SyncLock
            RaiseEvent ActivityChanged(Me, New EventArgs)
            'TODO: Redraw(myPos, mySize)
            'Console.WriteLine("Timed-Redraw of {0}", myPos.ToString)
        End Sub

        Public ReadOnly Property Active() As Boolean
            Get
                SyncLock (Me)
                    Return myBIsActive
                End SyncLock
            End Get
        End Property
    End Class

End Class
