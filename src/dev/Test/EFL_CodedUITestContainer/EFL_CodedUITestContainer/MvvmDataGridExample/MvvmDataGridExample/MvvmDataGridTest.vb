﻿Imports System.Text

Public Class MvvmDataGridTest
    Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.MvvmManager1.DataContext = New MainViewModel()
    End Sub

    Private Sub BuchungenDataGrid_ItemsDeleting(sender As Object, e As ActiveDevelop.EntitiesFormsLib.ItemsDeletingEventArgs) Handles BuchungenDataGrid.ItemsDeleting
        e.Cancel = MessageBox.Show("Sollen wirklich die ausgewählten Einträge gelöscht werden?", "Löschen", MessageBoxButtons.YesNo) = DialogResult.No
    End Sub

    Private Sub BuchungenDataGrid_ItemsDeleted(sender As Object, e As ActiveDevelop.EntitiesFormsLib.ItemsDeletedEventArgs) Handles BuchungenDataGrid.ItemsDeleted
        Dim sb As New StringBuilder()

        For Each item In e.DeletedItems
            sb.Append(DirectCast(item, Buchung).Buchungstext & ", ")
        Next

        MessageBox.Show("Es wurden " & sb.ToString.TrimEnd().Trim(","c) & " gelöscht")
    End Sub

    Private Sub ScrollToLasItemToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ScrollToLasItemToolStripMenuItem.Click
        Me.BuchungenDataGrid.ScrollIntoView(DirectCast(MvvmManager1.DataContext, MainViewModel).Buchungen.Last)
    End Sub
End Class
