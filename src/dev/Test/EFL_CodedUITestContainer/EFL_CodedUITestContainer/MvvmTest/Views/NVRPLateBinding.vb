Public Class NVRPLateBinding


    Public Sub New()
        ' Dieser Aufruf ist für den Designer erforderlich.
        InitializeComponent()

        ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
        MvvmManager1.DataContext = New LateBindingViewModel()
    End Sub


    Protected Overrides Async Sub OnLoad(e As EventArgs)
        MyBase.OnLoad(e)

        Await DirectCast(MvvmManager1.DataContext, LateBindingViewModel).LoadAsync()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Me.Close()
    End Sub
End Class