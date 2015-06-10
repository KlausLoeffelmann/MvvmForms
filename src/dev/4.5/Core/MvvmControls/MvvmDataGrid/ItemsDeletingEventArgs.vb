Public Class ItemsDeletingEventArgs
    Inherits EventArgs

    ''' <summary>
    ''' Wenn gesetzt, werden die selektierten Einträge nicht gelöscht
    ''' </summary>
    ''' <returns></returns>
    Property Cancel As Boolean = False
End Class
