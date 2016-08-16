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

    Private myAllowNonNumericKeys As Boolean = True

    Public Event ControlValidated(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Implements INullableValuePrimalControl.ControlValidated
    Public Event ControlValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Implements INullableValuePrimalControl.ControlValueChanged

    Sub New()
        MyBase.New()
        InitializeComponentInternal()
        AddHandler Me.TextBoxPart.KeyPress, AddressOf TextBoxPartKeyPressHandler
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

    Public Property AllowNonNumericKeys As Boolean
        Get
            Return myAllowNonNumericKeys
        End Get
        Set(value As Boolean)
            If Not Object.Equals(myAllowNonNumericKeys, value) Then
                myAllowNonNumericKeys = value
            End If
        End Set
    End Property

    Private Sub TextBoxPartKeyPressHandler(sender As Object, e As KeyPressEventArgs)
        MyBase.OnKeyPress(e)
        If Not AllowNonNumericKeys Then
            If Char.IsNumber(e.KeyChar) Or Char.IsPunctuation(e.KeyChar) Or Char.IsSeparator(e.KeyChar) Or Char.IsControl(e.KeyChar) Then
            Else
                e.Handled = True
            End If
        End If
    End Sub

    Protected Overrides Sub Dispose(disposing As Boolean)
        MyBase.Dispose(disposing)
        If disposing Then
            RemoveHandler Me.TextBoxPart.KeyPress, AddressOf TextBoxPartKeyPressHandler
        End If
    End Sub
End Class
