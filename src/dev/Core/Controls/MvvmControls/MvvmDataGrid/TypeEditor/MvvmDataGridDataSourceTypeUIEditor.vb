Imports System.Drawing.Design
Imports System.Windows.Forms.Design
Imports System.ComponentModel
Imports System.ComponentModel.Design

''' <summary>
''' Ableitung von DataSourceTypeUIForm, welches ein SerializableAttribute vorschreibt 
''' </summary>
''' <remarks></remarks>
Public Class DataSourceSerializableTypeUIForm
    Inherits DataSourceTypeUIForm

End Class

''' <summary>
''' UI-Editor um ein DataSourceType eines MvvmDataGrids auszuwaehlen
''' </summary>
''' <remarks></remarks>
Public Class MvvmDataGridDataSourceTypeUIEditor
    Inherits UITypeEditor

    Public Overrides Function GetEditStyle(context As System.ComponentModel.ITypeDescriptorContext) As System.Drawing.Design.UITypeEditorEditStyle
        Return UITypeEditorEditStyle.Modal
    End Function

    ''' <summary>
    ''' Wird aufgerufen, wenn ein Typ fuer die DataSource festgelegt werden soll 
    ''' </summary>
    ''' <param name="context"></param>
    ''' <param name="provider"></param>
    ''' <param name="value"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function EditValue(context As System.ComponentModel.ITypeDescriptorContext,
                                        provider As System.IServiceProvider, value As Object) As Object
        'Return MyBase.EditValue(context, provider, value)
        If System.Windows.Forms.Control.ModifierKeys = System.Windows.Forms.Keys.Shift Then
            If Debugger.IsAttached Then
                Debugger.Break()
            End If
        End If
        Dim wfEditService As IWindowsFormsEditorService = DirectCast(provider.GetService(
                                        GetType(IWindowsFormsEditorService)), IWindowsFormsEditorService)

        Dim myReturnValue As Type = Nothing

        If wfEditService IsNot Nothing Then
            Using frmTemp = New DataSourceTypeUIForm
                frmTemp.WFEditorService = wfEditService
                frmTemp.ComponentInstance = TryCast(context.Instance, Component)
                Dim site = frmTemp.ComponentInstance.Site
                If site IsNot Nothing Then
                    Dim host = TryCast(site.GetService(GetType(IDesignerHost)), IDesignerHost)
                    frmTemp.DialogResultValue = DirectCast(frmTemp.ComponentInstance, MvvmDataGrid).DataSourceType
                    myReturnValue = frmTemp.DialogResultValue           ' falls wir den Dialog später abbrechen, soll das der Wert sein, den wir zurückgeben wollen
                End If
                wfEditService.ShowDialog(frmTemp)
                If frmTemp.DialogResult = System.Windows.Forms.DialogResult.OK Then
                    myReturnValue = frmTemp.DialogResultValue
                End If
            End Using
        End If

        Return myReturnValue

    End Function


End Class
