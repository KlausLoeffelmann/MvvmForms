Imports ActiveDevelop.MvvmBaseLib.Mvvm
Imports ActiveDevelop.MvvmForms.WebApiClientSupport
Imports MRWebApiSelfHost.DataLayer.DataObjects

Public Class ContactViewModel
    Inherits MvvmBase

    Private myId As Guid
    Private myLastname As String
    Private myFirstname As String
    Private myPhone As String

    Public Shared Async Function GetContactForBuildingAsync(idBuilding As Guid) As Task(Of ContactViewModel)
        Dim getter = New WebApiAccess("http://localhost:9000/", "api")
        Dim contact = Await getter.GetDataAsync(Of ContactItem)(category:="contact",
                                                                params:=idBuilding.ToString)

    End Function

    Public Property id As Guid
        Get
            Return myId
        End Get
        Set(value As Guid)
            SetProperty(myId, value)
        End Set
    End Property

    Public Property Lastname As String
        Get
            Return myLastname
        End Get
        Set(value As String)
            SetProperty(myLastname, value)
        End Set
    End Property

    Public Property Phone As String
        Get
            Return myPhone
        End Get
        Set(value As String)
            SetProperty(myPhone, value)
        End Set
    End Property
End Class
