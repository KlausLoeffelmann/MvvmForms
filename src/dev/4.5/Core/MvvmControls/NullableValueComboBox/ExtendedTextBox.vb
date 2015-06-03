Imports System.Windows.Controls
Imports System.Windows
Imports System.Windows.Documents
Imports System.Windows.Media
Imports System.ComponentModel

<TemplatePart(Name:="PART_SelectionTextBox", Type:=GetType(RichTextBox))>
Public Class ExtendedTextBox
    Inherits TextBox

    Shared Sub New()
        DefaultStyleKeyProperty.OverrideMetadata(GetType(ExtendedTextBox), New FrameworkPropertyMetadata(GetType(ExtendedTextBox)))
    End Sub

    Private part_SelectionTextBox As RichTextBox

    Protected Overrides Sub OnSelectionChanged(e As RoutedEventArgs)
        MyBase.OnSelectionChanged(e)

        ShowSelectedText()
    End Sub

    Protected Overrides Sub OnLostFocus(e As RoutedEventArgs)
        MyBase.OnLostFocus(e)

        ClearSelectedText()
    End Sub

    Public Sub ClearSelectedText()
        part_SelectionTextBox.Document = New FlowDocument()
    End Sub

    Public Sub ShowSelectedText()
        Dim selectedTextBox As RichTextBox = part_SelectionTextBox

        If selectedTextBox IsNot Nothing AndAlso MyBase.IsSelectionActive Then
            selectedTextBox.Document = New FlowDocument()

            Dim par = New Paragraph()


            par.Inlines.Add(New Run(MyBase.Text.Substring(0, MyBase.SelectionStart)) With {.Foreground = Brushes.Transparent})
            par.Inlines.Add(New Run(MyBase.SelectedText) With {.Foreground = Brushes.White})

            selectedTextBox.Document.Blocks.Add(par)
        End If
    End Sub

    Public Overrides Sub OnApplyTemplate()
        MyBase.OnApplyTemplate()

        part_SelectionTextBox = DirectCast(GetTemplateChild("PART_SelectionTextBox"), RichTextBox)

        Dim prop As DependencyPropertyDescriptor = DependencyPropertyDescriptor.FromProperty(TextBox.IsSelectionActiveProperty, GetType(TextBox))

        prop.AddValueChanged(Me, Sub()
                                     If MyBase.IsSelectionActive Then
                                         Me.ShowSelectedText()
                                     Else
                                         Me.ClearSelectedText()
                                     End If

                                 End Sub)
    End Sub

End Class
