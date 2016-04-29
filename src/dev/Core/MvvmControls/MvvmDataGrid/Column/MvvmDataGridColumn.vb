Imports System.Windows.Media
Imports System.ComponentModel
Imports System.Windows.Controls
Imports System.Runtime.InteropServices
Imports System.Drawing
Imports System.ComponentModel.Design.Serialization
Imports System.CodeDom
Imports System.Reflection
Imports System.Windows
Imports System.Drawing.Design
Imports System.Windows.Forms.Design
Imports System.Windows.Data
Imports System.Windows.Forms
Imports System.Windows.Controls.Primitives

'<Designer(GetType(MvvmDataGridColumnControlDesigner))> 'waere dafuer da, dass wenn die column ein control waer, das die props aus control im designer ignoriert werden

''' <summary>
''' Columnklasse welche die WPF-Column wrapped 
''' </summary>
''' <remarks></remarks>
<Serializable>
Public Class MvvmDataGridColumn
    Implements IMvvmManager

    Shared Sub New()
        _bindingMappings = New Dictionary(Of String, System.Action(Of DataGridColumn, System.Windows.Data.Binding))
        AddBindingMappings()
    End Sub

    ''' <summary>
    ''' Dieser Name wird für alle neu angelegten Spalten verwendet
    ''' </summary>
    ''' <remarks></remarks>
    Friend Const NewColumnName As String = "-new column-"

    Public Sub New()
        'Hier muss im Ctor ein Spaltenname gesetzt werden, da er sonst noch durch die Serialisierung keinen Namen bekommen hat und er willkürlich versucht aus der Column bei einer Änderungen einen Namen für die Anzeuge zu bekommen (Header)
        Me.Name = NewColumnName
    End Sub

    Private _textWrapping As TextWrapping = TextWrapping.NoWrap

    ''' <summary>
    ''' Definiert ob der Text in der Spalte umgebrochen werden soll
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Category("Column")>
    Public Property TextWrapping As TextWrapping
        Get
            Return _textWrapping
        End Get
        Set(ByVal value As TextWrapping)
            _textWrapping = value
        End Set
    End Property

    Private _propertyBindings As New PropertyBindings()

    ''' <summary>
    ''' Hier werden die Bindungen fuer eine Zelle gespeichert
    ''' </summary>
    ''' <returns></returns>
    <Category("MVVM"), Editor(GetType(ColumnBindingsUITypeEditor), GetType(UITypeEditor)),
 DesignerSerializationVisibility(DesignerSerializationVisibility.Content)>
    Public Property PropertyCellBindings As PropertyBindings
        Get
            Return _propertyBindings
        End Get
        Set(ByVal value As PropertyBindings)
            _propertyBindings = value
        End Set
    End Property

    Public Function ShouldSerializePropertyCellBindings() As Boolean
        Return False
    End Function

    Private _dataSourceType As Type

    ''' <summary>
    ''' Hier wird der Typ vom Anwendungsentwickler angegeben, welcher fuer die Bindungen einer Spalte verwendet werden soll
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Browsable(False)>
    Property DataSourceType As Type Implements IMvvmManager.DataContextType
        Get
            Return _dataSourceType
        End Get
        Set(value As Type)
            _dataSourceType = value
        End Set
    End Property

    Private _wpfColumn As DataGridColumn

    ''' <summary>
    ''' Hier wird die intern verwendete WPF-Column-Instanz gespeichert
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Friend ReadOnly Property WpfColumn As DataGridColumn
        Get
            Return _wpfColumn
        End Get
        'Set(ByVal value As DataGridColumn)
        '    _wpfColumn = value
        'End Set
    End Property

    Public Function ShouldSerializeWpfColumn() As Boolean
        Return False
    End Function

    ''' <summary>
    ''' Erstellt anhand der MVVM-PropertyBindings die Bindungen fuer die WPF-Column und uebersetzt diese
    ''' </summary>
    ''' <remarks></remarks>
    Public Function InitializeColumn() As DataGridColumn

        'Neue Column anhand des Typs anlegen
        Select Case Me.ColumnType
            Case ColumnType.CheckBox
                Dim column = New DataGridCheckBoxColumn()

                If Me.ColumnTemplateExtender IsNot Nothing Then
                    Me.ColumnTemplateExtender.InitilizeColumn(column)
                End If

                _wpfColumn = column
            Case ColumnType.ComboBox
                Dim column = New MvvmDataGridComboBoxColumn()
                column.CellTemplate = New DataTemplate()

                Dim cboElement = New FrameworkElementFactory(GetType(Controls.ComboBox))
                cboElement.SetValue(Controls.ComboBox.DisplayMemberPathProperty, Me.DisplayMemberPath)

                column.CellTemplate.VisualTree = cboElement

                If Me.ColumnTemplateExtender IsNot Nothing Then
                    Me.ColumnTemplateExtender.InitilizeColumn(column)
                End If
                _wpfColumn = column
            Case EntitiesFormsLib.ColumnType.TextAndNumbers
                Dim textcolumn = New DataGridTextColumn()

                If Me.ColumnTemplateExtender IsNot Nothing Then
                    Me.ColumnTemplateExtender.InitilizeColumn(textcolumn)
                End If
                _wpfColumn = textcolumn
            Case EntitiesFormsLib.ColumnType.Hyperlink
                Dim textcolumn = New DataGridHyperlinkColumn()
                _wpfColumn = textcolumn
            Case EntitiesFormsLib.ColumnType.Image
                Dim column = New MvvmDataGridImageColumn()

                column.CellTemplate = New DataTemplate()

                Dim imageElement = New FrameworkElementFactory(GetType(Controls.Image))
                imageElement.SetValue(Controls.Image.SourceProperty, Me.Content)
                imageElement.SetValue(Controls.Image.UseLayoutRoundingProperty, True)
                imageElement.SetValue(Controls.Image.StretchProperty, Stretch.None)

                column.CellTemplate.VisualTree = imageElement

                If Me.ColumnTemplateExtender IsNot Nothing Then
                    Me.ColumnTemplateExtender.InitilizeColumn(column)
                End If
                _wpfColumn = column
        End Select

        Dim dataGridBoundColumn = TryCast(Me.WpfColumn, DataGridBoundColumn)

        MapCommonColumnProperties(Me.WpfColumn)

        If dataGridBoundColumn IsNot Nothing Then
            'Zunaechst werden die Werte (welcher der Anwendungsentwickler gesetzt hat) initialisiert:
            MapDataGridBoundColumnProperties(dataGridBoundColumn)
        End If

        'Allgemeine Columnwerte:
        Me.WpfColumn.Header = Me.Header 'Fuer Header

        'Hier werden jetzt die EFL-PropertyBindings in die WPF-Bindings uebersetzt
        For Each binding In Me.PropertyCellBindings
            'Bindingobjekt anlegen
            Dim wpfBindingLocal = New System.Windows.Data.Binding(binding.ViewModelProperty.PropertyName)

            If binding.Converter IsNot Nothing Then
                'Mit Konverter:
                wpfBindingLocal.Converter = DirectCast(Activator.CreateInstance(binding.Converter), IValueConverter)
                wpfBindingLocal.ConverterParameter = binding.ConverterParameter
            End If

            'Mode
            Select Case binding.BindingSetting.BindingMode
                Case MvvmBindingModes.OneTime
                    wpfBindingLocal.Mode = BindingMode.OneTime
                Case MvvmBindingModes.OneWay
                    wpfBindingLocal.Mode = BindingMode.OneWay
                Case MvvmBindingModes.OneWayToSource
                    wpfBindingLocal.Mode = BindingMode.OneWayToSource
                Case MvvmBindingModes.TwoWay
                    wpfBindingLocal.Mode = BindingMode.TwoWay
                Case Else
                    wpfBindingLocal.Mode = BindingMode.Default
            End Select

            'Trigger
            Select Case binding.BindingSetting.UpdateSourceTrigger
                Case UpdateSourceTriggerSettings.Explicit
                    wpfBindingLocal.UpdateSourceTrigger = UpdateSourceTrigger.Explicit
                Case UpdateSourceTriggerSettings.LostFocus
                    wpfBindingLocal.UpdateSourceTrigger = UpdateSourceTrigger.LostFocus
                Case UpdateSourceTriggerSettings.PropertyChangedImmediately
                    wpfBindingLocal.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                Case Else
                    wpfBindingLocal.UpdateSourceTrigger = UpdateSourceTrigger.Default
            End Select

            'Es muss ein Mapping von unseren Properties zu den Properties in WPF existieren
            'BindingOperations.SetBinding(ALLESMOEGLICHE_OBJECT, ALLESMOEGLICHE_PROPERTY, wpfBinding)

            If _bindingMappings.ContainsKey(binding.ControlProperty.PropertyName) Then
                _bindingMappings(binding.ControlProperty.PropertyName)(Me.WpfColumn, wpfBindingLocal)
            Else
                'Es wirde keine Mappinglogik gefunden, in AddBindingMappings muss diese angegeben werden
                System.Windows.MessageBox.Show("keine bindingmappinglogik fuer " & binding.ControlProperty.ToString() & " gefunden!")
            End If

        Next

        Return Me.WpfColumn
    End Function

    ''' <summary>
    ''' Interfaceimplementierung
    ''' </summary>
    ''' <param name="ctrl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetPropertyBindings(ctrl As Forms.Control) As PropertyBindings Implements IMvvmManager.GetPropertyBindings
        Return _propertyBindings
    End Function

