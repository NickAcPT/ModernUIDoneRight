using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace NickAc.ModernUIDoneRight.Objects.MenuItems
{
    [Serializable]
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

        /// <summary>
        /// Called to signal to subscribers that the item needs to be measured
        /// </summary>
        public event EventHandler<MeasureMenuItemEventArgs> MeasureItem;
        protected virtual void OnMeasureItem(MeasureMenuItemEventArgs e)
        {
            EventHandler<MeasureMenuItemEventArgs> eh = MeasureItem;

            eh?.Invoke(this, e);
        }

        /// <summary>
        /// Called to signal to subscribers that the item needs to be rendered
        /// </summary>
        public event EventHandler<RenderMenuItemEventArgs> RenderItem;
        protected virtual void OnRenderItem(RenderMenuItemEventArgs e)
        {
            EventHandler<RenderMenuItemEventArgs> eh = RenderItem;

            eh?.Invoke(this, e);
        }

        public Size GetSize(Font font, Graphics g)
        {
            MeasureMenuItemEventArgs args = new MeasureMenuItemEventArgs(font, g, Size.Empty);
            OnMeasureItem(args);
            return args.ItemSize;
        }


        public void DrawItem(Graphics g, Rectangle rect, Font font)
        {
            var args = new RenderMenuItemEventArgs(g, rect, font);
            OnRenderItem(args);
        }



        public void PerformClick()
        {
            OnClick(EventArgs.Empty);
        }

    }
}
