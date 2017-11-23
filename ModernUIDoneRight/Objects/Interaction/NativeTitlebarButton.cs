using System.Drawing;
using System.Windows.Forms;

namespace NickAc.ModernUIDoneRight.Objects.Interaction
{
    public class NativeTitlebarButton : ModernTitlebarButton
    {
        Form parent;
        readonly TitlebarAction action;
        //Make use of the Marlett font.
        //This font provides characters that can be used to display a caption button
        static Font fMarlett = new Font("Marlett", 10f);

        public NativeTitlebarButton(Form parent, int width, TitlebarAction action)
        {
            this.action = action;
            this.parent = parent;
            this.Width = width;
            this.Font = fMarlett;
            Click += TitlebarButton_Click;
        }

        private void TitlebarButton_Click(object sender, MouseEventArgs e)
        {
            switch (action) {
                case TitlebarAction.Minimize:
                    parent.WindowState = FormWindowState.Minimized;
                    break;
                case TitlebarAction.Maximize:
                    FormWindowState finalWindowState = FormWindowState.Normal;
                    if (parent.WindowState == FormWindowState.Normal)
                        finalWindowState = FormWindowState.Maximized;
                    parent.WindowState = finalWindowState;
                    break;
                case TitlebarAction.Close:
                    parent.Close();
                    break;
            }
        }

        private string GetButtonText()
        {
            switch (action) {
                case TitlebarAction.Minimize:
                    return "0"; //In Marlett, "0" represents minimize button
                case TitlebarAction.Maximize:
                    return parent.WindowState == FormWindowState.Maximized ? "2" : "1"; //In Marlett, "1" represents maximize and "2" represents restore button
                case TitlebarAction.Close:
                    return "r"; //In Marlett, "r" represents close button
            }
            return "";
        }

        public override string Text {
            get {
                return GetButtonText();
            }
            set => base.Text = value;
        }

        public override bool Visible {
            get {
                switch (action) {
                    case TitlebarAction.Minimize:
                        return parent.MinimizeBox;
                    case TitlebarAction.Maximize:
                        return parent.MaximizeBox;
                }
                return true;
            }
            set => base.Visible = value;
        }

        public enum TitlebarAction
        {
            Minimize, //Minimize
            Maximize, //Maximize/Restore
            Close //Close
        }
    }
}
