Imports ActiveDevelop.MvvmBaseLib
Imports ActiveDevelop.MvvmBaseLib.Mvvm
Imports ActiveDevelop.MvvmForms.WebApiClientSupport
Imports MRWebApiSelfHost.DataLayer.DataObjects

Public Class BuildingViewModel
    Inherits MvvmBase

    Private myId As Guid
    Private myIdNum As Integer
    Private myDescription As String
    Private myBuildYear As Integer
    Private myLocationAddressLine1 As String
    Private myLocationAddressLine2 As String
    Private myCity As String
    Private myZip As String
    Private myCountry As String

    Private myOwner As BindableAsyncLazy(Of ContactViewModel) =
        New BindableAsyncLazy(Of ContactViewModel)(
            Async Function(param As Object) As Task(Of ContactViewModel)
                Return Await ContactViewModel.GetContactForBuildingAsync(Me.id)
            End Function, Nothing)

    Public Shared Async Function GetAllBuildings() As Task(Of IEnumerable(Of BuildingViewModel))

        Dim getter = New WebApiAccess("http://localhost:9000/", "api")
        Dim buildingModels = Await getter.GetDataAsync(Of IEnumerable(Of BuildingItem))(category:="building")

        Dim buildings = New ObservableCollection(Of BuildingViewModel)
        For Each item In buildingModels
            Dim buildingVm = New BuildingViewModel
            buildingVm.CopyPropertiesFrom(item)
            buildings.Add(buildingVm)
        Next

        Return buildings

    End Function

    Public Property id As Guid
        Get
            Return myId
        End Get
        Set(value As Guid)
            SetProperty(myId, value)
        End Set
    End Property

    Public Property idNum As Integer
        Get
            Return myIdNum
        End Get
        Set(value As Integer)
            SetProperty(myIdNum, value)
        End Set
    End Property

    Public Property Description As String
        Get
            Return myDescription
        End Get
        Set(value As String)
            SetProperty(myDescription, value)
        End Set
    End Property

    Public Property Owner As BindableAsyncLazy(Of ContactViewModel)
        Get
            Return myOwner
        End Get
        Set(value As BindableAsyncLazy(Of ContactViewModel))
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
