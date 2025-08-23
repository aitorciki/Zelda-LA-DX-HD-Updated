namespace LADXHD_Migrater
{
    internal class Forms
    {
        public static Form_MainForm  mainDialog;
        public static Form_OkayForm  okayDialog; 
        public static Form_YesNoForm yesNoDialog; 

        public static void Initialize()
        {
            mainDialog  = new Form_MainForm();
            okayDialog  = new Form_OkayForm();
            yesNoDialog = new Form_YesNoForm();
        }
    }
}
