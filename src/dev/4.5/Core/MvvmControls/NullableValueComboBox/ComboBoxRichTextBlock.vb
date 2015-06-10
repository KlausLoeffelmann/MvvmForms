Imports System.Windows.Controls
Imports System.Windows.Documents

''' <summary>
''' RichTextBox-Ableitung eigens für die NullableValueComboBox
''' </summary>
''' <remarks></remarks>
Public Class ComboBoxRichTextBlock
    Inherits RichTextBox

    Private _raiseTextChangedEvent As Boolean = True

    Public Sub SetInlines(inlines As IEnumerable(Of Run))
        Try
            _raiseTextChangedEvent = False

            Dim doc = New FlowDocument()
            Dim par = New Paragraph()

            For Each inline In inlines
                par.Inlines.Add(inline)
                par.Inlines.Add(inline)
            Next

            doc.Blocks.Add(par)

            MyBase.Document = doc
        Finally
            _raiseTextChangedEvent = True
        End Try
    End Sub

    Public Sub ClearInlines()
        Try
            _raiseTextChangedEvent = False

            MyBase.Document = New FlowDocument()
        Finally
            _raiseTextChangedEvent = True
        End Try
    End Sub

    Protected Overrides Sub OnTextChanged(e As TextChangedEventArgs)
        If _raiseTextChangedEvent Then
            MyBase.OnTextChanged(e)
        End If
    End Sub
End Class