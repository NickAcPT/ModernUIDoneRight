using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace NickAc.ModernUIDoneRight.Native
{
    class DwmNative
    {

        [DllImport("dwmapi.dll", PreserveSig = true)]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, DWMWINDOWATTRIBUTE attr, ref int attrValue, int attrSize);

        private enum DWMWINDOWATTRIBUTE : uint
        {
            NCRenderingEnabled = 1,
            NCRenderingPolicy,
            TransitionsForceDisabled,
            AllowNCPaint,
            CaptionButtonBounds,
            NonClientRtlLayout,
            ForceIconicRepresentation,
            Flip3DPolicy,
            ExtendedFrameBounds,
            HasIconicBitmap,
            DisallowPeek,
            ExcludedFromPeek,
            Cloak,
            Cloaked,
            FreezeRepresentation
        }

        public static void AllowRenderInBorderless(Form f)
        {
            int val = 2;
            DwmSetWindowAttribute(f.Handle, DWMWINDOWATTRIBUTE.NCRenderingPolicy, ref val, 4);
        }


        [DllImport("dwmapi.dll", PreserveSig = true)]
        static extern int DwmExtendFrameIntoClientArea(IntPtr hwnd, ref MARGINS margins);

        [StructLayout(LayoutKind.Sequential)]
        private struct MARGINS
        {
            public int leftWidth;
            public int rightWidth;
            public int topHeight;
            public int bottomHeight;

            public MARGINS(int leftWidth, int rightWidth, int topHeight, int bottomHeight)
            {
                this.leftWidth = leftWidth;
                this.rightWidth = rightWidth;
                this.topHeight = topHeight;
                this.bottomHeight = bottomHeight;
            }
        }

        [DllImport("dwmapi.dll")]
        private static extern int DwmIsCompositionEnabled(out bool enabled);

        public static bool IsCompositionEnabled()
        {
            if (Environment.OSVersion.Version.Major < 6) return false;
            bool enabled;
            DwmIsCompositionEnabled(out enabled);
            return enabled;
        }

        public static bool ExtendFrameIntoClientArea(Form f, int left, int top, int right, int bottom)
        {
            if (IsCompositionEnabled()) {
                MARGINS margins = new MARGINS(left, right, top, bottom);
                DwmExtendFrameIntoClientArea(f.Handle, ref margins);
                return true;
            }
            return false;
        }

    }
}
