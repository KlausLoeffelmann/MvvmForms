namespace MvvmFormsCSharpDemos
{
    partial class frmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.quitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.testsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rxCollectionViewDemoTestToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aSyncShowDialogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.nullableTextValue4 = new ActiveDevelop.EntitiesFormsLib.NullableTextValue();
            this.nullableTextValue3 = new ActiveDevelop.EntitiesFormsLib.NullableTextValue();
            this.nullableTextValue2 = new ActiveDevelop.EntitiesFormsLib.NullableTextValue();
            this.nullableTextValue1 = new ActiveDevelop.EntitiesFormsLib.NullableTextValue();
            this.panel2 = new System.Windows.Forms.Panel();
            this.nullableIntValue1 = new ActiveDevelop.EntitiesFormsLib.NullableIntValue();
            this.panel3 = new System.Windows.Forms.Panel();
            this.numValueField = new ActiveDevelop.EntitiesFormsLib.NullableNumValue();
            this.panel4 = new System.Windows.Forms.Panel();
            this.nullableValueRelationPopup = new ActiveDevelop.EntitiesFormsLib.NullableValueRelationPopup();
            this.panel5 = new System.Windows.Forms.Panel();
            this.nullableOptionButton1 = new ActiveDevelop.EntitiesFormsLib.NullableOptionButton();
            this.nullableDateValue1 = new ActiveDevelop.EntitiesFormsLib.NullableDateValue();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.nullableOptionButton2 = new ActiveDevelop.EntitiesFormsLib.NullableOptionButton();
            this.nullableOptionButton3 = new ActiveDevelop.EntitiesFormsLib.NullableOptionButton();
            this.nullableCheckBox1 = new ActiveDevelop.EntitiesFormsLib.NullableCheckBox();
            this.commandButton1 = new ActiveDevelop.EntitiesFormsLib.CommandButton();
            this.menuStrip1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nullableValueRelationPopup)).BeginInit();
            this.panel5.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.testsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(791, 28);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.quitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(44, 24);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // quitToolStripMenuItem
            // 
            this.quitToolStripMenuItem.Name = "quitToolStripMenuItem";
            this.quitToolStripMenuItem.Size = new System.Drawing.Size(112, 26);
            this.quitToolStripMenuItem.Text = "&Quit";
            this.quitToolStripMenuItem.Click += new System.EventHandler(this.quitToolStripMenuItem_Click);
            // 
            // testsToolStripMenuItem
            // 
            this.testsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.rxCollectionViewDemoTestToolStripMenuItem,
            this.aSyncShowDialogToolStripMenuItem});
            this.testsToolStripMenuItem.Name = "testsToolStripMenuItem";
            this.testsToolStripMenuItem.Size = new System.Drawing.Size(53, 24);
            this.testsToolStripMenuItem.Text = "Tests";
            // 
            // rxCollectionViewDemoTestToolStripMenuItem
            // 
            this.rxCollectionViewDemoTestToolStripMenuItem.Name = "rxCollectionViewDemoTestToolStripMenuItem";
            this.rxCollectionViewDemoTestToolStripMenuItem.Size = new System.Drawing.Size(276, 26);
            this.rxCollectionViewDemoTestToolStripMenuItem.Text = "RxCollectionView Demo/Test";
            this.rxCollectionViewDemoTestToolStripMenuItem.Click += new System.EventHandler(this.rxCollectionViewDemoTestToolStripMenuItem_Click);
            // 
            // aSyncShowDialogToolStripMenuItem
            // 
            this.aSyncShowDialogToolStripMenuItem.Name = "aSyncShowDialogToolStripMenuItem";
            this.aSyncShowDialogToolStripMenuItem.Size = new System.Drawing.Size(276, 26);
            this.aSyncShowDialogToolStripMenuItem.Text = "ASync ShowDialog";
            this.aSyncShowDialogToolStripMenuItem.Click += new System.EventHandler(this.aSyncShowDialogToolStripMenuItem_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel3, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.panel4, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.panel5, 1, 4);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(16, 44);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 144F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 148F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 182F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 106F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 14F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(750, 737);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(5, 297);
            this.label3.Margin = new System.Windows.Forms.Padding(5);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(364, 170);
            this.label3.TabIndex = 4;
            this.label3.Text = resources.GetString("label3.Text");
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 5);
            this.label2.Margin = new System.Windows.Forms.Padding(5);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(365, 119);
            this.label2.TabIndex = 0;
            this.label2.Text = resources.GetString("label2.Text");
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 149);
            this.label1.Margin = new System.Windows.Forms.Padding(5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(365, 136);
            this.label1.TabIndex = 2;
            this.label1.Text = resources.GetString("label1.Text");
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.nullableTextValue4);
            this.panel1.Controls.Add(this.nullableTextValue3);
            this.panel1.Controls.Add(this.nullableTextValue2);
            this.panel1.Controls.Add(this.nullableTextValue1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(378, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(369, 138);
            this.panel1.TabIndex = 1;
            // 
            // nullableTextValue4
            // 
            this.nullableTextValue4.AssignedManagerComponent = null;
            this.nullableTextValue4.Borderstyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.nullableTextValue4.ContentPresentPermission = ActiveDevelop.EntitiesFormsLib.ContentPresentPermissions.Normal;
            this.nullableTextValue4.ForeColor = System.Drawing.Color.Gold;
            this.nullableTextValue4.ImitateTabByPageKeys = true;
            this.nullableTextValue4.Location = new System.Drawing.Point(3, 87);
            this.nullableTextValue4.MaxLength = 32767;
            this.nullableTextValue4.Name = "nullableTextValue4";
            this.nullableTextValue4.NullValueString = "";
            this.nullableTextValue4.ObfuscationChar = null;
            this.nullableTextValue4.PermissionReason = null;
            this.nullableTextValue4.Size = new System.Drawing.Size(363, 22);
            this.nullableTextValue4.TabIndex = 3;
            this.nullableTextValue4.UIGuid = new System.Guid("b29b2dfa-5a4d-4ccd-8d2a-0921bfa76aa8");
            this.nullableTextValue4.Value = null;
            this.nullableTextValue4.ValueValidationState = null;
            // 
            // nullableTextValue3
            // 
            this.nullableTextValue3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.nullableTextValue3.AssignedManagerComponent = null;
            this.nullableTextValue3.Borderstyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.nullableTextValue3.ContentPresentPermission = ActiveDevelop.EntitiesFormsLib.ContentPresentPermissions.Normal;
            this.nullableTextValue3.ForeColor = System.Drawing.Color.DodgerBlue;
            this.nullableTextValue3.ImitateTabByPageKeys = true;
            this.nullableTextValue3.Location = new System.Drawing.Point(0, 58);
            this.nullableTextValue3.MaxLength = 32767;
            this.nullableTextValue3.Name = "nullableTextValue3";
            this.nullableTextValue3.NullValueColor = System.Drawing.SystemColors.ControlDark;
            this.nullableTextValue3.NullValueString = "Enter a Text!";
            this.nullableTextValue3.ObfuscationChar = null;
            this.nullableTextValue3.PermissionReason = null;
            this.nullableTextValue3.Size = new System.Drawing.Size(369, 22);
            this.nullableTextValue3.TabIndex = 2;
            this.nullableTextValue3.UIGuid = new System.Guid("69e30f88-8446-4b99-b9b6-a651cf8e10a5");
            this.nullableTextValue3.Value = null;
            this.nullableTextValue3.ValueValidationState = null;
            // 
            // nullableTextValue2
            // 
            this.nullableTextValue2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.nullableTextValue2.AssignedManagerComponent = null;
            this.nullableTextValue2.Borderstyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.nullableTextValue2.ContentPresentPermission = ActiveDevelop.EntitiesFormsLib.ContentPresentPermissions.Normal;
            this.nullableTextValue2.ForeColor = System.Drawing.Color.Peru;
            this.nullableTextValue2.ImitateTabByPageKeys = true;
            this.nullableTextValue2.Location = new System.Drawing.Point(0, 30);
            this.nullableTextValue2.MaxLength = 32767;
            this.nullableTextValue2.Name = "nullableTextValue2";
            this.nullableTextValue2.NullValueColor = System.Drawing.SystemColors.ControlDark;
            this.nullableTextValue2.NullValueString = "Read only Text.";
            this.nullableTextValue2.ObfuscationChar = null;
            this.nullableTextValue2.PermissionReason = null;
            this.nullableTextValue2.ReadOnly = true;
            this.nullableTextValue2.Size = new System.Drawing.Size(369, 22);
            this.nullableTextValue2.TabIndex = 1;
            this.nullableTextValue2.UIGuid = new System.Guid("69e30f88-8446-4b99-b9b6-a651cf8e10a5");
            this.nullableTextValue2.Value = null;
            this.nullableTextValue2.ValueValidationState = null;
            // 
            // nullableTextValue1
            // 
            this.nullableTextValue1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.nullableTextValue1.AssignedManagerComponent = null;
            this.nullableTextValue1.Borderstyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.nullableTextValue1.ContentPresentPermission = ActiveDevelop.EntitiesFormsLib.ContentPresentPermissions.Normal;
            this.nullableTextValue1.ImitateTabByPageKeys = true;
            this.nullableTextValue1.Location = new System.Drawing.Point(3, 4);
            this.nullableTextValue1.MaxLength = 32767;
            this.nullableTextValue1.Name = "nullableTextValue1";
            this.nullableTextValue1.NullValueColor = System.Drawing.SystemColors.ControlDark;
            this.nullableTextValue1.NullValueString = "Enter a Text!";
            this.nullableTextValue1.ObfuscationChar = null;
            this.nullableTextValue1.PermissionReason = null;
            this.nullableTextValue1.Size = new System.Drawing.Size(362, 22);
            this.nullableTextValue1.TabIndex = 0;
            this.nullableTextValue1.UIGuid = new System.Guid("69e30f88-8446-4b99-b9b6-a651cf8e10a5");
            this.nullableTextValue1.Value = null;
            this.nullableTextValue1.ValueValidationState = null;
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.Controls.Add(this.nullableIntValue1);
            this.panel2.Location = new System.Drawing.Point(378, 147);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(369, 142);
            this.panel2.TabIndex = 3;
            // 
            // nullableIntValue1
            // 
            this.nullableIntValue1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.nullableIntValue1.AssignedManagerComponent = null;
            this.nullableIntValue1.Borderstyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.nullableIntValue1.ContentPresentPermission = ActiveDevelop.EntitiesFormsLib.ContentPresentPermissions.Normal;
            this.nullableIntValue1.ImitateTabByPageKeys = true;
            this.nullableIntValue1.Location = new System.Drawing.Point(3, 14);
            this.nullableIntValue1.MaxLength = 32767;
            this.nullableIntValue1.MaxValue = null;
            this.nullableIntValue1.MinValue = 0;
            this.nullableIntValue1.Name = "nullableIntValue1";
            this.nullableIntValue1.NullValueColor = System.Drawing.SystemColors.ControlDark;
            this.nullableIntValue1.NullValueString = "Enter an Integer value.";
            this.nullableIntValue1.ObfuscationChar = null;
            this.nullableIntValue1.PermissionReason = null;
            this.nullableIntValue1.Size = new System.Drawing.Size(362, 22);
            this.nullableIntValue1.TabIndex = 0;
            this.nullableIntValue1.UIGuid = new System.Guid("b564b830-a531-45ff-9bcb-3a74bcbae0ea");
            this.nullableIntValue1.Value = null;
            this.nullableIntValue1.ValueValidationState = null;
            // 
            // panel3
            // 
            this.panel3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel3.Controls.Add(this.nullableCheckBox1);
            this.panel3.Controls.Add(this.numValueField);
            this.panel3.Location = new System.Drawing.Point(378, 295);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(369, 176);
            this.panel3.TabIndex = 5;
            // 
            // numValueField
            // 
            this.numValueField.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.numValueField.AssignedManagerComponent = null;
            this.numValueField.Borderstyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numValueField.ContentPresentPermission = ActiveDevelop.EntitiesFormsLib.ContentPresentPermissions.Normal;
            this.numValueField.CurrencySymbolString = "$ ";
            this.numValueField.CurrencySymbolUpFront = true;
            this.numValueField.DecimalPlaces = 2;
            this.numValueField.ImitateTabByPageKeys = true;
            this.numValueField.Location = new System.Drawing.Point(3, 0);
            this.numValueField.MaxLength = 32767;
            this.numValueField.MaxValue = null;
            this.numValueField.MinValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numValueField.Name = "numValueField";
            this.numValueField.NullValueColor = System.Drawing.SystemColors.ControlDark;
            this.numValueField.NullValueString = "Enter a Floating Point Value or a Formula like \"12*(3+3)\"";
            this.numValueField.ObfuscationChar = null;
            this.numValueField.PermissionReason = null;
            this.numValueField.Size = new System.Drawing.Size(362, 22);
            this.numValueField.TabIndex = 0;
            this.numValueField.UIGuid = new System.Guid("48a3250e-b5b5-4f1b-9ab7-50c5c3802e5b");
            this.numValueField.Value = null;
            this.numValueField.ValueValidationState = null;
            // 
            // panel4
            // 
            this.panel4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel4.Controls.Add(this.commandButton1);
            this.panel4.Controls.Add(this.nullableValueRelationPopup);
            this.panel4.Location = new System.Drawing.Point(378, 477);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(369, 100);
            this.panel4.TabIndex = 6;
            // 
            // nullableValueRelationPopup
            // 
            this.nullableValueRelationPopup.AssignedManagerComponent = null;
            this.nullableValueRelationPopup.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.None;
            this.nullableValueRelationPopup.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.None;
            this.nullableValueRelationPopup.Borderstyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.nullableValueRelationPopup.ContentPresentPermission = ActiveDevelop.EntitiesFormsLib.ContentPresentPermissions.Normal;
            this.nullableValueRelationPopup.DataSource = null;
            this.nullableValueRelationPopup.DisplayMember = null;
            this.nullableValueRelationPopup.HideButtons = false;
            this.nullableValueRelationPopup.ImitateTabByPageKeys = true;
            this.nullableValueRelationPopup.IsPopupAutoSize = false;
            this.nullableValueRelationPopup.IsPopupResizable = true;
            this.nullableValueRelationPopup.Location = new System.Drawing.Point(3, 33);
            this.nullableValueRelationPopup.MaxLength = 32767;
            this.nullableValueRelationPopup.MinimumPopupSize = new System.Drawing.Size(362, 80);
            this.nullableValueRelationPopup.MultiSelect = false;
            this.nullableValueRelationPopup.Name = "nullableValueRelationPopup";
            this.nullableValueRelationPopup.NullValueString = "* - - - *";
            this.nullableValueRelationPopup.PermissionReason = null;
            this.nullableValueRelationPopup.Searchable = true;
            this.nullableValueRelationPopup.SearchColumnBackgroundColor = System.Drawing.Color.Empty;
            this.nullableValueRelationPopup.SearchColumnHeaderBackgroundColor = System.Drawing.Color.Empty;
            this.nullableValueRelationPopup.Size = new System.Drawing.Size(362, 22);
            this.nullableValueRelationPopup.TabIndex = 0;
            this.nullableValueRelationPopup.UIGuid = new System.Guid("669d0c6b-b374-4018-8dcf-9339438b7561");
            this.nullableValueRelationPopup.ValueMember = null;
            this.nullableValueRelationPopup.GetColumnSchema += new ActiveDevelop.EntitiesFormsLib.NullableValueRelationPopup.GetColumnSchemaEventHandler(this.nullableValueRelationPopup1_GetColumnSchema);
            // 
            // panel5
            // 
            this.panel5.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel5.Controls.Add(this.nullableOptionButton3);
            this.panel5.Controls.Add(this.nullableOptionButton2);
            this.panel5.Controls.Add(this.nullableOptionButton1);
            this.panel5.Controls.Add(this.nullableDateValue1);
            this.panel5.Location = new System.Drawing.Point(378, 583);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(369, 151);
            this.panel5.TabIndex = 7;
            // 
            // nullableOptionButton1
            // 
            this.nullableOptionButton1.AssignedManagerComponent = null;
            this.nullableOptionButton1.AutoSize = true;
            this.nullableOptionButton1.ContentPresentPermission = ActiveDevelop.EntitiesFormsLib.ContentPresentPermissions.Normal;
            this.nullableOptionButton1.ExecuteCallback = null;
            this.nullableOptionButton1.ImitateTabByPageKeys = true;
            this.nullableOptionButton1.Location = new System.Drawing.Point(6, 49);
            this.nullableOptionButton1.Name = "nullableOptionButton1";
            this.nullableOptionButton1.PermissionReason = null;
            this.nullableOptionButton1.Size = new System.Drawing.Size(148, 21);
            this.nullableOptionButton1.TabIndex = 1;
            this.nullableOptionButton1.Text = "nullableCheckBox1";
            this.nullableOptionButton1.UIGuid = new System.Guid("960e9e56-2f12-4838-8dfd-4705d38d0451");
            this.nullableOptionButton1.UseVisualStyleBackColor = true;
            this.nullableOptionButton1.Value = false;
            this.nullableOptionButton1.ValueChanged += new ActiveDevelop.EntitiesFormsLib.INullableValueDataBinding.ValueChangedEventHandler(this.nullableOptionButton1_ValueChanged);
            // 
            // nullableDateValue1
            // 
            this.nullableDateValue1.AssignedManagerComponent = null;
            this.nullableDateValue1.Borderstyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.nullableDateValue1.ContentPresentPermission = ActiveDevelop.EntitiesFormsLib.ContentPresentPermissions.Normal;
            this.nullableDateValue1.DaysDistanceBetweenLinkedControl = null;
            this.nullableDateValue1.DisplayFormatString = "MM/dd/yyyy";
            this.nullableDateValue1.ImitateTabByPageKeys = true;
            this.nullableDateValue1.LinkedToNullableDateControl = null;
            this.nullableDateValue1.Location = new System.Drawing.Point(6, 3);
            this.nullableDateValue1.MaxLength = 32767;
            this.nullableDateValue1.Name = "nullableDateValue1";
            this.nullableDateValue1.NullValueString = "* - - - *";
            this.nullableDateValue1.ObfuscationChar = null;
            this.nullableDateValue1.PermissionReason = null;
            this.nullableDateValue1.Size = new System.Drawing.Size(359, 22);
            this.nullableDateValue1.TabIndex = 0;
            this.nullableDateValue1.UIGuid = new System.Guid("9c8ca551-6d86-4219-8ff7-66eb809e0c5e");
            this.nullableDateValue1.Value = null;
            this.nullableDateValue1.ValueValidationState = null;
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 857);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(791, 25);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(27, 20);
            this.toolStripStatusLabel1.Text = "---";
            // 
            // nullableOptionButton2
            // 
            this.nullableOptionButton2.AssignedManagerComponent = null;
            this.nullableOptionButton2.AutoSize = true;
            this.nullableOptionButton2.ContentPresentPermission = ActiveDevelop.EntitiesFormsLib.ContentPresentPermissions.Normal;
            this.nullableOptionButton2.ExecuteCallback = null;
            this.nullableOptionButton2.ImitateTabByPageKeys = true;
            this.nullableOptionButton2.Location = new System.Drawing.Point(6, 92);
            this.nullableOptionButton2.Name = "nullableOptionButton2";
            this.nullableOptionButton2.PermissionReason = null;
            this.nullableOptionButton2.Size = new System.Drawing.Size(148, 21);
            this.nullableOptionButton2.TabIndex = 2;
            this.nullableOptionButton2.Text = "nullableCheckBox1";
            this.nullableOptionButton2.UIGuid = new System.Guid("960e9e56-2f12-4838-8dfd-4705d38d0451");
            this.nullableOptionButton2.UseVisualStyleBackColor = true;
            this.nullableOptionButton2.Value = false;
            // 
            // nullableOptionButton3
            // 
            this.nullableOptionButton3.AssignedManagerComponent = null;
            this.nullableOptionButton3.AutoSize = true;
            this.nullableOptionButton3.ContentPresentPermission = ActiveDevelop.EntitiesFormsLib.ContentPresentPermissions.Normal;
            this.nullableOptionButton3.ExecuteCallback = null;
            this.nullableOptionButton3.ImitateTabByPageKeys = true;
            this.nullableOptionButton3.Location = new System.Drawing.Point(6, 71);
            this.nullableOptionButton3.Name = "nullableOptionButton3";
            this.nullableOptionButton3.PermissionReason = null;
            this.nullableOptionButton3.Size = new System.Drawing.Size(148, 21);
            this.nullableOptionButton3.TabIndex = 3;
            this.nullableOptionButton3.Text = "nullableCheckBox1";
            this.nullableOptionButton3.UIGuid = new System.Guid("960e9e56-2f12-4838-8dfd-4705d38d0451");
            this.nullableOptionButton3.UseVisualStyleBackColor = true;
            this.nullableOptionButton3.Value = false;
            // 
            // nullableCheckBox1
            // 
            this.nullableCheckBox1.AssignedManagerComponent = null;
            this.nullableCheckBox1.AutoSize = true;
            this.nullableCheckBox1.ContentPresentPermission = ActiveDevelop.EntitiesFormsLib.ContentPresentPermissions.Normal;
            this.nullableCheckBox1.ExecuteCallback = null;
            this.nullableCheckBox1.ImitateTabByPageKeys = true;
            this.nullableCheckBox1.Location = new System.Drawing.Point(3, 28);
            this.nullableCheckBox1.Name = "nullableCheckBox1";
            this.nullableCheckBox1.PermissionReason = null;
            this.nullableCheckBox1.Size = new System.Drawing.Size(149, 21);
            this.nullableCheckBox1.TabIndex = 1;
            this.nullableCheckBox1.Text = "nullableCheckBox1";
            this.nullableCheckBox1.UIGuid = new System.Guid("a91bb58d-b56c-4ce5-bf7c-24e04413a791");
            this.nullableCheckBox1.UseVisualStyleBackColor = true;
            this.nullableCheckBox1.ValueChanged += new ActiveDevelop.EntitiesFormsLib.INullableValueDataBinding.ValueChangedEventHandler(this.nullableCheckBox1_ValueChanged);
            // 
            // commandButton1
            // 
            this.commandButton1.Command = null;
            this.commandButton1.CommandParameter = null;
            this.commandButton1.ImitateTabByPageKeys = true;
            this.commandButton1.Location = new System.Drawing.Point(101, 61);
            this.commandButton1.Name = "commandButton1";
            this.commandButton1.Size = new System.Drawing.Size(153, 34);
            this.commandButton1.TabIndex = 1;
            this.commandButton1.Text = "Async Show Dialog";
            this.commandButton1.UseVisualStyleBackColor = true;
            this.commandButton1.Click += new System.EventHandler(this.aSyncShowDialogToolStripMenuItem_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(791, 882);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CSharp MvvmForms Demo";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nullableValueRelationPopup)).EndInit();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem quitToolStripMenuItem;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel1;
        private ActiveDevelop.EntitiesFormsLib.NullableTextValue nullableTextValue3;
        private ActiveDevelop.EntitiesFormsLib.NullableTextValue nullableTextValue2;
        private ActiveDevelop.EntitiesFormsLib.NullableTextValue nullableTextValue1;
        private ActiveDevelop.EntitiesFormsLib.NullableTextValue nullableTextValue4;
        private System.Windows.Forms.ToolStripMenuItem testsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rxCollectionViewDemoTestToolStripMenuItem;
        private System.Windows.Forms.Panel panel2;
        private ActiveDevelop.EntitiesFormsLib.NullableIntValue nullableIntValue1;
        private System.Windows.Forms.Panel panel3;
        private ActiveDevelop.EntitiesFormsLib.NullableNumValue numValueField;
        private System.Windows.Forms.Panel panel4;
        private ActiveDevelop.EntitiesFormsLib.NullableValueRelationPopup nullableValueRelationPopup;
        private System.Windows.Forms.Panel panel5;
        private ActiveDevelop.EntitiesFormsLib.NullableDateValue nullableDateValue1;
        private System.Windows.Forms.ToolStripMenuItem aSyncShowDialogToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private ActiveDevelop.EntitiesFormsLib.NullableOptionButton nullableOptionButton1;
        private ActiveDevelop.EntitiesFormsLib.NullableOptionButton nullableOptionButton3;
        private ActiveDevelop.EntitiesFormsLib.NullableOptionButton nullableOptionButton2;
        private ActiveDevelop.EntitiesFormsLib.NullableCheckBox nullableCheckBox1;
        private ActiveDevelop.EntitiesFormsLib.CommandButton commandButton1;
    }
}

