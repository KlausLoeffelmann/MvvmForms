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
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
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
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 320)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(626, 76)
        Me.ToolStrip1.TabIndex = 1
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'AddToolStripButton
        '
        Me.AddToolStripButton.Image = CType(resources.GetObject("AddToolStripButton.Image"), System.Drawing.Image)
        Me.AddToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.AddToolStripButton.Name = "AddToolStripButton"
        Me.AddToolStripButton.Padding = New System.Windows.Forms.Padding(3)
        Me.AddToolStripButton.Size = New System.Drawing.Size(58, 73)
        Me.AddToolStripButton.Text = "Add"
        Me.AddToolStripButton.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.AddToolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        '
        'EditToolStripButton
        '
        Me.EditToolStripButton.Image = CType(resources.GetObject("EditToolStripButton.Image"), System.Drawing.Image)
        Me.EditToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.EditToolStripButton.Name = "EditToolStripButton"
        Me.EditToolStripButton.Padding = New System.Windows.Forms.Padding(3)
        Me.EditToolStripButton.Size = New System.Drawing.Size(58, 73)
        Me.EditToolStripButton.Text = "Edit"
        Me.EditToolStripButton.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.EditToolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        '
        'DeleteToolStripButton
        '
        Me.DeleteToolStripButton.Image = CType(resources.GetObject("DeleteToolStripButton.Image"), System.Drawing.Image)
        Me.DeleteToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.DeleteToolStripButton.Name = "DeleteToolStripButton"
        Me.DeleteToolStripButton.Padding = New System.Windows.Forms.Padding(3)
        Me.DeleteToolStripButton.Size = New System.Drawing.Size(58, 73)
        Me.DeleteToolStripButton.Text = "Delete"
        Me.DeleteToolStripButton.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.DeleteToolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        '
        'myBindingDataGrid
        '
        Me.myBindingDataGrid.AutoGenerateColumns = False
        Me.myBindingDataGrid.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Sunken
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.myBindingDataGrid.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.myBindingDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.myBindingDataGrid.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.ControlPropertyDataGridViewTextBoxColumn, Me.ConverterDataGridViewTextBoxColumn, Me.ConverterParameter, Me.BindingSettingDataGridViewTextBoxColumn, Me.ViewModelPropertyDataGridViewTextBoxColumn})
        Me.myBindingDataGrid.DataSource = Me.PropertyBindingItemBindingSource
        Me.myBindingDataGrid.Dock = System.Windows.Forms.DockStyle.Fill
        Me.myBindingDataGrid.Location = New System.Drawing.Point(0, 0)
        Me.myBindingDataGrid.Name = "myBindingDataGrid"
        Me.myBindingDataGrid.ReadOnly = True
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.myBindingDataGrid.RowHeadersDefaultCellStyle = DataGridViewCellStyle2
        Me.myBindingDataGrid.RowTemplate.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        Me.myBindingDataGrid.RowTemplate.DefaultCellStyle.Padding = New System.Windows.Forms.Padding(3)
        Me.myBindingDataGrid.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.myBindingDataGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.myBindingDataGrid.Size = New System.Drawing.Size(626, 320)
        Me.myBindingDataGrid.TabIndex = 0
        '
        'ControlPropertyDataGridViewTextBoxColumn
        '
        Me.ControlPropertyDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells
        Me.ControlPropertyDataGridViewTextBoxColumn.DataPropertyName = "ControlProperty"
        Me.ControlPropertyDataGridViewTextBoxColumn.HeaderText = "Control Property"
        Me.ControlPropertyDataGridViewTextBoxColumn.Name = "ControlPropertyDataGridViewTextBoxColumn"
        Me.ControlPropertyDataGridViewTextBoxColumn.ReadOnly = True
        Me.ControlPropertyDataGridViewTextBoxColumn.Width = 113
        '
        'ConverterDataGridViewTextBoxColumn
        '
        Me.ConverterDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells
        Me.ConverterDataGridViewTextBoxColumn.DataPropertyName = "Converter"
        Me.ConverterDataGridViewTextBoxColumn.HeaderText = "Converter"
        Me.ConverterDataGridViewTextBoxColumn.Name = "ConverterDataGridViewTextBoxColumn"
        Me.ConverterDataGridViewTextBoxColumn.ReadOnly = True
        Me.ConverterDataGridViewTextBoxColumn.Width = 87
        '
        'ConverterParameter
        '
        Me.ConverterParameter.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells
        Me.ConverterParameter.DataPropertyName = "ConverterParameter"
        Me.ConverterParameter.HeaderText = "Converter Parameter"
        Me.ConverterParameter.Name = "ConverterParameter"
        Me.ConverterParameter.ReadOnly = True
        Me.ConverterParameter.Width = 135
        '
        'BindingSettingDataGridViewTextBoxColumn
        '
        Me.BindingSettingDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells
        Me.BindingSettingDataGridViewTextBoxColumn.DataPropertyName = "BindingSetting"
        Me.BindingSettingDataGridViewTextBoxColumn.HeaderText = "Binding Setting"
        Me.BindingSettingDataGridViewTextBoxColumn.Name = "BindingSettingDataGridViewTextBoxColumn"
        Me.BindingSettingDataGridViewTextBoxColumn.ReadOnly = True
        Me.BindingSettingDataGridViewTextBoxColumn.Width = 108
        '
        'ViewModelPropertyDataGridViewTextBoxColumn
        '
        Me.ViewModelPropertyDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells
        Me.ViewModelPropertyDataGridViewTextBoxColumn.DataPropertyName = "ViewModelProperty"
        Me.ViewModelPropertyDataGridViewTextBoxColumn.HeaderText = "ViewModel Property"
        Me.ViewModelPropertyDataGridViewTextBoxColumn.Name = "ViewModelPropertyDataGridViewTextBoxColumn"
        Me.ViewModelPropertyDataGridViewTextBoxColumn.ReadOnly = True
        Me.ViewModelPropertyDataGridViewTextBoxColumn.Width = 132
        '
        'PropertyBindingItemBindingSource
        '
        Me.PropertyBindingItemBindingSource.DataSource = GetType(ActiveDevelop.EntitiesFormsLib.PropertyBindingItem)
        '
        'ControlBindingGrid
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.myBindingDataGrid)
        Me.Controls.Add(Me.ToolStrip1)
        Me.Name = "ControlBindingGrid"
        Me.Size = New System.Drawing.Size(626, 396)
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
