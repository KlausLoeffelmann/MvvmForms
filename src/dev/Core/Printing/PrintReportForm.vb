Public Class PrintReportForm

    Private Sub PrintReportForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.ReportViewer1.SetPageSettings(New System.Drawing.Printing.PageSettings With {.Landscape = True})
        Me.ReportViewer1.RefreshReport()
    End Sub
End Class