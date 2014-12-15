Imports System.ComponentModel
Imports System.ComponentModel.Design
Imports System.Windows.Forms
Imports System.Drawing

Public Class frmManageBindings

    Private enableDebugger As Boolean = True

    Private myDesignerHost As IDesignerHost
    Private myContainer As IContainer
    Private myMvvmManager As MvvmManager
    Private myMvvmPropAssignmentForm As New frmMvvmPropertyAssignment With
                                            {.EmbeddedMode = True}
    Private myChangeService As IComponentChangeService
    Private myPropertyChangeInformerCallBack As Action()

    Sub New()

        ' This call is required by the designer.
        InitializeComponent()
    End Sub

    Shadows Sub ShowDialog(host As IDesignerHost, container As IContainer,
                           mvvmManager As MvvmManager, changeService As IComponentChangeService)

        myDesignerHost = host
        myContainer = container
        myMvvmManager = mvvmManager
        myChangeService = changeService

        If Debugger.IsAttached Then
            Debugger.Break()
        End If

        ControlTreeView.Nodes.Clear()
        ImageList1.TransparentColor = Color.FromArgb(255, 0, 255)

        'Dim addNode As Action(Of ControlTreeNode) = Sub(n) TreeView1.Nodes.Add(n)
        Dim currentDesignControl As Control = TryCast(host.RootComponent, Control)
        If currentDesignControl Is Nothing Then
            MessageBox.Show("Der MVVM Manager kann nur verwendet werden, wenn ein Form oder Control designt wird.")
            Me.Close()
            Return
        End If

        Dim rootNode As New ControlTreeNode(currentDesignControl)
        ControlTreeView.Nodes.Add(rootNode)

        For Each c As Control In currentDesignControl.Controls
            AddChildren(c, rootNode, container)
        Next

        rootNode.Expand()
        PlaceMvvmAssigningDialog()
        myMvvmPropAssignmentForm.DesignTimeAssemblyLoader = DirectCast(myDesignerHost.GetService(GetType(IDesignTimeAssemblyLoader)), IDesignTimeAssemblyLoader)
        myMvvmPropAssignmentForm.ReferenceService = DirectCast(myDesignerHost.GetService(GetType(IReferenceService)), IReferenceService)
        myMvvmPropAssignmentForm.TypeDiscoveryService = myDesignerHost.GetService(Of ITypeDiscoveryService)()
        MyBase.ShowDialog()
    End Sub

    Private Sub PlaceMvvmAssigningDialog()
        myMvvmPropAssignmentForm.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        myMvvmPropAssignmentForm.TopLevel = False
        myMvvmPropAssignmentForm.Location = New Point(0, 0)
        myMvvmPropAssignmentForm.Dock = DockStyle.Fill
        'myMvvmPropAssignmentForm.Visible = True
 

        MainSplitContainer.Panel2.Controls.Add(myMvvmPropAssignmentForm)
        myMvvmPropAssignmentForm.MvvmManager = myMvvmManager
        myMvvmPropAssignmentForm.ComponentDesigner = TryCast(myDesignerHost.GetDesigner(DirectCast(myMvvmPropAssignmentForm.MvvmManager, IComponent)), MvvmManagerDesigner)

    End Sub

    Private Sub AddChildren(c As Control, parentNode As ControlTreeNode, container As IContainer)
        ' keine Ahnung, wie die Filterung in der Dokumentengliederung funktioniert 
        ' meine Lösung ist "frei schnautze"
        If Debugger.IsAttached Then
            Debugger.Break()
        End If

        'If String.IsNullOrWhiteSpace(c.Name) Then Return ' ohne Namen keine benannte Instanz -> !?!?!? keine Datenbindung !?!?!?!

        'Dim isOnCurrentDesignedControl = False
        'For Each cont As Component In container.Components
        '    Dim contAsC = TryCast(cont, Control)
        '    If contAsC IsNot Nothing AndAlso contAsC.Name = c.Name Then
        '        isOnCurrentDesignedControl = True
        '        Exit For
        '    End If
        'Next
        'If Not isOnCurrentDesignedControl Then Return


        If enableDebugger Then Debug.WriteLine("--- Control :" & c.Name & " ------")
        'If String.IsNullOrWhiteSpace(c.Name) AndAlso Debugger.IsAttached Then
        '    Debugger.Break()
        'End If
        Dim imageIdx = 0
        Dim conType = c.GetType

        'Dim custAttribs = (From custAttr In conType.GetCustomAttributes(True)).ToList
        'For Each caItem In custAttribs
        '    If enableDebugger Then Debug.WriteLine(caItem.ToString & " --------------- " & caItem.GetType.FullName)
        'Next


        Try
            ' das hier scheint zu funktionieren -> ob das immer ok ist -> kA

            Dim resNames = conType.Assembly.GetManifestResourceNames
            Dim matches = (From resItem In resNames Where resItem.StartsWith(conType.FullName))
            If matches.Count > 0 Then
                Dim bmp As System.Drawing.Image = System.Drawing.Image.FromStream(conType.Assembly.GetManifestResourceStream(matches(0)))
                imageIdx = ImageList1.Images.Count
                ImageList1.Images.Add(bmp)
            End If
        Catch
        End Try
        If imageIdx = 0 Then
            Dim custAttribs = (From custAttr In conType.GetCustomAttributes(GetType(System.Drawing.ToolboxBitmapAttribute), True)).ToList
            For Each caItem In custAttribs
                If enableDebugger Then Debug.WriteLine(caItem.ToString & " --------------- " & caItem.GetType.FullName)
                Dim toolboxImageAttr = TryCast(caItem, System.Drawing.ToolboxBitmapAttribute)
                If toolboxImageAttr IsNot Nothing Then
                    Dim bmp = toolboxImageAttr.GetImage(c)
                    imageIdx = ImageList1.Images.Count
                    ImageList1.Images.Add(bmp)
                End If
            Next

        End If

        Dim newC As New ControlTreeNode(c)
        parentNode.Nodes.Add(newC)
        newC.ImageIndex = imageIdx
        newC.SelectedImageIndex = imageIdx
        'newC.StateImageIndex = imageIdx
        Dim sortedControls = (From cItems In c.Controls
                              Order By DirectCast(cItems, Control).Location.Y,
                                     DirectCast(cItems, Control).Location.X).ToList

        For Each child As Control In sortedControls
            AddChildren(child, newC, container)
        Next
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs)
        ' test umbenennung -> kein Refresh im Designer; erst nach erneutem öffnen
        ' vermutlich liegts daran, dass keine Transaktion gestartet wird -> oder what ever

        If ControlTreeView.SelectedNode Is Nothing Then Return
        Dim ctn = TryCast(ControlTreeView.SelectedNode, ControlTreeNode)
        If ctn IsNot Nothing Then
            Dim c As Control = TryCast(ctn.Component, Control)
            If c.Name.EndsWith("___") Then
                c.Name = c.Name.Substring(0, c.Name.Length - 3)
            Else
                c.Name = c.Name & "___"
            End If
        End If
    End Sub

    Private Sub ControlTreeView_BeforeSelect(sender As Object, e As TreeViewCancelEventArgs) Handles ControlTreeView.BeforeSelect
        If myMvvmPropAssignmentForm.IsDirty Then
            Dim dr = MessageBox.Show("Möchten Sie Ihre Änderungen speichern, befor Sie die nächste" & vbNewLine &
                            "Eigenschaftenzuweisung bearbeiten?", "Änderungen speichern?",
                            MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If dr = Windows.Forms.DialogResult.Yes Then
                myMvvmPropAssignmentForm.CommitChanges()
            Else
                myMvvmPropAssignmentForm.IgnoreChanges()
            End If
        End If
    End Sub

    Private Sub ControlTreeView_AfterSelect(sender As Object, e As TreeViewEventArgs) Handles ControlTreeView.AfterSelect
        If e.Node.Nodes IsNot Nothing And e.Node.Nodes.Count > 0 Then
            ExpandCollapseThisToolStripButton.Enabled = True
        Else
            ExpandCollapseThisToolStripButton.Enabled = False
        End If

        Try
            Dim ctrlTemp = DirectCast(DirectCast(e.Node, ControlTreeNode).Component, Control)
            myMvvmPropAssignmentForm.PropertyBindings = myMvvmPropAssignmentForm.MvvmManager.GetPropertyBindings(ctrlTemp)
            myMvvmPropAssignmentForm.ControlToBind = ctrlTemp
            If myMvvmPropAssignmentForm.AfterAddEditOrDeleteCallbackAction Is Nothing Then
                myMvvmPropAssignmentForm.AfterAddEditOrDeleteCallbackAction = New Action(
                    Sub()

                        If Debugger.IsAttached Then
                            Debugger.Break()
                        End If

                        If myChangeService IsNot Nothing Then
                            Dim member As PropertyDescriptor = TypeDescriptor.GetProperties(
                                myMvvmPropAssignmentForm.ControlToBind).Item("PropertyBindings")
                            ' Designer Transaction anlegen, damit nur ein Undo-Item dabei rauskommt
                            Using transaction = myDesignerHost.CreateTransaction("Delete Controls")

                                myChangeService.OnComponentChanging(myMvvmPropAssignmentForm.ControlToBind,
                                                                    member)
                                myMvvmManager.SetPropertyBindings(DirectCast(myMvvmPropAssignmentForm.ControlToBind, Control),
                                                                  myMvvmPropAssignmentForm.PropertyBindings)
                                myChangeService.OnComponentChanged(myMvvmPropAssignmentForm.ControlToBind,
                                                                    member, Nothing, Nothing)
                                transaction.Commit()
                            End Using
                        End If
                    End Sub)
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
        myMvvmPropAssignmentForm.Visible = True
    End Sub

    Private Sub ExpandCollapseThisToolStripButton_Click(sender As Object, e As EventArgs) Handles ExpandCollapseThisToolStripButton.Click
        If ControlTreeView.SelectedNode IsNot Nothing AndAlso
            ControlTreeView.SelectedNode.Nodes IsNot Nothing AndAlso
                ControlTreeView.SelectedNode.Nodes.Count > 0 Then
            If ControlTreeView.SelectedNode.IsExpanded Then
                ControlTreeView.SelectedNode.Collapse(False)
            Else
                ControlTreeView.SelectedNode.ExpandAll()
            End If
        End If
    End Sub

    Private Sub ExpandAllToolStripButton_Click(sender As Object, e As EventArgs) Handles ExpandAllToolStripButton.Click
        ControlTreeView.ExpandAll()
    End Sub

    Private Sub CollapseAllToolStripButton_Click(sender As Object, e As EventArgs) Handles CollapseAllToolStripButton.Click
        ControlTreeView.CollapseAll()
    End Sub

    Private Sub ShowPrintReportButton_Click(sender As Object, e As EventArgs) Handles ShowPrintReportButton.Click
        'form mit dem reportviewer anlegen
        Dim reportDialog As New PrintReportForm

        'reporviewer processing mode auf local setzen
        reportDialog.ReportViewer1.ProcessingMode = Microsoft.Reporting.WinForms.ProcessingMode.Local

        'datasource zuweisen und die BindingInformationen laden
        reportDialog.BindingInformationForPrintingBindingSource.DataSource = PropertyBindingsForPrinting.getBindingInformation(myMvvmManager)

        'anzeigen
        reportDialog.ShowDialog()
    End Sub
End Class
