<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmMvvmPropertyAssignmentRev
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
                myConverters = Nothing
                myControlProperties = Nothing
                myFlatViewModelProperties = Nothing
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
        Me.TableLayoutPanel2 = New System.Windows.Forms.TableLayoutPanel()
        Me.lblCurrentViewModelType = New System.Windows.Forms.Label()
        Me.lblCurrentViewModelFullName = New System.Windows.Forms.Label()
        Me.lblCurrentControl = New System.Windows.Forms.Label()
        Me.lblCurrentControlType = New System.Windows.Forms.Label()
        Me.FormToolTip = New System.Windows.Forms.ToolTip(Me.components)
        Me.btnOK = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.ElementHost1 = New System.Windows.Forms.Integration.ElementHost()
        Me.UcBindingTreeview1 = New ActiveDevelop.EntitiesFormsLib.ucBindingTreeview()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnDelete = New System.Windows.Forms.Button()
        Me.TableLayoutPanel2.SuspendLayout()
        Me.SuspendLayout()
        '
        'TableLayoutPanel2
        '
        Me.TableLayoutPanel2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel2.ColumnCount = 2
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 46.8217!))
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 53.1783!))
        Me.TableLayoutPanel2.Controls.Add(Me.lblCurrentViewModelType, 1, 1)
        Me.TableLayoutPanel2.Controls.Add(Me.lblCurrentViewModelFullName, 1, 0)
        Me.TableLayoutPanel2.Controls.Add(Me.lblCurrentControl, 0, 1)
        Me.TableLayoutPanel2.Controls.Add(Me.lblCurrentControlType, 0, 0)
        Me.TableLayoutPanel2.Location = New System.Drawing.Point(8, 3)
        Me.TableLayoutPanel2.Margin = New System.Windows.Forms.Padding(2)
        Me.TableLayoutPanel2.Name = "TableLayoutPanel2"
        Me.TableLayoutPanel2.RowCount = 2
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.84615!))
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 66.15385!))
        Me.TableLayoutPanel2.Size = New System.Drawing.Size(797, 56)
        Me.TableLayoutPanel2.TabIndex = 17
        '
        'lblCurrentViewModelType
        '
        Me.lblCurrentViewModelType.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lblCurrentViewModelType.AutoEllipsis = True
        Me.lblCurrentViewModelType.AutoSize = True
        Me.lblCurrentViewModelType.Font = New System.Drawing.Font("Segoe UI", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCurrentViewModelType.Location = New System.Drawing.Point(375, 21)
        Me.lblCurrentViewModelType.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lblCurrentViewModelType.Name = "lblCurrentViewModelType"
        Me.lblCurrentViewModelType.Size = New System.Drawing.Size(123, 32)
        Me.lblCurrentViewModelType.TabIndex = 3
        Me.lblCurrentViewModelType.Text = "- not set -"
        '
        'lblCurrentViewModelFullName
        '
        Me.lblCurrentViewModelFullName.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lblCurrentViewModelFullName.AutoEllipsis = True
        Me.lblCurrentViewModelFullName.AutoSize = True
        Me.lblCurrentViewModelFullName.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCurrentViewModelFullName.Location = New System.Drawing.Point(380, 0)
        Me.lblCurrentViewModelFullName.Margin = New System.Windows.Forms.Padding(7, 0, 2, 0)
        Me.lblCurrentViewModelFullName.Name = "lblCurrentViewModelFullName"
        Me.lblCurrentViewModelFullName.Size = New System.Drawing.Size(319, 17)
        Me.lblCurrentViewModelFullName.TabIndex = 2
        Me.lblCurrentViewModelFullName.Text = "Set the MVVM-Manager's DataContextType property."
        '
        'lblCurrentControl
        '
        Me.lblCurrentControl.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lblCurrentControl.AutoEllipsis = True
        Me.lblCurrentControl.AutoSize = True
        Me.lblCurrentControl.Font = New System.Drawing.Font("Segoe UI", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCurrentControl.Location = New System.Drawing.Point(2, 21)
        Me.lblCurrentControl.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lblCurrentControl.Name = "lblCurrentControl"
        Me.lblCurrentControl.Size = New System.Drawing.Size(94, 32)
        Me.lblCurrentControl.TabIndex = 1
        Me.lblCurrentControl.Text = "Control"
        '
        'lblCurrentControlType
        '
        Me.lblCurrentControlType.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lblCurrentControlType.AutoEllipsis = True
        Me.lblCurrentControlType.AutoSize = True
        Me.lblCurrentControlType.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCurrentControlType.Location = New System.Drawing.Point(7, 0)
        Me.lblCurrentControlType.Margin = New System.Windows.Forms.Padding(7, 0, 2, 0)
        Me.lblCurrentControlType.Name = "lblCurrentControlType"
        Me.lblCurrentControlType.Size = New System.Drawing.Size(80, 17)
        Me.lblCurrentControlType.TabIndex = 0
        Me.lblCurrentControlType.Text = "Control type"
        '
        'btnOK
        '
        Me.btnOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnOK.Location = New System.Drawing.Point(643, 515)
        Me.btnOK.Margin = New System.Windows.Forms.Padding(2)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(77, 23)
        Me.btnOK.TabIndex = 0
        Me.btnOK.Text = "OK"
        Me.btnOK.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label1.Location = New System.Drawing.Point(9, 515)
        Me.Label1.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(510, 30)
        Me.Label1.TabIndex = 18
        Me.Label1.Text = "Note: If you implement an IValueConverter for the Value property of a NullableVal" &
    "ueText control, please remember to return StringValue and not String as the Conv" &
    "ert method's Data Type."
        '
        'ElementHost1
        '
        Me.ElementHost1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ElementHost1.Location = New System.Drawing.Point(8, 64)
        Me.ElementHost1.Name = "ElementHost1"
        Me.ElementHost1.Size = New System.Drawing.Size(797, 445)
        Me.ElementHost1.TabIndex = 19
        Me.ElementHost1.Text = "ElementHost1"
        Me.ElementHost1.Child = Me.UcBindingTreeview1
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(724, 515)
        Me.btnCancel.Margin = New System.Windows.Forms.Padding(2)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(77, 23)
        Me.btnCancel.TabIndex = 20
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'btnDelete
        '
        Me.btnDelete.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnDelete.Enabled = False
        Me.btnDelete.Location = New System.Drawing.Point(523, 515)
        Me.btnDelete.Margin = New System.Windows.Forms.Padding(2)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(77, 23)
        Me.btnDelete.TabIndex = 21
        Me.btnDelete.Text = "Delete"
        Me.btnDelete.UseVisualStyleBackColor = True
        '
        'frmMvvmPropertyAssignmentRev
        '
        Me.AcceptButton = Me.btnOK
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.CancelButton = Me.btnCancel
        Me.ClientSize = New System.Drawing.Size(812, 547)
        Me.Controls.Add(Me.btnDelete)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.ElementHost1)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.TableLayoutPanel2)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Margin = New System.Windows.Forms.Padding(2)
        Me.MinimumSize = New System.Drawing.Size(370, 254)
        Me.Name = "frmMvvmPropertyAssignmentRev"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Mvvm Property Assignment"
        Me.TableLayoutPanel2.ResumeLayout(False)
        Me.TableLayoutPanel2.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents TableLayoutPanel2 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents lblCurrentViewModelType As System.Windows.Forms.Label
    Friend WithEvents lblCurrentControl As System.Windows.Forms.Label
    Friend WithEvents FormToolTip As System.Windows.Forms.ToolTip
    Friend WithEvents lblCurrentControlType As System.Windows.Forms.Label
    Friend WithEvents lblCurrentViewModelFullName As System.Windows.Forms.Label
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents ElementHost1 As Windows.Forms.Integration.ElementHost
    Friend UcBindingTreeview1 As ucBindingTreeview
    Friend WithEvents btnCancel As Windows.Forms.Button
    Friend WithEvents btnDelete As Windows.Forms.Button
End Class
