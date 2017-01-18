<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMvvmEventAssignment
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
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.FormToolTip = New System.Windows.Forms.ToolTip(Me.components)
        Me.TableLayoutPanel2 = New System.Windows.Forms.TableLayoutPanel()
        Me.lblCurrentControlType = New System.Windows.Forms.Label()
        Me.lblCurrentViewModelFullName = New System.Windows.Forms.Label()
        Me.lblCurrentViewModelType = New System.Windows.Forms.Label()
        Me.lblCurrentControl = New System.Windows.Forms.Label()
        Me.EventBindingGrid = New ActiveDevelop.EntitiesFormsLib.ControlBindingGrid()
        Me.nvrControlEvent = New ActiveDevelop.EntitiesFormsLib.NullableValueRelationPopup()
        Me.nvrControlCanExecuteProperty = New ActiveDevelop.EntitiesFormsLib.NullableValueRelationPopup()
        Me.nvrViewModelCommand = New ActiveDevelop.EntitiesFormsLib.NullableValueRelationPopup()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.TableLayoutPanel2.SuspendLayout()
        CType(Me.nvrControlEvent, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nvrControlCanExecuteProperty, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nvrViewModelCommand, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label4
        '
        Me.Label4.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(0, 3)
        Me.Label4.Margin = New System.Windows.Forms.Padding(0, 0, 3, 0)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(93, 19)
        Me.Label4.TabIndex = 8
        Me.Label4.Text = "Control Event"
        '
        'Label5
        '
        Me.Label5.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(436, 3)
        Me.Label5.Margin = New System.Windows.Forms.Padding(0, 0, 3, 0)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(145, 19)
        Me.Label5.TabIndex = 10
        Me.Label5.Text = "View model command"
        '
        'Label7
        '
        Me.Label7.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(218, 3)
        Me.Label7.Margin = New System.Windows.Forms.Padding(0, 0, 3, 0)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(160, 19)
        Me.Label7.TabIndex = 15
        Me.Label7.Text = "Control CanExe Property"
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel1.ColumnCount = 3
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.TableLayoutPanel1.Controls.Add(Me.EventBindingGrid, 0, 2)
        Me.TableLayoutPanel1.Controls.Add(Me.Label4, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.Label7, 1, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.Label5, 2, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.nvrControlEvent, 0, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.nvrControlCanExecuteProperty, 1, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.nvrViewModelCommand, 2, 1)
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(12, 100)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 3
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(656, 261)
        Me.TableLayoutPanel1.TabIndex = 16
        '
        'TableLayoutPanel2
        '
        Me.TableLayoutPanel2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel2.ColumnCount = 2
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 46.0!))
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 54.0!))
        Me.TableLayoutPanel2.Controls.Add(Me.lblCurrentControlType, 0, 1)
        Me.TableLayoutPanel2.Controls.Add(Me.lblCurrentViewModelFullName, 0, 1)
        Me.TableLayoutPanel2.Controls.Add(Me.lblCurrentViewModelType, 1, 0)
        Me.TableLayoutPanel2.Controls.Add(Me.lblCurrentControl, 0, 0)
        Me.TableLayoutPanel2.Location = New System.Drawing.Point(12, 12)
        Me.TableLayoutPanel2.Name = "TableLayoutPanel2"
        Me.TableLayoutPanel2.RowCount = 2
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 66.66666!))
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.TableLayoutPanel2.Size = New System.Drawing.Size(656, 65)
        Me.TableLayoutPanel2.TabIndex = 18
        '
        'lblCurrentControlType
        '
        Me.lblCurrentControlType.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lblCurrentControlType.AutoEllipsis = True
        Me.lblCurrentControlType.AutoSize = True
        Me.lblCurrentControlType.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCurrentControlType.Location = New System.Drawing.Point(10, 43)
        Me.lblCurrentControlType.Margin = New System.Windows.Forms.Padding(10, 0, 3, 0)
        Me.lblCurrentControlType.Name = "lblCurrentControlType"
        Me.lblCurrentControlType.Size = New System.Drawing.Size(129, 22)
        Me.lblCurrentControlType.TabIndex = 5
        Me.lblCurrentControlType.Text = "ViewModelType"
        '
        'lblCurrentViewModelFullName
        '
        Me.lblCurrentViewModelFullName.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lblCurrentViewModelFullName.AutoEllipsis = True
        Me.lblCurrentViewModelFullName.AutoSize = True
        Me.lblCurrentViewModelFullName.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCurrentViewModelFullName.Location = New System.Drawing.Point(311, 43)
        Me.lblCurrentViewModelFullName.Margin = New System.Windows.Forms.Padding(10, 0, 3, 0)
        Me.lblCurrentViewModelFullName.Name = "lblCurrentViewModelFullName"
        Me.lblCurrentViewModelFullName.Size = New System.Drawing.Size(67, 22)
        Me.lblCurrentViewModelFullName.TabIndex = 4
        Me.lblCurrentViewModelFullName.Text = "Control"
        '
        'lblCurrentViewModelType
        '
        Me.lblCurrentViewModelType.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lblCurrentViewModelType.AutoEllipsis = True
        Me.lblCurrentViewModelType.AutoSize = True
        Me.lblCurrentViewModelType.Font = New System.Drawing.Font("Segoe UI", 20.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCurrentViewModelType.Location = New System.Drawing.Point(304, 0)
        Me.lblCurrentViewModelType.Name = "lblCurrentViewModelType"
        Me.lblCurrentViewModelType.Size = New System.Drawing.Size(259, 43)
        Me.lblCurrentViewModelType.TabIndex = 3
        Me.lblCurrentViewModelType.Text = "ViewModelType"
        '
        'lblCurrentControl
        '
        Me.lblCurrentControl.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lblCurrentControl.AutoEllipsis = True
        Me.lblCurrentControl.AutoSize = True
        Me.lblCurrentControl.Font = New System.Drawing.Font("Segoe UI", 20.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCurrentControl.Location = New System.Drawing.Point(3, 0)
        Me.lblCurrentControl.Name = "lblCurrentControl"
        Me.lblCurrentControl.Size = New System.Drawing.Size(132, 43)
        Me.lblCurrentControl.TabIndex = 2
        Me.lblCurrentControl.Text = "Control"
        '
        'EventBindingGrid
        '
        Me.EventBindingGrid.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel1.SetColumnSpan(Me.EventBindingGrid, 3)
        Me.EventBindingGrid.Location = New System.Drawing.Point(3, 58)
        Me.EventBindingGrid.Margin = New System.Windows.Forms.Padding(3, 8, 3, 3)
        Me.EventBindingGrid.Name = "EventBindingGrid"
        Me.EventBindingGrid.Size = New System.Drawing.Size(650, 200)
        Me.EventBindingGrid.TabIndex = 17
        '
        'nvrControlEvent
        '
        Me.nvrControlEvent.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.nvrControlEvent.AssignedManagerComponent = Nothing
        Me.nvrControlEvent.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.None
        Me.nvrControlEvent.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.None
        Me.nvrControlEvent.Borderstyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.nvrControlEvent.ContentPresentPermission = ActiveDevelop.EntitiesFormsLib.ContentPresentPermissions.Normal
        Me.nvrControlEvent.DataSource = Nothing
        Me.nvrControlEvent.DisplayMember = """{0}"", {EventName}"
        Me.nvrControlEvent.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.nvrControlEvent.HideButtons = False
        Me.nvrControlEvent.IsPopupAutoSize = False
        Me.nvrControlEvent.IsPopupResizable = True
        Me.nvrControlEvent.Location = New System.Drawing.Point(3, 28)
        Me.nvrControlEvent.MaxLength = 32767
        Me.nvrControlEvent.MinimumPopupSize = New System.Drawing.Size(223, 80)
        Me.nvrControlEvent.MultiSelect = False
        Me.nvrControlEvent.Name = "nvrControlEvent"
        Me.nvrControlEvent.NullValueString = "* - - - *"
        Me.nvrControlEvent.PermissionReason = Nothing
        Me.nvrControlEvent.PreferredVisibleColumnsOnOpen = 2
        Me.nvrControlEvent.PreferredVisibleRowsOnOpen = 10
        Me.nvrControlEvent.Searchable = True
        Me.nvrControlEvent.SearchColumnBackgroundColor = System.Drawing.Color.Empty
        Me.nvrControlEvent.SearchColumnHeaderBackgroundColor = System.Drawing.Color.Empty
        Me.nvrControlEvent.SearchColumnHeaderFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold)
        Me.nvrControlEvent.Size = New System.Drawing.Size(212, 26)
        Me.nvrControlEvent.TabIndex = 7
        Me.nvrControlEvent.UIGuid = New System.Guid("7b05d4b5-998f-4216-ae43-12e7bd51942c")
        Me.nvrControlEvent.ValueMember = Nothing
        '
        'nvrControlCanExecuteProperty
        '
        Me.nvrControlCanExecuteProperty.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.nvrControlCanExecuteProperty.AssignedManagerComponent = Nothing
        Me.nvrControlCanExecuteProperty.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.None
        Me.nvrControlCanExecuteProperty.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.None
        Me.nvrControlCanExecuteProperty.Borderstyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.nvrControlCanExecuteProperty.ContentPresentPermission = ActiveDevelop.EntitiesFormsLib.ContentPresentPermissions.Normal
        Me.nvrControlCanExecuteProperty.DataSource = Nothing
        Me.nvrControlCanExecuteProperty.DisplayMember = """{0}"", {PropertyName}"
        Me.nvrControlCanExecuteProperty.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.nvrControlCanExecuteProperty.HideButtons = False
        Me.nvrControlCanExecuteProperty.IsPopupAutoSize = False
        Me.nvrControlCanExecuteProperty.IsPopupResizable = True
        Me.nvrControlCanExecuteProperty.Location = New System.Drawing.Point(221, 28)
        Me.nvrControlCanExecuteProperty.MaxLength = 32767
        Me.nvrControlCanExecuteProperty.MinimumPopupSize = New System.Drawing.Size(223, 80)
        Me.nvrControlCanExecuteProperty.MultiSelect = False
        Me.nvrControlCanExecuteProperty.Name = "nvrControlCanExecuteProperty"
        Me.nvrControlCanExecuteProperty.NullValueString = "* - - - *"
        Me.nvrControlCanExecuteProperty.PermissionReason = Nothing
        Me.nvrControlCanExecuteProperty.PreferredVisibleColumnsOnOpen = 2
        Me.nvrControlCanExecuteProperty.PreferredVisibleRowsOnOpen = 10
        Me.nvrControlCanExecuteProperty.Searchable = True
        Me.nvrControlCanExecuteProperty.SearchColumnBackgroundColor = System.Drawing.Color.Empty
        Me.nvrControlCanExecuteProperty.SearchColumnHeaderBackgroundColor = System.Drawing.Color.Empty
        Me.nvrControlCanExecuteProperty.SearchColumnHeaderFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold)
        Me.nvrControlCanExecuteProperty.Size = New System.Drawing.Size(212, 26)
        Me.nvrControlCanExecuteProperty.TabIndex = 14
        Me.nvrControlCanExecuteProperty.UIGuid = New System.Guid("7b05d4b5-998f-4216-ae43-12e7bd51942c")
        Me.nvrControlCanExecuteProperty.ValueMember = Nothing
        '
        'nvrViewModelCommand
        '
        Me.nvrViewModelCommand.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.nvrViewModelCommand.AssignedManagerComponent = Nothing
        Me.nvrViewModelCommand.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.None
        Me.nvrViewModelCommand.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.None
        Me.nvrViewModelCommand.Borderstyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.nvrViewModelCommand.ContentPresentPermission = ActiveDevelop.EntitiesFormsLib.ContentPresentPermissions.Normal
        Me.nvrViewModelCommand.DataSource = Nothing
        Me.nvrViewModelCommand.DisplayMember = Nothing
        Me.nvrViewModelCommand.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.nvrViewModelCommand.HideButtons = False
        Me.nvrViewModelCommand.IsPopupAutoSize = False
        Me.nvrViewModelCommand.IsPopupResizable = True
        Me.nvrViewModelCommand.Location = New System.Drawing.Point(439, 28)
        Me.nvrViewModelCommand.MaxLength = 32767
        Me.nvrViewModelCommand.MinimumPopupSize = New System.Drawing.Size(342, 80)
        Me.nvrViewModelCommand.MultiSelect = False
        Me.nvrViewModelCommand.Name = "nvrViewModelCommand"
        Me.nvrViewModelCommand.NullValueString = "* - - - *"
        Me.nvrViewModelCommand.PermissionReason = Nothing
        Me.nvrViewModelCommand.PreferredVisibleColumnsOnOpen = 2
        Me.nvrViewModelCommand.PreferredVisibleRowsOnOpen = 10
        Me.nvrViewModelCommand.Searchable = True
        Me.nvrViewModelCommand.SearchColumnBackgroundColor = System.Drawing.Color.Empty
        Me.nvrViewModelCommand.SearchColumnHeaderBackgroundColor = System.Drawing.Color.Empty
        Me.nvrViewModelCommand.SearchColumnHeaderFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold)
        Me.nvrViewModelCommand.Size = New System.Drawing.Size(214, 26)
        Me.nvrViewModelCommand.TabIndex = 9
        Me.nvrViewModelCommand.UIGuid = New System.Guid("7b05d4b5-998f-4216-ae43-12e7bd51942c")
        Me.nvrViewModelCommand.ValueMember = Nothing
        '
        'frmMvvmEventAssignment
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 19.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(679, 373)
        Me.Controls.Add(Me.TableLayoutPanel2)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "frmMvvmEventAssignment"
        Me.Text = "Mvvm Event Assignment"
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.TableLayoutPanel1.PerformLayout()
        Me.TableLayoutPanel2.ResumeLayout(False)
        Me.TableLayoutPanel2.PerformLayout()
        CType(Me.nvrControlEvent, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nvrControlCanExecuteProperty, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nvrViewModelCommand, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents nvrControlEvent As ActiveDevelop.EntitiesFormsLib.NullableValueRelationPopup
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents nvrViewModelCommand As ActiveDevelop.EntitiesFormsLib.NullableValueRelationPopup
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents nvrControlCanExecuteProperty As ActiveDevelop.EntitiesFormsLib.NullableValueRelationPopup
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents EventBindingGrid As ActiveDevelop.EntitiesFormsLib.ControlBindingGrid
    Friend WithEvents FormToolTip As System.Windows.Forms.ToolTip
    Friend WithEvents TableLayoutPanel2 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents lblCurrentControlType As System.Windows.Forms.Label
    Friend WithEvents lblCurrentViewModelFullName As System.Windows.Forms.Label
    Friend WithEvents lblCurrentViewModelType As System.Windows.Forms.Label
    Friend WithEvents lblCurrentControl As System.Windows.Forms.Label
End Class
