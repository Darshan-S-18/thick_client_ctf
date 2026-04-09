namespace ThickClientCTF
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label statusLabel;
        private System.Windows.Forms.Button hiddenButton;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.statusLabel = new System.Windows.Forms.Label();
            this.hiddenButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // statusLabel
            // 
            this.statusLabel.AutoSize = true;
            this.statusLabel.Location = new System.Drawing.Point(29, 28);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(255, 13);
            this.statusLabel.TabIndex = 0;
            this.statusLabel.Text = "No output yet. Hint: some actions are not obvious...";
            // 
            // hiddenButton
            // 
            this.hiddenButton.Location = new System.Drawing.Point(32, 75);
            this.hiddenButton.Name = "hiddenButton";
            this.hiddenButton.Size = new System.Drawing.Size(252, 41);
            this.hiddenButton.TabIndex = 1;
            this.hiddenButton.Text = "Request Secure Flag";
            this.hiddenButton.UseVisualStyleBackColor = true;
            this.hiddenButton.Visible = false;
            this.hiddenButton.Click += new System.EventHandler(this.HiddenButton_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(614, 185);
            this.Controls.Add(this.hiddenButton);
            this.Controls.Add(this.statusLabel);
            this.KeyPreview = true;
            this.Name = "MainForm";
            this.Text = "Thick Client CTF";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
