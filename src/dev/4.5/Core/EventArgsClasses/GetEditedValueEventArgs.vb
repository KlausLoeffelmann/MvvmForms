Imports System.Windows.Forms
Imports System.Text

Public Class GetEditedValueEventArgs
    Inherits EventArgs

    Sub New(ByVal editedValue As String)
        myEditedValue = editedValue
    End Sub

    Private myEditedValue As String
    Public Property EditedValue() As String
        Get
            Return myEditedValue
        End Get
        Set(ByVal value As String)
            myEditedValue = value
        End Set
    End Property

End Class
