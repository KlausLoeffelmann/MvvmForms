<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class HighDpiTestForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.ComboBox1 = New System.Windows.Forms.ComboBox()
        Me.NullableDateValue1 = New ActiveDevelop.EntitiesFormsLib.NullableDateValue()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'ComboBox1
        '
        Me.ComboBox1.FormattingEnabled = True
        Me.ComboBox1.Location = New System.Drawing.Point(141, 90)
        Me.ComboBox1.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.ComboBox1.Name = "ComboBox1"
        Me.ComboBox1.Size = New System.Drawing.Size(273, 24)
        Me.ComboBox1.TabIndex = 2
        '
        'NullableDateValue1
        '
        Me.NullableDateValue1.AssignedManagerComponent = Nothing
        Me.NullableDateValue1.Borderstyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.NullableDateValue1.ContentPresentPermission = ActiveDevelop.EntitiesFormsLib.ContentPresentPermissions.Normal
        Me.NullableDateValue1.DaysDistanceBetweenLinkedControl = Nothing
        Me.NullableDateValue1.LinkedToNullableDateControl = Nothing
        Me.NullableDateValue1.Location = New System.Drawing.Point(42, 37)
        Me.NullableDateValue1.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.NullableDateValue1.MaxLength = 32767
        Me.NullableDateValue1.Name = "NullableDateValue1"
        Me.NullableDateValue1.ObfuscationChar = Nothing
        Me.NullableDateValue1.PermissionReason = Nothing
        Me.NullableDateValue1.Size = New System.Drawing.Size(161, 22)
        Me.NullableDateValue1.TabIndex = 3
        Me.NullableDateValue1.UIGuid = New System.Guid("38dee947-8af2-40b3-a33d-624cdc21f016")
        Me.NullableDateValue1.Value = Nothing
        Me.NullableDateValue1.ValueValidationState = Nothing
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(114, 179)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(189, 33)
        Me.Button1.TabIndex = 4
        Me.Button1.Text = "Button1"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'HighDpiTestForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(558, 320)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.NullableDateValue1)
        Me.Controls.Add(Me.ComboBox1)
        Me.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.Name = "HighDpiTestForm"
        Me.Text = "HighDpiTestForm"
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents ComboBox1 As ComboBox
    Friend WithEvents NullableDateValue1 As ActiveDevelop.EntitiesFormsLib.NullableDateValue
    Friend WithEvents Button1 As Button
End Class
