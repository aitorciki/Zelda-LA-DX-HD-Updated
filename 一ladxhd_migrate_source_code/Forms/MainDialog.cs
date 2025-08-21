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
    public partial class MainDialog : Form
    {
        public MainDialog()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Functions.MigrateFiles();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Functions.CreatePatches();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
