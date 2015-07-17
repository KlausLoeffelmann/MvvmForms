Imports System.Windows.Forms
Imports System.ComponentModel
Imports System.Drawing.Design

Public Class NullableDataGridView
    Inherits DataGridView

    Public Event DataRowClicked(sender As Object, e As DataViewRowEventArgs)
    Public Event DataRowDoubleClicked(sender As Object, e As DataViewRowEventArgs)
    Public Event SelectedDataViewRowChanged(sender As Object, e As DataViewRowEventArgs)

    Private myItemsSourceType As Type
    Private myItemsSource As IEnumerable
    Private WithEvents myBindingSource As BindingSource

    Sub New()
        MyBase.New()
        Me.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        Me.DoubleBuffered = True
    End Sub

    ''' <summary>
    ''' Bestimmt oder ermittelt, welchen Typ ViewModel die MVVMManager-Komponente später zur Laufzeit verarbeiten soll.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>Diese Eigenschaft ist wichtig zu setzen, BEVOR Zuweisungen über die PropertyBindings- bzw. EventBindings-Eigenschaften 
    ''' der entsprechenden Steuerelemente in der View gemacht werden können. Anhand dieser Eigenschaft erstellt die UI 
    ''' dieser Komponente eine Liste der bindbaren Eigenschaften und Ereignisse des ViewModels, und bietet diese im Dialog an, der 
    ''' aufgerufen wird, wenn im Eigenschaftenfenster auf die ...-Schaltfläche für die PropertyBindings-Eigenschaft bzw. 
    ''' der EventBindings-Eigenschaft geklickt wird.</remarks>
    <Editor(GetType(DataSourceTypeUIEditor), GetType(UITypeEditor)),
    Description("Bestimmt oder ermittelt, von welchem Typ die Quelliste ist, die diese DataGridView verarbeiten soll.")>
    Property ItemsSourceType As Type
        Get
            Return myItemsSourceType
        End Get
        Set(value As Type)
            myItemsSourceType = value
        End Set
    End Property

    Property ItemsSource As IEnumerable
        Get
            Return myItemsSource
        End Get
        Set(value As IEnumerable)
            If Not Object.Equals(value, myItemsSource) Then
                If myBindingSource IsNot Nothing Then
                    Me.DataSource = Nothing
                    myBindingSource.Dispose()
                End If

                myItemsSource = value

                If value IsNot Nothing Then
                    myBindingSource = New BindingSource()
                    myBindingSource.DataSource = myItemsSource
                    Me.DataSource = myBindingSource
                End If
            End If
        End Set
    End Property

    Protected Overrides Sub OnCellDoubleClick(e As DataGridViewCellEventArgs)
        MyBase.OnCellDoubleClick(e)
        RaiseEvent DataRowDoubleClicked(Me, New DataViewRowEventArgs(Me.Rows(e.RowIndex).DataBoundItem))
    End Sub

    Protected Overrides Sub OnCellClick(e As DataGridViewCellEventArgs)
        MyBase.OnCellClick(e)
        RaiseEvent DataRowClicked(Me, New DataViewRowEventArgs(Me.Rows(e.RowIndex).DataBoundItem))
    End Sub

    Private Sub myBindingSource_CurrentItemChanged(sender As Object, e As EventArgs) Handles myBindingSource.CurrentItemChanged
        If myBindingSource IsNot Nothing Then
            RaiseEvent SelectedDataViewRowChanged(Me, New DataViewRowEventArgs(myBindingSource.Current))
        End If
    End Sub
End Class


Public Class DataViewRowEventArgs
    Inherits EventArgs

    Sub New(dataViewRow As Object)
        Me.DataViewRow = dataViewRow
    End Sub

    Public Property DataViewRow As Object
End Class