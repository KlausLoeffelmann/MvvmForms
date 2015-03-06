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
        Me.Button1 = New System.Windows.Forms.Button()
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.TabPage2 = New System.Windows.Forms.TabPage()
        Me.NullableValueComboBox2 = New ActiveDevelop.EntitiesFormsLib.NullableValueComboBox()
        Me.Test1 = New EFL_CodedUITestContainer.Test()
        Me.TabControl1.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
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
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.TabPage1)
        Me.TabControl1.Controls.Add(Me.TabPage2)
        Me.TabControl1.Location = New System.Drawing.Point(420, 69)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(362, 296)
        Me.TabControl1.TabIndex = 5
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.GroupBox1)
        Me.TabPage1.Location = New System.Drawing.Point(4, 22)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(354, 270)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "TabPage1"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.NullableValueComboBox2)
        Me.GroupBox1.Location = New System.Drawing.Point(100, 62)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(220, 170)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "GroupBox1"
        '
        'TabPage2
        '
        Me.TabPage2.Location = New System.Drawing.Point(4, 22)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(354, 270)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "TabPage2"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'NullableValueComboBox2
        '
        Me.NullableValueComboBox2.AssignedManagerControl = Nothing
        Me.NullableValueComboBox2.DisplayMemberPath = ""
        Me.NullableValueComboBox2.ExceptionBalloonDuration = 0
        Me.NullableValueComboBox2.IsKeyField = False
        Me.NullableValueComboBox2.ItemSource = Nothing
        Me.NullableValueComboBox2.Location = New System.Drawing.Point(83, 68)
        Me.NullableValueComboBox2.Name = "NullableValueComboBox2"
        Me.NullableValueComboBox2.NullValueMessage = ""
        Me.NullableValueComboBox2.ProcessingPriority = 0
        Me.NullableValueComboBox2.SelectedForProcessing = False
        Me.NullableValueComboBox2.SelectedItem = Nothing
        Me.NullableValueComboBox2.Size = New System.Drawing.Size(120, 22)
        Me.NullableValueComboBox2.TabIndex = 7
        Me.NullableValueComboBox2.ValueNotFoundBehavior = ActiveDevelop.EntitiesFormsLib.ValueNotFoundBehavior.PreserveInput
        '
        'Test1
        '
        Me.Test1.BackColor = System.Drawing.SystemColors.AppWorkspace
        Me.Test1.Location = New System.Drawing.Point(69, 221)
        Me.Test1.Name = "Test1"
        Me.Test1.Size = New System.Drawing.Size(145, 128)
        Me.Test1.TabIndex = 7
        '
        'NullableValueComboBoxView
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(897, 464)
        Me.Controls.Add(Me.Test1)
        Me.Controls.Add(Me.TabControl1)
        Me.Controls.Add(Me.Button1)
        Me.Name = "NullableValueComboBoxView"
        Me.Text = "NullableValueComboBoxView"
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.GroupBox1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents NullableValueComboBox2 As ActiveDevelop.EntitiesFormsLib.NullableValueComboBox
    Friend WithEvents Test1 As EFL_CodedUITestContainer.Test
End Class
