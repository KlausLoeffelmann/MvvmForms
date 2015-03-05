Public Class NullableValueComboBoxView
    Protected Overrides Sub OnLoad(e As EventArgs)
        MyBase.OnLoad(e)

        NullableValueComboBox1.ItemSource = New List(Of String) From {"Abc", "DEF", "Xaaa", "Test"}
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        'NullableValueComboBox1.Select()
    End Sub
End Class