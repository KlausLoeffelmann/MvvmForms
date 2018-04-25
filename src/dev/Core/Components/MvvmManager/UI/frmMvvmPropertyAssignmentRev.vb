'*****************************************************************************************
'                              frmMvvmPropertyAssignmentEx.vb
'                    =======================================================
'
'          Part of MvvmForms - The Component Library for bringing the Model-View-Viewmodel
'                              pattern to Data Centric Windows Forms Apps in an easy,
'                              feasible and XAML-compatible way.
'
'
'    This designer code is proprtiety code. A licence can be obtained - CONTACT INFO, see below.
'    Permission is granted, to use the designer code (in terms of running it for developing purposes)
'    to maintain Open Source Projects according to the OSI (opensource.org). For maintaining 
'    designer code in commercial (propriety) projects, a licence must be obtained.
'
'                       ActiveDevelop
'                       Bremer Str. 4
'                       Lippstadt, DE-59555
'                       Germany
'                       email: mvvmforms at activedevelop . de. 
'*****************************************************************************************

Imports System.Windows.Forms
Imports System.Collections.ObjectModel
Imports System.Threading.Tasks
Imports System.ComponentModel
Imports System.ComponentModel.Design
Imports System.Windows.Data
Imports System.Runtime.CompilerServices
Imports System.Windows.Controls

