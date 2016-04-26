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
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.lblDate = New System.Windows.Forms.Label()
        Me.btnSetDateToNull = New System.Windows.Forms.Button()
        Me.Calendar = New ActiveDevelop.EntitiesFormsLib.MonthCalendarEx()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 2
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 80.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.lblDate, 1, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.btnSetDateToNull, 0, 0)
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(0, 164)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 1
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(203, 24)
        Me.TableLayoutPanel1.TabIndex = 0
        '
        'lblDate
        '
        Me.lblDate.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.lblDate.AutoEllipsis = True
        Me.lblDate.BackColor = System.Drawing.Color.White
        Me.lblDate.Location = New System.Drawing.Point(53, 2)
        Me.lblDate.Name = "lblDate"
        Me.lblDate.Size = New System.Drawing.Size(136, 19)
        Me.lblDate.TabIndex = 5
        Me.lblDate.Text = "Mo, 31. Sep. 2009"
        Me.lblDate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'btnSetDateToNull
        '
        Me.btnSetDateToNull.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.btnSetDateToNull.BackgroundImage = Global.ActiveDevelop.EntitiesFormsLib.My.Resources.Resources._92_cancel_16
        Me.btnSetDateToNull.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnSetDateToNull.FlatAppearance.BorderSize = 0
        Me.btnSetDateToNull.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.btnSetDateToNull.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSetDateToNull.Location = New System.Drawing.Point(3, 3)
        Me.btnSetDateToNull.Name = "btnSetDateToNull"
        Me.btnSetDateToNull.Size = New System.Drawing.Size(18, 18)
        Me.btnSetDateToNull.TabIndex = 6
        Me.btnSetDateToNull.UseVisualStyleBackColor = True
        '
        'Calendar
        '
        Me.Calendar.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Calendar.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Calendar.Location = New System.Drawing.Point(2, 2)
        Me.Calendar.Margin = New System.Windows.Forms.Padding(0)
        Me.Calendar.Name = "Calendar"
        Me.Calendar.ShowWeekNumbers = True
        Me.Calendar.TabIndex = 8
        '
        'CalendarPopupContent
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.BackColor = System.Drawing.SystemColors.ButtonHighlight
        Me.Controls.Add(Me.Calendar)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.Name = "CalendarPopupContent"
        Me.Size = New System.Drawing.Size(205, 191)
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents lblDate As System.Windows.Forms.Label
    Friend WithEvents btnSetDateToNull As System.Windows.Forms.Button
    Friend WithEvents Calendar As MonthCalendarEx
End Class
