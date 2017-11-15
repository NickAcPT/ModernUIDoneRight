﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NickAc.ModernUIDoneRight.Objects.Interaction
{
    public class ModernTitlebarButton
    {
        #region Events
        /// <summary>
        /// Called to signal to subscribers that this button was clicked
        /// </summary>
        public event EventHandler<MouseEventArgs> Click;
        public void OnClick(MouseEventArgs e)
        {
            var eh = Click;
            eh?.Invoke(this, e);
        }
        #endregion

        #region Properties
        public virtual Font Font { get; set; } = SystemFonts.DialogFont;
        public virtual String Text { get; set; }
        public virtual int Width { get; set; }
        public virtual Boolean Visible { get; set; } = true;

        #endregion
    }
}
