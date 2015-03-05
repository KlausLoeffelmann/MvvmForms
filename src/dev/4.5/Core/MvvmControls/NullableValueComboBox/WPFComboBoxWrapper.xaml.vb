Imports System.Windows
Imports System.Windows.Controls

Public Class WPFComboBoxWrapper

End Class

Public Class InnerComboBox
    Inherits ComboBox

    Private part_EditableTextBox As ExtendedTextBox

    Public Overrides Sub OnApplyTemplate()
        MyBase.OnApplyTemplate()

        part_EditableTextBox = DirectCast(MyBase.GetTemplateChild("PART_EditableTextBox"), ExtendedTextBox)
    End Sub

    Protected Overrides Sub OnDropDownOpened(e As EventArgs)
        MyBase.OnDropDownOpened(e)

        part_EditableTextBox.ClearSelectedText()

    End Sub

    Protected Overrides Sub OnDropDownClosed(e As EventArgs)
        MyBase.OnDropDownClosed(e)

        part_EditableTextBox.ShowSelectedText()

    End Sub
End Class