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
            this.AllowFormulaCheckBox = new System.Windows.Forms.CheckBox();
            this.numValueField = new ActiveDevelop.EntitiesFormsLib.NullableNumValue();
            this.menuStrip1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
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
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(9, 3, 0, 3);
            this.menuStrip1.Size = new System.Drawing.Size(1186, 42);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.quitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(64, 36);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // quitToolStripMenuItem
            // 
            this.quitToolStripMenuItem.Name = "quitToolStripMenuItem";
            this.quitToolStripMenuItem.Size = new System.Drawing.Size(161, 38);
            this.quitToolStripMenuItem.Text = "&Quit";
            this.quitToolStripMenuItem.Click += new System.EventHandler(this.quitToolStripMenuItem_Click);
            // 
            // testsToolStripMenuItem
            // 
            this.testsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.rxCollectionViewDemoTestToolStripMenuItem});
            this.testsToolStripMenuItem.Name = "testsToolStripMenuItem";
            this.testsToolStripMenuItem.Size = new System.Drawing.Size(79, 36);
            this.testsToolStripMenuItem.Text = "Tests";
            // 
            // rxCollectionViewDemoTestToolStripMenuItem
            // 
            this.rxCollectionViewDemoTestToolStripMenuItem.Name = "rxCollectionViewDemoTestToolStripMenuItem";
            this.rxCollectionViewDemoTestToolStripMenuItem.Size = new System.Drawing.Size(421, 38);
            this.rxCollectionViewDemoTestToolStripMenuItem.Text = "RxCollectionView Demo/Test";
            this.rxCollectionViewDemoTestToolStripMenuItem.Click += new System.EventHandler(this.rxCollectionViewDemoTestToolStripMenuItem_Click);
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
            this.tableLayoutPanel1.Location = new System.Drawing.Point(24, 69);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 225F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 231F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1125, 783);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 464);
            this.label3.Margin = new System.Windows.Forms.Padding(8);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(545, 250);
            this.label3.TabIndex = 4;
            this.label3.Text = resources.GetString("label3.Text");
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 8);
            this.label2.Margin = new System.Windows.Forms.Padding(8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(542, 175);
            this.label2.TabIndex = 0;
            this.label2.Text = resources.GetString("label2.Text");
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 233);
            this.label1.Margin = new System.Windows.Forms.Padding(8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(541, 200);
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
            this.panel1.Location = new System.Drawing.Point(566, 5);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(555, 215);
            this.panel1.TabIndex = 1;
            // 
            // nullableTextValue4
            // 
            this.nullableTextValue4.AssignedManagerComponent = null;
            this.nullableTextValue4.Borderstyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.nullableTextValue4.ContentPresentPermission = ActiveDevelop.EntitiesFormsLib.ContentPresentPermissions.Normal;
            this.nullableTextValue4.ForeColor = System.Drawing.Color.Gold;
            this.nullableTextValue4.Location = new System.Drawing.Point(4, 136);
            this.nullableTextValue4.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.nullableTextValue4.MaxLength = 32767;
            this.nullableTextValue4.Name = "nullableTextValue4";
            this.nullableTextValue4.NullValueString = "";
            this.nullableTextValue4.ObfuscationChar = null;
            this.nullableTextValue4.PermissionReason = null;
            this.nullableTextValue4.ImitateTabByPageKeys = false;
            this.nullableTextValue4.Size = new System.Drawing.Size(544, 31);
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
            this.nullableTextValue3.Location = new System.Drawing.Point(0, 91);
            this.nullableTextValue3.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.nullableTextValue3.MaxLength = 32767;
            this.nullableTextValue3.Name = "nullableTextValue3";
            this.nullableTextValue3.NullValueColor = System.Drawing.SystemColors.ControlDark;
            this.nullableTextValue3.NullValueString = "Enter a Text!";
            this.nullableTextValue3.ObfuscationChar = null;
            this.nullableTextValue3.PermissionReason = null;
            this.nullableTextValue3.ImitateTabByPageKeys = false;
            this.nullableTextValue3.Size = new System.Drawing.Size(555, 31);
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
            this.nullableTextValue2.Location = new System.Drawing.Point(0, 47);
            this.nullableTextValue2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.nullableTextValue2.MaxLength = 32767;
            this.nullableTextValue2.Name = "nullableTextValue2";
            this.nullableTextValue2.NullValueColor = System.Drawing.SystemColors.ControlDark;
            this.nullableTextValue2.NullValueString = "Read only Text.";
            this.nullableTextValue2.ObfuscationChar = null;
            this.nullableTextValue2.PermissionReason = null;
            this.nullableTextValue2.ReadOnly = true;
            this.nullableTextValue2.ImitateTabByPageKeys = false;
            this.nullableTextValue2.Size = new System.Drawing.Size(555, 31);
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
            this.nullableTextValue1.Location = new System.Drawing.Point(4, 6);
            this.nullableTextValue1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.nullableTextValue1.MaxLength = 32767;
            this.nullableTextValue1.Name = "nullableTextValue1";
            this.nullableTextValue1.NullValueColor = System.Drawing.SystemColors.ControlDark;
            this.nullableTextValue1.NullValueString = "Enter a Text!";
            this.nullableTextValue1.ObfuscationChar = null;
            this.nullableTextValue1.PermissionReason = null;
            this.nullableTextValue1.ImitateTabByPageKeys = false;
            this.nullableTextValue1.Size = new System.Drawing.Size(544, 31);
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
            this.panel2.Location = new System.Drawing.Point(566, 230);
            this.panel2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(555, 221);
            this.panel2.TabIndex = 6;
            // 
            // nullableIntValue1
            // 
            this.nullableIntValue1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.nullableIntValue1.AssignedManagerComponent = null;
            this.nullableIntValue1.Borderstyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.nullableIntValue1.ContentPresentPermission = ActiveDevelop.EntitiesFormsLib.ContentPresentPermissions.Normal;
            this.nullableIntValue1.Location = new System.Drawing.Point(4, 21);
            this.nullableIntValue1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.nullableIntValue1.MaxLength = 32767;
            this.nullableIntValue1.MaxValue = null;
            this.nullableIntValue1.MinValue = 0;
            this.nullableIntValue1.Name = "nullableIntValue1";
            this.nullableIntValue1.NullValueColor = System.Drawing.SystemColors.ControlDark;
            this.nullableIntValue1.NullValueString = "Enter an Integer value.";
            this.nullableIntValue1.ObfuscationChar = null;
            this.nullableIntValue1.PermissionReason = null;
            this.nullableIntValue1.ImitateTabByPageKeys = false;
            this.nullableIntValue1.Size = new System.Drawing.Size(531, 31);
            this.nullableIntValue1.TabIndex = 4;
            this.nullableIntValue1.UIGuid = new System.Guid("b564b830-a531-45ff-9bcb-3a74bcbae0ea");
            this.nullableIntValue1.Value = null;
            this.nullableIntValue1.ValueValidationState = null;
            // 
            // panel3
            // 
            this.panel3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel3.Controls.Add(this.AllowFormulaCheckBox);
            this.panel3.Controls.Add(this.numValueField);
            this.panel3.Location = new System.Drawing.Point(566, 461);
            this.panel3.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(555, 317);
            this.panel3.TabIndex = 7;
            // 
            // AllowFormulaCheckBox
            // 
            this.AllowFormulaCheckBox.AutoSize = true;
            this.AllowFormulaCheckBox.Location = new System.Drawing.Point(4, 64);
            this.AllowFormulaCheckBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.AllowFormulaCheckBox.Name = "AllowFormulaCheckBox";
            this.AllowFormulaCheckBox.Size = new System.Drawing.Size(179, 29);
            this.AllowFormulaCheckBox.TabIndex = 7;
            this.AllowFormulaCheckBox.Text = "Allow Formula";
            this.AllowFormulaCheckBox.UseVisualStyleBackColor = true;
            this.AllowFormulaCheckBox.CheckedChanged += new System.EventHandler(this.AllowFormulaCheckBox_CheckedChanged);
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
            this.numValueField.Location = new System.Drawing.Point(4, 20);
            this.numValueField.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
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
            this.numValueField.ImitateTabByPageKeys = false;
            this.numValueField.Size = new System.Drawing.Size(509, 31);
            this.numValueField.TabIndex = 6;
            this.numValueField.UIGuid = new System.Guid("48a3250e-b5b5-4f1b-9ab7-50c5c3802e5b");
            this.numValueField.Value = null;
            this.numValueField.ValueValidationState = null;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1186, 870);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
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
        private System.Windows.Forms.CheckBox AllowFormulaCheckBox;
        private ActiveDevelop.EntitiesFormsLib.NullableNumValue numValueField;
    }
}

