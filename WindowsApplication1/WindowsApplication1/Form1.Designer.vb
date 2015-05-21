<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
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
        Me.MvvmManager1 = New ActiveDevelop.EntitiesFormsLib.MvvmManager(Me.components)
        Me.WritableTextBox1 = New ActiveDevelop.EntitiesFormsLib.WritableTextBox()
        Me.NumericUpDown1 = New System.Windows.Forms.NumericUpDown()
        Me.NullableDateValue1 = New ActiveDevelop.EntitiesFormsLib.NullableDateValue()
        Me.NullableNumValue1 = New ActiveDevelop.EntitiesFormsLib.NullableNumValue()
        CType(Me.MvvmManager1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.NumericUpDown1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'MvvmManager1
        '
        Me.MvvmManager1.CancelButton = Nothing
        Me.MvvmManager1.ContainerControl = Me
        Me.MvvmManager1.CurrentContextGuid = New System.Guid("861fafc2-3724-48ce-9bcf-d4a6f0dc5f0b")
        Me.MvvmManager1.DataContext = Nothing
        Me.MvvmManager1.DataContextType = Nothing
        Me.MvvmManager1.DataSourceType = Nothing
        Me.MvvmManager1.DirtyStateManagerComponent = Nothing
        Me.MvvmManager1.DynamicEventHandlingList = Nothing
        Me.MvvmManager1.HostingForm = Me
        Me.MvvmManager1.HostingUserControl = Nothing
        '
        'WritableTextBox1
        '
        Me.WritableTextBox1.ConsumeGlobalWrite = False
        Me.MvvmManager1.SetEventBindings(Me.WritableTextBox1, Nothing)
        Me.WritableTextBox1.Location = New System.Drawing.Point(50, 142)
        Me.WritableTextBox1.Multiline = True
        Me.WritableTextBox1.Name = "WritableTextBox1"
        Me.WritableTextBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.WritableTextBox1.Size = New System.Drawing.Size(188, 221)
        Me.WritableTextBox1.TabIndex = 1
        '
        'NumericUpDown1
        '
        Me.MvvmManager1.SetEventBindings(Me.NumericUpDown1, Nothing)
        Me.NumericUpDown1.Location = New System.Drawing.Point(50, 17)
        Me.NumericUpDown1.Name = "NumericUpDown1"
        Me.NumericUpDown1.Size = New System.Drawing.Size(183, 26)
        Me.NumericUpDown1.TabIndex = 2
        '
        'NullableDateValue1
        '
        Me.NullableDateValue1.AssignedManagerComponent = Nothing
        Me.NullableDateValue1.Borderstyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.NullableDateValue1.ContentPresentPermission = ActiveDevelop.EntitiesFormsLib.ContentPresentPermissions.Normal
        Me.NullableDateValue1.DaysDistanceBetweenLinkedControl = Nothing
        Me.MvvmManager1.SetEventBindings(Me.NullableDateValue1, Nothing)
        Me.NullableDateValue1.LinkedToNullableDateControl = Nothing
        Me.NullableDateValue1.Location = New System.Drawing.Point(276, 151)
        Me.NullableDateValue1.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.NullableDateValue1.MaxLength = 32767
        Me.NullableDateValue1.Name = "NullableDateValue1"
        Me.NullableDateValue1.ObfuscationChar = Nothing
        Me.NullableDateValue1.PermissionReason = Nothing
        Me.NullableDateValue1.Size = New System.Drawing.Size(246, 26)
        Me.NullableDateValue1.TabIndex = 5
        Me.NullableDateValue1.UIGuid = New System.Guid("3203dbd5-c574-4111-923c-810e4ea7f41c")
        Me.NullableDateValue1.Value = Nothing
        Me.NullableDateValue1.ValueValidationState = Nothing
        '
        'NullableNumValue1
        '
        Me.NullableNumValue1.AssignedManagerComponent = Nothing
        Me.NullableNumValue1.Borderstyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.NullableNumValue1.ContentPresentPermission = ActiveDevelop.EntitiesFormsLib.ContentPresentPermissions.Normal
        Me.NullableNumValue1.CurrencySymbolString = Nothing
        Me.MvvmManager1.SetEventBindings(Me.NullableNumValue1, Nothing)
        Me.NullableNumValue1.Location = New System.Drawing.Point(259, 58)
        Me.NullableNumValue1.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.NullableNumValue1.MaxLength = 32767
        Me.NullableNumValue1.MaxValue = Nothing
        Me.NullableNumValue1.MinValue = New Decimal(New Integer() {0, 0, 0, 0})
        Me.NullableNumValue1.Name = "NullableNumValue1"
        Me.NullableNumValue1.ObfuscationChar = Nothing
        Me.NullableNumValue1.PermissionReason = Nothing
        Me.NullableNumValue1.Size = New System.Drawing.Size(263, 26)
        Me.NullableNumValue1.TabIndex = 6
        Me.NullableNumValue1.UIGuid = New System.Guid("cb9c528b-dc4f-43f4-bdbc-6f4280e13269")
        Me.NullableNumValue1.Value = Nothing
        Me.NullableNumValue1.ValueValidationState = Nothing
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(663, 478)
        Me.Controls.Add(Me.NullableNumValue1)
        Me.Controls.Add(Me.NullableDateValue1)
        Me.Controls.Add(Me.NumericUpDown1)
        Me.Controls.Add(Me.WritableTextBox1)
        Me.MvvmManager1.SetEventBindings(Me, Nothing)
        Me.Name = "Form1"
        Me.Text = "Form1"
        CType(Me.MvvmManager1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.NumericUpDown1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents MvvmManager1 As ActiveDevelop.EntitiesFormsLib.MvvmManager
    Friend WithEvents WritableTextBox1 As ActiveDevelop.EntitiesFormsLib.WritableTextBox
    Friend WithEvents NumericUpDown1 As NumericUpDown
    Friend WithEvents NullableNumValue1 As ActiveDevelop.EntitiesFormsLib.NullableNumValue
    Friend WithEvents NullableDateValue1 As ActiveDevelop.EntitiesFormsLib.NullableDateValue
End Class
