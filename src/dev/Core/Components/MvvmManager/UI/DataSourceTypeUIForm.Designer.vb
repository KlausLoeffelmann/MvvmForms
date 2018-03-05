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
        Me.txtTypeFilter = New ActiveDevelop.EntitiesFormsLib.NullableTextValue()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.checkCaseSensitive = New System.Windows.Forms.CheckBox()
        Me.SuspendLayout()
        '
        'btnOK
        '
        Me.btnOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnOK.Location = New System.Drawing.Point(332, 386)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(64, 28)
        Me.btnOK.TabIndex = 0
        Me.btnOK.Text = "OK"
        Me.btnOK.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.Location = New System.Drawing.Point(402, 386)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(64, 28)
        Me.btnCancel.TabIndex = 1
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(10, 10)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(94, 32)
        Me.Label2.TabIndex = 15
        Me.Label2.Text = "ViewModel/ DataSource Class:"
        '
        'chkGACAssemblies
        '
        Me.chkGACAssemblies.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.chkGACAssemblies.AutoSize = True
        Me.chkGACAssemblies.Location = New System.Drawing.Point(132, 392)
        Me.chkGACAssemblies.Name = "chkGACAssemblies"
        Me.chkGACAssemblies.Size = New System.Drawing.Size(180, 17)
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
        Me.DataSourceTreeView.Location = New System.Drawing.Point(133, 50)
        Me.DataSourceTreeView.Margin = New System.Windows.Forms.Padding(2)
        Me.DataSourceTreeView.Name = "DataSourceTreeView"
        Me.DataSourceTreeView.Size = New System.Drawing.Size(334, 331)
        Me.DataSourceTreeView.TabIndex = 20
        '
        'txtTypeFilter
        '
        Me.txtTypeFilter.AssignedManagerComponent = Nothing
        Me.txtTypeFilter.Borderstyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtTypeFilter.ContentPresentPermission = ActiveDevelop.EntitiesFormsLib.ContentPresentPermissions.Normal
        Me.txtTypeFilter.Location = New System.Drawing.Point(168, 18)
        Me.txtTypeFilter.MaxLength = 32767
        Me.txtTypeFilter.Name = "txtTypeFilter"
        Me.txtTypeFilter.NullValueString = "* - - - *"
        Me.txtTypeFilter.ObfuscationChar = Nothing
        Me.txtTypeFilter.PermissionReason = Nothing
        Me.txtTypeFilter.Size = New System.Drawing.Size(199, 20)
        Me.txtTypeFilter.TabIndex = 21
        Me.txtTypeFilter.UIGuid = New System.Guid("2f627d9e-4d57-429e-a55d-67400f69a7d7")
        Me.txtTypeFilter.Value = Nothing
        Me.txtTypeFilter.ValueValidationState = Nothing
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(130, 23)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(32, 13)
        Me.Label1.TabIndex = 22
        Me.Label1.Text = "Filter:"
        '
        'checkCaseSensitive
        '
        Me.checkCaseSensitive.AutoSize = True
        Me.checkCaseSensitive.Location = New System.Drawing.Point(373, 19)
        Me.checkCaseSensitive.Name = "checkCaseSensitive"
        Me.checkCaseSensitive.Size = New System.Drawing.Size(96, 17)
        Me.checkCaseSensitive.TabIndex = 23
        Me.checkCaseSensitive.Text = "Case Sensitive"
        Me.checkCaseSensitive.UseVisualStyleBackColor = True
        '
        'DataSourceTypeUIForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(474, 428)
        Me.Controls.Add(Me.checkCaseSensitive)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtTypeFilter)
        Me.Controls.Add(Me.DataSourceTreeView)
        Me.Controls.Add(Me.chkGACAssemblies)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnOK)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MinimumSize = New System.Drawing.Size(375, 210)
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
    Friend WithEvents txtTypeFilter As NullableTextValue
    Friend WithEvents Label1 As Windows.Forms.Label
    Friend WithEvents checkCaseSensitive As Windows.Forms.CheckBox
End Class
