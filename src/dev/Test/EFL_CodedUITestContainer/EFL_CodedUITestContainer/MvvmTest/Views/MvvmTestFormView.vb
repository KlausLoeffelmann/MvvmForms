Public Class MvvmTestFormView
    Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.MvvmManager1.DataContext = New MainNodeTestViewModel()
    End Sub

    Private Sub NullableNumValue1_ValueValidating(sender As Object, e As ActiveDevelop.EntitiesFormsLib.NullableValueValidationEventArgs(Of Decimal?)) Handles NullableNumValue1.ValueValidating
        If e.Value = 8 Then
            e.ValidationFailedUIMessage = "Problerm"
        Else
            e.ValidationFailedUIMessage = Nothing
        End If
    End Sub
End Class