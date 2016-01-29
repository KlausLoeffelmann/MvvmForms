Imports System.Collections.ObjectModel
Imports ActiveDevelop.MvvmBaseLib.Mvvm

''' <summary>
''' Bindet einige Properties erst 2 Sekunden nach Aufruf von "LoadAsync()"
''' </summary>
Public Class LateBindingViewModel
    Inherits MvvmViewModelBase

    Private myObsCollection As ObservableCollection(Of String)
    Private mySelectedItem As String

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>Bindbare Property</remarks>
    Public Property ObsCollection As ObservableCollection(Of String)
        Get
            Return myObsCollection
        End Get
        Set(ByVal value As ObservableCollection(Of String))
            MyBase.SetProperty(myObsCollection, value)
        End Set
    End Property

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>Bindbare Property</remarks>
    Public Property SelectedItem As String
        Get
            Return mySelectedItem
        End Get
        Set(ByVal value As String)
            MyBase.SetProperty(mySelectedItem, value)
        End Set
    End Property



    Public Async Function LoadAsync() As Threading.Tasks.Task
        Await Threading.Tasks.Task.Delay(2000)

        Dim rnd = New Random().Next(0, 19)
        Me.ObsCollection = New ObservableCollection(Of String)({"0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19"})
        Me.SelectedItem = ObsCollection(rnd)

    End Function


End Class
