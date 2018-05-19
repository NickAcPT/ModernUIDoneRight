using NickAc.ModernUIDoneRight.Forms;
using NickAc.ModernUIDoneRight.Objects.Interaction;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace NickAc.ModernUIDoneRight.Utils
{
    class FormUtils
    {
        public static ResizeResult ConvertToResizeResult(WindowHitTestResult r)
        {
            switch (r) {
                case WindowHitTestResult.Up:
                    return ResizeResult.Top;
                case WindowHitTestResult.UpLeft:
                    return ResizeResult.TopLeft;
                case WindowHitTestResult.Left:
                    return ResizeResult.Left;
                case WindowHitTestResult.BottomLeft:
                    return ResizeResult.BottomLeft;
                case WindowHitTestResult.Bottom:
                    return ResizeResult.Bottom;
                case WindowHitTestResult.BottomRight:
                    return ResizeResult.BottomRight;
                case WindowHitTestResult.Right:
                    return ResizeResult.Right;
                case WindowHitTestResult.UpRight:
                    return ResizeResult.TopRight;
                default:
                    return ResizeResult.Client;
            }
        }

        private static int ConvertRange(int originalStart, int originalEnd, int newStart, int newEnd, int value)
        {
            var scale = (double)(newEnd - newStart) / (originalEnd - originalStart);
            return (int)(newStart + ((value - originalStart) * scale));
        }

        public enum ResizeResult
        {
            Client = 1,
            TopLeft = 7,
            Top = 8,
            TopRight = 9,
            Left = 10,
            Right = 11,
            Bottom = 15,
            BottomLeft = 16,
            BottomRight = 17,
        }


        public static Cursor HitTestToCursor(ResizeResult result)
        {
            if ((result == ResizeResult.Left) || result == ResizeResult.Right)
                return Cursors.SizeWE;
            if (result == ResizeResult.Bottom || result == ResizeResult.Top)
                return Cursors.SizeNS;
            if (result == ResizeResult.BottomLeft || result == ResizeResult.TopRight)
                return Cursors.SizeNESW;
            if (result == ResizeResult.BottomRight || result == ResizeResult.TopLeft)
                return Cursors.SizeNWSE;
            return Cursors.Default;
        }

        public static void StartFormResizeFromEdge(Form f, ResizeResult result, Control c = null)
        {
            var minimum = f is ModernForm modernForm ? modernForm.MinimumSize : f.MinimumSize;
            var maximum = f is ModernForm form ? form.MaximumSize : f.MaximumSize;
            //Cursor.Clip = Screen.GetWorkingArea(f);
            var curLoc = f.PointToClient(Cursor.Position);
            var fLoc = new Point(f.Left, f.Top);
            var w = f.Width;
            var h = f.Height;
            var resultEnum = ((ResizeResult)result);

            Action<Object, MouseEventArgs> mouseMove = (s, e) => {


                if (f.WindowState != FormWindowState.Maximized) {
                    var changedSize = (new Size(curLoc) - new Size(e.Location)).Width;
                    var changedSizeH = (new Size(curLoc) - new Size(e.Location)).Height;
                    f.Cursor = HitTestToCursor(result);
                    switch (resultEnum) {
                        case ResizeResult.Left:
                            if (curLoc.X <= ModernForm.SizingBorder && ((f.Width + changedSize) >= minimum.Width)) {
                                f.Left -= changedSize;
                                fLoc = new Point(f.Left, f.Top);
                                f.Width += changedSize;
                                f.Update();
                            }
                            break;
                        case ResizeResult.Right:
                            if (curLoc.X >= minimum.Width - ModernForm.SizingBorder) {
                                f.Left = fLoc.X;
                                curLoc = f.PointToClient(Cursor.Position);
                                f.Width = w - changedSize;
                                f.Update();
                                w = f.Width;
                            }
                            break;
                        case ResizeResult.Bottom:
                            if (curLoc.Y >= minimum.Height - ModernForm.SizingBorder) {
                                f.Top = fLoc.Y;
                                curLoc = f.PointToClient(Cursor.Position);
                                f.Height = h - changedSizeH;
                                f.Update();
                                h = f.Height;
                            }
                            break;
                        case ResizeResult.BottomLeft:
                            if (curLoc.X <= ModernForm.SizingBorder && curLoc.Y >= minimum.Height - ModernForm.SizingBorder && ((f.Width + changedSize) >= minimum.Width)) {
                                var ww = e.Location.X - f.Left;
                                var hh = e.Location.Y - f.Bottom;

                                f.Height = f.Bottom + hh;
                                f.Left -= changedSize;
                                fLoc = new Point(f.Left, f.Top);
                                f.Width += changedSize;

                                f.Update();

                            }
                            break;

                        case ResizeResult.BottomRight:
                            var ww2 = e.Location.X - f.Right;
                            var hh2 = e.Location.Y - f.Bottom;

                            f.Height = f.Bottom + hh2;

                            fLoc = new Point(f.Left, f.Top);
                            f.Width += f.Left + ww2;

                            f.Update();

                            break;
                    }
                }
            };
            MouseEventHandler invoke = mouseMove.Invoke;
            f.MouseMove += invoke;
            MouseEventHandler invokeCMove = (s, e) => {
                var pt = f.PointToClient(((Control)s).PointToScreen(e.Location));
                var x = pt.X;
                var y = pt.Y;


                invoke(f, new MouseEventArgs(e.Button, e.Clicks, x, y, e.Delta));
            };
            if (c != null) c.MouseMove += invokeCMove;



            MouseEventHandler invoke2 = null;

            invoke2 = (s, e) => {
                if (c != null) c.MouseMove -= invokeCMove;
                if (c != null) c.MouseUp -= invoke2;
                f.MouseUp -= invoke2;
                f.MouseMove -= invoke;
                f.Update();
                //Cursor.Clip = new Rectangle();
            };

            f.MouseUp += invoke2;
            if (c != null) c.MouseUp += invoke2;


        }

        public static void StartFormDragFromTitlebar(Form f, Control c = null)
        {
            //Cursor.Clip = Screen.GetWorkingArea(f);
            var startCursorPosition = (c ?? f).PointToClient(Cursor.Position);
            Action<Object, MouseEventArgs> mouseMove = (Object s, MouseEventArgs e) => {
                if (f.WindowState == FormWindowState.Maximized) {
                    var beforeW = (f is ModernForm ? (((ModernForm)f).TextBarRectangle.Width) : f.Width);
                    f.WindowState = FormWindowState.Normal;
                    var afterW = (f is ModernForm ? (((ModernForm)f).TextBarRectangle.Width) : f.Width);
                    startCursorPosition = new Point(ConvertRange(0, beforeW, 0, afterW, startCursorPosition.X), startCursorPosition.Y);
                }
                f.Location += (new Size(e.Location) - new Size(startCursorPosition));
            };
            MouseEventHandler invoke = mouseMove.Invoke;
            (c ?? f).MouseMove += invoke;
            (c ?? f).MouseUp += (s, e) => {
                (c ?? f).MouseMove -= invoke;

                //Cursor.Clip = new Rectangle();
            };
        }
    }
}
