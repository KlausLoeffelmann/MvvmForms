Imports System.Collections.ObjectModel

Public Class CBOTestForm
    Private Sub CBOTestForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        NullableValueComboBox1.ItemSource = New ObservableCollection(Of Person)() From {New Person("Stephan"), New Person("Daniel"), New Person("Guido"), New Person("Andreas"), New Person("Stepan"), New Person("Andrea")}

    End Sub

    Private Sub NullableValueComboBox1_SelectedItemChanged(sender As Object, e As EventArgs) Handles NullableValueComboBox1.SelectedItemChanged
        SelectionChangedListBox.Items.Add(DirectCast(NullableValueComboBox1.SelectedItem, Person).Name)
    End Sub
End Class

Public Class Person
    Sub New(name As String)
        Me.Name = name
    End Sub

    Private _name As String
    Public Property Name As String
        Get
            Return _name
        End Get
        Set(ByVal value As String)
            _name = value
        End Set
    End Property
End Class