Imports Microsoft.WindowsAzure.MobileServices

Public Interface IMvvmAzureMobileClientService
    Property MobileClient As MobileServiceClient
    Function LoginAsync() As Task(Of MobileServiceUser)

End Interface
