Imports System.Collections.Specialized
Imports System.ComponentModel
Imports System.Drawing
Imports System.Reflection
Imports System.Windows.Forms
Imports ActiveDevelop.EntitiesFormsLib
Imports Microsoft.Reporting
Imports winforms = System.Windows.Forms

''' <summary>
''' TreeView welche eine Liste für die Erstellung des Knotenbaums (auf Basis einer Objektstruktur) zur Bindung bereitstellt
''' und zur Laufzeit eine Aktualisierung des aktuell ausgewählten Knotenelements zur Verfüfung stellt
''' </summary>
''' <remarks></remarks>
<ToolboxBitmap(GetType(winforms.TreeView)),
 ToolboxItem(True)>
Public Class BindableTreeView
    Inherits TreeView

    Private _dataSource As IEnumerable
    ''' <summary>
    ''' Die Quelle welche als Knoten abgebildet werden soll
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>Durch den MvvmManager bindbare View-Property</remarks>
    Public Property DataSource As IEnumerable
        Get
            Return _dataSource
        End Get
        Set(ByVal value As IEnumerable)
            If Not Object.Equals(_dataSource, value) Then
                Dim notifyList As INotifyCollectionChanged

                If _dataSource IsNot Nothing Then
                    notifyList = TryCast(_dataSource, INotifyCollectionChanged)

                    RemoveHandler notifyList.CollectionChanged, AddressOf DataSource_CollectionChanged
                End If

                If value IsNot Nothing Then
                    notifyList = TryCast(value, INotifyCollectionChanged)

                    AddHandler notifyList.CollectionChanged, AddressOf DataSource_CollectionChanged
                End If

                _dataSource = value

                RefreshTree()

                If Not LazyLoading Then
                    MyBase.ExpandAll()
                End If

                OnDataSourceChanged(EventArgs.Empty)
            End If
        End Set
    End Property

    Public Event DataSourceChanged As EventHandler

    Protected Overridable Sub OnDataSourceChanged(e As EventArgs)
        RaiseEvent DataSourceChanged(Me, e)
    End Sub
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList

    Private _selectedItem As Object
    ''' <summary>
    ''' Das Element welches in der TreeView ausgewählt wurde
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>Durch den MvvmManager bindbare View-Property</remarks>
    Public Property SelectedItem As Object
        Get
            Return _selectedItem
        End Get
        Set(ByVal value As Object)
            Dim oldVal = _selectedItem
            If Not Object.Equals(_selectedItem, value) Then
                Dim item = _nodes(value)
                _selectedItem = item.DataItem

                OnSelectedItemChanged(EventArgs.Empty)

                If value IsNot Nothing Then
                    SelectedNode = item
                Else
                    SelectedNode = Nothing
                End If

            End If
        End Set
    End Property

    Public Event SelectedItemChanged As EventHandler

    Protected Overridable Sub OnSelectedItemChanged(e As EventArgs)
        RaiseEvent SelectedItemChanged(Me, e)
    End Sub

    Private _selectedRootItem As Object
    ''' <summary>
    ''' Das Element welches in der TreeView in erster Ebene ausgewählt wurde
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>Durch den MvvmManager bindbare View-Property</remarks>
    Public Property SelectedRootItem As Object
        Get
            Return _selectedRootItem
        End Get
        Set(ByVal value As Object)
            Dim oldVal = _selectedRootItem
            If Not Object.Equals(_selectedRootItem, value) Then
                _selectedRootItem = value

                OnSelectedRootItemChanged(EventArgs.Empty)
                If Not _isSelecting Then
                    If value IsNot Nothing Then
                        SelectedNode = _nodes(value)
                    Else
                        SelectedNode = Nothing
                    End If
                End If

            End If
        End Set
    End Property

    Public Event SelectedRootItemChanged As EventHandler

    Protected Overridable Sub OnSelectedRootItemChanged(e As EventArgs)
        RaiseEvent SelectedRootItemChanged(Me, e)
    End Sub

    Protected _nodes As New NodesDictionary(Of Object)

    ''' <summary>
    ''' Erstellt den Baum anhand der DataSource neu
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub RefreshTree()

        For Each list In _notifyLists.GetLists()
            For Each node In _notifyLists.GetTreeNodes(list)
                RemoveAllChilds(node)
            Next
        Next

        Nodes.Clear()
        _nodes.Clear()

        For Each item In DataSource
            Dim node = CreateNode(item, 0, Not LazyLoading)

            Nodes.Add(node)
            _nodes.Add(item, node)
        Next
    End Sub

    ''' <summary>
    ''' Erstellt einen neues Knotenobjekt
    ''' </summary>
    ''' <param name="item"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Overridable Function CreateNode(item As Object, level As Integer, setSubItemsAsLoaded As Boolean) As DataTreeNode
        Dim node = New DataTreeNode() With {.DataItem = item, .IsLoaded = True}

        FillNode(node, item, level, setSubItemsAsLoaded)

        Return node
    End Function

    Protected Overridable Sub FillNode(node As DataTreeNode, item As Object, level As Integer, setSubItemsAsLoaded As Boolean)
        'Displaytext finden
        node.Text = GetDisplayMember(item, level)

        'Schauen ob der Node Subnodes hat:
        Dim subItems = GetChilds(item, level)
        If subItems IsNot Nothing Then
            'Subitems vorhanden
            For Each subItem In subItems
                Dim newNode As DataTreeNode
                If LazyLoading Then
                    If Not ChildMemberPath.Contains("\") AndAlso Not String.IsNullOrWhiteSpace(ChildMemberPath) AndAlso subItem.GetType = item.GetType Then
                        'Endlos-Rekursion
                        newNode = New DataTreeNode() With {.DataItem = subItem, .Text = GetDisplayMember(subItem, level)}
                    Else
                        newNode = New DataTreeNode() With {.DataItem = subItem, .Text = GetDisplayMember(subItem, level + 1)}
                    End If
                Else
                    'Rekursiv laden
                    newNode = CreateNode(subItem, level + 1, setSubItemsAsLoaded)
                End If
                newNode.IsLoaded = setSubItemsAsLoaded
                node.Nodes.Add(newNode)
                _nodes.Add(subItem, newNode)
            Next

            node.IsLoaded = True

            AddNotifyCollectionChangedHandler(subItems, node)
        End If
    End Sub

    Private Sub LoadNode(node As DataTreeNode, maxLvl As Integer)

        If Not LazyLoading OrElse node.Level = maxLvl Then
            Return
        End If

        'schauen ob der knoten schon geladen wurde und in der Verwaltung liegt
        If node.IsLoaded Then
            For Each subNode As DataTreeNode In node.Nodes
                LoadNode(subNode, maxLvl)
            Next
        Else
            Dim currentLevel = node.Level

            If Not ChildMemberPath.Contains("\") AndAlso Not String.IsNullOrWhiteSpace(ChildMemberPath) AndAlso node.DataItem.GetType = node.DataItem.GetType Then
                'Bei Endlosmodus auf gleicher Ebene bleiben
                currentLevel = 0
            End If
            FillNode(node, node.DataItem, currentLevel, False)
            node.IsLoaded = True

            LoadNode(node, maxLvl)
        End If
    End Sub

    Private Sub AddNotifyCollectionChangedHandler(subItems As IEnumerable, node As DataTreeNode)
        If GetType(INotifyCollectionChanged).IsAssignableFrom(subItems.GetType) Then
            'Liste Schmeisst Event
            Dim notifyList = DirectCast(subItems, INotifyCollectionChanged)

            Windows.WeakEventManager(Of INotifyCollectionChanged, NotifyCollectionChangedEventArgs).AddHandler(
                   notifyList, "CollectionChanged", AddressOf DataSource_CollectionChanged)
            _notifyLists.Add(subItems, node)
        End If
    End Sub

    Private _notifyLists As New NodesListDictionary

    Protected Overrides Sub OnAfterExpand(e As TreeViewEventArgs)
        MyBase.OnAfterExpand(e)
        Dim node = DirectCast(e.Node, DataTreeNode)

        If LazyLoading Then
            For Each subNode As DataTreeNode In node.Nodes
                If Not subNode.IsLoaded Then
                    If subNode.Nodes.Count > 0 Then
                        Debugger.Break()
                    End If

                    Dim currentLevel = subNode.Level

                    If Not ChildMemberPath.Contains("\") AndAlso Not String.IsNullOrWhiteSpace(ChildMemberPath) AndAlso subNode.DataItem.GetType = node.DataItem.GetType Then
                        'Bei Endlosmodus auf gleicher Ebene bleiben
                        currentLevel = 0
                    End If

                    FillNode(subNode, subNode.DataItem, currentLevel, False)
                End If
            Next

        End If

        node.IsLoaded = True

    End Sub

    Private _isSelecting As Boolean = False

    ''' <summary>
    ''' Selecteditem synchronisieren
    ''' </summary>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Overrides Sub OnAfterSelect(e As TreeViewEventArgs)
        MyBase.OnAfterSelect(e)

        Dim node = DirectCast(e.Node, DataTreeNode)

        _isSelecting = True
        Try
            SelectedItem = node.DataItem
            SelectedRootItem = GetRootNode(node).DataItem
        Finally
            _isSelecting = False
        End Try
    End Sub

    Private Function GetRootNode(node As DataTreeNode) As DataTreeNode
        If node.Parent Is Nothing Then
            Return node
        Else
            Return GetRootNode(DirectCast(node.Parent, DataTreeNode))
        End If
    End Function

    Private Sub RemoveAllChilds(node As DataTreeNode)
        Dim nodeList = GetChilds(node.DataItem, node.Level)
        If nodeList IsNot Nothing Then
            Windows.WeakEventManager(Of INotifyCollectionChanged, NotifyCollectionChangedEventArgs).RemoveHandler(
                       DirectCast(nodeList, INotifyCollectionChanged), "CollectionChanged", AddressOf DataSource_CollectionChanged)
            _notifyLists.Remove(nodeList, node)

            If node.Nodes Is Nothing Then
                Return
            Else
                For Each subNode As DataTreeNode In node.Nodes
                    Dim list = GetChilds(subNode.DataItem, subNode.Level)

                    If list IsNot Nothing AndAlso _notifyLists.ContainsKey(list) Then
                        RemoveAllChilds(subNode)

                        Windows.WeakEventManager(Of INotifyCollectionChanged, NotifyCollectionChangedEventArgs).RemoveHandler(
                           DirectCast(list, INotifyCollectionChanged), "CollectionChanged", AddressOf DataSource_CollectionChanged)

                        If _notifyLists.ContainsKey(list) Then
                            _notifyLists.Remove(list, subNode)
                        End If
                    End If
                Next
            End If
        End If
    End Sub

    Private components As System.ComponentModel.IContainer

    Private Sub DataSource_CollectionChanged(sender As Object, e As NotifyCollectionChangedEventArgs)

        Select Case e.Action
            Case NotifyCollectionChangedAction.Add

                For Each newItem In e.NewItems
                    Dim newNode As DataTreeNode = Nothing
                    If sender Is DataSource Then
                        'root
                        newNode = CreateNode(newItem, 0, True)
                        Nodes.Add(newNode)
                        newNode.IsLoaded = True
                    Else
                        'subnode
                        Dim subNodes = _notifyLists.GetTreeNodes(DirectCast(sender, IEnumerable))
                        For Each subnode In subNodes
                            newNode = CreateNode(newItem, subnode.Level + 1, True)

                            subnode.Nodes.Add(newNode)
                            newNode.IsLoaded = True
                        Next
                    End If

                    _nodes.Add(newItem, newNode)
                Next

            Case NotifyCollectionChangedAction.Remove
                For Each oldItem In e.OldItems
                    Dim nodes = _notifyLists.GetTreeNodes(DirectCast(sender, IEnumerable))
                    For Each rootNode In nodes
                        Dim oldNodes As New List(Of DataTreeNode)()

                        For Each subNode In rootNode.Nodes
                            Dim dataNode = DirectCast(subNode, DataTreeNode)
                            If dataNode.DataItem Is oldItem Then
                                oldNodes.Add(dataNode)
                            End If
                        Next

                        For Each node In oldNodes
                            RemoveAllChilds(node)
                            node.Remove()
                        Next
                    Next

                    _nodes.Remove(oldItem)
                Next

            Case Else
                RefreshTree()
        End Select
    End Sub

    Private _displayMemberPath As String = Nothing
    Public Property DisplayMemberPath As String
        Get
            Return _displayMemberPath
        End Get
        Set(ByVal value As String)
            _displayMemberPath = value
        End Set
    End Property

    Private _childMemberPath As String = Nothing
    Public Property ChildMemberPath As String
        Get
            Return _childMemberPath
        End Get
        Set(ByVal value As String)
            _childMemberPath = value
        End Set
    End Property

    Private _displayProps As New Dictionary(Of Integer, PropertyInfo)

    Protected Function GetDisplayMember(nodeItem As Object, level As Integer) As String
        If String.IsNullOrWhiteSpace(DisplayMemberPath) Then
            'ToString auswerten
            Return nodeItem.ToString()
        Else
            'DisplayMemberPath verwenden (Wenn nicht gefunden oder nicht gültig weil nicht richtiger Typ: Ex werfen)
            If Not _displayProps.ContainsKey(level) Then
                Dim displayMembers = DisplayMemberPath.Split("/"c)

                _displayProps.Add(level, nodeItem.GetType().GetProperties().Where(Function(prop) prop.Name = displayMembers(level) AndAlso prop.PropertyType = GetType(String)).Single)
            End If

            If _displayProps.ContainsKey(level) Then
                Return DirectCast(_displayProps(level).GetValue(nodeItem), String)

            Else
                Return nodeItem.ToString()
            End If
        End If
    End Function

    ''' <summary>
    ''' Level/Property
    ''' </summary>
    Private _childProps As New Dictionary(Of Integer, PropertyInfo)

    Protected Function GetChilds(nodeItem As Object, level As Integer) As IEnumerable
        If Not String.IsNullOrWhiteSpace(ChildMemberPath) Then
            'DisplayMemberPath verwenden (Wenn nicht gefunden oder nicht gültig weil nicht richtiger Typ: Ex werfen)
            Dim childMembers = ChildMemberPath.Split("/"c)

            'Prüfen ob letzte Ebene
            If childMembers.Count = level Then
                Return Nothing
            End If

            If Not _childProps.ContainsKey(level) Then

                _childProps.Add(level, nodeItem.GetType().GetProperties().Where(Function(prop) prop.Name = childMembers(level) AndAlso GetType(IEnumerable).IsAssignableFrom(prop.PropertyType)).Single)
            End If


            Return DirectCast(_childProps(level).GetValue(nodeItem), IEnumerable)
        Else
            Return Nothing
        End If
    End Function

    Private _lazyLoading As Boolean = False
    Public Property LazyLoading As Boolean
        Get
            Return _lazyLoading
        End Get
        Set(ByVal value As Boolean)
            _lazyLoading = value
        End Set
    End Property

    Public Class NodesDictionary(Of T)
        Inherits Dictionary(Of T, DataTreeNode)

        Overloads Sub Add(key As T, item As DataTreeNode)
            If Not ContainsKey(key) Then
                MyBase.Add(key, item)
            End If
        End Sub
    End Class

    Public Class NodesListDictionary
        Private _dictionary As New Dictionary(Of IEnumerable, DataTreeNode)()

        Private _additionalDictionary As New Dictionary(Of IEnumerable, List(Of DataTreeNode))()

        Sub Add(key As IEnumerable, item As DataTreeNode)
            If Not _dictionary.ContainsKey(key) Then
                _dictionary.Add(key, item)
            Else
                'Ist schon vorhanden-> in zusätzlicher Liste abspeichern
                If _additionalDictionary.ContainsKey(key) Then
                    _additionalDictionary(key).Add(item)
                Else
                    _additionalDictionary.Add(key, New List(Of DataTreeNode)() From {item})
                End If
            End If
        End Sub

        Function GetTreeNodes(key As IEnumerable) As IEnumerable(Of DataTreeNode)
            Dim nodes = New List(Of DataTreeNode)() From {_dictionary(key)}

            If _additionalDictionary.ContainsKey(key) Then
                nodes.AddRange(_additionalDictionary(key))
            End If

            Return nodes
        End Function

        Function GetLists() As IEnumerable(Of IEnumerable)
            Return _dictionary.Keys
        End Function

        Friend Sub Remove(nodeList As IEnumerable, node As DataTreeNode)
            If _dictionary(nodeList) Is node Then
                _dictionary.Remove(nodeList)
            Else
                _additionalDictionary(nodeList).Remove(node)
                If _additionalDictionary(nodeList).Count = 0 Then
                    _additionalDictionary.Remove(nodeList)
                End If
            End If
        End Sub

        Friend Function ContainsKey(list As IEnumerable) As Boolean
            Return _dictionary.ContainsKey(list)
        End Function
    End Class

    Private _loadedLevels As Integer? = Nothing

    Public Sub LoadLevels(lvlCount As Integer)
        If (Not _loadedLevels.HasValue) OrElse lvlCount > _loadedLevels.Value Then
            SuspendLayout()
            For Each node As DataTreeNode In Nodes
                LoadNode(node, lvlCount)
            Next
            _loadedLevels = lvlCount
            ResumeLayout()
        End If
    End Sub
End Class

''' <summary>
''' TreeNode welcher das entspechende Dataitem in einer Property speichert
''' </summary>
''' <remarks></remarks>
Public Class DataTreeNode
    Inherits TreeNode

    ''' <summary>
    ''' Das Item im Data-Backend
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Property DataItem As Object

    ''' <summary>
    ''' Flag welches den Knoten als geladen kennzeichnet
    ''' </summary>
    ''' <returns></returns>
    Property IsLoaded As Boolean = False
End Class