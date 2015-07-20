Imports System.ComponentModel
Imports ActiveDevelop.MvvmBaseLib
Imports ActiveDevelop.MvvmBaseLib.Mvvm
Imports MRViewModelLibrary

Public Class WinFormsDependencyService
    Implements IPlatformDependencyService

    Public ReadOnly Property IsDesignMode As Boolean Implements IPlatformDependencyService.IsDesignMode
        Get
            Return (LicenseManager.CurrentContext.UsageMode = LicenseUsageMode.Designtime)
        End Get
    End Property

    Public Function NavigateToAsync(Of t As MvvmBase)(viewModel As t, Optional pageTitel As String = Nothing) As Task Implements IPlatformDependencyService.NavigateToAsync
        Throw New NotImplementedException()
    End Function

    Public Function RequestAppBarClose() As Boolean Implements IPlatformDependencyService.RequestAppBarClose
        Throw New NotImplementedException()
    End Function

    Public Function RequestAppBarOpen() As Boolean Implements IPlatformDependencyService.RequestAppBarOpen
        Throw New NotImplementedException()
    End Function

    Public Function SaveStringToFileAsync(filename As String, data As String) As Task(Of String) Implements IPlatformDependencyService.SaveStringToFileAsync
        Throw New NotImplementedException()
    End Function

    Public Function SaveStringToPickableFileAsync(pickerTitel As String, defaultExtensions As IDictionary(Of String, IList(Of String)), commitButtonText As String, data As String) As Task(Of String) Implements IPlatformDependencyService.SaveStringToPickableFileAsync
        Throw New NotImplementedException()
    End Function

    Public Function ShowDialog(Of t As BindableBase)(viewModel As t, Optional dialogTitel As String = Nothing, Optional DialogType As MvvmDialogType = MvvmDialogType.Default, Optional validationCallbackAsync As Func(Of Task(Of Boolean)) = Nothing, Optional parameters As Object = Nothing) As MvvmDialogResult Implements IPlatformDependencyService.ShowDialog
        Throw New NotImplementedException()
    End Function

    Public Async Function ShowDialogAsync(Of t As BindableBase)(viewModel As t, Optional dialogTitel As String = Nothing, Optional DialogType As MvvmDialogType = MvvmDialogType.Default, Optional validationCallbackAsync As Func(Of Task(Of Boolean)) = Nothing, Optional parameters As Object = Nothing) As Task(Of MvvmDialogResult) Implements IPlatformDependencyService.ShowDialogAsync

        Dim formToShow As IMvvmForm = Nothing

        If TypeOf viewModel Is BuildingViewModel Then
            formToShow = New NewEditBuildingView
        End If

        If Not String.IsNullOrWhiteSpace(dialogTitel) Then
            formToShow.Self.Text = dialogTitel
        End If

        formToShow.MvvmManager.DataContext = viewModel
        If validationCallbackAsync IsNot Nothing Then
            AddHandler formToShow.Self.FormClosing,
                Async Sub(o, e)
                    e.Cancel = Await validationCallbackAsync.Invoke()
                End Sub
        End If

        Dim dr = formToShow.Self.ShowDialog()
        Await Task.Delay(0)

        Select Case dr
            Case DialogResult.OK
                Return MvvmDialogResult.OK
            Case DialogResult.Cancel
                Return MvvmDialogResult.Cancel
            Case DialogResult.Abort
                Return MvvmDialogResult.Abort
            Case DialogResult.No
                Return MvvmDialogResult.No
            Case DialogResult.Yes
                Return MvvmDialogResult.Yes
            Case DialogResult.Retry
                Return MvvmDialogResult.Retry
            Case DialogResult.Ignore
                Return MvvmDialogResult.Ignore
            Case Else
                Return MvvmDialogResult.None

        End Select
        formToShow.Self.Dispose()

    End Function

    Public Function ShowMessageBox(text As String, Optional caption As String = Nothing, Optional buttons As MvvMessageBoxButtons = MvvMessageBoxButtons.OK, Optional defaultButton As MvvmMessageBoxDefaultButton = MvvmMessageBoxDefaultButton.Button1, Optional icon As MvvmMessageBoxIcon = MvvmMessageBoxIcon.None) As MvvmDialogResult Implements IPlatformDependencyService.ShowMessageBox
        Throw New NotImplementedException()
    End Function

    Public Function ShowMessageBoxAsync(text As String, Optional caption As String = Nothing, Optional buttons As MvvMessageBoxButtons = MvvMessageBoxButtons.OK, Optional defaultButton As MvvmMessageBoxDefaultButton = MvvmMessageBoxDefaultButton.Button1, Optional icon As MvvmMessageBoxIcon = MvvmMessageBoxIcon.None) As Task(Of MvvmDialogResult) Implements IPlatformDependencyService.ShowMessageBoxAsync
        Throw New NotImplementedException()
    End Function
End Class
