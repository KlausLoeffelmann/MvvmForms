Imports System.ComponentModel
Imports System.Drawing.Design
Imports System.Reflection
Imports System.Windows.Input
Imports wpf = System.Windows.Controls
Imports winforms = System.Windows.Forms
Imports System.Windows.Controls
Imports System.Windows
Imports System.Windows.Forms
Imports System.Drawing
Imports ActiveDevelop.EntitiesFormsLib

''' <summary>
''' DataGrid zur Anzeige und Bearbeiten von Daten welche aus eine ItemsSource stammen.
''' </summary>
''' <remarks>Wrapper fuer das WPF-DataGrid. Verwendet intern das WPF-DataGrid (mittels ElementHost) und somit auch die WPF-Columns und WPF-Bindungen.</remarks>
<Designer(GetType(MvvmDataGridDesigner)),
 ToolboxBitmap(GetType(winforms.DataGrid)),
 ToolboxItem(True)>
Public Class MvvmDataGrid
    Implements System.ComponentModel.ISupportInitialize

    ''' <summary>
    ''' Wahr wenn momentan die Spalten-Reihenfolge geladen wird
    ''' </summary>
    ''' <remarks></remarks>
    Dim _isColumnDisplayIndexUpdating As Boolean

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.Columns = New GridColumnCollection()
        Me.CanUserDeleteRows = False
        Me.CanUserAddRows = False

        Me.SelectionMode = wpf.DataGridSelectionMode.Single

        Me.WpfDataGridViewWrapper.InnerDataGridView.CanUserAddRows = Me.CanUserAddRows
        Me.WpfDataGridViewWrapper.InnerDataGridView.CanUserDeleteRows = Me.CanUserDeleteRows
        Me.WpfDataGridViewWrapper.InnerDataGridView.SelectionMode = Me.SelectionMode

        AddHandler Me.WpfDataGridViewWrapper.InnerDataGridView.SelectionChanged, AddressOf InnerDataGridView_SelectionChanged
        AddHandler Me.WpfDataGridViewWrapper.InnerDataGridView.MouseDoubleClick, AddressOf InnerDataGridView_MouseDoubleClick
        AddHandler Me.WpfDataGridViewWrapper.InnerDataGridView.PreviewKeyDown, AddressOf InnerDataGridView_PreviewKeyDown
        AddHandler Me.WpfDataGridViewWrapper.InnerDataGridView.ColumnDisplayIndexChanged, AddressOf InnerDataGridView_ColumnDisplayIndexChanged
        AddHandler Me.WpfDataGridViewWrapper.InnerDataGridView.Sorted, AddressOf InnerDataGridView_Sorted
        AddHandler Me.WpfDataGridViewWrapper.InnerDataGridView.LayoutUpdated, AddressOf InnerDataGridView_LayoutUpdated
        AddHandler Me.WpfDataGridViewWrapper.InnerDataGridView.KeyDown, AddressOf InnerDataGridView_KeyDown
        AddHandler Me.WpfDataGridViewWrapper.InnerDataGridView.ItemsDeleted, AddressOf InnerDataGridView_ItemsDeleted
        AddHandler Me.WpfDataGridViewWrapper.InnerDataGridView.ItemsDeleting, AddressOf InnerDataGridView_ItemsDeleting
    End Sub

    ''' <summary>
    ''' Wird von interner Loeschfunktion vom DataGrid geworfen
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Public Event ItemsDeleting(ByVal sender As Object, ByVal e As ItemsDeletingEventArgs)

    Private Sub InnerDataGridView_ItemsDeleting(sender As Object, e As ItemsDeletingEventArgs)
        OnItemsDelete(e)
    End Sub

    Private Sub OnItemsDelete(e As ItemsDeletingEventArgs)
        RaiseEvent ItemsDeleting(Me, e)
    End Sub

    Private _mySettings As MvvmDataGridSetting

    Private Shared _settings As Global.ActiveDevelop.EntitiesFormsLib.MvvmDataGridSettings
    ''' <summary>
    ''' Hier kann initial bei Programmstart die Settingsinstanz für das Speichern der Einstellung eines MvvmDataGrids hinterlegt werden
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Property Settings As Global.ActiveDevelop.EntitiesFormsLib.MvvmDataGridSettings
        Get
            Return _settings
        End Get
        Set(ByVal value As Global.ActiveDevelop.EntitiesFormsLib.MvvmDataGridSettings)
            _settings = value
        End Set
    End Property

    Private _headersVisibility As DataGridHeadersVisibility = DataGridHeadersVisibility.All
    ''' <summary>
    ''' Gets or sets the value that specifies the visibility of the row and column headers.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>Durch den MvvmManager bindbare View-Property</remarks>
    Public Property HeadersVisibility As DataGridHeadersVisibility
        Get
            Return _headersVisibility
        End Get
        Set(ByVal value As DataGridHeadersVisibility)
            If Not Object.Equals(_headersVisibility, value) Then
                _headersVisibility = value

                Me.WpfDataGridViewWrapper.InnerDataGridView.HeadersVisibility = value

                OnHeadersVisibilityChanged(EventArgs.Empty)
            End If
        End Set
    End Property

    Public Event HeadersVisibilityChanged As EventHandler

    Protected Overridable Sub OnHeadersVisibilityChanged(e As EventArgs)
        RaiseEvent HeadersVisibilityChanged(Me, e)
    End Sub


    Private _canUserAddRows As Boolean = False
    ''' <summary>
    ''' Flag welches bestimmt ob neue Reihen angelegt werden dürfen
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>Durch den MvvmManager bindbare View-Property</remarks>
    Public Property CanUserAddRows As Boolean
        Get
            Return _canUserAddRows
        End Get
        Set(ByVal value As Boolean)
            If Not Object.Equals(_canUserAddRows, value) Then
                _canUserAddRows = value

                Me.WpfDataGridViewWrapper.InnerDataGridView.CanUserAddRows = value

                OnCanUserAddRowsChanged(EventArgs.Empty)
            End If
        End Set
    End Property

    ''' <summary>
    ''' Anzeigeeinstellung der Linien im DataGrid
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>Durch den MvvmManager bindbare View-Property</remarks>
    Public Property GridLinesVisibility As DataGridGridLinesVisibility
        Get
            Return Me.WpfDataGridViewWrapper.InnerDataGridView.GridLinesVisibility
        End Get
        Set(ByVal value As DataGridGridLinesVisibility)
            If Not Object.Equals(Me.WpfDataGridViewWrapper.InnerDataGridView.GridLinesVisibility, value) Then
                Me.WpfDataGridViewWrapper.InnerDataGridView.GridLinesVisibility = value
                OnGridLinesVisibilityChanged(EventArgs.Empty)
            End If
        End Set
    End Property

    Public Event GridLinesVisibilityChanged As EventHandler

    Protected Overridable Sub OnGridLinesVisibilityChanged(e As EventArgs)
        RaiseEvent GridLinesVisibilityChanged(Me, e)
    End Sub


    Private _canUserDeleteRows As Boolean
    ''' <summary>
    ''' Bestimmt ob der Benutzer auch Zeilen löschen kann
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>Durch den MvvmManager bindbare View-Property</remarks>
    Public Property CanUserDeleteRows As Boolean
        Get
            Return _canUserDeleteRows
        End Get
        Set(ByVal value As Boolean)
            If Not Object.Equals(_canUserDeleteRows, value) Then
                _canUserDeleteRows = value

                Me.WpfDataGridViewWrapper.InnerDataGridView.CanUserDeleteRows = value

                OnCanUserDeleteRowsChanged(EventArgs.Empty)
            End If
        End Set
    End Property

    Public Event CanUserDeleteRowsChanged As EventHandler

    Protected Overridable Sub OnCanUserDeleteRowsChanged(e As EventArgs)
        RaiseEvent CanUserDeleteRowsChanged(Me, e)
    End Sub


    Private _selectionMode As wpf.DataGridSelectionMode
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>Durch den MvvmManager bindbare View-Property</remarks>
    Public Property SelectionMode As wpf.DataGridSelectionMode
        Get
            Return _selectionMode
        End Get
        Set(ByVal value As wpf.DataGridSelectionMode)
            If Not Object.Equals(_selectionMode, value) Then
                _selectionMode = value

                Me.WpfDataGridViewWrapper.InnerDataGridView.SelectionMode = value

                OnSelectionModeChanged(EventArgs.Empty)
            End If
        End Set
    End Property

    Public Event SelectionModeChanged As EventHandler

    Protected Overridable Sub OnSelectionModeChanged(e As EventArgs)
        RaiseEvent SelectionModeChanged(Me, e)
    End Sub


    Private _enterAction As Action
    ''' <summary>
    ''' Action welche aufgerufen´wir beim Enter drücken
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>Durch den MvvmManager bindbare View-Property</remarks>
    Public Property EnterAction As Action
        Get
            Return _enterAction
        End Get
        Set(ByVal value As Action)
            If Not Object.Equals(_enterAction, value) Then
                _enterAction = value
                OnEnterActionChanged(EventArgs.Empty)
            End If
        End Set
    End Property

    Public Event EnterActionChanged As EventHandler

    Protected Overridable Sub OnEnterActionChanged(e As EventArgs)
        RaiseEvent EnterActionChanged(Me, e)
    End Sub

    Public Event CanUserAddRowsChanged As EventHandler

    Protected Overridable Sub OnCanUserAddRowsChanged(e As EventArgs)
        RaiseEvent CanUserAddRowsChanged(Me, e)
    End Sub

    ''' <summary>
    ''' Datenquelle
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>Durch den MvvmManager bindbare View-Property</remarks>
    Public Property ItemsSource As IEnumerable
        Get
            Return WpfDataGridViewWrapper.InnerDataGridView.ItemsSource
        End Get
        Set(ByVal value As IEnumerable)
            If Not Object.Equals(WpfDataGridViewWrapper.InnerDataGridView.ItemsSource, value) Then
                WpfDataGridViewWrapper.InnerDataGridView.ItemsSource = value
                OnItemsSourceChanged(EventArgs.Empty)
            End If
        End Set
    End Property

    Public Event ItemsSourceChanged As EventHandler

    Protected Overridable Sub OnItemsSourceChanged(e As EventArgs)
        RaiseEvent ItemsSourceChanged(Me, e)
    End Sub

    ''' <summary>
    ''' True, wenn die Columns automatisch ergaenzt werden sollen
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>Durch den MvvmManager bindbare View-Property</remarks>
    Public Property AutoGenerateColumns As Boolean
        Get
            Return WpfDataGridViewWrapper.InnerDataGridView.AutoGenerateColumns
        End Get
        Set(ByVal value As Boolean)
            If Not Object.Equals(WpfDataGridViewWrapper.InnerDataGridView.AutoGenerateColumns, value) Then
                WpfDataGridViewWrapper.InnerDataGridView.AutoGenerateColumns = value
                OnAutoGenerateColumnsChanged(EventArgs.Empty)
            End If
        End Set
    End Property

    ''' <summary>
    ''' Wird geworfen sobald eine Zeile via Doppel-Klick angewählt wurde
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Public Event ItemDoubleClick(ByVal sender As Object, ByVal e As ItemDoubleClickEventArgs)

    ''' <summary>
    ''' Wird geworfen wenn eine Taske auf der Tastatur gedrueckt wurde
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Public Event InnerKeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs)

    Protected Overridable Sub OnDataGridKeyDown(item As Object, e As System.Windows.Input.KeyEventArgs)
        RaiseEvent InnerKeyDown(item, e)
    End Sub

    Protected Overridable Sub OnItemDoubleClick(item As Object)
        RaiseEvent ItemDoubleClick(Me, New ItemDoubleClickEventArgs(item))
    End Sub


    Public Event AutoGenerateColumnsChanged As EventHandler

    Protected Overridable Sub OnAutoGenerateColumnsChanged(e As EventArgs)
        RaiseEvent AutoGenerateColumnsChanged(Me, e)
    End Sub

    Private _dataSourceType As Type

    ''' <summary>
    ''' Hier MUSS (wenn ein Cellbinding verwendet werden soll) der Datentyp ausgewaehlt werden, welcher innerhalb der ItemsSource verwendet werden soll
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>Gleiche Vorraussetzung am Typ wie DataContextTyp am MvvmManager</remarks>
    <Editor(GetType(MvvmDataGridDataSourceTypeUIEditor), GetType(UITypeEditor)), Category("MVVM"),
    Description("Bestimmt oder ermittelt, welchen Typ ViewModel das DataGridView später zur Laufzeit verarbeiten soll.")>
    Property DataSourceType As Type
        Get
            Return _dataSourceType
        End Get
        Set(value As Type)
            If _customColumnTemplateType IsNot value Then
                _dataSourceType = value

                For Each column In Me.Columns
                    column.DataSourceType = Me.DataSourceType
                Next
            End If
        End Set
    End Property

    Private _customColumnTemplateType As Type

    ''' <summary>
    ''' Hier wird der Datentyp definiert, welche bei fuer die Erweiterung (durch den Anwendungsentwickler) des Templates verwendet werden soll
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Browsable(False)>
    <Editor(GetType(MvvmDataGridCustomColumnTemplateTypeUIEditor), GetType(UITypeEditor)), Category("MVVM"),
    Description("Bestimmt oder ermittelt, welcher Typ fuer das DataGridView fuer die Erweiterung einer Column verwendet werden soll.")>
    Property CustomColumnTemplateType As Type
        Get
            Return _customColumnTemplateType
        End Get
        Set(value As Type)
            If _customColumnTemplateType IsNot value Then
                _customColumnTemplateType = value
            End If
        End Set
    End Property

    ''' <summary>
    ''' Hier wird das selektierte Item innerhalb des DataGrid gespeichert
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>Durch den MvvmManager bindbare View-Property</remarks>
    Public Property SelectedItem As Object
        Get
            Return WpfDataGridViewWrapper.InnerDataGridView.SelectedItem
        End Get
        Set(ByVal value As Object)
            If Not Object.Equals(WpfDataGridViewWrapper.InnerDataGridView.SelectedItem, value) Then
                WpfDataGridViewWrapper.InnerDataGridView.SelectedItem = value
                OnSelectedItemChanged(EventArgs.Empty)
            End If
        End Set
    End Property

    ''' <summary>
    ''' Wird geworfen wenn Items mittels interner Loeschfunktion vom DataGrid geloescht wurden
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Public Event ItemsDeleted(ByVal sender As Object, ByVal e As ItemsDeletedEventArgs)

    Protected Overridable Sub OnItemsDeleted(e As ItemsDeletedEventArgs)
        RaiseEvent ItemsDeleted(Me, e)
    End Sub

    Public Event SelectedItemChanged As EventHandler

    Protected Overridable Sub OnSelectedItemChanged(e As EventArgs)
        RaiseEvent SelectedItemChanged(Me, e)
    End Sub

    Private _columns As GridColumnCollection

    ''' <summary>
    ''' Hier werden die Columns des DataGrid gespeichert
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Content)>
    <Browsable(True)>
    <EditorAttribute(GetType(ColumnsEditor), GetType(UITypeEditor))>
    Public Property Columns As GridColumnCollection
        Get
            Return _columns
        End Get
        Private Set(ByVal value As GridColumnCollection)
            If _columns IsNot Nothing Then
                RemoveHandler _columns.CollectionChanged, AddressOf Columns_CollectionChanged
            End If

            _columns = value

            Me.WpfDataGridViewWrapper.InnerDataGridView.Columns.Clear()

            If value IsNot Nothing Then
                RemoveHandler value.CollectionChanged, AddressOf Columns_CollectionChanged
                AddHandler value.CollectionChanged, AddressOf Columns_CollectionChanged
            End If

        End Set
    End Property

    Private Function ShouldSerializeColumns() As Boolean
        Return Me.Columns IsNot Nothing
    End Function

    ''' <summary>
    ''' Sobald Columns durch den Designer oder zur Laufzeit hinzugefuegt werden, muessen diese in WPF-COlumns uebertragen werden und dem internen WPF-DataGrid hinzugefuegt werden
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Columns_CollectionChanged(sender As Object, e As Specialized.NotifyCollectionChangedEventArgs)

        If e.NewItems IsNot Nothing Then
            For Each newColumn As MvvmDataGridColumn In e.NewItems
                'Neue WPF-Column anlegen
                AddNewColumn(newColumn)
            Next
        ElseIf e.OldItems IsNot Nothing Then
            For Each oldItem As MvvmDataGridColumn In e.OldItems
                'Bestehende Column loeschen
                For Each column In Me.WpfDataGridViewWrapper.InnerDataGridView.Columns.ToList
                    Dim gridColumn = column.GetValue(MvvmDataGrid.GridColumnProperty)

                    If oldItem Is gridColumn Then
                        Me.WpfDataGridViewWrapper.InnerDataGridView.Columns.Remove(column)

                        If Not MyBase.DesignMode Then
                            _mySettings.ColumnDefinitions.Remove(_mySettings.ColumnDefinitions.Where(Function(col) col.Name = oldItem.Name).First)
                        End If

                        Exit For
                    End If

                Next
            Next
        End If

    End Sub

    ''' <summary>
    ''' Schluessel zur GridColumn-DependencyProperty
    ''' </summary>
    ''' <remarks></remarks>
    Friend Shared ReadOnly GridColumnProperty As System.Windows.DependencyProperty =
                           System.Windows.DependencyProperty.Register("GridColumn",
                           GetType(MvvmDataGridColumn), GetType(MvvmDataGrid))

    ''' <summary>
    ''' Fuegt dem WPF-DataGrid eine neue Spalte hinzu
    ''' </summary>
    ''' <param name="newColumn"></param>
    ''' <remarks></remarks>
    Private Sub AddNewColumn(newColumn As MvvmDataGridColumn)
        'Nun nicht mehr Default Spalten anlegen:
        Me.WpfDataGridViewWrapper.InnerDataGridView.AutoGenerateColumns = False

        'Alle Columns loeschen, die nicht mehr vorhanden sind
        SynchronizeColumnCollections()

        'Erst gucken ob es nicht schon angelegt wurde, dann die loeschen und neu anlegen
        For Each wpfColumn In Me.WpfDataGridViewWrapper.InnerDataGridView.Columns.ToList
            Dim c = wpfColumn.GetValue(MvvmDataGrid.GridColumnProperty)

            If c Is newColumn Then
                Me.WpfDataGridViewWrapper.InnerDataGridView.Columns.Remove(wpfColumn)
            End If

        Next


        'Den Datentyp setzen:
        newColumn.DataSourceType = Me.DataSourceType

        'Nur anlegen, wenn ein typ auch hinterlegt wurde...
        If Me.CustomColumnTemplateType IsNot Nothing AndAlso newColumn.ColumnTemplateExtender Is Nothing Then
            newColumn.ColumnTemplateExtender = DirectCast(Activator.CreateInstance(Me.CustomColumnTemplateType), IMvvmColumnTemplateExtender)
        End If


        'Column initialisieren
        Dim newWpfColumn = newColumn.InitializeColumn()

        'Tag zum identifizieren der Spalte setzen
        newWpfColumn.SetValue(MvvmDataGrid.GridColumnProperty, newColumn)

        'Und dem internen DataGrid hinzufuegen
        Me.WpfDataGridViewWrapper.InnerDataGridView.Columns.Add(newWpfColumn)

        If Not MyBase.DesignMode AndAlso _mySettings IsNot Nothing Then
            _mySettings.ColumnDefinitions.Add(New ColumnDefinition() With {.DisplayIndex = newWpfColumn.DisplayIndex, .Name = newColumn.Name, .Width = newWpfColumn.Width.ToString()})
        End If
    End Sub

    ''' <summary>
    ''' Loescht alle Columns, die nicht mehr vorhanden sind
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SynchronizeColumnCollections()
        'Erst gucken ob es nicht schon angelegt wurde, dann die loeschen und neu anlegen
        For Each wpfColumn In Me.WpfDataGridViewWrapper.InnerDataGridView.Columns.ToList
            Dim c As MvvmDataGridColumn = DirectCast(wpfColumn.GetValue(MvvmDataGrid.GridColumnProperty), MvvmDataGridColumn)

            If Not Me.Columns.Contains(c) Then
                Me.WpfDataGridViewWrapper.InnerDataGridView.Columns.Remove(wpfColumn)
            End If
        Next
    End Sub

    ''' <summary>
    ''' Wenn dem DataGrid schon ein DataSourceType bekannt gegeben wurde, können hiermit Spalten automatisch angelegt werden
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub CreateColumnsDataSourceType()
        'Hier müssen die Spalten angelegt werden
        'Was ist wenn schon Spalten vorhanden sind? ausblenden?

        If Me.DataSourceType Is Nothing Then Throw New InvalidOperationException("Es wurde noch kein DataSourceType definiert! Es können die Spalten erst erzeugt werden, wenn ein Typ angegeben wurde.")

        If Debugger.IsAttached Then
            Debugger.Break()
        End If

        Dim viewModelProps = Me.DataSourceType.
            GetProperties(BindingFlags.Public Or BindingFlags.Instance).ToList

        'Für jede Property soll nun eine Spalte anlegen
        For Each prop In viewModelProps

            'Wenn es eine System- (Infrastruktur-) Eigenschaft ist, dann überlesen.
            If prop.GetCustomAttribute(Of MvvmSystemElementAttribute)() IsNot Nothing Then
                Continue For
            End If

            Dim gridColumn = New MvvmDataGridColumn()

            gridColumn.Name = prop.Name & "Column"
            gridColumn.Header = prop.Name

            'Bindung erzeugen:
            Dim binding = New PropertyBindingItem()
            Dim bindingSetting = New BindingSetting(MvvmBindingModes.TwoWay, UpdateSourceTriggerSettings.PropertyChangedImmediately)

            binding.ControlProperty = New BindingProperty("Content", GetType(MvvmDataGridColumn))
            binding.ViewModelProperty = New BindingProperty(prop.Name, prop.DeclaringType)

            'Spaltentyp ermitteln:
            If prop.PropertyType Is GetType(Boolean) Then
                gridColumn.ColumnType = ColumnType.CheckBox

            ElseIf GetType(IEnumerable).IsAssignableFrom(prop.PropertyType) AndAlso prop.PropertyType IsNot GetType(String) Then
                gridColumn.ColumnType = ColumnType.ComboBox
                binding.ControlProperty = New BindingProperty("ComboBoxItemsSource", GetType(MvvmDataGridColumn))
            Else
                gridColumn.ColumnType = ColumnType.TextAndNumbers
                bindingSetting.UpdateSourceTrigger = UpdateSourceTriggerSettings.LostFocus
            End If

            'Bei ReadOnly-Props oder Nicht-Primitiven, nicht-nullable-Properties soll 
            'standardmäßig nur OneWay gebunden werden. Hier müsste der Entwickler einen
            'Konverter implementieren, um TwoWay zu binden.
            If prop.CanWrite Or prop.PropertyType.IsPrimitive Or
                    (prop.PropertyType.IsGenericType AndAlso
                    (prop.PropertyType.GetGenericTypeDefinition Is GetType(Nullable(Of )))) Then
                bindingSetting.BindingMode = MvvmBindingModes.TwoWay
            Else
                bindingSetting.BindingMode = MvvmBindingModes.OneWay
            End If

            binding.BindingSetting = bindingSetting

            gridColumn.PropertyCellBindings.Add(binding)

            Me.Columns.Add(gridColumn)
        Next

    End Sub

    ''' <summary>
    ''' Wenn im WPF-Grid die Auswahl geaendert wurde, dann soll zum WinForms-Grid delegiert werden
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub InnerDataGridView_SelectionChanged(sender As Object, e As wpf.SelectionChangedEventArgs)
        OnSelectedItemChanged(e)
    End Sub

    ''' <summary>
    ''' Delegiert das MouseDoubleClick des inneren Wpf-DataGrids an die WinForms-Umgebung
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub InnerDataGridView_MouseDoubleClick(sender As Object, e As System.Windows.Input.MouseButtonEventArgs)
        MyBase.OnMouseDoubleClick(Nothing)
    End Sub

    ''' <summary>
    ''' Setzt den Fokus auf das interne DataGrid
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub FocusGrid()
        Dim grid = Me.WpfDataGridViewWrapper.InnerDataGridView
        grid.Focus()
        Dim cellInfo = New DataGridCellInfo(grid.SelectedItem, grid.Columns(0))
        grid.CurrentCell = cellInfo
        grid.ScrollIntoView(grid.SelectedItem)
        grid.BeginEdit()
    End Sub

    Private Sub InnerDataGridView_PreviewKeyDown(sender As Object, e As System.Windows.Input.KeyEventArgs)
        If e.Key.Equals(Key.Enter) OrElse e.Key.Equals(Key.Return) Then
            If Me.EnterAction IsNot Nothing Then
                e.Handled = True
                Me.EnterAction.Invoke()
            End If
        End If

    End Sub

    ''' <summary>
    ''' Wenn der DisplayIndex vom Benutzer verschoben wurde, dann auch in Settings für nächsten Aufruf speichern
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub InnerDataGridView_ColumnDisplayIndexChanged(sender As Object, e As DataGridColumnEventArgs)
        If Not _isColumnDisplayIndexUpdating AndAlso _mySettings IsNot Nothing AndAlso _mySettings.ColumnDefinitions IsNot Nothing Then
            Dim column = Me.Columns.Where(Function(col) col.WpfColumn Is e.Column).Single

            _mySettings.ColumnDefinitions.Where(Function(c) c.Name = column.Name).Single().DisplayIndex = e.Column.DisplayIndex
        End If

    End Sub

    ''' <summary>
    ''' Wenn der Benutzer die Sortierung verändert, dann auch in Settings für nächsten Aufruf speichern
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub InnerDataGridView_Sorted(sender As Object, e As DataGridSortingEventArgs)
        UpdateAllColumnDefs()
    End Sub

    ''' <summary>
    ''' Wenn sich das LAyout verändert hat, alle Spaltenbreiten abspeichern
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub InnerDataGridView_LayoutUpdated(sender As Object, e As EventArgs)
        UpdateAllColumnDefs()
    End Sub

    ''' <summary>
    ''' Aktualisiert anhand der WPF-Spalten die internen Danträger für das Abspeichern der Einstellungen
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub UpdateAllColumnDefs()
        If _mySettings IsNot Nothing AndAlso _mySettings.ColumnDefinitions IsNot Nothing Then
            For Each column In Me.Columns
                Dim columnDef = _mySettings.ColumnDefinitions.Where(Function(c) c.Name = column.Name).Single()

                With column.WpfColumn
                    columnDef.SortDirection = .SortDirection
                    columnDef.SortMemberPath = .SortMemberPath
                    columnDef.Width = .Width.ToString()
                End With

            Next
        End If
    End Sub

    ''' <summary>
    ''' Doppel-Klick auf eine Zeile, weiter delegieren mit ItemClickedEvent...
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub InnerDataGridRow_MouseDoubleClick(sender As Object, e As RoutedEventArgs)
        Dim row = DirectCast(sender, DataGridRow)

        OnItemDoubleClick(row.Item)
    End Sub

    ''' <summary>
    ''' Ruft das InnerKeyDown-Event auf
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub InnerDataGridView_KeyDown(sender As Object, e As System.Windows.Input.KeyEventArgs)
        OnDataGridKeyDown(sender, e)
    End Sub

    ''' <summary>
    ''' Ruft das ItemsDeleted-Event auf
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub InnerDataGridView_ItemsDeleted(sender As Object, e As ItemsDeletedEventArgs)
        OnItemsDeleted(e)
    End Sub

    Public Sub BeginInit() Implements ISupportInitialize.BeginInit

    End Sub

    Public Sub EndInit() Implements ISupportInitialize.EndInit
        'If Not _isInitialized Then Initialize()
    End Sub

    Private _myParent As Forms.Control

    Private _isInitialized As Boolean = False

    Protected Overrides Sub OnLoad(e As System.EventArgs)
        MyBase.OnLoad(e)

        If Not _isInitialized Then Initialize()
    End Sub


    ''' <summary>
    ''' Initialisiert das MvvmDataGrid und konvertiert die Columns
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub Initialize()
        If Not MyBase.DesignMode AndAlso MyBase.Parent IsNot Nothing AndAlso _myParent IsNot MyBase.Parent Then
            _myParent = MyBase.Parent

            'Hier wird für das MouseDoubleClickEvent ein Style erstellt um mitzubekommen wenn eine Column ausgewählt wurdeDim style = New Style(GetType(DataGridRow))
            Dim style = New Style(GetType(DataGridRow))
            style.Setters.Add(New EventSetter(DataGridRow.MouseDoubleClickEvent, New MouseButtonEventHandler(AddressOf InnerDataGridRow_MouseDoubleClick)))
            Me.WpfDataGridViewWrapper.InnerDataGridView.RowStyle = style

            If Not MyBase.DesignMode AndAlso Settings IsNot Nothing Then
                Dim settingsKey = _myParent.Name & "." & Me.Name

                'Schauen ob es schon Settings gibt (für die aktuelle Instanz), wenn nicht dann anlegen
                If Not Settings.ContainsKey(settingsKey) Then
                    Settings.Add(settingsKey, New MvvmDataGridSetting())
                End If

                'Meine eigenen Instanz-Settings laden:
                _mySettings = Settings(settingsKey)

                'Nur wenn Einstelungen gesetzt worden sind:
                If _mySettings IsNot Nothing Then
                    If _mySettings.ColumnDefinitions.Count > 0 Then
                        Dim matchedColumns As New List(Of String)()
                        Dim converter = New DataGridLengthConverter()

                        Try
                            _isColumnDisplayIndexUpdating = True

                            For Each column In _mySettings.ColumnDefinitions.OrderBy(Function(c) c.DisplayIndex)
                                'Hier alle gleichen anpassen
                                For Each innerColumn In Me.Columns

                                    If column.Name = innerColumn.Name Then
                                        'Die gleiche, abgleichen:
                                        With innerColumn.WpfColumn
                                            .DisplayIndex = column.DisplayIndex
                                            .SortDirection = column.SortDirection
                                            .SortMemberPath = column.SortMemberPath
                                            If Not String.IsNullOrEmpty(column.Width) Then .Width = DirectCast(converter.ConvertFromString(column.Width), DataGridLength)

                                        End With

                                        matchedColumns.Add(column.Name)
                                    End If

                                Next
                            Next
                        Finally
                            _isColumnDisplayIndexUpdating = False
                        End Try


                        For Each notFoundColumnName In Me.Columns.Select(Function(c) c.Name).Except(matchedColumns).ToList()
                            Dim notFoundColumn = Me.Columns.Where(Function(wc) wc.Name = notFoundColumnName).Select(Function(iwc) iwc.WpfColumn).Single()

                            'Spalte ist nun neu drin, also auch wieder in Settings speichern:
                            _mySettings.ColumnDefinitions.Add(New ColumnDefinition() With {.DisplayIndex = notFoundColumn.DisplayIndex, .Name = notFoundColumnName, .Width = notFoundColumn.Width.ToString()})
                        Next

                        For Each notFoundColumnName In _mySettings.ColumnDefinitions.Select(Function(c) c.Name).Except(matchedColumns).ToList()
                            Dim notFoundColumn = _mySettings.ColumnDefinitions.Where(Function(cd) cd.Name = notFoundColumnName).Single()

                            'Spalte ist nicht mehr drin, also auch wieder löschen aus Settings:
                            _mySettings.ColumnDefinitions.Remove(notFoundColumn)
                        Next
                    Else
                        'und alle Spalten einmal einfügen
                        For Each column In Me.Columns
                            _mySettings.ColumnDefinitions.Add(New ColumnDefinition() With {.DisplayIndex = column.WpfColumn.DisplayIndex, .Name = column.Name, .Width = column.WpfColumn.Width.ToString()})
                        Next
                    End If
                End If
            End If

            _isInitialized = True
        ElseIf MyBase.DesignMode Then
            _isInitialized = True
        End If
    End Sub


End Class

