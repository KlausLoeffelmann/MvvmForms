Public Interface IMvvmPlatformServiceLocator
    Inherits IDisposable

    Property ContainerLocator As Func(Of Object)

    Function Resolve(Of type)() As type

End Interface