#Region "BindableProperties"
    ''' <summary>
    ''' Speichert statisch die Bindingmappings, welche dann in der Initialisierungsphase der Column verwendet werden
    ''' </summary>
    ''' <remarks></remarks>
    Private Shared _bindingMappings As Dictionary(Of String, Action(Of DataGridColumn, System.Windows.Data.Binding))
    ''' <summary>
    ''' Hier werden die Bindungen (welche der Anwendungsentwickler definiert hat) auf die WPF-Bindungen gemapped 
    ''' </summary>
    ''' <remarks></remarks>
    Private Shared Sub AddBindingMappings()
        _bindingMappings.Add(ContentProperty, Sub(wpfColumn As DataGridColumn, wpfBinding As System.Windows.Data.Binding)
                                                  Dim bindableColumn = TryCast(wpfColumn, DataGridBoundColumn)
                                                  If bindableColumn IsNot Nothing Then
                                                      'Hiermit werden CheckBox, Hyperlink und TextColumn gemapped
                                                      bindableColumn.Binding = wpfBinding
                                                  End If

                                                  Dim cboColumn = TryCast(wpfColumn, MvvmDataGridComboBoxColumn)
                                                  If cboColumn IsNot Nothing Then
                                                      'Hiermit wird die ComboBox gemapped
                                                      Dim cboElement = DirectCast(cboColumn.CellTemplate.VisualTree, FrameworkElementFactory)
                                                      cboElement.SetBinding(Controls.ComboBox.SelectedItemProperty, wpfBinding)

                                                      'TODO: SelectedValuePath fehlt.
                                                  End If

                                                  Dim imageColumn = TryCast(wpfColumn, MvvmDataGridImageColumn)
                                                  If imageColumn IsNot Nothing Then
                                                      'Hiermit wird die ComboBox gemapped
                                                      Dim imageElement = DirectCast(imageColumn.CellTemplate.VisualTree, FrameworkElementFactory)
                                                      imageElement.SetBinding(Controls.Image.SourceProperty, wpfBinding)
                                                  End If

                                              End Sub)
        _bindingMappings.Add(BackgroundColorProperty, Sub(wpfColumn As DataGridColumn, wpfBinding As System.Windows.Data.Binding)
                                                          'Brush anlegen
                                                          Dim colorBrush = New SolidColorBrush()

                                                          'LostFocus wird bei SolidColorBrush nicht unterstuetzt!
                                                          If wpfBinding.UpdateSourceTrigger = UpdateSourceTrigger.LostFocus Then
                                                              wpfBinding.UpdateSourceTrigger = UpdateSourceTrigger.Default
                                                          End If

                                                          'TwoWay fuehrt bei bearbeiten einer Zelle zur Exception!
                                                          If wpfBinding.Mode = BindingMode.TwoWay Then
                                                              wpfBinding.Mode = BindingMode.Default
                                                          End If

                                                          'Bindung an Brush
                                                          BindingOperations.SetBinding(colorBrush, SolidColorBrush.ColorProperty, wpfBinding)

                                                          'Brush im Style verwenden
                                                          wpfColumn.CellStyle.Setters.Add(New Setter(Controls.DataGridCell.BackgroundProperty, colorBrush))
                                                      End Sub)
        _bindingMappings.Add(IsEnabledProperty, Sub(wpfColumn As DataGridColumn, wpfBinding As System.Windows.Data.Binding)
                                                    wpfColumn.IsReadOnly = False
                                                    wpfColumn.CellStyle.Setters.Add(New Setter(Controls.DataGridCell.IsEnabledProperty, wpfBinding))
                                                End Sub)

        _bindingMappings.Add(ForegroundColorProperty, Sub(wpfColumn As DataGridColumn, wpfBinding As System.Windows.Data.Binding)
                                                          Dim colorBrush = New SolidColorBrush()

                                                          'LostFocus wird bei SolidColorBrush nicht unterstuetzt!
                                                          If wpfBinding.UpdateSourceTrigger = UpdateSourceTrigger.LostFocus Then
                                                              wpfBinding.UpdateSourceTrigger = UpdateSourceTrigger.Default
                                                          End If

                                                          'TwoWay fuehrt bei bearbeiten einer Zelle zur Exception!
                                                          If wpfBinding.Mode = BindingMode.TwoWay Then
                                                              wpfBinding.Mode = BindingMode.Default
                                                          End If

                                                          BindingOperations.SetBinding(colorBrush, SolidColorBrush.ColorProperty, wpfBinding)

                                                          wpfColumn.CellStyle.Setters.Add(New Setter(Controls.DataGridCell.ForegroundProperty, colorBrush))
                                                      End Sub)

        _bindingMappings.Add(HorizontalAlignmentProperty, Sub(wpfColumn As DataGridColumn, wpfBinding As System.Windows.Data.Binding)
                                                              wpfColumn.CellStyle.Setters.Add(New Setter(Controls.DataGridCell.HorizontalContentAlignmentProperty, wpfBinding))
                                                          End Sub)

        _bindingMappings.Add(VerticalAlignmentProperty, Sub(wpfColumn As DataGridColumn, wpfBinding As System.Windows.Data.Binding)
                                                            wpfColumn.CellStyle.Setters.Add(New Setter(Controls.DataGridCell.VerticalContentAlignmentProperty, wpfBinding))
                                                        End Sub)

        _bindingMappings.Add(CellPaddingProperty, Sub(wpfColumn As DataGridColumn, wpfBinding As System.Windows.Data.Binding)
                                                      Dim bindableColumn = TryCast(wpfColumn, DataGridBoundColumn)
                                                      If bindableColumn IsNot Nothing Then
                                                          Dim setter = New Setter(TextBlock.PaddingProperty, wpfBinding)
                                                          bindableColumn.ElementStyle.Setters.Add(setter)
                                                      End If
                                                  End Sub)

        _bindingMappings.Add(WidthProperty, Sub(wpfColumn As DataGridColumn, wpfBinding As System.Windows.Data.Binding)
                                                wpfColumn.CellStyle.Setters.Add(New Setter(Controls.DataGridCell.WidthProperty, wpfBinding))
                                            End Sub)

        _bindingMappings.Add(VisibilityProperty, Sub(wpfColumn As DataGridColumn, wpfBinding As System.Windows.Data.Binding)
                                                     wpfColumn.CellStyle.Setters.Add(New Setter(UIElement.VisibilityProperty, wpfBinding))
                                                 End Sub)

        _bindingMappings.Add(ComboBoxItemsSourceProperty, Sub(wpfColumn As DataGridColumn, wpfBinding As System.Windows.Data.Binding)
                                                              Dim cboColumn = TryCast(wpfColumn, MvvmDataGridComboBoxColumn)

                                                              If cboColumn IsNot Nothing Then
                                                                  Dim cboElement = DirectCast(cboColumn.CellTemplate.VisualTree, FrameworkElementFactory)
                                                                  cboElement.SetBinding(Controls.ComboBox.ItemsSourceProperty, wpfBinding)
                                                              End If

                                                          End Sub)

        _bindingMappings.Add(FontStyleProperty, Sub(wpfColumn As DataGridColumn, wpfBinding As System.Windows.Data.Binding)

                                                    wpfColumn.CellStyle.Setters.Add(New Setter(Controls.DataGridCell.FontStyleProperty, wpfBinding))

                                                    Dim bindableColumn = TryCast(wpfColumn, DataGridBoundColumn)
                                                    If bindableColumn IsNot Nothing Then
                                                        bindableColumn.ElementStyle.Setters.Add(New Setter(TextBlock.FontStyleProperty, wpfBinding))
                                                    End If
                                                End Sub)
        _bindingMappings.Add(FontWeightProperty, Sub(wpfColumn As DataGridColumn, wpfBinding As System.Windows.Data.Binding)

                                                     wpfColumn.CellStyle.Setters.Add(New Setter(Controls.DataGridCell.FontWeightProperty, wpfBinding))

                                                     Dim bindableColumn = TryCast(wpfColumn, DataGridBoundColumn)

                                                     If bindableColumn IsNot Nothing Then
                                                         bindableColumn.ElementStyle.Setters.Add(New Setter(TextBlock.FontWeightProperty, wpfBinding))
                                                     End If
                                                 End Sub)

        _bindingMappings.Add(FontFamilyProperty, Sub(wpfColumn As DataGridColumn, wpfBinding As System.Windows.Data.Binding)

                                                     wpfColumn.CellStyle.Setters.Add(New Setter(Controls.DataGridCell.FontFamilyProperty, wpfBinding))

                                                     Dim bindableColumn = TryCast(wpfColumn, DataGridBoundColumn)

                                                     If bindableColumn IsNot Nothing Then

                                                         bindableColumn.ElementStyle.Setters.Add(New Setter(TextBlock.FontFamilyProperty, wpfBinding))
                                                     End If
                                                 End Sub)
    End Sub

    ''' <summary>
    ''' Hier werden initial die allgmeinen Werte (welcher der Anwendungsentwickler definiert hat) um die Properties auf die WPF-Column zu mappen
    ''' </summary>
    ''' <param name="dataGridColumn"></param>
    ''' <remarks></remarks>
    Private Sub MapCommonColumnProperties(dataGridColumn As DataGridColumn)
        dataGridColumn.HeaderStyle = New System.Windows.Style()


        If Me.ColumnHeaderFont IsNot Nothing Then
            dataGridColumn.HeaderStyle.Setters.Add(New Setter(DataGridColumnHeader.FontFamilyProperty, New System.Windows.Media.FontFamily(Me.ColumnHeaderFont.Name)))
            dataGridColumn.HeaderStyle.Setters.Add(New Setter(DataGridColumnHeader.FontWeightProperty, If(Me.ColumnHeaderFont.Bold, System.Windows.FontWeights.Bold, System.Windows.FontWeights.Regular)))
            dataGridColumn.HeaderStyle.Setters.Add(New Setter(DataGridColumnHeader.FontStyleProperty, If(Me.ColumnHeaderFont.Italic, System.Windows.FontStyles.Italic, System.Windows.FontStyles.Normal)))
            dataGridColumn.HeaderStyle.Setters.Add(New Setter(DataGridColumnHeader.FontSizeProperty, Convert.ToDouble(Me.ColumnHeaderFont.Size)))
        End If

        If Me.ColumnHeaderPadding <> Forms.Padding.Empty Then
            dataGridColumn.HeaderStyle.Setters.Add(New Setter(DataGridColumnHeader.PaddingProperty, New Thickness(Me.ColumnHeaderPadding.Left, Me.ColumnHeaderPadding.Top, Me.ColumnHeaderPadding.Right, Me.ColumnHeaderPadding.Bottom)))
        End If


    End Sub

    ''' <summary>
    ''' Hier werden die DataGridBoundColumn spezifischen Properties initial die Werte (welcher der Anwendungsentwickler definiert hat) um die Properties auf die WPF-Column zu mappen
    ''' </summary>
    ''' <param name="dataGridBoundColumn"></param>
    ''' <remarks></remarks>
    Private Sub MapDataGridBoundColumnProperties(dataGridBoundColumn As DataGridBoundColumn)
        Dim colorBrush As SolidColorBrush

        'Neue Styles anlegen
        dataGridBoundColumn.ElementStyle = New System.Windows.Style()
        dataGridBoundColumn.EditingElementStyle = New System.Windows.Style()
        dataGridBoundColumn.CellStyle = New System.Windows.Style()

        'Background:
        If Me.BackgroundColor <> System.Drawing.Color.Empty Then
            colorBrush = New SolidColorBrush(System.Windows.Media.Color.FromArgb(Me.BackgroundColor.A, Me.BackgroundColor.R, Me.BackgroundColor.G, Me.BackgroundColor.B))
            dataGridBoundColumn.CellStyle.Setters.Add(New Setter(Controls.DataGridCell.BackgroundProperty, colorBrush))
        End If

        'IsEnabled:
        If TryCast(dataGridBoundColumn, DataGridCheckBoxColumn) IsNot Nothing Then
            'CheckBox muss anders gemacht werden (kann dann aber mit Click auch nicht mehr selektiert werden):
            dataGridBoundColumn.CellStyle.Setters.Add(New Setter(Controls.DataGridCell.IsEnabledProperty, Me.IsEnabled))
        Else
            'alle anderen MUSS ich mit ReadOnly machen, weil ich sonst die Zeile nicht mehr anklicken kann
            dataGridBoundColumn.IsReadOnly = Not Me.IsEnabled
        End If


        'Foreground:
        If Me.ForegroundColor <> System.Drawing.Color.Empty Then
            colorBrush = New SolidColorBrush(System.Windows.Media.Color.FromArgb(Me.ForegroundColor.A, Me.ForegroundColor.R, Me.ForegroundColor.G, Me.ForegroundColor.B))

            dataGridBoundColumn.CellStyle.Setters.Add(New Setter(Controls.DataGridCell.ForegroundProperty, colorBrush))
        End If


        'CellPadding
        'dataGridBoundColumn.CellStyle.Setters.Add(New Setter(Controls.DataGridCell.PaddingProperty, New Thickness(Me.CellPadding.Left, Me.CellPadding.Top, Me.CellPadding.Right, Me.CellPadding.Bottom)))
        dataGridBoundColumn.ElementStyle.Setters.Add(New Setter(TextBlock.PaddingProperty, New Thickness(Me.CellPadding.Left, Me.CellPadding.Top, Me.CellPadding.Right, Me.CellPadding.Bottom)))
        dataGridBoundColumn.EditingElementStyle.Setters.Add(New Setter(System.Windows.Controls.TextBox.PaddingProperty, New Thickness(Me.CellPadding.Left, Me.CellPadding.Top, Me.CellPadding.Right, Me.CellPadding.Bottom)))

        Dim textcolumn = TryCast(dataGridBoundColumn, DataGridTextColumn)

        If textcolumn IsNot Nothing Then
            'Font:
            If Me.Font IsNot Nothing Then
                textcolumn.FontFamily = New System.Windows.Media.FontFamily(Me.Font.Name)
                textcolumn.FontWeight = If(Me.Font.Bold, System.Windows.FontWeights.Bold, System.Windows.FontWeights.Regular)
                textcolumn.FontStyle = If(Me.Font.Italic, System.Windows.FontStyles.Italic, System.Windows.FontStyles.Normal)
                textcolumn.FontSize = Font.Size
            End If

            'MultiLine
            If TextWrapping <> TextWrapping.NoWrap Then
                textcolumn.ElementStyle.Setters.Add(New Setter(TextBlock.TextWrappingProperty, TextWrapping))

                textcolumn.EditingElementStyle.Setters.Add(New Setter(Controls.TextBox.TextWrappingProperty, TextWrapping))
            End If
        End If

        'VerticalAlignment
        dataGridBoundColumn.ElementStyle.Setters.Add(New Setter(Controls.Control.VerticalAlignmentProperty, Me.VerticalAlignment))
        dataGridBoundColumn.EditingElementStyle.Setters.Add(New Setter(Controls.Control.VerticalAlignmentProperty, Me.VerticalAlignment))

        'HorizontalAlignment
        dataGridBoundColumn.ElementStyle.Setters.Add(New Setter(Controls.Control.HorizontalAlignmentProperty, Me.HorizontalAlignment))
        dataGridBoundColumn.EditingElementStyle.Setters.Add(New Setter(Controls.Control.HorizontalAlignmentProperty, Me.HorizontalAlignment))


        'Width
        If Me.Width.HasValue Then
            dataGridBoundColumn.Width = New DataGridLength(Me.Width.Value, Me.WidthLengthUnitType)
        End If

        'Visibility
        dataGridBoundColumn.Visibility = Me.Visibility

    End Sub

    Private _fontFamily As Media.FontFamily
    ''' <summary>
    ''' Schriftart
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>Durch den MvvmManager bindbare View-Property</remarks>
    <Browsable(False)>
    Public Property FontFamily As Media.FontFamily
        Get
            Return _fontFamily
        End Get
        Set(ByVal value As Media.FontFamily)
            If Not Object.Equals(_fontFamily, value) Then
                _fontFamily = value
                OnFontFamilyChanged(EventArgs.Empty)
            End If
        End Set
    End Property

    Public Event FontFamilyChanged As EventHandler

    Public Const FontFamilyProperty As String = "FontFamily"

    Protected Overridable Sub OnFontFamilyChanged(e As EventArgs)
        RaiseEvent FontFamilyChanged(Me, e)
    End Sub


    Private _fontWeight As FontWeight
    ''' <summary>
    ''' Schriftform
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>Durch den MvvmManager bindbare View-Property</remarks>
    <Browsable(False)>
    Public Property FontWeight As FontWeight
        Get
            Return _fontWeight
        End Get
        Set(ByVal value As FontWeight)
            If Not Object.Equals(_fontWeight, value) Then
                _fontWeight = value
                OnFontWeightChanged(EventArgs.Empty)
            End If
        End Set
    End Property

    Public Event FontWeightChanged As EventHandler

    Protected Overridable Sub OnFontWeightChanged(e As EventArgs)
        RaiseEvent FontWeightChanged(Me, e)
    End Sub

    Public Const FontWeightProperty As String = "FontWeight"

    Private _fontStyle As System.Windows.FontStyle
    ''' <summary>
    ''' FontStyle
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>Durch den MvvmManager bindbare View-Property</remarks>
    <Browsable(False)>
    Public Property FontStyle As System.Windows.FontStyle
        Get
            Return _fontStyle
        End Get
        Set(ByVal value As System.Windows.FontStyle)
            If Not Object.Equals(_fontStyle, value) Then
                _fontStyle = value
                OnFontStyleChanged(EventArgs.Empty)
            End If
        End Set
    End Property

    Public Event FontStyleChanged As EventHandler

    Public Const FontStyleProperty As String = "FontStyle"

    Protected Overridable Sub OnFontStyleChanged(e As EventArgs)
        RaiseEvent FontStyleChanged(Me, e)
    End Sub


    Private _comboBoxItemsSource As Object
    ''' <summary>
    ''' Falls als Typ eine Combobox ausgewaehlt wurde, kann hier die Bindung zur ItemsSource definiert werden
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>Durch den MvvmManager bindbare View-Property</remarks>
    <Browsable(False)>
    Public Property ComboBoxItemsSource As Object
        Get
            Return _comboBoxItemsSource
        End Get
        Set(ByVal value As Object)
            If Not Object.Equals(_comboBoxItemsSource, value) Then
                _comboBoxItemsSource = value
                OnComboBoxItemsSourceChanged(EventArgs.Empty)
            End If
        End Set
    End Property

    Public Event ComboBoxItemsSourceChanged As EventHandler

    Public Const ComboBoxItemsSourceProperty As String = "ComboBoxItemsSource"

    Protected Overridable Sub OnComboBoxItemsSourceChanged(e As EventArgs)
        RaiseEvent ComboBoxItemsSourceChanged(Me, e)
    End Sub


    Private _width As Nullable(Of Double) = -1
    ''' <summary>
    ''' Spaltenbreite
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>Durch den MvvmManager bindbare View-Property</remarks>
    <Category("Column")>
    Public Property Width As Nullable(Of Double)
        Get
            Return _width
        End Get
        Set(ByVal value As Nullable(Of Double))
            If Not Object.Equals(_width, value) Then
                _width = value
                OnWidthChanged(EventArgs.Empty)
            End If
        End Set
    End Property

    Public Const WidthProperty As String = "Width"

    Public Event WidthChanged As EventHandler

    Protected Overridable Sub OnWidthChanged(e As EventArgs)
        RaiseEvent WidthChanged(Me, e)
    End Sub

    Private _widthLengthUnitType As DataGridLengthUnitType = DataGridLengthUnitType.Star
    ''' <summary>
    ''' Typ der Angabe der Spaltenbreite
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>Durch den MvvmManager bindbare View-Property</remarks>
    <Category("Column")>
    Public Property WidthLengthUnitType As DataGridLengthUnitType
        Get
            Return _widthLengthUnitType
        End Get
        Set(ByVal value As DataGridLengthUnitType)
            If Not Object.Equals(_widthLengthUnitType, value) Then
                _widthLengthUnitType = value
                OnWidthLengthUnitTypeChanged(EventArgs.Empty)
            End If
        End Set
    End Property

    Public Event WidthLengthUnitTypeChanged As EventHandler

    Protected Overridable Sub OnWidthLengthUnitTypeChanged(e As EventArgs)
        RaiseEvent WidthLengthUnitTypeChanged(Me, e)
    End Sub


    Private _visibility As Visibility = System.Windows.Visibility.Visible
    ''' <summary>
    ''' Sichtbarkeit der Spalte
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>Durch den MvvmManager bindbare View-Property</remarks>
    <Category("Column")>
    Public Property Visibility As Visibility
        Get
            Return _visibility
        End Get
        Set(ByVal value As Visibility)
            If Not Object.Equals(_visibility, value) Then
                If Me.WpfColumn IsNot Nothing Then
                    Me.WpfColumn.Visibility = value
                End If

                _visibility = value
                OnVisibilityChanged(EventArgs.Empty)
            End If
        End Set
    End Property

    Public Const VisibilityProperty As String = "Visibility"

    Public Event VisibilityChanged As EventHandler

    Protected Overridable Sub OnVisibilityChanged(e As EventArgs)
        RaiseEvent VisibilityChanged(Me, e)
    End Sub



    Private _content As Object
    ''' <summary>
    ''' Der Inhalt einer Zelle
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>Durch den MvvmManager bindbare View-Property</remarks>
    Public Property Content As Object
        Get
            Return _content
        End Get
        Set(ByVal value As Object)
            If Not Object.Equals(_content, value) Then
                _content = value
                OnContentChanged(EventArgs.Empty)
            End If
        End Set
    End Property

    Const ContentProperty As String = "Content"

    Public Event ContentChanged As EventHandler

    Protected Overridable Sub OnContentChanged(e As EventArgs)
        RaiseEvent ContentChanged(Me, e)
    End Sub

    Private _cellPadding As Padding
    ''' <summary>
    ''' Abstand der Zelle nach aussen
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>Durch den MvvmManager bindbare View-Property</remarks>
    <Category("Column")>
    Public Property CellPadding As Padding
        Get
            Return _cellPadding
        End Get
        Set(ByVal value As Padding)
            If Not Object.Equals(_cellPadding, value) Then
                _cellPadding = value
                OnCellPaddingChanged(EventArgs.Empty)
            End If
        End Set
    End Property

    Const CellPaddingProperty As String = "CellPadding"

    Public Event CellPaddingChanged As EventHandler

    Protected Overridable Sub OnCellPaddingChanged(e As EventArgs)
        RaiseEvent CellPaddingChanged(Me, e)
    End Sub

    Private _header As String
    ''' <summary>
    ''' Die Spaltenueberschrift
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>Durch den MvvmManager bindbare View-Property</remarks>
    <Category("ColumnHeader")>
    <DisplayName("Text")>
    Public Property Header As String
        Get
            Return _header
        End Get
        Set(ByVal value As String)
            If Not Object.Equals(_header, value) Then
                _header = value
            End If
        End Set
    End Property

    Private _isEnabled As Boolean = True

    ''' <summary>
    ''' Bolleanwert, welcher angibt ob die Zelle/Spalte ReadOnly ist
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Category("Column")>
    Public Property IsEnabled As Boolean
        Get
            Return _isEnabled
        End Get
        Set(ByVal value As Boolean)
            If Not Object.Equals(_isEnabled, value) Then
                _isEnabled = value
                OnIsEnabledChanged(EventArgs.Empty)
            End If
        End Set
    End Property

    Public Event IsEnabledChanged As EventHandler

    Protected Overridable Sub OnIsEnabledChanged(e As EventArgs)
        RaiseEvent IsEnabledChanged(Me, e)
    End Sub

    Public Const IsEnabledProperty As String = "IsEnabled"

    Private _backgroundColor As System.Drawing.Color = System.Drawing.Color.Empty
    ''' <summary>
    ''' Hintergrundfarbe
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>Durch den MvvmManager bindbare View-Property</remarks>
    <Category("Column")>
    Public Property BackgroundColor As System.Drawing.Color
        Get
            Return _backgroundColor
        End Get
        Set(ByVal value As System.Drawing.Color)
            If Not Object.Equals(_backgroundColor, value) Then
                _backgroundColor = value
                OnBackgroundColorChanged(EventArgs.Empty)
            End If
        End Set
    End Property

    Public Const BackgroundColorProperty As String = "BackgroundColor"

    Public Event BackgroundColorChanged As EventHandler

    Protected Overridable Sub OnBackgroundColorChanged(e As EventArgs)
        RaiseEvent BackgroundColorChanged(Me, e)
    End Sub

    Private _foregroundColor As System.Drawing.Color = System.Drawing.Color.Empty
    ''' <summary>
    ''' Vordergrundfarbe
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>Durch den MvvmManager bindbare View-Property</remarks>
    <Category("Column")>
    Public Property ForegroundColor As System.Drawing.Color
        Get
            Return _foregroundColor
        End Get
        Set(ByVal value As System.Drawing.Color)
            If Not Object.Equals(_foregroundColor, value) Then
                _foregroundColor = value
                OnForegroundColorChanged(EventArgs.Empty)
            End If
        End Set
    End Property

    Public Event ForegroundColorChanged As EventHandler

    Public Const ForegroundColorProperty As String = "ForegroundColor"

    Protected Overridable Sub OnForegroundColorChanged(e As EventArgs)
        RaiseEvent ForegroundColorChanged(Me, e)
    End Sub

    Private _horizontalAlignment As System.Windows.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch
    ''' <summary>
    ''' Hor. Ausrichtung
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>Durch den MvvmManager bindbare View-Property</remarks>
    <Category("Column")>
    Public Property HorizontalAlignment As System.Windows.HorizontalAlignment
        Get
            Return _horizontalAlignment
        End Get
        Set(ByVal value As System.Windows.HorizontalAlignment)
            If Not Object.Equals(_horizontalAlignment, value) Then
                _horizontalAlignment = value
                OnHorizontalAlignmentChanged(EventArgs.Empty)
            End If
        End Set
    End Property

    Public Const HorizontalAlignmentProperty As String = "HorizontalAlignment"

    Public Event HorizontalAlignmentChanged As EventHandler

    Protected Overridable Sub OnHorizontalAlignmentChanged(e As EventArgs)
        RaiseEvent HorizontalAlignmentChanged(Me, e)
    End Sub

    Private _verticalAlignment As VerticalAlignment
    ''' <summary>
    ''' Ver. Ausrichtung
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>Durch den MvvmManager bindbare View-Property</remarks>
    <Category("Column")>
    Public Property VerticalAlignment As VerticalAlignment
        Get
            Return _verticalAlignment
        End Get
        Set(ByVal value As VerticalAlignment)
            If Not Object.Equals(_verticalAlignment, value) Then
                _verticalAlignment = value
                OnVerticalAlignmentChanged(EventArgs.Empty)
            End If
        End Set
    End Property

    Public Const VerticalAlignmentProperty As String = "VerticalAlignment"

    Public Event VerticalAlignmentChanged As EventHandler

    Protected Overridable Sub OnVerticalAlignmentChanged(e As EventArgs)
        RaiseEvent VerticalAlignmentChanged(Me, e)
    End Sub


