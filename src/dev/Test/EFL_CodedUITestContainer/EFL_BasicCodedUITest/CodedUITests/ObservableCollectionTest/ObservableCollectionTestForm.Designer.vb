Imports ActiveDevelop.EntitiesFormsLib

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ObservableCollectionTestForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
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
        Me.ListBox1 = New System.Windows.Forms.ListBox()
        Me.CommandButton1 = New ActiveDevelop.EntitiesFormsLib.CommandButton()
        Me.MvvmManager1 = New ActiveDevelop.EntitiesFormsLib.MvvmManager(Me.components)
        CType(Me.MvvmManager1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ListBox1
        '
        Me.MvvmManager1.SetEventBindings(Me.ListBox1, Nothing)
        Me.ListBox1.FormattingEnabled = True
        Me.ListBox1.Location = New System.Drawing.Point(18, 19)
        Me.ListBox1.Name = "ListBox1"
        Me.ListBox1.Size = New System.Drawing.Size(369, 342)
        Me.ListBox1.TabIndex = 0
        '
        'CommandButton1
        '
        Me.CommandButton1.Command = Nothing
        Me.CommandButton1.CommandParameter = Nothing
        Me.MvvmManager1.SetEventBindings(Me.CommandButton1, Nothing)
        Me.CommandButton1.Location = New System.Drawing.Point(410, 19)
        Me.CommandButton1.Name = "CommandButton1"
        Me.CommandButton1.Size = New System.Drawing.Size(143, 48)
        Me.CommandButton1.TabIndex = 1
        Me.CommandButton1.Text = "Generate new List"
        Me.CommandButton1.UseVisualStyleBackColor = True
        '
        'MvvmManager1
        '
        Me.MvvmManager1.CancelButton = Nothing
        Me.MvvmManager1.ContainerControl = Me
        Me.MvvmManager1.CurrentContextGuid = New System.Guid("861fafc2-3724-48ce-9bcf-d4a6f0dc5f0b")
        Me.MvvmManager1.DataContext = Nothing
        Me.MvvmManager1.DataContextType = GetType(EFL_BasicCodedUITest.ObservableCollectionTestFormViewModel)
        Me.MvvmManager1.DataSourceType = GetType(EFL_BasicCodedUITest.ObservableCollectionTestFormViewModel)
        Me.MvvmManager1.DirtyStateManagerComponent = Nothing
        Me.MvvmManager1.DynamicEventHandlingList = Nothing
        Me.MvvmManager1.HostingForm = Me
        Me.MvvmManager1.HostingUserControl = Nothing
        Me.MvvmManager1.MvvmBindings.AddPropertyBinding(Me.ListBox1, New ActiveDevelop.EntitiesFormsLib.BindingSetting(ActiveDevelop.EntitiesFormsLib.MvvmBindingModes.OneWay, ActiveDevelop.EntitiesFormsLib.UpdateSourceTriggerSettings.PropertyChangedImmediately), New ActiveDevelop.EntitiesFormsLib.BindingProperty("DataSource", GetType(Object)), GetType(ActiveDevelop.EntitiesFormsLib.ObservableCollectionToBindingListConverter), Nothing, New ActiveDevelop.EntitiesFormsLib.BindingProperty("List", GetType(System.Collections.ObjectModel.ObservableCollection(Of EFL_BasicCodedUITest.ContactViewModel))))
        Me.MvvmManager1.MvvmBindings.AddPropertyBinding(Me.CommandButton1, New ActiveDevelop.EntitiesFormsLib.BindingSetting(ActiveDevelop.EntitiesFormsLib.MvvmBindingModes.TwoWay, ActiveDevelop.EntitiesFormsLib.UpdateSourceTriggerSettings.PropertyChangedImmediately), New ActiveDevelop.EntitiesFormsLib.BindingProperty("Command", GetType(System.Windows.Input.ICommand)), Nothing, Nothing, New ActiveDevelop.EntitiesFormsLib.BindingProperty("GenerateListCommand", GetType(ActiveDevelop.MvvmBaseLib.Mvvm.RelayCommand)))
        '
        'ObservableCollectionTestForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(565, 375)
        Me.Controls.Add(Me.CommandButton1)
        Me.Controls.Add(Me.ListBox1)
        Me.MvvmManager1.SetEventBindings(Me, Nothing)
        Me.Name = "ObservableCollectionTestForm"
        Me.Text = "ObservableCollectionTestForm"
        CType(Me.MvvmManager1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents ListBox1 As System.Windows.Forms.ListBox
    Friend WithEvents MvvmManager1 As MvvmManager
    Friend WithEvents CommandButton1 As CommandButton
End Class
