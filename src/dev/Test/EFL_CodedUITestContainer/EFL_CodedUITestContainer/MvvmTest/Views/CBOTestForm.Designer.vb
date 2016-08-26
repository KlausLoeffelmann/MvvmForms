<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class CBOTestForm
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
        Me.Test1 = New EFL_CodedUITestContainer.NullableComboboxTestControl()
        Me.SelectionChangedListBox = New System.Windows.Forms.ListBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.TextBox2 = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        'NullableValueComboBox1
        '
        Me.NullableValueComboBox1.AssignedManagerControl = Nothing
        Me.NullableValueComboBox1.DisplayMemberPath = "Name"
        Me.NullableValueComboBox1.ExceptionBalloonDuration = 0
        Me.NullableValueComboBox1.ImitateTabByPageKeys = True
        Me.NullableValueComboBox1.IsKeyField = False
        Me.NullableValueComboBox1.ItemSource = Nothing
        Me.NullableValueComboBox1.Location = New System.Drawing.Point(584, 141)
        Me.NullableValueComboBox1.Name = "NullableValueComboBox1"
        Me.NullableValueComboBox1.NullValueMessage = ""
        Me.NullableValueComboBox1.ProcessingPriority = 0
        Me.NullableValueComboBox1.SelectedForProcessing = False
        Me.NullableValueComboBox1.SelectedItem = Nothing
        Me.NullableValueComboBox1.SelectedValue = Nothing
        Me.NullableValueComboBox1.SelectedValuePath = ""
        Me.NullableValueComboBox1.Size = New System.Drawing.Size(120, 22)
        Me.NullableValueComboBox1.TabIndex = 1
        Me.NullableValueComboBox1.ValueNotFoundBehavior = ActiveDevelop.EntitiesFormsLib.ValueNotFoundBehavior.SelectFirst
        '
        'Test1
        '
        Me.Test1.Location = New System.Drawing.Point(12, 219)
        Me.Test1.Name = "Test1"
        Me.Test1.Size = New System.Drawing.Size(54, 30)
        Me.Test1.TabIndex = 6
        '
        'SelectionChangedListBox
        '
        Me.SelectionChangedListBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.SelectionChangedListBox.FormattingEnabled = True
        Me.SelectionChangedListBox.Location = New System.Drawing.Point(80, 99)
        Me.SelectionChangedListBox.Name = "SelectionChangedListBox"
        Me.SelectionChangedListBox.Size = New System.Drawing.Size(120, 433)
        Me.SelectionChangedListBox.TabIndex = 3
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(80, 77)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(97, 13)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "SelectionChanged:"
        '
        'TextBox1
        '
        Me.TextBox1.Location = New System.Drawing.Point(599, 89)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(100, 20)
        Me.TextBox1.TabIndex = 0
        '
        'TextBox2
        '
        Me.TextBox2.Location = New System.Drawing.Point(584, 196)
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.Size = New System.Drawing.Size(100, 20)
        Me.TextBox2.TabIndex = 2
        '
        'CBOTestForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(898, 542)
        Me.Controls.Add(Me.TextBox2)
        Me.Controls.Add(Me.TextBox1)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.SelectionChangedListBox)
        Me.Controls.Add(Me.NullableValueComboBox1)
        Me.Controls.Add(Me.Test1)
        Me.Name = "CBOTestForm"
        Me.Text = "CBOTestForm"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Test1 As EFL_CodedUITestContainer.NullableComboboxTestControl
    Friend WithEvents NullableValueComboBox1 As ActiveDevelop.EntitiesFormsLib.NullableValueComboBox
    Friend WithEvents SelectionChangedListBox As ListBox
    Friend WithEvents Label1 As Label
    Friend WithEvents TextBox1 As TextBox
    Friend WithEvents TextBox2 As TextBox
End Class
