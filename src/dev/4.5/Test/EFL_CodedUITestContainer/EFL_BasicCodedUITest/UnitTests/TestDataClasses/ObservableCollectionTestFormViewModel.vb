Imports System.Collections.ObjectModel
Imports ActiveDevelop.MvvmBaseLib.Mvvm

Public Class ObservableCollectionTestFormViewModel
    Inherits MvvmBase

    Private myList As ObservableCollection(Of ContactViewModel)
    Private myGenerateListCommand As RelayCommand = New RelayCommand(
        Sub()
            List = ContactViewModel.GetTestData(100)
        End Sub,
        Function() As Boolean
            Return True
        End Function)

    '--- GenerateListCommand---
    Public Property GenerateListCommand As RelayCommand
        Get
            Return myGenerateListCommand
        End Get
        Set(value As RelayCommand)
            SetProperty(myGenerateListCommand, value)
        End Set
    End Property

    '--- List---
    Public Property List As ObservableCollection(Of ContactViewModel)
        Get
            Return myList
        End Get
        Set(value As ObservableCollection(Of ContactViewModel))
            SetProperty(myList, value)
        End Set
    End Property
End Class
