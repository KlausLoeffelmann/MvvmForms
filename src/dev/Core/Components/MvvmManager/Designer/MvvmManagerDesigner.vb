'*****************************************************************************************
'                                    MvvmManagerDesigner.vb
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

Imports System.ComponentModel.Design
Imports System.ComponentModel
Imports System.Windows.Forms

Public Class MvvmManagerDesigner
    Inherits ComponentDesigner

    Private myVerbs As DesignerVerbCollection
    Private myBindingManagerFormVerb As DesignerVerb = Nothing
    Private myCurrentChangeService As IComponentChangeService

    Sub New()
    End Sub

    Public Overrides ReadOnly Property Verbs() As DesignerVerbCollection
        Get

            myCurrentChangeService = TryCast(Me.GetService(GetType(IComponentChangeService)), IComponentChangeService)

            If myVerbs Is Nothing Then
                ' Verbs-Collection erstellen und definieren.
                myVerbs = New DesignerVerbCollection()
                myBindingManagerFormVerb =
                    New DesignerVerb("Manage View/ViewModel Bindings...",
                            Sub(sender As Object, e As EventArgs)
                                Dim host As IDesignerHost = TryCast(Me.GetService(GetType(IDesignerHost)), IDesignerHost)

                                If host Is Nothing Then
                                    'Beep()
                                    MessageBox.Show("Could not retrieve Designer Host.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                    Return
                                End If

                                Dim container As IContainer = TryCast(Me.GetService(GetType(IContainer)), IContainer)

                                If Debugger.IsAttached Then
                                    Debugger.Break()
                                End If
                                Dim frm2show As New frmManageBindings
                                frm2show.ShowDialog(host, container, DirectCast(Me.Component, MvvmManager), myCurrentChangeService)

                            End Sub)

                myVerbs.Add(myBindingManagerFormVerb)
                myBindingManagerFormVerb.Enabled = True
            End If
            Return myVerbs
        End Get
    End Property

End Class
