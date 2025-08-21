using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LADXHD_Patcher
{
    public partial class MainDialog : Form
    {
        public MainDialog()
        {
            InitializeComponent();
        }

        private void button_ChangeLog_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/BigheadSMZ/Links-Awakening-DX-HD/blob/master/CHANGELOG.md");
        }

        private void button_Exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void button_Patch_Click(object sender, EventArgs e)
        {
            Functions.StartPatching();
        }
    }
}
