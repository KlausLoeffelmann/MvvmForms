Imports ActiveDevelop.MvvmBaseLib.Mvvm
Imports ActiveDevelop.MvvmForms.WebApiClientSupport
Imports MRWebApiSelfHost.DataLayer.DataObjects

Public Class MeterViewModel
    Inherits MvvmBase

    Private myId As Guid
    Private myBelongsTo As BuildingViewModel
    Private myMeterId As String
    Private myLocalDescription As String

    Public Shared Async Function GetMetersForBuildingAsync(idBuilding As Guid) As Task(Of IEnumerable(Of MeterViewModel))

        Dim getter = New WebApiAccess("http://localhost:9000/", "api")
        Dim meterModels = Await getter.GetDataAsync(Of IEnumerable(Of MeterItem))(category:="building",
                                                                                  params:=idBuilding.ToString)

        Dim meters = New ObservableCollection(Of MeterViewModel)
        For Each item In meterModels
            Dim meterVm = New MeterViewModel
            meterVm.CopyPropertiesFrom(item)
        Next

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

    Public Property LocalDescription As String
        Get
            Return myLocalDescription
        End Get
        Set(value As String)
            SetProperty(myLocalDescription, value)
        End Set
    End Property
End Class
