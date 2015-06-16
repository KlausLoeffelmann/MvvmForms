namespace MvvmCalculator
{
    partial class MainView
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
            this.components = new System.ComponentModel.Container();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.historyListBox = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.resultLabel = new System.Windows.Forms.Label();
            this.formularLabel = new System.Windows.Forms.Label();
            this.calcCommandButton = new ActiveDevelop.EntitiesFormsLib.CommandButton();
            this.mvvmManager1 = new ActiveDevelop.EntitiesFormsLib.MvvmManager(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.mvvmManager1)).BeginInit();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.mvvmManager1.SetEventBindings(this.label2, null);
            this.label2.Location = new System.Drawing.Point(12, 203);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(153, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Formular (e.g. 123.23+5^sin(2))";
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mvvmManager1.SetEventBindings(this.textBox1, null);
            this.textBox1.Location = new System.Drawing.Point(12, 219);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(439, 68);
            this.textBox1.TabIndex = 1;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // historyListBox
            // 
            this.historyListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mvvmManager1.SetEventBindings(this.historyListBox, null);
            this.historyListBox.FormattingEnabled = true;
            this.historyListBox.Location = new System.Drawing.Point(15, 24);
            this.historyListBox.Name = "historyListBox";
            this.historyListBox.Size = new System.Drawing.Size(436, 95);
            this.historyListBox.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.mvvmManager1.SetEventBindings(this.label1, null);
            this.label1.Location = new System.Drawing.Point(12, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "History:";
            // 
            // resultLabel
            // 
            this.resultLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.resultLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mvvmManager1.SetEventBindings(this.resultLabel, null);
            this.resultLabel.Font = new System.Drawing.Font("Consolas", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.resultLabel.Location = new System.Drawing.Point(292, 152);
            this.resultLabel.Name = "resultLabel";
            this.resultLabel.Size = new System.Drawing.Size(159, 37);
            this.resultLabel.TabIndex = 5;
            this.resultLabel.Text = "resultLabel";
            this.resultLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // formularLabel
            // 
            this.formularLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mvvmManager1.SetEventBindings(this.formularLabel, null);
            this.formularLabel.Location = new System.Drawing.Point(15, 152);
            this.formularLabel.Name = "formularLabel";
            this.formularLabel.Size = new System.Drawing.Size(262, 36);
            this.formularLabel.TabIndex = 6;
            this.formularLabel.Text = "formularLabel";
            // 
            // calcCommandButton
            // 
            this.calcCommandButton.Command = null;
            this.calcCommandButton.CommandParameter = null;
            this.mvvmManager1.SetEventBindings(this.calcCommandButton, null);
            this.calcCommandButton.Location = new System.Drawing.Point(352, 315);
            this.calcCommandButton.Name = "calcCommandButton";
            this.calcCommandButton.Size = new System.Drawing.Size(99, 37);
            this.calcCommandButton.TabIndex = 7;
            this.calcCommandButton.Text = "Calc";
            this.calcCommandButton.UseVisualStyleBackColor = true;
            // 
            // mvvmManager1
            // 
            this.mvvmManager1.CancelButton = null;
            this.mvvmManager1.ContainerControl = this;
            this.mvvmManager1.CurrentContextGuid = new System.Guid("861fafc2-3724-48ce-9bcf-d4a6f0dc5f0b");
            this.mvvmManager1.DataContext = null;
            this.mvvmManager1.DataContextType = typeof(MvvmCalculatorVMLib.MainViewModel);
            this.mvvmManager1.DataSourceType = typeof(MvvmCalculatorVMLib.MainViewModel);
            this.mvvmManager1.DirtyStateManagerComponent = null;
            this.mvvmManager1.DynamicEventHandlingList = null;
            this.mvvmManager1.HostingForm = this;
            this.mvvmManager1.HostingUserControl = null;
            this.mvvmManager1.MvvmBindings.AddPropertyBinding(this.textBox1, new ActiveDevelop.EntitiesFormsLib.BindingSetting(ActiveDevelop.EntitiesFormsLib.MvvmBindingModes.TwoWay, ActiveDevelop.EntitiesFormsLib.UpdateSourceTriggerSettings.LostFocus), new ActiveDevelop.EntitiesFormsLib.BindingProperty("Text", typeof(string)), null, null, new ActiveDevelop.EntitiesFormsLib.BindingProperty("EnteredFormula", typeof(string)));
            this.mvvmManager1.MvvmBindings.AddPropertyBinding(this.historyListBox, new ActiveDevelop.EntitiesFormsLib.BindingSetting(ActiveDevelop.EntitiesFormsLib.MvvmBindingModes.TwoWay, ActiveDevelop.EntitiesFormsLib.UpdateSourceTriggerSettings.PropertyChangedImmediately), new ActiveDevelop.EntitiesFormsLib.BindingProperty("DataSource", typeof(object)), typeof(ActiveDevelop.EntitiesFormsLib.ObservableCollectionToBindingListConverter), null, new ActiveDevelop.EntitiesFormsLib.BindingProperty("Formulas", typeof(System.Collections.ObjectModel.ObservableCollection<ActiveDevelop.MvvmBaseLib.FormulaEvaluator.FormulaEvaluator>)));
            this.mvvmManager1.MvvmBindings.AddPropertyBinding(this.resultLabel, new ActiveDevelop.EntitiesFormsLib.BindingSetting(ActiveDevelop.EntitiesFormsLib.MvvmBindingModes.OneWay, ActiveDevelop.EntitiesFormsLib.UpdateSourceTriggerSettings.PropertyChangedImmediately), new ActiveDevelop.EntitiesFormsLib.BindingProperty("Text", typeof(string)), null, null, new ActiveDevelop.EntitiesFormsLib.BindingProperty("Result", typeof(string)));
            this.mvvmManager1.MvvmBindings.AddPropertyBinding(this.formularLabel, new ActiveDevelop.EntitiesFormsLib.BindingSetting(ActiveDevelop.EntitiesFormsLib.MvvmBindingModes.OneWay, ActiveDevelop.EntitiesFormsLib.UpdateSourceTriggerSettings.PropertyChangedImmediately), new ActiveDevelop.EntitiesFormsLib.BindingProperty("Text", typeof(string)), null, null, new ActiveDevelop.EntitiesFormsLib.BindingProperty("CurrentFormular", typeof(string)));
            this.mvvmManager1.MvvmBindings.AddPropertyBinding(this.calcCommandButton, new ActiveDevelop.EntitiesFormsLib.BindingSetting(ActiveDevelop.EntitiesFormsLib.MvvmBindingModes.OneWay, ActiveDevelop.EntitiesFormsLib.UpdateSourceTriggerSettings.PropertyChangedImmediately), new ActiveDevelop.EntitiesFormsLib.BindingProperty("Command", typeof(System.Windows.Input.ICommand)), null, null, new ActiveDevelop.EntitiesFormsLib.BindingProperty("CalcCommand", typeof(ActiveDevelop.MvvmBaseLib.Mvvm.RelayCommand)));
            // 
            // MainView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(463, 364);
            this.Controls.Add(this.calcCommandButton);
            this.Controls.Add(this.formularLabel);
            this.Controls.Add(this.resultLabel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.historyListBox);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label2);
            this.mvvmManager1.SetEventBindings(this, null);
            this.Name = "MainView";
            this.Text = "MvvmCalc";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.mvvmManager1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.ListBox historyListBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label resultLabel;
        private System.Windows.Forms.Label formularLabel;
        private ActiveDevelop.EntitiesFormsLib.MvvmManager mvvmManager1;
        private ActiveDevelop.EntitiesFormsLib.CommandButton calcCommandButton;
    }
}

