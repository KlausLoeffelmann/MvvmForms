<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class HighDpiTestForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
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

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.ComboBox1 = New System.Windows.Forms.ComboBox()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.NullableTextValue1 = New ActiveDevelop.EntitiesFormsLib.NullableTextValue()
        Me.NullableDateValue1 = New ActiveDevelop.EntitiesFormsLib.NullableDateValue()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'ComboBox1
        '
        Me.ComboBox1.FormattingEnabled = True
        Me.ComboBox1.Location = New System.Drawing.Point(329, 25)
        Me.ComboBox1.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.ComboBox1.Name = "ComboBox1"
        Me.ComboBox1.Size = New System.Drawing.Size(273, 24)
        Me.ComboBox1.TabIndex = 1
        '
        'TextBox1
        '
        Me.TextBox1.Location = New System.Drawing.Point(329, 78)
        Me.TextBox1.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(272, 22)
        Me.TextBox1.TabIndex = 2
        '
        'NullableTextValue1
        '
        Me.NullableTextValue1.AssignedManagerComponent = Nothing
        Me.NullableTextValue1.Borderstyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.NullableTextValue1.ContentPresentPermission = ActiveDevelop.EntitiesFormsLib.ContentPresentPermissions.Normal
        Me.NullableTextValue1.Location = New System.Drawing.Point(23, 78)
        Me.NullableTextValue1.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.NullableTextValue1.MaxLength = 32767
        Me.NullableTextValue1.Name = "NullableTextValue1"
        Me.NullableTextValue1.ObfuscationChar = Nothing
        Me.NullableTextValue1.PermissionReason = Nothing
        Me.NullableTextValue1.Size = New System.Drawing.Size(272, 22)
        Me.NullableTextValue1.TabIndex = 4
        Me.NullableTextValue1.UIGuid = New System.Guid("8248b0ee-ae7f-4f7b-b5ea-575291963564")
        Me.NullableTextValue1.Value = Nothing
        Me.NullableTextValue1.ValueValidationState = Nothing
        '
        'NullableDateValue1
        '
        Me.NullableDateValue1.AssignedManagerComponent = Nothing
        Me.NullableDateValue1.Borderstyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.NullableDateValue1.ContentPresentPermission = ActiveDevelop.EntitiesFormsLib.ContentPresentPermissions.Normal
        Me.NullableDateValue1.DaysDistanceBetweenLinkedControl = Nothing
        Me.NullableDateValue1.LinkedToNullableDateControl = Nothing
        Me.NullableDateValue1.Location = New System.Drawing.Point(22, 25)
        Me.NullableDateValue1.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.NullableDateValue1.MaxLength = 32767
        Me.NullableDateValue1.Name = "NullableDateValue1"
        Me.NullableDateValue1.ObfuscationChar = Nothing
        Me.NullableDateValue1.PermissionReason = Nothing
        Me.NullableDateValue1.Size = New System.Drawing.Size(273, 22)
        Me.NullableDateValue1.TabIndex = 5
        Me.NullableDateValue1.UIGuid = New System.Guid("8e9c3dbc-de2b-40b5-b8a9-bb78c6826516")
        Me.NullableDateValue1.Value = Nothing
        Me.NullableDateValue1.ValueValidationState = Nothing
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(243, 152)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(140, 55)
        Me.Button1.TabIndex = 6
        Me.Button1.Text = "Button1"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'HighDpiTestForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(630, 245)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.NullableDateValue1)
        Me.Controls.Add(Me.NullableTextValue1)
        Me.Controls.Add(Me.TextBox1)
        Me.Controls.Add(Me.ComboBox1)
        Me.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.Name = "HighDpiTestForm"
        Me.Text = "HighDpiTestForm"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ComboBox1 As ComboBox
    Friend WithEvents TextBox1 As TextBox
    Friend WithEvents NullableTextValue1 As ActiveDevelop.EntitiesFormsLib.NullableTextValue
    Friend WithEvents NullableDateValue1 As ActiveDevelop.EntitiesFormsLib.NullableDateValue
    Friend WithEvents Button1 As Button
End Class
