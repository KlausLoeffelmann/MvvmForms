Imports System.Drawing.Design
Imports System.Windows.Forms.Design
Imports System.ComponentModel
Imports System.ComponentModel.Design


''' <summary>
''' UI-Editor um ein CustomColumnTemplateType eines MvvmDataGrids auszuwaehlen
''' </summary>
''' <remarks></remarks>
Public Class MvvmDataGridCustomColumnTemplateTypeUIEditor
    Inherits UITypeEditor
    Public Overrides Function GetEditStyle(context As System.ComponentModel.ITypeDescriptorContext) As System.Drawing.Design.UITypeEditorEditStyle
        Return UITypeEditorEditStyle.DropDown
    End Function

    ''' <summary>
    ''' Wird aufgerufen, wenn ein TemplateExtenderTyp ausgewaehtl werden soll
    ''' </summary>
    ''' <param name="context"></param>
    ''' <param name="provider"></param>
    ''' <param name="value"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function EditValue(context As System.ComponentModel.ITypeDescriptorContext,
                                        provider As System.IServiceProvider, value As Object) As Object

        If System.Windows.Forms.Control.ModifierKeys = System.Windows.Forms.Keys.Shift Then
            If Debugger.IsAttached Then
                Debugger.Break()
            End If
        End If
        Dim wfEditService As IWindowsFormsEditorService = DirectCast(provider.GetService(
                                        GetType(IWindowsFormsEditorService)), IWindowsFormsEditorService)
        Dim myReturnValue As Type = Nothing

        If wfEditService IsNot Nothing Then
            Using frmTemp = New DataSourceSerializableTypeUIForm
                frmTemp.WFEditorService = wfEditService
                frmTemp.ComponentInstance = TryCast(context.Instance, Component)
                Dim site = frmTemp.ComponentInstance.Site
                If site IsNot Nothing Then
                    Dim host = TryCast(site.GetService(GetType(IDesignerHost)), IDesignerHost)
                    frmTemp.DialogResultValue = DirectCast(frmTemp.ComponentInstance, MvvmDataGrid).CustomColumnTemplateType
                    myReturnValue = frmTemp.DialogResultValue           ' falls wir den Dialog später abbrechen, soll das der Wert sein, den wir zurückgeben wollen
                End If
                wfEditService.DropDownControl(frmTemp)
                If frmTemp.DialogResult = System.Windows.Forms.DialogResult.OK Then
                    myReturnValue = frmTemp.DialogResultValue
                End If
            End Using
        End If

        If myReturnValue IsNot Nothing Then
            'Hier ueberpruefen wir den Typen:
            Dim test = TryCast(Activator.CreateInstance(myReturnValue), IMvvmColumnTemplateExtender)

            If test Is Nothing Then
                System.Windows.MessageBox.Show("Der Typ muss mindestens von IMvvmColumnTemplateExtender erben! Evtl. neu builden")
                myReturnValue = Nothing
            End If
        End If

        Return myReturnValue

    End Function
End Class