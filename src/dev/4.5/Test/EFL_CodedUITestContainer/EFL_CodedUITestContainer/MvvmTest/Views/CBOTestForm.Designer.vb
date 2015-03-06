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
        Me.Test1 = New EFL_CodedUITestContainer.Test()
        Me.SuspendLayout()
        '
        'Test1
        '
        Me.Test1.Location = New System.Drawing.Point(113, 95)
        Me.Test1.Name = "Test1"
        Me.Test1.Size = New System.Drawing.Size(137, 60)
        Me.Test1.TabIndex = 0
        '
        'CBOTestForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(284, 261)
        Me.Controls.Add(Me.Test1)
        Me.Name = "CBOTestForm"
        Me.Text = "CBOTestForm"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Test1 As EFL_CodedUITestContainer.Test
End Class
