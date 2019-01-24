using ModernUITest.Properties;
using NickAc.ModernUIDoneRight;
using NickAc.ModernUIDoneRight.Controls;
using NickAc.ModernUIDoneRight.Forms;
using NickAc.ModernUIDoneRight.Objects;
using NickAc.ModernUIDoneRight.Objects.Interaction;
using System.Drawing;
using System.Windows.Forms;

namespace ModernUITest
{
    public partial class Form1 : ModernForm
    {

        public Form1()
        {
            InitializeComponent();

          //  this.ColorScheme = ColorScheme.CreateSimpleColorScheme(ColorTranslator.FromHtml("#2D2D30"));
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

            appBar1.ToolTip = new ModernToolTip();
            AppAction action1 = new AppAction();
            action1.Cursor = Cursors.Hand;
            action1.Image = Resources.Speaker;
            action1.Click+=Action1_Click;
            action1.ToolTip = "click me!";            
            this.appBar1.Actions.Add(action1);            

            AppAction action2 = new AppAction();
            action2.Cursor = Cursors.Help;
            action2.Click += Action1_Click;
            action2.Image = Resources.Speaker;
            action2.ToolTip = "ALICE was beginning to get very tired of sitting by her sister on the bank and of having nothing to do: \n" +
            	"once or twice she had peeped into the book her sister was reading, but it had no pictures or conversations in it, \n" +
            	"\"and what is the use of a book,\" thought Alice, \"without pictures or conversations?'\n\n";
            this.appBar1.Actions.Add(action2);            
        }

       
        void Action1_Click(object sender, System.EventArgs e)
        {
            if (appBar1.ToolTip != null)
            {
                appBar1.ToolTip.Dispose();
            }
            MessageBox.Show(sender.ToString() + " clicked");
            if (appBar1.ToolTip is ModernToolTip)
            {
                appBar1.ToolTip = new ToolTip();                
            }
            else
            {
                appBar1.ToolTip = new ModernToolTip();
            }
        }

    }
}
