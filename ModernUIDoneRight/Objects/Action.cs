﻿using NickAc.ModernUIDoneRight.Controls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace NickAc.ModernUIDoneRight.Objects
{
    [Serializable]
    public class AppAction
    {
        public Image Image { get; set; }

        /// <summary>
        /// Called to signal to subscribers that it was clicked.
        /// </summary>
        public event EventHandler Click;
        protected virtual void OnClick(EventArgs e)
        {
            EventHandler eh = Click;

            eh?.Invoke(this, e);
        }


        public Rectangle GetRectangle(AppBar bar, List<AppAction> containerList)
        {
            if (bar != null && containerList != null && containerList.Contains(this)) {
                int index = containerList.IndexOf(this);
                int size = bar.Height - (bar.XTextOffset * 2);
                int right = bar.Width - bar.XTextOffset - (index * size);
                return Rectangle.FromLTRB(right - size, bar.XTextOffset, right, bar.Height - bar.XTextOffset);
            }

            return Rectangle.Empty;
        }
    }
}
