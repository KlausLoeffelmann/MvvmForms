Public Module MvvmUiPlatformServiceLocatorExtender

    <Extension>
    Public Function GetMessageBoxService(instanceHolder As IIoCLifetimeController) As IMvvmMessageBox
        Return instanceHolder.Resolve(Of IMvvmMessageBox)()
    End Function

    <Extension>
    Public Function GetNavigationService(instanceHolder As IIoCLifetimeController) As IMvvmPageNavigationService
        Return instanceHolder.Resolve(Of IMvvmPageNavigationService)()
    End Function

End Module
