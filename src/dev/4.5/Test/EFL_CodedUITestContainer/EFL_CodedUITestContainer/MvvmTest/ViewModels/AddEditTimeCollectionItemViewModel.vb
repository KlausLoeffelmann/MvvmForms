Imports ActiveDevelop.EntitiesFormsLib
Imports ActiveDevelop.EntitiesFormsLib.ViewModelBase

<MvvmView("EFL_CodedUITestContainer.AddEditTimeCollectionItemView", "EFL_CodedUITestContainer", MvvmManager.DEFAULT_CONTEXT_GUID)>
Public Class AddEditTimeCollectionItemViewModel
    Inherits MvvmViewModelBase
    Implements IMvvmViewModelForModalDialog

    Private myIsDirty As Boolean
    Private myIsOkEnabled As Boolean
    Private myTimeCollectItem As TimeCollectItem
    Private myVorname As String

    Private myStartTime As TimeSpan?
    Private myEndTime As TimeSpan?
    Private myStartTimeDate As Date?
    Private myEndTimeDate As Date?

    Private myStartTimeValidated As String

    Private myIntTestValue As Integer

    Public Property Andreas As MVVMValidatableValue(Of String) =
        New MVVMValidatableValue(Of String) With {.Value = "Andreas",
                                                  .ValidationError = 0}

    Public Property Vorname As String
        Get
            Return myVorname
        End Get
        Set(value As String)
            MyBase.SetProperty(myVorname, value)
        End Set
    End Property

    Public Property IsDirty As Boolean
        Get
            Return myIsDirty
        End Get
        Set(value As Boolean)
            MyBase.SetProperty(myIsDirty, value)
        End Set
    End Property

    Public Property IsOKEnabled As Boolean
        Get
            Return myIsOkEnabled
        End Get
        Set(value As Boolean)
            MyBase.SetProperty(myIsOkEnabled, value)
        End Set
    End Property

    Public Property StartTime As TimeSpan?
        Get
            Return If(myTimeCollectItem.StartTime.HasValue, myTimeCollectItem.StartTime.Value.TimeOfDay, Nothing)
        End Get
        Set(value As TimeSpan?)
            MyBase.SetProperty(myStartTime, value)
            If value.HasValue Then
                myTimeCollectItem.StartTime = If(myStartTime.HasValue, myStartTimeDate.Value.Add(value.Value), Date.Now.Add(value.Value))
            Else
                myTimeCollectItem.StartTime = Nothing
            End If
        End Set
    End Property

    Public Property StartTimeValidated As String
        Get
            Return myStartTimeValidated
        End Get
        Set(value As String)
            MyBase.SetProperty(myStartTimeValidated, value)
        End Set
    End Property

    Public Property EndTime As TimeSpan?
        Get
            Return If(myTimeCollectItem.EndTime.HasValue, myTimeCollectItem.EndTime.Value.TimeOfDay, Nothing)
        End Get
        Set(value As TimeSpan?)
            MyBase.SetProperty(myEndTime, value)
            If value.HasValue Then
                myTimeCollectItem.EndTime = If(myEndTime.HasValue, myEndTimeDate.Value.Add(value.Value), Date.Now.Add(value.Value))
            Else
                myTimeCollectItem.EndTime = Nothing
            End If
        End Set
    End Property

    Public Property TimeCollectItem As TimeCollectItem
        Get
            Return myTimeCollectItem
        End Get
        Set(value As TimeCollectItem)
            myTimeCollectItem = value

            myStartTimeDate = If(myTimeCollectItem.StartTime.HasValue, myTimeCollectItem.StartTime.Value.Date, Nothing)
            myEndTimeDate = If(myTimeCollectItem.EndTime.HasValue, myTimeCollectItem.EndTime.Value.Date, Nothing)

            Me.StartTime = If(myTimeCollectItem.StartTime.HasValue, myTimeCollectItem.StartTime.Value.TimeOfDay, Nothing)
            Me.EndTime = If(myTimeCollectItem.EndTime.HasValue, myTimeCollectItem.EndTime.Value.TimeOfDay, Nothing)
        End Set
    End Property

    Public Property IntTestValue As Integer
        Get
            Return myIntTestValue
        End Get
        Set(value As Integer)
            SetProperty(myIntTestValue, value)
        End Set
    End Property

    <MvvmContextGuid()>
    Public ReadOnly Property ContextGuid As Guid
        Get
            Return New Guid(MvvmManager.DEFAULT_CONTEXT_GUID)
        End Get
    End Property


    Protected Overrides Sub InitializeViewModel()

    End Sub

    Public Property DialogReturnValue As MvvmMessageBoxReturnValue Implements IMvvmViewModelForModalDialog.DialogReturnValue
End Class
