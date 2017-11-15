using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NickAc.ModernUIDoneRight.Objects.Interaction
{
    public class MaximizeTitlebarButton : ModernTitlebarButton
    {
        Form parent;

        public MaximizeTitlebarButton(Form parent)
        {
            this.parent = parent;
            Click += MaximizeTitlebarButton_Click;
        }

        private void MaximizeTitlebarButton_Click(object sender, MouseEventArgs e)
        {
            FormWindowState finalWindowState = FormWindowState.Normal;
            if (parent.WindowState == FormWindowState.Normal)
                finalWindowState = FormWindowState.Maximized;
            parent.WindowState = finalWindowState;
        }

        public override string Text {
            get {
                return parent.WindowState == FormWindowState.Maximized ? "2" : "1";
            }
            set => base.Text = value;
        }

    }
}
