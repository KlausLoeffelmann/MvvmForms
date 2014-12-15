Imports ActiveDevelop.EntitiesFormsLib
Imports System.Windows.Forms

<CLSCompliant(False)>
Public Class ObservableCollectionTestForm

    Private myContacts As ObservableBindingList(Of CustomerTest) = New ObservableBindingList(Of CustomerTest)(CustomerTest.RandomCustomers(100, 10))

    Private Sub ObservableCollectionTestForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        CustomerTestBindingSource.DataSource = myContacts

    End Sub

    Protected Overrides Sub OnShown(e As EventArgs)
        MyBase.OnShown(e)
        Application.DoEvents()
    End Sub

    Public Property Contacts As ObservableBindingList(Of CustomerTest)
        Get
            Return myContacts
        End Get
        Set(value As ObservableBindingList(Of CustomerTest))
            myContacts = value
        End Set
    End Property

End Class