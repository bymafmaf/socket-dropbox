namespace _408Client
{
    partial class Form1
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
            this.serverIPAdressText = new System.Windows.Forms.TextBox();
            this.usernameText = new System.Windows.Forms.TextBox();
            this.portNumberInteger = new System.Windows.Forms.NumericUpDown();
            this.connectButton = new System.Windows.Forms.Button();
            this.disconnectButton = new System.Windows.Forms.Button();
            this.uploadButton = new System.Windows.Forms.Button();
            this.uploadedFilesList = new System.Windows.Forms.ListBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.fileAdressText = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.portNumberInteger)).BeginInit();
            this.SuspendLayout();
            // 
            // serverIPAdressText
            // 
            this.serverIPAdressText.Location = new System.Drawing.Point(13, 13);
            this.serverIPAdressText.Name = "serverIPAdressText";
            this.serverIPAdressText.Size = new System.Drawing.Size(100, 20);
            this.serverIPAdressText.TabIndex = 0;
            // 
            // usernameText
            // 
            this.usernameText.Location = new System.Drawing.Point(13, 40);
            this.usernameText.Name = "usernameText";
            this.usernameText.Size = new System.Drawing.Size(100, 20);
            this.usernameText.TabIndex = 1;
            // 
            // portNumberInteger
            // 
            this.portNumberInteger.Location = new System.Drawing.Point(120, 12);
            this.portNumberInteger.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.portNumberInteger.Name = "portNumberInteger";
            this.portNumberInteger.Size = new System.Drawing.Size(81, 20);
            this.portNumberInteger.TabIndex = 2;
            // 
            // connectButton
            // 
            this.connectButton.Location = new System.Drawing.Point(261, 13);
            this.connectButton.Name = "connectButton";
            this.connectButton.Size = new System.Drawing.Size(75, 23);
            this.connectButton.TabIndex = 3;
            this.connectButton.Text = "Connect";
            this.connectButton.UseVisualStyleBackColor = true;
            this.connectButton.Click += new System.EventHandler(this.connectButton_Click);
            // 
            // disconnectButton
            // 
            this.disconnectButton.Location = new System.Drawing.Point(260, 36);
            this.disconnectButton.Name = "disconnectButton";
            this.disconnectButton.Size = new System.Drawing.Size(75, 23);
            this.disconnectButton.TabIndex = 4;
            this.disconnectButton.Text = "Disconnect";
            this.disconnectButton.UseVisualStyleBackColor = true;
            // 
            // uploadButton
            // 
            this.uploadButton.Location = new System.Drawing.Point(13, 84);
            this.uploadButton.Name = "uploadButton";
            this.uploadButton.Size = new System.Drawing.Size(75, 23);
            this.uploadButton.TabIndex = 5;
            this.uploadButton.Text = "Upload";
            this.uploadButton.UseVisualStyleBackColor = true;
            this.uploadButton.Click += new System.EventHandler(this.uploadButton_Click);
            // 
            // uploadedFilesList
            // 
            this.uploadedFilesList.FormattingEnabled = true;
            this.uploadedFilesList.Location = new System.Drawing.Point(13, 124);
            this.uploadedFilesList.Name = "uploadedFilesList";
            this.uploadedFilesList.Size = new System.Drawing.Size(322, 95);
            this.uploadedFilesList.TabIndex = 6;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // fileAdressText
            // 
            this.fileAdressText.Location = new System.Drawing.Point(95, 86);
            this.fileAdressText.Name = "fileAdressText";
            this.fileAdressText.ReadOnly = true;
            this.fileAdressText.Size = new System.Drawing.Size(167, 20);
            this.fileAdressText.TabIndex = 7;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(348, 261);
            this.Controls.Add(this.fileAdressText);
            this.Controls.Add(this.uploadedFilesList);
            this.Controls.Add(this.uploadButton);
            this.Controls.Add(this.disconnectButton);
            this.Controls.Add(this.connectButton);
            this.Controls.Add(this.portNumberInteger);
            this.Controls.Add(this.usernameText);
            this.Controls.Add(this.serverIPAdressText);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.portNumberInteger)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox serverIPAdressText;
        private System.Windows.Forms.TextBox usernameText;
        private System.Windows.Forms.NumericUpDown portNumberInteger;
        private System.Windows.Forms.Button connectButton;
        private System.Windows.Forms.Button disconnectButton;
        private System.Windows.Forms.Button uploadButton;
        private System.Windows.Forms.ListBox uploadedFilesList;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.TextBox fileAdressText;
    }
}

