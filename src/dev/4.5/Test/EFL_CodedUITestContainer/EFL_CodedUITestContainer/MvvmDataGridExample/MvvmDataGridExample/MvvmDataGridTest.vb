Public Class MvvmDataGridTest
    Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.MvvmManager1.DataContext = New MainViewModel()
    End Sub
End Class
