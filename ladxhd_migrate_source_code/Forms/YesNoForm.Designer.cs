using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace LADXHD_Migrater
{
    partial class Form_YesNoForm
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
            ComponentResourceManager resources = new ComponentResourceManager(typeof(Form_YesNoForm));
            Button_Yes = new Button();
            Button_No = new Button();
            Label_Message = new Label();
            SuspendLayout();
            // 
            // Button_Yes
            // 
            Button_Yes.Location = new Point(52, 60);
            Button_Yes.Name = "Button_Yes";
            Button_Yes.Size = new Size(80, 28);
            Button_Yes.TabIndex = 1;
            Button_Yes.Text = "Yes";
            Button_Yes.UseVisualStyleBackColor = true;
            Button_Yes.Click += Button_Yes_Click;
            // 
            // Button_No
            // 
            Button_No.DialogResult = DialogResult.Cancel;
            Button_No.Location = new Point(170, 60);
            Button_No.Name = "Button_No";
            Button_No.Size = new Size(80, 28);
            Button_No.TabIndex = 2;
            Button_No.Text = "No";
            Button_No.UseVisualStyleBackColor = true;
            Button_No.Click += Button_No_Click;
            // 
            // Label_Message
            // 
            Label_Message.Location = new Point(12, 9);
            Label_Message.Name = "Label_Message";
            Label_Message.Size = new Size(280, 40);
            Label_Message.TabIndex = 3;
            // 
            // Form_YesNoForm
            // 
            AcceptButton = Button_Yes;
            AutoScaleMode = AutoScaleMode.None;
            CancelButton = Button_No;
            ClientSize = new Size(304, 96);
            Controls.Add(Label_Message);
            Controls.Add(Button_No);
            Controls.Add(Button_Yes);
            Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            KeyPreview = true;
            MaximizeBox = false;
            Name = "Form_YesNoForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Yes/No Dialog";
            FormClosing += Form_YesNoForm_FormClosing;
            ResumeLayout(false);

        }
        #endregion
        public System.Windows.Forms.Button Button_Yes;
        public System.Windows.Forms.Button Button_No;
        public System.Windows.Forms.Label Label_Message;
    }
}