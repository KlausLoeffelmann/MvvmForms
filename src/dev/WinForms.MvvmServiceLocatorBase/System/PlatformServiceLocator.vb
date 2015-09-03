Imports ActiveDevelop.IoC.Generic
Imports Autofac

Public Class MvvmPlatformServiceLocator
    Implements IMvvmPlatformServiceLocator

    Private disposedValue As Boolean ' To detect redundant calls
    Private myLifetimeScope As ILifetimeScope

    Public Property ContainerLocator As Func(Of Object) Implements IMvvmPlatformServiceLocator.ContainerLocator

    Public Function Resolve(Of type)() As type Implements IMvvmPlatformServiceLocator.Resolve
        Dim container As IContainer = DirectCast(ContainerLocator.Invoke, IContainer)
        myLifetimeScope = container.BeginLifetimeScope
        Return myLifetimeScope.Resolve(Of type)
    End Function


    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not disposedValue Then
            If disposing Then
                myLifetimeScope.Dispose()
            End If

            ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
            ' TODO: set large fields to null.
        End If
        disposedValue = True
    End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
        Dispose(True)
    End Sub

End Class
