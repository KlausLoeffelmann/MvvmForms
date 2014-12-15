Imports System.Windows.Forms
Imports System.Drawing

Public Class TransparentControlBase
    Inherits Panel

    Sub New()
        'SetStyle(ControlStyles.OptimizedDoubleBuffer, True)
    End Sub

    Protected Overrides ReadOnly Property CreateParams As System.Windows.Forms.CreateParams
        Get
            Dim cp = MyBase.CreateParams
            cp.ExStyle = cp.ExStyle Or &H20 'WS_EX_TRANSPARENT 
            Return cp
        End Get
    End Property

    Protected Overrides Sub OnPaintBackground(pevent As System.Windows.Forms.PaintEventArgs)
        'Don't call base method!
    End Sub

    Protected Sub InvalidateEx()

    End Sub

    ''' <summary>
    ''' Erhöht die Helligkeit der angegebenenen Farbe (Helligkeitsbereich gesamt: 0-1)
    ''' </summary>
    ''' <param name="color"></param>
    ''' <param name="value"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function Lighter(color As Color, value As Single) As Color
        Return HSL2RGB(color.GetHue, color.GetSaturation, color.GetBrightness + value)
    End Function

    ''' <summary>
    ''' Vermindert die Helligkeit der angegebenen Farbe (Helligkeitsbereich gesamt: 0-1)
    ''' </summary>
    ''' <param name="color"></param>
    ''' <param name="value"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function Darker(color As Color, value As Single) As Color
        Return HSL2RGB(color.GetHue, color.GetSaturation, color.GetBrightness - value)
    End Function

    Public Shared Function HSL2RGB(h As Double, sl As Double, l As Double) As Color
        Dim v As Double
        Dim r As Double, g As Double, b As Double

        r = l
        ' default to gray
        g = l
        b = l
        v = If((l <= 0.5), (l * (1.0 + sl)), (l + sl - l * sl))
        If v > 0 Then
            Dim m As Double
            Dim sv As Double
            Dim sextant As Integer
            Dim fract As Double, vsf As Double, mid1 As Double, mid2 As Double

            m = l + l - v
            sv = (v - m) / v
            h *= 6.0
            sextant = CInt(Math.Truncate(h))
            fract = h - sextant
            vsf = v * sv * fract
            mid1 = m + vsf
            mid2 = v - vsf
            Select Case sextant
                Case 0
                    r = v
                    g = mid1
                    b = m
                    Exit Select
                Case 1
                    r = mid2
                    g = v
                    b = m
                    Exit Select
                Case 2
                    r = m
                    g = v
                    b = mid1
                    Exit Select
                Case 3
                    r = m
                    g = mid2
                    b = v
                    Exit Select
                Case 4
                    r = mid1
                    g = m
                    b = v
                    Exit Select
                Case 5
                    r = v
                    g = m
                    b = mid2
                    Exit Select
            End Select
        End If
        If r > 1 Then r = 1
        If g > 1 Then g = 1
        If b > 1 Then b = 1
        Return System.Drawing.Color.FromArgb(255,
                                             Convert.ToByte(r * 255.0F),
                                             Convert.ToByte(g * 255.0F),
                                             Convert.ToByte(b * 255.0F))
    End Function
End Class
