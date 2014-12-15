<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class BindingModePopupView
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
        Me.gbBindingMode = New System.Windows.Forms.GroupBox()
        Me.rbOneWayToSource = New System.Windows.Forms.RadioButton()
        Me.rbOneTime = New System.Windows.Forms.RadioButton()
        Me.rbOneWay = New System.Windows.Forms.RadioButton()
        Me.rbTwoWay = New System.Windows.Forms.RadioButton()
        Me.gbUpdateSourceTrigger = New System.Windows.Forms.GroupBox()
        Me.rbExplicit = New System.Windows.Forms.RadioButton()
        Me.rbLostFocus = New System.Windows.Forms.RadioButton()
        Me.rbPropertyChangedImmediately = New System.Windows.Forms.RadioButton()
        Me.btnPopupDropUp = New System.Windows.Forms.Button()
        Me.ValidatesCheckBox = New System.Windows.Forms.CheckBox()
        Me.gbBindingMode.SuspendLayout()
        Me.gbUpdateSourceTrigger.SuspendLayout()
        Me.SuspendLayout()
        '
        'gbBindingMode
        '
        Me.gbBindingMode.Controls.Add(Me.rbOneWayToSource)
        Me.gbBindingMode.Controls.Add(Me.rbOneTime)
        Me.gbBindingMode.Controls.Add(Me.rbOneWay)
        Me.gbBindingMode.Controls.Add(Me.rbTwoWay)
        Me.gbBindingMode.Location = New System.Drawing.Point(9, 3)
        Me.gbBindingMode.Margin = New System.Windows.Forms.Padding(2)
        Me.gbBindingMode.Name = "gbBindingMode"
        Me.gbBindingMode.Size = New System.Drawing.Size(390, 152)
        Me.gbBindingMode.TabIndex = 0
        Me.gbBindingMode.TabStop = False
        Me.gbBindingMode.Text = "Binding Mode"
        '
        'rbOneWayToSource
        '
        Me.rbOneWayToSource.AutoSize = True
        Me.rbOneWayToSource.Location = New System.Drawing.Point(14, 117)
        Me.rbOneWayToSource.Margin = New System.Windows.Forms.Padding(2)
        Me.rbOneWayToSource.Name = "rbOneWayToSource"
        Me.rbOneWayToSource.Size = New System.Drawing.Size(177, 27)
        Me.rbOneWayToSource.TabIndex = 3
        Me.rbOneWayToSource.TabStop = True
        Me.rbOneWayToSource.Text = "One way to source"
        Me.rbOneWayToSource.UseVisualStyleBackColor = True
        '
        'rbOneTime
        '
        Me.rbOneTime.AutoSize = True
        Me.rbOneTime.Location = New System.Drawing.Point(14, 84)
        Me.rbOneTime.Margin = New System.Windows.Forms.Padding(2)
        Me.rbOneTime.Name = "rbOneTime"
        Me.rbOneTime.Size = New System.Drawing.Size(106, 27)
        Me.rbOneTime.TabIndex = 2
        Me.rbOneTime.TabStop = True
        Me.rbOneTime.Text = "One time"
        Me.rbOneTime.UseVisualStyleBackColor = True
        '
        'rbOneWay
        '
        Me.rbOneWay.AutoSize = True
        Me.rbOneWay.Location = New System.Drawing.Point(14, 52)
        Me.rbOneWay.Margin = New System.Windows.Forms.Padding(2)
        Me.rbOneWay.Name = "rbOneWay"
        Me.rbOneWay.Size = New System.Drawing.Size(101, 27)
        Me.rbOneWay.TabIndex = 1
        Me.rbOneWay.TabStop = True
        Me.rbOneWay.Text = "One way"
        Me.rbOneWay.UseVisualStyleBackColor = True
        '
        'rbTwoWay
        '
        Me.rbTwoWay.AutoSize = True
        Me.rbTwoWay.Location = New System.Drawing.Point(14, 22)
        Me.rbTwoWay.Margin = New System.Windows.Forms.Padding(2)
        Me.rbTwoWay.Name = "rbTwoWay"
        Me.rbTwoWay.Size = New System.Drawing.Size(100, 27)
        Me.rbTwoWay.TabIndex = 0
        Me.rbTwoWay.TabStop = True
        Me.rbTwoWay.Text = "Two way"
        Me.rbTwoWay.UseVisualStyleBackColor = True
        '
        'gbUpdateSourceTrigger
        '
        Me.gbUpdateSourceTrigger.Controls.Add(Me.rbExplicit)
        Me.gbUpdateSourceTrigger.Controls.Add(Me.rbLostFocus)
        Me.gbUpdateSourceTrigger.Controls.Add(Me.rbPropertyChangedImmediately)
        Me.gbUpdateSourceTrigger.Location = New System.Drawing.Point(9, 160)
        Me.gbUpdateSourceTrigger.Margin = New System.Windows.Forms.Padding(2)
        Me.gbUpdateSourceTrigger.Name = "gbUpdateSourceTrigger"
        Me.gbUpdateSourceTrigger.Size = New System.Drawing.Size(390, 131)
        Me.gbUpdateSourceTrigger.TabIndex = 1
        Me.gbUpdateSourceTrigger.TabStop = False
        Me.gbUpdateSourceTrigger.Text = "Update Source Trigger"
        '
        'rbExplicit
        '
        Me.rbExplicit.AutoSize = True
        Me.rbExplicit.Location = New System.Drawing.Point(14, 90)
        Me.rbExplicit.Margin = New System.Windows.Forms.Padding(2)
        Me.rbExplicit.Name = "rbExplicit"
        Me.rbExplicit.Size = New System.Drawing.Size(88, 27)
        Me.rbExplicit.TabIndex = 9
        Me.rbExplicit.TabStop = True
        Me.rbExplicit.Text = "Explicit"
        Me.rbExplicit.UseVisualStyleBackColor = True
        '
        'rbLostFocus
        '
        Me.rbLostFocus.AutoSize = True
        Me.rbLostFocus.Location = New System.Drawing.Point(14, 24)
        Me.rbLostFocus.Margin = New System.Windows.Forms.Padding(2)
        Me.rbLostFocus.Name = "rbLostFocus"
        Me.rbLostFocus.Size = New System.Drawing.Size(114, 27)
        Me.rbLostFocus.TabIndex = 5
        Me.rbLostFocus.TabStop = True
        Me.rbLostFocus.Text = "Lost Focus"
        Me.rbLostFocus.UseVisualStyleBackColor = True
        '
        'rbPropertyChangedImmediately
        '
        Me.rbPropertyChangedImmediately.AutoSize = True
        Me.rbPropertyChangedImmediately.Location = New System.Drawing.Point(14, 57)
        Me.rbPropertyChangedImmediately.Margin = New System.Windows.Forms.Padding(2)
        Me.rbPropertyChangedImmediately.Name = "rbPropertyChangedImmediately"
        Me.rbPropertyChangedImmediately.Size = New System.Drawing.Size(269, 27)
        Me.rbPropertyChangedImmediately.TabIndex = 4
        Me.rbPropertyChangedImmediately.TabStop = True
        Me.rbPropertyChangedImmediately.Text = "Property changed immediately"
        Me.rbPropertyChangedImmediately.UseVisualStyleBackColor = True
        '
        'btnPopupDropUp
        '
        Me.btnPopupDropUp.Location = New System.Drawing.Point(311, 305)
        Me.btnPopupDropUp.Name = "btnPopupDropUp"
        Me.btnPopupDropUp.Size = New System.Drawing.Size(88, 33)
        Me.btnPopupDropUp.TabIndex = 10
        Me.btnPopupDropUp.Text = "OK"
        Me.btnPopupDropUp.UseVisualStyleBackColor = True
        '
        'ValidatesCheckBox
        '
        Me.ValidatesCheckBox.AutoSize = True
        Me.ValidatesCheckBox.Location = New System.Drawing.Point(23, 309)
        Me.ValidatesCheckBox.Name = "ValidatesCheckBox"
        Me.ValidatesCheckBox.Size = New System.Drawing.Size(220, 27)
        Me.ValidatesCheckBox.TabIndex = 11
        Me.ValidatesCheckBox.Text = "Validates on Data Errors"
        Me.ValidatesCheckBox.UseVisualStyleBackColor = True
        '
        'BindingModePopupView
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 23.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.ValidatesCheckBox)
        Me.Controls.Add(Me.btnPopupDropUp)
        Me.Controls.Add(Me.gbUpdateSourceTrigger)
        Me.Controls.Add(Me.gbBindingMode)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "BindingModePopupView"
        Me.Size = New System.Drawing.Size(416, 358)
        Me.gbBindingMode.ResumeLayout(False)
        Me.gbBindingMode.PerformLayout()
        Me.gbUpdateSourceTrigger.ResumeLayout(False)
        Me.gbUpdateSourceTrigger.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents gbBindingMode As System.Windows.Forms.GroupBox
    Friend WithEvents rbOneWayToSource As System.Windows.Forms.RadioButton
    Friend WithEvents rbOneTime As System.Windows.Forms.RadioButton
    Friend WithEvents rbOneWay As System.Windows.Forms.RadioButton
    Friend WithEvents rbTwoWay As System.Windows.Forms.RadioButton
    Friend WithEvents gbUpdateSourceTrigger As System.Windows.Forms.GroupBox
    Friend WithEvents rbExplicit As System.Windows.Forms.RadioButton
    Friend WithEvents rbLostFocus As System.Windows.Forms.RadioButton
    Friend WithEvents rbPropertyChangedImmediately As System.Windows.Forms.RadioButton
    Friend WithEvents btnPopupDropUp As System.Windows.Forms.Button
    Friend WithEvents ValidatesCheckBox As System.Windows.Forms.CheckBox

End Class
