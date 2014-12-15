Imports System.Windows.Forms
Imports System.Text

''' <summary>
''' Definiert Funktionalitäten für das Binden von Werteauswahlen an FormsToBusinessClass-Manager.
''' </summary>
''' <remarks></remarks>
Public Interface INullableValueRelationBinding
    Inherits INullableValueDataBinding

    ''' <summary>
    ''' Ereignis, das ausgelöst wird, wenn sich der ausgewählte Wert geändert hat.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Event SelectedValueChanged(ByVal sender As Object, ByVal e As EventArgs)

    ''' <summary>
    ''' Bestimmt oder ermittelt, aufgrund welcher Datenquelle die Auswahlliste bereitgestellt wird.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Property Datasource() As Object

    ''' <summary>
    ''' Bestimmt oder ermittelt den Namen der Eigenschaft, mit dem der Wert in der Liste eindeutig erkannt werden kann und über den gebunden wird.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Property ValueMember() As String

End Interface
