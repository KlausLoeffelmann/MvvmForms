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

    Private PART_SelectionTextBox As ComboBoxRichTextBlock

    Protected Overrides Sub OnSelectionChanged(e As RoutedEventArgs)
        MyBase.OnSelectionChanged(e)

        ShowSelectedText()
    End Sub

    Protected Overrides Sub OnLostFocus(e As RoutedEventArgs)
        MyBase.OnLostFocus(e)

        ClearSelectedText()
    End Sub

    Public Sub ClearSelectedText()
        PART_SelectionTextBox.ClearInlines()
    End Sub

    Public Sub ShowSelectedText()
        Dim selectedTextBox As ComboBoxRichTextBlock = PART_SelectionTextBox

        If selectedTextBox IsNot Nothing AndAlso MyBase.IsSelectionActive Then
            selectedTextBox.SetInlines({New Run(MyBase.Text.Substring(0, MyBase.SelectionStart)) With {.Foreground = Brushes.Transparent},
                                           New Run(MyBase.SelectedText) With {.Foreground = Brushes.White}})

        End If
    End Sub

    Public Overrides Sub OnApplyTemplate()
        MyBase.OnApplyTemplate()

        PART_SelectionTextBox = DirectCast(GetTemplateChild("PART_SelectionTextBox"), ComboBoxRichTextBlock)

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
