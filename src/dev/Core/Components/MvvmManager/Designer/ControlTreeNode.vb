'*****************************************************************************************
'                                    ControlTreeNode.vb
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

Imports System.Windows.Forms
Imports System.ComponentModel

Public Class ControlTreeNode
    Inherits TreeNode

    Public Sub New()
    End Sub

    Public Sub New(comp As Component)
        SetComponent(comp)
    End Sub

    Private myComponent As WeakReference
    Private myCompIdentifier As String

    Public ReadOnly Property Component As Component
        Get
            If myComponent.IsAlive Then
                Return CType(myComponent.Target, Component)
            Else
                Throw New ObjectDisposedException("Das Object '" & myCompIdentifier & "' ist nicht mehr vorhanden.")
            End If
        End Get
    End Property

    Public Sub SetComponent(comp As Component)
        If comp Is Nothing Then Throw New ArgumentNullException("comp")
        myComponent = New WeakReference(comp)
        Dim c = TryCast(comp, Control)
        Dim typname = comp.GetType.FullName
        Dim tttxt As String = String.Format("... ({0})", comp.GetType.Name)
        If c IsNot Nothing Then
            myCompIdentifier = String.Format("'{0}' ({1})", c.Name, typname)
            tttxt = String.Format("{0} --- ({1})", c.Name, comp.GetType.Name)
        Else
            myCompIdentifier = String.Format("... ({0})", typname)
        End If

        Dim textTemp = If(String.IsNullOrWhiteSpace(c.Name), "...", c.Name)

        'Versuchen, den Inhalt der Texteigenschaft in Klammern dahinter zu setzen.
        Dim propInfo = c.GetType.GetProperty("Text")
        Dim propInfoText As String = ""
        If propInfo IsNot Nothing Then
            propInfoText = " (" & propInfo.GetValue(c, Nothing).ToString & ")"
        End If

        MyBase.Text = textTemp & propInfoText
        MyBase.ToolTipText = tttxt & propInfoText
    End Sub
End Class
