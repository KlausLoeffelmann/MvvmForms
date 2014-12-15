Imports System.Windows.Forms
Imports System.ComponentModel

Public Class ValueAssigningEventArgs
    Inherits CancelEventArgs

    Sub New()
        MyBase.New()
    End Sub

    Sub New(cancel As Boolean)
        MyBase.New(cancel)
    End Sub

    Property Control As Object
    Property ViewModelObject As Object
    Property ControlPropertyName As String
    Property ViewModelPropertyName As String
    Property Value As Object
    Property Target As Targets

End Class

Public Class ValueAssignedEventArgs
    Inherits EventArgs

    Private myValue As Object

    Sub New()
        MyBase.New()
    End Sub

    Sub New(e As ValueAssigningEventArgs)
        Me.Control = e.Control
        Me.ViewModelObject = e.ViewModelObject
        Me.ControlPropertyName = e.ControlPropertyName
        Me.ViewModelPropertyName = e.ViewModelPropertyName
        Me.Value = e.Value
        Me.Target = e.Target
    End Sub

    Property Control As Object
    Property ViewModelObject As Object
    Property ControlPropertyName As String
    Property ViewModelPropertyName As String
    Property Value As Object
        Get
            Return myvalue
        End Get
        Private Set(value As Object)
            myValue = value
        End Set
    End Property
    Property Target As Targets

End Class

Public Enum Targets
    Control
    ViewModel
End Enum
