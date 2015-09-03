Imports ActiveDevelop.IoC.Authentication
Imports Microsoft.WindowsAzure.MobileServices
Imports Windows.Security.Credentials

Public Class MvvmPasswordVaultService
    Implements IMvvmPasswordVaultService

    Dim myVault As New PasswordVault

    Public Sub StoreInVault(resource As String, user As MobileServiceUser) Implements IMvvmPasswordVaultService.StoreInVault

        Dim credential As New PasswordCredential(resource,
                                                 user.UserId,
                                                 user.MobileServiceAuthenticationToken)
        myVault.Add(credential)
    End Sub

    Public Function GetFromVault(resource As String) As MobileServiceUser Implements IMvvmPasswordVaultService.GetFromVault
        Dim credentials As PasswordCredential
        Try
            credentials = myVault.FindAllByResource(resource).FirstOrDefault
        Catch ex As Exception
            credentials = Nothing
        End Try

        If credentials Is Nothing Then
            Return Nothing
        Else
            Dim temp = New MobileServiceUser(credentials.UserName)
            temp.MobileServiceAuthenticationToken = myVault.Retrieve(resource, credentials.UserName).Password
            Return temp
        End If
    End Function

    Public Function RemoveFromVault(resource As String, user As MobileServiceUser) As Boolean Implements IMvvmPasswordVaultService.RemoveFromVault
        Try
            Dim credential As New PasswordCredential(resource,
                                                     user.UserId,
                                                     user.MobileServiceAuthenticationToken)
            myVault.Remove(credential)
        Catch ex As Exception
            Return False
        End Try
        Return True
    End Function
End Class
