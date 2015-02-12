Imports System.ComponentModel
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Windows.Forms

''' <summary>
''' EXPERIMENTAL, not ready for deployment.
''' </summary>
<ToolboxItem(False)>
Public Class ShapedButton
    Inherits TransparentControlBase

    Private myIsPushed As Boolean
    Private myIsHot As Boolean
    Private myImage As Image

    Sub New()
        MyBase.New()
        Me.BackColor = Color.Gray
    End Sub

    Protected Overrides Sub OnPaint(e As System.Windows.Forms.PaintEventArgs)
        DrawButton(e.Graphics)
    End Sub

    Protected Overridable Sub DrawButtonNormal(g As Graphics)
        Dim currRect = New Rectangle(Me.ClientRectangle.X + Me.Padding.Left,
                             Me.ClientRectangle.Y + Me.Padding.Top,
                             Me.ClientRectangle.Width - (Me.Padding.Right + Me.Padding.Left),
                             Me.ClientRectangle.Height - (Me.Padding.Bottom + Me.Padding.Top))

        Dim outerPen As Pen
        Dim innerPen As Pen

        If Me.Focused Then
            outerPen = New Pen(SystemColors.Control)
            innerPen = New Pen(SystemColors.Control)
        End If

        g.SmoothingMode = Drawing.Drawing2D.SmoothingMode.AntiAlias

        g.DrawEllipse(Drawing.Pens.Black, currRect)
        currRect.Inflate(-1, -1)
        g.DrawEllipse(Drawing.Pens.White, currRect)

        currRect.Inflate(-1, -1)

        g.FillPie(New SolidBrush(SystemColors.Control), currRect, 180, 180)
        g.FillPie(New SolidBrush(SystemColors.ControlLight), currRect, 0, 180)

    End Sub

    Protected Overridable Sub DrawButton(g As Graphics)
        Dim currRect = New Rectangle(Me.ClientRectangle.X + Me.Padding.Left,
                             Me.ClientRectangle.Y + Me.Padding.Top,
                             Me.ClientRectangle.Width - (Me.Padding.Right + Me.Padding.Left),
                             Me.ClientRectangle.Height - (Me.Padding.Bottom + Me.Padding.Top))

        g.SmoothingMode = Drawing.Drawing2D.SmoothingMode.AntiAlias

        Dim lightestPen = New Pen(ShapedButton.Lighter(Me.BackColor, CSng(ButtonShadowStrength / 20) * 3), ButtonShaddowDepth)
        Dim lightPen = New Pen(ShapedButton.Lighter(Me.BackColor, CSng(ButtonShadowStrength / 20 * 1.5)), ButtonShaddowDepth)
        Dim actualPen = New Pen(BackColor, ButtonShaddowDepth)
        Dim darkPen = New Pen(ShapedButton.Darker(Me.BackColor, CSng(ButtonShadowStrength / 20)), ButtonShaddowDepth)
        Dim darkestPen = New Pen(ShapedButton.Darker(Me.BackColor, CSng(ButtonShadowStrength / 20) * 2), ButtonShaddowDepth)

        'Ganz außen
        g.DrawArc(lightestPen, currRect, 180, 180)
        g.DrawArc(darkestPen, currRect, 0, 180)

        currRect.Inflate(-ButtonShaddowDepth, -ButtonShaddowDepth)
        g.DrawArc(lightPen, currRect, 180, 180)
        g.DrawArc(darkPen, currRect, 0, 180)

        currRect.Inflate(-ButtonShaddowDepth, -ButtonShaddowDepth)
        Dim currBrush = New SolidBrush(actualPen.Color)
        g.DrawEllipse(actualPen, currRect)
        g.FillEllipse(currBrush, currRect)

        If Image IsNot Nothing Then
            Dim path As New GraphicsPath()
            path.AddEllipse(currRect)
            path.FillMode = FillMode.Alternate
            g.SetClip(path)
            g.DrawImage(Image, currRect)
        End If

    End Sub

    Protected Overrides Sub OnMouseEnter(e As System.EventArgs)
        MyBase.OnMouseEnter(e)
        Debug.Print("MouseEnter")
    End Sub

    Protected Overrides Sub OnMouseLeave(e As System.EventArgs)
        MyBase.OnMouseLeave(e)
        Debug.Print("MouseLeave")
    End Sub

    Property ButtonShaddowDepth As Integer = 1
    Property ButtonShadowStrength As Single = 1
    Property Image As Image

End Class
