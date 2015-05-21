Public Class HighDpiTestForm
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Debug.Print(NullableTextValue1.Height.ToString & "; " & PreferredSize.ToString)
        Debug.Print(TextBox1.Height.ToString)
    End Sub
End Class