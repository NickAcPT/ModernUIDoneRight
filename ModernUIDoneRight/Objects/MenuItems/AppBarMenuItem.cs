using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace NickAc.ModernUIDoneRight.Objects.MenuItems
{
    public abstract class AppBarMenuItem
    {
        /// <summary>
        /// Called to signal to subscribers that this action was clicked
        /// </summary>
        public event EventHandler Click;
        protected virtual void OnClick(EventArgs e)
        {
            EventHandler eh = Click;

            eh?.Invoke(this, e);
        }

        public abstract void RenderItem(RenderMenuItemEventArgs e);
        public abstract void MeasureItem(MeasureMenuItemEventArgs e);
    }
}
