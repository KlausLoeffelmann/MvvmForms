Public Module MvvmUiPlatformServiceLocatorExtender

    <Extension>
    Public Function GetMessageBoxService(platformServiceLocator As IMvvmPlatformServiceLocator) As IMvvmMessageBox
        Return platformServiceLocator.Resolve(Of IMvvmMessageBox)()
    End Function

End Module
