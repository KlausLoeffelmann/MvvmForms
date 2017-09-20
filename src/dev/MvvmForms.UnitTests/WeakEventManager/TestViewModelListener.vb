
Imports System.ComponentModel
Imports ActiveDevelop.MvvmBaseLib.Mvvm

Public Class TestViewModelListener

    Property HandlerCalled As Boolean = False

    Public Sub AddHandlerWeak(target As TestViewModelSender)
        WeakEventManager.AddHandler(Of PropertyChangedEventArgs)(target, AddressOf Target_PropertyChanged, NameOf(target.PropertyChanged))
    End Sub


    Public Sub RemoveHandlerWeak(target As TestViewModelSender)
        WeakEventManager.RemoveHandler(Of PropertyChangedEventArgs)(target, AddressOf Target_PropertyChanged, NameOf(target.PropertyChanged))
    End Sub


    Private Sub Target_PropertyChanged(sender As Object, e As PropertyChangedEventArgs)
        HandlerCalled = True
    End Sub


    Friend Sub AddHandlerNormal(sender As TestViewModelSender)
        AddHandler sender.PropertyChanged, AddressOf Target_PropertyChanged
    End Sub


    Friend Sub AddHandlerWeakWPF(sender As TestViewModelSender)
        System.Windows.WeakEventManager(Of TestViewModelSender, PropertyChangedEventArgs).[AddHandler](sender, NameOf(sender.PropertyChanged), AddressOf Target_PropertyChanged)
    End Sub


    Friend Sub RemoveHandlerNormal(sender As TestViewModelSender)
        RemoveHandler sender.PropertyChanged, AddressOf Target_PropertyChanged
    End Sub

End Class
