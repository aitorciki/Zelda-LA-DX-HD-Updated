namespace LADXHD_Patcher
{
    partial class MainDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainDialog));
            this.button_Patch = new System.Windows.Forms.Button();
            this.button_Exit = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.button_ChangeLog = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // button_Patch
            // 
            this.button_Patch.Location = new System.Drawing.Point(9, 198);
            this.button_Patch.Name = "button_Patch";
            this.button_Patch.Size = new System.Drawing.Size(100, 30);
            this.button_Patch.TabIndex = 0;
            this.button_Patch.Text = "Patch";
            this.button_Patch.UseVisualStyleBackColor = true;
            this.button_Patch.Click += new System.EventHandler(this.button_Patch_Click);
            // 
            // button_Exit
            // 
            this.button_Exit.Location = new System.Drawing.Point(225, 198);
            this.button_Exit.Name = "button_Exit";
            this.button_Exit.Size = new System.Drawing.Size(100, 30);
            this.button_Exit.TabIndex = 1;
            this.button_Exit.Text = "Exit";
            this.button_Exit.UseVisualStyleBackColor = true;
            this.button_Exit.Click += new System.EventHandler(this.button_Exit_Click);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 131);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(311, 65);
            this.label1.TabIndex = 2;
            this.label1.Text = resources.GetString("label1.Text");
            // 
            // button_ChangeLog
            // 
            this.button_ChangeLog.Location = new System.Drawing.Point(117, 198);
            this.button_ChangeLog.Name = "button_ChangeLog";
            this.button_ChangeLog.Size = new System.Drawing.Size(100, 30);
            this.button_ChangeLog.TabIndex = 3;
            this.button_ChangeLog.Text = "Changelog";
            this.button_ChangeLog.UseVisualStyleBackColor = true;
            this.button_ChangeLog.Click += new System.EventHandler(this.button_ChangeLog_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::LADXHD_Patcher.Properties.Resources.la;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(334, 123);
            this.pictureBox1.TabIndex = 4;
            this.pictureBox1.TabStop = false;
            // 
            // MainDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(335, 239);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.button_ChangeLog);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button_Exit);
            this.Controls.Add(this.button_Patch);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Link\'s Awakening DX HD Patcher v1.1.0";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button_Patch;
        private System.Windows.Forms.Button button_Exit;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button_ChangeLog;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}

