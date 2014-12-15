Public Class TimeCollectionDataGridView

    Public Event DataSourceChanged(sender As Object, e As EventArgs)
    Public Event SelectedItemChanged(sender As Object, e As EventArgs)

    Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        AddHandler TimeCollectItemsBindingSource.CurrentChanged, Sub(sender, e)
                                                                     OnSelectedItemChanged(e)
                                                                 End Sub
    End Sub

    Public Property DataSource As Object
        Get
            Return TimeCollectItemsBindingSource.DataSource
        End Get
        Set(value As Object)
            TimeCollectItemsBindingSource.DataSource = value
        End Set
    End Property

    Public Property SelectedItem As Object
        Get
            Return TimeCollectItemsBindingSource.Current
        End Get
        Set(value As Object)
            Dim si = TimeCollectItemsBindingSource.IndexOf(value)
            If si > -1 Then
                TimeCollectItemsBindingSource.Position = si
            Else
                DataGridView1.ClearSelection()
            End If
        End Set
    End Property

    Protected Overridable Sub OnDataSourceChanged(e As EventArgs)
        RaiseEvent DataSourceChanged(Me, e)
    End Sub

    Protected Overridable Sub OnSelectedItemChanged(e As EventArgs)
        RaiseEvent SelectedItemChanged(Me, e)
    End Sub

End Class
