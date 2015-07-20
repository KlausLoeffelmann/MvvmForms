Imports ActiveDevelop.IoC.Generic
Imports ActiveDevelop.MvvmBaseLib.Mvvm

Public Class MainViewModel
    Inherits MvvmBase

    Private myPlatformServiceLocator As Func(Of IMvvmPlatformServiceLocator)

    Private myTestCommand As RelayCommand = New RelayCommand(
        Async Sub()
            If myPlatformServiceLocator IsNot Nothing Then
                Dim messageBox = myPlatformServiceLocator().GetMessageBoxService
                Dim result = Await messageBox.ShowAsync("Nachricht", "Titel")
            End If
        End Sub)

    Public Sub New(platformServiceLocator As Func(Of IMvvmPlatformServiceLocator))
        myPlatformServiceLocator = platformServiceLocator
    End Sub

    Public Property TestCommand As RelayCommand
        Get
            Return myTestCommand
        End Get
        Set(value As RelayCommand)
            SetProperty(myTestCommand, value)
        End Set
    End Property

End Class
