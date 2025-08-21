namespace LADXHD_Migrater
{
    internal class Forms
    {
        public static Form_MainForm mainDialog;
        public static Form_OkayForm okayDialog; 

        public static void Initialize()
        {
            mainDialog = new Form_MainForm();
            okayDialog = new Form_OkayForm();
        }
    }
}
