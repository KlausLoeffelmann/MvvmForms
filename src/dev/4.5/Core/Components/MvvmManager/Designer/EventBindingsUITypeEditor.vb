'*****************************************************************************************
'                                  EventBindingsUITypeEditor.vb
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
