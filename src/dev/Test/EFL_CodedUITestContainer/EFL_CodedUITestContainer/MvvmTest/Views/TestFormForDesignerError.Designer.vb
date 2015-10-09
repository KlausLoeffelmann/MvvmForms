<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class TestFormForDesignerError
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.MvvmManager1 = New ActiveDevelop.EntitiesFormsLib.MvvmManager(Me.components)
        Me.LastNameNTextValue = New ActiveDevelop.EntitiesFormsLib.NullableTextValue()
        Me.ListBox1 = New System.Windows.Forms.ListBox()
        Me.GridPanel1 = New ActiveDevelop.EntitiesFormsLib.GridPanel()
        CType(Me.MvvmManager1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'MvvmManager1
        '
        Me.MvvmManager1.CancelButton = Nothing
        Me.MvvmManager1.ContainerControl = Me
        Me.MvvmManager1.CurrentContextGuid = New System.Guid("861fafc2-3724-48ce-9bcf-d4a6f0dc5f0b")
        Me.MvvmManager1.DataContext = Nothing
        Me.MvvmManager1.DataContextType = GetType(EFL_CodedUITestContainer.CircTestPerson)
        Me.MvvmManager1.DataSourceType = GetType(EFL_CodedUITestContainer.CircTestPerson)
        Me.MvvmManager1.DirtyStateManagerComponent = Nothing
        Me.MvvmManager1.DynamicEventHandlingList = Nothing
        Me.MvvmManager1.HostingForm = Me
        Me.MvvmManager1.HostingUserControl = Nothing
        Me.MvvmManager1.MvvmBindings.AddPropertyBinding(Me, New ActiveDevelop.EntitiesFormsLib.BindingSetting(ActiveDevelop.EntitiesFormsLib.MvvmBindingModes.TwoWay, ActiveDevelop.EntitiesFormsLib.UpdateSourceTriggerSettings.LostFocus), New ActiveDevelop.EntitiesFormsLib.BindingProperty("CausesValidation", GetType(Boolean)), Nothing, Nothing, New ActiveDevelop.EntitiesFormsLib.BindingProperty("DeleteTimeCollectionItemCommand.CanExecuteState", GetType(Boolean)))
        Me.MvvmManager1.MvvmBindings.AddPropertyBinding(Me.LastNameNTextValue, New ActiveDevelop.EntitiesFormsLib.BindingSetting(ActiveDevelop.EntitiesFormsLib.MvvmBindingModes.TwoWay, ActiveDevelop.EntitiesFormsLib.UpdateSourceTriggerSettings.LostFocus), New ActiveDevelop.EntitiesFormsLib.BindingProperty("Value", GetType(System.Nullable(Of ActiveDevelop.EntitiesFormsLib.StringValue))), Nothing, Nothing, New ActiveDevelop.EntitiesFormsLib.BindingProperty("Anschrift.Person.Anschrift.Person.Name", GetType(String)))
        Me.MvvmManager1.MvvmBindings.AddPropertyBinding(Me.ListBox1, New ActiveDevelop.EntitiesFormsLib.BindingSetting(ActiveDevelop.EntitiesFormsLib.MvvmBindingModes.TwoWay, ActiveDevelop.EntitiesFormsLib.UpdateSourceTriggerSettings.LostFocus), New ActiveDevelop.EntitiesFormsLib.BindingProperty("DataSource", GetType(Object)), Nothing, Nothing, New ActiveDevelop.EntitiesFormsLib.BindingProperty("Anschrift.Person.Name", GetType(String)))
        '
        'LastNameNTextValue
        '
        Me.LastNameNTextValue.AssignedManagerComponent = Nothing
        Me.LastNameNTextValue.Borderstyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.LastNameNTextValue.ContentPresentPermission = ActiveDevelop.EntitiesFormsLib.ContentPresentPermissions.Normal
        Me.MvvmManager1.SetEventBindings(Me.LastNameNTextValue, Nothing)
        Me.LastNameNTextValue.Location = New System.Drawing.Point(64, 71)
        Me.LastNameNTextValue.MaxLength = 32767
        Me.LastNameNTextValue.Name = "LastNameNTextValue"
        Me.LastNameNTextValue.ObfuscationChar = Nothing
        Me.LastNameNTextValue.PermissionReason = Nothing
        Me.LastNameNTextValue.Size = New System.Drawing.Size(278, 20)
        Me.LastNameNTextValue.TabIndex = 1
        Me.LastNameNTextValue.UIGuid = New System.Guid("8cf7c996-7d2d-4891-835f-3dece0291f67")
        Me.LastNameNTextValue.Value = Nothing
        Me.LastNameNTextValue.ValueValidationState = Nothing
        '
        'ListBox1
        '
        Me.MvvmManager1.SetEventBindings(Me.ListBox1, Nothing)
        Me.ListBox1.FormattingEnabled = True
        Me.ListBox1.Location = New System.Drawing.Point(64, 110)
        Me.ListBox1.Name = "ListBox1"
        Me.ListBox1.Size = New System.Drawing.Size(278, 95)
        Me.ListBox1.TabIndex = 2
        '
        'GridPanel1
        '
        Me.GridPanel1.AutoTabIndex = True
        Me.MvvmManager1.SetEventBindings(Me.GridPanel1, Nothing)
        Me.GridPanel1.Location = New System.Drawing.Point(285, 343)
        Me.GridPanel1.MaxColumns = 10
        Me.GridPanel1.Name = "GridPanel1"
        Me.GridPanel1.Size = New System.Drawing.Size(200, 100)
        Me.GridPanel1.TabIndex = 3
        '
        'TestFormForDesignerError
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(540, 453)
        Me.Controls.Add(Me.GridPanel1)
        Me.Controls.Add(Me.ListBox1)
        Me.Controls.Add(Me.LastNameNTextValue)
        Me.MvvmManager1.SetEventBindings(Me, Nothing)
        Me.Margin = New System.Windows.Forms.Padding(2)
        Me.Name = "TestFormForDesignerError"
        Me.Text = "TestFormForDesignerError"
        CType(Me.MvvmManager1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents MvvmManager1 As ActiveDevelop.EntitiesFormsLib.MvvmManager
    Friend WithEvents LastNameNTextValue As ActiveDevelop.EntitiesFormsLib.NullableTextValue
    Friend WithEvents ListBox1 As ListBox
    Friend WithEvents GridPanel1 As ActiveDevelop.EntitiesFormsLib.GridPanel
End Class
