Imports System.Text

Public Class MvvmDataGridTest
    Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.MvvmManager1.DataContext = New MainViewModel()

        Dim cm = New Windows.Controls.ContextMenu()
        Dim bt = New Windows.Controls.MenuItem() With {.Header = "Löschen"}
        AddHandler bt.Click, Sub()
                                 MessageBox.Show(DirectCast(BuchungenDataGrid.SelectedItem, Buchung).Nummer & " Gelöscht!")
                             End Sub
        cm.Items.Add(bt)

        BuchungenDataGrid.ContextMenu = cm
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
End Class
