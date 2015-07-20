Imports System.ComponentModel
Imports ActiveDevelop.EntitiesFormsLib

<TestClass()>
Public Class ObservableCollectionBindingListAdapterTest

    <TestMethod()>
    Public Sub BasicTest()

        Dim token As Integer = 0
        Dim processedItem As ContactViewModel

        Dim InputList = ContactViewModel.GetTestData(1000)
        Dim OutputList = New ObservableCollectionBindingListAdapter
        OutputList.DataSource = InputList

        AddHandler OutputList.ListChanged, Sub(obj, eArgs)
                                               If token = 0 Then
                                                   Assert.AreEqual(0, eArgs.NewIndex)
                                                   Assert.AreEqual(500, eArgs.OldIndex)
                                                   Assert.AreEqual(ListChangedType.ItemDeleted, eArgs.ListChangedType)
                                               ElseIf token = 1 Then
                                                   Assert.AreEqual(999, eArgs.NewIndex)
                                                   Assert.AreEqual(-1, eArgs.OldIndex)
                                                   Assert.AreEqual(ListChangedType.ItemAdded, eArgs.ListChangedType)
                                               End If
                                           End Sub

        token = 0
        InputList.RemoveAt(500)
        processedItem = ContactViewModel.GetTestData(1)(0)
        token = 1
        InputList.Add(processedItem)

    End Sub

    Private Function CompareLists(list1 As ICollection, list2 As ICollection) As Boolean

        For c = 0 To list1.Count - 1
            If Not list1(0).Equals(list2(c)) Then
                Return False
            End If
        Next
        Return True
    End Function


End Class
