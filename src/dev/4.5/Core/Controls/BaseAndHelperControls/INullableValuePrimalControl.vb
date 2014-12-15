Imports System.Windows.Forms
Imports System.ComponentModel
Imports System.Drawing

''' <summary>
''' Definiert die Funktionalitäten für ein Steuerelement, damit als Basis eines neuen 
''' von <see cref="NullableValueBase(Of NullableType, ControlType)">NullableValueBase(Of NullableType, ControlType)</see> vererbten Steuerelementes
'''  verwendet werden kann.
''' </summary>
''' <remarks></remarks>
Public Interface INullableValuePrimalControl
    Event ControlValidated(ByVal sender As Object, ByVal e As CancelEventArgs)
    Event ControlValueChanged(ByVal sender As Object, ByVal e As EventArgs)

    Sub ControlSetFocus()
    Sub InitializeComponentInternal()

    Property ControlFont As Font
    Property ControlForeColor As Color

    Property Value As Object
End Interface

''' <summary>
''' Kennzeichnet ein Steuerelement als ein TextBox-Steuerelement basierendes.
''' </summary>
''' <remarks></remarks>
Public Interface ITextBoxBasedControl
    ReadOnly Property TextBoxPart As TextBox
End Interface

