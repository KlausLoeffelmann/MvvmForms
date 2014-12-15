Imports System.ComponentModel
Imports System.Drawing
Imports System.Windows.Forms
Imports System.Windows.Forms.VisualStyles
Imports System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox
Imports System.Reflection

''' <summary>
''' Infrastruktur-Klasse. Support-Control für <see cref="NullableDateValue">NullableDateValue-Steuerelement</see>.
''' </summary>
''' <remarks>Sie können dieses Steuerelement direkt verwenden, sein Einsatz ist aber in erster Linie als Basis für das
''' <see cref="NullableDateValue">NullableDateValue-Steuerelement</see> gedacht.</remarks>
<ToolboxItem(False)> _
Public Class NullableValuePrimalDatePicker
    Inherits TextBoxPopup
    Implements ITextBoxBasedControl, INullableValuePrimalControl

    Public Event ControlValidated(ByVal sender As Object,
                                  ByVal e As System.ComponentModel.CancelEventArgs) Implements INullableValuePrimalControl.ControlValidated
    Public Event ControlValueChanged(ByVal sender As Object,
                                     ByVal e As System.EventArgs) Implements INullableValuePrimalControl.ControlValueChanged

    Private myCalendarForm As CalendarPopupContent

    Sub New()
        MyBase.New()
        myCalendarForm = DirectCast(MyBase.HostingControl, CalendarPopupContent)
        Me.PopupControl.IsResizable = False
        Me.IsPopupAutoSize = True
    End Sub

    Public Sub InitializeComponentInternal() Implements INullableValuePrimalControl.InitializeComponentInternal
    End Sub

    Public Sub ResetCalender()
        myCalendarForm.SuspendLayout()
        Dim oldMonthCal = myCalendarForm.Calendar
        myCalendarForm.Calendar = New MonthCalendarEx
        myCalendarForm.Calendar.Location = oldMonthCal.Location
        myCalendarForm.Calendar.Size = oldMonthCal.Size
        myCalendarForm.Calendar.Margin = New System.Windows.Forms.Padding(0)
        myCalendarForm.Calendar.Name = "Calendar"
        myCalendarForm.Calendar.ShowWeekNumbers = True

        'Ereignisse neu verdrahten
        myCalendarForm.Calendar.AddDateChangedProcHandler(oldMonthCal.DateChangedProc)
        myCalendarForm.Calendar.AddDateSelectedProcHandler(oldMonthCal.DateSelectedProc)
        myCalendarForm.Calendar.AddMouseDownProcHandler(oldMonthCal.MouseDownProc)
        myCalendarForm.Calendar.AddMouseUpProcHandler(oldMonthCal.MouseUpProc)
        myCalendarForm.Calendar.AddClickProcHandler(oldMonthCal.ClickProc)

        myCalendarForm.Controls.Add(myCalendarForm.Calendar)
        myCalendarForm.ResumeLayout()
        oldMonthCal.Dispose()
    End Sub

    Protected Overrides Function GetPopupContent() As System.Windows.Forms.Control
        Dim tmpPc = New CalendarPopupContent
        'Me.PopupSize = tmpPc.Size
        Return tmpPc
    End Function

    Public Property ControlFont As System.Drawing.Font Implements INullableValuePrimalControl.ControlFont
        Get
            Return Me.TextBoxPart.Font
        End Get
        Set(ByVal value As System.Drawing.Font)
            Me.TextBoxPart.Font = value
        End Set
    End Property

    Public Property ControlForeColor As System.Drawing.Color Implements INullableValuePrimalControl.ControlForeColor
        Get
            Return Me.TextBoxPart.ForeColor
        End Get
        Set(ByVal value As System.Drawing.Color)
            Me.TextBoxPart.ForeColor = value
        End Set
    End Property

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

    Public Sub ControlSetFocus() Implements INullableValuePrimalControl.ControlSetFocus
        Me.TextBoxPart.Focus()
    End Sub

    Friend ReadOnly Property PopupFormDateLable As Label
        Get
            Return myCalendarForm.lblDate
        End Get
    End Property

    Friend ReadOnly Property PopupFormCalendar As MonthCalendarEx
        Get
            Return myCalendarForm.Calendar
        End Get
    End Property

    Friend ReadOnly Property PopupFormResetButton As Button
        Get
            Return myCalendarForm.btnSetDateToNull
        End Get
    End Property

End Class
