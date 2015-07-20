Imports System.Drawing
Imports System.Text.RegularExpressions
Imports System.Windows.Forms
Imports System.Windows.Input
Imports Microsoft.VisualStudio.TestTools.UITest.Extension
Imports Microsoft.VisualStudio.TestTools.UITesting
Imports Microsoft.VisualStudio.TestTools.UITesting.Keyboard
Imports Microsoft.VisualStudio.TestTools.UITesting.WinControls
Imports System.Threading

<CodedUITest()>
Public Class BasicFunctionTest

    Private Const TIME_IN_MS_BETWEEN_KEYSTROKES = 100

    <TestMethod(), Priority(0)>
    Public Sub BasicDataEntryTest()
        '            
        ' To generate code for this test, select "Generate Code for Coded UI Test" from the shortcut menu and select one of the menu items.
        ' For more information on generated code, see http://go.microsoft.com/fwlink/?LinkId=179463
        '
        Me.UIMap.LaunchBasicTestForm()
        Me.UIMap.EnterTestDataForNullableTextBox()
        Me.UIMap.EnterTestDataForNullableMultilineTextBox()
        Me.UIMap.EnterTestDataForNullableIntBox()
        Me.UIMap.EnterTestDataForNullableNumBox()
        Me.UIMap.EnterTestDataForNullableDateBox()
        Me.UIMap.FinishTestdateInput()
        Me.UIMap.CheckFormattedResults()

    End Sub

    <TestMethod()>
    Public Sub TestNullableValueRelationPopup_BasicFunction()

        Me.UIMap.LaunchBasicTestForm()
        Me.UIMap.PrepareNullableRelationPopup()
        SelectValueWithCursorKey()
        Me.UIMap.CheckForCorrectEventOrder()
        Me.UIMap.NullRelationalPopupInput()
        Me.UIMap.CheckForNulledCorrectEventOrder()

    End Sub

    <TestMethod()>
    Public Sub TestNullableValueRelationPopup_MultipleSearchKeyWords()

        Me.UIMap.LaunchBasicTestForm()
        Me.UIMap.PrepareNullableRelationPopup()
        Dim nRelPopup = Me.UIMap.UIEntityFormsLibTestCoWindow.NullableRelationalPopupContainer.NullableRelationalPopup

        'Erstmal "Clarke" eingeben. Dann den selektierten Zustand überprüfen.
        EnterKeysConsequtively("Clarke", nRelPopup)
        Keyboard.SendKeys(nRelPopup, "{Enter}", ModifierKeys.None) : Thread.Sleep(100)


    End Sub

    Private Sub SelectValueWithCursorKey()
        Dim nRelPopup As WinEdit = Me.UIMap.UIEntityFormsLibTestCoWindow.NullableRelationalPopupContainer.NullableRelationalPopup

        'Type 4 x '{Down}' in 'NullableRelationPopup' text box
        Keyboard.SendKeys(nRelPopup, "{Down}", ModifierKeys.None) : Thread.Sleep(100)
        Keyboard.SendKeys(nRelPopup, "{Down}", ModifierKeys.None) : Thread.Sleep(100)
        Keyboard.SendKeys(nRelPopup, "{Down}", ModifierKeys.None) : Thread.Sleep(100)
        Keyboard.SendKeys(nRelPopup, "{Down}", ModifierKeys.None) : Thread.Sleep(100)
        Keyboard.SendKeys(nRelPopup, "{Enter}", ModifierKeys.None) : Thread.Sleep(100)
        Keyboard.SendKeys(nRelPopup, "B", ModifierKeys.None) : Thread.Sleep(100)
        Keyboard.SendKeys(nRelPopup, "e", ModifierKeys.None) : Thread.Sleep(100)
        Keyboard.SendKeys(nRelPopup, "c", ModifierKeys.None) : Thread.Sleep(100)
        Keyboard.SendKeys(nRelPopup, "{Down}", ModifierKeys.None) : Thread.Sleep(100)
        Keyboard.SendKeys(nRelPopup, "{Down}", ModifierKeys.None) : Thread.Sleep(100)
        Keyboard.SendKeys(nRelPopup, "{Down}", ModifierKeys.None) : Thread.Sleep(100)
        Keyboard.SendKeys(nRelPopup, "{Down}", ModifierKeys.None) : Thread.Sleep(100)
        Keyboard.SendKeys(nRelPopup, "{Enter}", ModifierKeys.None) : Thread.Sleep(100)

    End Sub

    Private Sub EnterKeysConsequtively(Keys As String, NullableRelationPopup As WinEdit)

        For Each charItem In Keys.ToCharArray
            Keyboard.SendKeys(NullableRelationPopup, charItem, ModifierKeys.None) : Thread.Sleep(100)
        Next
    End Sub

#Region "Additional test attributes"
    '
    ' You can use the following additional attributes as you write your tests:
    '
    '' Use TestInitialize to run code before running each test
    '<TestInitialize()> Public Sub MyTestInitialize()
    '    '
    '    ' To generate code for this test, select "Generate Code for Coded UI Test" from the shortcut menu and select one of the menu items.
    '    ' For more information on generated code, see http://go.microsoft.com/fwlink/?LinkId=179463
    '    '
    'End Sub

    '' Use TestCleanup to run code after each test has run
    '<TestCleanup()> Public Sub MyTestCleanup()
    '    '
    '    ' To generate code for this test, select "Generate Code for Coded UI Test" from the shortcut menu and select one of the menu items.
    '    ' For more information on generated code, see http://go.microsoft.com/fwlink/?LinkId=179463
    '    '
    'End Sub

#End Region

    '''<summary>
    '''Gets or sets the test context which provides
    '''information about and functionality for the current test run.
    '''</summary>
    Public Property TestContext() As TestContext
        Get
            Return testContextInstance
        End Get
        Set(ByVal value As TestContext)
            testContextInstance = value
        End Set
    End Property

    Private testContextInstance As TestContext

    Public ReadOnly Property UIMap As EFL_BasicCodedUITest.UIMap
        Get
            If (Me.map Is Nothing) Then
                Me.map = New EFL_BasicCodedUITest.UIMap()
            End If

            Return Me.map
        End Get
    End Property
    Private map As EFL_BasicCodedUITest.UIMap
End Class
