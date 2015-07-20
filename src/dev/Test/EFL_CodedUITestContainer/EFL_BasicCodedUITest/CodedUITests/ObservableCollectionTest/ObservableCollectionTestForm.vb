Imports ActiveDevelop.EntitiesFormsLib
Imports System.Windows.Forms
Imports System.Collections.ObjectModel

Public Class ObservableCollectionTestForm

    Private myViewModel As New ObservableCollectionTestFormViewModel

    Private Sub ObservableCollectionTestForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        MvvmManager1.DataContext = myViewModel
    End Sub
End Class