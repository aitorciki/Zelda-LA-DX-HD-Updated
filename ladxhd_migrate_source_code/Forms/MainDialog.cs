using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LADXHD_Migrater
{
    public partial class Form_MainForm : Form
    {
        public Form_MainForm()
        {
            InitializeComponent();
        }
        public void ToggleDialog(bool toggle)
        {
            button1.Enabled = toggle;
            button2.Enabled = toggle;
            button3.Enabled = toggle;
            button4.Enabled = toggle;
            button5.Enabled = toggle;
        }

        private void button_Migrate_Click(object sender, EventArgs e)
        {
            Functions.MigrateFiles();
        }

        private void button_Patches_click(object sender, EventArgs e)
        {
            Functions.CreatePatches();
        }

        private void button_Exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button_Clean_Click(object sender, EventArgs e)
        {
            Functions.CleanBuildFiles();
        }

        private void button_Build_Click(object sender, EventArgs e)
        {
            Functions.CreateBuild();
        }

    }
}
