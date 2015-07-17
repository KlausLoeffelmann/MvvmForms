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
        Dim MvvmDataGrid1_idNumColumn As ActiveDevelop.EntitiesFormsLib.MvvmDataGridColumn = New ActiveDevelop.EntitiesFormsLib.MvvmDataGridColumn()
        Dim MvvmDataGrid1_BuildYearColumn As ActiveDevelop.EntitiesFormsLib.MvvmDataGridColumn = New ActiveDevelop.EntitiesFormsLib.MvvmDataGridColumn()
        Dim MvvmDataGrid1_DescriptionColumn As ActiveDevelop.EntitiesFormsLib.MvvmDataGridColumn = New ActiveDevelop.EntitiesFormsLib.MvvmDataGridColumn()
        Dim MvvmDataGrid1_CityColumn As ActiveDevelop.EntitiesFormsLib.MvvmDataGridColumn = New ActiveDevelop.EntitiesFormsLib.MvvmDataGridColumn()
        Dim MvvmDataGrid1_ZipColumn As ActiveDevelop.EntitiesFormsLib.MvvmDataGridColumn = New ActiveDevelop.EntitiesFormsLib.MvvmDataGridColumn()
        Dim MvvmDataGrid1_CountryColumn As ActiveDevelop.EntitiesFormsLib.MvvmDataGridColumn = New ActiveDevelop.EntitiesFormsLib.MvvmDataGridColumn()
        Dim MvvmDataGrid2_MeterIdColumn As ActiveDevelop.EntitiesFormsLib.MvvmDataGridColumn = New ActiveDevelop.EntitiesFormsLib.MvvmDataGridColumn()
        Dim MvvmDataGrid2_LocalDescriptionColumn As ActiveDevelop.EntitiesFormsLib.MvvmDataGridColumn = New ActiveDevelop.EntitiesFormsLib.MvvmDataGridColumn()
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
        Me.MvvmDataGrid1 = New ActiveDevelop.EntitiesFormsLib.MvvmDataGrid()
        Me.MvvmDataGrid2 = New ActiveDevelop.EntitiesFormsLib.MvvmDataGrid()
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.TextBox2 = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        MvvmDataGrid1_idNumColumn.DataSourceType = GetType(MRViewModelLibrary.BuildingViewModel)
        MvvmDataGrid1_idNumColumn.FontWeight = System.Windows.FontWeight.FromOpenTypeWeight(400)
        'TODO: Code generation for '' failed because of Exception 'Value cannot be null.
        'Parameter name: e'.
        MvvmDataGrid1_idNumColumn.Width = -1.0R
        MvvmDataGrid1_idNumColumn.WidthLengthUnitType = System.Windows.Controls.DataGridLengthUnitType.Star
        MvvmDataGrid1_idNumColumn.Visibility = System.Windows.Visibility.Visible
        MvvmDataGrid1_idNumColumn.CellPadding = New System.Windows.Forms.Padding(0, 0, 0, 0)
        MvvmDataGrid1_idNumColumn.Header = "idNum"
        MvvmDataGrid1_idNumColumn.IsEnabled = True
        MvvmDataGrid1_idNumColumn.BackgroundColor = System.Drawing.Color.Empty
        MvvmDataGrid1_idNumColumn.ForegroundColor = System.Drawing.Color.Empty
        MvvmDataGrid1_idNumColumn.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch
        MvvmDataGrid1_idNumColumn.VerticalAlignment = System.Windows.VerticalAlignment.Top
        MvvmDataGrid1_idNumColumn.ColumnHeaderPadding = New System.Windows.Forms.Padding(0)
        MvvmDataGrid1_idNumColumn.Name = "idNumColumn"
        MvvmDataGrid1_idNumColumn.ColumnType = ActiveDevelop.EntitiesFormsLib.ColumnType.TextAndNumbers
        MvvmDataGrid1_BuildYearColumn.DataSourceType = GetType(MRViewModelLibrary.BuildingViewModel)
        MvvmDataGrid1_BuildYearColumn.FontWeight = System.Windows.FontWeight.FromOpenTypeWeight(400)
        'TODO: Code generation for '' failed because of Exception 'Value cannot be null.
        'Parameter name: e'.
        MvvmDataGrid1_BuildYearColumn.Width = -1.0R
        MvvmDataGrid1_BuildYearColumn.WidthLengthUnitType = System.Windows.Controls.DataGridLengthUnitType.Star
        MvvmDataGrid1_BuildYearColumn.Visibility = System.Windows.Visibility.Visible
        MvvmDataGrid1_BuildYearColumn.CellPadding = New System.Windows.Forms.Padding(0, 0, 0, 0)
        MvvmDataGrid1_BuildYearColumn.Header = "BuildYear"
        MvvmDataGrid1_BuildYearColumn.IsEnabled = True
        MvvmDataGrid1_BuildYearColumn.BackgroundColor = System.Drawing.Color.Empty
        MvvmDataGrid1_BuildYearColumn.ForegroundColor = System.Drawing.Color.Empty
        MvvmDataGrid1_BuildYearColumn.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch
        MvvmDataGrid1_BuildYearColumn.VerticalAlignment = System.Windows.VerticalAlignment.Top
        MvvmDataGrid1_BuildYearColumn.ColumnHeaderPadding = New System.Windows.Forms.Padding(0)
        MvvmDataGrid1_BuildYearColumn.Name = "BuildYearColumn"
        MvvmDataGrid1_BuildYearColumn.ColumnType = ActiveDevelop.EntitiesFormsLib.ColumnType.TextAndNumbers
        MvvmDataGrid1_DescriptionColumn.DataSourceType = GetType(MRViewModelLibrary.BuildingViewModel)
        MvvmDataGrid1_DescriptionColumn.FontWeight = System.Windows.FontWeight.FromOpenTypeWeight(400)
        'TODO: Code generation for '' failed because of Exception 'Value cannot be null.
        'Parameter name: e'.
        MvvmDataGrid1_DescriptionColumn.Width = -1.0R
        MvvmDataGrid1_DescriptionColumn.WidthLengthUnitType = System.Windows.Controls.DataGridLengthUnitType.Star
        MvvmDataGrid1_DescriptionColumn.Visibility = System.Windows.Visibility.Visible
        MvvmDataGrid1_DescriptionColumn.CellPadding = New System.Windows.Forms.Padding(0, 0, 0, 0)
        MvvmDataGrid1_DescriptionColumn.Header = "Description"
        MvvmDataGrid1_DescriptionColumn.IsEnabled = True
        MvvmDataGrid1_DescriptionColumn.BackgroundColor = System.Drawing.Color.Empty
        MvvmDataGrid1_DescriptionColumn.ForegroundColor = System.Drawing.Color.Empty
        MvvmDataGrid1_DescriptionColumn.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch
        MvvmDataGrid1_DescriptionColumn.VerticalAlignment = System.Windows.VerticalAlignment.Top
        MvvmDataGrid1_DescriptionColumn.ColumnHeaderPadding = New System.Windows.Forms.Padding(0)
        MvvmDataGrid1_DescriptionColumn.Name = "DescriptionColumn"
        MvvmDataGrid1_DescriptionColumn.ColumnType = ActiveDevelop.EntitiesFormsLib.ColumnType.TextAndNumbers
        MvvmDataGrid1_CityColumn.DataSourceType = GetType(MRViewModelLibrary.BuildingViewModel)
        MvvmDataGrid1_CityColumn.FontWeight = System.Windows.FontWeight.FromOpenTypeWeight(400)
        'TODO: Code generation for '' failed because of Exception 'Value cannot be null.
        'Parameter name: e'.
        MvvmDataGrid1_CityColumn.Width = -1.0R
        MvvmDataGrid1_CityColumn.WidthLengthUnitType = System.Windows.Controls.DataGridLengthUnitType.Star
        MvvmDataGrid1_CityColumn.Visibility = System.Windows.Visibility.Visible
        MvvmDataGrid1_CityColumn.CellPadding = New System.Windows.Forms.Padding(0, 0, 0, 0)
        MvvmDataGrid1_CityColumn.Header = "City"
        MvvmDataGrid1_CityColumn.IsEnabled = True
        MvvmDataGrid1_CityColumn.BackgroundColor = System.Drawing.Color.Empty
        MvvmDataGrid1_CityColumn.ForegroundColor = System.Drawing.Color.Empty
        MvvmDataGrid1_CityColumn.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch
        MvvmDataGrid1_CityColumn.VerticalAlignment = System.Windows.VerticalAlignment.Top
        MvvmDataGrid1_CityColumn.ColumnHeaderPadding = New System.Windows.Forms.Padding(0)
        MvvmDataGrid1_CityColumn.Name = "CityColumn"
        MvvmDataGrid1_CityColumn.ColumnType = ActiveDevelop.EntitiesFormsLib.ColumnType.TextAndNumbers
        MvvmDataGrid1_ZipColumn.DataSourceType = GetType(MRViewModelLibrary.BuildingViewModel)
        MvvmDataGrid1_ZipColumn.FontWeight = System.Windows.FontWeight.FromOpenTypeWeight(400)
        'TODO: Code generation for '' failed because of Exception 'Value cannot be null.
        'Parameter name: e'.
        MvvmDataGrid1_ZipColumn.Width = -1.0R
        MvvmDataGrid1_ZipColumn.WidthLengthUnitType = System.Windows.Controls.DataGridLengthUnitType.Star
        MvvmDataGrid1_ZipColumn.Visibility = System.Windows.Visibility.Visible
        MvvmDataGrid1_ZipColumn.CellPadding = New System.Windows.Forms.Padding(0, 0, 0, 0)
        MvvmDataGrid1_ZipColumn.Header = "Zip"
        MvvmDataGrid1_ZipColumn.IsEnabled = True
        MvvmDataGrid1_ZipColumn.BackgroundColor = System.Drawing.Color.Empty
        MvvmDataGrid1_ZipColumn.ForegroundColor = System.Drawing.Color.Empty
        MvvmDataGrid1_ZipColumn.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch
        MvvmDataGrid1_ZipColumn.VerticalAlignment = System.Windows.VerticalAlignment.Top
        MvvmDataGrid1_ZipColumn.ColumnHeaderPadding = New System.Windows.Forms.Padding(0)
        MvvmDataGrid1_ZipColumn.Name = "ZipColumn"
        MvvmDataGrid1_ZipColumn.ColumnType = ActiveDevelop.EntitiesFormsLib.ColumnType.TextAndNumbers
        MvvmDataGrid1_CountryColumn.DataSourceType = GetType(MRViewModelLibrary.BuildingViewModel)
        MvvmDataGrid1_CountryColumn.FontWeight = System.Windows.FontWeight.FromOpenTypeWeight(400)
        'TODO: Code generation for '' failed because of Exception 'Value cannot be null.
        'Parameter name: e'.
        MvvmDataGrid1_CountryColumn.Width = -1.0R
        MvvmDataGrid1_CountryColumn.WidthLengthUnitType = System.Windows.Controls.DataGridLengthUnitType.Star
        MvvmDataGrid1_CountryColumn.Visibility = System.Windows.Visibility.Visible
        MvvmDataGrid1_CountryColumn.CellPadding = New System.Windows.Forms.Padding(0, 0, 0, 0)
        MvvmDataGrid1_CountryColumn.Header = "Country"
        MvvmDataGrid1_CountryColumn.IsEnabled = True
        MvvmDataGrid1_CountryColumn.BackgroundColor = System.Drawing.Color.Empty
        MvvmDataGrid1_CountryColumn.ForegroundColor = System.Drawing.Color.Empty
        MvvmDataGrid1_CountryColumn.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch
        MvvmDataGrid1_CountryColumn.VerticalAlignment = System.Windows.VerticalAlignment.Top
        MvvmDataGrid1_CountryColumn.ColumnHeaderPadding = New System.Windows.Forms.Padding(0)
        MvvmDataGrid1_CountryColumn.Name = "CountryColumn"
        MvvmDataGrid1_CountryColumn.ColumnType = ActiveDevelop.EntitiesFormsLib.ColumnType.TextAndNumbers
        MvvmDataGrid2_MeterIdColumn.DataSourceType = GetType(MRViewModelLibrary.MeterViewModel)
        MvvmDataGrid2_MeterIdColumn.FontWeight = System.Windows.FontWeight.FromOpenTypeWeight(400)
        'TODO: Code generation for '' failed because of Exception 'Value cannot be null.
        'Parameter name: e'.
        MvvmDataGrid2_MeterIdColumn.Width = 1.0R
        MvvmDataGrid2_MeterIdColumn.WidthLengthUnitType = System.Windows.Controls.DataGridLengthUnitType.Star
        MvvmDataGrid2_MeterIdColumn.Visibility = System.Windows.Visibility.Visible
        MvvmDataGrid2_MeterIdColumn.CellPadding = New System.Windows.Forms.Padding(0, 0, 0, 0)
        MvvmDataGrid2_MeterIdColumn.Header = "MeterId"
        MvvmDataGrid2_MeterIdColumn.IsEnabled = True
        MvvmDataGrid2_MeterIdColumn.BackgroundColor = System.Drawing.Color.Empty
        MvvmDataGrid2_MeterIdColumn.ForegroundColor = System.Drawing.Color.Empty
        MvvmDataGrid2_MeterIdColumn.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch
        MvvmDataGrid2_MeterIdColumn.VerticalAlignment = System.Windows.VerticalAlignment.Top
        MvvmDataGrid2_MeterIdColumn.ColumnHeaderPadding = New System.Windows.Forms.Padding(0)
        MvvmDataGrid2_MeterIdColumn.Name = "MeterIdColumn"
        MvvmDataGrid2_MeterIdColumn.ColumnType = ActiveDevelop.EntitiesFormsLib.ColumnType.TextAndNumbers
        MvvmDataGrid2_LocalDescriptionColumn.DataSourceType = GetType(MRViewModelLibrary.MeterViewModel)
        MvvmDataGrid2_LocalDescriptionColumn.FontWeight = System.Windows.FontWeight.FromOpenTypeWeight(400)
        'TODO: Code generation for '' failed because of Exception 'Value cannot be null.
        'Parameter name: e'.
        MvvmDataGrid2_LocalDescriptionColumn.Width = 5.0R
        MvvmDataGrid2_LocalDescriptionColumn.WidthLengthUnitType = System.Windows.Controls.DataGridLengthUnitType.Star
        MvvmDataGrid2_LocalDescriptionColumn.Visibility = System.Windows.Visibility.Visible
        MvvmDataGrid2_LocalDescriptionColumn.CellPadding = New System.Windows.Forms.Padding(0, 0, 0, 0)
        MvvmDataGrid2_LocalDescriptionColumn.Header = "LocalDescription"
        MvvmDataGrid2_LocalDescriptionColumn.IsEnabled = True
        MvvmDataGrid2_LocalDescriptionColumn.BackgroundColor = System.Drawing.Color.Empty
        MvvmDataGrid2_LocalDescriptionColumn.ForegroundColor = System.Drawing.Color.Empty
        MvvmDataGrid2_LocalDescriptionColumn.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch
        MvvmDataGrid2_LocalDescriptionColumn.VerticalAlignment = System.Windows.VerticalAlignment.Top
        MvvmDataGrid2_LocalDescriptionColumn.ColumnHeaderPadding = New System.Windows.Forms.Padding(0)
        MvvmDataGrid2_LocalDescriptionColumn.Name = "LocalDescriptionColumn"
        MvvmDataGrid2_LocalDescriptionColumn.ColumnType = ActiveDevelop.EntitiesFormsLib.ColumnType.TextAndNumbers
        Me.MenuStrip1.SuspendLayout()
        CType(Me.MvvmManager1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.MvvmDataGrid1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.MvvmDataGrid2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'MenuStrip1
        '
        Me.MvvmManager1.SetEventBindings(Me.MenuStrip1, Nothing)
        Me.MenuStrip1.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FileToolStripMenuItem, Me.EditToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Padding = New System.Windows.Forms.Padding(8, 2, 0, 2)
        Me.MenuStrip1.Size = New System.Drawing.Size(949, 28)
        Me.MenuStrip1.TabIndex = 0
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'FileToolStripMenuItem
        '
        Me.FileToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NewReadingToolStripMenuItem, Me.NewContactToolStripMenuItem, Me.NewBuildingToolStripMenuItem, Me.ToolStripMenuItem1, Me.QuitToolStripMenuItem})
        Me.FileToolStripMenuItem.Name = "FileToolStripMenuItem"
        Me.FileToolStripMenuItem.Size = New System.Drawing.Size(44, 24)
        Me.FileToolStripMenuItem.Text = "&File"
        '
        'NewReadingToolStripMenuItem
        '
        Me.NewReadingToolStripMenuItem.Name = "NewReadingToolStripMenuItem"
        Me.NewReadingToolStripMenuItem.Size = New System.Drawing.Size(182, 26)
        Me.NewReadingToolStripMenuItem.Text = "New Reading..."
        '
        'NewContactToolStripMenuItem
        '
        Me.NewContactToolStripMenuItem.Name = "NewContactToolStripMenuItem"
        Me.NewContactToolStripMenuItem.Size = New System.Drawing.Size(182, 26)
        Me.NewContactToolStripMenuItem.Text = "New Contact..."
        '
        'NewBuildingToolStripMenuItem
        '
        Me.NewBuildingToolStripMenuItem.Name = "NewBuildingToolStripMenuItem"
        Me.NewBuildingToolStripMenuItem.Size = New System.Drawing.Size(182, 26)
        Me.NewBuildingToolStripMenuItem.Text = "New Building..."
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(179, 6)
        '
        'QuitToolStripMenuItem
        '
        Me.QuitToolStripMenuItem.Name = "QuitToolStripMenuItem"
        Me.QuitToolStripMenuItem.Size = New System.Drawing.Size(182, 26)
        Me.QuitToolStripMenuItem.Text = "Quit"
        '
        'EditToolStripMenuItem
        '
        Me.EditToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.EditContactToolStripMenuItem, Me.EditBuildingToolStripMenuItem})
        Me.EditToolStripMenuItem.Name = "EditToolStripMenuItem"
        Me.EditToolStripMenuItem.Size = New System.Drawing.Size(47, 24)
        Me.EditToolStripMenuItem.Text = "&Edit"
        '
        'EditContactToolStripMenuItem
        '
        Me.EditContactToolStripMenuItem.Name = "EditContactToolStripMenuItem"
        Me.EditContactToolStripMenuItem.Size = New System.Drawing.Size(178, 26)
        Me.EditContactToolStripMenuItem.Text = "Edit Contact..."
        '
        'EditBuildingToolStripMenuItem
        '
        Me.EditBuildingToolStripMenuItem.Name = "EditBuildingToolStripMenuItem"
        Me.EditBuildingToolStripMenuItem.Size = New System.Drawing.Size(178, 26)
        Me.EditBuildingToolStripMenuItem.Text = "Edit Building..."
        '
        'MvvmManager1
        '
        Me.MvvmManager1.CancelButton = Nothing
        Me.MvvmManager1.ContainerControl = Me
        Me.MvvmManager1.CurrentContextGuid = New System.Guid("861fafc2-3724-48ce-9bcf-d4a6f0dc5f0b")
        Me.MvvmManager1.DataContext = Nothing
        Me.MvvmManager1.DataContextType = GetType(MRViewModelLibrary.MainViewModel)
        Me.MvvmManager1.DataSourceType = GetType(MRViewModelLibrary.MainViewModel)
        Me.MvvmManager1.DirtyStateManagerComponent = Nothing
        Me.MvvmManager1.DynamicEventHandlingList = Nothing
        Me.MvvmManager1.HostingForm = Me
        Me.MvvmManager1.HostingUserControl = Nothing
        Me.MvvmManager1.MvvmBindings.AddPropertyBinding(Me.MvvmDataGrid1, New ActiveDevelop.EntitiesFormsLib.BindingSetting(ActiveDevelop.EntitiesFormsLib.MvvmBindingModes.TwoWay, ActiveDevelop.EntitiesFormsLib.UpdateSourceTriggerSettings.PropertyChangedImmediately), New ActiveDevelop.EntitiesFormsLib.BindingProperty("ItemsSource", GetType(System.Collections.IEnumerable)), Nothing, Nothing, New ActiveDevelop.EntitiesFormsLib.BindingProperty("Buildings", GetType(System.Collections.ObjectModel.ObservableCollection(Of MRViewModelLibrary.BuildingViewModel))))
        Me.MvvmManager1.MvvmBindings.AddPropertyBinding(Me.MvvmDataGrid1, New ActiveDevelop.EntitiesFormsLib.BindingSetting(ActiveDevelop.EntitiesFormsLib.MvvmBindingModes.TwoWay, ActiveDevelop.EntitiesFormsLib.UpdateSourceTriggerSettings.PropertyChangedImmediately), New ActiveDevelop.EntitiesFormsLib.BindingProperty("SelectedItem", GetType(Object)), Nothing, Nothing, New ActiveDevelop.EntitiesFormsLib.BindingProperty("SelectedBuilding", GetType(MRViewModelLibrary.BuildingViewModel)))
        Me.MvvmManager1.MvvmBindings.AddPropertyBinding(Me.MvvmDataGrid2, New ActiveDevelop.EntitiesFormsLib.BindingSetting(ActiveDevelop.EntitiesFormsLib.MvvmBindingModes.OneWay, ActiveDevelop.EntitiesFormsLib.UpdateSourceTriggerSettings.PropertyChangedImmediately), New ActiveDevelop.EntitiesFormsLib.BindingProperty("ItemsSource", GetType(System.Collections.IEnumerable)), Nothing, Nothing, New ActiveDevelop.EntitiesFormsLib.BindingProperty("MetersForBuilding.Value", GetType(System.Collections.ObjectModel.ObservableCollection(Of MRViewModelLibrary.MeterViewModel))))
        '
        'MvvmDataGrid1
        '
        Me.MvvmDataGrid1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.MvvmDataGrid1.AutoGenerateColumns = False
        Me.MvvmDataGrid1.CanUserAddRows = False
        Me.MvvmDataGrid1.CanUserDeleteRows = False
        MvvmDataGrid1_idNumColumn.PropertyCellBindings.Add(New ActiveDevelop.EntitiesFormsLib.BindingSetting(ActiveDevelop.EntitiesFormsLib.MvvmBindingModes.TwoWay, ActiveDevelop.EntitiesFormsLib.UpdateSourceTriggerSettings.LostFocus), New ActiveDevelop.EntitiesFormsLib.BindingProperty("Content", GetType(ActiveDevelop.EntitiesFormsLib.MvvmDataGridColumn)), Nothing, Nothing, New ActiveDevelop.EntitiesFormsLib.BindingProperty("idNum", GetType(MRViewModelLibrary.BuildingViewModel)))
        MvvmDataGrid1_idNumColumn.PropertyCellBindings.Add(New ActiveDevelop.EntitiesFormsLib.BindingSetting(ActiveDevelop.EntitiesFormsLib.MvvmBindingModes.TwoWay, ActiveDevelop.EntitiesFormsLib.UpdateSourceTriggerSettings.PropertyChangedImmediately), New ActiveDevelop.EntitiesFormsLib.BindingProperty("CellPadding", GetType(System.Windows.Forms.Padding)), GetType(MeterReaderMvvmForms.IntegerToPaddingConverter), Nothing, New ActiveDevelop.EntitiesFormsLib.BindingProperty("TableCellPadding", GetType(Integer)))
        MvvmDataGrid1_idNumColumn.PropertyCellBindings.Add(New ActiveDevelop.EntitiesFormsLib.BindingSetting(ActiveDevelop.EntitiesFormsLib.MvvmBindingModes.TwoWay, ActiveDevelop.EntitiesFormsLib.UpdateSourceTriggerSettings.PropertyChangedImmediately), New ActiveDevelop.EntitiesFormsLib.BindingProperty("BackgroundColor", GetType(System.Drawing.Color)), GetType(MeterReaderMvvmForms.BooleanToColorConverter), Nothing, New ActiveDevelop.EntitiesFormsLib.BindingProperty("AlternatingColorFlag", GetType(Boolean)))
        Me.MvvmDataGrid1.Columns.Add(MvvmDataGrid1_idNumColumn)
        MvvmDataGrid1_BuildYearColumn.PropertyCellBindings.Add(New ActiveDevelop.EntitiesFormsLib.BindingSetting(ActiveDevelop.EntitiesFormsLib.MvvmBindingModes.TwoWay, ActiveDevelop.EntitiesFormsLib.UpdateSourceTriggerSettings.LostFocus), New ActiveDevelop.EntitiesFormsLib.BindingProperty("Content", GetType(ActiveDevelop.EntitiesFormsLib.MvvmDataGridColumn)), Nothing, Nothing, New ActiveDevelop.EntitiesFormsLib.BindingProperty("BuildYear", GetType(MRViewModelLibrary.BuildingViewModel)))
        MvvmDataGrid1_BuildYearColumn.PropertyCellBindings.Add(New ActiveDevelop.EntitiesFormsLib.BindingSetting(ActiveDevelop.EntitiesFormsLib.MvvmBindingModes.TwoWay, ActiveDevelop.EntitiesFormsLib.UpdateSourceTriggerSettings.PropertyChangedImmediately), New ActiveDevelop.EntitiesFormsLib.BindingProperty("BackgroundColor", GetType(System.Drawing.Color)), GetType(MeterReaderMvvmForms.BooleanToColorConverter), Nothing, New ActiveDevelop.EntitiesFormsLib.BindingProperty("AlternatingColorFlag", GetType(Boolean)))
        Me.MvvmDataGrid1.Columns.Add(MvvmDataGrid1_BuildYearColumn)
        MvvmDataGrid1_DescriptionColumn.PropertyCellBindings.Add(New ActiveDevelop.EntitiesFormsLib.BindingSetting(ActiveDevelop.EntitiesFormsLib.MvvmBindingModes.TwoWay, ActiveDevelop.EntitiesFormsLib.UpdateSourceTriggerSettings.LostFocus), New ActiveDevelop.EntitiesFormsLib.BindingProperty("Content", GetType(ActiveDevelop.EntitiesFormsLib.MvvmDataGridColumn)), Nothing, Nothing, New ActiveDevelop.EntitiesFormsLib.BindingProperty("Description", GetType(MRViewModelLibrary.BuildingViewModel)))
        MvvmDataGrid1_DescriptionColumn.PropertyCellBindings.Add(New ActiveDevelop.EntitiesFormsLib.BindingSetting(ActiveDevelop.EntitiesFormsLib.MvvmBindingModes.TwoWay, ActiveDevelop.EntitiesFormsLib.UpdateSourceTriggerSettings.PropertyChangedImmediately), New ActiveDevelop.EntitiesFormsLib.BindingProperty("BackgroundColor", GetType(System.Drawing.Color)), GetType(MeterReaderMvvmForms.BooleanToColorConverter), Nothing, New ActiveDevelop.EntitiesFormsLib.BindingProperty("AlternatingColorFlag", GetType(Boolean)))
        Me.MvvmDataGrid1.Columns.Add(MvvmDataGrid1_DescriptionColumn)
        MvvmDataGrid1_CityColumn.PropertyCellBindings.Add(New ActiveDevelop.EntitiesFormsLib.BindingSetting(ActiveDevelop.EntitiesFormsLib.MvvmBindingModes.TwoWay, ActiveDevelop.EntitiesFormsLib.UpdateSourceTriggerSettings.LostFocus), New ActiveDevelop.EntitiesFormsLib.BindingProperty("Content", GetType(ActiveDevelop.EntitiesFormsLib.MvvmDataGridColumn)), Nothing, Nothing, New ActiveDevelop.EntitiesFormsLib.BindingProperty("City", GetType(MRViewModelLibrary.BuildingViewModel)))
        MvvmDataGrid1_CityColumn.PropertyCellBindings.Add(New ActiveDevelop.EntitiesFormsLib.BindingSetting(ActiveDevelop.EntitiesFormsLib.MvvmBindingModes.TwoWay, ActiveDevelop.EntitiesFormsLib.UpdateSourceTriggerSettings.PropertyChangedImmediately), New ActiveDevelop.EntitiesFormsLib.BindingProperty("BackgroundColor", GetType(System.Drawing.Color)), GetType(MeterReaderMvvmForms.BooleanToColorConverter), Nothing, New ActiveDevelop.EntitiesFormsLib.BindingProperty("AlternatingColorFlag", GetType(Boolean)))
        Me.MvvmDataGrid1.Columns.Add(MvvmDataGrid1_CityColumn)
        MvvmDataGrid1_ZipColumn.PropertyCellBindings.Add(New ActiveDevelop.EntitiesFormsLib.BindingSetting(ActiveDevelop.EntitiesFormsLib.MvvmBindingModes.TwoWay, ActiveDevelop.EntitiesFormsLib.UpdateSourceTriggerSettings.LostFocus), New ActiveDevelop.EntitiesFormsLib.BindingProperty("Content", GetType(ActiveDevelop.EntitiesFormsLib.MvvmDataGridColumn)), Nothing, Nothing, New ActiveDevelop.EntitiesFormsLib.BindingProperty("Zip", GetType(MRViewModelLibrary.BuildingViewModel)))
        MvvmDataGrid1_ZipColumn.PropertyCellBindings.Add(New ActiveDevelop.EntitiesFormsLib.BindingSetting(ActiveDevelop.EntitiesFormsLib.MvvmBindingModes.TwoWay, ActiveDevelop.EntitiesFormsLib.UpdateSourceTriggerSettings.PropertyChangedImmediately), New ActiveDevelop.EntitiesFormsLib.BindingProperty("BackgroundColor", GetType(System.Drawing.Color)), GetType(MeterReaderMvvmForms.BooleanToColorConverter), Nothing, New ActiveDevelop.EntitiesFormsLib.BindingProperty("AlternatingColorFlag", GetType(Boolean)))
        Me.MvvmDataGrid1.Columns.Add(MvvmDataGrid1_ZipColumn)
        MvvmDataGrid1_CountryColumn.PropertyCellBindings.Add(New ActiveDevelop.EntitiesFormsLib.BindingSetting(ActiveDevelop.EntitiesFormsLib.MvvmBindingModes.TwoWay, ActiveDevelop.EntitiesFormsLib.UpdateSourceTriggerSettings.LostFocus), New ActiveDevelop.EntitiesFormsLib.BindingProperty("Content", GetType(ActiveDevelop.EntitiesFormsLib.MvvmDataGridColumn)), Nothing, Nothing, New ActiveDevelop.EntitiesFormsLib.BindingProperty("Country", GetType(MRViewModelLibrary.BuildingViewModel)))
        MvvmDataGrid1_CountryColumn.PropertyCellBindings.Add(New ActiveDevelop.EntitiesFormsLib.BindingSetting(ActiveDevelop.EntitiesFormsLib.MvvmBindingModes.TwoWay, ActiveDevelop.EntitiesFormsLib.UpdateSourceTriggerSettings.PropertyChangedImmediately), New ActiveDevelop.EntitiesFormsLib.BindingProperty("BackgroundColor", GetType(System.Drawing.Color)), GetType(MeterReaderMvvmForms.BooleanToColorConverter), Nothing, New ActiveDevelop.EntitiesFormsLib.BindingProperty("AlternatingColorFlag", GetType(Boolean)))
        Me.MvvmDataGrid1.Columns.Add(MvvmDataGrid1_CountryColumn)
        Me.MvvmDataGrid1.CustomColumnTemplateType = Nothing
        Me.MvvmDataGrid1.DataSourceType = GetType(MRViewModelLibrary.BuildingViewModel)
        Me.MvvmDataGrid1.EnterAction = Nothing
        Me.MvvmManager1.SetEventBindings(Me.MvvmDataGrid1, Nothing)
        Me.MvvmDataGrid1.GridLinesVisibility = System.Windows.Controls.DataGridGridLinesVisibility.All
        Me.MvvmDataGrid1.ItemsSource = Nothing
        Me.MvvmDataGrid1.Location = New System.Drawing.Point(16, 15)
        Me.MvvmDataGrid1.Margin = New System.Windows.Forms.Padding(5)
        Me.MvvmDataGrid1.Name = "MvvmDataGrid1"
        Me.MvvmDataGrid1.SelectedItem = Nothing
        Me.MvvmDataGrid1.SelectionMode = System.Windows.Controls.DataGridSelectionMode.[Single]
        Me.MvvmDataGrid1.Size = New System.Drawing.Size(668, 254)
        Me.MvvmDataGrid1.TabIndex = 0
        '
        'MvvmDataGrid2
        '
        Me.MvvmDataGrid2.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.MvvmDataGrid2.AutoGenerateColumns = False
        Me.MvvmDataGrid2.CanUserAddRows = False
        Me.MvvmDataGrid2.CanUserDeleteRows = False
        MvvmDataGrid2_MeterIdColumn.PropertyCellBindings.Add(New ActiveDevelop.EntitiesFormsLib.BindingSetting(ActiveDevelop.EntitiesFormsLib.MvvmBindingModes.TwoWay, ActiveDevelop.EntitiesFormsLib.UpdateSourceTriggerSettings.LostFocus), New ActiveDevelop.EntitiesFormsLib.BindingProperty("Content", GetType(ActiveDevelop.EntitiesFormsLib.MvvmDataGridColumn)), Nothing, Nothing, New ActiveDevelop.EntitiesFormsLib.BindingProperty("MeterId", GetType(MRViewModelLibrary.MeterViewModel)))
        Me.MvvmDataGrid2.Columns.Add(MvvmDataGrid2_MeterIdColumn)
        MvvmDataGrid2_LocalDescriptionColumn.PropertyCellBindings.Add(New ActiveDevelop.EntitiesFormsLib.BindingSetting(ActiveDevelop.EntitiesFormsLib.MvvmBindingModes.TwoWay, ActiveDevelop.EntitiesFormsLib.UpdateSourceTriggerSettings.PropertyChangedImmediately), New ActiveDevelop.EntitiesFormsLib.BindingProperty("Content", GetType(Object)), Nothing, Nothing, New ActiveDevelop.EntitiesFormsLib.BindingProperty("LocationDescription", GetType(String)))
        Me.MvvmDataGrid2.Columns.Add(MvvmDataGrid2_LocalDescriptionColumn)
        Me.MvvmDataGrid2.CustomColumnTemplateType = Nothing
        Me.MvvmDataGrid2.DataSourceType = GetType(MRViewModelLibrary.MeterViewModel)
        Me.MvvmDataGrid2.EnterAction = Nothing
        Me.MvvmManager1.SetEventBindings(Me.MvvmDataGrid2, Nothing)
        Me.MvvmDataGrid2.GridLinesVisibility = System.Windows.Controls.DataGridGridLinesVisibility.All
        Me.MvvmDataGrid2.ItemsSource = Nothing
        Me.MvvmDataGrid2.Location = New System.Drawing.Point(16, 17)
        Me.MvvmDataGrid2.Margin = New System.Windows.Forms.Padding(5)
        Me.MvvmDataGrid2.Name = "MvvmDataGrid2"
        Me.MvvmDataGrid2.SelectedItem = Nothing
        Me.MvvmDataGrid2.SelectionMode = System.Windows.Controls.DataGridSelectionMode.[Single]
        Me.MvvmDataGrid2.Size = New System.Drawing.Size(917, 269)
        Me.MvvmDataGrid2.TabIndex = 0
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.MvvmManager1.SetEventBindings(Me.SplitContainer1, Nothing)
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 28)
        Me.SplitContainer1.Margin = New System.Windows.Forms.Padding(4)
        Me.SplitContainer1.Name = "SplitContainer1"
        Me.SplitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.GroupBox1)
        Me.SplitContainer1.Panel1.Controls.Add(Me.MvvmDataGrid1)
        Me.MvvmManager1.SetEventBindings(Me.SplitContainer1.Panel1, Nothing)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.MvvmDataGrid2)
        Me.MvvmManager1.SetEventBindings(Me.SplitContainer1.Panel2, Nothing)
        Me.SplitContainer1.Size = New System.Drawing.Size(949, 601)
        Me.SplitContainer1.SplitterDistance = 292
        Me.SplitContainer1.SplitterWidth = 5
        Me.SplitContainer1.TabIndex = 1
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.TextBox2)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.TextBox1)
        Me.MvvmManager1.SetEventBindings(Me.GroupBox1, Nothing)
        Me.GroupBox1.Location = New System.Drawing.Point(701, 17)
        Me.GroupBox1.Margin = New System.Windows.Forms.Padding(4)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Padding = New System.Windows.Forms.Padding(4)
        Me.GroupBox1.Size = New System.Drawing.Size(231, 250)
        Me.GroupBox1.TabIndex = 1
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Contact for Building"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.MvvmManager1.SetEventBindings(Me.Label2, Nothing)
        Me.Label2.Location = New System.Drawing.Point(8, 85)
        Me.Label2.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(74, 17)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Firstname:"
        '
        'TextBox2
        '
        Me.MvvmManager1.SetEventBindings(Me.TextBox2, Nothing)
        Me.TextBox2.Location = New System.Drawing.Point(8, 105)
        Me.TextBox2.Margin = New System.Windows.Forms.Padding(4)
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.Size = New System.Drawing.Size(213, 22)
        Me.TextBox2.TabIndex = 2
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.MvvmManager1.SetEventBindings(Me.Label1, Nothing)
        Me.Label1.Location = New System.Drawing.Point(8, 26)
        Me.Label1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(74, 17)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Lastname:"
        '
        'TextBox1
        '
        Me.MvvmManager1.SetEventBindings(Me.TextBox1, Nothing)
        Me.TextBox1.Location = New System.Drawing.Point(8, 46)
        Me.TextBox1.Margin = New System.Windows.Forms.Padding(4)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(213, 22)
        Me.TextBox1.TabIndex = 0
        '
        'frmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(949, 629)
        Me.Controls.Add(Me.SplitContainer1)
        Me.Controls.Add(Me.MenuStrip1)
        Me.MvvmManager1.SetEventBindings(Me, Nothing)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.Name = "frmMain"
        Me.Text = "Meter Reader Manager - MvvmForms Demo"
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        CType(Me.MvvmManager1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.MvvmDataGrid1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.MvvmDataGrid2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
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
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents Label2 As Label
    Friend WithEvents TextBox2 As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents TextBox1 As TextBox
End Class
