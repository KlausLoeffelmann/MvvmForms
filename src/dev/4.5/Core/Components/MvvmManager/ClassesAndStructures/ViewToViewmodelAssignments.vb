Imports System.Windows.Forms
Imports System.Collections.ObjectModel
Imports ActiveDevelop.EntitiesFormsLib.ViewModelBase
Imports System.ComponentModel

''' <summary>
''' Verwaltet eine Datenstruktur, die das Nachschlagen eines View-Types auf Basis eines ViewModel-Typs ermöglicht.
''' </summary>
''' <remarks></remarks>
Public Class ViewToViewmodelAssignments
    Inherits KeyedCollection(Of Type, ViewToViewmodelAssignment)

    Protected Overrides Function GetKeyForItem(item As ViewToViewmodelAssignment) As Type
        Return item.ViewModelType
    End Function
End Class

''' <summary>
''' Repräsentiert eine Zuordnungseinheit ViewModel --> View und ermöglicht das Nachschlagen einer View auf Basis eines ViewModels.
''' </summary>
''' <remarks></remarks>
Public Class ViewToViewmodelAssignment

    Sub New(viewType As Type, viewModelType As Type)
        CheckTypesWithExceptions(viewType, viewModelType)
        Me.ViewType = viewType
        Me.ViewModelType = viewModelType
    End Sub

    Public Shared Sub CheckTypesWithExceptions(viewType As Type, viewModelType As Type)
        If Not (viewType.IsAssignableFrom(GetType(ContainerControl))) Then
            Throw New ArgumentException("Only types derived from ContainerControl could act as Views under Windows Forms. Choose another type, like Typeof(UserControl) or Typeof(Form) (Gettype(type) in Visual Basic).")
        End If

        If Not (viewModelType.IsAssignableFrom(GetType(INotifyPropertyChanged))) Then
            Throw New ArgumentException("Only types implementing the IMvvmViewModel interdace could act as ViewsModels under Windows Forms. Choose a type that implements this interface and pass it with TypeOf (or Gettype in Visual Basic).")
        End If
    End Sub

    Public Property ViewType As Type
    Public Property ViewModelType As Type

End Class
