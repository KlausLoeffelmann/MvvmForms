Imports ActiveDevelop.Mvvmbase

Public Class BuildingViewModel
    Inherits TempBase

    Private myId As Guid
    Private myOwner As ContactViewModel
    Private myBuildYear As Integer
    Private myLocationAddressLine1 As String
    Private myLocationAddressLine2 As String
    Private myCity As String
    Private myZip As String
    Private myCountry As String

    Public Property id As Guid
        Get
            Return myId
        End Get
        Set(value As Guid)
            SetProperty(myId, value)
        End Set
    End Property

    Public Property Owner As ContactViewModel
        Get
            Return myOwner
        End Get
        Set(value As ContactViewModel)
            SetProperty(myOwner, value)
        End Set
    End Property

    Public Property BuildYear As Integer
        Get
            Return myBuildYear
        End Get
        Set(value As Integer)
            SetProperty(myBuildYear, value)
        End Set
    End Property

    Public Property LocationAddressLine1 As String
        Get
            Return myLocationAddressLine1
        End Get
        Set(value As String)
            SetProperty(myLocationAddressLine1, value)
        End Set
    End Property

    Public Property LocationAddressLine2 As String
        Get
            Return myLocationAddressLine2
        End Get
        Set(value As String)
            SetProperty(myLocationAddressLine2, value)
        End Set
    End Property

    Public Property City As String
        Get
            Return myCity
        End Get
        Set(value As String)
            SetProperty(myCity, value)
        End Set
    End Property

    Public Property Zip As String
        Get
            Return myZip
        End Get
        Set(value As String)
            SetProperty(myZip, value)
        End Set
    End Property

    Public Property Country As String
        Get
            Return myCountry
        End Get
        Set(value As String)
            SetProperty(myCountry, value)
        End Set
    End Property

End Class

Public Class TempBase

    Public Function SetProperty(Of t)(ByRef storage As t, value As t,
                                      <CallerMemberName> Optional propertyName As String = Nothing) As Boolean

    End Function
End Class

