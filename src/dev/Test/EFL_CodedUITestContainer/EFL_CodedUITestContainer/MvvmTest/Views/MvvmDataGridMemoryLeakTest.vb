Public Class MvvmDataGridMemoryLeakTest



    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        GC.Collect()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        For i = 0 To 50
            Dim frm = New EmptyMvvmDataGridForm()
            frm.Show()
            frm.Close()
        Next
    End Sub
End Class