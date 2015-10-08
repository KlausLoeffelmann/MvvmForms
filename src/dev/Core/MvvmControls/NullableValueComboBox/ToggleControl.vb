Imports System.ComponentModel
Imports System.Windows.Forms.Integration
Imports System.Windows.Controls
Imports System.Windows
Imports ActiveDevelop.EntitiesFormsLib

''' <summary>
''' WPF-ToggleButton
''' </summary>
''' <remarks>WPF-ToggleButton welcher von der UI auf dem EFL-ComboButton</remarks>
Public Class ToggleButton
    Inherits ContentControl

    Private _innerControl As ComboButton

    Sub New()
        'MyBase.Appearance = Forms.Appearance.Button

        Dim host = New WindowsFormsHost()
        _innerControl = New ComboButton()

        AddHandler _innerControl.Click, AddressOf CheckBox_Click

        host.Child = _innerControl

        MyBase.Content = host
    End Sub

    ''' <summary>
    ''' Schluessel zur IsChecked-DependencyProperty
    ''' </summary>
    ''' <remarks></remarks>
    Friend Shared ReadOnly IsCheckedProperty As DependencyProperty = _
                           DependencyProperty.Register("IsChecked", _
                           GetType(Object), GetType(ToggleButton))

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>(Dependency Property)</remarks>
    Public Property IsChecked As Boolean
        Get
            Return CType(GetValue(IsCheckedProperty), Boolean)
        End Get
        Set(ByVal value As Boolean)
            SetValue(IsCheckedProperty, value)
        End Set
    End Property

    Private Sub CheckBox_Click(sender As Object, e As EventArgs)
        'Nach Klicken Popup wieder schliessen lassen (bzw öffnen)
        Me.IsChecked = Not Me.IsChecked
    End Sub

End Class
