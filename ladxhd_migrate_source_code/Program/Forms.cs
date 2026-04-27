using System.Drawing;
using System.Windows.Forms;

namespace LADXHD_Migrater
{
    internal class Forms
    {
        public static Form_MainForm  MainDialog;
        public static Form_OkayForm  OkayDialog; 
        public static Form_YesNoForm YesNoDialog; 

        public static void Initialize()
        {
            MainDialog  = new Form_MainForm();
            OkayDialog  = new Form_OkayForm();
            YesNoDialog = new Form_YesNoForm();

            // The size of forms needs to be manually scaled.
            MainDialog.Size = DPI.Scale(new Size(375, 420));
            OkayDialog.Size = DPI.Scale(new Size(320, 135));
            YesNoDialog.Size = DPI.Scale(new Size(320, 135));

            // Scale the controls through a loop.
            ScaleControls(MainDialog);
            ScaleControls(OkayDialog);
            ScaleControls(YesNoDialog);
        }

        static void ScaleControls(Control parent)
        {
            foreach (Control ctrl in parent.Controls)
            {
                ctrl.Size = DPI.Scale(ctrl.Size);

                if (ctrl.GetType() == typeof(TextBox))
                    ctrl.Location = DPI.Scale(ctrl.Location, AddY:1);
                else
                    ctrl.Location = DPI.Scale(ctrl.Location);

                if (ctrl.HasChildren)
                    ScaleControls(ctrl);
            }
        }
    }
}
