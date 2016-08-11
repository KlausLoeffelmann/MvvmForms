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
            this.nullableIntValue1 = new ActiveDevelop.EntitiesFormsLib.NullableIntValue();
            this.label1 = new System.Windows.Forms.Label();
            this.nullableNumValue1 = new ActiveDevelop.EntitiesFormsLib.NullableNumValue();
            this.panel1 = new System.Windows.Forms.Panel();
            this.nullableTextValue4 = new ActiveDevelop.EntitiesFormsLib.NullableTextValue();
            this.nullableTextValue3 = new ActiveDevelop.EntitiesFormsLib.NullableTextValue();
            this.nullableTextValue2 = new ActiveDevelop.EntitiesFormsLib.NullableTextValue();
            this.nullableTextValue1 = new ActiveDevelop.EntitiesFormsLib.NullableTextValue();
            this.menuStrip1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
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
            this.rxCollectionViewDemoTestToolStripMenuItem});
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
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.nullableIntValue1, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.nullableNumValue1, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 1, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(16, 44);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 144F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 148F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(750, 501);
            this.tableLayoutPanel1.TabIndex = 0;
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
            // nullableIntValue1
            // 
            this.nullableIntValue1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.nullableIntValue1.AssignedManagerComponent = null;
            this.nullableIntValue1.Borderstyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.nullableIntValue1.ContentPresentPermission = ActiveDevelop.EntitiesFormsLib.ContentPresentPermissions.Normal;
            this.nullableIntValue1.Location = new System.Drawing.Point(378, 207);
            this.nullableIntValue1.MaxLength = 32767;
            this.nullableIntValue1.MaxValue = null;
            this.nullableIntValue1.MinValue = 0;
            this.nullableIntValue1.Name = "nullableIntValue1";
            this.nullableIntValue1.NullValueColor = System.Drawing.SystemColors.ControlDark;
            this.nullableIntValue1.NullValueString = "Enter an Integer value.";
            this.nullableIntValue1.ObfuscationChar = null;
            this.nullableIntValue1.PermissionReason = null;
            this.nullableIntValue1.Size = new System.Drawing.Size(369, 22);
            this.nullableIntValue1.TabIndex = 3;
            this.nullableIntValue1.UIGuid = new System.Guid("b564b830-a531-45ff-9bcb-3a74bcbae0ea");
            this.nullableIntValue1.Value = null;
            this.nullableIntValue1.ValueValidationState = null;
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
            // nullableNumValue1
            // 
            this.nullableNumValue1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.nullableNumValue1.AssignedManagerComponent = null;
            this.nullableNumValue1.Borderstyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.nullableNumValue1.ContentPresentPermission = ActiveDevelop.EntitiesFormsLib.ContentPresentPermissions.Normal;
            this.nullableNumValue1.CurrencySymbolString = "$ ";
            this.nullableNumValue1.CurrencySymbolUpFront = true;
            this.nullableNumValue1.DecimalPlaces = 2;
            this.nullableNumValue1.Location = new System.Drawing.Point(378, 385);
            this.nullableNumValue1.MaxLength = 32767;
            this.nullableNumValue1.MaxValue = null;
            this.nullableNumValue1.MinValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.nullableNumValue1.Name = "nullableNumValue1";
            this.nullableNumValue1.NullValueColor = System.Drawing.SystemColors.ControlDark;
            this.nullableNumValue1.NullValueString = "Enter a Floating Point Value or a Formula like \"12*(3+3)\"";
            this.nullableNumValue1.ObfuscationChar = null;
            this.nullableNumValue1.PermissionReason = null;
            this.nullableNumValue1.Size = new System.Drawing.Size(369, 22);
            this.nullableNumValue1.TabIndex = 5;
            this.nullableNumValue1.UIGuid = new System.Guid("48a3250e-b5b5-4f1b-9ab7-50c5c3802e5b");
            this.nullableNumValue1.Value = null;
            this.nullableNumValue1.ValueValidationState = null;
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
            this.nullableTextValue3.Location = new System.Drawing.Point(0, 59);
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
            this.nullableTextValue2.Location = new System.Drawing.Point(0, 31);
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
            this.nullableTextValue1.Location = new System.Drawing.Point(0, 3);
            this.nullableTextValue1.MaxLength = 32767;
            this.nullableTextValue1.Name = "nullableTextValue1";
            this.nullableTextValue1.NullValueColor = System.Drawing.SystemColors.ControlDark;
            this.nullableTextValue1.NullValueString = "Enter a Text!";
            this.nullableTextValue1.ObfuscationChar = null;
            this.nullableTextValue1.PermissionReason = null;
            this.nullableTextValue1.Size = new System.Drawing.Size(369, 22);
            this.nullableTextValue1.TabIndex = 0;
            this.nullableTextValue1.UIGuid = new System.Guid("69e30f88-8446-4b99-b9b6-a651cf8e10a5");
            this.nullableTextValue1.Value = null;
            this.nullableTextValue1.ValueValidationState = null;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(791, 557);
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
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem quitToolStripMenuItem;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private ActiveDevelop.EntitiesFormsLib.NullableIntValue nullableIntValue1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private ActiveDevelop.EntitiesFormsLib.NullableNumValue nullableNumValue1;
        private System.Windows.Forms.Panel panel1;
        private ActiveDevelop.EntitiesFormsLib.NullableTextValue nullableTextValue3;
        private ActiveDevelop.EntitiesFormsLib.NullableTextValue nullableTextValue2;
        private ActiveDevelop.EntitiesFormsLib.NullableTextValue nullableTextValue1;
        private ActiveDevelop.EntitiesFormsLib.NullableTextValue nullableTextValue4;
        private System.Windows.Forms.ToolStripMenuItem testsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rxCollectionViewDemoTestToolStripMenuItem;
    }
}

