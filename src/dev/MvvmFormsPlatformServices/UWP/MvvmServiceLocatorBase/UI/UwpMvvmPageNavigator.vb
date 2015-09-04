Imports ActiveDevelop.IoC.Generic

''' <summary>
''' UWP Implementation of the MvvmPageNavigationService
''' </summary>
Public Class UwpMvvmPageNavigationService
    Implements IMvvmPageNavigationService

    Private Shared myNavigationFrame As Frame

    Public Sub GoBack() Implements IMvvmPageNavigationService.GoBack
        NavigationFrame.GoBack()
    End Sub

    Public Sub GoForward() Implements IMvvmPageNavigationService.GoForward
        NavigationFrame.GoForward()
    End Sub

    Public Sub NavigateTo(pageThroughViewmodel As INotifyPropertyChanged) Implements IMvvmPageNavigationService.NavigateTo
        Dim newPage = UwpMvvmPlatformServiceLocator.ViewModelToPageResolver(pageThroughViewmodel)
        NavigationFrame.Navigate(newPage)
        DirectCast(NavigationFrame.Content, Page).DataContext = pageThroughViewmodel
    End Sub

    Public Function CanGoBack() As Boolean Implements IMvvmPageNavigationService.CanGoBack
        Return NavigationFrame.CanGoBack
    End Function

    Public Function CanGoForward() As Boolean Implements IMvvmPageNavigationService.CanGoForward
        Return NavigationFrame.CanGoForward
    End Function

    Public Shared Property NavigationFrame As Frame
        Get
            Return myNavigationFrame
        End Get
        Friend Set(value As Frame)
            myNavigationFrame = value
        End Set
    End Property
End Class
