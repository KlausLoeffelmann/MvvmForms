Imports System.Windows.Forms
Imports System.Drawing
Imports System.Windows.Forms.VisualStyles
Imports System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox
Imports System.ComponentModel

Public Class TextBoxButtonBase(Of ButtonType As {MultiPurposeButtonBase, New})
    Inherits ContainerControl
    Implements ITextBoxBasedControl

    Public Class DebuggableTextBox
        Inherits TextBox

        Public Overrides Property Text As String
            Get
                Return MyBase.Text
            End Get
            Set(value As String)
                MyBase.Text = value
            End Set
        End Property
    End Class

    Private myTextBox As TextBox
    Private myButton As ButtonType
    Private myBorderStyle As BorderStyle
    Private myButtons As New List(Of MultiPurposeButtonBase)
    Private myBackColorBrush As Brush
    Private myHideButtons As Boolean
    Private myBorderstyleHasBeenSetForInstance As Boolean

    Public Event ButtonAction(ByVal sender As Object, ByVal e As ButtonActionEventArgs)
    Public Event ButtonPressedStateChange(ByVal sender As Object, ByVal e As PressedStateChangedEventArgs)

    Private Const CONTROLDEFAULTWIDTH As Integer = 120
    Private Const PREFERREDBUTTONWIDTH As Integer = 16

    Sub New()
        MyBase.new()
        SetStyle(ControlStyles.UseTextForAccessibility, False)
        SetStyle(ControlStyles.StandardClick, False)
        SetStyle(ControlStyles.SupportsTransparentBackColor, True)
        SetStyle(ControlStyles.ContainerControl, True)
        SetStyle(ControlStyles.ResizeRedraw, True)
        SetStyle(ControlStyles.FixedHeight, True)
        InitializeControlsInternal()
    End Sub

    Protected Overridable Sub InitializeControlsInternal()

#If DEBUG Then
        myTextBox = New DebuggableTextBox
#Else
        myTextBox = New TextBox
