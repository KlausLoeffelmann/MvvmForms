<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MainForm
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
        Me.MainMenuStrip = New System.Windows.Forms.MenuStrip()
        Me.DateiToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.QuitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.TestToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.RunBasicUITestToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.MVVMGridDemoToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ZoomingTestToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.NullableNumValue1 = New ActiveDevelop.EntitiesFormsLib.NullableNumValue()
        Me.NullableValueRelationPopup2 = New ActiveDevelop.EntitiesFormsLib.NullableValueRelationPopup()
        Me.NullableValueRelationPopup1 = New ActiveDevelop.EntitiesFormsLib.NullableValueRelationPopup()
        Me.ZoomToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem2 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem3 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem4 = New System.Windows.Forms.ToolStripMenuItem()
        Me.MainMenuStrip.SuspendLayout
        CType(Me.NullableValueRelationPopup2,System.ComponentModel.ISupportInitialize).BeginInit
        CType(Me.NullableValueRelationPopup1,System.ComponentModel.ISupportInitialize).BeginInit
        Me.SuspendLayout
        '
        'MainMenuStrip
        '
        Me.MainMenuStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.DateiToolStripMenuItem, Me.TestToolStripMenuItem, Me.ZoomToolStripMenuItem})
        Me.MainMenuStrip.Location = New System.Drawing.Point(0, 0)
        Me.MainMenuStrip.Name = "MainMenuStrip"
        Me.MainMenuStrip.Size = New System.Drawing.Size(945, 24)
        Me.MainMenuStrip.TabIndex = 0
        Me.MainMenuStrip.Text = "MenuStrip1"
        '
        'DateiToolStripMenuItem
        '
        Me.DateiToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.QuitToolStripMenuItem})
        Me.DateiToolStripMenuItem.Name = "DateiToolStripMenuItem"
        Me.DateiToolStripMenuItem.Size = New System.Drawing.Size(46, 20)
        Me.DateiToolStripMenuItem.Text = "&Datei"
        '
        'QuitToolStripMenuItem
        '
        Me.QuitToolStripMenuItem.Name = "QuitToolStripMenuItem"
        Me.QuitToolStripMenuItem.Size = New System.Drawing.Size(97, 22)
        Me.QuitToolStripMenuItem.Text = "&Quit"
        '
        'TestToolStripMenuItem
        '
        Me.TestToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.RunBasicUITestToolStripMenuItem, Me.MVVMGridDemoToolStripMenuItem, Me.ZoomingTestToolStripMenuItem})
        Me.TestToolStripMenuItem.Name = "TestToolStripMenuItem"
        Me.TestToolStripMenuItem.Size = New System.Drawing.Size(41, 20)
        Me.TestToolStripMenuItem.Text = "&Test"
        '
        'RunBasicUITestToolStripMenuItem
        '
        Me.RunBasicUITestToolStripMenuItem.Name = "RunBasicUITestToolStripMenuItem"
        Me.RunBasicUITestToolStripMenuItem.Size = New System.Drawing.Size(170, 22)
        Me.RunBasicUITestToolStripMenuItem.Text = "Show basic test UI"
        '
        'MVVMGridDemoToolStripMenuItem
        '
        Me.MVVMGridDemoToolStripMenuItem.Name = "MVVMGridDemoToolStripMenuItem"
        Me.MVVMGridDemoToolStripMenuItem.Size = New System.Drawing.Size(170, 22)
        Me.MVVMGridDemoToolStripMenuItem.Text = "MVVM Grid Demo"
        '
        'ZoomingTestToolStripMenuItem
        '
        Me.ZoomingTestToolStripMenuItem.Name = "ZoomingTestToolStripMenuItem"
        Me.ZoomingTestToolStripMenuItem.Size = New System.Drawing.Size(170, 22)
        Me.ZoomingTestToolStripMenuItem.Text = "Zooming Test"
        '
        'NullableNumValue1
        '
        Me.NullableNumValue1.AssignedManagerComponent = Nothing
        Me.NullableNumValue1.Borderstyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.NullableNumValue1.ContentPresentPermission = ActiveDevelop.EntitiesFormsLib.ContentPresentPermissions.Normal
        Me.NullableNumValue1.CurrencySymbolString = Nothing
        Me.NullableNumValue1.Location = New System.Drawing.Point(21, 53)
        Me.NullableNumValue1.MaxLength = 32767
        Me.NullableNumValue1.MaxValue = New Decimal(New Integer() {5, 0, 0, 0})
        Me.NullableNumValue1.MinValue = New Decimal(New Integer() {0, 0, 0, 0})
        Me.NullableNumValue1.Name = "NullableNumValue1"
        Me.NullableNumValue1.ObfuscationChar = Nothing
        Me.NullableNumValue1.PermissionReason = Nothing
        Me.NullableNumValue1.Size = New System.Drawing.Size(266, 20)
        Me.NullableNumValue1.TabIndex = 6
        Me.NullableNumValue1.UIGuid = New System.Guid("46b920b4-6450-4b70-a5ba-fb45e95e005f")
        Me.NullableNumValue1.Value = Nothing
        Me.NullableNumValue1.ValueValidationState = Nothing
        '
        'NullableValueRelationPopup2
        '
        Me.NullableValueRelationPopup2.AssignedManagerComponent = Nothing
        Me.NullableValueRelationPopup2.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.None
        Me.NullableValueRelationPopup2.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.None
        Me.NullableValueRelationPopup2.BeepOnFailedValidation = false
        Me.NullableValueRelationPopup2.Borderstyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.NullableValueRelationPopup2.ContentPresentPermission = ActiveDevelop.EntitiesFormsLib.ContentPresentPermissions.Normal
        Me.NullableValueRelationPopup2.DataSource = Nothing
        Me.NullableValueRelationPopup2.DisplayMember = Nothing
        Me.NullableValueRelationPopup2.HasAddButton = true
        Me.NullableValueRelationPopup2.HideButtons = false
        Me.NullableValueRelationPopup2.IsPopupAutoSize = false
        Me.NullableValueRelationPopup2.IsPopupResizable = true
        Me.NullableValueRelationPopup2.Location = New System.Drawing.Point(675, 52)
        Me.NullableValueRelationPopup2.MinimumPopupSize = New System.Drawing.Size(311, 80)
        Me.NullableValueRelationPopup2.MultiSelect = false
        Me.NullableValueRelationPopup2.Name = "NullableValueRelationPopup2"
        Me.NullableValueRelationPopup2.NullValueString = "* - - - *"
        Me.NullableValueRelationPopup2.PermissionReason = Nothing
        Me.NullableValueRelationPopup2.SearchColumnHeaderFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold)
        Me.NullableValueRelationPopup2.Size = New System.Drawing.Size(233, 20)
        Me.NullableValueRelationPopup2.TabIndex = 5
        Me.NullableValueRelationPopup2.UIGuid = New System.Guid("71bdc40d-2956-4a56-af6b-79ef44251f0c")
        Me.NullableValueRelationPopup2.ValueMember = Nothing
        '
        'NullableValueRelationPopup1
        '
        Me.NullableValueRelationPopup1.AssignedManagerComponent = Nothing
        Me.NullableValueRelationPopup1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.None
        Me.NullableValueRelationPopup1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.None
        Me.NullableValueRelationPopup1.BeepOnFailedValidation = false
        Me.NullableValueRelationPopup1.Borderstyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.NullableValueRelationPopup1.ContentPresentPermission = ActiveDevelop.EntitiesFormsLib.ContentPresentPermissions.Normal
        Me.NullableValueRelationPopup1.DataSource = Nothing
        Me.NullableValueRelationPopup1.DisplayMember = Nothing
        Me.NullableValueRelationPopup1.HasAddButton = true
        Me.NullableValueRelationPopup1.HideButtons = false
        Me.NullableValueRelationPopup1.IsPopupAutoSize = false
        Me.NullableValueRelationPopup1.IsPopupResizable = true
        Me.NullableValueRelationPopup1.Location = New System.Drawing.Point(675, 95)
        Me.NullableValueRelationPopup1.MinimumPopupSize = New System.Drawing.Size(311, 80)
        Me.NullableValueRelationPopup1.MultiSelect = false
        Me.NullableValueRelationPopup1.Name = "NullableValueRelationPopup1"
        Me.NullableValueRelationPopup1.NullValueString = "* - - - *"
        Me.NullableValueRelationPopup1.PermissionReason = Nothing
        Me.NullableValueRelationPopup1.SearchColumnHeaderFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold)
        Me.NullableValueRelationPopup1.Size = New System.Drawing.Size(233, 20)
        Me.NullableValueRelationPopup1.TabIndex = 4
        Me.NullableValueRelationPopup1.UIGuid = New System.Guid("d38add81-45e8-436d-8891-6f1770a86ca7")
        Me.NullableValueRelationPopup1.ValueMember = Nothing
        '
        'ZoomToolStripMenuItem
        '
        Me.ZoomToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItem2, Me.ToolStripMenuItem3, Me.ToolStripMenuItem4})
        Me.ZoomToolStripMenuItem.Name = "ZoomToolStripMenuItem"
        Me.ZoomToolStripMenuItem.Size = New System.Drawing.Size(51, 20)
        Me.ZoomToolStripMenuItem.Text = "Zoom"
        '
        'ToolStripMenuItem2
        '
        Me.ToolStripMenuItem2.Name = "ToolStripMenuItem2"
        Me.ToolStripMenuItem2.Size = New System.Drawing.Size(152, 22)
        Me.ToolStripMenuItem2.Text = "100 %"
        '
        'ToolStripMenuItem3
        '
        Me.ToolStripMenuItem3.Name = "ToolStripMenuItem3"
        Me.ToolStripMenuItem3.Size = New System.Drawing.Size(152, 22)
        Me.ToolStripMenuItem3.Text = "125 %"
        '
        'ToolStripMenuItem4
        '
        Me.ToolStripMenuItem4.Name = "ToolStripMenuItem4"
        Me.ToolStripMenuItem4.Size = New System.Drawing.Size(152, 22)
        Me.ToolStripMenuItem4.Text = "150 %"
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6!, 13!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(945, 519)
        Me.Controls.Add(Me.NullableNumValue1)
        Me.Controls.Add(Me.NullableValueRelationPopup2)
        Me.Controls.Add(Me.NullableValueRelationPopup1)
        Me.Controls.Add(Me.MainMenuStrip)
        Me.Name = "MainForm"
        Me.Text = "EntityFormsLib TestContainer"
        Me.MainMenuStrip.ResumeLayout(false)
        Me.MainMenuStrip.PerformLayout
        CType(Me.NullableValueRelationPopup2,System.ComponentModel.ISupportInitialize).EndInit
        CType(Me.NullableValueRelationPopup1,System.ComponentModel.ISupportInitialize).EndInit
        Me.ResumeLayout(false)
        Me.PerformLayout

End Sub
    Friend WithEvents MainMenuStrip As System.Windows.Forms.MenuStrip
    Friend WithEvents DateiToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents QuitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents TestToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents RunBasicUITestToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents NullableValueRelationPopup2 As ActiveDevelop.EntitiesFormsLib.NullableValueRelationPopup
    Friend WithEvents NullableValueRelationPopup1 As ActiveDevelop.EntitiesFormsLib.NullableValueRelationPopup
    Friend WithEvents NullableNumValue1 As ActiveDevelop.EntitiesFormsLib.NullableNumValue
    Friend WithEvents MVVMGridDemoToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ZoomingTestToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ZoomToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem2 As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem3 As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem4 As ToolStripMenuItem
End Class
