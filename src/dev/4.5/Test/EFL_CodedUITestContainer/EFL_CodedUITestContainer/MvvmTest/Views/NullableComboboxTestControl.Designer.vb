<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class NullableComboboxTestControl
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
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
        Me.SuspendLayout()
        '
        'NullableValueComboBox1
        '
        Me.NullableValueComboBox1.AssignedManagerControl = Nothing
        Me.NullableValueComboBox1.DisplayMemberPath = ""
        Me.NullableValueComboBox1.ExceptionBalloonDuration = 0
        Me.NullableValueComboBox1.IsKeyField = False
        Me.NullableValueComboBox1.ItemSource = Nothing
        Me.NullableValueComboBox1.Location = New System.Drawing.Point(3, 3)
        Me.NullableValueComboBox1.Name = "NullableValueComboBox1"
        Me.NullableValueComboBox1.NullValueMessage = ""
        Me.NullableValueComboBox1.ProcessingPriority = 0
        Me.NullableValueComboBox1.SelectedForProcessing = False
        Me.NullableValueComboBox1.SelectedItem = Nothing
        Me.NullableValueComboBox1.Size = New System.Drawing.Size(120, 22)
        Me.NullableValueComboBox1.TabIndex = 0
        Me.NullableValueComboBox1.ValueNotFoundBehavior = ActiveDevelop.EntitiesFormsLib.ValueNotFoundBehavior.PreserveInput
        '
        'Test
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.NullableValueComboBox1)
        Me.Name = "Test"
        Me.Size = New System.Drawing.Size(137, 60)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents NullableValueComboBox1 As ActiveDevelop.EntitiesFormsLib.NullableValueComboBox

End Class
