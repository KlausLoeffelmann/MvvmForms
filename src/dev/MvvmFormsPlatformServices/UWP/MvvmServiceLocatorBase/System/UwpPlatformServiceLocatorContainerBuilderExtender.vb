Imports ActiveDevelop.IoC.Generic
Imports Autofac

Public Module UwpPlatformServiceLocatorContainerBuilderExtender

    <Extension>
    Public Function RegisterPlatformServiceLocator(builder As ContainerBuilder, navigationFrame As Frame) As ContainerBuilder
        builder.RegisterType(Of UwpMvvmPlatformServiceLocator).As(Of IMvvmPlatformServiceLocator)()
        builder.RegisterType(Of UwpMvvmPageNavigationService).As(Of IMvvmPageNavigationService)()
        builder.RegisterType(Of UwpMvvmMessageBox).As(Of IMvvmMessageBox)()
        UwpMvvmPageNavigationService.NavigationFrame = navigationFrame
        Return builder
    End Function

    <Extension>
    Public Sub EndRegister(builder As ContainerBuilder)
        UwpMvvmPlatformServiceLocator.Container = builder.Build
    End Sub

    <Extension>
    Public Function RegisterTypes(builder As ContainerBuilder,
                                     viewModelList As IEnumerable(Of Type)) As ContainerBuilder
        For Each item In viewModelList
            builder.RegisterType(item)
        Next
        Return builder
    End Function

End Module