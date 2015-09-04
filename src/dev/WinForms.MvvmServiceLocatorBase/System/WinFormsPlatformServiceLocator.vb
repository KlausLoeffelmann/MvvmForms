Imports System.ComponentModel
Imports System.Windows.Forms
Imports ActiveDevelop.IoC.Generic
Imports Autofac


Public Class WinFormsMvvmPlatformServiceLocator
    Implements IMvvmPlatformServiceLocator

    Private Shared mySharedContainer As Autofac.IContainer

    Public Shared Property Container As Autofac.IContainer
        Get
            Return mySharedContainer
        End Get
        Friend Set(value As Autofac.IContainer)
            mySharedContainer = value
        End Set
    End Property

    ''' <summary>
    ''' Centralized Viewsolver for finding Views as Pages based on ViewModels
    ''' </summary>
    ''' <returns></returns>
    Public Shared Property ViewModelToPageResolver As Func(Of INotifyPropertyChanged, Type)

    ''' <summary>
    ''' Centralized View solver for finding Views as ContentDialogs based on ViewModels
    ''' </summary>
    ''' <returns></returns>
    Public Shared Property ViewToDialogResolver As Func(Of INotifyPropertyChanged, Form)

    Private disposedValue As Boolean ' To detect redundant calls
    Private myLifetimeScope As ILifetimeScope

    Sub New()
        MyBase.New
        ContainerLocator = Function() Container
    End Sub

    Public Property ContainerLocator As Func(Of Object) Implements IMvvmPlatformServiceLocator.ContainerLocator

    Public Function GetLifetimeController() As IIoCLifetimeController _
                  Implements IMvvmPlatformServiceLocator.GetLifetimeController
        Dim container As Autofac.IContainer = DirectCast(ContainerLocator.Invoke, Autofac.IContainer)
        myLifetimeScope = container.BeginLifetimeScope
        Return New WinFormsIoCLifetimeController(myLifetimeScope)
    End Function

    Public Shared Function BeginRegister() As ContainerBuilder
        Dim builder = New ContainerBuilder
        Return builder
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
