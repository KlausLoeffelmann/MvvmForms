Imports System.Runtime.CompilerServices
Imports System.Windows.Forms
Imports ActiveDevelop.IoC.Generic
Imports Autofac

Public Module WinFormsPlatformServiceLocatorContainerBuilderExtender

    <Extension>
    Public Function RegisterPlatformServiceLocator(builder As ContainerBuilder, mainWindow As Form) As ContainerBuilder
        builder.RegisterType(Of WinFormsMvvmPlatformServiceLocator).As(Of IMvvmPlatformServiceLocator)()
        builder.RegisterType(Of WinFormsMvvmPageNavigationService).As(Of IMvvmPageNavigationService)()
        builder.RegisterType(Of WinFormsMvvmMessageBox).As(Of IMvvmMessageBox)()
        WinFormsMvvmPageNavigationService.MainWindow = mainWindow
        Return builder
    End Function

    <Extension>
    Public Sub EndRegister(builder As ContainerBuilder)
        WinFormsMvvmPlatformServiceLocator.Container = builder.Build
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