<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class EmptyMvvmDataGridForm
    Inherits System.Windows.Forms.Form

    'Das Formular überschreibt den Löschvorgang, um die Komponentenliste zu bereinigen.
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
        Dim MvvmGrid_column As ActiveDevelop.EntitiesFormsLib.MvvmDataGridColumn = New ActiveDevelop.EntitiesFormsLib.MvvmDataGridColumn()
        Me.MvvmGrid = New ActiveDevelop.EntitiesFormsLib.MvvmDataGrid()
        MvvmGrid_column.TextWrapping = System.Windows.TextWrapping.NoWrap
        MvvmGrid_column.FontWeight = System.Windows.FontWeight.FromOpenTypeWeight(400)
        'TODO: Ausnahme "Value cannot be null.
        'Parameter name: e" beim Generieren des Codes für "".
        MvvmGrid_column.Width = -1.0R
        MvvmGrid_column.WidthLengthUnitType = System.Windows.Controls.DataGridLengthUnitType.Star
        MvvmGrid_column.Visibility = System.Windows.Visibility.Visible
        MvvmGrid_column.CellPadding = New System.Windows.Forms.Padding(0, 0, 0, 0)
        MvvmGrid_column.Header = "Blabla"
        MvvmGrid_column.IsEnabled = True
        MvvmGrid_column.BackgroundColor = System.Drawing.Color.Empty
        MvvmGrid_column.ForegroundColor = System.Drawing.Color.Empty
        MvvmGrid_column.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch
        MvvmGrid_column.VerticalAlignment = System.Windows.VerticalAlignment.Top
        MvvmGrid_column.ColumnHeaderPadding = New System.Windows.Forms.Padding(0)
        MvvmGrid_column.Name = "column"
        MvvmGrid_column.ColumnType = ActiveDevelop.EntitiesFormsLib.ColumnType.TextAndNumbers
        CType(Me.MvvmGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'MvvmGrid
        '
        Me.MvvmGrid.AutoGenerateColumns = False
        Me.MvvmGrid.AutoSize = True
        Me.MvvmGrid.CanUserAddRows = False
        Me.MvvmGrid.CanUserDeleteRows = False
        Me.MvvmGrid.CanUserSortColumns = True
        Me.MvvmGrid.Columns.Add(MvvmGrid_column)
        Me.MvvmGrid.CustomColumnTemplateType = Nothing
        Me.MvvmGrid.DataSourceType = Nothing
        Me.MvvmGrid.EnterAction = Nothing
        Me.MvvmGrid.FilterCaseSensitive = True
        Me.MvvmGrid.GridLinesVisibility = System.Windows.Controls.DataGridGridLinesVisibility.All
        Me.MvvmGrid.HeadersVisibility = CType((System.Windows.Controls.DataGridHeadersVisibility.Column Or System.Windows.Controls.DataGridHeadersVisibility.Row), System.Windows.Controls.DataGridHeadersVisibility)
        Me.MvvmGrid.IsFilteringEnabled = False
        Me.MvvmGrid.IsReadOnly = False
        Me.MvvmGrid.ItemsSource = Nothing
        Me.MvvmGrid.Location = New System.Drawing.Point(12, 12)
        Me.MvvmGrid.Name = "MvvmGrid"
        Me.MvvmGrid.SelectedIndex = -1
        Me.MvvmGrid.SelectedItem = Nothing
        Me.MvvmGrid.SelectionMode = System.Windows.Controls.DataGridSelectionMode.[Single]
        Me.MvvmGrid.Size = New System.Drawing.Size(260, 237)
        Me.MvvmGrid.TabIndex = 0
        '
        'EmptyMvvmDataGridForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(284, 261)
        Me.Controls.Add(Me.MvvmGrid)
        Me.Name = "EmptyMvvmDataGridForm"
        Me.Text = "EmptyMvvmDataGridForm"
        CType(Me.MvvmGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents MvvmGrid As ActiveDevelop.EntitiesFormsLib.MvvmDataGrid
End Class
