''' <summary>
''' Generic Navigationservice which can be retrieved through <see cref="IMvvmPlatformServiceLocator"/>.
''' </summary>
Public Interface IMvvmPageNavigationService
    Function CanGoBack() As Boolean
    Sub GoBack()
    Function CanGoForward() As Boolean
    Sub GoForward()
    Sub NavigateTo(pageThroughViewmodel As INotifyPropertyChanged)

End Interface
