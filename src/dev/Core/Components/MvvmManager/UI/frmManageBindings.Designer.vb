<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmManageBindings
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
        Me.components = New System.ComponentModel.Container()
        Dim TreeNode1 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Node0")
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmManageBindings))
        Me.ControlTreeView = New System.Windows.Forms.TreeView()
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.MainSplitContainer = New System.Windows.Forms.SplitContainer()
        Me.MainToolStrip = New System.Windows.Forms.ToolStrip()
        Me.ExpandAllToolStripButton = New System.Windows.Forms.ToolStripButton()
        Me.CollapseAllToolStripButton = New System.Windows.Forms.ToolStripButton()
        Me.ExpandCollapseThisToolStripButton = New System.Windows.Forms.ToolStripButton()
        Me.ShowPrintReportButton = New System.Windows.Forms.ToolStripButton()
        Me.SearchToolStripLabel = New System.Windows.Forms.ToolStripLabel()
        Me.FindControlToolStripTextBox = New System.Windows.Forms.ToolStripTextBox()
        CType(Me.MainSplitContainer, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.MainSplitContainer.Panel1.SuspendLayout()
        Me.MainSplitContainer.SuspendLayout()
        Me.MainToolStrip.SuspendLayout()
        Me.SuspendLayout()
        '
        'ControlTreeView
        '
        Me.ControlTreeView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ControlTreeView.FullRowSelect = True
        Me.ControlTreeView.HideSelection = False
        Me.ControlTreeView.ImageIndex = 0
        Me.ControlTreeView.ImageList = Me.ImageList1
        Me.ControlTreeView.Location = New System.Drawing.Point(0, 0)
        Me.ControlTreeView.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.ControlTreeView.Name = "ControlTreeView"
        TreeNode1.Name = "Node0"
        TreeNode1.Text = "Node0"
        Me.ControlTreeView.Nodes.AddRange(New System.Windows.Forms.TreeNode() {TreeNode1})
        Me.ControlTreeView.SelectedImageIndex = 0
        Me.ControlTreeView.ShowNodeToolTips = True
        Me.ControlTreeView.Size = New System.Drawing.Size(399, 809)
        Me.ControlTreeView.TabIndex = 0
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageList1.Images.SetKeyName(0, "fragezeichen.bmp")
        '
        'MainSplitContainer
        '
        Me.MainSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill
        Me.MainSplitContainer.Location = New System.Drawing.Point(0, 27)
        Me.MainSplitContainer.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.MainSplitContainer.Name = "MainSplitContainer"
        '
        'MainSplitContainer.Panel1
        '
        Me.MainSplitContainer.Panel1.Controls.Add(Me.ControlTreeView)
        Me.MainSplitContainer.Size = New System.Drawing.Size(1205, 809)
        Me.MainSplitContainer.SplitterDistance = 399
        Me.MainSplitContainer.SplitterWidth = 5
        Me.MainSplitContainer.TabIndex = 1
        '
        'MainToolStrip
        '
        Me.MainToolStrip.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.MainToolStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ExpandAllToolStripButton, Me.CollapseAllToolStripButton, Me.ExpandCollapseThisToolStripButton, Me.ShowPrintReportButton, Me.SearchToolStripLabel, Me.FindControlToolStripTextBox})
        Me.MainToolStrip.Location = New System.Drawing.Point(0, 0)
        Me.MainToolStrip.Name = "MainToolStrip"
        Me.MainToolStrip.Size = New System.Drawing.Size(1205, 27)
        Me.MainToolStrip.TabIndex = 2
        Me.MainToolStrip.Text = "ToolStrip1"
        '
        'ExpandAllToolStripButton
        '
        Me.ExpandAllToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ExpandAllToolStripButton.Image = CType(resources.GetObject("ExpandAllToolStripButton.Image"), System.Drawing.Image)
        Me.ExpandAllToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ExpandAllToolStripButton.Name = "ExpandAllToolStripButton"
        Me.ExpandAllToolStripButton.Size = New System.Drawing.Size(24, 24)
        Me.ExpandAllToolStripButton.Text = "Expand All"
        '
        'CollapseAllToolStripButton
        '
        Me.CollapseAllToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.CollapseAllToolStripButton.Image = CType(resources.GetObject("CollapseAllToolStripButton.Image"), System.Drawing.Image)
        Me.CollapseAllToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.CollapseAllToolStripButton.Name = "CollapseAllToolStripButton"
        Me.CollapseAllToolStripButton.Size = New System.Drawing.Size(24, 24)
        Me.CollapseAllToolStripButton.Text = "ToolStripButton1"
        Me.CollapseAllToolStripButton.ToolTipText = "Collapse All"
        '
        'ExpandCollapseThisToolStripButton
        '
        Me.ExpandCollapseThisToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ExpandCollapseThisToolStripButton.Image = CType(resources.GetObject("ExpandCollapseThisToolStripButton.Image"), System.Drawing.Image)
        Me.ExpandCollapseThisToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ExpandCollapseThisToolStripButton.Name = "ExpandCollapseThisToolStripButton"
        Me.ExpandCollapseThisToolStripButton.Size = New System.Drawing.Size(24, 24)
        Me.ExpandCollapseThisToolStripButton.Text = "ToolStripButton1"
        Me.ExpandCollapseThisToolStripButton.ToolTipText = "Expand/Collapse Node"
        '
        'ShowPrintReportButton
        '
        Me.ShowPrintReportButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ShowPrintReportButton.Image = CType(resources.GetObject("ShowPrintReportButton.Image"), System.Drawing.Image)
        Me.ShowPrintReportButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ShowPrintReportButton.Name = "ShowPrintReportButton"
        Me.ShowPrintReportButton.Size = New System.Drawing.Size(24, 24)
        Me.ShowPrintReportButton.Text = "ShowPrintReportButton"
        '
        'SearchToolStripLabel
        '
        Me.SearchToolStripLabel.Name = "SearchToolStripLabel"
        Me.SearchToolStripLabel.Size = New System.Drawing.Size(90, 24)
        Me.SearchToolStripLabel.Text = "Find Control"
        '
        'FindControlToolStripTextBox
        '
        Me.FindControlToolStripTextBox.BackColor = System.Drawing.Color.White
        Me.FindControlToolStripTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.FindControlToolStripTextBox.Name = "FindControlToolStripTextBox"
        Me.FindControlToolStripTextBox.Size = New System.Drawing.Size(133, 27)
        '
        'frmManageBindings
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1205, 836)
        Me.Controls.Add(Me.MainSplitContainer)
        Me.Controls.Add(Me.MainToolStrip)
        Me.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.Name = "frmManageBindings"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Assigning of ViewModel Properties to the View"
        Me.MainSplitContainer.Panel1.ResumeLayout(False)
        CType(Me.MainSplitContainer, System.ComponentModel.ISupportInitialize).EndInit()
        Me.MainSplitContainer.ResumeLayout(False)
        Me.MainToolStrip.ResumeLayout(False)
        Me.MainToolStrip.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ControlTreeView As System.Windows.Forms.TreeView
    Friend WithEvents MainSplitContainer As System.Windows.Forms.SplitContainer
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    Friend WithEvents MainToolStrip As System.Windows.Forms.ToolStrip
    Friend WithEvents ExpandAllToolStripButton As System.Windows.Forms.ToolStripButton
    Friend WithEvents CollapseAllToolStripButton As System.Windows.Forms.ToolStripButton
    Friend WithEvents ExpandCollapseThisToolStripButton As System.Windows.Forms.ToolStripButton
    Friend WithEvents SearchToolStripLabel As System.Windows.Forms.ToolStripLabel
    Friend WithEvents FindControlToolStripTextBox As System.Windows.Forms.ToolStripTextBox
    Friend WithEvents ShowPrintReportButton As System.Windows.Forms.ToolStripButton
End Class
