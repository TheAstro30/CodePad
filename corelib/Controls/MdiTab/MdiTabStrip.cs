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
using System.Linq;
using System.Windows.Forms;
using System.Drawing;

namespace corelib.Controls.MdiTab
{
    public class MdiTabStrip : ToolStrip
    {
        private Color _activeForeColor;
        private Color _inactiveForeColor;
        private Color _activeCloseBoxColor;
        private Color _inactiveCloseBoxColor;
        private Color _activeBorderColor;
        private Color _inactiveBorderColor;
        private bool _activeIsBold;
        private bool _drawBorder;
        private MdiTabStripButton _selectedTab;

        /* Constructor */
        public MdiTabStrip()
        {
            _activeForeColor = DefaultActiveForeColor;
            _inactiveCloseBoxColor = DefaultInactiveCloseBoxColor;
            _activeCloseBoxColor = DefaultActiveCloseBoxColor;
            _inactiveForeColor = DefaultInactiveForeColor;
            _activeBorderColor = DefaultActiveBorderColor;
            _inactiveBorderColor = DefaultInactiveBorderColor;
            _activeIsBold = DefaultActiveIsBold;
            _drawBorder = DefaultDrawBorder;
        }
       
        static MdiTabStrip()
        {
            DefaultActiveIsBold = true;
            DefaultInactiveBorderColor = Color.White;
            DefaultActiveBorderColor = Color.Silver;
            DefaultInactiveCloseBoxColor = Color.Gray;
            DefaultActiveCloseBoxColor = Color.DarkOrange;
            DefaultInactiveForeColor = Color.Gray;
            DefaultActiveForeColor = DefaultForeColor;
        }

        /* Default properties */
        public static Color DefaultActiveForeColor { get; set; }
        public static Color DefaultInactiveForeColor { get; set; }
        public static Color DefaultActiveCloseBoxColor { get; set; }
        public static Color DefaultInactiveCloseBoxColor { get; set; }
        public static Color DefaultActiveBorderColor { get; set; }
        public static Color DefaultInactiveBorderColor { get; set; }
        public static bool DefaultActiveIsBold { get; set; }
        public static bool DefaultDrawBorder { get; set; }

        /* Private and protected members */
        protected virtual MdiTabStripButton CreateMdiButton(Form f)
        {
            return new MdiTabStripButton(f);
        }

        private void FormMdiChildActivate(object sender, EventArgs e)
        {
            var f = sender as Form;
            if (f == null)
            {
                return;
            }
            AddMdiChild(f.ActiveMdiChild);
            OnFormMdiChildActivate(f.ActiveMdiChild);
        }

        protected virtual void OnFormMdiChildActivate(Form activeForm)
        {
            /* Called when a form is activated by the MDI parent form */
        }
        
        /* Overrides */
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            var mdiForm = FindForm();
            if (null != mdiForm && mdiForm.IsMdiContainer)
            {
                mdiForm.MdiChildActivate += FormMdiChildActivate;
            }
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            base.OnHandleDestroyed(e);
            var mdiForm = FindForm();
            if (null != mdiForm && mdiForm.IsMdiContainer)
            {
                mdiForm.MdiChildActivate -= FormMdiChildActivate;
            }
        }

        protected override void OnItemAdded(ToolStripItemEventArgs e)
        {
            if (!(e.Item is MdiTabStripButton))
            {
                Items.Remove(e.Item);
                return;
            }
            base.OnItemAdded(e);
        }

        [Browsable(false)]
        public override ToolStripItemCollection Items
        {
            get { return base.Items; }
        }

        /* Public interface */
        [Category("Appearance"), DefaultValue(typeof(Color), "ControlText")]
        public Color ActiveForeColor
        {
            get { return _activeForeColor; }
            set
            {
                if (value == ActiveForeColor)
                {
                    return;
                }
                _activeForeColor = value;
                Invalidate();
            }
        }

        [Category("Appearance"), DefaultValue(typeof(Color), "Gray")]
        public Color InactiveForeColor
        {
            get { return _inactiveForeColor; }
            set
            {
                if (value == InactiveForeColor)
                {
                    return;
                }
                _inactiveForeColor = value;
                Invalidate();
            }
        }

        [Category("Appearance"), DefaultValue(typeof(Color), "DarkOrange")]
        public Color ActiveCloseBoxColor
        {
            get { return _activeCloseBoxColor; }
            set
            {
                if (value == ActiveCloseBoxColor)
                {
                    return;
                }
                _activeCloseBoxColor = value;
                Invalidate();
            }
        }

        [Category("Appearance"), DefaultValue(typeof(Color), "Gray")]
        public Color InactiveCloseBoxColor
        {
            get { return _inactiveCloseBoxColor; }
            set
            {
                if (value == InactiveCloseBoxColor)
                {
                    return;
                }
                _inactiveCloseBoxColor = value;
                Invalidate();
            }
        }

        [Category("Appearance"), DefaultValue(typeof(Color), "Silver")]
        public Color ActiveBorderColor
        {
            get { return _activeBorderColor; }
            set
            {
                if (value == ActiveBorderColor)
                {
                    return;
                }
                _activeBorderColor = value;
                Invalidate();
            }
        }

        [Category("Appearance"), DefaultValue(typeof(Color), "White")]
        public Color InactiveBorderColor
        {
            get { return _inactiveBorderColor; }
            set
            {
                if (value == InactiveBorderColor)
                {
                    return;
                }
                _inactiveBorderColor = value;
                Invalidate();
            }
        }

        [Category("Appearance"), DefaultValue(true)]
        public bool ActiveIsBold
        {
            get { return _activeIsBold; }
            set
            {
                if (value == ActiveIsBold)
                {
                    return;
                }
                _activeIsBold = value;
                Invalidate();
            }
        }

        [Category("Appearance"), DefaultValue(false)]
        public bool DrawBorder
        {
            get { return _drawBorder; }
            set
            {
                if (value == DrawBorder)
                {
                    return;
                }
                _drawBorder = value;
                Invalidate();
            }
        }

        [Browsable(false)]
        public MdiTabStripButton SelectedTab
        {
            get { return _selectedTab; }
            set
            {
                if (value == null || !Equals(value.Owner))
                {
                    return;
                }
                if (value.Equals(_selectedTab))
                {
                    return;
                }
                _selectedTab = value;
                foreach (MdiTabStripButton tsb in Items)
                {
                    tsb.Font = Font;
                    tsb.Invalidate();
                }
                if (ActiveIsBold)
                {
                    _selectedTab.Font = new Font(Font, FontStyle.Bold);
                }
                OnItemClicked(new ToolStripItemClickedEventArgs(value));
            }
        }

        public MdiTabStripButton FindTab(Form f)
        {
            return f == null ? null : Items.Cast<MdiTabStripButton>().FirstOrDefault(tsb => f.Equals(tsb.GetMdiChild()));
        }

        public bool ContainsMdiChild(Form f)
        {
            return f != null && Items.Cast<MdiTabStripButton>().Any(tsb => f.Equals(tsb.GetMdiChild()));
        }

        public void AddMdiChild(Form f)
        {
            if (f == null)
            {
                return;
            }
            var tsb = FindTab(f);
            if (tsb != null)
            {
                SelectedTab = tsb;
            }
            else
            {
                tsb = CreateMdiButton(f);
                Items.Add(tsb);
                SelectedTab = tsb;
            }
        }
    }
}
