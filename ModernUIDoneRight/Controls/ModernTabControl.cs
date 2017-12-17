using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NickAc.ModernUIDoneRight.Controls
{
    [Designer(typeof(ModernTabControlDesigner))]
    public class ModernTabControl : Panel
    {
        
        #region Fields

        private int _currentIndex;

        #endregion

        #region Constructors
        //ParentControlDesigner 
        public ModernTabControl()
        {
            Padding = new Padding(5);
            TabPages = new ModernTabPageCollection(this);
        }

        #endregion

        #region Properties

        public int CurrentIndex {
            get { return _currentIndex; }
            set {
                _currentIndex = value;
                InternalSetCurrentIndex(value);
            }
        }

        public TabPage CurrentTabPage {
            get {
                return (TabPages.Count > 0 && TabPages.Count > _currentIndex) ? TabPages[_currentIndex] : null;
            }
            set {
                InternalSetCurrentIndex(value);
            }
        }

        public override Rectangle DisplayRectangle => Rectangle.FromLTRB(Padding.Left, Padding.Top + TabHeight, Width - Padding.Right, Height - Padding.Bottom);

        public int TabHeight { get; set; } = 20;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ModernTabPageCollection TabPages { get; set; }

        #endregion

        #region Methods

        internal void InternalSetCurrentIndex(int value)
        {
            throw new NotImplementedException();
        }

        internal void InternalSetCurrentIndex(TabPage value)
        {
            InternalSetCurrentIndex(TabPages.IndexOf(value));
        }

        #endregion

        #region Classes

        public class ModernTabPageCollection : List<TabPage>
        {
            public ModernTabPageCollection(ModernTabControl owner)
            {
                Add(new TabPage());
                Owner = owner;
            }

            #region Properties

            public ModernTabControl Owner { get; set; }

            #endregion

            #region Methods

            public new void Add(TabPage item)
            {
                base.Add(item);
            }

            #endregion

        }

        #endregion
    }
}
