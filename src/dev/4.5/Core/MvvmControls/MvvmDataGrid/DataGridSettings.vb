Imports System.Xml.Serialization
Imports System
Imports System.Collections.Generic
Imports System.Text
Imports System.Drawing
Imports System.Windows.Controls
Imports System.ComponentModel

''' <summary>
''' Liste für alle Einstellungen von jedem DataGrid
''' </summary>
''' <remarks>Schlüssel ist der Name des Formulars</remarks>
<XmlRoot("dictionary")>
Public Class MvvmDataGridSettings
    Inherits Dictionary(Of String, MvvmDataGridSetting)
    Implements IXmlSerializable
#Region "IXmlSerializable Members"
    Public Function GetSchema() As System.Xml.Schema.XmlSchema Implements IXmlSerializable.GetSchema
        Return Nothing
    End Function

    Public Sub ReadXml(reader As System.Xml.XmlReader) Implements IXmlSerializable.ReadXml
        Dim keySerializer As New XmlSerializer(GetType(String))
        Dim valueSerializer As New XmlSerializer(GetType(MvvmDataGridSetting))

        Dim wasEmpty As Boolean = reader.IsEmptyElement
        reader.Read()

        If wasEmpty Then
            Return
        End If

        While reader.NodeType <> System.Xml.XmlNodeType.EndElement
            reader.ReadStartElement("item")

            reader.ReadStartElement("key")
            Dim key As String = DirectCast(keySerializer.Deserialize(reader), String)
            reader.ReadEndElement()

            reader.ReadStartElement("value")
            Dim value As MvvmDataGridSetting = DirectCast(valueSerializer.Deserialize(reader), MvvmDataGridSetting)
            reader.ReadEndElement()

            Me.Add(key, value)

            reader.ReadEndElement()
            reader.MoveToContent()
        End While
        reader.ReadEndElement()
    End Sub

    Public Sub WriteXml(writer As System.Xml.XmlWriter) Implements IXmlSerializable.WriteXml
        Dim keySerializer As New XmlSerializer(GetType(String))
        Dim valueSerializer As New XmlSerializer(GetType(MvvmDataGridSetting))

        For Each key As String In Me.Keys
            writer.WriteStartElement("item")

            writer.WriteStartElement("key")
            keySerializer.Serialize(writer, key)
            writer.WriteEndElement()

            writer.WriteStartElement("value")
            Dim value As MvvmDataGridSetting = Me(key)
            valueSerializer.Serialize(writer, value)
            writer.WriteEndElement()

            writer.WriteEndElement()
        Next
    End Sub
#End Region
End Class

''' <summary>
''' Datenklasse welche die einzelnen Einstellungen eines DataGrids abspeichert
''' </summary>
''' <remarks></remarks>
<Serializable>
Public Class MvvmDataGridSetting
    'Hier kommen die Informationen für ein DataGrid rein, welche
    Public Property ColumnDefinitions As New List(Of ColumnDefinition)
End Class

Public Class ColumnDefinition
    Property DisplayIndex As Integer

    Property Name As String

    Property Width As String

    Property SortDirection As ListSortDirection?

    Property SortMemberPath As String

End Class