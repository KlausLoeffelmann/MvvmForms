Public Class ContactViewModel
    Inherits TempBase

    Private myId As Guid
    Private myLastname As String
    Private myFirstname As String
    Private myPhone As String

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
