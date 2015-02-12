Imports System.ComponentModel
Imports System.Windows.Forms

<ToolboxItem(False)>
Public Class ControlBindingGrid

    Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Private Sub PropertyBindingItemBindingSource_CurrentItemChanged(sender As Object, e As EventArgs) _
        Handles PropertyBindingItemBindingSource.CurrentItemChanged
        If PropertyBindingItemBindingSource.Current Is Nothing Then
            DeleteToolStripButton.Enabled = False
            EditToolStripButton.Enabled = False
        Else
            DeleteToolStripButton.Enabled = True
            EditToolStripButton.Enabled = True
        End If
    End Sub


    Public ReadOnly Property AddButton As ToolStripButton
        Get
            Return AddToolStripButton
        End Get
    End Property

    Public ReadOnly Property DeleteButton As ToolStripButton
        Get
            Return DeleteToolStripButton
        End Get
    End Property

    Public ReadOnly Property ChangeButton As ToolStripButton
        Get
            Return EditToolStripButton
        End Get
    End Property

    Public ReadOnly Property GridDataSource As BindingSource
        Get
            Return PropertyBindingItemBindingSource
        End Get
    End Property

    Public ReadOnly Property BindingGrid As DataGridView
        Get
            Return myBindingDataGrid
        End Get
    End Property

End Class
