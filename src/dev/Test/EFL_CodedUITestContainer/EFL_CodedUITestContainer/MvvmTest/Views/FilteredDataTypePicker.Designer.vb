<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FilteredDataTypePicker
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
        CType(Me.MvvmManager1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
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
        '
        'FilteredDataTypePicker
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(284, 261)
        Me.MvvmManager1.SetEventBindings(Me, Nothing)
        Me.Name = "FilteredDataTypePicker"
        Me.Text = "FilteredDataTypePicker"
        CType(Me.MvvmManager1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents MvvmManager1 As ActiveDevelop.EntitiesFormsLib.MvvmManager
End Class
