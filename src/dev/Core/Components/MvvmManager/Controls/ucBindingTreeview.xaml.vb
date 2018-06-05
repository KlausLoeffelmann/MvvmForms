Imports System.Collections.ObjectModel
Imports System.ComponentModel
Imports System.Threading.Tasks
Imports System.Windows
Imports System.Windows.Controls
Imports ActiveDevelop.EntitiesFormsLib

Public Class ucBindingTreeview
    Implements INotifyPropertyChanged

    ''' <summary>
    ''' Schluessel zur PropertyBindings-DependencyProperty
    ''' </summary>
    ''' <remarks></remarks>
    Friend Shared ReadOnly PropertyBindingsProperty As DependencyProperty =
                           DependencyProperty.Register(NameOf(PropertyBindings),
                           GetType(PropertyBindings), GetType(ucBindingTreeview))

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>(Dependency Property)</remarks>
    Public Property PropertyBindings As PropertyBindings
        Get
            Return CType(GetValue(PropertyBindingsProperty), PropertyBindings)
        End Get
        Set(ByVal value As PropertyBindings)
            Dim isDifferent = (PropertyBindings IsNot value)
            SetValue(PropertyBindingsProperty, value)
            If isDifferent Then
                Dim tmpPropertyBindingsInternal = New ObservableCollection(Of PropertyBindingItem)()
                If value IsNot Nothing Then
                    For Each item In value
                        tmpPropertyBindingsInternal.Add(item.Clone)
                    Next
                End If
                If _newItem Is Nothing OrElse Not tmpPropertyBindingsInternal.Contains(_newItem) Then
                    _newItem = GetNewItem()

                    tmpPropertyBindingsInternal.Add(_newItem)
                End If
                PropertyBindingsInternal = tmpPropertyBindingsInternal
                lvBinding.ItemsSource = PropertyBindingsInternal
            End If
        End Set
    End Property


    ''' <summary>
    ''' this Member also contains the new-Row which PropertyBindings doesn't have
    ''' </summary>
    Public Property PropertyBindingsInternal As ObservableCollection(Of PropertyBindingItem)


    Friend Shared ReadOnly ControlPropertiesProperty As DependencyProperty =
                           DependencyProperty.Register(NameOf(ControlProperties),
                           GetType(ObservableCollection(Of BindingProperty)), GetType(ucBindingTreeview))

    Public Property ControlProperties As ObservableCollection(Of BindingProperty)
        Get
            Return CType(GetValue(ControlPropertiesProperty), ObservableCollection(Of BindingProperty))
        End Get
        Set(ByVal value As ObservableCollection(Of BindingProperty))
            SetValue(ControlPropertiesProperty, value)
        End Set
    End Property

    Friend Shared ReadOnly ConverterListProperty As DependencyProperty =
                           DependencyProperty.Register(NameOf(ConverterList),
                           GetType(ObservableCollection(Of ConverterDisplayItem)), GetType(ucBindingTreeview))

    Public Property ConverterList As ObservableCollection(Of ConverterDisplayItem)
        Get
            Return CType(GetValue(ConverterListProperty), ObservableCollection(Of ConverterDisplayItem))
        End Get
        Set(ByVal value As ObservableCollection(Of ConverterDisplayItem))
            SetValue(ConverterListProperty, value)
        End Set
    End Property

    Friend Shared ReadOnly ViewModelPropertiesProperty As DependencyProperty =
                           DependencyProperty.Register(NameOf(ViewModelProperties),
                           GetType(ObservableCollection(Of PropertyBindingNodeDefinition)), GetType(ucBindingTreeview))

    Public Property ViewModelProperties As ObservableCollection(Of PropertyBindingNodeDefinition)
        Get
            Return CType(GetValue(ViewModelPropertiesProperty), ObservableCollection(Of PropertyBindingNodeDefinition))
        End Get
        Set(ByVal value As ObservableCollection(Of PropertyBindingNodeDefinition))
            Dim forceReload = (ViewModelPropertiesProperty IsNot value)
            SetValue(ViewModelPropertiesProperty, value)
        End Set
    End Property

    Private _loadingDone As Boolean = False
    Private _newItem As PropertyBindingItem = Nothing
    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

    Private Sub ComboBox_Control_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        If Not _loadingDone Then Return
        CheckNew()
    End Sub

    Private Sub ucPropertySelector_SelectedPropertyChanged(sender As Object, e As RoutedPropertyChangedEventArgs(Of Object))
        If Not _loadingDone Then Return
        CheckNew()
    End Sub

    Private Sub CheckNew()
        If _newItem.ControlProperty IsNot Nothing AndAlso _newItem.ViewModelProperty IsNot Nothing Then
            _newItem = GetNewItem()
            PropertyBindingsInternal.Add(_newItem)
        End If
    End Sub

    Private Function GetNewItem() As PropertyBindingItem
        Return New PropertyBindingItem() With {.ControlProperty = Nothing, .ViewModelProperty = Nothing, .BindingSetting = New ActiveDevelop.EntitiesFormsLib.BindingSetting(ActiveDevelop.EntitiesFormsLib.MvvmBindingModes.TwoWay, UpdateSourceTriggerSettings.PropertyChangedImmediately)}
    End Function

    Private _BindingContextInfo As String = "Control Property"
    Public Property BindingContextInfo As String
        Get
            Return _BindingContextInfo
        End Get
        Set(value As String)
            Dim eq = _BindingContextInfo Is value
            If Not eq Then
                _BindingContextInfo = value
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(BindingContextInfo)))
            End If
        End Set
    End Property


    Private Async Sub UserControl_Loaded(sender As Object, e As RoutedEventArgs)
        _loadingDone = True
        Await Task.Delay(0)
    End Sub

    Public Sub ApplyChanges()
        CheckNew()        ' ensure a valid binding will be serialized. if we get e new row, it will be filtered out
        Dim tmp = PropertyBindingsInternal.ToList
        If tmp.Contains(_newItem) Then tmp.Remove(_newItem)
        PropertyBindings.Clear()
        PropertyBindings.AddRange(tmp)
    End Sub

    Public Sub DeleteBindings(items2del() As PropertyBindingItem)
        For Each row In items2del
            PropertyBindingsInternal.Remove(row)
        Next
        If _newItem Is Nothing OrElse Not PropertyBindingsInternal.Contains(_newItem) Then
            _newItem = GetNewItem()

            PropertyBindingsInternal.Add(_newItem)
        End If
    End Sub

End Class
