Imports ActiveDevelop.MvvmBaseLib.Mvvm

Public Class ReadingViewModel
    Inherits MvvmBase

    Private myId As Guid
    Private myMeter As MeterViewModel
    Private myValue As Decimal
    Private myReadingDate As DateTimeOffset

    Public Property Id As Guid
        Get
            Return myId
        End Get
        Set(value As Guid)
            SetProperty(myId, value)
        End Set
    End Property

    Public Property Meter As MeterViewModel
        Get
            Return myMeter
        End Get
        Set(value As MeterViewModel)
            SetProperty(myMeter, value)
        End Set
    End Property

    Public Property Value As Decimal
        Get
            Return myValue
        End Get
        Set(value As Decimal)
            SetProperty(myValue, value)
        End Set
    End Property

    Public Property ReadingDate As DateTimeOffset
        Get
            Return myReadingDate
        End Get
        Set(value As DateTimeOffset)
            SetProperty(myReadingDate, value)
        End Set
    End Property

End Class
