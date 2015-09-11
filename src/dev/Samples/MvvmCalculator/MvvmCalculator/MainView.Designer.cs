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
            ActiveDevelop.EntitiesFormsLib.MvvmDataGridColumn mvvmDataGrid1_ResultColumn = new ActiveDevelop.EntitiesFormsLib.MvvmDataGridColumn();
            ActiveDevelop.EntitiesFormsLib.MvvmDataGridColumn mvvmDataGrid1_FormulaColumn = new ActiveDevelop.EntitiesFormsLib.MvvmDataGridColumn();
            this.entryfieldLabel = new System.Windows.Forms.Label();
            this.formulaTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.resultLabel = new System.Windows.Forms.Label();
            this.calcCommandButton = new ActiveDevelop.EntitiesFormsLib.CommandButton();
            this.clearListCommandButton = new ActiveDevelop.EntitiesFormsLib.CommandButton();
            this.functionPlotterCommandButton = new ActiveDevelop.EntitiesFormsLib.CommandButton();
            this.mvvmManager1 = new ActiveDevelop.EntitiesFormsLib.MvvmManager(this.components);
            this.mvvmDataGrid1 = new ActiveDevelop.EntitiesFormsLib.MvvmDataGrid();
            mvvmDataGrid1_ResultColumn.DataSourceType = typeof(MvvmCalculatorVMLib.HistoryItemViewModel);
            mvvmDataGrid1_ResultColumn.FontWeight = System.Windows.FontWeight.FromOpenTypeWeight(400);
// TODO: Code generation for '' failed because of Exception 'Value cannot be null.
//Parameter name: e'.
            mvvmDataGrid1_ResultColumn.Width = 2D;
            mvvmDataGrid1_ResultColumn.WidthLengthUnitType = System.Windows.Controls.DataGridLengthUnitType.Star;
            mvvmDataGrid1_ResultColumn.Visibility = System.Windows.Visibility.Visible;
            mvvmDataGrid1_ResultColumn.CellPadding = new System.Windows.Forms.Padding(5);
            mvvmDataGrid1_ResultColumn.Header = "Result";
            mvvmDataGrid1_ResultColumn.IsEnabled = true;
            mvvmDataGrid1_ResultColumn.BackgroundColor = System.Drawing.Color.Empty;
            mvvmDataGrid1_ResultColumn.ForegroundColor = System.Drawing.Color.Empty;
            mvvmDataGrid1_ResultColumn.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
            mvvmDataGrid1_ResultColumn.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            mvvmDataGrid1_ResultColumn.ColumnHeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            mvvmDataGrid1_ResultColumn.ColumnHeaderPadding = new System.Windows.Forms.Padding(0);
            mvvmDataGrid1_ResultColumn.Name = "ResultColumn";
            mvvmDataGrid1_ResultColumn.ColumnType = ActiveDevelop.EntitiesFormsLib.ColumnType.TextAndNumbers;
            mvvmDataGrid1_ResultColumn.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            mvvmDataGrid1_FormulaColumn.DataSourceType = typeof(MvvmCalculatorVMLib.HistoryItemViewModel);
            mvvmDataGrid1_FormulaColumn.FontWeight = System.Windows.FontWeight.FromOpenTypeWeight(400);
