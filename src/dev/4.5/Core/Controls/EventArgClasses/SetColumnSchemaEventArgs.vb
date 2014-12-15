Imports System.Windows.Forms
Imports System.Collections.ObjectModel

Public Class GetColumnSchemaEventArgs
    Inherits EventArgs

    Public Sub New(ByVal schemaFieldnames As DataGridViewColumnFieldnames)
        Me.SchemaFieldnames = schemaFieldnames
    End Sub

    Public Sub New(ByVal schemaFieldnames As DataGridViewColumnFieldnames, FillUpGrid As Boolean)
        Me.SchemaFieldnames = schemaFieldnames
    End Sub

    Public Property SchemaFieldnames As DataGridViewColumnFieldnames
    Public Property AutoSizeColumnsMode As DataGridViewAutoSizeColumnsMode
End Class

Public Class DataGridViewColumnFieldname
    Public Sub New()
        MyBase.new()
    End Sub

    Public Sub New(ByVal schemaFieldName As String)
        Me.SchemaFieldName = schemaFieldName
        Me.DisplayName = schemaFieldName
    End Sub

    Public Sub New(ByVal schemaFieldName As String, ByVal displayName As String)
        Me.SchemaFieldName = schemaFieldName
        Me.DisplayName = displayName
    End Sub

    Public Sub New(ByVal schemaFieldName As String, fillWeight As Integer, ByVal displayName As String)
        Me.SchemaFieldName = schemaFieldName
        Me.DisplayName = displayName
        Me.FillWeight = fillWeight
        Me.AutoSizeColumnMode = DataGridViewAutoSizeColumnMode.Fill
    End Sub

    Public Sub New(ByVal schemaFieldName As String, ByVal displayName As String, fixedWidth As Integer)
        Me.SchemaFieldName = schemaFieldName
        Me.DisplayName = displayName
        Me.AutoSizeColumnMode = DataGridViewAutoSizeColumnMode.NotSet
        Me.FixedWidth = fixedWidth
    End Sub

    Public Property SchemaFieldName As String
    Public Property DisplayName As String
    Public Property OrdinalNo As Integer
    Public Property FillWeight As Integer
    Public Property AutoSizeColumnMode As DataGridViewAutoSizeColumnMode
    Public Property FixedWidth As Integer?
End Class

Public Class DataGridViewColumnFieldnames
    Inherits KeyedCollection(Of String, DataGridViewColumnFieldname)

    Private myHighestOridinalNo As Integer

    Public Overloads Sub Add(ByVal schemaFieldname As String, ByVal displayName As String)
        Me.Add(New DataGridViewColumnFieldname(schemaFieldname, displayName))
    End Sub

    Public Overloads Sub Add(ByVal schemaFieldname As String, ByVal displayName As String, fixedWidth As Integer)
        Me.Add(New DataGridViewColumnFieldname(schemaFieldname, displayName, fixedWidth))
    End Sub

    Public Overloads Sub Add(ByVal schemaFieldname As String, fillWeight As Integer, ByVal displayName As String)
        Me.Add(New DataGridViewColumnFieldname(schemaFieldname, fillWeight, displayName))
    End Sub

    Public Overloads Sub Add(ByVal schemaFieldname As [Enum], ByVal displayName As String)
        Me.Add(New DataGridViewColumnFieldname(schemaFieldname.ToString, displayName))
    End Sub

    Public Overloads Sub Add(ByVal schemaFieldname As [Enum], ByVal displayName As String, fixedWidth As Integer)
        Me.Add(New DataGridViewColumnFieldname(schemaFieldname.ToString, displayName, fixedWidth))
    End Sub

    Public Overloads Sub Add(ByVal schemaFieldname As [Enum], fillWeight As Integer, ByVal displayName As String)
        Me.Add(New DataGridViewColumnFieldname(schemaFieldname.ToString, fillWeight, displayName))
    End Sub

    Protected Overrides Function GetKeyForItem(ByVal item As DataGridViewColumnFieldname) As String
        item.OrdinalNo = myHighestOridinalNo
        myHighestOridinalNo += 1
        Return item.SchemaFieldName
    End Function
End Class
