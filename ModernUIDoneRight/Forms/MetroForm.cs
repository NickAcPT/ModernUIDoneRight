using NickAc.ModernUIDoneRight.Objects;
using NickAc.ModernUIDoneRight.Objects.Interaction;
using NickAc.ModernUIDoneRight.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NickAc.ModernUIDoneRight.Forms
{
    public class MetroForm : Form
    {
        private const int DEFAULT_TITLEBAR_HEIGHT = 32;

        #region Global Variables
        ColorScheme colorScheme;
        readonly List<ModernTitlebarButton> titlebarButtons;
        #endregion

        #region Constructor
        public MetroForm()
        {
            ColorScheme = DefaultColorSchemes.Blue;

            DoubleBuffered = true;
            AutoScaleMode = AutoScaleMode.None;

            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.ContainerControl, true);
            SetStyle(ControlStyles.ResizeRedraw, true);

            titlebarButtons = InitTitleBarButtonList(DEFAULT_TITLEBAR_HEIGHT, this);

            BackColor = Color.White;

            FormBorderStyle = FormBorderStyle.None;
        }
        #endregion

        #region Properties
        public int TitlebarHeight { get; set; } = DEFAULT_TITLEBAR_HEIGHT;

        public ColorScheme ColorScheme {
            get { return colorScheme; }
            set {
                colorScheme = value;
                ForeColor = GraphicUtils.ForegroundColorForBackground(value.SecondaryColor);
                Refresh();
            }
        }
        public Rectangle FormBounds => new Rectangle(Point.Empty, Size);
        public Rectangle TitlebarRectangle => Rectangle.FromLTRB(1, 1, FormBounds.Right - 1, TitlebarHeight + 1);
        public Rectangle TitlebarButtonsRectangle {
            get {
                var btnWidth = GetTitleBarButtonsWidth();
                var titlebarRect = TitlebarRectangle;
                return Rectangle.FromLTRB(titlebarRect.Right - btnWidth, titlebarRect.Top, titlebarRect.Right, titlebarRect.Bottom);
            }
        }
        #endregion

        #region Methods

        private int GetTitleBarButtonsWidth()
        {
            int titlebarButtonOffset = 0;
            for (int i = 0; i < titlebarButtons.Count; i++)
            {
                var btn = titlebarButtons[i];
                if (!btn.Visible) continue;
                Rectangle rect = GetTitlebarButtonRectangle(titlebarButtonOffset, btn);
                titlebarButtonOffset += btn.Width;
            }
            return titlebarButtonOffset;
        }

        private Rectangle GetTitlebarButtonRectangle(int offset, ModernTitlebarButton btn)
        {
            return Rectangle.FromLTRB(TitlebarRectangle.Right - btn.Width - offset, 0, TitlebarRectangle.Right - offset, TitlebarRectangle.Bottom);
        }

        private static List<ModernTitlebarButton> InitTitleBarButtonList(int width, Form parent)
        {
            Font fMarlett = new Font("Marlett", 10f);
            List<ModernTitlebarButton> list = new List<ModernTitlebarButton>();

            //Close
            var close = new ModernTitlebarButton();
            close.Font = fMarlett;
            close.Text = "r";
            close.Width = width;

            close.Click += (sender, e) =>
            {
                parent.Close();
            };

            list.Add(close);


            //Maximize
            var max = new MaximizeTitlebarButton(parent);
            max.Font = fMarlett;
            max.Width = width;
            list.Add(max);

            //Minimize
            var min = new ModernTitlebarButton();
            min.Font = fMarlett;
            min.Text = "0";
            min.Width = width;
            min.Click += (sender, e) =>
            {
                parent.WindowState = FormWindowState.Minimized;
            };
            list.Add(min);

            return list;
        }
        #endregion

        #region Overriden Methods
        public override Size MaximumSize {
            get {
                return Screen.GetWorkingArea(this).Size;
            }
            set {
                value = Screen.GetWorkingArea(this).Size;
                base.MaximumSize = value;
            }
        }

        public override Rectangle DisplayRectangle => Rectangle.FromLTRB(TitlebarRectangle.Left, TitlebarRectangle.Bottom, TitlebarRectangle.Right, FormBounds.Bottom - 1);

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (TitlebarButtonsRectangle.Contains(e.Location)) { this.Invalidate(TitlebarButtonsRectangle); }
        }
        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            this.Invalidate(TitlebarButtonsRectangle);
        }
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            this.Invalidate(TitlebarButtonsRectangle);
        }
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (TitlebarButtonsRectangle.Contains(e.Location)) { this.Invalidate(TitlebarButtonsRectangle); }
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (TitlebarButtonsRectangle.Contains(e.Location)) { this.Invalidate(TitlebarButtonsRectangle); }
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            int titlebarButtonOffset = 0;
            if (TitlebarButtonsRectangle.Contains(e.Location))
            {
                for (int i = 0; i < titlebarButtons.Count; i++)
                {
                    var btn = titlebarButtons[i];
                    if (!btn.Visible) continue;
                    Rectangle rect = GetTitlebarButtonRectangle(titlebarButtonOffset, btn);
                    titlebarButtonOffset += btn.Width;
                    if (rect.Contains(e.Location))
                    {
                        btn.OnClick(e);
                    }
                }
            }
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);
            Point curLoc = PointToClient(Cursor.Position);
            using (SolidBrush primary = new SolidBrush(ColorScheme.PrimaryColor))
            {
                using (SolidBrush secondary = new SolidBrush(ColorScheme.SecondaryColor))
                {
                    e.Graphics.FillRectangle(primary, TitlebarRectangle);
                    GraphicUtils.DrawRectangleBorder(FormBounds, e.Graphics, ColorScheme.SecondaryColor);

                    int titlebarButtonOffset = 0;
                    for (int i = 0; i < titlebarButtons.Count; i++)
                    {
                        var btn = titlebarButtons[i];
                        if (!btn.Visible) continue;
                        Rectangle rect = GetTitlebarButtonRectangle(titlebarButtonOffset, btn);
                        if (rect.Contains(curLoc))
                            e.Graphics.FillRectangle(secondary, rect);
                        GraphicUtils.DrawCenteredText(e.Graphics, btn.Text, btn.Font, rect, ForeColor);
                        titlebarButtonOffset += btn.Width;
                    }
                }

            }
        }
        #endregion
    }
}
