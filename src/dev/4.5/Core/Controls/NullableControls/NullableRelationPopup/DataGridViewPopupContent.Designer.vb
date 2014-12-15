<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class DataGridViewPopupContent
    Inherits System.Windows.Forms.UserControl

    'UserControl überschreibt den Löschvorgang, um die Komponentenliste zu bereinigen.
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

    'Wird vom Windows Form-Designer benötigt.
    Private components As System.ComponentModel.IContainer

    'Hinweis: Die folgende Prozedur ist für den Windows Form-Designer erforderlich.
    'Das Bearbeiten ist mit dem Windows Form-Designer möglich.  
    'Das Bearbeiten mit dem Code-Editor ist nicht möglich.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.BindableDataGridView = New ActiveDevelop.EntitiesFormsLib.BindableDataGridView()
        CType(Me.BindableDataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'BindableDataGridView
        '
        Me.BindableDataGridView.AllowUserToAddRows = False
        Me.BindableDataGridView.AllowUserToDeleteRows = False
        Me.BindableDataGridView.AllowUserToResizeRows = False
        Me.BindableDataGridView.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.BindableDataGridView.AssignedManagerComponent = Nothing
        Me.BindableDataGridView.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.BindableDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.BindableDataGridView.ContentPresentPermission = ActiveDevelop.EntitiesFormsLib.ContentPresentPermissions.Normal
        Me.BindableDataGridView.DatafieldDescription = Nothing
        Me.BindableDataGridView.DatafieldName = Nothing
        Me.BindableDataGridView.Location = New System.Drawing.Point(0, 0)
        Me.BindableDataGridView.Name = "BindableDataGridView"
        Me.BindableDataGridView.NullValueMessage = Nothing
        Me.BindableDataGridView.PermissionReason = Nothing
        Me.BindableDataGridView.PreventFirstRowSelection = False
        Me.BindableDataGridView.ReadOnly = True
        Me.BindableDataGridView.RowHeadersVisible = False
        Me.BindableDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.BindableDataGridView.Size = New System.Drawing.Size(249, 183)
        Me.BindableDataGridView.TabIndex = 4
        Me.BindableDataGridView.UIGuid = New System.Guid("0498b165-c84f-412a-bf68-0455a689a852")
        Me.BindableDataGridView.Value = Nothing
        Me.BindableDataGridView.ValueBase = Nothing
        Me.BindableDataGridView.ValueMember = Nothing
        '
        'DataGridViewPopupContent
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.BindableDataGridView)
        Me.Name = "DataGridViewPopupContent"
        Me.Size = New System.Drawing.Size(249, 184)
        CType(Me.BindableDataGridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents BindableDataGridView As ActiveDevelop.EntitiesFormsLib.BindableDataGridView

End Class
