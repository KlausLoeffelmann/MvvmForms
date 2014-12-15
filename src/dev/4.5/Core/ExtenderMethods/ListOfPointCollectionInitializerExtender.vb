Imports System.Runtime.CompilerServices
Imports System.Drawing

Friend Module ListOfPointCollectionInitializerExtender

    <Extension()>
    Sub Add(ByVal list As List(Of PointF), ByVal X As Single,
                                           ByVal Y As Single)
        list.Add(New PointF With {.X = X, .Y = Y})
    End Sub

End Module
