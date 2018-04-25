Imports System.Threading
Imports ActiveDevelop.EntitiesFormsLib

Public Class MainForm

    Private Sub QuitToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs)
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
        Dim dlg = New MvvmDataGridTest()
        dlg.ShowDialog()
        dlg.Dispose()
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

    Private Sub NullableNumValue1_IsDirtyChanged(sender As Object, e As ActiveDevelop.EntitiesFormsLib.IsDirtyChangedEventArgs) Handles NullableNumValue1.IsDirtyChanged

    End Sub

    Private Sub NullableIntValue1_Click(sender As Object, e As EventArgs) Handles NullableIntValue1.Click

    End Sub

    Private Sub CBOToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CBOToolStripMenuItem.Click
        NullableValueComboBoxView.ShowDialog()
    End Sub

    Private Sub TreeViewToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles TreeViewToolStripMenuItem.Click
        Dim frm = New MvvmTestFormView()
        frm.ShowDialog()
    End Sub

    Private Sub NVRPLateBindingToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles NVRPLateBindingToolStripMenuItem.Click
        Dim frm = New NVRPLateBinding()
        frm.ShowDialog()
    End Sub

    Private Sub MVVMGridMemoryLeakToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles MVVMGridMemoryLeakToolStripMenuItem.Click
        Dim frm = New MvvmDataGridMemoryLeakTest
        frm.ShowDialog()
    End Sub

    Private Sub PropertyBindingsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PropertyBindingsToolStripMenuItem.Click
        Dim frm As New frmMvvmPropertyAssignmentRev

        frm.ShowDialogWithTestdata(GetType(MainNodeTestViewModel))
    End Sub
End Class
