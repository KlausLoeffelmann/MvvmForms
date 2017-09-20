Imports ActiveDevelop.MvvmBaseLib
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Threading.Tasks

Public Class TestViewModelSender
    Inherits BindableBase

    Public Sub CallPropertyChanged()
        OnPropertyChanged("Test")

    End Sub

End Class
