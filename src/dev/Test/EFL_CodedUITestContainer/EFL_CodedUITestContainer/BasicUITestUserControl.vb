Imports ActiveDevelop.EntitiesFormsLib
Imports System.ComponentModel
Imports System.Runtime.CompilerServices

Public Class BasicUITestUserControl

    Private myDemoContacts As List(Of Contact)
    Private myTestSettings As New TestConfigurationProperties

    Protected Overrides Sub OnLoad(e As System.EventArgs)
        MyBase.OnLoad(e)
        SingleResult1.Text = "- not used yet -"
        SingleResult2.Text = "- not used yet -"
        SingleResult3.Text = "- not used yet -"
        SingleResult4.Text = "- not used yet -"
        IsDirtyToolStripStatusLabel.Enabled = False
    End Sub

    Private Sub SetNullableTextValuePropertiesButton_Click(sender As System.Object, e As System.EventArgs) Handles SetNullableTextValuePropertiesButton.Click
        MainTab.SelectedIndex = 1
        MainPropertyGrid.SelectedObject = NullableTextValueInput
    End Sub

    Private Sub SetNullableMultilineTextValuePropertiesButton_Click(sender As System.Object, e As System.EventArgs) Handles SetNullableMultilineTextValuePropertiesButton.Click
        MainTab.SelectedIndex = 1
        MainPropertyGrid.SelectedObject = NullableMultilineTextValueInput
    End Sub

    Private Sub SetNullableIntValuePropertiesButton_Click(sender As System.Object, e As System.EventArgs) Handles SetNullableIntValuePropertiesButton.Click
        MainTab.SelectedIndex = 1
        MainPropertyGrid.SelectedObject = NullableIntValueInput
    End Sub

    Private Sub SetNullableNumValuePropertiesButton_Click(sender As System.Object, e As System.EventArgs) Handles SetNullableNumValuePropertiesButton.Click
        MainTab.SelectedIndex = 1
        MainPropertyGrid.SelectedObject = NullableNumValueInput
    End Sub

    Private Sub SetNullableDateValuePropertiesButton_Click(sender As System.Object, e As System.EventArgs) Handles SetNullableDateValuePropertiesButton.Click
        MainTab.SelectedIndex = 1
        MainPropertyGrid.SelectedObject = NullableDateValueInput
    End Sub

    Private Sub SetNullableRelationPopupPropertiesButton_Click(sender As System.Object, e As System.EventArgs) Handles SetNullableRelationPopupPropertiesButton.Click
        MainTab.SelectedIndex = 1
        MainPropertyGrid.SelectedObject = NullableValueRelationPopupInput
    End Sub

    Private Sub SubTest01Button_Click(sender As System.Object, e As System.EventArgs) Handles SubTest01Button.Click

        myDemoContacts = Contact.RandomContacts(100000)
        NullableValueRelationPopupInput.DataSource = myDemoContacts
        NullableValueRelationPopupInput.DisplayMember = """{0:0000}: {1}, {2}"",{IDContact},{LastName},{FirstName}"
        NullableValueRelationPopupInput.SearchPattern = """{0:0000}: {1}, {2}, {3}"",{IDContact},{LastName},{FirstName},{City}"
        NullableValueRelationPopupInput.PreferredVisibleColumnsOnOpen = 4
        NullableValueRelationPopupInput.PreferredVisibleRowsOnOpen = 10
        NullableValueRelationPopupInput.ValueMember = "IDContact"
        ResultTextBox.Clear()
        NullableValueRelationPopupInput.Select()

        nvrPreserveInputTest.PreserveInput = True
        nvrPreserveInputTest.DataSource = Address.RandomAddresses(10000)
        nvrPreserveInputTest.DisplayMember = """{0}"",{Street}"
        nvrPreserveInputTest.SearchPattern = """{0}, {1}: {2}"",{Street},{ZIP},{City}"
        nvrPreserveInputTest.PreferredVisibleColumnsOnOpen = 4
        nvrPreserveInputTest.PreferredVisibleRowsOnOpen = 10

    End Sub

    Private Sub NullableValueRelationPopupInput_GetColumnSchema(sender As Object, e As ActiveDevelop.EntitiesFormsLib.GetColumnSchemaEventArgs) Handles NullableValueRelationPopupInput.GetColumnSchema
        Dim fn As New DataGridViewColumnFieldnames
        fn.Add("LastName", "Nachname")
        fn.Add("FirstName", "Vorname")
        fn.Add("Street", "Straße")
        fn.Add("Zip", "PLZ")
        fn.Add("City", "Ort")
        e.SchemaFieldnames = fn
    End Sub

    Private Sub NullableValueRelationPopupInput_IsDirtyChanged(sender As Object, e As System.EventArgs) Handles NullableValueRelationPopupInput.IsDirtyChanged
        ResultTextBox.WriteLine("IsDirtyChanged")
    End Sub

    Private Sub NullableValueRelationPopupInput_SelectedValueChanged(sender As Object, e As System.EventArgs) Handles NullableValueRelationPopupInput.SelectedValueChanged

        If NullableValueRelationPopupInput.SelectedValue Is Nothing Then
            SingleResult1.Text = NullableValueRelationPopupInput.NullValueString
        Else
            SingleResult1.Text = NullableValueRelationPopupInput.SelectedValue.ToString
        End If
        ResultTextBox.WriteLine("SelectedValueChanged to " & SingleResult1.Text)
    End Sub

    Private Sub NullableValueRelationPopupInput_ValueChanged(sender As Object, e As ActiveDevelop.EntitiesFormsLib.ValueChangedEventArgs) Handles NullableValueRelationPopupInput.ValueChanged
        If NullableValueRelationPopupInput.Value Is Nothing Then
            SingleResult2.Text = NullableValueRelationPopupInput.NullValueString
        Else
            SingleResult2.Text = NullableValueRelationPopupInput.Value.ToString
        End If
        ResultTextBox.WriteLine("ValueChanged to " & SingleResult2.Text)

    End Sub

    Private Sub IsDirtyToolStripStatusLabel_DoubleClick(sender As Object, e As System.EventArgs) Handles IsDirtyToolStripStatusLabel.DoubleClick
        ftbcmMain.CancelEdit()
        IsDirtyToolStripStatusLabel.Text = "Keine Änderungen"
        IsDirtyToolStripStatusLabel.Enabled = False
    End Sub

    Private Sub ftbcmMain_IsFormDirtyChanged(sender As Object, e As ActiveDevelop.EntitiesFormsLib.IsFormDirtyChangedEventArgs) Handles ftbcmMain.IsFormDirtyChanged
        IsDirtyToolStripStatusLabel.Text = "Änderungen vorhanden!"
        IsDirtyToolStripStatusLabel.Enabled = True
    End Sub

    Private Sub TestConfigurationPropertiesButton_Click(sender As System.Object, e As System.EventArgs) Handles TestConfigurationPropertiesButton.Click
        MainTab.SelectedIndex = 1
        MainPropertyGrid.SelectedObject = myTestSettings
    End Sub

    Private Sub ClearTestResultButton_Click(sender As System.Object, e As System.EventArgs) Handles ClearTestResultButton.Click
        ResultTextBox.Clear()
    End Sub

    Private Sub NullableValueRelationPopupInput_IsDirtyChanged(sender As Object, e As IsDirtyChangedEventArgs) Handles NullableValueRelationPopupInput.IsDirtyChanged

    End Sub

    Private Sub nvrPreserveInputTest_SelectedValueChanged(sender As Object, e As EventArgs) Handles nvrPreserveInputTest.SelectedValueChanged
        If nvrPreserveInputTest.SelectedValue Is Nothing Then
            SingleResult1.Text = nvrPreserveInputTest.NullValueString
        Else
            SingleResult1.Text = nvrPreserveInputTest.SelectedValue.ToString
        End If
        ResultTextBox.WriteLine("SelectedValueChanged to " & SingleResult1.Text)
    End Sub

    Private Sub nvrPreserveInputTest_TextChanged(sender As Object, e As EventArgs) Handles nvrPreserveInputTest.TextChanged

    End Sub

    Private Sub nvrPreserveInputTest_ValueChanged(sender As Object, e As ValueChangedEventArgs) Handles nvrPreserveInputTest.ValueChanged
        If nvrPreserveInputTest.Value Is Nothing Then
            SingleResult1.Text = nvrPreserveInputTest.NullValueString
        Else
            SingleResult1.Text = nvrPreserveInputTest.Value.ToString
        End If
        ResultTextBox.WriteLine("ValueChanged to " & SingleResult1.Text)
    End Sub
End Class

Public Class TestConfigurationProperties

    Property DisplayIsDirtyChangeInTestResults As Boolean


End Class

Public Module TextBoxExtender

    <Extension()>
    Public Sub WriteLine(Tb As TextBox, text As String)
        Tb.AppendText(text & vbNewLine)
        Tb.SelectionStart = Tb.Text.Length - 1
        Tb.SelectionLength = 1
        Tb.ScrollToCaret()
    End Sub
End Module