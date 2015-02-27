Public Class NullableValueComboBoxView
    Protected Overrides Sub OnLoad(e As EventArgs)
        MyBase.OnLoad(e)

        NullableValueComboBox1.ItemSource = New List(Of String) From {"Abc", "DEF", "Xaaa", "Test"}
    End Sub
End Class