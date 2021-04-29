namespace BackupDatabaseApp
{
    partial class UserControl2
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControl2));
            this.button1 = new System.Windows.Forms.Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.richTextBox2 = new System.Windows.Forms.RichTextBox();
            this.button5 = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(316, 136);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(126, 29);
            this.button1.TabIndex = 9;
            this.button1.Text = "Choose location";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // richTextBox1
            // 
            this.richTextBox1.BackColor = System.Drawing.SystemColors.Menu;
            this.richTextBox1.Location = new System.Drawing.Point(45, 136);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(246, 29);
            this.richTextBox1.TabIndex = 10;
            this.richTextBox1.Text = "";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.button3);
            this.panel1.Controls.Add(this.button2);
            this.panel1.Location = new System.Drawing.Point(45, 246);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(397, 38);
            this.panel1.TabIndex = 11;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(268, 3);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(126, 29);
            this.button3.TabIndex = 1;
            this.button3.Text = "Open File";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(3, 6);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(126, 29);
            this.button2.TabIndex = 0;
            this.button2.Text = "Open Folder";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(316, 189);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(126, 30);
            this.button4.TabIndex = 12;
            this.button4.Text = "Export";
            this.button4.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(45, 117);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(198, 16);
            this.label1.TabIndex = 13;
            this.label1.Text = "Please, choose saving location:";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pictureBox1.BackgroundImage")));
            this.pictureBox1.Location = new System.Drawing.Point(302, 21);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(162, 55);
            this.pictureBox1.TabIndex = 14;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.UseWaitCursor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(42, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(244, 31);
            this.label2.TabIndex = 15;
            this.label2.Text = "Backup Database";
            // 
            // richTextBox2
            // 
            this.richTextBox2.BackColor = System.Drawing.SystemColors.Menu;
            this.richTextBox2.Location = new System.Drawing.Point(45, 189);
            this.richTextBox2.Name = "richTextBox2";
            this.richTextBox2.Size = new System.Drawing.Size(246, 29);
            this.richTextBox2.TabIndex = 16;
            this.richTextBox2.Text = "";
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(316, 301);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(126, 26);
            this.button5.TabIndex = 17;
            this.button5.Text = "Close";
            this.button5.UseVisualStyleBackColor = true;
            // 
            // UserControl2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.Controls.Add(this.button5);
            this.Controls.Add(this.richTextBox2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.button1);
            this.Name = "UserControl2";
            this.Size = new System.Drawing.Size(494, 330);
            this.Load += new System.EventHandler(this.UserControl2_Load);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RichTextBox richTextBox2;
        private System.Windows.Forms.Button button5;
    }
}
