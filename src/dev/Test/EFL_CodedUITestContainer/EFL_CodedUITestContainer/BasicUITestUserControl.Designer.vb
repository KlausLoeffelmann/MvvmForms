<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class BasicUITestUserControl
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(BasicUITestUserControl))
        Me.NullableTextBoxCaption = New System.Windows.Forms.Label()
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.TestConfigurationPropertiesButton = New System.Windows.Forms.Button()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.SingleResult4 = New System.Windows.Forms.Label()
        Me.SingleResult3 = New System.Windows.Forms.Label()
        Me.SingleResult2 = New System.Windows.Forms.Label()
        Me.SingleResult1 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.SubTest02Button = New System.Windows.Forms.Button()
        Me.SubTest01Button = New System.Windows.Forms.Button()
        Me.SetNullableRelationPopupPropertiesButton = New System.Windows.Forms.Button()
        Me.SetNullableDateValuePropertiesButton = New System.Windows.Forms.Button()
        Me.SetNullableNumValuePropertiesButton = New System.Windows.Forms.Button()
        Me.SetNullableIntValuePropertiesButton = New System.Windows.Forms.Button()
        Me.SetNullableMultilineTextValuePropertiesButton = New System.Windows.Forms.Button()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.NullableValueRelationPopupInput = New ActiveDevelop.EntitiesFormsLib.NullableValueRelationPopup()
        Me.ftbcmMain = New ActiveDevelop.EntitiesFormsLib.FormToBusinessClassManager()
        Me.SetNullableTextValuePropertiesButton = New System.Windows.Forms.Button()
        Me.NullableMultilineTextValueInput = New ActiveDevelop.EntitiesFormsLib.NullableMultilineTextValue()
        Me.NullableTextValueInput = New ActiveDevelop.EntitiesFormsLib.NullableTextValue()
        Me.NullableNumValueInput = New ActiveDevelop.EntitiesFormsLib.NullableNumValue()
        Me.NullableDateValueInput = New ActiveDevelop.EntitiesFormsLib.NullableDateValue()
        Me.NullableIntValueInput = New ActiveDevelop.EntitiesFormsLib.NullableIntValue()
        Me.MainTab = New System.Windows.Forms.TabControl()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.ClearTestResultButton = New System.Windows.Forms.Button()
        Me.ResultTextBox = New System.Windows.Forms.TextBox()
        Me.TabPage2 = New System.Windows.Forms.TabPage()
        Me.MainPropertyGrid = New System.Windows.Forms.PropertyGrid()
        Me.StatusStripMain = New System.Windows.Forms.StatusStrip()
        Me.IsDirtyToolStripStatusLabel = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ResetEventCounterToolStripDropDownButton = New System.Windows.Forms.ToolStripDropDownButton()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.nvrPreserveInputTest = New ActiveDevelop.EntitiesFormsLib.NullableValueRelationPopup()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        CType(Me.NullableValueRelationPopupInput, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ftbcmMain, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.MainTab.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        Me.StatusStripMain.SuspendLayout()
        CType(Me.nvrPreserveInputTest, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'NullableTextBoxCaption
        '
        Me.NullableTextBoxCaption.AutoSize = True
        Me.NullableTextBoxCaption.Location = New System.Drawing.Point(15, 14)
        Me.NullableTextBoxCaption.Name = "NullableTextBoxCaption"
        Me.NullableTextBoxCaption.Size = New System.Drawing.Size(87, 13)
        Me.NullableTextBoxCaption.TabIndex = 0
        Me.NullableTextBoxCaption.Text = "NullableTextBox:"
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SplitContainer1.Location = New System.Drawing.Point(3, 3)
        Me.SplitContainer1.Name = "SplitContainer1"
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.AutoScroll = True
        Me.SplitContainer1.Panel1.Controls.Add(Me.Button2)
        Me.SplitContainer1.Panel1.Controls.Add(Me.Label7)
        Me.SplitContainer1.Panel1.Controls.Add(Me.nvrPreserveInputTest)
        Me.SplitContainer1.Panel1.Controls.Add(Me.TestConfigurationPropertiesButton)
        Me.SplitContainer1.Panel1.Controls.Add(Me.Button1)
        Me.SplitContainer1.Panel1.Controls.Add(Me.SingleResult4)
        Me.SplitContainer1.Panel1.Controls.Add(Me.SingleResult3)
        Me.SplitContainer1.Panel1.Controls.Add(Me.SingleResult2)
        Me.SplitContainer1.Panel1.Controls.Add(Me.SingleResult1)
        Me.SplitContainer1.Panel1.Controls.Add(Me.Label6)
        Me.SplitContainer1.Panel1.Controls.Add(Me.SubTest02Button)
        Me.SplitContainer1.Panel1.Controls.Add(Me.SubTest01Button)
        Me.SplitContainer1.Panel1.Controls.Add(Me.SetNullableRelationPopupPropertiesButton)
        Me.SplitContainer1.Panel1.Controls.Add(Me.SetNullableDateValuePropertiesButton)
        Me.SplitContainer1.Panel1.Controls.Add(Me.SetNullableNumValuePropertiesButton)
        Me.SplitContainer1.Panel1.Controls.Add(Me.SetNullableIntValuePropertiesButton)
        Me.SplitContainer1.Panel1.Controls.Add(Me.SetNullableMultilineTextValuePropertiesButton)
        Me.SplitContainer1.Panel1.Controls.Add(Me.Label5)
        Me.SplitContainer1.Panel1.Controls.Add(Me.Label4)
        Me.SplitContainer1.Panel1.Controls.Add(Me.Label3)
        Me.SplitContainer1.Panel1.Controls.Add(Me.Label2)
        Me.SplitContainer1.Panel1.Controls.Add(Me.Label1)
        Me.SplitContainer1.Panel1.Controls.Add(Me.NullableValueRelationPopupInput)
        Me.SplitContainer1.Panel1.Controls.Add(Me.SetNullableTextValuePropertiesButton)
        Me.SplitContainer1.Panel1.Controls.Add(Me.NullableTextBoxCaption)
        Me.SplitContainer1.Panel1.Controls.Add(Me.NullableMultilineTextValueInput)
        Me.SplitContainer1.Panel1.Controls.Add(Me.NullableTextValueInput)
        Me.SplitContainer1.Panel1.Controls.Add(Me.NullableNumValueInput)
        Me.SplitContainer1.Panel1.Controls.Add(Me.NullableDateValueInput)
        Me.SplitContainer1.Panel1.Controls.Add(Me.NullableIntValueInput)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.MainTab)
        Me.SplitContainer1.Size = New System.Drawing.Size(810, 435)
        Me.SplitContainer1.SplitterDistance = 506
        Me.SplitContainer1.TabIndex = 0
        '
        'TestConfigurationPropertiesButton
        '
        Me.TestConfigurationPropertiesButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TestConfigurationPropertiesButton.Location = New System.Drawing.Point(162, 238)
        Me.TestConfigurationPropertiesButton.Name = "TestConfigurationPropertiesButton"
        Me.TestConfigurationPropertiesButton.Size = New System.Drawing.Size(267, 23)
        Me.TestConfigurationPropertiesButton.TabIndex = 28
        Me.TestConfigurationPropertiesButton.Text = "Testconfigurations-Eigenschaften"
        Me.TestConfigurationPropertiesButton.UseVisualStyleBackColor = True
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(14, 370)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(162, 32)
        Me.Button1.TabIndex = 27
        Me.Button1.Text = "SubTest 3: Event Order Test"
        Me.Button1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Button1.UseVisualStyleBackColor = True
        '
        'SingleResult4
        '
        Me.SingleResult4.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SingleResult4.AutoSize = True
        Me.SingleResult4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.SingleResult4.Location = New System.Drawing.Point(219, 360)
        Me.SingleResult4.Name = "SingleResult4"
        Me.SingleResult4.Size = New System.Drawing.Size(59, 15)
        Me.SingleResult4.TabIndex = 25
        Me.SingleResult4.Text = "#Result4#"
        '
        'SingleResult3
        '
        Me.SingleResult3.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SingleResult3.AutoSize = True
        Me.SingleResult3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.SingleResult3.Location = New System.Drawing.Point(219, 344)
        Me.SingleResult3.Name = "SingleResult3"
        Me.SingleResult3.Size = New System.Drawing.Size(59, 15)
        Me.SingleResult3.TabIndex = 24
        Me.SingleResult3.Text = "#Result3#"
        '
        'SingleResult2
        '
        Me.SingleResult2.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SingleResult2.AutoSize = True
        Me.SingleResult2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.SingleResult2.Location = New System.Drawing.Point(219, 327)
        Me.SingleResult2.Name = "SingleResult2"
        Me.SingleResult2.Size = New System.Drawing.Size(59, 15)
        Me.SingleResult2.TabIndex = 23
        Me.SingleResult2.Text = "#Result2#"
        '
        'SingleResult1
        '
        Me.SingleResult1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SingleResult1.AutoSize = True
        Me.SingleResult1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.SingleResult1.Location = New System.Drawing.Point(219, 310)
        Me.SingleResult1.Name = "SingleResult1"
        Me.SingleResult1.Size = New System.Drawing.Size(59, 15)
        Me.SingleResult1.TabIndex = 22
        Me.SingleResult1.Text = "#Result1#"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(216, 294)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(64, 13)
        Me.Label6.TabIndex = 20
        Me.Label6.Text = "Test results:"
        '
        'SubTest02Button
        '
        Me.SubTest02Button.Location = New System.Drawing.Point(14, 332)
        Me.SubTest02Button.Name = "SubTest02Button"
        Me.SubTest02Button.Size = New System.Drawing.Size(162, 32)
        Me.SubTest02Button.TabIndex = 19
        Me.SubTest02Button.Text = "SubTest 2: IsDirtyTest"
        Me.SubTest02Button.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.SubTest02Button.UseVisualStyleBackColor = True
        '
        'SubTest01Button
        '
        Me.SubTest01Button.Location = New System.Drawing.Point(14, 294)
        Me.SubTest01Button.Name = "SubTest01Button"
        Me.SubTest01Button.Size = New System.Drawing.Size(162, 32)
        Me.SubTest01Button.TabIndex = 18
        Me.SubTest01Button.Text = "SubTest 1: Popup Tests"
        Me.SubTest01Button.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.SubTest01Button.UseVisualStyleBackColor = True
        '
        'SetNullableRelationPopupPropertiesButton
        '
        Me.SetNullableRelationPopupPropertiesButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SetNullableRelationPopupPropertiesButton.Location = New System.Drawing.Point(439, 185)
        Me.SetNullableRelationPopupPropertiesButton.Name = "SetNullableRelationPopupPropertiesButton"
        Me.SetNullableRelationPopupPropertiesButton.Size = New System.Drawing.Size(50, 23)
        Me.SetNullableRelationPopupPropertiesButton.TabIndex = 17
        Me.SetNullableRelationPopupPropertiesButton.Text = "Prop's"
        Me.SetNullableRelationPopupPropertiesButton.UseVisualStyleBackColor = True
        '
        'SetNullableDateValuePropertiesButton
        '
        Me.SetNullableDateValuePropertiesButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SetNullableDateValuePropertiesButton.Location = New System.Drawing.Point(439, 158)
        Me.SetNullableDateValuePropertiesButton.Name = "SetNullableDateValuePropertiesButton"
        Me.SetNullableDateValuePropertiesButton.Size = New System.Drawing.Size(50, 23)
        Me.SetNullableDateValuePropertiesButton.TabIndex = 14
        Me.SetNullableDateValuePropertiesButton.Text = "Prop's"
        Me.SetNullableDateValuePropertiesButton.UseVisualStyleBackColor = True
        '
        'SetNullableNumValuePropertiesButton
        '
        Me.SetNullableNumValuePropertiesButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SetNullableNumValuePropertiesButton.Location = New System.Drawing.Point(439, 132)
        Me.SetNullableNumValuePropertiesButton.Name = "SetNullableNumValuePropertiesButton"
        Me.SetNullableNumValuePropertiesButton.Size = New System.Drawing.Size(50, 23)
        Me.SetNullableNumValuePropertiesButton.TabIndex = 11
        Me.SetNullableNumValuePropertiesButton.Text = "Prop's"
        Me.SetNullableNumValuePropertiesButton.UseVisualStyleBackColor = True
        '
        'SetNullableIntValuePropertiesButton
        '
        Me.SetNullableIntValuePropertiesButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SetNullableIntValuePropertiesButton.Location = New System.Drawing.Point(439, 106)
        Me.SetNullableIntValuePropertiesButton.Name = "SetNullableIntValuePropertiesButton"
        Me.SetNullableIntValuePropertiesButton.Size = New System.Drawing.Size(50, 23)
        Me.SetNullableIntValuePropertiesButton.TabIndex = 8
        Me.SetNullableIntValuePropertiesButton.Text = "Prop's"
        Me.SetNullableIntValuePropertiesButton.UseVisualStyleBackColor = True
        '
        'SetNullableMultilineTextValuePropertiesButton
        '
        Me.SetNullableMultilineTextValuePropertiesButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SetNullableMultilineTextValuePropertiesButton.Location = New System.Drawing.Point(439, 39)
        Me.SetNullableMultilineTextValuePropertiesButton.Name = "SetNullableMultilineTextValuePropertiesButton"
        Me.SetNullableMultilineTextValuePropertiesButton.Size = New System.Drawing.Size(50, 23)
        Me.SetNullableMultilineTextValuePropertiesButton.TabIndex = 5
        Me.SetNullableMultilineTextValuePropertiesButton.Text = "Prop's"
        Me.SetNullableMultilineTextValuePropertiesButton.UseVisualStyleBackColor = True
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(15, 186)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(124, 13)
        Me.Label5.TabIndex = 15
        Me.Label5.Text = "Nullable - RelationPopup"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(15, 163)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(128, 13)
        Me.Label4.TabIndex = 12
        Me.Label4.Text = "Nullable - Date - TextBox:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(15, 137)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(127, 13)
        Me.Label3.TabIndex = 9
        Me.Label3.Text = "Nullable - Num - TextBox:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(15, 111)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(117, 13)
        Me.Label2.TabIndex = 6
        Me.Label2.Text = "Nullable - Int - TextBox:"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(15, 48)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(143, 13)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "Nullable - MultilineText - Box:"
        '
        'NullableValueRelationPopupInput
        '
        Me.NullableValueRelationPopupInput.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.NullableValueRelationPopupInput.AssignedManagerComponent = Me.ftbcmMain
        Me.NullableValueRelationPopupInput.AutoResizeColumnsOnOpen = False
        Me.NullableValueRelationPopupInput.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.None
        Me.NullableValueRelationPopupInput.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.None
        Me.NullableValueRelationPopupInput.BeepOnFailedValidation = False
        Me.NullableValueRelationPopupInput.Borderstyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.NullableValueRelationPopupInput.ContentPresentPermission = ActiveDevelop.EntitiesFormsLib.ContentPresentPermissions.Normal
        Me.NullableValueRelationPopupInput.DataSource = Nothing
        Me.NullableValueRelationPopupInput.DisplayMember = Nothing
        Me.NullableValueRelationPopupInput.HasAddButton = True
        Me.NullableValueRelationPopupInput.HideButtons = False
        Me.NullableValueRelationPopupInput.IsPopupAutoSize = False
        Me.NullableValueRelationPopupInput.IsPopupResizable = True
        Me.NullableValueRelationPopupInput.Location = New System.Drawing.Point(162, 186)
        Me.NullableValueRelationPopupInput.MinimumPopupSize = New System.Drawing.Size(267, 80)
        Me.NullableValueRelationPopupInput.MultiSelect = False
        Me.NullableValueRelationPopupInput.Name = "NullableValueRelationPopupInput"
        Me.NullableValueRelationPopupInput.NullValueString = "* - - - *"
        Me.NullableValueRelationPopupInput.PermissionReason = Nothing
        Me.NullableValueRelationPopupInput.Searchable = True
        Me.NullableValueRelationPopupInput.SearchColumnHeaderFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.NullableValueRelationPopupInput.SearchKeywordOrChar = Global.Microsoft.VisualBasic.ChrW(59)
        Me.NullableValueRelationPopupInput.Size = New System.Drawing.Size(267, 20)
        Me.NullableValueRelationPopupInput.TabIndex = 16
        Me.NullableValueRelationPopupInput.UIGuid = New System.Guid("bbac7d92-bf33-49e8-baf0-3e960048745c")
        Me.NullableValueRelationPopupInput.Value = Nothing
        Me.NullableValueRelationPopupInput.ValueMember = Nothing
        '
        'ftbcmMain
        '
        Me.ftbcmMain.CancelButton = Nothing
        Me.ftbcmMain.ContainerControl = Me
        Me.ftbcmMain.DataSourceType = Nothing
        Me.ftbcmMain.DynamicEventHandlingList = Nothing
        Me.ftbcmMain.HostingForm = Nothing
        Me.ftbcmMain.HostingUserControl = Me
        '
        'SetNullableTextValuePropertiesButton
        '
        Me.SetNullableTextValuePropertiesButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SetNullableTextValuePropertiesButton.Location = New System.Drawing.Point(439, 10)
        Me.SetNullableTextValuePropertiesButton.Name = "SetNullableTextValuePropertiesButton"
        Me.SetNullableTextValuePropertiesButton.Size = New System.Drawing.Size(50, 23)
        Me.SetNullableTextValuePropertiesButton.TabIndex = 2
        Me.SetNullableTextValuePropertiesButton.Text = "Prop's"
        Me.SetNullableTextValuePropertiesButton.UseVisualStyleBackColor = True
        '
        'NullableMultilineTextValueInput
        '
        Me.NullableMultilineTextValueInput.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.NullableMultilineTextValueInput.AssignedManagerComponent = Me.ftbcmMain
        Me.NullableMultilineTextValueInput.Borderstyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.NullableMultilineTextValueInput.ContentPresentPermission = ActiveDevelop.EntitiesFormsLib.ContentPresentPermissions.Normal
        Me.NullableMultilineTextValueInput.Location = New System.Drawing.Point(162, 43)
        Me.NullableMultilineTextValueInput.MaxLength = 32767
        Me.NullableMultilineTextValueInput.Name = "NullableMultilineTextValueInput"
        Me.NullableMultilineTextValueInput.ObfuscationChar = Nothing
        Me.NullableMultilineTextValueInput.PermissionReason = Nothing
        Me.NullableMultilineTextValueInput.Size = New System.Drawing.Size(267, 59)
        Me.NullableMultilineTextValueInput.TabIndex = 4
        Me.NullableMultilineTextValueInput.UIGuid = New System.Guid("e4768cc4-c3d7-44ca-b8d9-df9994acaf1d")
        Me.NullableMultilineTextValueInput.Value = Nothing
        Me.NullableMultilineTextValueInput.ValueValidationState = Nothing
        '
        'NullableTextValueInput
        '
        Me.NullableTextValueInput.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.NullableTextValueInput.AssignedManagerComponent = Me.ftbcmMain
        Me.NullableTextValueInput.Borderstyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.NullableTextValueInput.ContentPresentPermission = ActiveDevelop.EntitiesFormsLib.ContentPresentPermissions.Normal
        Me.NullableTextValueInput.Location = New System.Drawing.Point(162, 12)
        Me.NullableTextValueInput.MaxLength = 32767
        Me.NullableTextValueInput.Name = "NullableTextValueInput"
        Me.NullableTextValueInput.ObfuscationChar = Nothing
        Me.NullableTextValueInput.PermissionReason = Nothing
        Me.NullableTextValueInput.Size = New System.Drawing.Size(267, 20)
        Me.NullableTextValueInput.TabIndex = 1
        Me.NullableTextValueInput.UIGuid = New System.Guid("924325d4-e5ec-4bee-b59f-321464d410e5")
        Me.NullableTextValueInput.Value = Nothing
        Me.NullableTextValueInput.ValueValidationState = Nothing
        '
        'NullableNumValueInput
        '
        Me.NullableNumValueInput.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.NullableNumValueInput.AssignedManagerComponent = Me.ftbcmMain
        Me.NullableNumValueInput.Borderstyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.NullableNumValueInput.ContentPresentPermission = ActiveDevelop.EntitiesFormsLib.ContentPresentPermissions.Normal
        Me.NullableNumValueInput.CurrencySymbolString = Nothing
        Me.NullableNumValueInput.Location = New System.Drawing.Point(162, 134)
        Me.NullableNumValueInput.MaxLength = 32767
        Me.NullableNumValueInput.MaxValue = Nothing
        Me.NullableNumValueInput.MinValue = New Decimal(New Integer() {0, 0, 0, 0})
        Me.NullableNumValueInput.Name = "NullableNumValueInput"
        Me.NullableNumValueInput.ObfuscationChar = Nothing
        Me.NullableNumValueInput.PermissionReason = Nothing
        Me.NullableNumValueInput.Size = New System.Drawing.Size(267, 20)
        Me.NullableNumValueInput.TabIndex = 10
        Me.NullableNumValueInput.UIGuid = New System.Guid("42bc45f4-cd10-4d30-b4b8-2bbe6e62f113")
        Me.NullableNumValueInput.Value = Nothing
        Me.NullableNumValueInput.ValueValidationState = Nothing
        '
        'NullableDateValueInput
        '
        Me.NullableDateValueInput.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.NullableDateValueInput.AssignedManagerComponent = Me.ftbcmMain
        Me.NullableDateValueInput.Borderstyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.NullableDateValueInput.ContentPresentPermission = ActiveDevelop.EntitiesFormsLib.ContentPresentPermissions.Normal
        Me.NullableDateValueInput.DaysDistanceBetweenLinkedControl = Nothing
        Me.NullableDateValueInput.LinkedToNullableDateControl = Nothing
        Me.NullableDateValueInput.Location = New System.Drawing.Point(162, 160)
        Me.NullableDateValueInput.MaxLength = 32767
        Me.NullableDateValueInput.Name = "NullableDateValueInput"
        Me.NullableDateValueInput.ObfuscationChar = Nothing
        Me.NullableDateValueInput.PermissionReason = Nothing
        Me.NullableDateValueInput.Size = New System.Drawing.Size(267, 20)
        Me.NullableDateValueInput.TabIndex = 13
        Me.NullableDateValueInput.UIGuid = New System.Guid("78321b4d-7cac-41a5-ae31-6b2c7f39719c")
        Me.NullableDateValueInput.Value = Nothing
        Me.NullableDateValueInput.ValueValidationState = Nothing
        '
        'NullableIntValueInput
        '
        Me.NullableIntValueInput.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.NullableIntValueInput.AssignedManagerComponent = Me.ftbcmMain
        Me.NullableIntValueInput.Borderstyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.NullableIntValueInput.ContentPresentPermission = ActiveDevelop.EntitiesFormsLib.ContentPresentPermissions.Normal
        Me.NullableIntValueInput.Location = New System.Drawing.Point(162, 108)
        Me.NullableIntValueInput.MaxLength = 32767
        Me.NullableIntValueInput.MaxValue = Nothing
        Me.NullableIntValueInput.MinValue = 0
        Me.NullableIntValueInput.Name = "NullableIntValueInput"
        Me.NullableIntValueInput.ObfuscationChar = Nothing
        Me.NullableIntValueInput.PermissionReason = Nothing
        Me.NullableIntValueInput.Size = New System.Drawing.Size(267, 20)
        Me.NullableIntValueInput.TabIndex = 7
        Me.NullableIntValueInput.UIGuid = New System.Guid("2576c8b7-270f-40e8-aa07-1a1cf3f5dd62")
        Me.NullableIntValueInput.Value = Nothing
        Me.NullableIntValueInput.ValueValidationState = Nothing
        '
        'MainTab
        '
        Me.MainTab.Controls.Add(Me.TabPage1)
        Me.MainTab.Controls.Add(Me.TabPage2)
        Me.MainTab.Location = New System.Drawing.Point(14, 17)
        Me.MainTab.Name = "MainTab"
        Me.MainTab.SelectedIndex = 0
        Me.MainTab.Size = New System.Drawing.Size(255, 395)
        Me.MainTab.TabIndex = 0
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.ClearTestResultButton)
        Me.TabPage1.Controls.Add(Me.ResultTextBox)
        Me.TabPage1.Location = New System.Drawing.Point(4, 22)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(247, 369)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "Test results"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'ClearTestResultButton
        '
        Me.ClearTestResultButton.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ClearTestResultButton.Location = New System.Drawing.Point(6, 342)
        Me.ClearTestResultButton.Name = "ClearTestResultButton"
        Me.ClearTestResultButton.Size = New System.Drawing.Size(235, 23)
        Me.ClearTestResultButton.TabIndex = 27
        Me.ClearTestResultButton.Text = "Clear Test Results"
        Me.ClearTestResultButton.UseVisualStyleBackColor = True
        '
        'ResultTextBox
        '
        Me.ResultTextBox.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ResultTextBox.Location = New System.Drawing.Point(6, 9)
        Me.ResultTextBox.Multiline = True
        Me.ResultTextBox.Name = "ResultTextBox"
        Me.ResultTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.ResultTextBox.Size = New System.Drawing.Size(235, 328)
        Me.ResultTextBox.TabIndex = 26
        '
        'TabPage2
        '
        Me.TabPage2.Controls.Add(Me.MainPropertyGrid)
        Me.TabPage2.Location = New System.Drawing.Point(4, 22)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(247, 369)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "Properties"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'MainPropertyGrid
        '
        Me.MainPropertyGrid.Dock = System.Windows.Forms.DockStyle.Fill
        Me.MainPropertyGrid.Location = New System.Drawing.Point(3, 3)
        Me.MainPropertyGrid.Name = "MainPropertyGrid"
        Me.MainPropertyGrid.Size = New System.Drawing.Size(241, 363)
        Me.MainPropertyGrid.TabIndex = 1
        '
        'StatusStripMain
        '
        Me.StatusStripMain.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.IsDirtyToolStripStatusLabel, Me.ResetEventCounterToolStripDropDownButton})
        Me.StatusStripMain.Location = New System.Drawing.Point(0, 463)
        Me.StatusStripMain.Name = "StatusStripMain"
        Me.StatusStripMain.Size = New System.Drawing.Size(825, 24)
        Me.StatusStripMain.TabIndex = 1
        Me.StatusStripMain.Text = "StatusStrip1"
        '
        'IsDirtyToolStripStatusLabel
        '
        Me.IsDirtyToolStripStatusLabel.BorderSides = CType((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) _
            Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) _
            Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom), System.Windows.Forms.ToolStripStatusLabelBorderSides)
        Me.IsDirtyToolStripStatusLabel.BorderStyle = System.Windows.Forms.Border3DStyle.Sunken
        Me.IsDirtyToolStripStatusLabel.DoubleClickEnabled = True
        Me.IsDirtyToolStripStatusLabel.Name = "IsDirtyToolStripStatusLabel"
        Me.IsDirtyToolStripStatusLabel.Size = New System.Drawing.Size(109, 19)
        Me.IsDirtyToolStripStatusLabel.Text = "Keine Änderungen"
        '
        'ResetEventCounterToolStripDropDownButton
        '
        Me.ResetEventCounterToolStripDropDownButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.ResetEventCounterToolStripDropDownButton.Image = CType(resources.GetObject("ResetEventCounterToolStripDropDownButton.Image"), System.Drawing.Image)
        Me.ResetEventCounterToolStripDropDownButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ResetEventCounterToolStripDropDownButton.Name = "ResetEventCounterToolStripDropDownButton"
        Me.ResetEventCounterToolStripDropDownButton.ShowDropDownArrow = False
        Me.ResetEventCounterToolStripDropDownButton.Size = New System.Drawing.Size(111, 22)
        Me.ResetEventCounterToolStripDropDownButton.Text = "ResetEventCounter"
        '
        'Button2
        '
        Me.Button2.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button2.Location = New System.Drawing.Point(439, 211)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(50, 23)
        Me.Button2.TabIndex = 31
        Me.Button2.Text = "Prop's"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(15, 212)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(126, 13)
        Me.Label7.TabIndex = 29
        Me.Label7.Text = "Nullable - (PreserveInput)"
        '
        'nvrPreserveInputTest
        '
        Me.nvrPreserveInputTest.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.nvrPreserveInputTest.AssignedManagerComponent = Me.ftbcmMain
        Me.nvrPreserveInputTest.AutoResizeColumnsOnOpen = False
        Me.nvrPreserveInputTest.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.None
        Me.nvrPreserveInputTest.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.None
        Me.nvrPreserveInputTest.BeepOnFailedValidation = False
        Me.nvrPreserveInputTest.Borderstyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.nvrPreserveInputTest.ContentPresentPermission = ActiveDevelop.EntitiesFormsLib.ContentPresentPermissions.Normal
        Me.nvrPreserveInputTest.DataSource = Nothing
        Me.nvrPreserveInputTest.DisplayMember = Nothing
        Me.nvrPreserveInputTest.HasAddButton = True
        Me.nvrPreserveInputTest.HideButtons = False
        Me.nvrPreserveInputTest.IsPopupAutoSize = False
        Me.nvrPreserveInputTest.IsPopupResizable = True
        Me.nvrPreserveInputTest.Location = New System.Drawing.Point(162, 212)
        Me.nvrPreserveInputTest.MinimumPopupSize = New System.Drawing.Size(267, 80)
        Me.nvrPreserveInputTest.MultiSelect = False
        Me.nvrPreserveInputTest.Name = "nvrPreserveInputTest"
        Me.nvrPreserveInputTest.NullValueString = "* - - - *"
        Me.nvrPreserveInputTest.PermissionReason = Nothing
        Me.nvrPreserveInputTest.PreserveInput = True
        Me.nvrPreserveInputTest.Searchable = True
        Me.nvrPreserveInputTest.SearchColumnHeaderFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.nvrPreserveInputTest.SearchKeywordOrChar = Global.Microsoft.VisualBasic.ChrW(59)
        Me.nvrPreserveInputTest.Size = New System.Drawing.Size(267, 20)
        Me.nvrPreserveInputTest.TabIndex = 30
        Me.nvrPreserveInputTest.UIGuid = New System.Guid("bbac7d92-bf33-49e8-baf0-3e960048745c")
        Me.nvrPreserveInputTest.Value = Nothing
        Me.nvrPreserveInputTest.ValueMember = Nothing
        '
        'BasicUITestUserControl
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.StatusStripMain)
        Me.Controls.Add(Me.SplitContainer1)
        Me.Name = "BasicUITestUserControl"
        Me.Size = New System.Drawing.Size(825, 487)
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel1.PerformLayout()
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        CType(Me.NullableValueRelationPopupInput, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ftbcmMain, System.ComponentModel.ISupportInitialize).EndInit()
        Me.MainTab.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage1.PerformLayout()
        Me.TabPage2.ResumeLayout(False)
        Me.StatusStripMain.ResumeLayout(False)
        Me.StatusStripMain.PerformLayout()
        CType(Me.nvrPreserveInputTest, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents NullableTextValueInput As ActiveDevelop.EntitiesFormsLib.NullableTextValue
    Friend WithEvents NullableTextBoxCaption As System.Windows.Forms.Label
    Friend WithEvents NullableDateValueInput As ActiveDevelop.EntitiesFormsLib.NullableDateValue
    Friend WithEvents NullableIntValueInput As ActiveDevelop.EntitiesFormsLib.NullableIntValue
    Friend WithEvents NullableNumValueInput As ActiveDevelop.EntitiesFormsLib.NullableNumValue
    Friend WithEvents NullableMultilineTextValueInput As ActiveDevelop.EntitiesFormsLib.NullableMultilineTextValue
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents SetNullableRelationPopupPropertiesButton As System.Windows.Forms.Button
    Friend WithEvents SetNullableDateValuePropertiesButton As System.Windows.Forms.Button
    Friend WithEvents SetNullableNumValuePropertiesButton As System.Windows.Forms.Button
    Friend WithEvents SetNullableIntValuePropertiesButton As System.Windows.Forms.Button
    Friend WithEvents SetNullableMultilineTextValuePropertiesButton As System.Windows.Forms.Button
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents NullableValueRelationPopupInput As ActiveDevelop.EntitiesFormsLib.NullableValueRelationPopup
    Friend WithEvents SetNullableTextValuePropertiesButton As System.Windows.Forms.Button
    Friend WithEvents SubTest02Button As System.Windows.Forms.Button
    Friend WithEvents SubTest01Button As System.Windows.Forms.Button
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents ftbcmMain As ActiveDevelop.EntitiesFormsLib.FormToBusinessClassManager
    Friend WithEvents SingleResult4 As System.Windows.Forms.Label
    Friend WithEvents SingleResult3 As System.Windows.Forms.Label
    Friend WithEvents SingleResult2 As System.Windows.Forms.Label
    Friend WithEvents SingleResult1 As System.Windows.Forms.Label
    Friend WithEvents StatusStripMain As System.Windows.Forms.StatusStrip
    Friend WithEvents IsDirtyToolStripStatusLabel As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents ResetEventCounterToolStripDropDownButton As System.Windows.Forms.ToolStripDropDownButton
    Friend WithEvents ResultTextBox As System.Windows.Forms.TextBox
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents MainTab As System.Windows.Forms.TabControl
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
    Friend WithEvents MainPropertyGrid As System.Windows.Forms.PropertyGrid
    Friend WithEvents TestConfigurationPropertiesButton As System.Windows.Forms.Button
    Friend WithEvents ClearTestResultButton As System.Windows.Forms.Button
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents nvrPreserveInputTest As ActiveDevelop.EntitiesFormsLib.NullableValueRelationPopup

End Class
