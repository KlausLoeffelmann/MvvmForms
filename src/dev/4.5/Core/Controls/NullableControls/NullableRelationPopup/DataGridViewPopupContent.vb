Imports System.ComponentModel
Imports System.Windows.Forms

<ToolboxItem(False)>
Public Class DataGridViewPopupContent

    Public Property AutoSizeColumnsMode As DataGridViewAutoSizeColumnsMode
        Get
            Return BindableDataGridView.AutoSizeColumnsMode
        End Get
        Set(value As DataGridViewAutoSizeColumnsMode)
            BindableDataGridView.AutoSizeColumnsMode = value
        End Set
    End Property

    Public Property AutoSizeRowsMode As DataGridViewAutoSizeRowsMode
        Get
            Return BindableDataGridView.AutoSizeRowsMode
        End Get
        Set(value As DataGridViewAutoSizeRowsMode)
            BindableDataGridView.AutoSizeRowsMode = value
        End Set
    End Property
End Class
