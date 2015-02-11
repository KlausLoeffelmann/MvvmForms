Imports System.Windows.Forms
Imports System.Drawing
Imports System.Collections.ObjectModel
Imports System.Threading.Tasks
Imports System.Text
Imports System.Windows.Forms.Design
Imports System.ComponentModel
Imports System.ComponentModel.Design
Imports System.IO
Imports System.Runtime.CompilerServices
Imports System.Reflection
Imports System.Windows.Data

Imports ActiveDevelop.EntitiesFormsLib.ViewModelBase

Public Class frmMvvmPropertyAssignment

    Private myViewModelType As Type
    Private myControlToBind As Object

    Private myControlProperties As ObservableCollection(Of BindingProperty)
    Private myViewModelProperties As ObservableCollection(Of BindingProperty)

    Private myPropertyBindings As ObservableBindingList(Of PropertyBindingItem)
    Private myConverters As ObservableCollection(Of ConverterDisplayItem)
    'Private myReferencedAssemblies As List(Of Assembly)
    Private myEventHandlerHaveBeenWired As Boolean

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

    Protected Overrides Sub OnLoad(e As EventArgs)
        MyBase.OnLoad(e)
        PopulateDisplayValues()
        Me.KeyPreview = True
    End Sub

    Public Sub PopulateDisplayValues()

        '#If DEBUG Then
        '        If ControlToBind Is Nothing Then
        '            MessageBox.Show("Warning: Null controls detected, Control must not not be null.",
        '                            "Null detected", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        '            ControlToBind = New NullableTextValue With {.Name = "ndvDateOfBirthTesting"}
        '            myViewModelType = GetType(ContactTest)
        '        End If
        '#End If
        myViewModelType = If(Me.MvvmManager Is Nothing, Nothing, Me.MvvmManager.DataContextType)

        If myViewModelType Is Nothing OrElse myControlToBind Is Nothing Then

        End If

        Dim viewName As String = myControlToBind.ToString()
        Dim control = TryCast(myControlToBind, Control)
        Dim viewType As String = "view"

        If control IsNot Nothing Then
            viewName = control.Name
            viewType = "control"
        End If

        If TypeOf myControlToBind Is MvvmDataGridColumn Then
            viewType = "column"
            'und das Label setzen (ist jetzt keine controlprop, sondern eine columnprop):
            lblControlProperty.Text = "Cell Property"
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

        nvrControlProperties.DataSource = myControlProperties
        nvrViewModelProperty.DataSource = myViewModelProperties
        nvrConverters.DataSource = myConverters

        If Not myEventHandlerHaveBeenWired Then
            AddHandler PropertyBindingGrid.GridDataSource.CurrentChanged, AddressOf CurrentDataGridRowChanged

            AddHandler PropertyBindingGrid.AddButton.Click, AddressOf AddOrChangePropertyItemHandler
            AddHandler PropertyBindingGrid.ChangeButton.Click, AddressOf AddOrChangePropertyItemHandler

            AddHandler PropertyBindingGrid.DeleteButton.Click, AddressOf DeletePropertyItemHandler
            myEventHandlerHaveBeenWired = True
        End If

        'Ganz zum Schluss erst Quelle binden, damit die Ereignisse, die das zur Folge hat, auch abgearbeitet werden.
        PropertyBindingGrid.GridDataSource.DataSource = IIf(myPropertyBindings IsNot Nothing AndAlso myPropertyBindings.Count > 0,
                                                            myPropertyBindings,
                                                            GetType(PropertyBindingItem))
        CurrentDataGridRowChanged(Nothing, EventArgs.Empty)
    End Sub

    Private Sub CurrentDataGridRowChanged(sender As Object, eventargs As EventArgs)

        Dim selectedPropBindingItem = DirectCast(PropertyBindingGrid.GridDataSource.Current, PropertyBindingItem)

        If selectedPropBindingItem IsNot Nothing Then
            Try
                nvrControlProperties.Value = selectedPropBindingItem.ControlProperty
            Catch ex As Exception
                Debug.Print("INCONSISTENT PROPERTY MAPPING: While deserializing the property mapping from code (control property:  '" &
                            selectedPropBindingItem.ControlProperty.PropertyName & "'), the following exception occured:" & ex.Message)
            End Try

            Try
                nvrConverterParameter.Value = selectedPropBindingItem.ConverterParameter
                nvrConverters.Value = (From convItem In myConverters
                                      Where convItem.ConverterType Is selectedPropBindingItem.Converter).FirstOrDefault
            Catch ex As Exception
                Debug.Print("INCONSISTENT PROPERTY MAPPING: While deserializing the property mapping from code (control property:  '" &
                            selectedPropBindingItem.Converter.ToString & "'), the following exception occured:" & ex.Message)
            End Try

            Try
                nvrViewModelProperty.Value = selectedPropBindingItem.ViewModelProperty
            Catch ex As Exception
                Debug.Print("INCONSISTENT PROPERTY MAPPING: While deserializing the property mapping from code (viewmodel property:  '" &
                            selectedPropBindingItem.ViewModelProperty.PropertyName & "'), the following exception occured:" & ex.Message)
            End Try
            BindingSettingPopup.BindingSetting = selectedPropBindingItem.BindingSetting
        End If

    End Sub

    Public Sub DeletePropertyItemHandler(sender As Object, e As EventArgs)
        If myPropertyBindings IsNot Nothing Then
            Dim bProperty = DirectCast(PropertyBindingGrid.GridDataSource.Current, PropertyBindingItem)
            myPropertyBindings.Remove(bProperty)
        End If
    End Sub

    Public Sub AddOrChangePropertyItemHandler(sender As Object, e As EventArgs)

        Dim ctrlProperty = DirectCast(nvrControlProperties.Value, BindingProperty)
        Dim vmProperty = DirectCast(nvrViewModelProperty.Value, BindingProperty)

        'Testen, ob die Daten vollständig erfasst wurden
        If nvrControlProperties.Value IsNot Nothing AndAlso
            nvrViewModelProperty.Value IsNot Nothing Then

            If nvrConverters.Value Is Nothing Then
                'Auf Typgleichheit prüfen, ansonsten Warnung ausgeben:
                If Not ctrlProperty.PropertyType.IsAssignableFrom(vmProperty.PropertyType) Then

                    If Debugger.IsAttached Then
                        Debugger.Break()
                    End If

                    Dim controlType = ctrlProperty.PropertyType

                    'Special case 1: We have a Nullable-Value on the control side. In this case, we're allow implicit conversion, and permit
                    'without warning, if the generic type parameter is of the same type than the VM Property type.
                    If controlType.IsGenericType AndAlso controlType.GetGenericTypeDefinition Is GetType(Nullable(Of )) Then
                        'We have to evaluate the first (and only, since it's a nullable) generic type arguments.
                        controlType = controlType.GetGenericArguments(0)
                    End If

                    'Special case 2: StringValue a control and String in a VM is OK, because it will be tolerated by the BindingEngine.
                    If (GetType(StringValue).IsAssignableFrom(controlType) And GetType(String).IsAssignableFrom(vmProperty.PropertyType)) Then
                        GoTo skipWarning
                    End If

                    'Special case: StringValue a control and String in a VM is OK, because it will be tolerated by the BindingEngine.
                    Dim dr = MessageBox.Show("The type of the control property and the view model property does not match." & vbNewLine &
                                "You should consider to either use matching types for the properties or implement a type converter," & vbNewLine &
                                "to equalize the types, which you can later choose here from the Converter combobox.",
                                "Types do not match:", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning)
                    If dr = Windows.Forms.DialogResult.Cancel Then
                        Exit Sub
                    End If
