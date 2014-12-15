<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class DataSourceTypeUIForm
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
        Me.btnOK = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.chkGACAssemblies = New System.Windows.Forms.CheckBox()
        Me.DataSourceTreeView = New System.Windows.Forms.TreeView()
        Me.SuspendLayout()
        '
        'btnOK
        '
        Me.btnOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnOK.Location = New System.Drawing.Point(443, 475)
        Me.btnOK.Margin = New System.Windows.Forms.Padding(4)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(85, 34)
        Me.btnOK.TabIndex = 0
        Me.btnOK.Text = "OK"
        Me.btnOK.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.Location = New System.Drawing.Point(536, 475)
        Me.btnCancel.Margin = New System.Windows.Forms.Padding(4)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(85, 34)
        Me.btnCancel.TabIndex = 1
        Me.btnCancel.Text = "Abbruch"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(13, 12)
        Me.Label2.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(125, 40)
        Me.Label2.TabIndex = 15
        Me.Label2.Text = "ViewModel/ DataSource Class:"
        '
        'chkGACAssemblies
        '
        Me.chkGACAssemblies.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.chkGACAssemblies.AutoSize = True
        Me.chkGACAssemblies.Location = New System.Drawing.Point(176, 483)
        Me.chkGACAssemblies.Margin = New System.Windows.Forms.Padding(4)
        Me.chkGACAssemblies.Name = "chkGACAssemblies"
        Me.chkGACAssemblies.Size = New System.Drawing.Size(233, 21)
        Me.chkGACAssemblies.TabIndex = 16
        Me.chkGACAssemblies.Text = "Include GAC/System Assemblies"
        Me.chkGACAssemblies.UseVisualStyleBackColor = True
        '
        'DataSourceTreeView
        '
        Me.DataSourceTreeView.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DataSourceTreeView.HideSelection = False
        Me.DataSourceTreeView.Location = New System.Drawing.Point(177, 12)
        Me.DataSourceTreeView.Name = "DataSourceTreeView"
        Me.DataSourceTreeView.Size = New System.Drawing.Size(444, 456)
        Me.DataSourceTreeView.TabIndex = 20
        '
        'DataSourceTypeUIForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(632, 527)
        Me.Controls.Add(Me.DataSourceTreeView)
        Me.Controls.Add(Me.chkGACAssemblies)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnOK)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.MinimumSize = New System.Drawing.Size(495, 249)
        Me.Name = "DataSourceTypeUIForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Pick ViewModel (DataSource) Type:"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents chkGACAssemblies As System.Windows.Forms.CheckBox
    Friend WithEvents DataSourceTreeView As System.Windows.Forms.TreeView
End Class
