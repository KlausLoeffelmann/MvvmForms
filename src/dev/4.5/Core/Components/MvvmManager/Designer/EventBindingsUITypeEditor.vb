Imports System.Drawing.Design
Imports System.Windows.Forms.Design
Imports System.ComponentModel
Imports System.ComponentModel.Design
Imports System.Windows.Forms

Public Class EventBindingsUITypeEditor
    Inherits UITypeEditor

    Public Overrides Function GetEditStyle(context As System.ComponentModel.ITypeDescriptorContext) As System.Drawing.Design.UITypeEditorEditStyle
        Return UITypeEditorEditStyle.Modal
    End Function

    Public Overrides Function EditValue(context As System.ComponentModel.ITypeDescriptorContext, provider As System.IServiceProvider, value As Object) As Object
        'Return MyBase.EditValue(context, provider, value)
        Dim wfEditService As IWindowsFormsEditorService = DirectCast(provider.GetService(
                                        GetType(IWindowsFormsEditorService)), IWindowsFormsEditorService)

        Dim myReturnValue As ObservableBindingList(Of EventBindingItem) = Nothing

        If wfEditService IsNot Nothing Then
            Dim frmTemp = New frmMvvmEventAssignment
            frmTemp.ComponentInstance = TryCast(context.Instance, Control)

            'Um den MVVM-Manager zu finden, iterieren wir jetzt durch alle Items des Containers
            If frmTemp.ComponentInstance.Container IsNot Nothing Then
                frmTemp.MvvmManager = DirectCast((From items In frmTemp.ComponentInstance.Container.Components
                                        Where GetType(MvvmManager).IsAssignableFrom(items.GetType)).SingleOrDefault, MvvmManager)
                frmTemp.EventBindings = frmTemp.MvvmManager.GetEventBindings(frmTemp.ComponentInstance)
            End If

            wfEditService.ShowDialog(frmTemp)
            myReturnValue = frmTemp.EventBindings
        End If

        Return myReturnValue

    End Function

End Class
