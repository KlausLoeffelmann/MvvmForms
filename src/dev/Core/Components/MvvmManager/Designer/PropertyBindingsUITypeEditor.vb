'*****************************************************************************************
'                                 PropertyBindingsUITypeEditor.vb
'                    =======================================================
'
'          Part of MvvmForms - The Component Library for bringing the Model-View-Viewmodel
'                              pattern to Data Centric Windows Forms Apps in an easy,
'                              feasible and XAML-compatible way.
'
'                    Copyright -2015 by Klaus Loeffelmann
'
'    This program is free software; you can redistribute it and/or modify
'    it under the terms of the GNU General Public License as published by
'    the Free Software Foundation; either version 2 of the License, or
'    (at your option) any later version.
'
'    This program is distributed in the hope that it will be useful,
'    but WITHOUT ANY WARRANTY; without even the implied warranty Of
'    MERCHANTABILITY Or FITNESS FOR A PARTICULAR PURPOSE.  See the
'    GNU General Public License For more details.
'
'    You should have received a copy of the GNU General Public License along
'    with this program; if not, write to the Free Software Foundation, Inc.,
'    51 Franklin Street, Fifth Floor, Boston, MA 02110-1301 USA.
'
'    MvvmForms is dual licenced. A permissive licence can be obtained - CONTACT INFO:
'
'                       ActiveDevelop
'                       Bremer Str. 4
'                       Lippstadt, DE-59555
'                       Germany
'                       email: mvvmforms at activedevelop . de. 
'*****************************************************************************************

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
            Dim frmTemp = New frmMvvmPropertyAssignmentEx()

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
