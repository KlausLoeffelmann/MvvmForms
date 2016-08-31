Public Class MvvmTestFormView
    Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Dim vm = New MainNodeTestViewModel()
        Me.MvvmManager1.DataContext = vm

        AddHandler vm.PropertyChanged, Sub(s, e)
                                           If e.PropertyName = NameOf(MainNodeTestViewModel.SelectedNode) Then
                                               If vm.SelectedNode IsNot Nothing Then
                                                   ListBox2.Items.Add(vm.SelectedNode?.Nachname)
                                               Else
                                                   ListBox2.Items.Add("-null-")
                                               End If

                                               ListBox2.SelectedIndex = ListBox2.Items.Count - 1
                                           End If
                                       End Sub
    End Sub

    Private Sub NullableNumValue1_ValueValidating(sender As Object, e As ActiveDevelop.EntitiesFormsLib.NullableValueValidationEventArgs(Of Decimal?)) Handles NullableNumValue1.ValueValidating
        If e.Value = 8 Then
            e.ValidationFailedUIMessage = "Problerm"
        Else
            e.ValidationFailedUIMessage = Nothing
        End If
    End Sub

    Private Sub MvvmTestFormView_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
End Class