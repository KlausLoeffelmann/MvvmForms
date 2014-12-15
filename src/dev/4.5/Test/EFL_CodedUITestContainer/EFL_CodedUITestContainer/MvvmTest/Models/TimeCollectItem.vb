Imports ActiveDevelop.EntitiesFormsLib
Imports ActiveDevelop.EntitiesFormsLib.ViewModelBase

<Serializable(), BusinessClass(ActiveDevelop.EntitiesFormsLib.BusinessClassAttributeOptions.IncludeAllPropertiesByDefault)>
Public Class TimeCollectItems
    Inherits ObservableBindingList(Of TimeCollectItem)

End Class

<Serializable(), BusinessClass(ActiveDevelop.EntitiesFormsLib.BusinessClassAttributeOptions.IncludeAllPropertiesByDefault)>
Public Class TimeCollectItem
    Inherits BindableBase

    Private myStartTime As DateTime?
    Private myEndTime As DateTime?
    Private myDuration As TimeSpan?
    Private myActivityDescription As String

    Public Property StartTime As DateTime?
        Get
            Return myStartTime
        End Get
        Set(value As DateTime?)
            If MyBase.SetProperty(myStartTime, value) Then
                CalculateDuration()
            End If
        End Set
    End Property

    Public Property EndTime As DateTime?
        Get
            Return myEndTime
        End Get
        Set(value As DateTime?)
            If MyBase.SetProperty(myEndTime, value) Then
                CalculateDuration()
            End If
        End Set
    End Property

    Public Property Duration As TimeSpan?
        Get
            Return myDuration
        End Get
        Private Set(value As TimeSpan?)
            MyBase.SetProperty(myDuration, value)
        End Set
    End Property

    Public Property ActivityDescription As String
        Get
            Return myActivityDescription
        End Get
        Set(value As String)
            MyBase.SetProperty(myActivityDescription, value)
        End Set
    End Property

    Private Sub CalculateDuration()
        Me.Duration = EndTime - StartTime
    End Sub


    Public Overrides Function ToString() As String
        Return StartTime.ToString & " - " & EndTime.ToString
    End Function

End Class
