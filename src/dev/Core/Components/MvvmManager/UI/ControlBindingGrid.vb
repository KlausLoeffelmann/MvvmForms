'*****************************************************************************************
'                                    ControlBindingGrid.vb
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

Imports System.ComponentModel
Imports System.Windows.Forms

<ToolboxItem(False)>
Public Class ControlBindingGrid

    Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Private Sub PropertyBindingItemBindingSource_CurrentItemChanged(sender As Object, e As EventArgs) _
        Handles PropertyBindingItemBindingSource.CurrentItemChanged
        If PropertyBindingItemBindingSource.Current Is Nothing Then
            DeleteToolStripButton.Enabled = False
            EditToolStripButton.Enabled = False
        Else
            DeleteToolStripButton.Enabled = True
            EditToolStripButton.Enabled = True
        End If
    End Sub


    Public ReadOnly Property AddButton As ToolStripButton
        Get
            Return AddToolStripButton
        End Get
    End Property

    Public ReadOnly Property DeleteButton As ToolStripButton
        Get
            Return DeleteToolStripButton
        End Get
    End Property

    Public ReadOnly Property ChangeButton As ToolStripButton
        Get
            Return EditToolStripButton
        End Get
    End Property

    Public ReadOnly Property GridDataSource As BindingSource
        Get
            Return PropertyBindingItemBindingSource
        End Get
    End Property

    Public ReadOnly Property BindingGrid As DataGridView
        Get
            Return myBindingDataGrid
        End Get
    End Property

End Class
