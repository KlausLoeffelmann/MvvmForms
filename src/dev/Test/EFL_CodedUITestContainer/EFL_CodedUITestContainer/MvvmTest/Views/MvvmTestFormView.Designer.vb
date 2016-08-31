Imports ActiveDevelop.EntitiesFormsLib

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class MvvmTestFormView
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.TreeView2 = New ActiveDevelop.EntitiesFormsLib.BindableTreeView()
        Me.ListBox1 = New System.Windows.Forms.ListBox()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.TextBox2 = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.Button3 = New System.Windows.Forms.Button()
        Me.Button4 = New System.Windows.Forms.Button()
        Me.BindableTreeView1 = New ActiveDevelop.EntitiesFormsLib.BindableTreeView()
        Me.BindableTreeView2 = New ActiveDevelop.EntitiesFormsLib.BindableTreeView()
        Me.NullableNumValue1 = New ActiveDevelop.EntitiesFormsLib.NullableNumValue()
        Me.Button5 = New System.Windows.Forms.Button()
        Me.MvvmManager1 = New ActiveDevelop.EntitiesFormsLib.MvvmManager(Me.components)
        CType(Me.MvvmManager1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'TreeView2
        '
        Me.TreeView2.ChildMemberPath = "Anschriften/Orte"
        Me.TreeView2.DataSource = Nothing
        Me.TreeView2.DisplayMemberPath = "Nachname/Anschrift/Bezeichnung"
        Me.MvvmManager1.SetEventBindings(Me.TreeView2, Nothing)
        Me.TreeView2.HideSelection = False
        Me.TreeView2.LazyLoading = True
        Me.TreeView2.LineColor = System.Drawing.Color.Blue
        Me.TreeView2.Location = New System.Drawing.Point(95, 12)
        Me.TreeView2.Name = "TreeView2"
        Me.TreeView2.SelectedItem = Nothing
        Me.TreeView2.SelectedRootItem = Nothing
        Me.TreeView2.Size = New System.Drawing.Size(360, 291)
        Me.TreeView2.TabIndex = 2
        '
        'ListBox1
        '
        Me.MvvmManager1.SetEventBindings(Me.ListBox1, Nothing)
        Me.ListBox1.FormattingEnabled = True
        Me.ListBox1.Location = New System.Drawing.Point(520, 12)
        Me.ListBox1.Name = "ListBox1"
        Me.ListBox1.Size = New System.Drawing.Size(311, 199)
        Me.ListBox1.TabIndex = 3
        '
        'TextBox1
        '
        Me.MvvmManager1.SetEventBindings(Me.TextBox1, Nothing)
        Me.TextBox1.Location = New System.Drawing.Point(573, 328)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(200, 20)
        Me.TextBox1.TabIndex = 4
        '
        'TextBox2
        '
        Me.MvvmManager1.SetEventBindings(Me.TextBox2, Nothing)
        Me.TextBox2.Location = New System.Drawing.Point(573, 354)
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.Size = New System.Drawing.Size(200, 20)
        Me.TextBox2.TabIndex = 5
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.MvvmManager1.SetEventBindings(Me.Label1, Nothing)
        Me.Label1.Location = New System.Drawing.Point(470, 331)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(92, 13)
        Me.Label1.TabIndex = 6
        Me.Label1.Text = "SelectedRootItem"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.MvvmManager1.SetEventBindings(Me.Label3, Nothing)
        Me.Label3.Location = New System.Drawing.Point(493, 357)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(69, 13)
        Me.Label3.TabIndex = 8
        Me.Label3.Text = "SelectedItem"
        '
        'Button1
        '
        Me.MvvmManager1.SetEventBindings(Me.Button1, Nothing)
        Me.Button1.Location = New System.Drawing.Point(462, 231)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(75, 23)
        Me.Button1.TabIndex = 9
        Me.Button1.Text = "Button1"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Button2
        '
        Me.MvvmManager1.SetEventBindings(Me.Button2, Nothing)
        Me.Button2.Location = New System.Drawing.Point(461, 260)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(75, 23)
        Me.Button2.TabIndex = 10
        Me.Button2.Text = "Button2"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'Button3
        '
        Me.MvvmManager1.SetEventBindings(Me.Button3, Nothing)
        Me.Button3.Location = New System.Drawing.Point(226, 321)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(75, 23)
        Me.Button3.TabIndex = 11
        Me.Button3.Text = "Remove"
        Me.Button3.UseVisualStyleBackColor = True
        '
        'Button4
        '
        Me.MvvmManager1.SetEventBindings(Me.Button4, Nothing)
        Me.Button4.Location = New System.Drawing.Point(307, 321)
        Me.Button4.Name = "Button4"
        Me.Button4.Size = New System.Drawing.Size(75, 23)
        Me.Button4.TabIndex = 12
        Me.Button4.Text = "Add"
        Me.Button4.UseVisualStyleBackColor = True
        '
        'BindableTreeView1
        '
        Me.BindableTreeView1.ChildMemberPath = "Personen"
        Me.BindableTreeView1.DataSource = Nothing
        Me.BindableTreeView1.DisplayMemberPath = "Nachname"
        Me.MvvmManager1.SetEventBindings(Me.BindableTreeView1, Nothing)
        Me.BindableTreeView1.HideSelection = False
        Me.BindableTreeView1.LazyLoading = True
        Me.BindableTreeView1.LineColor = System.Drawing.Color.Blue
        Me.BindableTreeView1.Location = New System.Drawing.Point(82, 385)
        Me.BindableTreeView1.Name = "BindableTreeView1"
        Me.BindableTreeView1.SelectedItem = Nothing
        Me.BindableTreeView1.SelectedRootItem = Nothing
        Me.BindableTreeView1.Size = New System.Drawing.Size(126, 195)
        Me.BindableTreeView1.TabIndex = 13
        '
        'BindableTreeView2
        '
        Me.BindableTreeView2.ChildMemberPath = "SubProperties"
        Me.BindableTreeView2.DataSource = Nothing
        Me.BindableTreeView2.DisplayMemberPath = "Description"
        Me.MvvmManager1.SetEventBindings(Me.BindableTreeView2, Nothing)
        Me.BindableTreeView2.HideSelection = False
        Me.BindableTreeView2.LazyLoading = True
        Me.BindableTreeView2.LineColor = System.Drawing.Color.Blue
        Me.BindableTreeView2.Location = New System.Drawing.Point(239, 385)
        Me.BindableTreeView2.Name = "BindableTreeView2"
        Me.BindableTreeView2.SelectedItem = Nothing
        Me.BindableTreeView2.SelectedRootItem = Nothing
        Me.BindableTreeView2.Size = New System.Drawing.Size(455, 195)
        Me.BindableTreeView2.TabIndex = 14
        '
        'NullableNumValue1
        '
        Me.NullableNumValue1.AssignedManagerComponent = Nothing
        Me.NullableNumValue1.Borderstyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.NullableNumValue1.ContentPresentPermission = ActiveDevelop.EntitiesFormsLib.ContentPresentPermissions.Normal
        Me.NullableNumValue1.CurrencySymbolString = Nothing
        Me.NullableNumValue1.DecimalPlaces = 2
        Me.NullableNumValue1.DropDownCalculatorMode = ActiveDevelop.EntitiesFormsLib.CalculatorType.Simple
        Me.MvvmManager1.SetEventBindings(Me.NullableNumValue1, Nothing)
        Me.NullableNumValue1.Location = New System.Drawing.Point(604, 277)
        Me.NullableNumValue1.MaxLength = 32767
        Me.NullableNumValue1.MaxValue = Nothing
        Me.NullableNumValue1.MinValue = New Decimal(New Integer() {0, 0, 0, 0})
        Me.NullableNumValue1.Name = "NullableNumValue1"
        Me.NullableNumValue1.NullValueString = "* - - - *"
        Me.NullableNumValue1.ObfuscationChar = Nothing
        Me.NullableNumValue1.PermissionReason = Nothing
        Me.NullableNumValue1.Size = New System.Drawing.Size(120, 20)
        Me.NullableNumValue1.TabIndex = 16
        Me.NullableNumValue1.UIGuid = New System.Guid("b80663b5-f9a2-49d4-81b6-4a337d438301")
        Me.NullableNumValue1.Value = Nothing
        Me.NullableNumValue1.ValueValidationState = Nothing
        '
        'Button5
        '
        Me.MvvmManager1.SetEventBindings(Me.Button5, Nothing)
        Me.Button5.Location = New System.Drawing.Point(604, 231)
        Me.Button5.Name = "Button5"
        Me.Button5.Size = New System.Drawing.Size(75, 23)
        Me.Button5.TabIndex = 15
        Me.Button5.Text = "Button5"
        Me.Button5.UseVisualStyleBackColor = True
        '
        'MvvmManager1
        '
        Me.MvvmManager1.CancelButton = Nothing
        Me.MvvmManager1.ContainerControl = Me
        Me.MvvmManager1.CurrentContextGuid = New System.Guid("861fafc2-3724-48ce-9bcf-d4a6f0dc5f0b")
        Me.MvvmManager1.DataContext = Nothing
        Me.MvvmManager1.DataContextType = GetType(EFL_CodedUITestContainer.MainNodeTestViewModel)
        Me.MvvmManager1.DataSourceType = GetType(EFL_CodedUITestContainer.MainNodeTestViewModel)
        Me.MvvmManager1.DirtyStateManagerComponent = Nothing
        Me.MvvmManager1.DynamicEventHandlingList = Nothing
        Me.MvvmManager1.HostingForm = Me
        Me.MvvmManager1.HostingUserControl = Nothing
        Me.MvvmManager1.MvvmBindings.AddPropertyBinding(Me.TreeView2, New ActiveDevelop.EntitiesFormsLib.BindingSetting(ActiveDevelop.EntitiesFormsLib.MvvmBindingModes.TwoWay, ActiveDevelop.EntitiesFormsLib.UpdateSourceTriggerSettings.LostFocus), New ActiveDevelop.EntitiesFormsLib.BindingProperty("DataSource", GetType(System.Collections.IEnumerable)), Nothing, Nothing, New ActiveDevelop.EntitiesFormsLib.BindingProperty("Personen", GetType(System.Collections.ObjectModel.ObservableCollection(Of EFL_CodedUITestContainer.PersonenViewModelNodeTest))))
        Me.MvvmManager1.MvvmBindings.AddPropertyBinding(Me.BindableTreeView1, New ActiveDevelop.EntitiesFormsLib.BindingSetting(ActiveDevelop.EntitiesFormsLib.MvvmBindingModes.TwoWay, ActiveDevelop.EntitiesFormsLib.UpdateSourceTriggerSettings.LostFocus), New ActiveDevelop.EntitiesFormsLib.BindingProperty("DataSource", GetType(System.Collections.IEnumerable)), Nothing, Nothing, New ActiveDevelop.EntitiesFormsLib.BindingProperty("Personen2", GetType(System.Collections.ObjectModel.ObservableCollection(Of EFL_CodedUITestContainer.PersonenViewModelNodeTest))))
        Me.MvvmManager1.MvvmBindings.AddPropertyBinding(Me.BindableTreeView2, New ActiveDevelop.EntitiesFormsLib.BindingSetting(ActiveDevelop.EntitiesFormsLib.MvvmBindingModes.OneWay, ActiveDevelop.EntitiesFormsLib.UpdateSourceTriggerSettings.PropertyChangedImmediately), New ActiveDevelop.EntitiesFormsLib.BindingProperty("DataSource", GetType(System.Collections.IEnumerable)), Nothing, Nothing, New ActiveDevelop.EntitiesFormsLib.BindingProperty("ViewModelTypes", GetType(System.Collections.ObjectModel.ObservableCollection(Of ActiveDevelop.EntitiesFormsLib.PropertyBindingNodeDefinition))))
        Me.MvvmManager1.MvvmBindings.AddPropertyBinding(Me.NullableNumValue1, New ActiveDevelop.EntitiesFormsLib.BindingSetting(ActiveDevelop.EntitiesFormsLib.MvvmBindingModes.TwoWay, ActiveDevelop.EntitiesFormsLib.UpdateSourceTriggerSettings.PropertyChangedImmediately), New ActiveDevelop.EntitiesFormsLib.BindingProperty("Value", GetType(System.Nullable(Of Decimal))), Nothing, Nothing, New ActiveDevelop.EntitiesFormsLib.BindingProperty("NumVal", GetType(System.Nullable(Of Decimal))))
        '
        'MvvmTestFormView
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(843, 592)
        Me.Controls.Add(Me.NullableNumValue1)
        Me.Controls.Add(Me.Button5)
        Me.Controls.Add(Me.BindableTreeView2)
        Me.Controls.Add(Me.BindableTreeView1)
        Me.Controls.Add(Me.Button4)
        Me.Controls.Add(Me.Button3)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.TextBox2)
        Me.Controls.Add(Me.TextBox1)
        Me.Controls.Add(Me.ListBox1)
        Me.Controls.Add(Me.TreeView2)
        Me.MvvmManager1.SetEventBindings(Me, Nothing)
        Me.Name = "MvvmTestFormView"
        Me.Text = "MvvmTestFormView"
        CType(Me.MvvmManager1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents MvvmManager1 As ActiveDevelop.EntitiesFormsLib.MvvmManager
    Friend WithEvents TreeView1 As BindableTreeView
    Friend WithEvents TreeView2 As BindableTreeView
    Friend WithEvents Label1 As Label
    Friend WithEvents TextBox2 As TextBox
    Friend WithEvents TextBox1 As TextBox
    Friend WithEvents ListBox1 As ListBox
    Friend WithEvents Label3 As Label
    Friend WithEvents Button2 As Button
    Friend WithEvents Button1 As Button
    Friend WithEvents Button3 As Button
    Friend WithEvents Button4 As Button
    Friend WithEvents BindableTreeView1 As BindableTreeView
    Friend WithEvents BindableTreeView2 As BindableTreeView
    Friend WithEvents Button5 As Button
    Friend WithEvents NullableNumValue1 As NullableNumValue
End Class
