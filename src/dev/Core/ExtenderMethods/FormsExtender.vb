Imports System.Runtime.CompilerServices
Imports System.Threading.Tasks
Imports System.Windows.Forms

Public Module FormsExtender

    <Extension>
    Public Async Function ShowDialogAsync(form As Form) As Task(Of DialogResult)

        Return Await Task.FromResult(Of DialogResult)(form.ShowDialog)

    End Function

End Module
