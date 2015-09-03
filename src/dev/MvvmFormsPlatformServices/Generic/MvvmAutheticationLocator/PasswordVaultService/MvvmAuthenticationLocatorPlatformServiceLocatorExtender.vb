Imports ActiveDevelop.IoC.Generic

Public Module MvvmAuthenticationLocatorPlatformServiceLocatorExtender

    <Extension>
    Public Function GetPasswordVaultService(instanceHolder As IIoCLifetimeController) As IMvvmPasswordVaultService
        Return instanceHolder.Resolve(Of IMvvmPasswordVaultService)()
    End Function

    <Extension>
    Public Function GetAzureMobileClientService(instanceHolder As IIoCLifetimeController) As IMvvmAzureMobileClientService
        Return instanceHolder.Resolve(Of IMvvmAzureMobileClientService)()
    End Function

End Module
