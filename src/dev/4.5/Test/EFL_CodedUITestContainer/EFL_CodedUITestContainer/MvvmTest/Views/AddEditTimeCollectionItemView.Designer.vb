<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AddEditTimeCollectionItemView
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
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.NullableMultilineTextValue1 = New ActiveDevelop.EntitiesFormsLib.NullableMultilineTextValue()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.DirtyStateManager1 = New ActiveDevelop.EntitiesFormsLib.DirtyStateManager()
        Me.MaskedTextBox1 = New System.Windows.Forms.MaskedTextBox()
        Me.MaskedTextBox2 = New System.Windows.Forms.MaskedTextBox()
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.SaveChangesStatusLabel = New System.Windows.Forms.ToolStripStatusLabel()
        Me.Button3 = New System.Windows.Forms.Button()
        Me.MvvmManagerMain = New ActiveDevelop.EntitiesFormsLib.MvvmManager(Me.components)
        CType(Me.DirtyStateManager1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.StatusStrip1.SuspendLayout()
        CType(Me.MvvmManagerMain, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.MvvmManagerMain.SetEventBindings(Me.Label1, Nothing)
        Me.DirtyStateManager1.SetIsDirtyChangedCausingEvent(Me.Label1, "")
        Me.Label1.Location = New System.Drawing.Point(55, 48)
        Me.Label1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(73, 20)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Startzeit:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.MvvmManagerMain.SetEventBindings(Me.Label2, Nothing)
        Me.DirtyStateManager1.SetIsDirtyChangedCausingEvent(Me.Label2, "")
        Me.Label2.Location = New System.Drawing.Point(292, 45)
        Me.Label2.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(67, 20)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Endzeit:"
        '
        'Button1
        '
        Me.Button1.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.MvvmManagerMain.SetEventBindings(Me.Button1, Nothing)
        Me.DirtyStateManager1.SetIsDirtyChangedCausingEvent(Me.Button1, "")
        Me.Button1.Location = New System.Drawing.Point(449, 225)
        Me.Button1.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(120, 42)
        Me.Button1.TabIndex = 4
        Me.Button1.Text = "OK"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Button2
        '
        Me.Button2.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.MvvmManagerMain.SetEventBindings(Me.Button2, Nothing)
        Me.DirtyStateManager1.SetIsDirtyChangedCausingEvent(Me.Button2, "")
        Me.Button2.Location = New System.Drawing.Point(577, 225)
        Me.Button2.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(120, 42)
        Me.Button2.TabIndex = 5
        Me.Button2.Text = "Abbrechen"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'NullableMultilineTextValue1
        '
        Me.NullableMultilineTextValue1.AssignedManagerComponent = Nothing
        Me.NullableMultilineTextValue1.Borderstyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.NullableMultilineTextValue1.ContentPresentPermission = ActiveDevelop.EntitiesFormsLib.ContentPresentPermissions.Normal
        Me.MvvmManagerMain.SetEventBindings(Me.NullableMultilineTextValue1, Nothing)
        Me.DirtyStateManager1.SetIsDirtyChangedCausingEvent(Me.NullableMultilineTextValue1, "IsDirtyChanged")
        Me.NullableMultilineTextValue1.Location = New System.Drawing.Point(136, 100)
        Me.NullableMultilineTextValue1.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.NullableMultilineTextValue1.MaxLength = 32767
        Me.NullableMultilineTextValue1.Name = "NullableMultilineTextValue1"
        Me.NullableMultilineTextValue1.ObfuscationChar = Nothing
        Me.NullableMultilineTextValue1.PermissionReason = Nothing
        Me.NullableMultilineTextValue1.Size = New System.Drawing.Size(561, 92)
        Me.NullableMultilineTextValue1.TabIndex = 6
        Me.NullableMultilineTextValue1.UIGuid = New System.Guid("d1fc6d7b-03f4-4579-bbcd-429c92972b80")
        Me.NullableMultilineTextValue1.Value = Nothing
        Me.NullableMultilineTextValue1.ValueValidationState = Nothing
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.MvvmManagerMain.SetEventBindings(Me.Label3, Nothing)
        Me.DirtyStateManager1.SetIsDirtyChangedCausingEvent(Me.Label3, "")
        Me.Label3.Location = New System.Drawing.Point(536, 48)
        Me.Label3.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(57, 20)
        Me.Label3.TabIndex = 7
        Me.Label3.Text = "Dauer:"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.MvvmManagerMain.SetEventBindings(Me.Label4, Nothing)
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DirtyStateManager1.SetIsDirtyChangedCausingEvent(Me.Label4, "")
        Me.Label4.Location = New System.Drawing.Point(603, 40)
        Me.Label4.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(86, 29)
        Me.Label4.TabIndex = 8
        Me.Label4.Text = "Label4"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.MvvmManagerMain.SetEventBindings(Me.Label5, Nothing)
        Me.DirtyStateManager1.SetIsDirtyChangedCausingEvent(Me.Label5, "")
        Me.Label5.Location = New System.Drawing.Point(15, 100)
        Me.Label5.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(111, 20)
        Me.Label5.TabIndex = 9
        Me.Label5.Text = "Beschreibung:"
        '
        'DirtyStateManager1
        '
        '
        'MaskedTextBox1
        '
        Me.MvvmManagerMain.SetEventBindings(Me.MaskedTextBox1, Nothing)
        Me.DirtyStateManager1.SetIsDirtyChangedCausingEvent(Me.MaskedTextBox1, "TextChanged")
        Me.MaskedTextBox1.Location = New System.Drawing.Point(136, 42)
        Me.MaskedTextBox1.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.MaskedTextBox1.Mask = "90:90:99"
        Me.MaskedTextBox1.Name = "MaskedTextBox1"
        Me.MaskedTextBox1.Size = New System.Drawing.Size(121, 26)
        Me.MaskedTextBox1.TabIndex = 10
        '
        'MaskedTextBox2
        '
        Me.MvvmManagerMain.SetEventBindings(Me.MaskedTextBox2, Nothing)
        Me.DirtyStateManager1.SetIsDirtyChangedCausingEvent(Me.MaskedTextBox2, "TextChanged")
        Me.MaskedTextBox2.Location = New System.Drawing.Point(369, 42)
        Me.MaskedTextBox2.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.MaskedTextBox2.Mask = "90:90:99"
        Me.MaskedTextBox2.Name = "MaskedTextBox2"
        Me.MaskedTextBox2.Size = New System.Drawing.Size(121, 26)
        Me.MaskedTextBox2.TabIndex = 11
        '
        'StatusStrip1
        '
        Me.MvvmManagerMain.SetEventBindings(Me.StatusStrip1, Nothing)
        Me.DirtyStateManager1.SetIsDirtyChangedCausingEvent(Me.StatusStrip1, "")
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SaveChangesStatusLabel})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 308)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Padding = New System.Windows.Forms.Padding(1, 0, 21, 0)
        Me.StatusStrip1.Size = New System.Drawing.Size(716, 30)
        Me.StatusStrip1.TabIndex = 12
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'SaveChangesStatusLabel
        '
        Me.SaveChangesStatusLabel.Enabled = False
        Me.SaveChangesStatusLabel.Name = "SaveChangesStatusLabel"
        Me.SaveChangesStatusLabel.Size = New System.Drawing.Size(190, 25)
        Me.SaveChangesStatusLabel.Text = "Änderungen speichern"
        '
        'Button3
        '
        Me.Button3.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.MvvmManagerMain.SetEventBindings(Me.Button3, Nothing)
        Me.DirtyStateManager1.SetIsDirtyChangedCausingEvent(Me.Button3, "")
        Me.Button3.Location = New System.Drawing.Point(136, 225)
        Me.Button3.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(199, 42)
        Me.Button3.TabIndex = 13
        Me.Button3.Text = "Messagebox-Test"
        Me.Button3.UseVisualStyleBackColor = True
        '
        'MvvmManagerMain
        '
        Me.MvvmManagerMain.CancelButton = Nothing
        Me.MvvmManagerMain.ContainerControl = Me
        Me.MvvmManagerMain.CurrentContextGuid = New System.Guid("861fafc2-3724-48ce-9bcf-d4a6f0dc5f0b")
        Me.MvvmManagerMain.DataContextType = GetType(EFL_CodedUITestContainer.AddEditTimeCollectionItemViewModel)
        Me.MvvmManagerMain.DataSourceType = GetType(EFL_CodedUITestContainer.AddEditTimeCollectionItemViewModel)
        Me.MvvmManagerMain.DirtyStateManagerComponent = Me.DirtyStateManager1
        Me.MvvmManagerMain.DynamicEventHandlingList = Nothing
        Me.MvvmManagerMain.HostingForm = Me
        Me.MvvmManagerMain.HostingUserControl = Nothing
        '
        'AddEditTimeCollectionItemView
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(716, 338)
        Me.Controls.Add(Me.Button3)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.MaskedTextBox2)
        Me.Controls.Add(Me.MaskedTextBox1)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.NullableMultilineTextValue1)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.MvvmManagerMain.SetEventBindings(Me, Nothing)
        Me.DirtyStateManager1.SetIsDirtyChangedCausingEvent(Me, "")
        Me.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.Name = "AddEditTimeCollectionItemView"
        Me.Text = "AddEditTimeCollectionItem"
        CType(Me.DirtyStateManager1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        CType(Me.MvvmManagerMain, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents MvvmManagerMain As ActiveDevelop.EntitiesFormsLib.MvvmManager
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents NullableMultilineTextValue1 As ActiveDevelop.EntitiesFormsLib.NullableMultilineTextValue
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents MaskedTextBox2 As System.Windows.Forms.MaskedTextBox
    Friend WithEvents MaskedTextBox1 As System.Windows.Forms.MaskedTextBox
    Friend WithEvents DirtyStateManager1 As ActiveDevelop.EntitiesFormsLib.DirtyStateManager
    Friend WithEvents StatusStrip1 As System.Windows.Forms.StatusStrip
    Friend WithEvents SaveChangesStatusLabel As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents Button3 As System.Windows.Forms.Button
End Class
