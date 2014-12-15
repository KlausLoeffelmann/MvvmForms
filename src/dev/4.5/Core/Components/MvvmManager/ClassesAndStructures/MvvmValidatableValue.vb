Imports ActiveDevelop.EntitiesFormsLib.ViewModelBase

<MvvmViewModel, MvvmSystemElement>
Public Class MVVMValidatableValue(Of t As IComparable)
    Inherits BindableBase

    Private myValue As t
    Private myValidationError As Integer
    Private myInternalPropertyChangeAction As Action =
        Sub()
            If ShouldCallGlobalValidator AndAlso Validator Is Nothing AndAlso GlobalValidator IsNot Nothing Then
                ValidationError = GlobalValidator.Invoke()
            Else
                If Validator IsNot Nothing Then
                    ValidationError = Validator.Invoke()
                End If
            End If

            If PropertyChangeAction IsNot Nothing Then
                PropertyChangeAction.Invoke()
            End If
        End Sub

    Sub New(Optional Validator As Func(Of Integer) = Nothing,
            Optional propertyChangeAction As Action = Nothing)
        MyBase.New()
        Me.Validator = Validator
        Me.PropertyChangeAction = propertyChangeAction
    End Sub

    Sub New(initialValue As t,
                Optional validator As Func(Of Integer) = Nothing,
                Optional propertyChangeAction As Action = Nothing)
        myValue = initialValue
        Me.Validator = validator
        Me.PropertyChangeAction = propertyChangeAction
    End Sub

    Property Value As t
        Get
            Return myValue
        End Get
        Set(value As t)
            If MyBase.SetProperty(myValue, value) Then
                myInternalPropertyChangeAction()
            End If
        End Set
    End Property

    Property ValidationError As Integer
        Get
            Return myValidationError
        End Get
        Set(value As Integer)
            MyBase.SetProperty(myValidationError, value)
        End Set
    End Property

    Property Validator As Func(Of Integer)
    Property PropertyChangeAction As Action

    Public Shared Property ShouldCallGlobalValidator As Boolean = False
    Public Shared Property GlobalValidator As Func(Of Integer)

End Class
