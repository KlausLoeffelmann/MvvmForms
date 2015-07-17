Imports ArtHorizViewModel
Imports Autofac
Imports Windows.UI.Popups

'Pretend this to be in an UWP.ServiceLocatorBase NuGet
Public Class MvvmPlatformServiceLocator
    Implements IMvvmPlatformServiceLocator

    Public Property ContainerLocator As Func(Of Object) Implements IMvvmPlatformServiceLocator.ContainerLocator

    Public Function Resolve(Of type)() As type Implements IMvvmPlatformServiceLocator.Resolve
        Dim container As IContainer = DirectCast(ContainerLocator.Invoke, IContainer)
        Dim scope = container.BeginLifetimeScope
        Return scope.Resolve(Of type)
    End Function

End Class

