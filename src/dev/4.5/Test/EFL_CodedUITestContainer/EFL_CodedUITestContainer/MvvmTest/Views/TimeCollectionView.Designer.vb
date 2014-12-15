<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class TimeCollectionView
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.AddButton = New System.Windows.Forms.Button()
        Me.DeleteButton = New System.Windows.Forms.Button()
        Me.MvvmViewController2 = New ActiveDevelop.EntitiesFormsLib.MvvmViewController()
        Me.TimeCollectionDataGridView1 = New EFL_CodedUITestContainer.TimeCollectionDataGridView()
        Me.MainMvvmManager = New ActiveDevelop.EntitiesFormsLib.MvvmManager(Me.components)
        Me.ChangeButton = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'AddButton
        '
        Me.AddButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.MainMvvmManager.SetEventBindings(Me.AddButton, Nothing)
        Me.AddButton.Location = New System.Drawing.Point(482, 15)
        Me.AddButton.Name = "AddButton"
        Me.AddButton.Size = New System.Drawing.Size(136, 44)
        Me.AddButton.TabIndex = 1
        Me.AddButton.Text = "Hinzufügen"
        Me.AddButton.UseVisualStyleBackColor = True
        '
        'DeleteButton
        '
        Me.DeleteButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.MainMvvmManager.SetEventBindings(Me.DeleteButton, Nothing)
        Me.DeleteButton.Location = New System.Drawing.Point(482, 115)
        Me.DeleteButton.Name = "DeleteButton"
        Me.DeleteButton.Size = New System.Drawing.Size(136, 44)
        Me.DeleteButton.TabIndex = 2
        Me.DeleteButton.Text = "Löschen"
        Me.DeleteButton.UseVisualStyleBackColor = True
        '
        'MvvmViewController2
        '
        Me.MainMvvmManager.SetEventBindings(Me.MvvmViewController2, Nothing)
        Me.MvvmViewController2.Location = New System.Drawing.Point(641, 523)
        Me.MvvmViewController2.Name = "MvvmViewController2"
        Me.MvvmViewController2.Size = New System.Drawing.Size(25, 23)
        Me.MvvmViewController2.TabIndex = 3
        Me.MvvmViewController2.View = Nothing
        '
        'TimeCollectionDataGridView1
        '
        Me.TimeCollectionDataGridView1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TimeCollectionDataGridView1.DataSource = GetType(EFL_CodedUITestContainer.TimeCollectItems)
        Me.MainMvvmManager.SetEventBindings(Me.TimeCollectionDataGridView1, Nothing)
        Me.TimeCollectionDataGridView1.Location = New System.Drawing.Point(11, 15)
        Me.TimeCollectionDataGridView1.Margin = New System.Windows.Forms.Padding(4)
        Me.TimeCollectionDataGridView1.Name = "TimeCollectionDataGridView1"
        Me.MainMvvmManager.GetPropertyBindings(Me.TimeCollectionDataGridView1).Add(New ActiveDevelop.EntitiesFormsLib.BindingSetting(ActiveDevelop.EntitiesFormsLib.MvvmBindingModes.TwoWay, ActiveDevelop.EntitiesFormsLib.UpdateSourceTriggerSettings.PropertyChangedImmediately), New ActiveDevelop.EntitiesFormsLib.BindingProperty("DataSource", GetType(Object)), Nothing, Nothing, New ActiveDevelop.EntitiesFormsLib.BindingProperty("TimeCollectionItems", GetType(EFL_CodedUITestContainer.TimeCollectItems)))
        Me.MainMvvmManager.GetPropertyBindings(Me.TimeCollectionDataGridView1).Add(New ActiveDevelop.EntitiesFormsLib.BindingSetting(ActiveDevelop.EntitiesFormsLib.MvvmBindingModes.TwoWay, ActiveDevelop.EntitiesFormsLib.UpdateSourceTriggerSettings.PropertyChangedImmediately), New ActiveDevelop.EntitiesFormsLib.BindingProperty("SelectedItem", GetType(Object)), Nothing, Nothing, New ActiveDevelop.EntitiesFormsLib.BindingProperty("SelectedItem", GetType(EFL_CodedUITestContainer.TimeCollectItem)))
        Me.TimeCollectionDataGridView1.SelectedItem = Nothing
        Me.TimeCollectionDataGridView1.Size = New System.Drawing.Size(464, 535)
        Me.TimeCollectionDataGridView1.TabIndex = 4
        '
        'MainMvvmManager
        '
        Me.MainMvvmManager.CancelButton = Nothing
        Me.MainMvvmManager.CurrentContextGuid = New System.Guid("861fafc2-3724-48ce-9bcf-d4a6f0dc5f0b")
        Me.MainMvvmManager.DataContext = Nothing
        Me.MainMvvmManager.DataContextType = GetType(EFL_CodedUITestContainer.TimeCollectionViewModel)
        Me.MainMvvmManager.DataSource = Nothing
        Me.MainMvvmManager.DataSourceType = GetType(EFL_CodedUITestContainer.TimeCollectionViewModel)
        Me.MainMvvmManager.DynamicEventHandlingList = Nothing
        Me.MainMvvmManager.HostingForm = Nothing
        Me.MainMvvmManager.HostingUserControl = Me
        '
        'ChangeButton
        '
        Me.ChangeButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.MainMvvmManager.SetEventBindings(Me.ChangeButton, Nothing)
        Me.ChangeButton.Location = New System.Drawing.Point(482, 65)
        Me.ChangeButton.Name = "ChangeButton"
        Me.ChangeButton.Size = New System.Drawing.Size(136, 44)
        Me.ChangeButton.TabIndex = 5
        Me.ChangeButton.Text = "Ändern"
        Me.ChangeButton.UseVisualStyleBackColor = True
        '
        'TimeCollectionView
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.ChangeButton)
        Me.Controls.Add(Me.TimeCollectionDataGridView1)
        Me.Controls.Add(Me.MvvmViewController2)
        Me.Controls.Add(Me.DeleteButton)
        Me.Controls.Add(Me.AddButton)
        Me.MainMvvmManager.SetEventBindings(Me, Nothing)
        Me.Name = "TimeCollectionView"
        Me.Size = New System.Drawing.Size(621, 561)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents AddButton As System.Windows.Forms.Button
    Friend WithEvents DeleteButton As System.Windows.Forms.Button
    Friend WithEvents MainMvvmManager As ActiveDevelop.EntitiesFormsLib.MvvmManager
    Friend WithEvents MvvmViewController1 As ActiveDevelop.EntitiesFormsLib.MvvmViewController
    Friend WithEvents MvvmViewController2 As ActiveDevelop.EntitiesFormsLib.MvvmViewController
    Friend WithEvents TimeCollectionDataGridView1 As EFL_CodedUITestContainer.TimeCollectionDataGridView
    Friend WithEvents ChangeButton As System.Windows.Forms.Button

End Class
