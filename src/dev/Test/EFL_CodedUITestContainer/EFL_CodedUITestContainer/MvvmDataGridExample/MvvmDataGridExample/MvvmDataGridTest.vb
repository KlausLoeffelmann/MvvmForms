Imports System.Text
Imports System.Windows.Data
Imports ActiveDevelop.EntitiesFormsLib

Public Class MvvmDataGridTest

    Private Shared _settings As New MvvmDataGridSettings()

    Sub New()

        ' This call is required by the designer.
        InitializeComponent()
        MvvmDataGrid.Settings = _settings

        ' Add any initialization after the InitializeComponent() call.
        Me.MvvmManager1.DataContext = New MainViewModel()

        Dim cm = New System.Windows.Controls.ContextMenu()
        Dim bt = New System.Windows.Controls.MenuItem() With {.Header = "Löschen"}
        AddHandler bt.Click, Sub()
                                 MessageBox.Show(DirectCast(BuchungenDataGrid.SelectedItem, Buchung).Nummer & " Gelöscht!")
                             End Sub
        cm.Items.Add(bt)

        BuchungenDataGrid.ContextMenu = cm
    End Sub

    Private Sub BuchungenDataGrid_ItemsDeleting(sender As Object, e As ActiveDevelop.EntitiesFormsLib.ItemsDeletingEventArgs) Handles BuchungenDataGrid.ItemsDeleting
        e.Cancel = MessageBox.Show("Sollen wirklich die ausgewählten Einträge gelöscht werden?", "Löschen", MessageBoxButtons.YesNo) = DialogResult.No
    End Sub

    Protected Overrides Sub OnLoad(e As EventArgs)
        MyBase.OnLoad(e)

        Dim cvs = DirectCast(BuchungenDataGrid.ItemsSource, ListCollectionView)

        cvs.GroupDescriptions.Add(New PropertyGroupDescription(NameOf(Buchung.Kostenart)))
        cvs.GroupDescriptions.Add(New PropertyGroupDescription(NameOf(Buchung.Wichtig)))
        cvs.GroupDescriptions.Add(New PropertyGroupDescription(NameOf(Buchung.Betrag)))
    End Sub

    Private Sub BuchungenDataGrid_ItemsDeleted(sender As Object, e As ActiveDevelop.EntitiesFormsLib.ItemsDeletedEventArgs) Handles BuchungenDataGrid.ItemsDeleted
        Dim sb As New StringBuilder()

        For Each item In e.DeletedItems
            sb.Append(DirectCast(item, Buchung).Buchungstext & ", ")
        Next

        MessageBox.Show("Es wurden " & sb.ToString.TrimEnd().Trim(","c) & " gelöscht")
    End Sub
End Class
