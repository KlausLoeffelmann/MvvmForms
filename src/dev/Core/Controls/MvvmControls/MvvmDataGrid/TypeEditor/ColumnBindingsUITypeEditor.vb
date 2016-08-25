Imports System.Drawing.Design
Imports System.Windows.Forms
Imports System.ComponentModel.Design
Imports System.Windows.Forms.Design

''' <summary>
''' TypeEditor welcher fuer Bearbeitung der Zellenbindungen einer Spalte verwendet werden soll
''' </summary>
''' <remarks></remarks>
Public Class ColumnBindingsUITypeEditor
    Inherits UITypeEditor

    Public Overrides Function GetEditStyle(context As System.ComponentModel.ITypeDescriptorContext) As System.Drawing.Design.UITypeEditorEditStyle
        Return UITypeEditorEditStyle.Modal
    End Function

    ''' <summary>
    ''' Wird aufgerufen, sobal die Bindungen bearbeitet werden sollen
    ''' </summary>
    ''' <param name="context"></param>
    ''' <param name="provider"></param>
    ''' <param name="value"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function EditValue(context As System.ComponentModel.ITypeDescriptorContext,
                                        provider As System.IServiceProvider, value As Object) As Object
        Dim wfEditService As IWindowsFormsEditorService = DirectCast(provider.GetService(
                                        GetType(IWindowsFormsEditorService)), IWindowsFormsEditorService)

        Dim myReturnValue As PropertyBindings = Nothing
        Dim selectedColumn = DirectCast(context.Instance, MvvmDataGridColumn)

        If wfEditService IsNot Nothing Then

            If Control.ModifierKeys = Keys.Shift Then
                If Debugger.IsAttached Then
                    Debugger.Break()
                End If
            End If

            Dim frmTemp = New frmMvvmPropertyAssignmentEx

            frmTemp.ControlToBind = selectedColumn
            frmTemp.DesignTimeAssemblyLoader = DirectCast(provider.GetService(GetType(IDesignTimeAssemblyLoader)), IDesignTimeAssemblyLoader)
            frmTemp.ReferenceService = DirectCast(provider.GetService(GetType(IReferenceService)), IReferenceService)
            frmTemp.TypeDiscoveryService = provider.GetService(Of ITypeDiscoveryService)()

            frmTemp.MvvmManager = selectedColumn

            frmTemp.PropertyBindings = frmTemp.MvvmManager.GetPropertyBindings(Nothing)

            'Form aufrufen
            wfEditService.ShowDialog(frmTemp)

            'Danach PropertyBindings abfragen und zurueckgeben
            myReturnValue = frmTemp.PropertyBindings
        End If

        Return myReturnValue

    End Function

End Class
