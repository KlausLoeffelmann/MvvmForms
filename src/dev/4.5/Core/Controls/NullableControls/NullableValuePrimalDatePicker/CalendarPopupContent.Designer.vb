<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class CalendarPopupContent
    Inherits System.Windows.Forms.UserControl

    'UserControl überschreibt den Löschvorgang, um die Komponentenliste zu bereinigen.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Wird vom Windows Form-Designer benötigt.
    Private components As System.ComponentModel.IContainer

    'Hinweis: Die folgende Prozedur ist für den Windows Form-Designer erforderlich.
    'Das Bearbeiten ist mit dem Windows Form-Designer möglich.  
    'Das Bearbeiten mit dem Code-Editor ist nicht möglich.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.pnlDateLabelContainer = New System.Windows.Forms.Panel()
        Me.btnSetDateToNull = New System.Windows.Forms.Button()
        Me.lblDate = New System.Windows.Forms.Label()
        Me.Calendar = New ActiveDevelop.EntitiesFormsLib.MonthCalendarEx()
        Me.pnlDateLabelContainer.SuspendLayout()
        Me.SuspendLayout()
        '
        'pnlDateLabelContainer
        '
        Me.pnlDateLabelContainer.BackColor = System.Drawing.Color.White
        Me.pnlDateLabelContainer.Controls.Add(Me.btnSetDateToNull)
        Me.pnlDateLabelContainer.Controls.Add(Me.lblDate)
        Me.pnlDateLabelContainer.Location = New System.Drawing.Point(-1, 160)
        Me.pnlDateLabelContainer.Margin = New System.Windows.Forms.Padding(0)
        Me.pnlDateLabelContainer.Name = "pnlDateLabelContainer"
        Me.pnlDateLabelContainer.Size = New System.Drawing.Size(198, 25)
        Me.pnlDateLabelContainer.TabIndex = 3
        '
        'btnSetDateToNull
        '
        Me.btnSetDateToNull.BackgroundImage = Global.ActiveDevelop.EntitiesFormsLib.My.Resources.Resources._92_cancel_16
        Me.btnSetDateToNull.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnSetDateToNull.FlatAppearance.BorderSize = 0
        Me.btnSetDateToNull.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.btnSetDateToNull.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSetDateToNull.Location = New System.Drawing.Point(5, 3)
        Me.btnSetDateToNull.Name = "btnSetDateToNull"
        Me.btnSetDateToNull.Size = New System.Drawing.Size(18, 18)
        Me.btnSetDateToNull.TabIndex = 2
        Me.btnSetDateToNull.UseVisualStyleBackColor = True
        '
        'lblDate
        '
        Me.lblDate.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblDate.AutoEllipsis = True
        Me.lblDate.BackColor = System.Drawing.Color.White
        Me.lblDate.Location = New System.Drawing.Point(29, -1)
        Me.lblDate.Name = "lblDate"
        Me.lblDate.Size = New System.Drawing.Size(165, 24)
        Me.lblDate.TabIndex = 0
        Me.lblDate.Text = "Mo, 31. Sep. 2009"
        Me.lblDate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Calendar
        '
        Me.Calendar.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Calendar.Location = New System.Drawing.Point(-2, -2)
        Me.Calendar.Margin = New System.Windows.Forms.Padding(0)
        Me.Calendar.Name = "Calendar"
        Me.Calendar.ShowWeekNumbers = True
        Me.Calendar.TabIndex = 4
        '
        'CalendarPopupContent
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.pnlDateLabelContainer)
        Me.Controls.Add(Me.Calendar)
        Me.Name = "CalendarPopupContent"
        Me.Size = New System.Drawing.Size(195, 183)
        Me.pnlDateLabelContainer.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents pnlDateLabelContainer As System.Windows.Forms.Panel
    Friend WithEvents btnSetDateToNull As System.Windows.Forms.Button
    Friend WithEvents lblDate As System.Windows.Forms.Label
    Friend WithEvents Calendar As ActiveDevelop.EntitiesFormsLib.MonthCalendarEx

End Class