skipWarning:
                End If
            End If
        Else
            Return
        End If

        If myPropertyBindings Is Nothing Then
            myPropertyBindings = New ObservableBindingList(Of PropertyBindingItem)
        End If

        If DirectCast(sender, ToolStripButton).Name = "AddToolStripButton" Then

            Dim bProperty As New PropertyBindingItem() With
                    {.ControlProperty = ctrlProperty,
                     .ViewModelProperty = vmProperty,
                     .Converter = If(nvrConverters.Value Is Nothing, Nothing, DirectCast(nvrConverters.Value, ConverterDisplayItem).ConverterType),
                     .ConverterParameter = If(nvrConverterParameter.Value.HasValue, nvrConverterParameter.Value.Value, Nothing),
                     .BindingSetting = BindingSettingPopup.BindingSetting}

            myPropertyBindings.Add(bProperty)
            If PropertyBindingGrid.GridDataSource.DataSource IsNot myPropertyBindings Then
                PropertyBindingGrid.GridDataSource.DataSource = myPropertyBindings
            End If
        Else
            Dim bProperty = DirectCast(PropertyBindingGrid.GridDataSource.Current, PropertyBindingItem)
            With bProperty
                .ControlProperty = ctrlProperty
                .ViewModelProperty = vmProperty
                .Converter = If(nvrConverters.Value Is Nothing, Nothing, DirectCast(nvrConverters.Value, ConverterDisplayItem).ConverterType)
                .ConverterParameter = If(nvrConverterParameter.Value.HasValue, nvrConverterParameter.Value.Value, Nothing)
                .BindingSetting = BindingSettingPopup.BindingSetting
            End With
            PropertyBindingGrid.GridDataSource.ResetCurrentItem()
        End If
        If AfterAddEditOrDeleteCallbackAction IsNot Nothing Then
            AfterAddEditOrDeleteCallbackAction.Invoke()
        End If
    End Sub

    Private Function InitializeConvertersAsync() As Task

        Return Task.Factory.StartNew(AddressOf InitializeConvertersSyncWorker)

    End Function

    Private Sub InitializeConvertersSyncWorker()

        Dim messageBoxContent As String = ""

        Try
            Dim tmpConverterlist As New List(Of ConverterDisplayItem)
            Dim convs = TypeDiscoveryService.GetTypes(GetType(IValueConverter), False)

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
                (From methItem In myControlToBind.GetType.GetMethods(Reflection.BindingFlags.NonPublic Or
                                                                     Reflection.BindingFlags.Public Or
                                                                     Reflection.BindingFlags.Instance)
                 Where methItem.Name.StartsWith("On") And methItem.Name.EndsWith("Changed")
                 Join propItem In myControlToBind.GetType.GetProperties(Reflection.BindingFlags.NonPublic Or
                                                                        Reflection.BindingFlags.Public Or
                                                                        Reflection.BindingFlags.Instance) On
                 methItem.Name.Substring(2, methItem.Name.Length - 9).ToUpper Equals propItem.Name.ToUpper
                 Order By propItem.Name
                 Select New BindingProperty With
                 {.PropertyName = propItem.Name,
                  .PropertyType = propItem.PropertyType}))
        Catch ex As Exception
            MessageBox.Show("Error in Handling:" & vbNewLine & ex.Message & vbNewLine & vbNewLine & ex.StackTrace, "Not wanted:", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End Try
    End Sub

    Private Sub InitializeViewModelProperties()
        If Debugger.IsAttached Then
            Debugger.Break()
        End If

        If myViewModelType Is Nothing Then
            myViewModelProperties = New ObservableCollection(Of BindingProperty)
            Return
        End If

        Try
            Dim attList = From attItem In myViewModelType.GetCustomAttributes(True)
                     Where GetType(BusinessClassAttribute).IsAssignableFrom(attItem.GetType) Or
                     GetType(MvvmViewModelAttribute).IsAssignableFrom(attItem.GetType)

            If attList IsNot Nothing Then

                Dim businessClassAtt = (From bcaItem In attList Where GetType(BusinessClassAttribute).IsAssignableFrom(bcaItem.GetType)).SingleOrDefault

                'Das ViewModel-Attribute brauchen wir in diesem Kontext eigentlich nicht mehr.
                'Bis Februar 2014 war es erforderlich, ein ViewModel mit dem MvvmViewModel-Attribute auszuzeichnen,
                'dass es in der Typauflistung erschien - nunmehr reicht es, wenn eine Klasse INotifyPropertyChanged implementiert.
                'Deswegen ist es hier aber ebenfalls notwendig, eine entsprechende Änderung zu machen, sodass, wenn keine 
                'Attribut-Auszeichnung über der Klasse steht, an dieser Stelle die Klasse als ViewModel incl. aller ihrer
                'Eigenschaften berücksichtigt wird.
                Dim viemModelAtt = (From bcaItem In attList Where GetType(MvvmViewModelAttribute).IsAssignableFrom(bcaItem.GetType)).SingleOrDefault

                If (businessClassAtt IsNot Nothing AndAlso DirectCast(businessClassAtt, BusinessClassAttribute).Options.HasFlag(BusinessClassAttributeOptions.IncludeAllPropertiesByDefault)) Or
                    businessClassAtt Is Nothing Then

                    'Include bei Default.
                    Dim propertiesList = New List(Of PropertyCheckBoxItemController)
                    ReflectionHelper.CreateSubPropsAsList(myViewModelType, "", propertiesList, 5, False)

                    myViewModelProperties = New ObservableCollection(Of BindingProperty)(
                        From pItem In propertiesList
                        Order By pItem.PropertyFullname
                        Select New BindingProperty With
                        {.PropertyName = pItem.PropertyFullname,
                        .PropertyType = pItem.PropertyType})

                ElseIf (businessClassAtt IsNot Nothing) AndAlso
                    (DirectCast(businessClassAtt, BusinessClassAttribute).Options.HasFlag(BusinessClassAttributeOptions.ExcludeAllPropertiesByDefault)) AndAlso viemModelAtt Is Nothing Then

                    'Exclude bei Default
                    Dim propertiesList = New List(Of PropertyCheckBoxItemController)
                    ReflectionHelper.CreateSubPropsAsList(myViewModelType, "", propertiesList, 5, True)

                    myViewModelProperties = New ObservableCollection(Of BindingProperty)(
                        From pItem In propertiesList
                        Order By pItem.PropertyFullname
                        Select New BindingProperty With
                        {.PropertyName = pItem.PropertyFullname,
                        .PropertyType = pItem.PropertyType})
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Error in Handling:" & vbNewLine & ex.Message & vbNewLine & vbNewLine & ex.StackTrace, "Not wanted:", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End Try
    End Sub

    ''' <summary>
    ''' Hier wird das Control gespeichert, welches von diesem Formular als Bindungselement verwendet wird.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
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
    Friend Property DesignTimeAssemblyLoader As IDesignTimeAssemblyLoader
    Friend Property ReferenceService As IReferenceService
    Friend Property TypeDiscoveryService As ITypeDiscoveryService

    Friend Property EmbeddedMode As Boolean
    Friend Property IsDirty As Boolean

    Private Sub btnOK_Click(sender As Object, e As EventArgs) Handles btnOK.Click
        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Protected Overrides Sub OnClosing(e As CancelEventArgs)
        MyBase.OnClosing(e)
        If Me.DialogResult <> Windows.Forms.DialogResult.OK Then
            'TODO: Fill out!
        End If
    End Sub

    Private Sub nvrControlProperties_Click(sender As Object, e As EventArgs) Handles nvrControlProperties.IsDirtyChanged,
        nvrConverterParameter.IsDirtyChanged, nvrConverters.IsDirtyChanged, nvrViewModelProperty.IsDirtyChanged
        If nvrConverterParameter Is sender Then
            Me.IsDirty = True
        ElseIf DirectCast(sender, INullableValueRelationBinding).IsDirty Then
            Me.IsDirty = True
        End If

    End Sub

    Sub CommitChanges()
    End Sub

    Sub IgnoreChanges()
    End Sub

    Private Sub frmMvvmPropertyAssignment_LocationChanged(sender As Object, e As EventArgs) Handles MyBase.LocationChanged
        If Control.ModifierKeys = (Keys.Alt Or Keys.Shift Or Keys.Control) Then
            If Debugger.IsAttached Then
                Debugger.Break()
            End If
        End If

    End Sub
End Class

Friend Class ConverterDisplayItem

    Property ConverterName As String
    Property ConverterAssembly As String
    Property ConverterType As Type

End Class