#End If

        myButton = New ButtonType
        myButton.Name = "mainButton"
        myTextBox.AutoSize = False
        myButton.AutoSize = False
        myTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None
        myButtons.Add(myButton)

        Me.Controls.Add(myTextBox)
        Me.Controls.Add(myButton)

        'Maximal 3 weitere Buttons holen
        Dim count = 3
        Do While count > 0
            Dim buttonControlTuple = GetAdditionalButton(3 - count)
            If buttonControlTuple.Item1 IsNot Nothing Then
                buttonControlTuple.Item1.Name = "Button" & (3 - count)
                Me.Controls.Add(buttonControlTuple.Item1)
                myButtons.Add(buttonControlTuple.Item1)
            End If
            If Not buttonControlTuple.Item2 Then
                Exit Do
            End If
            count -= 1
        Loop

        SetInitialDefaultBorderstyleOnDemand()

        AddHandler myButton.UpDownCombo, AddressOf UpDownComboHandler
        AddHandler myButton.RequestFocus, AddressOf RequestFocusHandler
        AddHandler myTextBox.Click, AddressOf TextBoxClickHandler
        AddHandler myButton.PressedStateChanged, AddressOf ButtonPressedStateChangedHandler
    End Sub

    Protected Overridable Function GetAdditionalButton(buttonCount As Integer) As Tuple(Of MultiPurposeButtonBase, Boolean)
        Return New Tuple(Of MultiPurposeButtonBase, Boolean)(Nothing, False)
    End Function

    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        RemoveHandler myButton.UpDownCombo, AddressOf UpDownComboHandler
        RemoveHandler myButton.RequestFocus, AddressOf RequestFocusHandler
        RemoveHandler myTextBox.Click, AddressOf TextBoxClickHandler
        RemoveHandler myButton.PressedStateChanged, AddressOf ButtonPressedStateChangedHandler
        MyBase.Dispose(disposing)
    End Sub

    Private Sub UpDownComboHandler(ByVal sender As Object, ByVal e As UpDownComboEventArgs)
        OnButtonAction(New ButtonActionEventArgs(e.UpDownComboButtonID))
    End Sub

    ''' <summary>
    ''' Behandelt das Verhalten des Setzens des Borderstyles beim Instanziieren des Controls.
    ''' </summary>
    ''' <remarks>Da das Control *vor* dem Aufruf von DefaultSize von Control(Base) 
    ''' keine Gelegenheit im Basiskonstruktor bekommt, den Borderstyle zu setzen, 
    ''' müssen wir beim ersten Aufruf von DefaultSize UND im Konstruktor 
    ''' den Borderstyle setzen. Das darf natürlich nur dann passieren, 
    ''' wenn der Borderstyle noch nie initial gesetzt wurde - dafür 
    ''' dient das entsprechende Flag. Dieses Verhalten sollte, wenn überhaupt, 
    ''' nur mit Bedacht überschrieben werden!</remarks>
    Protected Overridable Sub SetInitialDefaultBorderstyleOnDemand()
        If myBorderstyleHasBeenSetForInstance Then Return
        Me.Borderstyle = DefaultBorderStyle()
        myBorderstyleHasBeenSetForInstance = True
    End Sub

    ''' <summary>
    ''' Liefert den Default-Borderstyle-Wert zurück.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>Überschreiben Sie diese Methode, wenn Sie das BorderStyle-Verhalten 
    ''' beim Instanziieren des Steuerelementes beeinflussen wollen.</remarks>
    Protected Overridable Function DefaultBorderStyle() As System.Windows.Forms.BorderStyle
        Return System.Windows.Forms.BorderStyle.None
    End Function

    Protected Overrides ReadOnly Property DefaultSize As Size
        Get
            'DefaultSize kann aufgerufen werden, noch bevor
            'der Konstruktor durch ist. Deswegen ist dieser
            'Aufruf hier notwendig.
            SetInitialDefaultBorderstyleOnDemand()
            Return New Size(CONTROLDEFAULTWIDTH,
                                Me.PreferredHeight)
        End Get
    End Property

    <Browsable(False), EditorBrowsable(EditorBrowsableState.Advanced),
     DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public ReadOnly Property PreferredHeight As Integer
        Get
            Dim fontHeight As Integer = MyBase.FontHeight
            If (Me.Borderstyle <> Borderstyle.None) Then
                Return (fontHeight + ((SystemInformation.BorderSize.Height * 4) + 3))
            End If
            Return (fontHeight + 3)
        End Get
    End Property

    Protected Overrides Sub OnResize(ByVal e As System.EventArgs)
        MyBase.Height = Me.PreferredHeight
        Me.Invalidate()
        Me.PositionControls()
    End Sub

    Protected Overridable Sub OnButtonAction(ByVal e As ButtonActionEventArgs)
        RaiseEvent ButtonAction(Me, e)
    End Sub

    Private Sub TextBoxClickHandler(ByVal sender As Object, ByVal e As EventArgs)
        myButton.SnapOutButton()
    End Sub

    Private Sub ButtonPressedStateChangedHandler(ByVal sender As Object, ByVal e As PressedStateChangedEventArgs)
        OnButtonPressedStateChange(e)
    End Sub

    Protected Overridable Sub OnButtonPressedStateChange(ByVal e As PressedStateChangedEventArgs)
        RaiseEvent ButtonPressedStateChange(Me, e)
    End Sub

    Protected Overrides Sub OnEnter(ByVal e As System.EventArgs)
        MyBase.OnEnter(e)
    End Sub

    Protected Overrides Sub OnGotFocus(ByVal e As System.EventArgs)
        MyBase.OnGotFocus(e)
    End Sub

    Protected Overrides Sub OnLeave(ByVal e As System.EventArgs)
        MyBase.OnLeave(e)
    End Sub

    Protected Overrides Sub OnLostFocus(ByVal e As System.EventArgs)
        MyBase.OnLostFocus(e)
        myButton.SnapOutButton()
    End Sub

    Private Sub RequestFocusHandler(ByVal sender As Object, ByVal e As RequestFocusEventArgs)
        e.Succeeded = myTextBox.Focus()
    End Sub

    Protected Sub SetButtonBehaviour(ByVal btnBehaviour As ButtonBehaviour)
        myButton.ButtonBehaviour = btnBehaviour
    End Sub

    Protected Overrides Sub WndProc(ByRef m As System.Windows.Forms.Message)
        MyBase.WndProc(m)
    End Sub

    Protected Overridable Sub SnapOutButton()
        myButton.SnapOutButton()
    End Sub

    Protected Overridable Sub SnapInButton()
        myButton.SnapInButton()
    End Sub

    <Browsable(False)>
    Public ReadOnly Property ButtonPressedState As Boolean
        Get
            Return myButton.PressedState
        End Get
    End Property

    <Browsable(False)>
    Public ReadOnly Property MouseOverButton As Boolean
        Get
            Return myButton.MouseOverButton
        End Get
    End Property

#Region "Layout-and Style-Handling"
    <DefaultValue(Borderstyle.None)>
    Property Borderstyle As BorderStyle
        Set(ByVal value As BorderStyle)
            If (Me.Borderstyle <> value) Then
                myBorderStyle = value
                MyBase.RecreateHandle()
            End If
        End Set
        Get
            Return myBorderStyle
        End Get
    End Property

    Protected Overrides ReadOnly Property CreateParams As System.Windows.Forms.CreateParams
        Get
            Dim tmpParams As CreateParams = MyBase.CreateParams
            tmpParams.Style = (tmpParams.Style And -8388609)
            If Not Application.RenderWithVisualStyles Then
                Select Case Me.Borderstyle
                    Case Borderstyle.FixedSingle
                        tmpParams.Style = (tmpParams.Style Or &H800000)
                        Return tmpParams
                    Case Borderstyle.Fixed3D
                        tmpParams.ExStyle = (tmpParams.ExStyle Or &H200)
                        Return tmpParams
                End Select
            End If
            Return tmpParams
        End Get
    End Property

    Protected Overrides Sub OnLayout(ByVal e As System.Windows.Forms.LayoutEventArgs)
        PositionControls()
        MyBase.OnLayout(e)
    End Sub

    Protected Overrides Sub OnFontChanged(e As System.EventArgs)
        MyBase.OnFontChanged(e)
        PositionControls()
    End Sub

    Protected Overridable Sub PositionControls()
        Dim textBoxArea As Rectangle = Rectangle.Empty
        Dim tmpRec As New Rectangle(Point.Empty, MyBase.ClientSize)
        Dim width As Integer = tmpRec.Width
        Dim renderWithVisualStyles As Boolean = Application.RenderWithVisualStyles
        Dim borderStyle As BorderStyle = Me.Borderstyle

        If renderWithVisualStyles Then
            Dim offset As Integer = If((Me.Borderstyle = borderStyle.None), 0, 2)
            tmpRec.Inflate(-offset, -offset)
        End If
        textBoxArea = tmpRec
        Dim btnArea = tmpRec

        If Not Me.HideButtons Then
            textBoxArea.Width -= GetPreferredButtonsWidth()
        End If
        myTextBox.Bounds = textBoxArea

        Dim bCount As Integer = 0
        Dim bOffset = 0
        For Each bItem In myButtons
            If bItem.Visible Then
                With bItem
                    btnArea = New Rectangle(tmpRec.Left + textBoxArea.Width + bOffset, tmpRec.Top,
                                            GetPreferredButtonWidth(bCount), tmpRec.Height)
                    bOffset += GetPreferredButtonWidth(bCount)
                End With
            End If
            bItem.Bounds = btnArea
            bItem.Invalidate()
        Next
    End Sub

    Protected Overrides Sub OnPaintBackground(e As System.Windows.Forms.PaintEventArgs)
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        MyBase.OnPaint(e)
        Dim bounds As Rectangle = myTextBox.Bounds
        For Each bItem In Me.myButtons
            bounds = Rectangle.Union(bounds, bItem.Bounds)
        Next

        If myBackColorBrush Is Nothing Then
            myBackColorBrush = New SolidBrush(Me.BackColor)
        End If

        Dim rec = New Rectangle(0, 0, Me.ClientSize.Width, Me.ClientSize.Height)
        e.Graphics.FillRectangle(myBackColorBrush, rec)

        If Application.RenderWithVisualStyles Then
            If (Me.Borderstyle = Borderstyle.None) Then
                GoTo endSkip
            End If
            Dim clientRectangle As Rectangle = MyBase.ClientRectangle
            Dim clipRectangle As Rectangle = e.ClipRectangle
            Dim renderer As New VisualStyleRenderer(TextEdit.Normal)
            Dim width As Integer = 1
            Dim surface1 As New Rectangle(clientRectangle.Left, clientRectangle.Top, width, clientRectangle.Height)
            Dim surface2 As New Rectangle(clientRectangle.Left, clientRectangle.Top, clientRectangle.Width, width)
            Dim surface3 As New Rectangle((clientRectangle.Right - width), clientRectangle.Top, width, clientRectangle.Height)
            Dim surface4 As New Rectangle(clientRectangle.Left, (clientRectangle.Bottom - width), clientRectangle.Width, width)
            surface1.Intersect(clipRectangle)
            surface2.Intersect(clipRectangle)
            surface3.Intersect(clipRectangle)
            surface4.Intersect(clipRectangle)
            renderer.DrawBackground(e.Graphics, clientRectangle, surface1)
            renderer.DrawBackground(e.Graphics, clientRectangle, surface2)
            renderer.DrawBackground(e.Graphics, clientRectangle, surface3)
            renderer.DrawBackground(e.Graphics, clientRectangle, surface4)
            Using pen As Pen = New Pen(Me.BackColor)
                Dim rect As Rectangle = bounds
                rect.X -= 1
                rect.Y -= 1
                rect.Width += 1
                rect.Height += 1
                e.Graphics.DrawRectangle(pen, rect)
                GoTo endSkip
            End Using
        End If
        Using pen2 As Pen = New Pen(Me.BackColor, If(MyBase.Enabled, CSng(2), CSng(1)))
            Dim tmpSurface As Rectangle = bounds
            tmpSurface.Inflate(1, 1)
            If Not MyBase.Enabled Then
                tmpSurface.X -= 1
                tmpSurface.Y -= 1
                tmpSurface.Width += 1
                tmpSurface.Height += 1
            End If
            e.Graphics.DrawRectangle(pen2, tmpSurface)
        End Using
endSkip:
    End Sub

    Protected Overridable Function GetPreferredButtonsWidth() As Integer

        Dim tmpWidth As Integer = 0

        For Each bItem In myButtons
            If bItem.Visible Then
                tmpWidth += PREFERREDBUTTONWIDTH
            End If
        Next
        Return tmpWidth
    End Function

    Protected Overridable Function GetPreferredButtonWidth(ButtonNo As Integer) As Integer
        Return If(myButtons(ButtonNo).Visible, PREFERREDBUTTONWIDTH, 0)
    End Function
#End Region

    <Browsable(False), EditorBrowsable(EditorBrowsableState.Never)>
    Friend ReadOnly Property TextBoxPart As System.Windows.Forms.TextBox Implements ITextBoxBasedControl.TextBoxPart
        Get
            Return myTextBox
        End Get
    End Property

    Public Property HideButtons As Boolean
        Get
            Return myHideButtons
        End Get
        Set(value As Boolean)
            myHideButtons = value
            PositionControls()
        End Set
    End Property
End Class
