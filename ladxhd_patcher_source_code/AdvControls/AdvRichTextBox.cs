using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace LADXHD_Patcher
{
    public enum TextAlign 
    { 
        Left = 1, 
        Right = 2, 
        Center = 3, 
        Justify = 4 
    }
    public class AdvRichTextBox : RichTextBox
    {
        public void BeginUpdate()
        {
            ++updating;
            if ( updating > 1 ) return;
            oldEventMask = SendMessage( new HandleRef( this, Handle ), EM_SETEVENTMASK, 0, 0 );
            SendMessage( new HandleRef( this, Handle ), WM_SETREDRAW, 0, 0 );
        }
        public void EndUpdate()
        {
            --updating;
            if ( updating > 0 ) return;
            SendMessage( new HandleRef( this, Handle ), WM_SETREDRAW, 1, 0 );
            SendMessage( new HandleRef( this, Handle ), EM_SETEVENTMASK, 0, oldEventMask );
        }
        public new TextAlign SelectionAlignment
        {
            get
            {
                PARAFORMAT fmt = new PARAFORMAT();
                fmt.cbSize = Marshal.SizeOf( fmt );
                SendMessage( new HandleRef( this, Handle ), EM_GETPARAFORMAT, SCF_SELECTION, ref fmt );
                if ( ( fmt.dwMask & PFM_ALIGNMENT ) == 0 )
                return TextAlign.Left;
                return ( TextAlign )fmt.wAlignment;
            }
            set
            {
                PARAFORMAT fmt = new PARAFORMAT();
                fmt.cbSize = Marshal.SizeOf( fmt );
                fmt.dwMask = PFM_ALIGNMENT;
                fmt.wAlignment = ( short )value;
                SendMessage( new HandleRef( this, Handle ), EM_SETPARAFORMAT, SCF_SELECTION, ref fmt );
            }
        }
        protected override void OnHandleCreated( EventArgs e )
        {
            base.OnHandleCreated( e );
            SendMessage( new HandleRef( this, Handle ), EM_SETTYPOGRAPHYOPTIONS, TO_ADVANCEDTYPOGRAPHY, TO_ADVANCEDTYPOGRAPHY );
        }
        private int updating = 0;
        private int oldEventMask = 0;
        private const int EM_SETEVENTMASK = 1073;
        private const int EM_GETPARAFORMAT = 1085;
        private const int EM_SETPARAFORMAT = 1095;
        private const int EM_SETTYPOGRAPHYOPTIONS = 1226;
        private const int WM_SETREDRAW = 11;
        private const int TO_ADVANCEDTYPOGRAPHY = 1;
        private const int PFM_ALIGNMENT = 8;
        private const int SCF_SELECTION = 1;

        [StructLayout( LayoutKind.Sequential )]
        private struct PARAFORMAT
        {
            public int cbSize;
            public uint dwMask;
            public short wNumbering;
            public short wReserved;
            public int dxStartIndent;
            public int dxRightIndent;
            public int dxOffset;
            public short wAlignment;
            public short cTabCount;
            [MarshalAs( UnmanagedType.ByValArray, SizeConst = 32 )]
            public int[] rgxTabs;
            public int dySpaceBefore;
            public int dySpaceAfter;
            public int dyLineSpacing;
            public short sStyle;
            public byte bLineSpacingRule;
            public byte bOutlineLevel;
            public short wShadingWeight;
            public short wShadingStyle;
            public short wNumberingStart;
            public short wNumberingStyle;
            public short wNumberingTab;
            public short wBorderSpace;
            public short wBorderWidth;
            public short wBorders;
        }
        [DllImport( "user32", CharSet = CharSet.Auto )]private static extern int SendMessage( HandleRef hWnd, int msg, int wParam, int lParam );
        [DllImport( "user32", CharSet = CharSet.Auto )]private static extern int SendMessage( HandleRef hWnd, int msg, int wParam, ref PARAFORMAT lp );
    }
}
