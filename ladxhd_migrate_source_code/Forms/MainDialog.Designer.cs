namespace LADXHD_Migrater
{
    partial class Form_MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_MainForm));
            button_Migrate = new System.Windows.Forms.Button();
            button_Patches = new System.Windows.Forms.Button();
            button_Exit = new System.Windows.Forms.Button();
            pictureBox1 = new System.Windows.Forms.PictureBox();
            button_Build = new System.Windows.Forms.Button();
            button_Clean = new System.Windows.Forms.Button();
            label_Platform = new System.Windows.Forms.Label();
            comboBox_Platform = new System.Windows.Forms.ComboBox();
            combBox_API = new System.Windows.Forms.ComboBox();
            label_API = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // button_Migrate
            // 
            button_Migrate.Location = new System.Drawing.Point(9, 283);
            button_Migrate.Name = "button_Migrate";
            button_Migrate.Size = new System.Drawing.Size(110, 42);
            button_Migrate.TabIndex = 0;
            button_Migrate.Text = "Migrate Assets From v1.0.0";
            button_Migrate.UseVisualStyleBackColor = true;
            button_Migrate.Click += button_Migrate_Click;
            // 
            // button_Patches
            // 
            button_Patches.Location = new System.Drawing.Point(125, 283);
            button_Patches.Name = "button_Patches";
            button_Patches.Size = new System.Drawing.Size(110, 42);
            button_Patches.TabIndex = 1;
            button_Patches.Text = "Create Patches of Updated Assets";
            button_Patches.UseVisualStyleBackColor = true;
            button_Patches.Click += button_Patches_click;
            // 
            // button_Exit
            // 
            button_Exit.Location = new System.Drawing.Point(241, 283);
            button_Exit.Name = "button_Exit";
            button_Exit.Size = new System.Drawing.Size(110, 42);
            button_Exit.TabIndex = 2;
            button_Exit.Text = "Exit";
            button_Exit.UseVisualStyleBackColor = true;
            button_Exit.Click += button_Exit_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            pictureBox1.Image = Properties.Resources.la;
            pictureBox1.Location = new System.Drawing.Point(0, 0);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new System.Drawing.Size(358, 248);
            pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 3;
            pictureBox1.TabStop = false;
            // 
            // button_Build
            // 
            button_Build.Location = new System.Drawing.Point(183, 331);
            button_Build.Name = "button_Build";
            button_Build.Size = new System.Drawing.Size(168, 42);
            button_Build.TabIndex = 4;
            button_Build.Text = "Create a New Build";
            button_Build.UseVisualStyleBackColor = true;
            button_Build.Click += button_Build_Click;
            // 
            // button_Clean
            // 
            button_Clean.Location = new System.Drawing.Point(9, 331);
            button_Clean.Name = "button_Clean";
            button_Clean.Size = new System.Drawing.Size(168, 42);
            button_Clean.TabIndex = 5;
            button_Clean.Text = "Clean Build Files";
            button_Clean.UseVisualStyleBackColor = true;
            button_Clean.Click += button_Clean_Click;
            // 
            // label_Platform
            // 
            label_Platform.Location = new System.Drawing.Point(9, 259);
            label_Platform.Name = "label_Platform";
            label_Platform.Size = new System.Drawing.Size(48, 16);
            label_Platform.TabIndex = 6;
            label_Platform.Text = "Platform:";
            // 
            // comboBox_Platform
            // 
            comboBox_Platform.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboBox_Platform.FormattingEnabled = true;
            comboBox_Platform.Items.AddRange(new object[] { "Windows", "Android", "Linux (x86-64)", "Linux (Arm64)", "MacOS (x86-64)", "MacOS (Arm64)" });
            comboBox_Platform.Location = new System.Drawing.Point(59, 256);
            comboBox_Platform.Name = "comboBox_Platform";
            comboBox_Platform.Size = new System.Drawing.Size(110, 21);
            comboBox_Platform.TabIndex = 7;
            comboBox_Platform.SelectedIndexChanged += comboBox_Platform_SelectedIndexChanged;
            // 
            // combBox_API
            // 
            combBox_API.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            combBox_API.FormattingEnabled = true;
            combBox_API.Items.AddRange(new object[] { "DirectX", "OpenGL" });
            combBox_API.Location = new System.Drawing.Point(227, 256);
            combBox_API.Name = "combBox_API";
            combBox_API.Size = new System.Drawing.Size(110, 21);
            combBox_API.TabIndex = 9;
            combBox_API.SelectedIndexChanged += combBox_API_SelectedIndexChanged;
            // 
            // label_API
            // 
            label_API.Location = new System.Drawing.Point(182, 259);
            label_API.Name = "label_API";
            label_API.Size = new System.Drawing.Size(41, 16);
            label_API.TabIndex = 8;
            label_API.Text = "Target:";
            // 
            // Form_MainForm
            // 
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            ClientSize = new System.Drawing.Size(359, 381);
            Controls.Add(combBox_API);
            Controls.Add(label_API);
            Controls.Add(comboBox_Platform);
            Controls.Add(label_Platform);
            Controls.Add(button_Clean);
            Controls.Add(button_Build);
            Controls.Add(pictureBox1);
            Controls.Add(button_Exit);
            Controls.Add(button_Patches);
            Controls.Add(button_Migrate);
            Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Form_MainForm";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "Link's Awakening DX HD Migration Tool";
            Load += Form_MainForm_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.Button button_Migrate;
        public System.Windows.Forms.Button button_Patches;
        public System.Windows.Forms.Button button_Exit;
        public System.Windows.Forms.PictureBox pictureBox1;
        public System.Windows.Forms.Button button_Build;
        public System.Windows.Forms.Button button_Clean;
        public System.Windows.Forms.Label label_Platform;
        public System.Windows.Forms.ComboBox comboBox_Platform;
        public System.Windows.Forms.ComboBox combBox_API;
        public System.Windows.Forms.Label label_API;
    }
}

