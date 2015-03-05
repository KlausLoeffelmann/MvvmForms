' Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
'
' Step 1a) Using this custom control in a XAML file that exists in the current project.
' Add this XmlNamespace attribute to the root element of the markup file where it is 
' to be used:
'
'     xmlns:MyNamespace="clr-namespace:WpfApplication18"
'
'
' Step 1b) Using this custom control in a XAML file that exists in a different project.
' Add this XmlNamespace attribute to the root element of the markup file where it is 
' to be used:
'
'     xmlns:MyNamespace="clr-namespace:WpfApplication18;assembly=WpfApplication18"
'
' You will also need to add a project reference from the project where the XAML file lives
' to this project and Rebuild to avoid compilation errors:
'
'     Right click on the target project in the Solution Explorer and
'     "Add Reference"->"Projects"->[Browse to and select this project]
'
'
' Step 2)
' Go ahead and use your control in the XAML file. Note that Intellisense in the
' XML editor does not currently work on custom controls and its child elements.
'
'     <MyNamespace:ExtendedTextBox/>
'

Imports System.Windows.Controls.Primitives
Imports System.Windows.Controls
Imports System.Windows
Imports System.Windows.Documents
Imports System.Windows.Media

<TemplatePart(Name:="PART_SelectionTextBox", Type:=GetType(RichTextBox))>
Public Class ExtendedTextBox
    Inherits TextBox

    Shared Sub New()
        'This OverrideMetadata call tells the system that this element wants to provide a style that is different than its base class.
        'This style is defined in themes\generic.xaml
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

        If selectedTextBox IsNot Nothing Then
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
    End Sub

End Class
