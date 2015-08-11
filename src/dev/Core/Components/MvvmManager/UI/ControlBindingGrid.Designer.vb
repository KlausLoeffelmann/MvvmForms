Imports System.ComponentModel

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class ControlBindingGrid
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ControlBindingGrid))
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
        Me.AddToolStripButton = New System.Windows.Forms.ToolStripButton()
        Me.EditToolStripButton = New System.Windows.Forms.ToolStripButton()
        Me.DeleteToolStripButton = New System.Windows.Forms.ToolStripButton()
        Me.myBindingDataGrid = New System.Windows.Forms.DataGridView()
        Me.ControlPropertyDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ConverterDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ConverterParameter = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.BindingSettingDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ViewModelPropertyDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.PropertyBindingItemBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.ToolStrip1.SuspendLayout()
        CType(Me.myBindingDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PropertyBindingItemBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ToolStrip1
        '
        Me.ToolStrip1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.ToolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.ToolStrip1.ImageScalingSize = New System.Drawing.Size(48, 48)
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AddToolStripButton, Me.EditToolStripButton, Me.DeleteToolStripButton})
        Me.ToolStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 424)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(639, 70)
        Me.ToolStrip1.TabIndex = 1
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'AddToolStripButton
        '
        Me.AddToolStripButton.Image = CType(resources.GetObject("AddToolStripButton.Image"), System.Drawing.Image)
        Me.AddToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.AddToolStripButton.Name = "AddToolStripButton"
        Me.AddToolStripButton.Size = New System.Drawing.Size(52, 67)
        Me.AddToolStripButton.Text = "Add"
        Me.AddToolStripButton.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.AddToolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        '
        'EditToolStripButton
        '
        Me.EditToolStripButton.Image = CType(resources.GetObject("EditToolStripButton.Image"), System.Drawing.Image)
        Me.EditToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.EditToolStripButton.Name = "EditToolStripButton"
        Me.EditToolStripButton.Size = New System.Drawing.Size(52, 67)
        Me.EditToolStripButton.Text = "Edit"
        Me.EditToolStripButton.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.EditToolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        '
        'DeleteToolStripButton
        '
        Me.DeleteToolStripButton.Image = CType(resources.GetObject("DeleteToolStripButton.Image"), System.Drawing.Image)
        Me.DeleteToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.DeleteToolStripButton.Name = "DeleteToolStripButton"
        Me.DeleteToolStripButton.Size = New System.Drawing.Size(52, 67)
        Me.DeleteToolStripButton.Text = "Delete"
        Me.DeleteToolStripButton.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.DeleteToolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        '
        'myBindingDataGrid
        '
        Me.myBindingDataGrid.AutoGenerateColumns = False
        Me.myBindingDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.myBindingDataGrid.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.ControlPropertyDataGridViewTextBoxColumn, Me.ConverterDataGridViewTextBoxColumn, Me.ConverterParameter, Me.BindingSettingDataGridViewTextBoxColumn, Me.ViewModelPropertyDataGridViewTextBoxColumn})
        Me.myBindingDataGrid.DataSource = Me.PropertyBindingItemBindingSource
        Me.myBindingDataGrid.Dock = System.Windows.Forms.DockStyle.Fill
        Me.myBindingDataGrid.Location = New System.Drawing.Point(0, 0)
        Me.myBindingDataGrid.Name = "myBindingDataGrid"
        Me.myBindingDataGrid.ReadOnly = True
        Me.myBindingDataGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.myBindingDataGrid.Size = New System.Drawing.Size(639, 494)
        Me.myBindingDataGrid.TabIndex = 0
        '
        'ControlPropertyDataGridViewTextBoxColumn
        '
        Me.ControlPropertyDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells
        Me.ControlPropertyDataGridViewTextBoxColumn.DataPropertyName = "ControlProperty"
        Me.ControlPropertyDataGridViewTextBoxColumn.HeaderText = "Control Property"
        Me.ControlPropertyDataGridViewTextBoxColumn.Name = "ControlPropertyDataGridViewTextBoxColumn"
        Me.ControlPropertyDataGridViewTextBoxColumn.ReadOnly = True
        Me.ControlPropertyDataGridViewTextBoxColumn.Width = 98
        '
        'ConverterDataGridViewTextBoxColumn
        '
        Me.ConverterDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells
        Me.ConverterDataGridViewTextBoxColumn.DataPropertyName = "Converter"
        Me.ConverterDataGridViewTextBoxColumn.HeaderText = "Converter"
        Me.ConverterDataGridViewTextBoxColumn.Name = "ConverterDataGridViewTextBoxColumn"
        Me.ConverterDataGridViewTextBoxColumn.ReadOnly = True
        Me.ConverterDataGridViewTextBoxColumn.Width = 78
        '
        'ConverterParameter
        '
        Me.ConverterParameter.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells
        Me.ConverterParameter.DataPropertyName = "ConverterParameter"
        Me.ConverterParameter.HeaderText = "Converter Parameter"
        Me.ConverterParameter.Name = "ConverterParameter"
        Me.ConverterParameter.ReadOnly = True
        Me.ConverterParameter.Width = 118
        '
        'BindingSettingDataGridViewTextBoxColumn
        '
        Me.BindingSettingDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells
        Me.BindingSettingDataGridViewTextBoxColumn.DataPropertyName = "BindingSetting"
        Me.BindingSettingDataGridViewTextBoxColumn.HeaderText = "Binding Setting"
        Me.BindingSettingDataGridViewTextBoxColumn.Name = "BindingSettingDataGridViewTextBoxColumn"
        Me.BindingSettingDataGridViewTextBoxColumn.ReadOnly = True
        Me.BindingSettingDataGridViewTextBoxColumn.Width = 95
        '
        'ViewModelPropertyDataGridViewTextBoxColumn
        '
        Me.ViewModelPropertyDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells
        Me.ViewModelPropertyDataGridViewTextBoxColumn.DataPropertyName = "ViewModelProperty"
        Me.ViewModelPropertyDataGridViewTextBoxColumn.HeaderText = "ViewModel Property"
        Me.ViewModelPropertyDataGridViewTextBoxColumn.Name = "ViewModelPropertyDataGridViewTextBoxColumn"
        Me.ViewModelPropertyDataGridViewTextBoxColumn.ReadOnly = True
        Me.ViewModelPropertyDataGridViewTextBoxColumn.Width = 115
        '
        'PropertyBindingItemBindingSource
        '
        Me.PropertyBindingItemBindingSource.DataSource = GetType(ActiveDevelop.EntitiesFormsLib.PropertyBindingItem)
        '
        'ControlBindingGrid
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.ToolStrip1)
        Me.Controls.Add(Me.myBindingDataGrid)
        Me.Name = "ControlBindingGrid"
        Me.Size = New System.Drawing.Size(639, 494)
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        CType(Me.myBindingDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PropertyBindingItemBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents myBindingDataGrid As System.Windows.Forms.DataGridView
    Friend WithEvents PropertyBindingItemBindingSource As System.Windows.Forms.BindingSource
    Friend WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
    Friend WithEvents AddToolStripButton As System.Windows.Forms.ToolStripButton
    Friend WithEvents EditToolStripButton As System.Windows.Forms.ToolStripButton
    Friend WithEvents DeleteToolStripButton As System.Windows.Forms.ToolStripButton
    Friend WithEvents ControlPropertyDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ConverterDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ConverterParameter As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents BindingSettingDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ViewModelPropertyDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn

End Class
