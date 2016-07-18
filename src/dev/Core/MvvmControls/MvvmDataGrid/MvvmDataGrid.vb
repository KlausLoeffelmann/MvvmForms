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
Imports System.Windows.Controls.Primitives
Imports System.Collections.Specialized
Imports System.Windows.Data

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
    Private _isColumnDisplayIndexUpdating As Boolean
    Private _collectionView As ICollectionView ' Bei eingeschaltener Filterung wird die CollectionView verwendet

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
        AddHandler Me.WpfDataGridViewWrapper.InnerDataGridView.Sorting, AddressOf InnerDataGridView_Sorting
    End Sub

    ''' <summary>
    ''' Wird aufgerufen wenn eine Spalte angefangen werden soll zu sortieren
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Public Event Sorting(ByVal sender As Object, ByVal e As DataGridSortingEventArgs)

    Private Sub InnerDataGridView_Sorting(sender As Object, e As DataGridSortingEventArgs)
        OnSorting(e)
    End Sub

    Private Sub OnSorting(e As DataGridSortingEventArgs)
        RaiseEvent Sorting(Me, e)
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
    ''' Gets or sets a value that indicates whether the user can sort columns by clicking the column header.
    ''' </summary>
    ''' <returns></returns>
    Public Property CanUserSortColumns As Boolean
        Get
            Return Me.WpfDataGridViewWrapper.InnerDataGridView.CanUserSortColumns
        End Get
        Set(ByVal value As Boolean)
            Me.WpfDataGridViewWrapper.InnerDataGridView.CanUserSortColumns = value
        End Set
    End Property

    ''' <summary>
    ''' Gets the items in the System.Windows.Controls.Primitives.MultiSelector that are selected.
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property SelectedItems As IList
        Get
            Return WpfDataGridViewWrapper.InnerDataGridView.SelectedItems
        End Get
    End Property

    Private _contextMenu As wpf.ContextMenu
    ''' <summary>
    ''' Gets or sets the context menu element that should appear whenever the context menu is requested through user interface (UI) from within this element.(Inherited from FrameworkElement.)
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>Durch den MvvmManager bindbare View-Property</remarks>
    Public Shadows Property ContextMenu As wpf.ContextMenu
        Get
            Return _contextMenu
        End Get
        Set(ByVal value As wpf.ContextMenu)
            If Not Object.Equals(_contextMenu, value) Then
                _contextMenu = value

                Me.WpfDataGridViewWrapper.InnerDataGridView.ContextMenu = value

                Me.OnContextMenuChanged(EventArgs.Empty)
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

    Public Shadows Event ContextMenuChanged As EventHandler

    Protected Overridable Shadows Sub OnContextMenuChanged(e As EventArgs)
        RaiseEvent ContextMenuChanged(Me, e)
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
                If IsFilteringEnabled Then
                    _collectionView = CollectionViewSource.GetDefaultView(value)

                    WpfDataGridViewWrapper.InnerDataGridView.ItemsSource = _collectionView
                Else
                    WpfDataGridViewWrapper.InnerDataGridView.ItemsSource = value
                End If

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

    <Description("Gets or sets a value that indicates whether the user can edit values in the DataGrid.")>
    Property IsReadOnly As Boolean
        Get
            Return WpfDataGridViewWrapper.InnerDataGridView.IsReadOnly
        End Get
        Set(value As Boolean)
            If WpfDataGridViewWrapper.InnerDataGridView.IsReadOnly <> value Then
                WpfDataGridViewWrapper.InnerDataGridView.IsReadOnly = value
            End If
        End Set
    End Property

    Private _isFilteringEnabled As Boolean = False

    ''' <summary>
    ''' Wenn gesetzt wird das Filtern vom DataGrid bei String-Eigenschaften aktiviert
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>Gleiche Vorraussetzung am Typ wie DataContextTyp am MvvmManager</remarks>
    <Category("Filter"),
    Description("Wert welcher angibt, ob das DataGrid das Filtern von Daten ermöglicht.")>
    Property IsFilteringEnabled As Boolean
        Get
            Return _isFilteringEnabled
        End Get
        Set(value As Boolean)
            If _isFilteringEnabled <> value Then
                _isFilteringEnabled = value
            End If
        End Set
    End Property

    Private _filterCaseSensitive As Boolean = True

    ''' <summary>
    ''' Filterung unterscheidet Groß- und Kleinschreibung
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>Gleiche Vorraussetzung am Typ wie DataContextTyp am MvvmManager</remarks>
    <Category("Filter"),
    Description("Gibt an ob Klein- oder Großschreibung in der Filterung unterschieden wird.")>
    Property FilterCaseSensitive As Boolean
        Get
            Return _filterCaseSensitive
        End Get
        Set(value As Boolean)
            If _filterCaseSensitive <> value Then
                _filterCaseSensitive = value
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
    ''' Gibt das erste Element in der aktuellen Auswahl bzw. NULL zurück,
    ''' wenn die Auswahl leer ist, oder legt das Element fest
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
    ''' Ruft den Index des ersten Elements in der aktuellen Auswahl ab bzw. legt diesen fest,
    ''' oder gibt eine negative Eins ("-1") zurück, falls die Auswahl leer ist
    ''' </summary>
    ''' <returns></returns>
    Public Property SelectedIndex As Integer
        Get
            Return WpfDataGridViewWrapper.InnerDataGridView.SelectedIndex
        End Get
        Set(ByVal value As Integer)
            If Not Object.Equals(WpfDataGridViewWrapper.InnerDataGridView.SelectedIndex, value) Then
                WpfDataGridViewWrapper.InnerDataGridView.SelectedIndex = value
                OnSelectedIndexChanged(EventArgs.Empty)
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

    Public Event SelectedIndexChanged As EventHandler

    Protected Overridable Sub OnSelectedIndexChanged(e As EventArgs)
        RaiseEvent SelectedIndexChanged(Me, e)
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

                        If Not MyBase.DesignMode AndAlso _mySettings IsNot Nothing Then
                            _mySettings.ColumnDefinitions.Remove(_mySettings.ColumnDefinitions.Where(Function(col) col.Name = oldItem.Name).First)
                        End If

                        Exit For
                    End If

                Next
            Next
        ElseIf e.Action = Specialized.NotifyCollectionChangedAction.Reset Then
            'Clear all
            WpfDataGridViewWrapper.InnerDataGridView.Columns.Clear()

            If _mySettings IsNot Nothing Then
                _mySettings.ColumnDefinitions.Clear()
            End If
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

        newColumn.IsFilteringEnabled = IsFilteringEnabled

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

        If IsFilteringEnabled Then
            If newColumn.DataSourceType IsNot Nothing AndAlso newColumn.PropertyCellBindings IsNot Nothing Then
                Dim binding = newColumn.PropertyCellBindings.Where(Function(p) p.ControlProperty.PropertyName = "Content").SingleOrDefault()

                If binding IsNot Nothing AndAlso binding.ViewModelProperty IsNot Nothing Then
                    Dim propdef = newColumn.DataSourceType.GetProperty(binding.ViewModelProperty.PropertyName)

                    newColumn.BoundPropertyInfo = propdef

                    If binding.Converter IsNot Nothing Then
                        newColumn.FilterConverterInstance = DirectCast(Activator.CreateInstance(binding.Converter), IValueConverter)
                    End If
                End If
            End If
        End If

        'Den Datentyp setzen:
        If DataSourceType IsNot Nothing Then
            newColumn.DataSourceType = Me.DataSourceType
        End If


        'Nur anlegen, wenn ein typ auch hinterlegt wurde...
        If Me.CustomColumnTemplateType IsNot Nothing AndAlso newColumn.ColumnTemplateExtender Is Nothing Then
            newColumn.ColumnTemplateExtender = DirectCast(Activator.CreateInstance(Me.CustomColumnTemplateType), IMvvmColumnTemplateExtender)
        End If


        'Column initialisieren
        Dim newWpfColumn = newColumn.InitializeColumn()

        'Tag zum identifizieren der Spalte setzen
        newWpfColumn.SetValue(MvvmDataGrid.GridColumnProperty, newColumn)

        If IsFilteringEnabled AndAlso newColumn.BoundPropertyInfo IsNot Nothing Then
            If TypeOf newWpfColumn Is DataGridTextColumn Then
                newWpfColumn.HeaderStyle = CType(WpfDataGridViewWrapper.InnerDataGridView.FindResource("DataGridColumnHeaderStyle"), Style)
            End If
        End If

        'Und dem internen DataGrid hinzufuegen
        Me.WpfDataGridViewWrapper.InnerDataGridView.Columns.Add(newWpfColumn)

        If Not MyBase.DesignMode AndAlso _mySettings IsNot Nothing Then
            _mySettings.ColumnDefinitions.Add(New ColumnDefinition() With {.DisplayIndex = newWpfColumn.DisplayIndex, .Name = newColumn.Name, .Width = newWpfColumn.Width.ToString()})
        End If

        If Not DesignMode Then
            DependencyPropertyDescriptor.FromProperty(DataGridColumn.WidthProperty,
                                                                                              GetType(DataGridColumn)).AddValueChanged(newWpfColumn,
                                                                                                                                       Sub(s, e)
                                                                                                                                           If _isInitialized Then
                                                                                                                                               UpdateAllColumnDefs()
                                                                                                                                           End If
                                                                                                                                       End Sub)
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

            Dim containsProp As Boolean = False

            'Schauen ob die Spalte schon vorhanden ist (also schon eine Bindung existiert)
            For Each column As MvvmDataGridColumn In Me.Columns
                For Each columnBinding In column.PropertyCellBindings
                    If columnBinding.ViewModelProperty.PropertyName = prop.Name AndAlso columnBinding.ViewModelProperty.PropertyType = prop.DeclaringType Then
                        'Bereits vorhanden
                        containsProp = True
                        Continue For
                    End If
                Next
                If containsProp Then Continue For
            Next
            If containsProp Then Continue For

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

            If Not prop.CanWrite OrElse Not prop.PropertyType.IsPrimitive OrElse
                    (prop.PropertyType.IsGenericType AndAlso
                    prop.PropertyType.GetGenericTypeDefinition Is GetType(Nullable(Of ))) Then
                If prop.PropertyType Is GetType(String) Then
                    bindingSetting.BindingMode = MvvmBindingModes.TwoWay
                Else
                    bindingSetting.BindingMode = MvvmBindingModes.OneWay
                End If
            Else
                bindingSetting.BindingMode = MvvmBindingModes.TwoWay
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
        OnSelectedIndexChanged(e)
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

            If IsFilteringEnabled AndAlso WpfDataGridViewWrapper.InnerDataGridView.SelectedItem Is Nothing Then
                e.Handled = True
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
        If IsFilteringEnabled AndAlso Not MyBase.DesignMode Then
            UpdateFilterAddons()
        End If
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

    ''' <summary>
    ''' Refresh:
    ''' </summary>
    Public Sub EndInit() Implements ISupportInitialize.EndInit
        Dim c = Columns.ToList()

        Columns.Clear()

        c.ForEach(Sub(itm) Columns.Add(itm))
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

    ''' <summary>
    ''' Scrollt zum übergebenen Objekt welches sich innerhalb der ItemsSource befindet
    ''' </summary>
    ''' <param name="item"></param>
    Public Sub ScrollIntoView(item As Object)
        Me.WpfDataGridViewWrapper.InnerDataGridView.ScrollIntoView(item)
    End Sub

    ''' <summary>
    ''' Liefert en Spaltenkopf von einer Spalte (Da UI-Visualisierung im Grid selber gemacht wird, gibt es keine direkte Referenz)
    ''' </summary>
    ''' <param name="column"></param>
    ''' <param name="reference"></param>
    ''' <returns></returns>
    Private Function GetHeader(column As DataGridColumn, reference As DependencyObject) As Primitives.DataGridColumnHeader
        For i As Integer = 0 To Media.VisualTreeHelper.GetChildrenCount(reference) - 1
            Dim child As DependencyObject = Media.VisualTreeHelper.GetChild(reference, i)

            Dim colHeader As DataGridColumnHeader = TryCast(child, DataGridColumnHeader)
            If (colHeader IsNot Nothing) AndAlso (colHeader.Column Is column) Then
                Return colHeader
            End If

            colHeader = GetHeader(column, child)
            If colHeader IsNot Nothing Then
                Return colHeader
            End If
        Next

        Return Nothing
    End Function

    ''' <summary>
    ''' Aktualisiert -wenn eingeschaltet- die Filter-UI für die Spaltenköpfe
    ''' </summary>
    Private Sub UpdateFilterAddons()
        Dim style = CType(WpfDataGridViewWrapper.InnerDataGridView.FindResource("DataGridColumnHeaderStyle"), Style)

        For Each c In Columns

            'Filteraddons pflegen
            If TypeOf c.WpfColumn Is DataGridTextColumn AndAlso c.BoundPropertyInfo IsNot Nothing Then
                For Each setter As Setter In style.Setters
                    If setter.Property Is DataGridColumnHeader.TemplateProperty Then
                        Dim value = DirectCast(setter.Value, ControlTemplate)
                        Dim header = GetHeader(c.WpfColumn, WpfDataGridViewWrapper.InnerDataGridView)
                        Dim column = c

                        Try
                            If header IsNot Nothing Then
                                Dim btn = DirectCast(value.FindName("PART_FilterButton", header), wpf.Button)
                                Dim tb = DirectCast(value.FindName("PART_FilterTextBox", header), wpf.TextBox)
                                Dim isClosedByTB As Boolean = False

                                If c.FilterButton IsNot btn OrElse c.FilterTextBox IsNot tb Then
                                    If c.FilterButton IsNot Nothing Then
                                        RemoveHandler c.FilterButton.Click, AddressOf FilterButton_Click
                                        RemoveHandler c.FilterButton.PreviewMouseDown, AddressOf FilterButton_PreviewMouseDown
                                    End If

                                    If c.FilterTextBox IsNot Nothing Then
                                        RemoveHandler c.FilterTextBox.KeyUp, AddressOf FilterTextBox_KeyUp
                                        RemoveHandler c.FilterTextBox.GotFocus, AddressOf FilterTextBox_GotFocus
                                        RemoveHandler c.FilterTextBox.LostFocus, AddressOf FilterTextBox_LostFocus
                                    End If

                                    c.FilterButton = btn
                                    c.FilterTextBox = tb

                                    AddHandler c.FilterButton.Click, AddressOf FilterButton_Click
                                    AddHandler c.FilterButton.PreviewMouseDown, AddressOf FilterButton_PreviewMouseDown
                                    AddHandler c.FilterTextBox.KeyUp, AddressOf FilterTextBox_KeyUp
                                    AddHandler c.FilterTextBox.GotFocus, AddressOf FilterTextBox_GotFocus
                                    AddHandler c.FilterTextBox.LostFocus, AddressOf FilterTextBox_LostFocus
                                End If

                            End If
                        Catch ex As InvalidOperationException
                            TraceError("Fehler beim Laden der Filter-Controls einer Spalte")
                        End Try
                    End If
                Next
            End If
        Next
    End Sub

    Private _isFilterClosedByTB As wpf.TextBox = Nothing

    Private Sub FilterButton_PreviewMouseDown(sender As Object, e As MouseButtonEventArgs)
        Dim btn = DirectCast(sender, wpf.Button)
        Dim column = Columns.Where(Function(c) c.FilterButton Is btn).SingleOrDefault()

        If column IsNot Nothing Then
            If column.FilterTextBox.IsFocused AndAlso String.IsNullOrWhiteSpace(column.FilterTextBox.Text) Then
                _isFilterClosedByTB = column.FilterTextBox
            End If
        End If

    End Sub

    Private Sub FilterButton_Click(sender As Object, e As RoutedEventArgs)
        Dim btn = DirectCast(sender, wpf.Button)
        Dim column = Columns.Where(Function(c) c.FilterButton Is btn).SingleOrDefault()

        If _isFilterClosedByTB IsNot Nothing AndAlso column.FilterTextBox Is _isFilterClosedByTB Then
            _isFilterClosedByTB = Nothing
            Return
        End If

        If column.FilterTextBox.Visibility = Visibility.Visible Then
            column.FilterTextBox.Visibility = Visibility.Collapsed
            column.AddFilterButton()
            ResetFilter()
        Else
            CloseAllFilter()
            column.FilterTextBox.Visibility = Visibility.Visible
            column.RemoveFilterButton()
            column.FilterTextBox.Focus()
        End If
    End Sub

    Private Sub FilterTextBox_LostFocus(sender As Object, e As RoutedEventArgs)
        Dim tb = DirectCast(sender, wpf.TextBox)
        Dim column = Columns.Where(Function(c) c.FilterTextBox Is tb).SingleOrDefault()

        If String.IsNullOrWhiteSpace(tb.Text) Then
            tb.Visibility = Visibility.Collapsed
            column.AddFilterButton()
            tb.Text = String.Empty
            ResetFilter()
        End If
    End Sub

    Private Sub FilterTextBox_GotFocus(sender As Object, e As RoutedEventArgs)
        WpfDataGridViewWrapper.InnerDataGridView.SelectedItem = Nothing
    End Sub

    Private Sub FilterTextBox_KeyUp(sender As Object, e As Input.KeyEventArgs)
        Dim tb = DirectCast(sender, wpf.TextBox)
        Dim c = Columns.Where(Function(col) col.FilterTextBox Is tb).SingleOrDefault()

        If e.Key = Key.Enter Then
            'Filter anwenden
            FilterColumn(tb.Text, c)
        End If
    End Sub


    ''' <summary>
    ''' Schließt alle eventuell geöffneten Filteraddons
    ''' </summary>
    Private Sub CloseAllFilter()
        Dim style = CType(WpfDataGridViewWrapper.InnerDataGridView.FindResource("DataGridColumnHeaderStyle"), Style)

        For Each c In Columns

            'Filteraddons pflegen
            ClearColumn(c)
        Next
    End Sub

    Private Sub ClearColumn(c As MvvmDataGridColumn)
        If TypeOf c.WpfColumn Is DataGridTextColumn Then
            If c.FilterTextBox IsNot Nothing AndAlso c.FilterButton IsNot Nothing Then
                c.FilterTextBox.Text = String.Empty
                c.FilterTextBox.Visibility = Visibility.Collapsed
                c.AddFilterButton()
            End If
        End If
    End Sub

    ''' <summary>
    ''' Setzt den Filter in der CVS zurück
    ''' </summary>
    Private Sub ResetFilter()
        If _collectionView IsNot Nothing AndAlso _collectionView.Filter IsNot Nothing Then
            _collectionView.Filter = Nothing
        End If
    End Sub

    ''' <summary>
    ''' Filtert eine Spalte
    ''' </summary>
    ''' <param name="filterString"></param>
    ''' <param name="column"></param>
    Private Sub FilterColumn(filterString As String, column As MvvmDataGridColumn)
        If _collectionView IsNot Nothing Then
            If String.IsNullOrWhiteSpace(filterString) Then
                _collectionView.Filter = Nothing
            Else
                Try
                    _collectionView.Filter = Function(p)
                                                 Dim suchStr = filterString
                                                 Dim prop = column.BoundPropertyInfo
                                                 Dim binding = column.PropertyCellBindings.Where(Function(pb) pb.ControlProperty.PropertyName = "Content").SingleOrDefault()

                                                 If binding IsNot Nothing AndAlso binding.Converter IsNot Nothing AndAlso column.FilterConverterInstance IsNot Nothing Then
                                                     'mit converter

                                                     Dim val = prop.GetValue(p)
                                                     Dim convertedValue = column.FilterConverterInstance.Convert(val, GetType(String), binding.ConverterParameter, Globalization.CultureInfo.CurrentCulture)

                                                     If convertedValue IsNot Nothing Then
                                                         Return FilterColumnValue(suchStr, convertedValue.ToString())
                                                     Else
                                                         Return FilterColumnValue(suchStr, Nothing)
                                                     End If
                                                 Else
                                                     If prop.PropertyType = GetType(String) Then
                                                         Dim val = DirectCast(prop.GetValue(p), String)

                                                         Return FilterColumnValue(suchStr, val)
                                                     Else

                                                         Dim val = prop.GetValue(p)

                                                         If val IsNot Nothing Then
                                                             Return FilterColumnValue(suchStr, val.ToString())
                                                         Else
                                                             Return FilterColumnValue(suchStr, Nothing)
                                                         End If
                                                     End If
                                                 End If

                                                 Return False
                                             End Function
                Catch ex As InvalidOperationException
                    Forms.MessageBox.Show(ex.Message, "Fehler beim Filtern", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    ClearColumn(column)
                End Try
            End If
        End If
    End Sub

    ''' <summary>
    ''' Filtert einen Wert in einer Spalte
    ''' </summary>
    ''' <param name="suchStr"></param>
    ''' <param name="val"></param>
    ''' <returns></returns>
    Private Function FilterColumnValue(suchStr As String, val As String) As Boolean
        If val Is Nothing Then
            Return False
        End If

        If Not FilterCaseSensitive Then
            suchStr = suchStr.ToLower()
            val = val.ToLower()
        End If

        If suchStr.First = "*"c AndAlso Not suchStr.Last = "*"c Then
            If val.EndsWith(suchStr.Trim("*"c)) Then
                Return True
            End If
        ElseIf Not suchStr.First = "*"c AndAlso suchStr.Last = "*"c Then
            If val.StartsWith(suchStr.Trim("*"c)) Then
                Return True
            End If
        ElseIf suchStr.First = "*"c AndAlso suchStr.Last = "*"c Then
            If val.Contains(suchStr.Trim("*"c)) Then
                Return True
            End If
        ElseIf Not suchStr.Contains("*"c) Then
            If suchStr = val Then
                Return True
            End If
        Else
            Return False
        End If

        Return False
    End Function

    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub
End Class

