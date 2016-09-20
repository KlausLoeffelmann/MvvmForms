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


    Private WithEvents myDumpIsOpen_Timer As New Timer() With {.Interval = 200}

    Private Sub NullableNumValue1_Enter(sender As Object, e As EventArgs) Handles NullableNumValue1.Enter
        myDumpIsOpen_Timer.Enabled = True
        NullableNumValue1.Tag = Date.Now
    End Sub

    Private Sub NullableNumValue1_Leave(sender As Object, e As EventArgs) Handles NullableNumValue1.Leave
    End Sub

    Private Sub Print_IsOpen(sender As Object, e As EventArgs) Handles myDumpIsOpen_Timer.Tick

        Dim v = CStr(myDumpIsOpen_Timer.Tag)
        Dim c = NullableNumValue1.IsCalculatorOpen.ToString
        If c IsNot v Then
            myDumpIsOpen_Timer.Tag = c
            Debug.WriteLine($"IsCalculatorOpen hat sich kürzlich verändert von {v} nach {c}")
        End If
        If ActiveControl Is NullableNumValue1 Then NullableNumValue1.Tag = Date.Now
        Dim ld = CDate(NullableNumValue1.Tag)
        If ld.AddSeconds(10) < Date.Now Then
            myDumpIsOpen_Timer.Enabled = False
        End If

    End Sub
End Class