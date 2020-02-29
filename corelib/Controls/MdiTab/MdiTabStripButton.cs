/* CodePad
 * Written by Jason James Newland
 * Copyright (C) 2020
 * Provided AS-IS with no warranty expressed or implied
 *  
 * Original control by beep, 2008 (code modified and cleaned up)
 * https://www.codeproject.com/Articles/25993/Easy-Tabbed-Mdi-Interface-Using-the-ToolStrip
 */
using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace corelib.Controls.MdiTab
{
    public class MdiTabStripButton : ToolStripSplitButton
    {
        private Form _mdiChild;

        /* Constructor */
        public MdiTabStripButton(Form mdiChild)
        {
            if (null == mdiChild)
            {
                throw new ArgumentNullException("mdiChild");
            }
            SetMdiChild(mdiChild);
        }
        
        /* Private and protected members */
        private void SetMdiChild(Form f)
        {
            if (f == null)
            {
                return;
            }
            if (_mdiChild != null)
            {
                _mdiChild.Activated -= FormActivated;
                _mdiChild.HandleDestroyed -= FormHandleDestroyed;
                _mdiChild.TextChanged -= FormTextChanged;
            }
            _mdiChild = f;
            Text = f.Text;
            f.Activated += FormActivated;
            f.HandleDestroyed += FormHandleDestroyed;
            f.TextChanged += FormTextChanged;
        }

        private void FormActivated(object sender, EventArgs e)
        {
            var ts = Owner as MdiTabStrip;
            if (ts != null)
            {
                ts.SelectedTab = this;
            }
        }

        private void FormTextChanged(object sender, EventArgs e)
        {
            var f = sender as Form;
            if (f != null)
            {
                Text = f.Text;
            }
        }

        private void FormHandleDestroyed(object sender, EventArgs e)
        {
            var f = sender as Form;
            if (f != null)
            {
                f.Activated -= FormActivated;
                f.TextChanged -= FormTextChanged;
                f.HandleDestroyed -= FormHandleDestroyed;
            }
            Owner.Items.Remove(this);
        }
        
        /* Overrides */
        protected override Padding DefaultMargin
        {
            get { return new Padding(0, 1, 5, 2); }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            /* Draw the "close" button */
            var g = e.Graphics;
            var blank = DropDownButtonBounds;
            var rxx = blank.X + 2;
            var rsize = blank.Width - 4;
            var r = new Rectangle(rxx, rsize, rsize, rsize);
            //using (Brush b = new SolidBrush(BackColor))
            //{
            //    g.FillRectangle(b, blank);
            //}
            g.SmoothingMode = SmoothingMode.AntiAlias;
            using (var p = new Pen(CloseBoxColor, 2.0F))
            {
                g.DrawLine(p, r.Left, r.Top, r.Left + r.Width, r.Top + r.Height);
                g.DrawLine(p, r.Left, r.Top + r.Height, r.Left + r.Width, r.Top);
            }
            if (DrawBorder)
            {
                DoDrawBorder(g);
            }
        }

        protected virtual void DoDrawBorder(Graphics g)
        {
            g.SmoothingMode = SmoothingMode.Default;
            using (var p = new Pen(BorderColor, 2F))
            {
                p.Brush = new LinearGradientBrush(new Point(0, 0), new Point(Width / 2 + 2, 2), BackColor, BorderColor);
                g.DrawLine(p, 0, 1, Width / 2 + 2, 1);
                p.Brush = new LinearGradientBrush(new Point(Width / 2 - 2, 2), new Point(Width, 0), BorderColor, BackColor);
                g.DrawLine(p, Width / 2 - 2, 1, Width, 1);
            }
        }

        protected override void OnButtonClick(EventArgs e)
        {
            base.OnButtonClick(e);
            var f = GetMdiChild();
            if (f != null)
            {
                f.Activate();
            }
        }

        protected override void OnDropDownShow(EventArgs e)
        {
            base.OnDropDownShow(e);
            HideDropDown();
            var f = GetMdiChild();
            if (f != null)
            {
                f.Close();
            }
        }

        [Browsable(false)]
        public override Color ForeColor
        {
            get
            {
                return IsSelectedTab ? ActiveForeColor : InactiveForeColor;
            }
        }

        /* Public interface */
        [Browsable(false)]
        public Color ActiveForeColor
        {
            get
            {
                var ts = Owner as MdiTabStrip;
                return ts != null ? ts.ActiveForeColor : MdiTabStrip.DefaultActiveForeColor;
            }
        }

        [Browsable(false)]
        public Color InactiveForeColor
        {
            get
            {
                var ts = Owner as MdiTabStrip;
                return ts != null ? ts.InactiveForeColor : MdiTabStrip.DefaultInactiveForeColor;
            }
        }

        [Browsable(false)]
        public Color CloseBoxColor
        {
            get
            {
                return IsSelectedTab ? ActiveCloseBoxColor : InactiveCloseBoxColor;
            }
        }

        [Browsable(false)]
        public Color ActiveCloseBoxColor
        {
            get
            {
                var ts = Owner as MdiTabStrip;
                return ts != null ? ts.ActiveCloseBoxColor : MdiTabStrip.DefaultActiveCloseBoxColor;
            }
        }

        [Browsable(false)]
        public Color InactiveCloseBoxColor
        {
            get
            {
                var ts = Owner as MdiTabStrip;
                return ts != null ? ts.InactiveCloseBoxColor : MdiTabStrip.DefaultInactiveCloseBoxColor;
            }
        }

        [Browsable(false)]
        public Color BorderColor
        {
            get
            {
                return IsSelectedTab ? ActiveBorderColor : InactiveBorderColor;
            }
        }

        [Browsable(false)]
        public Color ActiveBorderColor
        {
            get
            {
                var ts = Owner as MdiTabStrip;
                return ts != null ? ts.ActiveBorderColor : MdiTabStrip.DefaultActiveBorderColor;
            }
        }

        [Browsable(false)]
        public Color InactiveBorderColor
        {
            get
            {
                var ts = Owner as MdiTabStrip;
                return ts != null ? ts.InactiveBorderColor : MdiTabStrip.DefaultInactiveBorderColor;
            }
        }

        [Browsable(false)]
        public bool DrawBorder
        {
            get
            {
                var ts = Owner as MdiTabStrip;
                return ts != null ? ts.DrawBorder : MdiTabStrip.DefaultDrawBorder;
            }
        }

        [Browsable(false)]
        public bool IsSelectedTab
        {
            get
            {
                var ts = Owner as MdiTabStrip;
                return ts != null && Equals(ts.SelectedTab);
            }
        }

        public Form GetMdiChild()
        {
            return _mdiChild;
        }
    }
}
