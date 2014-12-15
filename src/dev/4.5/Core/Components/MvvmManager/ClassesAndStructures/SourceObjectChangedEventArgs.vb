Imports System.ComponentModel

Public Class SourceObjectChangedEventArgs
    Inherits CancelEventArgs

    Sub New(originalObject As INotifyPropertyChanged)
        Me.OriginalObject = originalObject
    End Sub

    Public Property OriginalObject As INotifyPropertyChanged

End Class
