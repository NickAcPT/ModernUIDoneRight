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

            AppAction item = new AppAction
            {
                Image = Icon.ToBitmap()
            };
            item.Click += (sender, e) => MessageBox.Show(sender.ToString());
            appBar1.Actions.Add(item);

            AppBarMenuTextItem item1 = new AppBarMenuTextItem("1");
            item1.Click += (s, e) => {
                MessageBox.Show(this, "1");
            };
            appBar1.MenuItems.Add(item1);
            
            AppBarMenuTextItem item2 = new AppBarMenuTextItem("2");
            item2.Click += (s, e) => {
                MessageBox.Show(this, "2");
            };
            appBar1.MenuItems.Add(item2);

            AppBarMenuTextItem item3 = new AppBarMenuTextItem("3");
            item3.Click += (s, e) => {
                MessageBox.Show(this, "3");
            };
            appBar1.MenuItems.Add(item3);
        }

        private void metroButton1_Click(object sender, System.EventArgs e)
        {
            modernShadowPanel1.Refresh();
        }
    }
}
