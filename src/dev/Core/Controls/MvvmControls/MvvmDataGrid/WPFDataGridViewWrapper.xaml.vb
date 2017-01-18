Imports System.Windows.Controls

Public Class WPFDataGridViewWrapper

    ''' <summary>
    ''' Set the GroupStyle of the DataGrid
    ''' </summary>
    Public Sub EnableGroupStyle()
        InnerDataGridView?.GroupStyle?.Add(DirectCast(Resources("GridGroupStyle"), GroupStyle))
    End Sub

    ''' <summary>
    ''' Cleans the GroupStyle of the DataGrid
    ''' </summary>
    Public Sub DisableGroupStyle()
        InnerDataGridView?.GroupStyle?.Clear()
    End Sub
End Class
