using NickAc.ModernUIDoneRight.Forms;
using NickAc.ModernUIDoneRight.Objects.Interaction;
using System.Windows.Forms;

namespace ModernUITest
{
    public partial class Form1 : MetroForm
    {
        public Form1()
        {
            InitializeComponent();
            ModernTitlebarButton btn2 = new ModernTitlebarButton
            {
                Text = "2"

            };
            btn2.Click += (s, e) => MessageBox.Show("2");
            TitlebarButtons.Add(btn2);
            ModernTitlebarButton btn1 = new ModernTitlebarButton
            {
                Text = "1"
            };
            btn1.Click += (s, e) => MessageBox.Show("1");
            TitlebarButtons.Add(btn1);
        }
    }
}
