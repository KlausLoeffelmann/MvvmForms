Imports ActiveDevelop.IoC.Generic
Imports Autofac

Public Class UwpIoCLifetimeController
    Implements IIoCLifetimeController

    Private myLifetimeScope As ILifetimeScope

    Sub New(lifetimeScope As ILifetimeScope)
        myLifetimeScope = lifetimeScope
    End Sub

    Public Function Resolve(Of t)() As t Implements IIoCLifetimeController.Resolve
        Return myLifetimeScope.Resolve(Of t)
    End Function
End Class