Public Class frmMvvmPropertyAssignmentRev

    Private myShowTestdata As Boolean = False
    Public Function ShowDialogWithTestdata(vmType As Type) As DialogResult
        Me.myShowTestdata = True
        Dim dta = New PropertyBindings From {
New PropertyBindingItem() With {.ControlProperty = New ActiveDevelop.EntitiesFormsLib.BindingProperty("Text", GetType(String)), .ViewModelProperty = New ActiveDevelop.EntitiesFormsLib.BindingProperty("Title", GetType(String)), .BindingSetting = New ActiveDevelop.EntitiesFormsLib.BindingSetting(ActiveDevelop.EntitiesFormsLib.MvvmBindingModes.TwoWay, UpdateSourceTrigger.PropertyChanged)},
    New PropertyBindingItem() With {.ControlProperty = New ActiveDevelop.EntitiesFormsLib.BindingProperty("TabIndex", GetType(Integer)), .ViewModelProperty = New ActiveDevelop.EntitiesFormsLib.BindingProperty("AShortVal", GetType(Short)), .BindingSetting = New ActiveDevelop.EntitiesFormsLib.BindingSetting(ActiveDevelop.EntitiesFormsLib.MvvmBindingModes.OneTime, UpdateSourceTrigger.PropertyChanged), .Converter = GetType(ShortToIntConverter)},
    New PropertyBindingItem() With {.ControlProperty = New ActiveDevelop.EntitiesFormsLib.BindingProperty("Enabled", GetType(Boolean)), .ViewModelProperty = New ActiveDevelop.EntitiesFormsLib.BindingProperty("Changeable", GetType(Boolean)), .BindingSetting = New ActiveDevelop.EntitiesFormsLib.BindingSetting(ActiveDevelop.EntitiesFormsLib.MvvmBindingModes.OneWay, UpdateSourceTrigger.PropertyChanged)}
}
        Me.PropertyBindings = dta
        Me.MvvmManager = New MvvmManager
        Me.MvvmManager.DataContextType = vmType
        myControlToBind = New Form
        Return MyBase.ShowDialog
    End Function

    Private myViewModelType As Type
    Private myControlToBind As Object

    Private myControlProperties As ObservableCollection(Of BindingProperty)
    Private myFlatViewModelProperties As ObservableCollection(Of BindingProperty)
    Private myTreeViewViewModelProperties As ObservableCollection(Of PropertyBindingNodeDefinition)

    Private myPropertyBindings As ObservableBindingList(Of PropertyBindingItem)
    Private myConverters As ObservableCollection(Of ConverterDisplayItem)

    Private myEventHandlerHaveBeenWired As Boolean
    Private myIsDirty As Boolean

    Property PropertyBindings As PropertyBindings
        Get
            Return New PropertyBindings(myPropertyBindings)
        End Get
        Set(value As PropertyBindings)
            If Not Object.Equals(value, myPropertyBindings) Then
                myPropertyBindings = value.ToObservableItemList
            End If
        End Set
    End Property

    Property AfterAddEditOrDeleteCallbackAction As Action

    Private WithEvents mySelectionListbox As System.Windows.Controls.ListBox

    Protected Overrides Async Sub OnLoad(e As EventArgs)
        mySelectionListbox = UcBindingTreeview1.lvBinding
        MyBase.OnLoad(e)
        PopulateDisplayValues()

        'Await Task.Delay(5000)
        'myTreeViewViewModelProperties(0).SubProperties2.Add(New PropertyBindingNodeDefinition() With {.PropertyName = "Blub", .Binding = New BindingProperty() With {.PropertyName = "bla.Blub", .PropertyType = GetType(String)}})


        UcBindingTreeview1.ControlProperties = myControlProperties
        UcBindingTreeview1.ViewModelProperties = myTreeViewViewModelProperties
        UcBindingTreeview1.PropertyBindings = Me.PropertyBindings
        UcBindingTreeview1.ConverterList = Me.myConverters
        Await Task.Delay(0)
        Me.KeyPreview = True
    End Sub

    Public Sub PopulateDisplayValues()

        myViewModelType = If(Me.MvvmManager Is Nothing, Nothing, Me.MvvmManager.DataContextType)

        Dim viewName As String = myControlToBind.ToString()
        Dim control = TryCast(myControlToBind, System.Windows.Forms.Control)
        Dim viewType As String = "view"

        If control IsNot Nothing Then
            viewName = control.Name
            viewType = "control"
        End If

        If TypeOf myControlToBind Is MvvmDataGridColumn Then
            viewType = "column"
            'und das Label setzen (ist jetzt keine controlprop, sondern eine columnprop):
            UcBindingTreeview1.BindingContextInfo = "Cell Property"
        End If

        lblCurrentControl.Text = If(myControlToBind Is Nothing, "Control", viewName)
        lblCurrentControlType.Text = If(myControlToBind Is Nothing, "(not set)", myControlToBind.GetType.Name & " " & viewType)

        If Me.EmbeddedMode Then
            Me.btnOK.Visible = False
        Else
            Me.btnOK.Visible = True
            If myViewModelType Is Nothing Then
                'Me.btnOK.Enabled = False
                Exit Sub
            End If
        End If

        lblCurrentViewModelType.Text = If(myViewModelType Is Nothing, "ViewModel Type", myViewModelType.Name)
        lblCurrentViewModelFullName.Text = If(myViewModelType Is Nothing, "(not set)", myViewModelType.FullName)

        InitializeConvertersSyncWorker()
        InitializeViewModelProperties()
        InitializeControlProperties()

        CurrentDataGridRowChanged(Nothing, EventArgs.Empty)
    End Sub

    Private Sub CurrentDataGridRowChanged(sender As Object, eventargs As EventArgs)
    End Sub

    Public Sub DeletePropertyItemHandler(sender As Object, e As EventArgs)
    End Sub

    Public Sub AddOrChangePropertyItemHandler(sender As Object, e As EventArgs)
    End Sub

    Private Sub InitializeConvertersSyncWorker()

        Dim messageBoxContent As String = ""

        Try
            Dim tmpConverterlist As New List(Of ConverterDisplayItem)
            Dim convs = TypeDiscoveryService?.GetTypes(GetType(IValueConverter), False)
            If convs Is Nothing AndAlso myShowTestdata Then
                convs = Me.GetType.Assembly.GetTypes().Where(Function(t) GetType(IValueConverter).IsAssignableFrom(t)).ToList
            End If

            For Each item In convs
                Dim typeItem = DirectCast(item, System.Type)
                Dim tmpConverterDisplayItem = New ConverterDisplayItem With {
                                    .ConverterName = typeItem.Name,
                                    .ConverterAssembly = typeItem.Assembly.ToString,
                                    .ConverterType = typeItem}
                Dim addIt = Not typeItem.IsInterface AndAlso Not typeItem.IsAbstract
                If addIt Then
                    addIt = Not typeItem.FullName.StartsWith("MS.Internal.")
                End If

                If addIt Then
                    tmpConverterlist.Add(tmpConverterDisplayItem)
                End If
            Next

            myConverters = New ObservableCollection(Of ConverterDisplayItem)((
                    From retListItems In tmpConverterlist Order By retListItems.ConverterName).ToList)
        Catch ex As Exception
            MessageBox.Show("Error in Handling:" & vbNewLine & ex.Message & vbNewLine & vbNewLine & messageBoxContent, "Not wanted:", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End Try

    End Sub

    Private Sub InitializeControlProperties()
        If myControlToBind Is Nothing Then
            myControlProperties = New ObservableCollection(Of BindingProperty)
            Return
        End If

        Try
            myControlProperties = New ObservableCollection(Of BindingProperty)(
                (From methodItem In myControlToBind.GetType.GetMethods(Reflection.BindingFlags.NonPublic Or
                                                                     Reflection.BindingFlags.Public Or
                                                                     Reflection.BindingFlags.Instance)
                 Where methodItem.Name.StartsWith("On") And methodItem.Name.EndsWith("Changed")
                 Join propItem In myControlToBind.GetType.GetProperties(Reflection.BindingFlags.NonPublic Or
                                                                    Reflection.BindingFlags.Public Or
                                                                    Reflection.BindingFlags.Instance) On
                            methodItem.Name.Substring(2, methodItem.Name.Length - 9).ToUpper Equals propItem.Name.ToUpper
                 Order By propItem.Name
                 Select New BindingProperty With
                {.PropertyName = propItem.Name,
                 .PropertyType = propItem.PropertyType}))
        Catch ex As Exception
            MessageBox.Show("Error in Handling:" & vbNewLine & ex.Message &
                            vbNewLine & vbNewLine & ex.StackTrace, "Not wanted:",
                            MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End Try
    End Sub

    Private Sub InitializeViewModelProperties()
        'If Debugger.IsAttached Then
        '    Debugger.Break()
        'End If

        If myViewModelType Is Nothing Then
            myFlatViewModelProperties = New ObservableCollection(Of BindingProperty)
            Return
        End If

        Try
            Dim attList = From attItem In myViewModelType.GetCustomAttributes(True)
                          Where GetType(BusinessClassAttribute).IsAssignableFrom(attItem.GetType) Or
                          GetType(MvvmViewModelAttribute).IsAssignableFrom(attItem.GetType)

            If attList IsNot Nothing Then

                Dim businessClassAtt = (From bcaItem In attList Where GetType(BusinessClassAttribute).IsAssignableFrom(bcaItem.GetType)).SingleOrDefault

                'Legacy-Info (DE-Projects only): Das ViewModel-Attribute brauchen wir in diesem Kontext eigentlich nicht mehr.
                'Bis Februar 2014 war es erforderlich, ein ViewModel mit dem MvvmViewModel-Attribute auszuzeichnen,
                'dass es in der Typauflistung erschien - nunmehr reicht es, wenn eine Klasse INotifyPropertyChanged implementiert.
                'Deswegen ist es hier aber ebenfalls notwendig, eine entsprechende Änderung zu machen, sodass, wenn keine 
                'Attribut-Auszeichnung über der Klasse steht, an dieser Stelle die Klasse als ViewModel incl. aller ihrer
                'Eigenschaften berücksichtigt wird.
                Dim viemModelAtt = (From bcaItem In attList Where GetType(MvvmViewModelAttribute).IsAssignableFrom(bcaItem.GetType)).SingleOrDefault

                If (businessClassAtt IsNot Nothing AndAlso
                    DirectCast(businessClassAtt, BusinessClassAttribute).Options.
                        HasFlag(BusinessClassAttributeOptions.IncludeAllPropertiesByDefault)) Or
                            businessClassAtt Is Nothing Then

                    'Include bei Default.
                    Dim propertiesList = New List(Of PropertyCheckBoxItemController)
                    ReflectionHelper.CreateFlatSubPropAsList(myViewModelType, "", propertiesList, True)

                    myFlatViewModelProperties = New ObservableCollection(Of BindingProperty)(
                        From pItem In propertiesList
                        Order By pItem.PropertyFullname
                        Select New BindingProperty With
                        {.PropertyName = pItem.PropertyFullname,
                         .PropertyType = pItem.PropertyType})

                ElseIf (businessClassAtt IsNot Nothing) AndAlso
                    (DirectCast(businessClassAtt, BusinessClassAttribute).Options.HasFlag(BusinessClassAttributeOptions.ExcludeAllPropertiesByDefault)) AndAlso viemModelAtt Is Nothing Then

                    'Exclude bei Default
                    Dim propertiesList = New List(Of PropertyCheckBoxItemController)
                    ReflectionHelper.CreateFlatSubPropAsList(myViewModelType, "", propertiesList, True)

                    myFlatViewModelProperties = New ObservableCollection(Of BindingProperty)(
                        From pItem In propertiesList
                        Order By pItem.PropertyFullname
                        Select New BindingProperty With
                        {.PropertyName = pItem.PropertyFullname,
                         .PropertyType = pItem.PropertyType})
                End If

                myTreeViewViewModelProperties = New ObservableCollection(Of PropertyBindingNodeDefinition)()

                'Create List for TreeView
                For Each binding In myFlatViewModelProperties
                    myTreeViewViewModelProperties.Add(New PropertyBindingNodeDefinition() With {.Binding = binding, .PropertyName = binding.PropertyName})
                Next
            End If
        Catch ex As Exception
            MessageBox.Show("Error in Handling:" & vbNewLine & ex.Message & vbNewLine & vbNewLine & ex.StackTrace, "Not wanted:", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End Try
    End Sub

    ''' Hier wird das Control gespeichert, welches von diesem Formular als Bindungselement verwendet wird.
    Friend Property ControlToBind As Object
        Get
            Return myControlToBind
        End Get
        Set(value As Object)
            If Not Object.Equals(myControlToBind, value) Then
                myControlToBind = value
            End If
            OnControlToAssignChanged()
        End Set
    End Property

    Protected Overridable Sub OnControlToAssignChanged()
        PopulateDisplayValues()
    End Sub

    Friend Property MvvmManager As IMvvmManager
    Friend Property DesignerHost As IDesignerHost
    Friend Property ComponentDesigner As MvvmManagerDesigner
    'Friend Property DesignTimeAssemblyLoader As IDesignTimeAssemblyLoader
    'Friend Property ReferenceService As IReferenceService
    Friend Property TypeDiscoveryService As ITypeDiscoveryService

    Friend Property EmbeddedMode As Boolean

    Private Sub btnOK_Click(sender As Object, e As EventArgs) Handles btnOK.Click
        UcBindingTreeview1.ApplyChanges()
        myPropertyBindings.Clear()
        For Each item In UcBindingTreeview1.PropertyBindings
            myPropertyBindings.Add(item)
        Next
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Protected Overrides Sub OnClosing(e As CancelEventArgs)
        MyBase.OnClosing(e)

        If Debugger.IsAttached Then
            Debugger.Break()
        End If

        'If Me.DialogResult = System.Windows.Forms.DialogResult.OK Then
        '    If IsDirty Then
        '        Dim dr = MessageBox.Show("Do you want to discard your pending changes?",
        '                         "Pending changes",
        '                         MessageBoxButtons.YesNo, MessageBoxIcon.Error)
        '        e.Cancel = (dr = DialogResult.No)
        '    End If
        'End If
    End Sub

    Sub CommitChanges()
    End Sub

    Sub IgnoreChanges()
    End Sub

    Private Sub frmMvvmPropertyAssignment_LocationChanged(sender As Object, e As EventArgs) Handles MyBase.LocationChanged
        If System.Windows.Forms.Control.ModifierKeys = (Keys.Alt Or Keys.Shift Or Keys.Control) Then
            If Debugger.IsAttached Then
                Debugger.Break()
            End If
        End If
    End Sub

    Private Sub BindingSettingPopup_BindingSettingChanged(sender As Object, e As EventArgs)
        ' Me.IsDirty = True
    End Sub

    Private Sub SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles mySelectionListbox.SelectionChanged
        btnDelete.Enabled = (mySelectionListbox.SelectedItems IsNot Nothing AndAlso mySelectionListbox.SelectedItems.Count > 0)
    End Sub

    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        If mySelectionListbox.SelectedItems Is Nothing Then Return
        Dim items2del = mySelectionListbox.SelectedItems.Cast(Of PropertyBindingItem).ToArray
        UcBindingTreeview1.DeleteBindings(items2del)
    End Sub
End Class

