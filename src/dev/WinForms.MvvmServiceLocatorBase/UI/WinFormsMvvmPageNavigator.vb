Imports System.ComponentModel
Imports System.Windows.Forms
Imports ActiveDevelop.EntitiesFormsLib
Imports ActiveDevelop.IoC.Generic

''' <summary>
''' UWP Implementation of the MvvmPageNavigationService
''' </summary>
Public Class WinFormsMvvmPageNavigationService
    Implements IMvvmPageNavigationService

    Private Shared myMainWindow As Form

    Public Sub GoBack() Implements IMvvmPageNavigationService.GoBack
        Throw New NotImplementedException("Windows Forms does not support Go-Back-Navigation.")
    End Sub

    Public Sub GoForward() Implements IMvvmPageNavigationService.GoForward
        Throw New NotImplementedException("Windows Forms does not support Go-Forward-Navigation.")
    End Sub

    Public Sub NavigateTo(pageThroughViewmodel As INotifyPropertyChanged) Implements IMvvmPageNavigationService.NavigateTo
        Dim newFormType = WinFormsMvvmPlatformServiceLocator.ViewModelToPageResolver(pageThroughViewmodel)
        Dim newForm = DirectCast(Activator.CreateInstance(newFormType), Form)

        'Find out, if target has implemented DataContext-Property
        If newForm IsNot Nothing Then
            Dim dataContextProperty = newForm.GetType().GetProperty("DataContext")
            If dataContextProperty IsNot Nothing Then
                dataContextProperty.SetValue(newForm, pageThroughViewmodel)
            Else
                Throw New ArgumentException("Could not assign ViewModel to Page (Form) of type '" & newForm.GetType.Name &
                                            "', because it does not have DataContext property." & vbNewLine &
                                            "Please, implement a DataContext property and wire it up to the MvvmManager component to make it a proper view.")
            End If
        Else
            Throw New ArgumentException("The View of type '" & newForm.GetType.Name & "' could not be created.")
        End If

        newForm.ShowDialog()

    End Sub

    Public Function CanGoBack() As Boolean Implements IMvvmPageNavigationService.CanGoBack
        Throw New NotImplementedException("Not implemented yet.")
    End Function

    Public Function CanGoForward() As Boolean Implements IMvvmPageNavigationService.CanGoForward
        Throw New NotImplementedException("Windows Forms does not support Go-Forward-Navigation.")
    End Function

    Public Shared Property MainWindow As Form
        Get
            Return myMainWindow
        End Get
        Friend Set(value As Form)
            myMainWindow = value
        End Set
    End Property
End Class
