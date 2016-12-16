namespace Server
{
    partial class ServerMainWindow
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
            this.choosePath = new System.Windows.Forms.Button();
            this.startListening = new System.Windows.Forms.Button();
            this.connectedClientsListBox = new System.Windows.Forms.ListBox();
            this.pathTextBox = new System.Windows.Forms.TextBox();
            this.myIPTextBox = new System.Windows.Forms.TextBox();
            this.portNumberInt = new System.Windows.Forms.NumericUpDown();
            this.choosePathDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.portNumberInt)).BeginInit();
            this.SuspendLayout();
            // 
            // choosePath
            // 
            this.choosePath.Location = new System.Drawing.Point(13, 13);
            this.choosePath.Name = "choosePath";
            this.choosePath.Size = new System.Drawing.Size(108, 23);
            this.choosePath.TabIndex = 0;
            this.choosePath.Text = "Choose Path";
            this.choosePath.UseVisualStyleBackColor = true;
            this.choosePath.Click += new System.EventHandler(this.choosePath_Click);
            // 
            // startListening
            // 
            this.startListening.Location = new System.Drawing.Point(500, 46);
            this.startListening.Name = "startListening";
            this.startListening.Size = new System.Drawing.Size(75, 23);
            this.startListening.TabIndex = 1;
            this.startListening.Text = "Start";
            this.startListening.UseVisualStyleBackColor = true;
            this.startListening.Click += new System.EventHandler(this.startListening_Click);
            // 
            // connectedClientsListBox
            // 
            this.connectedClientsListBox.FormattingEnabled = true;
            this.connectedClientsListBox.Location = new System.Drawing.Point(13, 46);
            this.connectedClientsListBox.Name = "connectedClientsListBox";
            this.connectedClientsListBox.Size = new System.Drawing.Size(481, 199);
            this.connectedClientsListBox.TabIndex = 3;
            // 
            // pathTextBox
            // 
            this.pathTextBox.Location = new System.Drawing.Point(128, 16);
            this.pathTextBox.Name = "pathTextBox";
            this.pathTextBox.ReadOnly = true;
            this.pathTextBox.Size = new System.Drawing.Size(262, 20);
            this.pathTextBox.TabIndex = 5;
            // 
            // myIPTextBox
            // 
            this.myIPTextBox.Location = new System.Drawing.Point(500, 94);
            this.myIPTextBox.Name = "myIPTextBox";
            this.myIPTextBox.ReadOnly = true;
            this.myIPTextBox.Size = new System.Drawing.Size(75, 20);
            this.myIPTextBox.TabIndex = 6;
            // 
            // portNumberInt
            // 
            this.portNumberInt.Location = new System.Drawing.Point(500, 12);
            this.portNumberInt.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.portNumberInt.Name = "portNumberInt";
            this.portNumberInt.Size = new System.Drawing.Size(75, 20);
            this.portNumberInt.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(459, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(26, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Port";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(508, 78);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "IP Address";
            // 
            // ServerMainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(587, 261);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.portNumberInt);
            this.Controls.Add(this.myIPTextBox);
            this.Controls.Add(this.pathTextBox);
            this.Controls.Add(this.connectedClientsListBox);
            this.Controls.Add(this.startListening);
            this.Controls.Add(this.choosePath);
            this.Name = "ServerMainWindow";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.portNumberInt)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button choosePath;
        private System.Windows.Forms.Button startListening;
        private System.Windows.Forms.ListBox connectedClientsListBox;
        private System.Windows.Forms.TextBox pathTextBox;
        private System.Windows.Forms.TextBox myIPTextBox;
        private System.Windows.Forms.NumericUpDown portNumberInt;
        private System.Windows.Forms.FolderBrowserDialog choosePathDialog;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}

