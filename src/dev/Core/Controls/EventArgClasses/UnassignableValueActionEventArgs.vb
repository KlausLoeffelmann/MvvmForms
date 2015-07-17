Imports System.ComponentModel

Public Class UnassignableValueDetectedEventArgs
    Inherits EventArgs

    Sub New(ByVal exceptionHandled As Boolean, ByVal newValue As Object)
        MyBase.New()
        Me.ExceptionHandled = exceptionHandled
        Me.NewValue = newValue
    End Sub

    Property ExceptionHandled As Boolean
    Property NewValue As Object

End Class