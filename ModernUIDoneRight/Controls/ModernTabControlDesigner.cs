using System;
using System.ComponentModel.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace NickAc.ModernUIDoneRight.Controls
{
    class ModernTabControlDesigner : ParentControlDesigner
    {

        private void AddTabPage(object sender, EventArgs e)
        {
            if (Control is ModernTabControl tabC) {
                tabC.TabPages.Add(new TabPage());
            }
        }
        DesignerVerbCollection verbs;
        public override System.ComponentModel.Design.DesignerVerbCollection Verbs {
            get {
                if (verbs == null) {
                    verbs = new DesignerVerbCollection();
                    verbs.Add(new DesignerVerb("Add TabPage", AddTabPage));
                }
                return verbs;
            }
        }

        public override bool CanParent(Control control)
        {
            return (control is TabPage && !Control.Contains(control));
        }

    }
}