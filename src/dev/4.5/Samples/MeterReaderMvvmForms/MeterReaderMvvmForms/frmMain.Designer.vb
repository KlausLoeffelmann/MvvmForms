<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmMain
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.FileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.NewReadingToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.NewContactToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.NewBuildingToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripSeparator()
        Me.QuitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.EditToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.EditContactToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.EditBuildingToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.MvvmManager1 = New ActiveDevelop.EntitiesFormsLib.MvvmManager(Me.components)
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.MvvmDataGrid1 = New ActiveDevelop.EntitiesFormsLib.MvvmDataGrid()
        Me.MvvmDataGrid2 = New ActiveDevelop.EntitiesFormsLib.MvvmDataGrid()
        Me.MenuStrip1.SuspendLayout()
        CType(Me.MvvmManager1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        CType(Me.MvvmDataGrid1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.MvvmDataGrid2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'MenuStrip1
        '
        Me.MvvmManager1.SetEventBindings(Me.MenuStrip1, Nothing)
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FileToolStripMenuItem, Me.EditToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(712, 24)
        Me.MenuStrip1.TabIndex = 0
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'FileToolStripMenuItem
        '
        Me.FileToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NewReadingToolStripMenuItem, Me.NewContactToolStripMenuItem, Me.NewBuildingToolStripMenuItem, Me.ToolStripMenuItem1, Me.QuitToolStripMenuItem})
        Me.FileToolStripMenuItem.Name = "FileToolStripMenuItem"
        Me.FileToolStripMenuItem.Size = New System.Drawing.Size(37, 20)
        Me.FileToolStripMenuItem.Text = "&File"
        '
        'NewReadingToolStripMenuItem
        '
        Me.NewReadingToolStripMenuItem.Name = "NewReadingToolStripMenuItem"
        Me.NewReadingToolStripMenuItem.Size = New System.Drawing.Size(154, 22)
        Me.NewReadingToolStripMenuItem.Text = "New Reading..."
        '
        'NewContactToolStripMenuItem
        '
        Me.NewContactToolStripMenuItem.Name = "NewContactToolStripMenuItem"
        Me.NewContactToolStripMenuItem.Size = New System.Drawing.Size(154, 22)
        Me.NewContactToolStripMenuItem.Text = "New Contact..."
        '
        'NewBuildingToolStripMenuItem
        '
        Me.NewBuildingToolStripMenuItem.Name = "NewBuildingToolStripMenuItem"
        Me.NewBuildingToolStripMenuItem.Size = New System.Drawing.Size(154, 22)
        Me.NewBuildingToolStripMenuItem.Text = "New Building..."
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(151, 6)
        '
        'QuitToolStripMenuItem
        '
        Me.QuitToolStripMenuItem.Name = "QuitToolStripMenuItem"
        Me.QuitToolStripMenuItem.Size = New System.Drawing.Size(154, 22)
        Me.QuitToolStripMenuItem.Text = "Quit"
        '
        'EditToolStripMenuItem
        '
        Me.EditToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.EditContactToolStripMenuItem, Me.EditBuildingToolStripMenuItem})
        Me.EditToolStripMenuItem.Name = "EditToolStripMenuItem"
        Me.EditToolStripMenuItem.Size = New System.Drawing.Size(39, 20)
        Me.EditToolStripMenuItem.Text = "&Edit"
        '
        'EditContactToolStripMenuItem
        '
        Me.EditContactToolStripMenuItem.Name = "EditContactToolStripMenuItem"
        Me.EditContactToolStripMenuItem.Size = New System.Drawing.Size(150, 22)
        Me.EditContactToolStripMenuItem.Text = "Edit Contact..."
        '
        'EditBuildingToolStripMenuItem
        '
        Me.EditBuildingToolStripMenuItem.Name = "EditBuildingToolStripMenuItem"
        Me.EditBuildingToolStripMenuItem.Size = New System.Drawing.Size(150, 22)
        Me.EditBuildingToolStripMenuItem.Text = "Edit Building..."
        '
        'MvvmManager1
        '
        Me.MvvmManager1.CancelButton = Nothing
        Me.MvvmManager1.ContainerControl = Me
        Me.MvvmManager1.CurrentContextGuid = New System.Guid("861fafc2-3724-48ce-9bcf-d4a6f0dc5f0b")
        Me.MvvmManager1.DataContext = Nothing
        Me.MvvmManager1.DataContextType = Nothing
        Me.MvvmManager1.DataSourceType = Nothing
        Me.MvvmManager1.DirtyStateManagerComponent = Nothing
        Me.MvvmManager1.DynamicEventHandlingList = Nothing
        Me.MvvmManager1.HostingForm = Me
        Me.MvvmManager1.HostingUserControl = Nothing
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.MvvmManager1.SetEventBindings(Me.SplitContainer1, Nothing)
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 24)
        Me.SplitContainer1.Name = "SplitContainer1"
        Me.SplitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.MvvmDataGrid1)
        Me.MvvmManager1.SetEventBindings(Me.SplitContainer1.Panel1, Nothing)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.MvvmDataGrid2)
        Me.MvvmManager1.SetEventBindings(Me.SplitContainer1.Panel2, Nothing)
        Me.SplitContainer1.Size = New System.Drawing.Size(712, 487)
        Me.SplitContainer1.SplitterDistance = 237
        Me.SplitContainer1.TabIndex = 1
        '
        'MvvmDataGrid1
        '
        Me.MvvmDataGrid1.AutoGenerateColumns = False
        Me.MvvmDataGrid1.CanUserAddRows = False
        Me.MvvmDataGrid1.CanUserDeleteRows = False
        Me.MvvmDataGrid1.CustomColumnTemplateType = Nothing
        Me.MvvmDataGrid1.DataSourceType = GetType(MRViewModelLibrary.BuildingViewModel)
        Me.MvvmDataGrid1.EnterAction = Nothing
        Me.MvvmManager1.SetEventBindings(Me.MvvmDataGrid1, Nothing)
        Me.MvvmDataGrid1.GridLinesVisibility = System.Windows.Controls.DataGridGridLinesVisibility.All
        Me.MvvmDataGrid1.ItemsSource = Nothing
        Me.MvvmDataGrid1.Location = New System.Drawing.Point(12, 12)
        Me.MvvmDataGrid1.Name = "MvvmDataGrid1"
        Me.MvvmDataGrid1.SelectedItem = Nothing
        Me.MvvmDataGrid1.SelectionMode = System.Windows.Controls.DataGridSelectionMode.[Single]
        Me.MvvmDataGrid1.Size = New System.Drawing.Size(688, 206)
        Me.MvvmDataGrid1.TabIndex = 0
        '
        'MvvmDataGrid2
        '
        Me.MvvmDataGrid2.AutoGenerateColumns = True
        Me.MvvmDataGrid2.CanUserAddRows = False
        Me.MvvmDataGrid2.CanUserDeleteRows = False
        Me.MvvmDataGrid2.CustomColumnTemplateType = Nothing
        Me.MvvmDataGrid2.DataSourceType = Nothing
        Me.MvvmDataGrid2.EnterAction = Nothing
        Me.MvvmManager1.SetEventBindings(Me.MvvmDataGrid2, Nothing)
        Me.MvvmDataGrid2.GridLinesVisibility = System.Windows.Controls.DataGridGridLinesVisibility.All
        Me.MvvmDataGrid2.ItemsSource = Nothing
        Me.MvvmDataGrid2.Location = New System.Drawing.Point(12, 14)
        Me.MvvmDataGrid2.Name = "MvvmDataGrid2"
        Me.MvvmDataGrid2.SelectedItem = Nothing
        Me.MvvmDataGrid2.SelectionMode = System.Windows.Controls.DataGridSelectionMode.[Single]
        Me.MvvmDataGrid2.Size = New System.Drawing.Size(688, 220)
        Me.MvvmDataGrid2.TabIndex = 0
        '
        'frmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(712, 511)
        Me.Controls.Add(Me.SplitContainer1)
        Me.Controls.Add(Me.MenuStrip1)
        Me.MvvmManager1.SetEventBindings(Me, Nothing)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "frmMain"
        Me.Text = "Meter Reader Manager - MvvmForms Demo"
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        CType(Me.MvvmManager1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        CType(Me.MvvmDataGrid1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.MvvmDataGrid2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents MenuStrip1 As MenuStrip
    Friend WithEvents FileToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents NewReadingToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents NewContactToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents NewBuildingToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem1 As ToolStripSeparator
    Friend WithEvents QuitToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents EditToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents EditContactToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents EditBuildingToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents MvvmManager1 As ActiveDevelop.EntitiesFormsLib.MvvmManager
    Friend WithEvents SplitContainer1 As SplitContainer
    Friend WithEvents MvvmDataGrid1 As ActiveDevelop.EntitiesFormsLib.MvvmDataGrid
    Friend WithEvents MvvmDataGrid2 As ActiveDevelop.EntitiesFormsLib.MvvmDataGrid
End Class
