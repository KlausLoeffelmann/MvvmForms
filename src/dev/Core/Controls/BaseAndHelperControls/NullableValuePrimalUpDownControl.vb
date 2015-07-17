Imports System.ComponentModel
Imports System.Windows.Forms

''' <summary>
''' Infrastruktur-Klasse. Support-Control für das <see cref="NullableIntValue">NullableIntValue-Steuerelement</see>.
''' </summary>
''' <remarks>Sie können dieses Steuerelement direkt verwenden, sein Einsatz ist aber in erster Linie als Basis für das
''' <see cref="NullableIntValue">NullableIntValue-Steuerelement</see> gedacht.</remarks>
<ToolboxItem(False)>
Public Class NullableValuePrimalUpDownControl
    Inherits TextBoxSpinButton
    Implements ITextBoxBasedControl, INullableValuePrimalControl

    Public Event ControlValidated(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Implements INullableValuePrimalControl.ControlValidated
    Public Event ControlValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Implements INullableValuePrimalControl.ControlValueChanged

    Sub New()
        MyBase.New()
        InitializeComponentInternal()
    End Sub

    Protected Overridable Sub InitializeComponentInternal() Implements INullableValuePrimalControl.InitializeComponentInternal
        Me.Text = ""
        Me.Borderstyle = System.Windows.Forms.BorderStyle.None
        Me.AutoSize = False
    End Sub

    Protected Overrides Sub OnTextChanged(ByVal e As System.EventArgs)
        MyBase.OnTextChanged(e)
        RaiseEvent ControlValueChanged(Me, e)
    End Sub

    Protected Overrides Sub OnValidating(ByVal e As System.ComponentModel.CancelEventArgs)
        RaiseEvent ControlValidated(Me, e)
        MyBase.OnValidating(e)
    End Sub

    Public Sub ControlSetFocus() Implements INullableValuePrimalControl.ControlSetFocus
        Me.Focus()
    End Sub

    Public Property Value As Object Implements INullableValuePrimalControl.Value
        Get
            Return Me.TextBoxPart.Text
        End Get
        Set(ByVal value As Object)
            If value Is Nothing Then
                Me.TextBoxPart.Text = String.Empty
            Else
                Me.TextBoxPart.Text = value.ToString
            End If
        End Set
    End Property

    Public Property ControlFont As System.Drawing.Font Implements INullableValuePrimalControl.ControlFont
        Get
            Return Me.Font
        End Get
        Set(ByVal value As System.Drawing.Font)
            Me.Font = value
        End Set
    End Property

    Public Property ControlForeColor As System.Drawing.Color Implements INullableValuePrimalControl.ControlForeColor
        Get
            Return Me.ForeColor
        End Get
        Set(ByVal value As System.Drawing.Color)
            Me.ForeColor = value
        End Set
    End Property
End Class