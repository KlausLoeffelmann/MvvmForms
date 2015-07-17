Imports System.ComponentModel.Design
Imports System.Windows.Forms.Design
Imports System.ComponentModel

''' <summary>
''' Designer für das MvvmDataGrid
''' </summary>
''' <remarks></remarks>
Public Class MvvmDataGridDesigner
    Inherits ControlDesigner

    Private myVerbs As DesignerVerbCollection
    Private myInsertColumnsVerb As DesignerVerb = Nothing
    Private mySetDataContextTypeVerb As DesignerVerb = Nothing
    Private myEditColumnsVerb As DesignerVerb = Nothing

    ''' <summary>
    ''' Liefert die DataGrid-Instanz zu diesem Designer
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property MvvmDataGridInstance As MvvmDataGrid
        Get
            Return DirectCast(MyBase.Control, MvvmDataGrid)
        End Get
    End Property


    ' DesignerVerbCollection wird von ComponentDesigner überschrieben.
    ' Ein DesignerVerb ist quasi ein Command im Designer an das entsprechende Steuerelement.
    Public Overrides ReadOnly Property Verbs() As DesignerVerbCollection
        Get
            If myVerbs Is Nothing Then
                ' verbs-collection erstellen und definieren.
                myVerbs = New DesignerVerbCollection()

                myInsertColumnsVerb = New DesignerVerb("Create Columns from Data Source...", AddressOf InsertColumns)
                mySetDataContextTypeVerb = New DesignerVerb("Choose Data Source...", AddressOf SetDataContextType)
                myEditColumnsVerb = New DesignerVerb("Edit Columns...", AddressOf EditColumns)

                myVerbs.Add(mySetDataContextTypeVerb)
                myVerbs.Add(myInsertColumnsVerb)
                myVerbs.Add(myEditColumnsVerb)

            End If

            UpdateVerbEnablingStates()

            Return myVerbs
        End Get
    End Property

    ''' <summary>
    ''' Ruft auf dem MvvmDataGrid eine Methode auf, welche anhand des DataContextTypes bereits gebundene Spalten definiert
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub InsertColumns(sender As Object, e As EventArgs)
        Dim dataGrid = DirectCast(MyBase.Control, MvvmDataGrid)

        dataGrid.CreateColumnsDataSourceType()
    End Sub

    Private Sub EditColumns(sender As Object, e As EventArgs)
        'TODO: Column-Editor aufrufen.
    End Sub

    ''' <summary>
    ''' Ruft das gleiche Bearbeitenfenster für den DataContextType auf dem MvvmDataGrid auf, wie das im Property-Window
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub SetDataContextType(sender As Object, e As EventArgs)
        Dim myReturnValue As Type = Nothing

        Using frmTemp = New DataSourceTypeUIForm
            frmTemp.ComponentInstance = Me.MvvmDataGridInstance
            Dim site = frmTemp.ComponentInstance.Site
            If site IsNot Nothing Then
                Dim host = TryCast(site.GetService(GetType(IDesignerHost)), IDesignerHost)
                frmTemp.DialogResultValue = DirectCast(frmTemp.ComponentInstance, MvvmDataGrid).DataSourceType
                myReturnValue = frmTemp.DialogResultValue           ' falls wir den Dialog später abbrechen, soll das der Wert sein, den wir zurückgeben wollen
            End If
            frmTemp.ShowDialog()
            If frmTemp.DialogResult = System.Windows.Forms.DialogResult.OK Then
                myReturnValue = frmTemp.DialogResultValue
            End If
        End Using

        'Neuen Wert setzen:
        Me.MvvmDataGridInstance.DataSourceType = myReturnValue

        'und verb aktualisieren
        UpdateVerbEnablingStates()
    End Sub

    ''' <summary>
    ''' Prüft die Verfügbarkeit alle Designer Verbs
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub UpdateVerbEnablingStates()
        myInsertColumnsVerb.Enabled = Me.MvvmDataGridInstance.DataSourceType IsNot Nothing
    End Sub

End Class