// TODO: Code generation for '' failed because of Exception 'Value cannot be null.
//Parameter name: e'.
            mvvmDataGrid1_FormulaColumn.Width = 5D;
            mvvmDataGrid1_FormulaColumn.WidthLengthUnitType = System.Windows.Controls.DataGridLengthUnitType.Star;
            mvvmDataGrid1_FormulaColumn.Visibility = System.Windows.Visibility.Visible;
            mvvmDataGrid1_FormulaColumn.CellPadding = new System.Windows.Forms.Padding(5);
            mvvmDataGrid1_FormulaColumn.Header = "Formula";
            mvvmDataGrid1_FormulaColumn.IsEnabled = true;
            mvvmDataGrid1_FormulaColumn.BackgroundColor = System.Drawing.Color.Empty;
            mvvmDataGrid1_FormulaColumn.ForegroundColor = System.Drawing.Color.Empty;
            mvvmDataGrid1_FormulaColumn.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
            mvvmDataGrid1_FormulaColumn.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            mvvmDataGrid1_FormulaColumn.ColumnHeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            mvvmDataGrid1_FormulaColumn.ColumnHeaderPadding = new System.Windows.Forms.Padding(0);
            mvvmDataGrid1_FormulaColumn.Name = "FormulaColumn";
            mvvmDataGrid1_FormulaColumn.ColumnType = ActiveDevelop.EntitiesFormsLib.ColumnType.TextAndNumbers;
            mvvmDataGrid1_FormulaColumn.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            ((System.ComponentModel.ISupportInitialize)(this.mvvmManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mvvmDataGrid1)).BeginInit();
            this.SuspendLayout();
            // 
            // entryfieldLabel
            // 
            this.entryfieldLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.entryfieldLabel.AutoSize = true;
            this.mvvmManager1.SetEventBindings(this.entryfieldLabel, null);
            this.entryfieldLabel.Location = new System.Drawing.Point(18, 538);
            this.entryfieldLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.entryfieldLabel.Name = "entryfieldLabel";
            this.entryfieldLabel.Size = new System.Drawing.Size(282, 20);
            this.entryfieldLabel.TabIndex = 0;
            this.entryfieldLabel.Text = "Math expression (e.g. 123.23+5^sin(2))";
            // 
            // formulaTextBox
            // 
            this.formulaTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mvvmManager1.SetEventBindings(this.formulaTextBox, null);
            this.formulaTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.formulaTextBox.Location = new System.Drawing.Point(18, 563);
            this.formulaTextBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.formulaTextBox.Multiline = true;
            this.formulaTextBox.Name = "formulaTextBox";
            this.formulaTextBox.Size = new System.Drawing.Size(508, 113);
            this.formulaTextBox.TabIndex = 1;
            this.formulaTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.mvvmManager1.SetEventBindings(this.label1, null);
            this.label1.Location = new System.Drawing.Point(18, 12);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 20);
            this.label1.TabIndex = 4;
            this.label1.Text = "History:";
            // 
            // resultLabel
            // 
            this.resultLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.resultLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mvvmManager1.SetEventBindings(this.resultLabel, null);
            this.resultLabel.Font = new System.Drawing.Font("Consolas", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.resultLabel.Location = new System.Drawing.Point(22, 417);
            this.resultLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.resultLabel.Name = "resultLabel";
            this.resultLabel.Size = new System.Drawing.Size(504, 104);
            this.resultLabel.TabIndex = 5;
            this.resultLabel.Text = "resultLabel";
            this.resultLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // calcCommandButton
            // 
            this.calcCommandButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.calcCommandButton.Command = null;
            this.calcCommandButton.CommandParameter = null;
            this.mvvmManager1.SetEventBindings(this.calcCommandButton, null);
            this.calcCommandButton.Location = new System.Drawing.Point(375, 708);
            this.calcCommandButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.calcCommandButton.Name = "calcCommandButton";
            this.calcCommandButton.Size = new System.Drawing.Size(148, 57);
            this.calcCommandButton.TabIndex = 7;
            this.calcCommandButton.Text = "Calc";
            this.calcCommandButton.UseVisualStyleBackColor = true;
            // 
            // clearListCommandButton
            // 
            this.clearListCommandButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.clearListCommandButton.Command = null;
            this.clearListCommandButton.CommandParameter = null;
            this.mvvmManager1.SetEventBindings(this.clearListCommandButton, null);
            this.clearListCommandButton.Location = new System.Drawing.Point(219, 708);
            this.clearListCommandButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.clearListCommandButton.Name = "clearListCommandButton";
            this.clearListCommandButton.Size = new System.Drawing.Size(148, 57);
            this.clearListCommandButton.TabIndex = 8;
            this.clearListCommandButton.Text = "Clear List";
            this.clearListCommandButton.UseVisualStyleBackColor = true;
            // 
            // functionPlotterCommandButton
            // 
            this.functionPlotterCommandButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.functionPlotterCommandButton.Command = null;
            this.functionPlotterCommandButton.CommandParameter = null;
            this.mvvmManager1.SetEventBindings(this.functionPlotterCommandButton, null);
            this.functionPlotterCommandButton.Location = new System.Drawing.Point(63, 708);
            this.functionPlotterCommandButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.functionPlotterCommandButton.Name = "functionPlotterCommandButton";
            this.functionPlotterCommandButton.Size = new System.Drawing.Size(148, 57);
            this.functionPlotterCommandButton.TabIndex = 9;
            this.functionPlotterCommandButton.Text = "Function Plotter";
            this.functionPlotterCommandButton.UseVisualStyleBackColor = true;
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
            this.mvvmManager1.MvvmBindings.AddPropertyBinding(this.formulaTextBox, new ActiveDevelop.EntitiesFormsLib.BindingSetting(ActiveDevelop.EntitiesFormsLib.MvvmBindingModes.TwoWay, ActiveDevelop.EntitiesFormsLib.UpdateSourceTriggerSettings.PropertyChangedImmediately), new ActiveDevelop.EntitiesFormsLib.BindingProperty("Text", typeof(string)), null, null, new ActiveDevelop.EntitiesFormsLib.BindingProperty("EnteredFormula", typeof(string)));
            this.mvvmManager1.MvvmBindings.AddPropertyBinding(this.resultLabel, new ActiveDevelop.EntitiesFormsLib.BindingSetting(ActiveDevelop.EntitiesFormsLib.MvvmBindingModes.OneWay, ActiveDevelop.EntitiesFormsLib.UpdateSourceTriggerSettings.PropertyChangedImmediately), new ActiveDevelop.EntitiesFormsLib.BindingProperty("Text", typeof(string)), null, null, new ActiveDevelop.EntitiesFormsLib.BindingProperty("Result", typeof(string)));
            this.mvvmManager1.MvvmBindings.AddPropertyBinding(this.resultLabel, new ActiveDevelop.EntitiesFormsLib.BindingSetting(ActiveDevelop.EntitiesFormsLib.MvvmBindingModes.OneWay, ActiveDevelop.EntitiesFormsLib.UpdateSourceTriggerSettings.PropertyChangedImmediately), new ActiveDevelop.EntitiesFormsLib.BindingProperty("BackColor", typeof(System.Drawing.Color)), typeof(MvvmCalculator.Converter.StringNotEmptyToColorConverter), null, new ActiveDevelop.EntitiesFormsLib.BindingProperty("ErrorText", typeof(string)));
            this.mvvmManager1.MvvmBindings.AddPropertyBinding(this.calcCommandButton, new ActiveDevelop.EntitiesFormsLib.BindingSetting(ActiveDevelop.EntitiesFormsLib.MvvmBindingModes.OneWay, ActiveDevelop.EntitiesFormsLib.UpdateSourceTriggerSettings.PropertyChangedImmediately), new ActiveDevelop.EntitiesFormsLib.BindingProperty("Command", typeof(System.Windows.Input.ICommand)), null, null, new ActiveDevelop.EntitiesFormsLib.BindingProperty("CalcCommand", typeof(ActiveDevelop.MvvmBaseLib.Mvvm.RelayCommand)));
            this.mvvmManager1.MvvmBindings.AddPropertyBinding(this.clearListCommandButton, new ActiveDevelop.EntitiesFormsLib.BindingSetting(ActiveDevelop.EntitiesFormsLib.MvvmBindingModes.OneWay, ActiveDevelop.EntitiesFormsLib.UpdateSourceTriggerSettings.PropertyChangedImmediately), new ActiveDevelop.EntitiesFormsLib.BindingProperty("Command", typeof(System.Windows.Input.ICommand)), null, null, new ActiveDevelop.EntitiesFormsLib.BindingProperty("ClearListCommand", typeof(ActiveDevelop.MvvmBaseLib.Mvvm.RelayCommand)));
            this.mvvmManager1.MvvmBindings.AddPropertyBinding(this.functionPlotterCommandButton, new ActiveDevelop.EntitiesFormsLib.BindingSetting(ActiveDevelop.EntitiesFormsLib.MvvmBindingModes.OneWay, ActiveDevelop.EntitiesFormsLib.UpdateSourceTriggerSettings.PropertyChangedImmediately), new ActiveDevelop.EntitiesFormsLib.BindingProperty("Command", typeof(System.Windows.Input.ICommand)), null, null, new ActiveDevelop.EntitiesFormsLib.BindingProperty("CallFunctionPlotterCommand", typeof(ActiveDevelop.MvvmBaseLib.Mvvm.RelayCommand)));
            this.mvvmManager1.MvvmBindings.AddPropertyBinding(this.mvvmDataGrid1, new ActiveDevelop.EntitiesFormsLib.BindingSetting(ActiveDevelop.EntitiesFormsLib.MvvmBindingModes.OneWay, ActiveDevelop.EntitiesFormsLib.UpdateSourceTriggerSettings.PropertyChangedImmediately), new ActiveDevelop.EntitiesFormsLib.BindingProperty("ItemsSource", typeof(System.Collections.IEnumerable)), null, null, new ActiveDevelop.EntitiesFormsLib.BindingProperty("Formulas", typeof(System.Collections.ObjectModel.ObservableCollection<ActiveDevelop.MvvmBaseLib.FormulaEvaluator.FormulaEvaluator>)));
            this.mvvmManager1.MvvmBindings.AddPropertyBinding(this.mvvmDataGrid1, new ActiveDevelop.EntitiesFormsLib.BindingSetting(ActiveDevelop.EntitiesFormsLib.MvvmBindingModes.TwoWay, ActiveDevelop.EntitiesFormsLib.UpdateSourceTriggerSettings.PropertyChangedImmediately), new ActiveDevelop.EntitiesFormsLib.BindingProperty("SelectedItem", typeof(object)), null, null, new ActiveDevelop.EntitiesFormsLib.BindingProperty("SelectedFormula", typeof(MvvmCalculatorVMLib.HistoryItemViewModel)));
            // 
            // mvvmDataGrid1
            // 
            this.mvvmDataGrid1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mvvmDataGrid1.AutoGenerateColumns = false;
            this.mvvmDataGrid1.CanUserAddRows = false;
            this.mvvmDataGrid1.CanUserDeleteRows = false;
            mvvmDataGrid1_ResultColumn.PropertyCellBindings.Add(new ActiveDevelop.EntitiesFormsLib.BindingSetting(ActiveDevelop.EntitiesFormsLib.MvvmBindingModes.TwoWay, ActiveDevelop.EntitiesFormsLib.UpdateSourceTriggerSettings.LostFocus), new ActiveDevelop.EntitiesFormsLib.BindingProperty("Content", typeof(ActiveDevelop.EntitiesFormsLib.MvvmDataGridColumn)), null, null, new ActiveDevelop.EntitiesFormsLib.BindingProperty("Result", typeof(MvvmCalculatorVMLib.HistoryItemViewModel)));
            this.mvvmDataGrid1.Columns.Add(mvvmDataGrid1_ResultColumn);
            mvvmDataGrid1_FormulaColumn.PropertyCellBindings.Add(new ActiveDevelop.EntitiesFormsLib.BindingSetting(ActiveDevelop.EntitiesFormsLib.MvvmBindingModes.TwoWay, ActiveDevelop.EntitiesFormsLib.UpdateSourceTriggerSettings.LostFocus), new ActiveDevelop.EntitiesFormsLib.BindingProperty("Content", typeof(ActiveDevelop.EntitiesFormsLib.MvvmDataGridColumn)), null, null, new ActiveDevelop.EntitiesFormsLib.BindingProperty("Formula", typeof(MvvmCalculatorVMLib.HistoryItemViewModel)));
            this.mvvmDataGrid1.Columns.Add(mvvmDataGrid1_FormulaColumn);
            this.mvvmDataGrid1.CustomColumnTemplateType = null;
            this.mvvmDataGrid1.DataSourceType = typeof(MvvmCalculatorVMLib.HistoryItemViewModel);
            this.mvvmDataGrid1.EnterAction = null;
            this.mvvmManager1.SetEventBindings(this.mvvmDataGrid1, null);
            this.mvvmDataGrid1.GridLinesVisibility = System.Windows.Controls.DataGridGridLinesVisibility.None;
            this.mvvmDataGrid1.HeadersVisibility = System.Windows.Controls.DataGridHeadersVisibility.None;
            this.mvvmDataGrid1.ItemsSource = null;
            this.mvvmDataGrid1.Location = new System.Drawing.Point(22, 37);
            this.mvvmDataGrid1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.mvvmDataGrid1.Name = "mvvmDataGrid1";
            this.mvvmDataGrid1.SelectedItem = null;
            this.mvvmDataGrid1.SelectionMode = System.Windows.Controls.DataGridSelectionMode.Single;
            this.mvvmDataGrid1.Size = new System.Drawing.Size(504, 348);
            this.mvvmDataGrid1.TabIndex = 10;
            // 
            // MainView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(546, 786);
            this.Controls.Add(this.mvvmDataGrid1);
            this.Controls.Add(this.functionPlotterCommandButton);
            this.Controls.Add(this.clearListCommandButton);
            this.Controls.Add(this.calcCommandButton);
            this.Controls.Add(this.resultLabel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.formulaTextBox);
            this.Controls.Add(this.entryfieldLabel);
            this.mvvmManager1.SetEventBindings(this, null);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "MainView";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MvvmCalc";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.mvvmManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mvvmDataGrid1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label entryfieldLabel;
        private System.Windows.Forms.TextBox formulaTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label resultLabel;
        private ActiveDevelop.EntitiesFormsLib.MvvmManager mvvmManager1;
        private ActiveDevelop.EntitiesFormsLib.CommandButton calcCommandButton;
        private ActiveDevelop.EntitiesFormsLib.CommandButton clearListCommandButton;
        private ActiveDevelop.EntitiesFormsLib.CommandButton functionPlotterCommandButton;
        private ActiveDevelop.EntitiesFormsLib.MvvmDataGrid mvvmDataGrid1;
    }
}

