namespace MvvmCalculator
{
    partial class FunctionPlotterView
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
            this.updateCommandButton = new ActiveDevelop.EntitiesFormsLib.CommandButton();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.functionTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.fromLabel = new System.Windows.Forms.Label();
            this.fromTextBox = new System.Windows.Forms.TextBox();
            this.toTextBox = new System.Windows.Forms.TextBox();
            this.mvvmManager1 = new ActiveDevelop.EntitiesFormsLib.MvvmManager(this.components);
            this.functionPlotterRenderer1 = new MvvmCalculator.FunctionPlotter.FunctionPlotterRenderer();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mvvmManager1)).BeginInit();
            this.SuspendLayout();
            // 
            // updateCommandButton
            // 
            this.updateCommandButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.updateCommandButton.Command = null;
            this.updateCommandButton.CommandParameter = null;
            this.mvvmManager1.SetEventBindings(this.updateCommandButton, null);
            this.updateCommandButton.Location = new System.Drawing.Point(773, 792);
            this.updateCommandButton.Name = "updateCommandButton";
            this.updateCommandButton.Size = new System.Drawing.Size(196, 69);
            this.updateCommandButton.TabIndex = 1;
            this.updateCommandButton.Text = "Render!";
            this.updateCommandButton.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.Controls.Add(this.functionTextBox, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label1, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.fromLabel, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.fromTextBox, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.toTextBox, 3, 0);
            this.mvvmManager1.SetEventBindings(this.tableLayoutPanel1, null);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(26, 705);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(722, 155);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // functionTextBox
            // 
            this.functionTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.functionTextBox, 3);
            this.mvvmManager1.SetEventBindings(this.functionTextBox, null);
            this.functionTextBox.Location = new System.Drawing.Point(190, 103);
            this.functionTextBox.Margin = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.functionTextBox.Name = "functionTextBox";
            this.functionTextBox.Size = new System.Drawing.Size(522, 26);
            this.functionTextBox.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label2.AutoSize = true;
            this.mvvmManager1.SetEventBindings(this.label2, null);
            this.label2.Location = new System.Drawing.Point(102, 106);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 20);
            this.label2.TabIndex = 4;
            this.label2.Text = "Function:";
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label1.AutoSize = true;
            this.mvvmManager1.SetEventBindings(this.label1, null);
            this.label1.Location = new System.Drawing.Point(510, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(27, 20);
            this.label1.TabIndex = 3;
            this.label1.Text = "to:";
            // 
            // fromLabel
            // 
            this.fromLabel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.fromLabel.AutoSize = true;
            this.mvvmManager1.SetEventBindings(this.fromLabel, null);
            this.fromLabel.Location = new System.Drawing.Point(132, 28);
            this.fromLabel.Name = "fromLabel";
            this.fromLabel.Size = new System.Drawing.Size(45, 20);
            this.fromLabel.TabIndex = 0;
            this.fromLabel.Text = "from:";
            // 
            // fromTextBox
            // 
            this.fromTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.mvvmManager1.SetEventBindings(this.fromTextBox, null);
            this.fromTextBox.Location = new System.Drawing.Point(190, 25);
            this.fromTextBox.Margin = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.fromTextBox.Name = "fromTextBox";
            this.fromTextBox.Size = new System.Drawing.Size(160, 26);
            this.fromTextBox.TabIndex = 1;
            // 
            // toTextBox
            // 
            this.toTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.mvvmManager1.SetEventBindings(this.toTextBox, null);
            this.toTextBox.Location = new System.Drawing.Point(550, 25);
            this.toTextBox.Margin = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.toTextBox.Name = "toTextBox";
            this.toTextBox.Size = new System.Drawing.Size(162, 26);
            this.toTextBox.TabIndex = 2;
            // 
            // mvvmManager1
            // 
            this.mvvmManager1.CancelButton = null;
            this.mvvmManager1.ContainerControl = this;
            this.mvvmManager1.CurrentContextGuid = new System.Guid("861fafc2-3724-48ce-9bcf-d4a6f0dc5f0b");
            this.mvvmManager1.DataContext = null;
            this.mvvmManager1.DataContextType = typeof(MvvmCalculatorVMLib.FunctionPlotterViewModel);
            this.mvvmManager1.DataSourceType = typeof(MvvmCalculatorVMLib.FunctionPlotterViewModel);
            this.mvvmManager1.DirtyStateManagerComponent = null;
            this.mvvmManager1.DynamicEventHandlingList = null;
            this.mvvmManager1.HostingForm = this;
            this.mvvmManager1.HostingUserControl = null;
            this.mvvmManager1.MvvmBindings.AddPropertyBinding(this.updateCommandButton, new ActiveDevelop.EntitiesFormsLib.BindingSetting(ActiveDevelop.EntitiesFormsLib.MvvmBindingModes.OneWay, ActiveDevelop.EntitiesFormsLib.UpdateSourceTriggerSettings.PropertyChangedImmediately), new ActiveDevelop.EntitiesFormsLib.BindingProperty("Command", typeof(System.Windows.Input.ICommand)), null, null, new ActiveDevelop.EntitiesFormsLib.BindingProperty("CalculateCommand", typeof(ActiveDevelop.MvvmBaseLib.Mvvm.RelayCommand)));
            this.mvvmManager1.MvvmBindings.AddPropertyBinding(this.functionTextBox, new ActiveDevelop.EntitiesFormsLib.BindingSetting(ActiveDevelop.EntitiesFormsLib.MvvmBindingModes.TwoWay, ActiveDevelop.EntitiesFormsLib.UpdateSourceTriggerSettings.LostFocus), new ActiveDevelop.EntitiesFormsLib.BindingProperty("Text", typeof(string)), null, null, new ActiveDevelop.EntitiesFormsLib.BindingProperty("Function", typeof(string)));
            this.mvvmManager1.MvvmBindings.AddPropertyBinding(this.fromTextBox, new ActiveDevelop.EntitiesFormsLib.BindingSetting(ActiveDevelop.EntitiesFormsLib.MvvmBindingModes.TwoWay, ActiveDevelop.EntitiesFormsLib.UpdateSourceTriggerSettings.LostFocus), new ActiveDevelop.EntitiesFormsLib.BindingProperty("Text", typeof(string)), typeof(MvvmCalculator.Converter.NullableDoubleToStringConverter), null, new ActiveDevelop.EntitiesFormsLib.BindingProperty("StartRange", typeof(System.Nullable<double>)));
            this.mvvmManager1.MvvmBindings.AddPropertyBinding(this.toTextBox, new ActiveDevelop.EntitiesFormsLib.BindingSetting(ActiveDevelop.EntitiesFormsLib.MvvmBindingModes.TwoWay, ActiveDevelop.EntitiesFormsLib.UpdateSourceTriggerSettings.LostFocus), new ActiveDevelop.EntitiesFormsLib.BindingProperty("Text", typeof(string)), typeof(MvvmCalculator.Converter.NullableDoubleToStringConverter), null, new ActiveDevelop.EntitiesFormsLib.BindingProperty("EndRange", typeof(System.Nullable<double>)));
            this.mvvmManager1.MvvmBindings.AddPropertyBinding(this.functionPlotterRenderer1, new ActiveDevelop.EntitiesFormsLib.BindingSetting(ActiveDevelop.EntitiesFormsLib.MvvmBindingModes.OneWay, ActiveDevelop.EntitiesFormsLib.UpdateSourceTriggerSettings.PropertyChangedImmediately), new ActiveDevelop.EntitiesFormsLib.BindingProperty("PointsToPlot", typeof(System.Collections.ObjectModel.ObservableCollection<MvvmCalculatorVMLib.Point>)), null, null, new ActiveDevelop.EntitiesFormsLib.BindingProperty("PointsToPlot", typeof(System.Collections.ObjectModel.ObservableCollection<MvvmCalculatorVMLib.Point>)));
            this.mvvmManager1.MvvmBindings.AddPropertyBinding(this.functionPlotterRenderer1, new ActiveDevelop.EntitiesFormsLib.BindingSetting(ActiveDevelop.EntitiesFormsLib.MvvmBindingModes.TwoWay, ActiveDevelop.EntitiesFormsLib.UpdateSourceTriggerSettings.PropertyChangedImmediately), new ActiveDevelop.EntitiesFormsLib.BindingProperty("MvvmRenderSize", typeof(System.Nullable<MvvmCalculatorVMLib.Size>)), null, null, new ActiveDevelop.EntitiesFormsLib.BindingProperty("RenderSize", typeof(System.Nullable<MvvmCalculatorVMLib.Size>)));
            // 
            // functionPlotterRenderer1
            // 
            this.functionPlotterRenderer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mvvmManager1.SetEventBindings(this.functionPlotterRenderer1, null);
            this.functionPlotterRenderer1.Location = new System.Drawing.Point(26, 41);
            this.functionPlotterRenderer1.Name = "functionPlotterRenderer1";
            this.functionPlotterRenderer1.PointsToPlot = null;
            this.functionPlotterRenderer1.Size = new System.Drawing.Size(943, 628);
            this.functionPlotterRenderer1.TabIndex = 0;
            // 
            // FunctionPlotterView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1002, 890);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.updateCommandButton);
            this.Controls.Add(this.functionPlotterRenderer1);
            this.mvvmManager1.SetEventBindings(this, null);
            this.Name = "FunctionPlotterView";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FunctionPlotterView";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mvvmManager1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private ActiveDevelop.EntitiesFormsLib.MvvmManager mvvmManager1;
        private FunctionPlotter.FunctionPlotterRenderer functionPlotterRenderer1;
        private ActiveDevelop.EntitiesFormsLib.CommandButton updateCommandButton;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TextBox functionTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label fromLabel;
        private System.Windows.Forms.TextBox fromTextBox;
        private System.Windows.Forms.TextBox toTextBox;
    }
}