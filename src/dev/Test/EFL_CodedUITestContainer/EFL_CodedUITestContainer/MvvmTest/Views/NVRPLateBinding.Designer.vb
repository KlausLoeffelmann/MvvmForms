<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class NVRPLateBinding
    Inherits System.Windows.Forms.Form

    'Das Formular überschreibt den Löschvorgang, um die Komponentenliste zu bereinigen.
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

    'Wird vom Windows Form-Designer benötigt.
    Private components As System.ComponentModel.IContainer

    'Hinweis: Die folgende Prozedur ist für den Windows Form-Designer erforderlich.
    'Das Bearbeiten ist mit dem Windows Form-Designer möglich.  
    'Das Bearbeiten mit dem Code-Editor ist nicht möglich.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.NullableValueRelationPopup1 = New ActiveDevelop.EntitiesFormsLib.NullableValueRelationPopup()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.MvvmManager1 = New ActiveDevelop.EntitiesFormsLib.MvvmManager(Me.components)
        CType(Me.NullableValueRelationPopup1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.MvvmManager1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'NullableValueRelationPopup1
        '
        Me.NullableValueRelationPopup1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.NullableValueRelationPopup1.AssignedManagerComponent = Nothing
        Me.NullableValueRelationPopup1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.None
        Me.NullableValueRelationPopup1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.None
        Me.NullableValueRelationPopup1.BeepOnFailedValidation = False
        Me.NullableValueRelationPopup1.Borderstyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.NullableValueRelationPopup1.ContentPresentPermission = ActiveDevelop.EntitiesFormsLib.ContentPresentPermissions.Normal
        Me.NullableValueRelationPopup1.DataSource = Nothing
        Me.NullableValueRelationPopup1.DisplayMember = Nothing
        Me.MvvmManager1.SetEventBindings(Me.NullableValueRelationPopup1, Nothing)
        Me.NullableValueRelationPopup1.HideButtons = False
        Me.NullableValueRelationPopup1.IsPopupAutoSize = False
        Me.NullableValueRelationPopup1.IsPopupResizable = True
        Me.NullableValueRelationPopup1.Location = New System.Drawing.Point(12, 12)
        Me.NullableValueRelationPopup1.MaxLength = 32767
        Me.NullableValueRelationPopup1.MinimumPopupSize = New System.Drawing.Size(260, 80)
        Me.NullableValueRelationPopup1.MultiSelect = False
        Me.NullableValueRelationPopup1.Name = "NullableValueRelationPopup1"
        Me.NullableValueRelationPopup1.NullValueString = "* - - - *"
        Me.NullableValueRelationPopup1.PermissionReason = Nothing
        Me.NullableValueRelationPopup1.Size = New System.Drawing.Size(260, 20)
        Me.NullableValueRelationPopup1.TabIndex = 0
        Me.NullableValueRelationPopup1.UIGuid = New System.Guid("e33cd3d7-6da2-417e-9f68-5c3954c626cc")
        Me.NullableValueRelationPopup1.ValueMember = Nothing
        '
        'Button1
        '
        Me.Button1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.MvvmManager1.SetEventBindings(Me.Button1, Nothing)
        Me.Button1.Location = New System.Drawing.Point(12, 226)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(260, 23)
        Me.Button1.TabIndex = 1
        Me.Button1.Text = "&Schließen"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'MvvmManager1
        '
        Me.MvvmManager1.CancelButton = Nothing
        Me.MvvmManager1.ContainerControl = Me
        Me.MvvmManager1.CurrentContextGuid = New System.Guid("861fafc2-3724-48ce-9bcf-d4a6f0dc5f0b")
        Me.MvvmManager1.DataContext = Nothing
        Me.MvvmManager1.DataContextType = GetType(EFL_CodedUITestContainer.LateBindingViewModel)
        Me.MvvmManager1.DataSourceType = GetType(EFL_CodedUITestContainer.LateBindingViewModel)
        Me.MvvmManager1.DirtyStateManagerComponent = Nothing
        Me.MvvmManager1.DynamicEventHandlingList = Nothing
        Me.MvvmManager1.HostingForm = Me
        Me.MvvmManager1.HostingUserControl = Nothing
        Me.MvvmManager1.MvvmBindings.AddPropertyBinding(Me.NullableValueRelationPopup1, New ActiveDevelop.EntitiesFormsLib.BindingSetting(ActiveDevelop.EntitiesFormsLib.MvvmBindingModes.OneWay, ActiveDevelop.EntitiesFormsLib.UpdateSourceTriggerSettings.PropertyChangedImmediately), New ActiveDevelop.EntitiesFormsLib.BindingProperty("DataSource", GetType(Object)), Nothing, Nothing, New ActiveDevelop.EntitiesFormsLib.BindingProperty("ObsCollection", GetType(System.Collections.ObjectModel.ObservableCollection(Of Integer))))
        Me.MvvmManager1.MvvmBindings.AddPropertyBinding(Me.NullableValueRelationPopup1, New ActiveDevelop.EntitiesFormsLib.BindingSetting(ActiveDevelop.EntitiesFormsLib.MvvmBindingModes.TwoWay, ActiveDevelop.EntitiesFormsLib.UpdateSourceTriggerSettings.PropertyChangedImmediately), New ActiveDevelop.EntitiesFormsLib.BindingProperty("Value", GetType(Object)), Nothing, Nothing, New ActiveDevelop.EntitiesFormsLib.BindingProperty("SelectedItem", GetType(Integer)))
        '
        'NVRPLateBinding
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(284, 261)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.NullableValueRelationPopup1)
        Me.MvvmManager1.SetEventBindings(Me, Nothing)
        Me.Name = "NVRPLateBinding"
        Me.Text = "NVRPLateBinding"
        CType(Me.NullableValueRelationPopup1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.MvvmManager1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents NullableValueRelationPopup1 As ActiveDevelop.EntitiesFormsLib.NullableValueRelationPopup
    Friend WithEvents MvvmManager1 As ActiveDevelop.EntitiesFormsLib.MvvmManager
    Friend WithEvents Button1 As Button
End Class
