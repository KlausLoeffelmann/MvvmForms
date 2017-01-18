'*****************************************************************************************
'                                    DataSourceTypeUIForm.vb
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

Imports System.Windows.Forms.Design
Imports System.Reflection
Imports System.ComponentModel
Imports System.ComponentModel.Design
Imports System.Windows.Forms
Imports System.Drawing
Imports ActiveDevelop.MvvmBaseLib

Public Class DataSourceTypeUIForm

    Private myDialogResultValue As Type

    Private Class DisplayableAssemblyItem

        Property ShortName As String
        Property Target As String
        Property Version As String
        Property Location As String
        Property EntryPoint As MethodInfo
        Property FullName As String
        Property OrderNr As Integer
        Property Assembly As Assembly
        Property Types As List(Of Type)
    End Class

    Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        'Me.TopLevel = False
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.ControlBox = False
        Me.ShowInTaskbar = False
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow
    End Sub

    Public Sub InitializeListView(includeGACAssemblies As Boolean)

        Dim selectedNode As TreeNode = Nothing
        Me.DataSourceTreeView.Nodes.Clear()

        'Header setzen
        If ComponentInstance IsNot Nothing Then

            'Das mit dem gettype(object) ist doof, laut msdn darf der parameter bei dem funktionsaufruf auch null sein (siehe interface beschreiung). 
            'Die Implementierung sieht das aber wohl etwas anders, wenn wir kriegen hier eine argumentnullexception, deswegen so:

            Dim t_s = TypeDiscoveryService.GetTypes(GetType(Object), Not includeGACAssemblies)
            Dim typliste As New List(Of System.Type)

            For Each item In t_s
                typliste.Add(DirectCast(item, System.Type))
            Next

            Dim assemblyList As IEnumerable(Of DisplayableAssemblyItem) = Nothing

            assemblyList = From tItem In typliste
                           Let assembly = tItem.Assembly
                           Select assembly Distinct
                           Select New DisplayableAssemblyItem With
                                 {.FullName = assembly.FullName,
                                 .ShortName = assembly.GetName().Name,
                                 .Assembly = assembly,
                                 .EntryPoint = assembly.EntryPoint,
                                 .Location = assembly.Location,
                                 .Target = assembly.ImageRuntimeVersion,
                                 .Types = (From item In typliste
                                           Where item.Assembly Is assembly
                                           Order By item.Name).ToList
                                 }

            DataSourceTreeView.BeginUpdate()
            For Each assemblyItem In assemblyList
                Dim assemblyNode = New TreeNode() With {.Text = assemblyItem.ShortName,
                                                        .ToolTipText = assemblyItem.FullName,
                                                        .NodeFont = New Font(Me.Font, FontStyle.Bold)}

                For Each typeItem In assemblyItem.Types

                    Dim isBusinessClass As Boolean = False
                    Dim isINotifyPropertyChanged As Boolean = False
                    Dim isSystemClass As Boolean = False
                    Dim isBindableBase As Boolean = False

                    'If class is marked with the MvvmSystemElementAttribute, we do not list the class.
                    For Each att In typeItem.GetCustomAttributes(False)
                        If GetType(MvvmSystemElementAttribute).IsAssignableFrom(att.GetType) Then
                            isSystemClass = True
                        End If
                    Next

                    'We filter out Abstract classes, so BindableBase, MvvmViewModelBase, and all the derived system classes are not listed, here.
                    If isSystemClass Or typeItem.IsAbstract Then
                        Continue For
                    End If

                    For Each att In typeItem.GetCustomAttributes(True)
                        If GetType(BusinessClassAttribute).IsAssignableFrom(att.GetType) Then
                            isBusinessClass = True
                        End If
                    Next

                    If GetType(INotifyPropertyChanged).IsAssignableFrom(typeItem) Then
                        isINotifyPropertyChanged = True
                    End If
                    If GetType(BindableBase).IsAssignableFrom(typeItem) Then
                        isBindableBase = True
                    End If

                    If isBusinessClass Or isINotifyPropertyChanged Then
                        Dim currentTypeNode = New TreeNode()
                        currentTypeNode.Text = typeItem.Name
                        currentTypeNode.ToolTipText = typeItem.FullName
                        currentTypeNode.Tag = typeItem
                        If isINotifyPropertyChanged Or isBindableBase Then
                            If isINotifyPropertyChanged Then
                                currentTypeNode.NodeFont = New Font(Me.Font, FontStyle.Italic)
                            End If

                            If isBindableBase Then
                                currentTypeNode.NodeFont = New Font(Me.Font, FontStyle.Bold)
                            End If

                            If Not assemblyNode.IsExpanded Then
                                assemblyNode.Expand()
                            End If
                        End If
                        assemblyNode.Nodes.Add(currentTypeNode)
                        If Debugger.IsAttached Then
                            Debugger.Break()
                        End If
                        If typeItem Is Me.DialogResultValue Then
                            selectedNode = currentTypeNode
                        End If
                    End If
                Next
                If assemblyNode.Nodes.Count > 0 Then
                    DataSourceTreeView.Nodes.Add(assemblyNode)
                End If
            Next
            DataSourceTreeView.SelectedNode = selectedNode
            DataSourceTreeView.EndUpdate()
        End If
    End Sub

    Protected Overrides Sub OnLoad(e As EventArgs)
        MyBase.OnLoad(e)
        InitializeListView(False)
    End Sub

    Public Property DialogResultValue As Type
        Friend Set(value As Type)
            myDialogResultValue = value
        End Set
        Get
            Return myDialogResultValue
        End Get
    End Property

    Protected Overrides Sub OnClosed(e As System.EventArgs)
        MyBase.OnClosed(e)
        If WFEditorService IsNot Nothing Then WFEditorService.CloseDropDown()
    End Sub

    Private Sub btnOK_Click(sender As Object, e As EventArgs) Handles btnOK.Click

        If DataSourceTreeView.SelectedNode Is Nothing Then
            Me.DialogResultValue = Nothing
        Else
            Me.DialogResultValue = DirectCast(DataSourceTreeView.SelectedNode.Tag, Type)
        End If

        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Close()
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        myDialogResultValue = Nothing
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Close()
    End Sub

    Friend Property WFEditorService As IWindowsFormsEditorService
    Friend Property ComponentInstance As Component
    Friend ReadOnly Property ComponentDesigner As FormToBusinessClassManagerDesigner
        Get
            Return TryCast(DesignerHost.GetDesigner(Me.ComponentInstance), FormToBusinessClassManagerDesigner)
        End Get
    End Property

    Friend ReadOnly Property DesignerHost As IDesignerHost
        Get
            Return TryCast(ComponentInstance.Site.GetService(GetType(IDesignerHost)), IDesignerHost)
        End Get
    End Property

    Friend ReadOnly Property DesignTimeAssemblyLoader As IDesignTimeAssemblyLoader
        Get
            Return ComponentInstance.Site.GetService(Of IDesignTimeAssemblyLoader)()
        End Get
    End Property

    Friend ReadOnly Property ReferenceService As IReferenceService
        Get
            Return ComponentInstance.Site.GetService(Of IReferenceService)()
        End Get
    End Property

    Friend ReadOnly Property TypeDiscoveryService As ITypeDiscoveryService
        Get
            Return ComponentInstance.Site.GetService(Of ITypeDiscoveryService)()
        End Get
    End Property

    Private Sub DataSourceTreeView_AfterSelect(sender As Object, e As TreeViewEventArgs) Handles DataSourceTreeView.AfterSelect
        Dim selectedType = TryCast(DataSourceTreeView.SelectedNode.Tag, Type)
        If selectedType Is Nothing Then
            Me.btnOK.Enabled = False
        Else
            Me.DialogResultValue = selectedType
            Me.btnOK.Enabled = True
        End If
    End Sub

    Private Sub chkGACAssemblies_CheckedChanged(sender As Object, e As EventArgs) Handles chkGACAssemblies.CheckedChanged
        If chkGACAssemblies.Checked Then
            InitializeListView(True)
        Else
            InitializeListView(False)
        End If
    End Sub
End Class