#End Region

    Private _columnHeaderFont As Font

    ''' <summary>
    ''' Betrifft die Schriftart der Kopfzeile einer Spalte
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Category("ColumnHeader")>
    <DisplayName("Font")>
    Public Property ColumnHeaderFont As Font
        Get
            Return _columnHeaderFont
        End Get
        Set(ByVal value As Font)
            _columnHeaderFont = value
        End Set
    End Property

    Private _columnHeaderPadding As Forms.Padding = Forms.Padding.Empty
    ''' <summary>
    '''  Betrifft das Padding der Kopfzeile einer Spalte
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Category("ColumnHeader")>
    <DisplayName("Padding")>
    Public Property ColumnHeaderPadding As Forms.Padding
        Get
            Return _columnHeaderPadding
        End Get
        Set(ByVal value As Forms.Padding)
            _columnHeaderPadding = value
        End Set
    End Property



    Private _displayMemberPath As String

    ''' <summary>
    ''' Falls Typ eine CBO ist, wird dieser MemberPath verwendet 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Category("ComboBox")>
    Public Property DisplayMemberPath As String
        Get
            Return _displayMemberPath
        End Get
        Set(ByVal value As String)
            _displayMemberPath = value
        End Set
    End Property


    Private _name As String
    ''' <summary>
    ''' Name der Spalte welcher fuer die Serialisierung verwendet wird
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Name As String
        Get
            Return _name
        End Get
        Set(ByVal value As String)
            _name = value
        End Set
    End Property


    Private _columnType As ColumnType
    ''' <summary>
    ''' Der Typ der Spalte
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>Durch den MvvmManager bindbare View-Property</remarks>
    <Category("Column")>
    Public Property ColumnType As ColumnType
        Get
            Return _columnType
        End Get
        Set(ByVal value As ColumnType)
            If Not Object.Equals(_columnType, value) Then
                _columnType = value
            End If
        End Set
    End Property

    Private _font As Font
    ''' <summary>
    ''' Schriftart etc.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>Durch den MvvmManager bindbare View-Property</remarks>
    <Category("Column")>
    Public Property Font As Font
        Get
            Return _font
        End Get
        Set(ByVal value As Font)
            If Not Object.Equals(_font, value) Then
                _font = value
            End If
        End Set
    End Property

    ''' <summary>
    ''' Hier wird die Instanz (falls ein Typ ausgeaehtl wurde) fuer die Columnerweiterung hinterlegt
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Browsable(False)>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    <Category("Column")>
    Friend Property ColumnTemplateExtender As IMvvmColumnTemplateExtender

    <Browsable(False)>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    <Category("Column")>
    Friend Property BoundPropertyInfo As PropertyInfo

    <Browsable(False)>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    <Category("Column")>
    Friend Property FilterConverterInstance As IValueConverter

    <Browsable(False)>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    <Category("Column")>
    Friend Property IsFilteringEnabled As Boolean

    <Browsable(False)>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    <Category("Column")>
    Friend Property FilterButton As Controls.Button

    <Browsable(False)>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    <Category("Column")>
    Friend Property FilterTextBox As Controls.TextBox

    Private Function ShouldSerializeColumnTemplateExtender() As Boolean
        Return False
    End Function

    ''' <summary>
    ''' Serialisiert die aktuelle Columninstanz in CodeStatements und fuegt diese direkt der CodeStatementCollection hinzu
    ''' </summary>
    ''' <param name="columnName">Name der Spalte welche fuer die Serialisierung verwendet werden soll</param>
    ''' <param name="statements">CodeStatementCollection fuer den serialisierten Code</param>
    ''' <remarks></remarks>
    Public Sub AddSerializedColumn(columnName As String, statements As CodeStatementCollection)

        'Erst ggf.  Extender serialisieren:
        If Me.ColumnTemplateExtender IsNot Nothing Then
            Dim extenderName = columnName & "ColumnTemplateExtenderInstance"
            statements.Add(New CodeVariableDeclarationStatement(Me.ColumnTemplateExtender.GetType(), extenderName, New CodeObjectCreateExpression(Me.ColumnTemplateExtender.GetType())))

            'Extender initialisieren:
            For Each prop In Me.ColumnTemplateExtender.GetType.GetProperties()
                Dim assignStatement = GetPropertyStatement(prop, extenderName, Me.ColumnTemplateExtender)
                If assignStatement IsNot Nothing Then
                    statements.Add(assignStatement)
                End If
            Next
        End If

        'Nun die PropertyBindings serialisieren:
        For Each item In Me.PropertyCellBindings
            Dim addParams As New List(Of CodeExpression)

            'BindingSettings-Objekt erstellen
            Dim bindSetting = New CodeObjectCreateExpression(GetType(BindingSetting),
                                                                    New CodeExpression() {New CodePropertyReferenceExpression(New CodeVariableReferenceExpression("ActiveDevelop.EntitiesFormsLib.MvvmBindingModes"), item.BindingSetting.BindingMode.ToString()),
                                                                                          New CodePropertyReferenceExpression(New CodeVariableReferenceExpression("ActiveDevelop.EntitiesFormsLib.UpdateSourceTriggerSettings"), item.BindingSetting.UpdateSourceTrigger.ToString())})
            addParams.Add(bindSetting)

            'ControlProperty-Parameter:
            addParams.Add(New CodeObjectCreateExpression(GetType(BindingProperty), {New CodePrimitiveExpression(item.ControlProperty.PropertyName),
                                                                                    New CodeTypeOfExpression(item.ControlProperty.PropertyType)}))

            'Converter-Parameter
            If item.Converter IsNot Nothing Then
                addParams.Add(New CodeTypeOfExpression(item.Converter))
            Else
                addParams.Add(New CodePrimitiveExpression(Nothing))
            End If

            'ConverterParameter-Parameter
            If String.IsNullOrEmpty(item.ConverterParameter) Then
                addParams.Add(New CodePrimitiveExpression(Nothing))
            Else
                addParams.Add(New CodePrimitiveExpression(item.ConverterParameter))
            End If

            'ControlProperty-Parameter:
            addParams.Add(New CodeObjectCreateExpression(GetType(BindingProperty), {New CodePrimitiveExpression(item.ViewModelProperty.PropertyName),
                                                                                    New CodeTypeOfExpression(item.ViewModelProperty.PropertyType)}))

            'PropertyCellBindings:
            Dim AddMethodStatement As New CodeMethodInvokeExpression(New CodeVariableReferenceExpression(columnName & ".PropertyCellBindings"),
                                                                     "Add",
                                                                     addParams.ToArray)
            statements.Add(AddMethodStatement)
        Next


        statements.Add(New CodeVariableDeclarationStatement(GetType(MvvmDataGridColumn), columnName, New CodeObjectCreateExpression(GetType(MvvmDataGridColumn))))

        If Me.ColumnTemplateExtender IsNot Nothing Then
            'und dann zuweisen
            statements.Add(New CodeAssignStatement(New CodeVariableReferenceExpression(columnName & ".ColumnTemplateExtender"), New CodeVariableReferenceExpression(columnName & "ColumnTemplateExtenderInstance")))
        End If

        'Hier der Rest (ColumnProperties)
        For Each prop In GetMyPropertiesForSerialization()
            Dim assignStatement = GetPropertyStatement(prop, columnName, Me)
            If assignStatement IsNot Nothing Then
                statements.Add(assignStatement)
            End If
        Next

    End Sub

    ''' <summary>
    ''' Hier werden die Propertynamen als String angegeben, welche vom CodeSerializer ignoriert werden sollen
    ''' </summary>
    ''' <remarks></remarks>
    Private Shared _myManuelSerializedProperties As List(Of String) = New List(Of String) From {"PropertyCellBindings", "ColumnTemplateExtender", "WpfColumn"}

    ''' <summary>
    ''' Liefert NUR die Properties, welche in dieser Klasse auch definiert wurden (und nicht die in der Basisklasse)
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetMyPropertiesForSerialization() As PropertyInfo()
        Return GetType(MvvmDataGridColumn).GetProperties(BindingFlags.DeclaredOnly Or BindingFlags.Public Or BindingFlags.Instance).Where(Function(prop) Not _myManuelSerializedProperties.Contains(prop.Name)).ToArray
    End Function

    ''' <summary>
    ''' Liefert das CodeAssignStatement, welches eine Property der Column in Code serialisiert
    ''' </summary>
    ''' <param name="prop"></param>
    ''' <param name="varName"></param>
    ''' <param name="host"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetPropertyStatement(prop As PropertyInfo, varName As String, host As Object) As CodeAssignStatement
        Dim value = prop.GetValue(host)

        If prop.PropertyType.IsEnum Then
            Return New CodeAssignStatement(New CodeVariableReferenceExpression(varName & "." & prop.Name), New CodeFieldReferenceExpression(New CodeTypeReferenceExpression(prop.PropertyType.FullName), value.ToString()))
        ElseIf Not prop.PropertyType.IsInterface Then
            If value IsNot Nothing Then
                Dim dsm As New DesignerSerializationManager

                dsm.PreserveNames = True
                dsm.RecycleInstances = False
                dsm.PropertyProvider = value
                dsm.CreateSession()

                Dim cds As CodeDomSerializer = DirectCast(dsm.GetSerializer(dsm.PropertyProvider.GetType, GetType(CodeDomSerializer)), CodeDomSerializer)
                Dim code As CodeExpression = DirectCast(cds.Serialize(dsm, dsm.PropertyProvider), CodeExpression)

                Return New CodeAssignStatement(New CodeVariableReferenceExpression(varName & "." & prop.Name), code)
            End If

            'Return New CodeAssignStatement(New CodeVariableReferenceExpression(varName & "." & prop.Name), New CodePrimitiveExpression(value))
        End If

        Return Nothing
    End Function

    Public Overrides Function ToString() As String
        Return Me.Header
    End Function

    ''' <summary>
    ''' Klont mittels Reflection ICloneable-Interface eine Column-Instanz in eine neue MvvmDataGridColumn
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Friend Function Clone() As MvvmDataGridColumn
        Dim clonedColumn = DirectCast(MyBase.MemberwiseClone(), MvvmDataGridColumn)
        Dim props = Me.GetType.GetRuntimeProperties

        'Hier werden alle Properties, welche einen Referenztyp enthalten der ICloneable implementiert, geklont (dies sind die meisten UI-Typen)
        For Each propItem In props
            Dim value As Object = propItem.GetValue(Me, Nothing)
            Dim clonableValue = TryCast(value, ICloneable)

            If clonableValue IsNot Nothing Then
                propItem.SetValue(clonedColumn, clonableValue.Clone(), Nothing)
            End If

        Next

        'Hier werden die PropertyBindings geklont
        clonedColumn.PropertyCellBindings = Me.PropertyCellBindings.Clone()

        Return clonedColumn
    End Function


End Class

' ''' <summary>
' ''' Da die MvvmDataGridColumn ein Control sein muss (damit das Bindungszuweisungsfenster damit arbeiten kann), sollen hierdurch die Properties des Controls (die auch intern nicht verwendet werden) versteckt werden
' ''' </summary>
' ''' <remarks></remarks>
'Public Class MvvmDataGridColumnControlDesigner
'    Inherits ControlDesigner

'    Protected Overrides Sub PreFilterProperties(properties As IDictionary)
'        MyBase.PreFilterProperties(properties)

'        Dim myProps = MvvmDataGridColumn.GetMyPropertiesForSerialization().Select(Function(prop) prop.Name).ToList
'        Dim propsToRemoved = New List(Of Object)

'        For Each key In properties.Keys
'            If Not myProps.Contains(DirectCast(key, String)) Then
'                propsToRemoved.Add(key)
'            End If
'        Next

'        For Each prop In propsToRemoved
'            properties.Remove(prop)
'        Next
'    End Sub

'End Class