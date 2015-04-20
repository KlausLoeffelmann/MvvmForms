Imports ActiveDevelop.MvvmBaseLib.Mvvm

Public Class MeterViewModel
    Inherits MvvmBase

    Private myId As Guid
    Private myBelongsTo As BuildingViewModel
    Private myMeterId As String
    Private myLocalDescription As String

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
