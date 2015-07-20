namespace MRWebApiSelfHost
{
    partial class WebApiSelfHostForm
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.Label1 = new System.Windows.Forms.Label();
            this.LogTextBox = new System.Windows.Forms.TextBox();
            this.StartWebApiServiceButton = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Location = new System.Drawing.Point(13, 9);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(28, 13);
            this.Label1.TabIndex = 8;
            this.Label1.Text = "Log:";
            // 
            // LogTextBox
            // 
            this.LogTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LogTextBox.Location = new System.Drawing.Point(12, 27);
            this.LogTextBox.Multiline = true;
            this.LogTextBox.Name = "LogTextBox";
            this.LogTextBox.Size = new System.Drawing.Size(278, 276);
            this.LogTextBox.TabIndex = 7;
            // 
            // StartWebApiServiceButton
            // 
            this.StartWebApiServiceButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.StartWebApiServiceButton.Location = new System.Drawing.Point(296, 27);
            this.StartWebApiServiceButton.Name = "StartWebApiServiceButton";
            this.StartWebApiServiceButton.Size = new System.Drawing.Size(125, 35);
            this.StartWebApiServiceButton.TabIndex = 6;
            this.StartWebApiServiceButton.Text = "Start Web Api-Service";
            this.StartWebApiServiceButton.UseVisualStyleBackColor = true;
            this.StartWebApiServiceButton.Click += new System.EventHandler(this.StartWebApiServiceButton_Click);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(296, 68);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(125, 35);
            this.button1.TabIndex = 9;
            this.button1.Text = "Test Entity";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.TestEntityButton_Click);
            // 
            // WebApiSelfHostForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(433, 315);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.Label1);
            this.Controls.Add(this.LogTextBox);
            this.Controls.Add(this.StartWebApiServiceButton);
            this.Name = "WebApiSelfHostForm";
            this.Text = "Meter Reader Service: WebApi Status";
            this.Load += new System.EventHandler(this.WebApiSelfHostForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.Label Label1;
        internal System.Windows.Forms.TextBox LogTextBox;
        internal System.Windows.Forms.Button StartWebApiServiceButton;
        internal System.Windows.Forms.Button button1;
    }
}

