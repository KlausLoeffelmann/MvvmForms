''' <summary>
''' EventArgs für DoppelKlick im MvvmDataGrid in einer Zeile
''' </summary>
''' <remarks></remarks>
Public Class ItemDoubleClickEventArgs

    ''' <summary>
    ''' Ausgewählte Item innerhalb der Zeile
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Property Item As Object

    Sub New(item As Object)
        Me.Item = item
    End Sub

End Class
