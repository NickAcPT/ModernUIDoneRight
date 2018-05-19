using NickAc.ModernUIDoneRight;
using NickAc.ModernUIDoneRight.Forms;
using NickAc.ModernUIDoneRight.Objects;
using NickAc.ModernUIDoneRight.Objects.Interaction;
using NickAc.ModernUIDoneRight.Objects.MenuItems;
using System.Windows.Forms;

namespace ModernUITest
{
    public partial class Form1 : ModernForm
    {
        public Form1()
        {
            InitializeComponent();
            TopMost = true;
            var btn2 = new ModernTitlebarButton
            {
                Text = "2"

            };
            btn2.Click += (s, e) => MessageBox.Show("2");
            TitlebarButtons.Add(btn2);
            var btn1 = new ModernTitlebarButton
            {
                Text = "1"
            };
            btn1.Click += (s, e) => MessageBox.Show("1");
            TitlebarButtons.Add(btn1);
            
            var item11 = new SidebarTextItem("Text 1");
            item11.Click += (s, e) => { MessageBox.Show(this, "3"); };
        }

        private void Form1_Load(object sender, System.EventArgs e)
        {

        }
    }
}
