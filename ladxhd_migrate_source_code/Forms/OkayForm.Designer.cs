using System;
using System.Drawing;
using System.Windows.Forms;

namespace LADXHD_Migrater
{
    partial class Form_OkayForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_OkayForm));
            Button_OK = new Button();
            Label_Message = new Label();
            SuspendLayout();
            // 
            // Button_OK
            // 
            Button_OK.Location = new Point(110, 60);
            Button_OK.Name = "Button_OK";
            Button_OK.Size = new Size(80, 28);
            Button_OK.TabIndex = 0;
            Button_OK.Text = "OK";
            Button_OK.UseVisualStyleBackColor = true;
            Button_OK.Click += Button_OK_Click;
            // 
            // Label_Message
            // 
            Label_Message.Location = new Point(12, 9);
            Label_Message.Name = "Label_Message";
            Label_Message.Size = new Size(280, 40);
            Label_Message.TabIndex = 1;
            // 
            // Form_OkayForm
            // 
            AcceptButton = Button_OK;
            AutoScaleMode = AutoScaleMode.None;
            ClientSize = new Size(304, 96);
            Controls.Add(Button_OK);
            Controls.Add(Label_Message);
            Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            KeyPreview = true;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Form_OkayForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Okay Dialog";
            FormClosing += Form_OkayForm_FormClosing;
            Load += Form_OkayForm_Load;
            ResumeLayout(false);

        }
        #endregion
        public System.Windows.Forms.Button Button_OK;
        public System.Windows.Forms.Label  Label_Message;
    }
}