Imports System.ComponentModel.Design.Serialization
Imports System.Collections.ObjectModel

''' <summary>
''' ObservableCollection mit MvvmDataGridColumns welche mittels der MvvmDataGridColumnsCodeDomSerializer-Klasse serialisiert wird
''' </summary>
''' <remarks></remarks>
<Serializable, DesignerSerializer(GetType(MvvmDataGridColumnsCodeDomSerializer),
                                  GetType(CodeDomSerializer)), MvvmSystemElement>
Public Class GridColumnCollection
    Inherits ObservableCollection(Of MvvmDataGridColumn)

    ''' <summary>
    ''' Klont den gesamten Inhalt der Collection in eine einfache Liste
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Friend Function Clone() As List(Of MvvmDataGridColumn)
        Dim clones = New List(Of MvvmDataGridColumn)

        For Each column In MyBase.Items
            clones.Add(column.Clone())
        Next

        Return clones
    End Function
End Class