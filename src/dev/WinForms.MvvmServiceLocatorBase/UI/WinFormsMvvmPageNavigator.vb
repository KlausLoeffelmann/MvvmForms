Imports System.ComponentModel
Imports System.Windows.Forms
Imports ActiveDevelop.IoC.Generic

''' <summary>
''' UWP Implementation of the MvvmPageNavigationService
''' </summary>
Public Class WinFormsMvvmPageNavigationService
    Implements IMvvmPageNavigationService

    Private Shared myMainWindow As Form

    Public Sub GoBack() Implements IMvvmPageNavigationService.GoBack
        Throw New NotImplementedException("Windows Forms does not support Go-Back-Navigation.")
    End Sub

    Public Sub GoForward() Implements IMvvmPageNavigationService.GoForward
        Throw New NotImplementedException("Windows Forms does not support Go-Forward-Navigation.")
    End Sub

    Public Sub NavigateTo(pageThroughViewmodel As INotifyPropertyChanged) Implements IMvvmPageNavigationService.NavigateTo
        Throw New NotImplementedException("Not implemented yet.")
    End Sub

    Public Function CanGoBack() As Boolean Implements IMvvmPageNavigationService.CanGoBack
        Throw New NotImplementedException("Not implemented yet.")
    End Function

    Public Function CanGoForward() As Boolean Implements IMvvmPageNavigationService.CanGoForward
        Throw New NotImplementedException("Windows Forms does not support Go-Forward-Navigation.")
    End Function

    Public Shared Property MainWindow As Form
        Get
            Return myMainWindow
        End Get
        Friend Set(value As Form)
            myMainWindow = value
        End Set
    End Property
End Class
