Imports System.ComponentModel.Design
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Windows

''' <summary>
''' TypeEditor welcher fuer Bearbeitung der Spalten selber verwendet werden soll
''' </summary>
''' <remarks>Basiert auf dem CollectionEditor vom Framework. Erweitert allerdings die Klasse um eine korrekte Undo-Funktion</remarks>
Public Class ColumnsEditor
    Inherits CollectionEditor

    Sub New(type As Type)
        MyBase.New(type)
    End Sub

    ''' <summary>
    ''' Speichert die ursprungs-Columns
    ''' </summary>
    ''' <remarks></remarks>
    Private _prevColumns As List(Of MvvmDataGridColumn)

    ''' <summary>
    ''' Aktuelle GridColumnCollection
    ''' </summary>
    ''' <remarks></remarks>
    Private _columnCollection As GridColumnCollection

    ''' <summary>
    ''' True wenn die Aenderungen abgebrochen wurden
    ''' </summary>
    ''' <remarks></remarks>
    Private _editCanceled As Boolean = False

    ''' <summary>
    ''' Formular welches fuer das Bearbeiten (stammt aus Basis CollectionEditor) verwenden wird
    ''' </summary>
    ''' <remarks></remarks>
    Private _collectionForm As Form

    ''' <summary>
    ''' Ruft CollectionEditor.EditValue-Methode auf, klont jedoch erst die Columns VORHER um sie eventuell danach zu restaurieren
    ''' </summary>
    ''' <param name="context"></param>
    ''' <param name="provider"></param>
    ''' <param name="value"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function EditValue(context As ITypeDescriptorContext, provider As IServiceProvider, value As Object) As Object
        _columnCollection = DirectCast(value, GridColumnCollection)
        _prevColumns = _columnCollection.Clone()
        _editCanceled = False
        Dim result = DirectCast(MyBase.EditValue(context, provider, value), GridColumnCollection)

        If _editCanceled Then
            'Aenderungen verworfen, alte Spalten restaurieren
            result.Clear()
            For Each oldColumn In _prevColumns
                result.Add(oldColumn)
            Next
        End If

        Return result
    End Function

    ''' <summary>
    ''' Fuegt ein FormClosed-Eventhandler an, um auch beim Schliessen des Forms bei "X" die Aenderungen zu verwerfen
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Overrides Function CreateCollectionForm() As System.ComponentModel.Design.CollectionEditor.CollectionForm
        Dim form = MyBase.CreateCollectionForm()

        _collectionForm = form

        AddHandler form.FormClosed, AddressOf CollectionForm_FormClosed

        Return form
    End Function

    Protected Overrides Sub CancelChanges()
        _editCanceled = True
    End Sub

    Private Sub CollectionForm_FormClosed(sender As Object, e As Forms.FormClosedEventArgs)
        If _collectionForm.DialogResult = DialogResult.Cancel Then
            CancelChanges()
        End If

        RemoveHandler _collectionForm.FormClosed, AddressOf CollectionForm_FormClosed
    End Sub

End Class