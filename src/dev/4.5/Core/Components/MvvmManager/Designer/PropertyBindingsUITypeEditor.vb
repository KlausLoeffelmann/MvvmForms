Imports System.Drawing.Design
Imports System.Windows.Forms.Design
Imports System.ComponentModel
Imports System.ComponentModel.Design
Imports System.Windows.Forms

Public Class PropertyBindingsUITypeEditor
    Inherits UITypeEditor

    Public Overrides Function GetEditStyle(context As System.ComponentModel.ITypeDescriptorContext) As System.Drawing.Design.UITypeEditorEditStyle
        Return UITypeEditorEditStyle.Modal
    End Function

    Public Overrides Function EditValue(context As System.ComponentModel.ITypeDescriptorContext,
                                        provider As System.IServiceProvider, value As Object) As Object
        'Return MyBase.EditValue(context, provider, value)
        Dim wfEditService As IWindowsFormsEditorService = DirectCast(provider.GetService(
                                        GetType(IWindowsFormsEditorService)), IWindowsFormsEditorService)

        Dim myReturnValue As PropertyBindings = Nothing

        If wfEditService IsNot Nothing Then

            If Control.ModifierKeys = Keys.Shift Then
                If Debugger.IsAttached Then
                    Debugger.Break()
                End If
            End If

            Dim controlToBind = TryCast(context.Instance, Control)
            Dim frmTemp = New frmMvvmPropertyAssignment

            frmTemp.ControlToBind = controlToBind

            Dim host = TryCast(controlToBind.Site.GetService(GetType(IDesignerHost)), IDesignerHost)
            Dim cDesigner = host.GetDesigner(controlToBind)
            frmTemp.DesignerHost = host
            frmTemp.DesignTimeAssemblyLoader = DirectCast(provider.GetService(GetType(IDesignTimeAssemblyLoader)), IDesignTimeAssemblyLoader)
            frmTemp.ReferenceService = DirectCast(provider.GetService(GetType(IReferenceService)), IReferenceService)
            frmTemp.TypeDiscoveryService = provider.GetService(Of ITypeDiscoveryService)()
            'Um den MVVM-Manager zu finden, iterieren wir jetzt durch alle Items des Containers
            If controlToBind.Container IsNot Nothing Then
                frmTemp.MvvmManager = DirectCast((From items In controlToBind.Container.Components
                                        Where GetType(MvvmManager).IsAssignableFrom(items.GetType)).SingleOrDefault, MvvmManager)
                If frmTemp.MvvmManager IsNot Nothing Then
                    frmTemp.ComponentDesigner = TryCast(host.GetDesigner(DirectCast(frmTemp.MvvmManager, IComponent)), MvvmManagerDesigner)
                End If

                frmTemp.PropertyBindings = frmTemp.MvvmManager.GetPropertyBindings(controlToBind)
            End If

            wfEditService.ShowDialog(frmTemp)
            myReturnValue = frmTemp.PropertyBindings
        End If

        Return myReturnValue

    End Function

End Class
