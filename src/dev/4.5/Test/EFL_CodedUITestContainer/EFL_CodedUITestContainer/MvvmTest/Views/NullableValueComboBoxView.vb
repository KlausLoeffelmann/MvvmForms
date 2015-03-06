Public Class NullableValueComboBoxView
    Protected Overrides Sub OnLoad(e As EventArgs)
        MyBase.OnLoad(e)

        'NullableValueComboBox1.ItemSource = New List(Of String) From {"Abc", "DEF", "Xaaa", "Test"}
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        'NullableValueComboBox1.Select()

        Dim x = New CBOTestForm()

        x.MdiParent = Me

        x.Show()
    End Sub

    Private Sub NullableValueComboBoxView_Leave(sender As Object, e As EventArgs) Handles MyBase.Leave

    End Sub

    Private Sub NullableValueComboBoxView_Deactivate(sender As Object, e As EventArgs) Handles MyBase.Deactivate

    End Sub
End Class