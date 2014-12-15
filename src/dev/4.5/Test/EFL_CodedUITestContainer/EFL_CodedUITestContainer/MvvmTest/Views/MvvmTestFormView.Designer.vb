<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MvvmTestFormView
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
        Me.TimeCollectionView1 = New EFL_CodedUITestContainer.TimeCollectionView()
        Me.SuspendLayout()
        '
        'TimeCollectionView1
        '
        Me.TimeCollectionView1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TimeCollectionView1.Location = New System.Drawing.Point(13, 13)
        Me.TimeCollectionView1.Margin = New System.Windows.Forms.Padding(4)
        Me.TimeCollectionView1.Name = "TimeCollectionView1"
        Me.TimeCollectionView1.Size = New System.Drawing.Size(810, 556)
        Me.TimeCollectionView1.TabIndex = 0
        '
        'MvvmTestFormView
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(843, 592)
        Me.Controls.Add(Me.TimeCollectionView1)
        Me.Name = "MvvmTestFormView"
        Me.Text = "MvvmTestFormView"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents TimeCollectionView1 As EFL_CodedUITestContainer.TimeCollectionView
End Class
