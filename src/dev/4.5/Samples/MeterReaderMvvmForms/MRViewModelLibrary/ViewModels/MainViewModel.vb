Imports ActiveDevelop.MvvmBaseLib
Imports ActiveDevelop.MvvmBaseLib.Mvvm

Public Class MainViewModel
    Inherits MvvmBase

    '*** BACKING FIELDS ***
    Private myBuildings As ObservableCollection(Of BuildingViewModel)
    Private mySelectedBuilding As BuildingViewModel
    Private myMeters As BindableAsyncLazy(Of ObservableCollection(Of MeterViewModel)) =
        New BindableAsyncLazy(Of ObservableCollection(Of MeterViewModel))(
            Async Function(param As Object) As Task(Of ObservableCollection(Of MeterViewModel))
                If param Is Nothing OrElse CType(param, Guid) = Guid.Empty Then
                    Return Nothing
                Else
                    Return Await MeterViewModel.GetMetersForBuildingAsync(CType(param, Guid))
                End If
            End Function, Nothing)

    Private myNewBuildingCommand As New RelayCommand(AddressOf NewBuildingCommandProc,
                                                     Function() True)

    '*** BINDABLE PROPERTIES  ***

    Public Property Buildings As ObservableCollection(Of BuildingViewModel)
        Get
            Return myBuildings
        End Get
        Set(value As ObservableCollection(Of BuildingViewModel))
            SetProperty(myBuildings, value)
        End Set
    End Property

    Public Property SelectedBuilding As BuildingViewModel
        Get
            Return mySelectedBuilding
        End Get
        Set(value As BuildingViewModel)
            If SetProperty(mySelectedBuilding, value) Then
                If value IsNot Nothing Then
                    MetersForBuilding.Param = value.id
                Else
                    Me.MetersForBuilding.Param = Guid.Empty
                End If
            End If
        End Set
    End Property

    Public Property MetersForBuilding As BindableAsyncLazy(Of ObservableCollection(Of MeterViewModel))
        Get
            Return myMeters
        End Get
        Set(value As BindableAsyncLazy(Of ObservableCollection(Of MeterViewModel)))
            SetProperty(myMeters, value)
        End Set
    End Property

    '*** COMMANDS ***

    Public Property NewBuildingCommand As RelayCommand
        Get
            Return myNewBuildingCommand
        End Get
        Set(value As RelayCommand)
            SetProperty(myNewBuildingCommand, value)
        End Set
    End Property

    Private Async Sub NewBuildingCommandProc(obj As Object)

        Dim newId = Await BuildingViewModel.GetNextId

        Dim buildingVm As New BuildingViewModel With {.idNum = newId,
                                                      .id = Guid.NewGuid}

        Dim dr = Await DependencyService.ShowDialogAsync(buildingVm, "New Building")
        If dr = MvvmDialogResult.OK Then
            'TODO: Create new Building via Web Api.
            Me.Buildings.Add(buildingVm)
        End If
    End Sub

    '*** IoC ***

    Public Property DependencyService As IPlatformDependencyService

End Class
