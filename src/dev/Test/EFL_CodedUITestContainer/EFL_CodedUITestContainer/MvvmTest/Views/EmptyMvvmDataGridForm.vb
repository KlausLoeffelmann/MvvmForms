Public Class EmptyMvvmDataGridForm


    Public Sub New()
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Dim itemsource = New List(Of Integer)
        For i = 0 To 100
            itemsource.Add(i)
        Next
        MvvmGrid.ItemsSource = itemsource
    End Sub
End Class