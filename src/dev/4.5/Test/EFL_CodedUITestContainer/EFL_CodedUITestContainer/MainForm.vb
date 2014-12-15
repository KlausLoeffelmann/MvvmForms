Imports System.Threading

Public Class MainForm

    Private Sub QuitToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles QuitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub RunBasicUITestToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles RunBasicUITestToolStripMenuItem.Click
        Dim userControl As New BasicUITestUserControl
        Me.SuspendLayout()
        userControl.Dock = DockStyle.Fill
        Me.Controls.Add(userControl)
        userControl.BringToFront()
        Me.ResumeLayout()
    End Sub

    Private Sub NullableValueRelationPopup2_AddButtonClick(sender As Object, e As EventArgs) Handles NullableValueRelationPopup2.AddButtonClick
        MessageBox.Show("AddButton has been clicked!")
    End Sub

    Private Sub NullableNumValue1_IsDirtyChanged(sender As Object, e As EventArgs) Handles NullableNumValue1.IsDirtyChanged
        Debug.Print(NullableNumValue1.IsDirty.ToString)
    End Sub

    Private Sub NullableNumValue1_ValueValidating(sender As Object, e As ActiveDevelop.EntitiesFormsLib.NullableValueValidationEventArgs(Of Decimal?)) Handles NullableNumValue1.ValueValidating
        Debug.Print(NullableNumValue1.IsDirty.ToString)
    End Sub

    Private Sub MVVMGridDemoToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles MVVMGridDemoToolStripMenuItem.Click
        Call (New MvvmDataGridTest).ShowDialog()
    End Sub

    Private Sub ZoomingTestToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ZoomingTestToolStripMenuItem.Click
        Call (New BasicZoomingTest).ShowDialog()
    End Sub

    Private Sub ToolStripMenuItem2_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem2.Click
        Me.Font = New Font(Me.Font.FontFamily, 8.25)
    End Sub

    Private Sub ToolStripMenuItem3_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem3.Click
        Me.Font = New Font(Me.Font.FontFamily, 12)
    End Sub

    Private Sub ToolStripMenuItem4_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem4.Click
        Me.Font = New Font(Me.Font.FontFamily, 16)
    End Sub
End Class
