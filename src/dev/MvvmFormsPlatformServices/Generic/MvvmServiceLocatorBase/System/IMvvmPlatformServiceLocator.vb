Public Interface IIoCLifetimeController
    Function Resolve(Of t)() As t
End Interface


Public Interface IMvvmPlatformServiceLocator
    Inherits IDisposable

    Property ContainerLocator As Func(Of Object)
    Function GetLifetimeController() As IIoCLifetimeController

End Interface
