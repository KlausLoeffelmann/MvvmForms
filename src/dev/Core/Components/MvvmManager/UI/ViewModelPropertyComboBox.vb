Imports System.Collections.ObjectModel
Imports System.Windows.Forms

''' <summary>
''' Expansion of NullableValueComboBox with PropertyBinding-Context
''' </summary>
Public Class ViewModelPropertyComboBox
    Inherits NullableValueComboBox

    ''' <summary>
    ''' The current selected Binding-Node
    ''' </summary>
    ''' <returns></returns>
    Public Property SelectedNode As PropertyBindingNodeDefinition
        Get
            Return CType(MyBase.SelectedItem, PropertyBindingNodeDefinition)
        End Get
        Set(ByVal value As PropertyBindingNodeDefinition)
            If value Is Nothing Then
                MyBase.SelectedItem = Nothing
                Return
            End If

            _oldSelectedNode = value
            _oldTextValue = _oldSelectedNode.Binding.PropertyName

            RefreshDataSource(value)

            MyBase.SelectedItem = value
        End Set
    End Property

    Private Sub RefreshDataSource(value As PropertyBindingNodeDefinition)
        If Debugger.IsAttached Then Debugger.Break()
        'loading depth level
        Dim props = value.Binding.PropertyName.Split("."c)
        Dim firstProp As Boolean = True

        'DS-Reset
        Dim newDS = New ObservableCollection(Of PropertyBindingNodeDefinition)(_rootNodes)
        Dim node As PropertyBindingNodeDefinition = Nothing

        If props.Count > 1 Then
            For Each prop In props

                If Not firstProp Then
                    node = node.SubProperties.Where(Function(n) n.PropertyName = prop).Single 'III und b
                Else
                    node = _rootNodes.Where(Function(n) n.PropertyName = prop).Single 'A
                    firstProp = False
                End If

                newDS.AddRange(node.SubProperties)
            Next
        End If

        'All necessary nodes in
        MyBase.ItemSource = newDS

    End Sub

    Private _rootNodes As ObservableCollection(Of PropertyBindingNodeDefinition)

    ''' <summary>
    ''' The DataSource (Flat-ViewModel-Properties) for the Binding-Nodes
    ''' </summary>
    ''' <returns></returns>
    Public Property NodesSource As ObservableCollection(Of PropertyBindingNodeDefinition)
        Get
            Return CType(MyBase.ItemSource, ObservableCollection(Of PropertyBindingNodeDefinition))
        End Get
        Set(ByVal value As ObservableCollection(Of PropertyBindingNodeDefinition))
            If value IsNot Nothing Then
                _rootNodes = value
                MyBase.ItemSource = New ObservableCollection(Of PropertyBindingNodeDefinition)(value)
            End If
        End Set
    End Property

    Private _oldTextValue As String
    Private _oldSelectedNode As PropertyBindingNodeDefinition

    Protected Overrides Sub OnKeyDown(e As KeyEventArgs)
        MyBase.OnKeyDown(e)

        _oldTextValue = MyBase.Text
        _oldSelectedNode = SelectedNode
    End Sub

    Protected Overrides Sub OnTextChanged(e As EventArgs)
        MyBase.OnTextChanged(e)

        If _oldTextValue IsNot Nothing AndAlso _oldSelectedNode IsNot Nothing Then
            'Nur wenn der letzte String ein Node enstpricht
            If Text.Split("."c).Last = _oldSelectedNode.PropertyName OrElse
                Text.Trim("."c).Split("."c).Last = _oldSelectedNode.PropertyName Then
                If _oldTextValue.Last() <> "."c AndAlso MyBase.Text.Last() = "."c Then
                    'Punkt zum Schluss eingegeben, SubProps in Liste hinzufügen
                    If Me._oldSelectedNode.SubProperties IsNot Nothing AndAlso Me._oldSelectedNode.SubProperties.Count > 0 Then
                        Dim src = DirectCast(ItemSource, ObservableCollection(Of PropertyBindingNodeDefinition))
                        src.AddRange(Me._oldSelectedNode.SubProperties)
                        'TODO: wieso muss hier ne neue angelegt werden, mit obs.col sollte dorch das auch gehen?!?!
                        MyBase.ItemSource = New ObservableCollection(Of PropertyBindingNodeDefinition)(src)
                        WpfComboBoxWrapper1.InnerComboBox.IsDropDownOpen = True
                        'WpfComboBoxWrapper1.InnerComboBox.AppendText(NodesSource.First.PropertyName.First)
                        WpfComboBoxWrapper1.InnerComboBox.SetCursorToEnd()
                    End If

                ElseIf _oldTextValue.Last() = "."c AndAlso MyBase.Text.Last() <> "."c Then
                    SelectedNode = Nothing
                End If
            End If
        End If
    End Sub

    Protected Overrides Sub OnInnerSelectedItemChanged(e As Windows.Controls.SelectionChangedEventArgs)
        If SelectedItem IsNot Nothing Then
            If PreviousItem IsNot SelectedItem Then
                OnSelectedItemChanged(e)
                SelectedNode = DirectCast(SelectedItem, PropertyBindingNodeDefinition)
            End If

        ElseIf Not (ValueNotFoundBehavior = ValueNotFoundBehavior.KeepFocus OrElse
                      ValueNotFoundBehavior = ValueNotFoundBehavior.SelectFirst) Then
            'Wenn SelctedItem Nothing ist und ein unbestimmter Wert nicht angegeben werden darf, darf das PropChanged nicht geworfen werden (da der Benutzer gezwungen wird ein validen Wert später beim 
            'Leave auszuwählen bzw automatisch ausgewählt)
            OnSelectedItemChanged(e)
        End If

        PreviousItem = SelectedItem
    End Sub
End Class
