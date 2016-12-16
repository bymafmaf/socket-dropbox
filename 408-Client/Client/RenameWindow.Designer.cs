namespace Client
{
    partial class RenameWindow
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
            this.newNameTextField = new System.Windows.Forms.TextBox();
            this.sendNewNameButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // newNameTextField
            // 
            this.newNameTextField.Location = new System.Drawing.Point(12, 12);
            this.newNameTextField.Name = "newNameTextField";
            this.newNameTextField.Size = new System.Drawing.Size(178, 20);
            this.newNameTextField.TabIndex = 0;
            // 
            // sendNewNameButton
            // 
            this.sendNewNameButton.Location = new System.Drawing.Point(55, 38);
            this.sendNewNameButton.Name = "sendNewNameButton";
            this.sendNewNameButton.Size = new System.Drawing.Size(75, 23);
            this.sendNewNameButton.TabIndex = 1;
            this.sendNewNameButton.Text = "Rename";
            this.sendNewNameButton.UseVisualStyleBackColor = true;
            this.sendNewNameButton.Click += new System.EventHandler(this.sendNewNameButton_Click);
            // 
            // RenameWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(204, 69);
            this.Controls.Add(this.sendNewNameButton);
            this.Controls.Add(this.newNameTextField);
            this.Name = "RenameWindow";
            this.Text = "RenameWindow";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox newNameTextField;
        private System.Windows.Forms.Button sendNewNameButton;
    }
}