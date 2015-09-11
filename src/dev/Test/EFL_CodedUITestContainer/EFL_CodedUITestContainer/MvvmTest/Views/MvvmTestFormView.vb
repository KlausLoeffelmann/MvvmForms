Public Class MvvmTestFormView
    Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.MvvmManager1.DataContext = New MainNodeTestViewModel()
    End Sub
End Class