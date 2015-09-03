Imports ActiveDevelop.IoC.Authentication
Imports Microsoft.WindowsAzure.MobileServices

Imports Windows.Security.Credentials

Public Class MvvmAzureMobileClientService
    Implements IMvvmAzureMobileClientService

    Private myMobileClient As MobileServiceClient

    Public Property MobileClient As MobileServiceClient Implements IMvvmAzureMobileClientService.MobileClient
        Get
            Return myMobileClient
        End Get
        Set(value As Microsoft.WindowsAzure.MobileServices.MobileServiceClient)
            myMobileClient = value
        End Set
    End Property

    Public Async Function LoginAsync() As Task(Of MobileServiceUser) Implements IMvvmAzureMobileClientService.LoginAsync
        Return Await MobileClient.LoginAsync(MobileServiceAuthenticationProvider.MicrosoftAccount)
    End Function
End Class
