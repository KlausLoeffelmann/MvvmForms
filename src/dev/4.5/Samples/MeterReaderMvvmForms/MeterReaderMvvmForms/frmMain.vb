Imports MRViewModelLibrary

Public Class frmMain

    Private myMainViewModel As New MainViewModel

    Private Async Sub TestToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles TestToolStripMenuItem.Click
        Dim buildings = Await MRViewModelLibrary.BuildingViewModel.GetAllBuildings()
    End Sub

    Private Async Sub frmMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        MvvmManager1.DataContext = myMainViewModel
        myMainViewModel.Buildings = Await MRViewModelLibrary.BuildingViewModel.GetAllBuildings()
    End Sub
End Class
