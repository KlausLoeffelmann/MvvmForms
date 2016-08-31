Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Input

Public Class WPFComboBoxWrapper

    Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        'AddHandler Application.Current.Deactivated, AddressOf Application_Deactivated


    End Sub

End Class

Public Class InnerComboBox
    Inherits ComboBox

    Private part_EditableTextBox As ExtendedTextBox
    Private myImitateTabByPageKeys As Boolean 'Emuliert das Tabben

    Public Overrides Sub OnApplyTemplate()
        MyBase.OnApplyTemplate()

        part_EditableTextBox = DirectCast(MyBase.GetTemplateChild("PART_EditableTextBox"), ExtendedTextBox)

        AddHandler part_EditableTextBox.PreviewKeyDown, AddressOf part_EditableTextBox_PreviewKeyDown
    End Sub

    Private Sub part_EditableTextBox_PreviewKeyDown(sender As Object, e As KeyEventArgs)
        If ImitateTabByPageKeys Then
            If e.Key = Key.Next OrElse e.Key = Key.PageUp Then
                e.Handled = True

                If e.Key = Key.Next Then Forms.SendKeys.SendWait("{TAB}")
                If e.Key = Key.PageUp Then Forms.SendKeys.SendWait("+{TAB}")
            End If
        End If
    End Sub

    Protected Overrides Sub OnDropDownOpened(e As EventArgs)
        MyBase.OnDropDownOpened(e)

        part_EditableTextBox.ClearSelectedText()
    End Sub

    ''' <summary>
    ''' Returns or sets a flag which determines that the use can cycle between entry fields with Page up and Page down rather than Tab and Shift+Tab.
    ''' </summary>
    ''' <returns></returns>
    Public Property ImitateTabByPageKeys As Boolean
        Get
            Return myImitateTabByPageKeys
        End Get
        Set(value As Boolean)
            If Not Object.Equals(myImitateTabByPageKeys, value) Then
                myImitateTabByPageKeys = value
            End If
        End Set
    End Property

    Protected Overrides Sub OnDropDownClosed(e As EventArgs)
        MyBase.OnDropDownClosed(e)

        part_EditableTextBox.ShowSelectedText()

    End Sub

    ''' <summary>
    ''' Moves the cursor to the end
    ''' </summary>
    Friend Sub SetCursorToEnd()
        part_EditableTextBox.SetCursorToEnd()
    End Sub
End Class