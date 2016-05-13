''' <summary>
''' Raised, when a NullableValueRelationPopup control retrieves unplausible values 
''' by the GetColumnsSchema Event, and layouting the Grid causes an exception.
''' </summary>
Public Class BindableDataGridLayoutException
    Inherits Exception

    Public Sub New(message As String, currentColumnWidth As Integer?, innerException As Exception)
        MyBase.New(message, innerException)
        Me.CurrentColumnWidth = currentColumnWidth
    End Sub

    ''' <summary>
    ''' The CurrentColumnWidth which most probable caused the exception.
    ''' </summary>
    ''' <returns></returns>
    Public Property CurrentColumnWidth As Integer?

End Class

Public Class BindableDataGridLayoutExceptionEventArgs
    Inherits EventArgs

    Public Sub New()
        MyBase.New
    End Sub

    Public Sub New(ex As BindableDataGridLayoutException)
        Me.LayoutException = ex
    End Sub

    Public Property LayoutException As BindableDataGridLayoutException

End Class