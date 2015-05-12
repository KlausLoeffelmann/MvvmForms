Imports ActiveDevelop.MvvmBaseLib.Mvvm
Imports ActiveDevelop.MvvmForms.WebApiClientSupport
Imports MrModelLibrary.DataObjects

Public Class MeterViewModel
    Inherits MvvmBase

    Private myId As Guid
    Private myBelongsTo As BuildingViewModel
    Private myMeterId As String
    Private myLocationDescription As String

    Public Shared Async Function GetMetersForBuildingAsync(idBuilding As Guid) As Task(Of IEnumerable(Of MeterViewModel))

        Dim getter = New WebApiAccess("http://localhost:9000/", "api")
        Dim meterModels = Await getter.GetDataAsync(Of IEnumerable(Of MeterItem))(category:="meter",
                                                                                  params:=idBuilding.ToString)
        Dim meters = New ObservableCollection(Of MeterViewModel)(
            MeterViewModel.FromModelList(Of MeterViewModel, MeterItem)(meterModels))

        Return meters

    End Function

    Public Property id As Guid
        Get
            Return myId
        End Get
        Set(value As Guid)
            SetProperty(myId, value)
        End Set
    End Property

    Public Property MeterId As String
        Get
            Return myMeterId
        End Get
        Set(value As String)
            SetProperty(myMeterId, value)
        End Set
    End Property

    Public Property LocationDescription As String
        Get
            Return myLocationDescription
        End Get
        Set(value As String)
            SetProperty(myLocationDescription, value)
        End Set
    End Property

    Public Property EntryPadding As Integer = 3
End Class
