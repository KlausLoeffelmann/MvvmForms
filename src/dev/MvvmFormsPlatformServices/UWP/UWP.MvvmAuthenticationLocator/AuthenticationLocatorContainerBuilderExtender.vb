Imports ActiveDevelop.IoC.Authentication
Imports Autofac

Public Module AuthenticationLocatorContainerBuilderExtender

    <Extension>
    Public Function RegisterAuthenticationLocator(builder As ContainerBuilder) As ContainerBuilder
        builder.RegisterType(Of MvvmAzureMobileClientService).As(Of IMvvmAzureMobileClientService)()
        builder.RegisterType(Of MvvmPasswordVaultService).As(Of IMvvmPasswordVaultService)()
        Return builder
    End Function


End Module
