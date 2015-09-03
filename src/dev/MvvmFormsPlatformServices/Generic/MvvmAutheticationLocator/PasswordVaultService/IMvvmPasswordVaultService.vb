Imports Microsoft.WindowsAzure.MobileServices

Public Interface IMvvmPasswordVaultService
    Sub StoreInVault(resource As String, user As MobileServiceUser)
    Function GetFromVault(resource As String) As MobileServiceUser
    Function RemoveFromVault(resource As String, user As MobileServiceUser) As Boolean

End Interface
