Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting

Imports EFL_BasicCodedUITest
Imports System.ComponentModel
Imports ActiveDevelop.EntitiesFormsLib

<TestClass()>
Public Class MvvmPropertyBindingAssignmentTest

    Private Shared myBindingItems As BindingItems
    Private Shared myTestBindingItem As PropertyBindingItem
    Private Shared myTestControl As TestBindingControl
    Private Shared mySourceObject As TestViewModel
    Private Shared myTestContext As TestContext

    <ClassInitialize()>
    Public Shared Sub SetupMvvmTestStructure(testContext As TestContext)


        mySourceObject = New TestViewModel
        myTestControl = New TestBindingControl
        myBindingItems = New BindingItems()
        myTestBindingItem = New PropertyBindingItem With
                                            {.BindingSetting = New BindingSetting(MvvmBindingModes.TwoWay,
                                                                                UpdateSourceTriggerSettings.LostFocus),
                                             .ControlProperty = New BindingProperty("Value", GetType(Date?)),
                                             .Converter = Nothing,
                                             .ViewModelProperty = New BindingProperty("SomeDateNullableValue", GetType(Date?))}

        myBindingItems.Add(New BindingItem() With {.Control = myTestControl,
                                                 .MvvmItem = New MvvmBindingItem With {.ConverterAssembly = Nothing,
                                                                                       .PropertyBindings = New PropertyBindings From {myTestBindingItem}}})

        myTestContext = testContext
    End Sub

    <TestMethod()>
    Public Sub BindingManager_BasicFunctionTest()

        Dim testDateValue = #7/24/1969#

        Dim tmpSourceObject = New TestViewModel
        Dim tmpTestControl = New TestBindingControl
        Dim tmpTestControl2 = New TestBindingControl
        Dim tmpBindingItems = New BindingItems()

        'Test: Binding Nicht-Nullable an Nullable
        Dim tmpTestBindingItem = New PropertyBindingItem With
                                            {.BindingSetting = New BindingSetting(MvvmBindingModes.TwoWay,
                                                                                UpdateSourceTriggerSettings.LostFocus),
                                             .ControlProperty = New BindingProperty("Value", GetType(Date?)),
                                             .Converter = Nothing,
                                             .ViewModelProperty = New BindingProperty("SomeDateNullableValue", GetType(Date?))}

        'Test: Binding der zweiten View-Modell-Eigenschaft an die selbe Eigenschaft, wie beim vorherigen Test.
        Dim tmpTestBindingItem2 = New PropertyBindingItem With
                                            {.BindingSetting = New BindingSetting(MvvmBindingModes.TwoWay,
                                                                                UpdateSourceTriggerSettings.LostFocus),
                                             .ControlProperty = New BindingProperty("Value", GetType(Date?)),
                                             .Converter = Nothing,
                                             .ViewModelProperty = New BindingProperty("SomeDateValue", GetType(Date))}

        Dim tmpTestBindingItem3 = New PropertyBindingItem With
                                    {.BindingSetting = New BindingSetting(MvvmBindingModes.TwoWay,
                                                                        UpdateSourceTriggerSettings.LostFocus),
                                     .ControlProperty = New BindingProperty("SimpleStringValue", GetType(String)),
                                     .Converter = Nothing,
                                     .ViewModelProperty = New BindingProperty("SomeSubViewModel.SomeStringValue", GetType(String))}

        tmpBindingItems.Add(New BindingItem() With {.Control = tmpTestControl,
                                                    .MvvmItem = New MvvmBindingItem With {
                                                        .ConverterAssembly = Nothing,
                                                        .PropertyBindings = New PropertyBindings From {tmpTestBindingItem,
                                                                                                       tmpTestBindingItem2,
                                                                                                       tmpTestBindingItem3}}})

        Dim bm As New BindingManager(tmpSourceObject, tmpBindingItems, Nothing)

        'Diese Wertänderung sollte nun dazu führen, ...
        tmpTestControl.Value = testDateValue
        tmpTestControl.SimulateLostFocus()

        '...dass in beiden Eigenschaften, die gebunden sind, dieser Wert drin steht:
        Assert.AreEqual(testDateValue, tmpSourceObject.SomeDateNullableValue)
        Assert.AreEqual(testDateValue, tmpSourceObject.SomeDateValue)

        'Und umgekehrt sollte das hier:
        tmpSourceObject.SomeDateNullableValue = #10/13/1968#
        '...bewirken, dass das hier drin steht:
        Assert.AreEqual(#10/13/1968#, tmpTestControl.Value)

        'Und jetzt noch das Ganze mit komplexen Objekten:
        Dim subViewModel As New TestSubViewModel With {.SomeDoubleValue = 100,
                                                       .SomeStringValue = "Magges"}

        tmpSourceObject.SomeSubViewModel = subViewModel
        tmpSourceObject.SomeSubViewModel.SomeStringValue = "Klaus"
        Assert.AreEqual("Klaus", tmpTestControl.SimpleStringValue)

        Dim subViewModel2 = New TestSubViewModel With {.SomeDoubleValue = 100,
                                                  .SomeStringValue = "Magges"}

        tmpSourceObject.SomeSubViewModel = subViewModel2
        Assert.AreEqual(tmpTestControl.SimpleStringValue, "Magges")

        'Auch hier: umgekehrten Weg testen:
        tmpTestControl.SimpleStringValue = "Adriana"
        tmpTestControl.SimulateLostFocus()
        Assert.AreEqual(tmpSourceObject.SomeSubViewModel.SomeStringValue, "Adriana")

        'Aber: in subViewModel darf Adriana nicht mehr stehen, und...
        Assert.AreNotEqual(subViewModel.SomeStringValue, "Adriana")

        '...eine Änderung in subViewModel darf auch SimpleStringValue nicht beeinflussen.
        subViewModel.SomeStringValue = "Andreas"
        Assert.AreNotEqual(tmpTestControl.SimpleStringValue, "Andreas")

        'Wenn wir an das ViewModel Nothing zuweisen, sollten die Inhalte gelöscht sein.
        tmpTestControl.SimpleDateValue = Date.MaxValue
        tmpTestControl.Value = Date.MaxValue
        tmpTestControl.SimpleStringValue = "Hier is something in it, definitely."
        bm.ViewModel = Nothing
        Assert.IsNull(tmpTestControl.SimpleStringValue)
        Assert.IsNull(tmpTestControl.Value)

        'Gleichzeitig darf es keine Kommunikation mehr zwischen ViewModel und View geben:
        tmpSourceObject.SomeDateNullableValue = testDateValue
        Assert.IsNull(tmpTestControl.Value)

        tmpSourceObject.SomeDateNullableValue = Nothing
        tmpTestControl.Value = testDateValue
        Assert.IsNull(tmpSourceObject.SomeDateNullableValue)

        'Und sie muss in der gleichen Instanz wieder bindbar sein:
        tmpSourceObject.SomeDateNullableValue = testDateValue
        bm.ViewModel = tmpSourceObject
        Assert.AreEqual(testDateValue, tmpSourceObject.SomeDateNullableValue)

        'Disposing in alle Richtungen testen:
        bm.Dispose()
    End Sub

    <TestMethod()>
    Sub BindingManager_BindingTest_TwoWayWithReadonlyProperty()

        'Keine Exceptions bei Readonly-Eigenschaften schmeissen
        BindingManager.ThrowExceptionOnUnassignableBindingProperties = False

        Dim testDateValue = #7/24/1969#

        Dim sourceObject = New TestViewModel
        Dim testControl = New TestBindingControl
        Dim bindingItems = New BindingItems()

        'Test: Binding OneWay. That is ViewModel to Control.
        Dim simpleTestBindingItem = New PropertyBindingItem With
                                            {.BindingSetting = New BindingSetting(MvvmBindingModes.TwoWay,
                                                                                UpdateSourceTriggerSettings.PropertyChangedImmediately),
                                             .ControlProperty = New BindingProperty("SimpleStringValue", GetType(String)),
                                             .Converter = Nothing,
                                             .ViewModelProperty = New BindingProperty("SomeReadOnlyStringValue", GetType(String))}

        bindingItems.Add(New BindingItem() With {.Control = testControl,
                                                    .MvvmItem = New MvvmBindingItem With {
                                                        .ConverterAssembly = Nothing,
                                                        .PropertyBindings = New PropertyBindings From {
                                                                    simpleTestBindingItem}}})


        sourceObject.SetSomeReadOnlyStringValue("MvvmForms rules!")
        'The setting up of the Binding should lead to...
        Dim bm As New BindingManager(sourceObject, bindingItems, Nothing,
                                     updateControlsAfterInstatiating:=True)


        '...the value SimpleStringValue being changed in the control.
        Assert.AreEqual(Of String)(testControl.SimpleStringValue, sourceObject.SomeReadOnlyStringValue)

        'And this now should not have any exception consequences!
        testControl.SimpleStringValue = "Let's see how this goes!"

    End Sub


    <TestMethod()>
    Sub BindingManager_DirectionTest_OneWay()
        Dim testDateValue = #7/24/1969#

        Dim sourceObject = New TestViewModel
        Dim testControl = New TestBindingControl
        Dim bindingItems = New BindingItems()

        'Test: Binding OneWay. That is ViewModel to Control.
        Dim simpleTestBindingItem = New PropertyBindingItem With
                                            {.BindingSetting = New BindingSetting(MvvmBindingModes.OneWay,
                                                                                UpdateSourceTriggerSettings.LostFocus),
                                             .ControlProperty = New BindingProperty("Value", GetType(Date?)),
                                             .Converter = Nothing,
                                             .ViewModelProperty = New BindingProperty("SomeDateNullableValue", GetType(Date?))}

        Dim complexTestBindingItem = New PropertyBindingItem With
                                    {.BindingSetting = New BindingSetting(MvvmBindingModes.OneWay,
                                                                        UpdateSourceTriggerSettings.LostFocus),
                                     .ControlProperty = New BindingProperty("SimpleStringValue", GetType(String)),
                                     .Converter = Nothing,
                                     .ViewModelProperty = New BindingProperty("SomeSubViewModel.SomeStringValue", GetType(String))}

        bindingItems.Add(New BindingItem() With {.Control = testControl,
                                                    .MvvmItem = New MvvmBindingItem With {
                                                        .ConverterAssembly = Nothing,
                                                        .PropertyBindings = New PropertyBindings From {
                                                                    simpleTestBindingItem,
                                                                    complexTestBindingItem}}})

        Dim bm As New BindingManager(sourceObject, bindingItems, Nothing)

        'This change of SomeDateValue in the ViewModel should lead to...
        sourceObject.SomeDateNullableValue = #1969-07-24#

        '...the value SimpleDateValue being changed in the control.
        Assert.AreEqual(testControl.SimpleDateValue, sourceObject.SomeDateValue)

        'The same with a more complex object and a 2-level Property Path (viewmodel.SomeSubViewModel.SomeStringValue)
        Dim subViewModel As New TestSubViewModel With {.SomeDoubleValue = 100,
                                                       .SomeStringValue = "Initial Value."}

        sourceObject.SomeSubViewModel = subViewModel
        Assert.AreEqual("Initial Value.", testControl.SimpleStringValue)

        sourceObject.SomeSubViewModel.SomeStringValue = "Klaus"
        Assert.AreEqual("Klaus", testControl.SimpleStringValue)

        'The other direction MUST NOT work!
        testControl.SimpleStringValue = "Adriana"
        'So this must still be Klaus, and NOT Adriana. 
        Assert.AreEqual("Klaus", sourceObject.SomeSubViewModel.SomeStringValue)

    End Sub

    <TestMethod()>
    Sub BindingManager_DirectionTest_OneWayToSource()
        Dim testDateValue = #7/24/1969#

        Dim sourceObject = New TestViewModel
        Dim testControl = New TestBindingControl
        Dim bindingItems = New BindingItems()

        'Test: Binding OneWay. That is ViewModel to Control.
        Dim simpleTestBindingItem = New PropertyBindingItem With
                                            {.BindingSetting = New BindingSetting(MvvmBindingModes.OneWayToSource,
                                                                                UpdateSourceTriggerSettings.LostFocus),
                                             .ControlProperty = New BindingProperty("Value", GetType(Date?)),
                                             .Converter = Nothing,
                                             .ViewModelProperty = New BindingProperty("SomeDateNullableValue", GetType(Date?))}

        Dim complexTestBindingItem = New PropertyBindingItem With
                                    {.BindingSetting = New BindingSetting(MvvmBindingModes.OneWayToSource,
                                                                        UpdateSourceTriggerSettings.LostFocus),
                                     .ControlProperty = New BindingProperty("SimpleStringValue", GetType(String)),
                                     .Converter = Nothing,
                                     .ViewModelProperty = New BindingProperty("SomeSubViewModel.SomeStringValue", GetType(String))}

        bindingItems.Add(New BindingItem() With {.Control = testControl,
                                                    .MvvmItem = New MvvmBindingItem With {
                                                    .ConverterAssembly = Nothing,
                                                    .PropertyBindings = New PropertyBindings From {
                                                                simpleTestBindingItem,
                                                                complexTestBindingItem}}})

        Dim bm As New BindingManager(sourceObject, bindingItems, Nothing)

        'This change of SomeDateValue in the ViewModel should lead to...
        testControl.SimpleDateValue = #1969-07-24#

        '...the value SimpleDateValue being changed in the control.
        Assert.AreEqual(sourceObject.SomeDateNullableValue, sourceObject.SomeDateValue)

        'The same with a more complex object and a 2-level Property Path (viewmodel.SomeSubViewModel.SomeStringValue)
        Dim subViewModel As New TestSubViewModel With {.SomeDoubleValue = 100,
                                                       .SomeStringValue = "Initial Value."}

        'The other direction MUST NOT work!
        sourceObject.SomeSubViewModel = subViewModel
        Assert.AreNotEqual("Initial Value.", testControl.SimpleStringValue)

        'This does.
        testControl.SimpleStringValue = "Klaus"
        Assert.AreEqual("Klaus", sourceObject.SomeSubViewModel.SomeStringValue)
    End Sub


    <TestMethod()>
    Sub BindingManager_AmbigiousPropertyNameTest()

        Dim tmpBindingItems = New BindingItems()
        Dim tmpSourceObject = New TestViewModel
        Dim tmpTestControl = New TestBindingControl
        Dim tmpTestControl2 = New TestBindingControl

        'Bindings in model with the same name in the last part of the property path on two or more seperate controls (after weird behaviour @ G**/Jan)
        Dim tmpTestBindingItem = New PropertyBindingItem With
                                            {.BindingSetting = New BindingSetting(MvvmBindingModes.TwoWay,
                                                                                  UpdateSourceTriggerSettings.LostFocus),
                                             .ControlProperty = New BindingProperty("SimpleStringValue", GetType(String)),
                                             .Converter = Nothing,
                                             .ViewModelProperty = New BindingProperty("SomeStringValue", GetType(String))}

        Dim tmpTestBindingItem2 = New PropertyBindingItem With
                                    {.BindingSetting = New BindingSetting(MvvmBindingModes.TwoWay,
                                                                        UpdateSourceTriggerSettings.LostFocus),
                                     .ControlProperty = New BindingProperty("SimpleStringValue", GetType(String)),
                                     .Converter = Nothing,
                                     .ViewModelProperty = New BindingProperty("SomeSubViewModel.SomeStringValue", GetType(String))}

        tmpBindingItems.Add(New BindingItem() With {.Control = tmpTestControl,
                                                    .MvvmItem = New MvvmBindingItem With {
                                                    .ConverterAssembly = Nothing,
                                                    .PropertyBindings = New PropertyBindings From {tmpTestBindingItem}}})

        tmpBindingItems.Add(New BindingItem() With {.Control = tmpTestControl2,
                                            .MvvmItem = New MvvmBindingItem With {
                                            .ConverterAssembly = Nothing,
                                            .PropertyBindings = New PropertyBindings From {tmpTestBindingItem2}}})

        Dim bm = New BindingManager(tmpSourceObject, tmpBindingItems, Nothing)
        Dim stringValueToCompareResultsWith = "StringValueToTest"

        tmpSourceObject.SomeStringValue = stringValueToCompareResultsWith

        Assert.AreEqual(stringValueToCompareResultsWith, tmpTestControl.SimpleStringValue)
        Assert.AreNotEqual(stringValueToCompareResultsWith, tmpTestControl2.SimpleStringValue)
    End Sub

    <TestMethod()>
    Sub BindingManager_SameViewModelPropertyToDifferentControlsTest()

        Dim tmpBindingItems = New BindingItems()
        Dim tmpSourceObject = New TestViewModel
        Dim tmpTestControl = New TestBindingControl
        Dim tmpTestControl2 = New TestBindingControl

        'Bindungen im Model mit gleichem Namen im Endpfad an zwei unterschiedliche Controls (nach Fehler bei GAP/Jan)
        Dim tmpTestBindingItem = New PropertyBindingItem With
                                            {.BindingSetting = New BindingSetting(MvvmBindingModes.TwoWay,
                                                                                  UpdateSourceTriggerSettings.LostFocus),
                                             .ControlProperty = New BindingProperty("SimpleStringValue", GetType(String)),
                                             .Converter = Nothing,
                                             .ViewModelProperty = New BindingProperty("SomeStringValue", GetType(String))}

        Dim tmpTestBindingItem2 = New PropertyBindingItem With
                                            {.BindingSetting = New BindingSetting(MvvmBindingModes.TwoWay,
                                                                                  UpdateSourceTriggerSettings.LostFocus),
                                             .ControlProperty = New BindingProperty("SimpleStringValue", GetType(String)),
                                             .Converter = Nothing,
                                             .ViewModelProperty = New BindingProperty("SomeStringValue", GetType(String))}

        tmpBindingItems.Add(New BindingItem() With {.Control = tmpTestControl,
                                                    .MvvmItem = New MvvmBindingItem With {
                                                    .ConverterAssembly = Nothing,
                                                    .PropertyBindings = New PropertyBindings From {tmpTestBindingItem}}})

        tmpBindingItems.Add(New BindingItem() With {.Control = tmpTestControl2,
                                            .MvvmItem = New MvvmBindingItem With {
                                            .ConverterAssembly = Nothing,
                                            .PropertyBindings = New PropertyBindings From {tmpTestBindingItem2}}})

        Dim bm = New BindingManager(tmpSourceObject, tmpBindingItems, Nothing)
        Dim stringValueToCompareResultsWith = "StringValueToTest"
        'tmpSourceObject.SomeSubViewModel = New TestSubViewModel
        'tmpSourceObject.SomeSubViewModel.SomeStringValue = stringValueToCompareResultsWith

        Assert.IsTrue(String.IsNullOrEmpty(tmpTestControl.SimpleStringValue))
        Assert.IsTrue(String.IsNullOrEmpty(tmpTestControl2.SimpleStringValue))

        tmpSourceObject.SomeStringValue = stringValueToCompareResultsWith

        Assert.AreEqual(stringValueToCompareResultsWith, tmpTestControl.SimpleStringValue)
        tmpTestControl.SimpleStringValue = ""
        Assert.AreEqual(stringValueToCompareResultsWith, tmpTestControl2.SimpleStringValue)
        Assert.IsTrue(String.IsNullOrEmpty(tmpTestControl.SimpleStringValue))

    End Sub

    <TestMethod()>
    Sub BindingManager_SameViewModelPropertyToDifferentControlsWithDifferentPropertyNames()

        Dim tmpBindingItems = New BindingItems()
        Dim tmpSourceObject = New TestViewModel
        Dim tmpTestControl = New TestBindingControl
        Dim tmpTestControl2 = New TestBindingControlDifferentProperties

        'Bindungen im Model mit gleichem Namen im Endpfad an zwei unterschiedliche Controls (nach Fehler bei GAP/Jan)
        Dim tmpTestBindingItem = New PropertyBindingItem With
                                            {.BindingSetting = New BindingSetting(MvvmBindingModes.TwoWay,
                                                                                  UpdateSourceTriggerSettings.LostFocus),
                                             .ControlProperty = New BindingProperty("SimpleStringValue", GetType(String)),
                                             .Converter = Nothing,
                                             .ViewModelProperty = New BindingProperty("SomeStringValue", GetType(String))}

        Dim tmpTestBindingItem2 = New PropertyBindingItem With
                                            {.BindingSetting = New BindingSetting(MvvmBindingModes.TwoWay,
                                                                                  UpdateSourceTriggerSettings.LostFocus),
                                             .ControlProperty = New BindingProperty("NotSoSimpleStringValue", GetType(String)),
                                             .Converter = Nothing,
                                             .ViewModelProperty = New BindingProperty("SomeStringValue", GetType(String))}

        tmpBindingItems.Add(New BindingItem() With {.Control = tmpTestControl,
                                                    .MvvmItem = New MvvmBindingItem With {
                                                    .ConverterAssembly = Nothing,
                                                    .PropertyBindings = New PropertyBindings From {tmpTestBindingItem}}})

        tmpBindingItems.Add(New BindingItem() With {.Control = tmpTestControl2,
                                            .MvvmItem = New MvvmBindingItem With {
                                            .ConverterAssembly = Nothing,
                                            .PropertyBindings = New PropertyBindings From {tmpTestBindingItem2}}})

        Dim bm = New BindingManager(tmpSourceObject, tmpBindingItems, Nothing)
        Dim stringValueToCompareResultsWith = "StringValueToTest"
        'tmpSourceObject.SomeSubViewModel = New TestSubViewModel
        'tmpSourceObject.SomeSubViewModel.SomeStringValue = stringValueToCompareResultsWith

        Assert.IsTrue(String.IsNullOrEmpty(tmpTestControl.SimpleStringValue))
        Assert.IsTrue(String.IsNullOrEmpty(tmpTestControl2.NotSoSimpleStringValue))

        tmpSourceObject.SomeStringValue = stringValueToCompareResultsWith

        Assert.AreEqual(stringValueToCompareResultsWith, tmpTestControl.SimpleStringValue)
        tmpTestControl.SimpleStringValue = ""
        Assert.AreEqual(stringValueToCompareResultsWith, tmpTestControl2.NotSoSimpleStringValue)
        Assert.IsTrue(String.IsNullOrEmpty(tmpTestControl.SimpleStringValue))

    End Sub

    <TestMethod()>
    Sub BindingManager_SameDeepViewModelPropertyToDifferentControlsWithDifferentPropertyNames()

        Dim tmpBindingItems = New BindingItems()
        Dim tmpSourceObject = New TestViewModel
        Dim tmpTestControl = New TestBindingControl
        Dim tmpTestControl2 = New TestBindingControlDifferentProperties

        'Bindungen im Model mit gleichem Namen im Endpfad an zwei unterschiedliche Controls (nach Fehler bei GAP/Jan)
        Dim tmpTestBindingItem = New PropertyBindingItem With
                                            {.BindingSetting = New BindingSetting(MvvmBindingModes.TwoWay,
                                                                                  UpdateSourceTriggerSettings.LostFocus),
                                             .ControlProperty = New BindingProperty("SimpleStringValue", GetType(String)),
                                             .Converter = Nothing,
                                             .ViewModelProperty = New BindingProperty("SomeSubViewModel.SomeStringValue", GetType(String))}

        Dim tmpTestBindingItem2 = New PropertyBindingItem With
                                            {.BindingSetting = New BindingSetting(MvvmBindingModes.TwoWay,
                                                                                  UpdateSourceTriggerSettings.LostFocus),
                                             .ControlProperty = New BindingProperty("NotSoSimpleStringValue", GetType(String)),
                                             .Converter = Nothing,
                                             .ViewModelProperty = New BindingProperty("SomeSubViewModel.SomeStringValue", GetType(String))}

        tmpBindingItems.Add(New BindingItem() With {.Control = tmpTestControl,
                                                    .MvvmItem = New MvvmBindingItem With {
                                                    .ConverterAssembly = Nothing,
                                                    .PropertyBindings = New PropertyBindings From {tmpTestBindingItem}}})

        tmpBindingItems.Add(New BindingItem() With {.Control = tmpTestControl2,
                                            .MvvmItem = New MvvmBindingItem With {
                                            .ConverterAssembly = Nothing,
                                            .PropertyBindings = New PropertyBindings From {tmpTestBindingItem2}}})

        Dim bm = New BindingManager(tmpSourceObject, tmpBindingItems, Nothing)
        Dim stringValueToCompareResultsWith = "StringValueToTest"

        tmpSourceObject.SomeStringValue = stringValueToCompareResultsWith

        'Hier wird es gesetzt und hier kommt es zum Fehler:
        tmpSourceObject.SomeSubViewModel = New TestSubViewModel() With {.SomeStringValue = "test"}

        tmpTestControl.SimpleStringValue = ""

    End Sub

    <TestMethod()>
    Sub NullableValueBase_IsDirtyQueryAndValueValidationTest()

        Dim numControl As New TestBindingNumControl

        numControl.MaxValue = 5
        numControl.MinValue = 0
        numControl.EmulatedIsFocusedForUnitTesting = True
        numControl.SimulateGotFocus()
        numControl.Text = 6.ToString
        numControl.SimulateLostFocus()
        Assert.IsTrue(numControl.IsDirty)
    End Sub

End Class

<CLSCompliant(False)>
Public Class TestBindingNumControl
    Inherits NullableNumValue

    Public Sub SimulateGotFocus()
        MyBase.OnGotFocus(EventArgs.Empty)
    End Sub

    Public Sub SimulateLostFocus()
        Dim ceArgs As New CancelEventArgs
        MyBase.OnValidating(ceArgs)
        If ceArgs.Cancel Then Return
        MyBase.OnLeave(EventArgs.Empty)
    End Sub

End Class

<CLSCompliant(False)>
Public Class TestBindingControl
    Inherits NullableDateValue

    Public Event SimpleDateValueChanged(sender As Object, e As EventArgs)
    Public Event SimpleNullableDateValueChanged(sender As Object, e As EventArgs)
    Public Event SimpleStringValueChanged(sender As Object, e As EventArgs)

    Private mySimpleDateValue As Date
    Private mySimpleStringValue As String

    Public Sub SimulateLostFocus()
        MyBase.OnLeave(EventArgs.Empty)
    End Sub

    Public Property SimpleDateValue As Date
        Get
            Return mySimpleDateValue
        End Get
        Set(value As Date)
            If Not Object.Equals(value, mySimpleDateValue) Then
                mySimpleDateValue = value
                RaiseEvent SimpleDateValueChanged(Me, EventArgs.Empty)
            End If
        End Set
    End Property

    Public Property SimpleStringValue As String
        Get
            Return mySimpleStringValue
        End Get
        Set(value As String)
            If Not Object.Equals(value, mySimpleStringValue) Then
                mySimpleStringValue = value
                RaiseEvent SimpleStringValueChanged(Me, EventArgs.Empty)
            End If
        End Set
    End Property
End Class

<CLSCompliant(False)>
Public Class TestBindingControlDifferentProperties
    Inherits NullableDateValue

    Public Event SimpleDateValueChanged(sender As Object, e As EventArgs)
    Public Event SimpleNullableDateValueChanged(sender As Object, e As EventArgs)
    Public Event SimpleStringValueChanged(sender As Object, e As EventArgs)

    Private mySimpleDateValue As Date
    Private mySimpleStringValue As String

    Public Sub SimulateLostFocus()
        MyBase.OnLeave(EventArgs.Empty)
    End Sub

    Public Property NotSoSimpleDateValue As Date
        Get
            Return mySimpleDateValue
        End Get
        Set(value As Date)
            If Not Object.Equals(value, mySimpleDateValue) Then
                mySimpleDateValue = value
                RaiseEvent SimpleDateValueChanged(Me, EventArgs.Empty)
            End If
        End Set
    End Property

    Public Property NotSoSimpleStringValue As String
        Get
            Return mySimpleStringValue
        End Get
        Set(value As String)
            If Not Object.Equals(value, mySimpleStringValue) Then
                mySimpleStringValue = value
                RaiseEvent SimpleStringValueChanged(Me, EventArgs.Empty)
            End If
        End Set
    End Property
End Class


Public Class TestViewModel
    Implements INotifyPropertyChanged

    Private mySomeDateValue As Date
    Private mySomeDoubleValue As Double
    Private mySomeDateNullableValue As Date?
    Private mySomeStringValue As String
    Private mySomeSubValueModel As TestSubViewModel
    Private mySomeReadOnlyStringValue As String
    Private mySomeSubViewModelWithPrivateSetter As TestSubViewModel
    Public Event PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Implements INotifyPropertyChanged.PropertyChanged

    Public Property SomeDateValue As Date
        Get
            Return mySomeDateValue
        End Get
        Set(value As Date)
            If Not Object.Equals(value, mySomeDateValue) Then
                mySomeDateValue = value
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("SomeDateValue"))
            End If
        End Set
    End Property

    Public Property SomeDateNullableValue As Date?
        Get
            Return mySomeDateNullableValue
        End Get
        Set(value As Date?)
            If Not Object.Equals(value, mySomeDateValue) Then
                mySomeDateNullableValue = value
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("SomeDateNullableValue"))
            End If
        End Set
    End Property


    Public Property SomeDoubleValue As Double
        Get
            Return mySomeDoubleValue
        End Get
        Set(value As Double)
            If Not Object.Equals(value, mySomeDoubleValue) Then
                mySomeDoubleValue = value
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("SomeDoubleValue"))
            End If
        End Set
    End Property

    Public Property SomeStringValue As String
        Get
            Return mySomeStringValue
        End Get
        Set(value As String)
            If Not Object.Equals(value, mySomeStringValue) Then
                mySomeStringValue = value
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("SomeStringValue"))
            End If
        End Set
    End Property

    Public Sub SetSomeReadOnlyStringValue(value As String)
        mySomeReadOnlyStringValue = value
    End Sub

    Public ReadOnly Property SomeReadOnlyStringValue As String
        Get
            Return mySomeReadOnlyStringValue
        End Get
    End Property

    Public Property SomeSubViewModel As TestSubViewModel
        Get
            Return mySomeSubValueModel
        End Get
        Set(value As TestSubViewModel)
            If Not Object.Equals(value, mySomeSubValueModel) Then
                mySomeSubValueModel = value
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("SomeSubViewModel"))
            End If
        End Set
    End Property

    Public Sub SetSomeSubViewModelWithPrivateSetter(value As TestSubViewModel)
        SomeSubViewModelWithPrivateSetter = value
    End Sub

    Public Property SomeSubViewModelWithPrivateSetter As TestSubViewModel
        Get
            Return mySomeSubViewModelWithPrivateSetter
        End Get
        Private Set(value As TestSubViewModel)
            If Not Object.Equals(value, mySomeSubViewModelWithPrivateSetter) Then
                mySomeSubViewModelWithPrivateSetter = value
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("SomeSubViewModelWithPrivateSetter"))
            End If
        End Set
    End Property
End Class

Public Class TestSubViewModel
    Implements INotifyPropertyChanged

    Private mySomeDoubleValue As Double
    Private mySomeStringValue As String

    Public Event PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Implements INotifyPropertyChanged.PropertyChanged

    Public Property SomeDoubleValue As Double
        Get
            Return mySomeDoubleValue
        End Get
        Set(value As Double)
            If Not Object.Equals(value, mySomeDoubleValue) Then
                mySomeDoubleValue = value
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("SomeDoubleValue"))
            End If
        End Set
    End Property

    Public Property SomeStringValue As String
        Get
            Return mySomeStringValue
        End Get
        Set(value As String)
            If Not Object.Equals(value, mySomeStringValue) Then
                mySomeStringValue = value
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("SomeStringValue"))
            End If
        End Set
    End Property
End Class