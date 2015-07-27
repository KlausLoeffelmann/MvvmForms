Imports ArtHorizViewModel
Imports Autofac

''' <summary>
''' An empty page that can be used on its own or navigated to within a Frame.
''' </summary>
Public NotInheritable Class MainPage
    Inherits Page

    'Scope of ViewModel is same as Scope of Page.
    Private myScope As ILifetimeScope

    Protected Overrides Sub OnNavigatedTo(e As NavigationEventArgs)
        MyBase.OnNavigatedTo(e)
        If Not Windows.ApplicationModel.DesignMode.DesignModeEnabled Then
            myScope = App.Container.BeginLifetimeScope
            Me.DataContext = myScope.Resolve(Of MainViewModel)
        End If
    End Sub

End Class
