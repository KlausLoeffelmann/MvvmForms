<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class NullableValueComboBoxView
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
        Me.NullableValueComboBox1 = New ActiveDevelop.EntitiesFormsLib.NullableValueComboBox()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'NullableValueComboBox1
        '
        Me.NullableValueComboBox1.AssignedManagerControl = Nothing
        Me.NullableValueComboBox1.DisplayMemberPath = ""
        Me.NullableValueComboBox1.ExceptionBalloonDuration = 0
        Me.NullableValueComboBox1.IsKeyField = False
        Me.NullableValueComboBox1.ItemSource = Nothing
        Me.NullableValueComboBox1.Location = New System.Drawing.Point(85, 67)
        Me.NullableValueComboBox1.Name = "NullableValueComboBox1"
        Me.NullableValueComboBox1.NullValueMessage = ""
        Me.NullableValueComboBox1.ProcessingPriority = 0
        Me.NullableValueComboBox1.SelectedForProcessing = False
        Me.NullableValueComboBox1.SelectedItem = Nothing
        Me.NullableValueComboBox1.Size = New System.Drawing.Size(120, 22)
        Me.NullableValueComboBox1.TabIndex = 0
        Me.NullableValueComboBox1.ValueNotFoundBehavior = ActiveDevelop.EntitiesFormsLib.ValueNotFoundBehavior.KeepFocus
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(85, 118)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(75, 23)
        Me.Button1.TabIndex = 1
        Me.Button1.Text = "Button1"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'NullableValueComboBoxView
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(284, 261)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.NullableValueComboBox1)
        Me.Name = "NullableValueComboBoxView"
        Me.Text = "NullableValueComboBoxView"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents NullableValueComboBox1 As ActiveDevelop.EntitiesFormsLib.NullableValueComboBox
    Friend WithEvents Button1 As System.Windows.Forms.Button
End Class
