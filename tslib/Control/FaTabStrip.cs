using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using tslib.BaseClasses;
using tslib.Design;

namespace tslib.Control
{
    [Designer(typeof (FaTabStripDesigner))]
    [DefaultEvent("TabStripItemSelectionChanged")]
    [DefaultProperty("Items")]
    [ToolboxItem(true)]
    public sealed class FaTabStrip : BaseStyledPanel, ISupportInitialize
    {
        internal static int PreferredWidth = 350;
        internal static int PreferredHeight = 200;

        private const int DefGlyphWidth = 40;

        private int _defHeaderHeight;

        private int _defStartPos = 10;

        public event TabStripItemClosingHandler TabStripItemClosing;
        public event TabStripItemChangedHandler TabStripItemSelectionChanged;
        public event HandledEventHandler MenuItemsLoading;
        public event EventHandler MenuItemsLoaded;
        public event EventHandler TabStripItemClosed;

        private Rectangle _stripButtonRect = Rectangle.Empty;
        private FaTabStripItem _selectedItem;
        private readonly FaTabStripMenuGlyph _menuGlyph;
        private readonly FaTabStripCloseButton _closeButton;
        private readonly FaTabStripItemCollection _items;
        private readonly StringFormat _sf;
        private new static readonly Font DefaultFont = new Font("Segoe", 9f, FontStyle.Regular);

        private bool _alwaysShowClose = true;
        private bool _isIniting;
        private bool _alwaysShowMenuGlyph = true;
        private bool _menuOpen;

        public HitTestResult HitTest(Point pt)
        {
            if(_closeButton.Bounds.Contains(pt))
            {
                return HitTestResult.CloseButton;
            }            
            if(_menuGlyph.Bounds.Contains(pt))
            {
                return HitTestResult.MenuGlyph;
            }
            return GetTabItemByPoint(pt) != null ? HitTestResult.TabItem : HitTestResult.None;
        }
        
        public void AddTab(FaTabStripItem tabItem)
        {
            AddTab(tabItem, false);
        }
        
        public void AddTab(FaTabStripItem tabItem, bool autoSelect)
        {
            tabItem.Dock = DockStyle.Fill;
            Items.Add(tabItem);
            if ((!autoSelect || !tabItem.Visible) && (!tabItem.Visible || Items.DrawnCount >= 1))
            {
                return;
            }
            SelectedItem = tabItem;
            SelectItem(tabItem);
        }

        public void RemoveTab(FaTabStripItem tabItem)
        {
            var tabIndex = Items.IndexOf(tabItem);
            if (tabIndex >= 0)
            {
                UnSelectItem(tabItem);
                Items.Remove(tabItem);
            }
            if (Items.Count <= 0)
            {
                return;
            }
            if (RightToLeft == RightToLeft.No)
            {
                SelectedItem = Items[tabIndex - 1] ?? Items.FirstVisible;
            }
            else
            {
                SelectedItem = Items[tabIndex + 1] ?? Items.LastVisible;
            }
        }

        public FaTabStripItem GetTabItemByPoint(Point pt)
        {
            FaTabStripItem item = null;
            var found = false;          
            for (var i = 0; i < Items.Count; i++)
            {
                var current = Items[i];
                if (current.StripRect.Contains(pt) && current.Visible && current.IsDrawn)
                {
                    item = current;
                    found = true;
                }
                if (found)
                {
                    break;
                }
            }
            return item;
        }

        public void ShowMenu()
        {
            if (CtxMenu.Visible || CtxMenu.Items.Count <= 0)
            {
                return;
            }
            CtxMenu.Show(this,
                      RightToLeft == RightToLeft.No
                          ? new Point(_menuGlyph.Bounds.Left, _menuGlyph.Bounds.Bottom)
                          : new Point(_menuGlyph.Bounds.Right, _menuGlyph.Bounds.Bottom));
            _menuOpen = true;
        }

        internal void UnDrawAll()
        {
            for (var i = 0; i < Items.Count; i++)
            {
                Items[i].IsDrawn = false;
            }
        }

        internal void SelectItem(FaTabStripItem tabItem)
        {
            tabItem.Dock = DockStyle.Fill;
            tabItem.Visible = true;
            tabItem.Selected = true;
        }

        internal void UnSelectItem(FaTabStripItem tabItem)
        {
            tabItem.Selected = false;
        }

        internal void OnTabStripItemClosing(TabStripItemClosingEventArgs e)
        {
            if (TabStripItemClosing != null)
            {
                TabStripItemClosing(e);
            }
        }

        internal void OnTabStripItemClosed(EventArgs e)
        {
            if (TabStripItemClosed != null)
            {
                TabStripItemClosed(this, e);
            }
        }

        private void OnMenuItemsLoading(HandledEventArgs e)
        {
            if (MenuItemsLoading != null)
            {
                MenuItemsLoading(this, e);
            }
        }

        private void OnMenuItemsLoaded(EventArgs e)
        {
            if (MenuItemsLoaded != null)
            {
                MenuItemsLoaded(this, e);
            }
        }

        private void OnTabStripItemChanged(TabStripItemChangedEventArgs e)
        {
            if (TabStripItemSelectionChanged != null)
            {
                TabStripItemSelectionChanged(e);
            }
        }

        private void OnMenuItemsLoad()
        {
            CtxMenu.RightToLeft = RightToLeft;
            CtxMenu.Items.Clear();
            for (var i = 0; i < Items.Count; i++)
            {
                var item = Items[i];
                if (!item.Visible)
                {
                    continue;
                }
                var tItem = new ToolStripMenuItem(item.Title) {Tag = item, Image = item.Image};
                CtxMenu.Items.Add(tItem);
            }        
            OnMenuItemsLoaded(EventArgs.Empty);
        }

        protected override void OnRightToLeftChanged(EventArgs e)
        {
            base.OnRightToLeftChanged(e);
            UpdateLayout();
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            SetDefaultSelected();
            var borderRc = ClientRectangle;
            borderRc.Width--;
            borderRc.Height--;
            _defStartPos = RightToLeft == RightToLeft.No ? 10 : _stripButtonRect.Right;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            /* Draw pages */
            for (var i = 0; i < Items.Count; i++)
            {
                var currentItem = Items[i];
                if (!currentItem.Visible && !DesignMode)
                {
                    continue;
                }
                OnCalcTabPage(e.Graphics, currentItem);
                currentItem.IsDrawn = false;
                if (!AllowDraw(currentItem))
                {
                    continue;
                }
                OnDrawTabPage(e.Graphics, currentItem);
            }
            /* Draw UndePage Line */
            if (RightToLeft == RightToLeft.No)
            {
                if (Items.DrawnCount == 0 || Items.VisibleCount == 0)
                {
                    e.Graphics.DrawLine(SystemPens.ControlDark, new Point(0, _defHeaderHeight),
                                        new Point(ClientRectangle.Width, _defHeaderHeight));
                }
                else if (SelectedItem != null && SelectedItem.IsDrawn)
                {
                    var end = new Point((int)SelectedItem.StripRect.Left - 9, _defHeaderHeight);                   
                    e.Graphics.DrawLine(new Pen(SystemColors.ActiveCaption), new Point(0, _defHeaderHeight), end);
                    e.Graphics.DrawLine(new Pen(SystemColors.ActiveCaption), end, new Point(ClientRectangle.Width, _defHeaderHeight));
                }
            }
            else
            {
                if (Items.DrawnCount == 0 || Items.VisibleCount == 0)
                {
                    e.Graphics.DrawLine(SystemPens.ControlDark, new Point(0, _defHeaderHeight),
                                        new Point(ClientRectangle.Width, _defHeaderHeight));
                }
                else if (SelectedItem != null && SelectedItem.IsDrawn)
                {
                    var end = new Point((int)SelectedItem.StripRect.Left, _defHeaderHeight);
                    e.Graphics.DrawLine(SystemPens.ControlDark, new Point(0, _defHeaderHeight), end);
                    end.X += (int)SelectedItem.StripRect.Width + 20;
                    e.Graphics.DrawLine(SystemPens.ControlDark, end, new Point(ClientRectangle.Width, _defHeaderHeight));
                }
            }
            /* Draw menu and close glyphs */
            if (AlwaysShowMenuGlyph || Items.DrawnCount > Items.VisibleCount)
            {
                _menuGlyph.DrawGlyph(e.Graphics);
            }
            if (AlwaysShowClose || (SelectedItem != null && SelectedItem.CanClose))
            {
                _closeButton.DrawCross(e.Graphics);
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button != MouseButtons.Left)
            {
                return;
            }
            var result = HitTest(e.Location);
            switch (result)
            {
                case HitTestResult.MenuGlyph:
                    var args = new HandledEventArgs(false);
                    OnMenuItemsLoading(args);
                    if (!args.Handled)
                    {
                        OnMenuItemsLoad();
                    }
                    ShowMenu();
                    break;

                case HitTestResult.CloseButton:
                    if (SelectedItem != null)
                    {
                        var closingArgs = new TabStripItemClosingEventArgs(SelectedItem);
                        OnTabStripItemClosing(closingArgs);
                        if (!closingArgs.Cancel && SelectedItem.CanClose)
                        {
                            RemoveTab(SelectedItem);
                            OnTabStripItemClosed(EventArgs.Empty);
                        }
                    }
                    break;

                case HitTestResult.TabItem:
                    var item = GetTabItemByPoint(e.Location);
                    if (item != null)
                    {
                        SelectedItem = item;
                    }
                    break;
            }
            Invalidate();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (_menuGlyph.Bounds.Contains(e.Location))
            {
                _menuGlyph.IsMouseOver = true;
                Invalidate(_menuGlyph.Bounds);
            }
            else
            {
                if (_menuGlyph.IsMouseOver && !_menuOpen)
                {
                    _menuGlyph.IsMouseOver = false;
                    Invalidate(_menuGlyph.Bounds);
                }
            }
            if (_closeButton.Bounds.Contains(e.Location))
            {
                _closeButton.IsMouseOver = true;
                Invalidate(_closeButton.Bounds);
            }
            else
            {
                if (_closeButton.IsMouseOver)
                {
                    _closeButton.IsMouseOver = false;
                    Invalidate(_closeButton.Bounds);
                }
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            _menuGlyph.IsMouseOver = false;
            Invalidate(_menuGlyph.Bounds);
            _closeButton.IsMouseOver = false;
            Invalidate(_closeButton.Bounds);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            if (_isIniting)
            {
                return;
            }
            UpdateLayout();
        }

        private bool AllowDraw(FaTabStripItem item)
        {
            var result = true;
            if (RightToLeft == RightToLeft.No)
            {
                if (item.StripRect.Right >= _stripButtonRect.Width)
                {
                    result = false;
                }
            }
            else
            {
                if (item.StripRect.Left <= _stripButtonRect.Left)
                {
                    return false;
                }
            }
            return result;
        }

        private void SetDefaultSelected()
        {
            if (_selectedItem == null && Items.Count > 0)
            {
                SelectedItem = Items[0];
            }
            for (var i = 0; i < Items.Count; i++)
            {
                var itm = Items[i];
                itm.Dock = DockStyle.Fill;
            }
        }

        private void OnMenuItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            var clickedItem = (FaTabStripItem) e.ClickedItem.Tag;
            SelectedItem = clickedItem;
        }

        private void OnMenuVisibleChanged(object sender, EventArgs e)
        {
            if (CtxMenu.Visible == false)
            {
                _menuOpen = false;
            }
        }

        private void OnCalcTabPage(Graphics g, FaTabStripItem currentItem)
        {
            var currentFont = Font;
            if (currentItem == SelectedItem)
            {
                currentFont = new Font(Font, FontStyle.Bold);
            }
            var textSize = g.MeasureString(currentItem.Title, currentFont, new SizeF(200, currentFont.Height), _sf);
            textSize.Width += 20;
            if (RightToLeft == RightToLeft.No)
            {
                var buttonRect = new RectangleF(_defStartPos - 9, 1, textSize.Width, _defHeaderHeight);
                currentItem.StripRect = buttonRect;
                _defStartPos += (int) textSize.Width;
            }
            else
            {
                var buttonRect = new RectangleF(_defStartPos - textSize.Width + 1, 3, textSize.Width - 1, _defHeaderHeight);
                currentItem.StripRect = buttonRect;
                _defStartPos -= (int) textSize.Width;
            }
        }

        private void OnDrawTabPage(Graphics g, FaTabStripItem currentItem)
        {
            LinearGradientBrush brush = null;
            var flag = Items.IndexOf(currentItem) == 0;
            var currentFont = Font;
            if (currentItem == SelectedItem)
            {
                currentFont = new Font(Font, FontStyle.Bold);
            }
            var ef = g.MeasureString(currentItem.Title, currentFont, new SizeF(200, currentFont.Height), _sf);
            ef.Width += 20f;
            var stripRect = currentItem.StripRect;
            var layoutRectangle = stripRect;
            using (var path = new GraphicsPath())
            {
                const int num = 3;
                switch (RightToLeft)
                {
                    case RightToLeft.No:
                        if (currentItem == SelectedItem || flag)
                        {
                            path.AddLine(stripRect.Left - 10f, stripRect.Bottom - 1f,
                                         (stripRect.Left + (stripRect.Height/2f)) - 4f, num + 4);
                        }
                        else
                        {
                            path.AddLine(stripRect.Left, stripRect.Bottom - 1f, stripRect.Left,
                                         (stripRect.Bottom - (stripRect.Height/2f)) - 2f);
                            path.AddLine(stripRect.Left, (stripRect.Bottom - (stripRect.Height/2f)) - 3f,
                                         (stripRect.Left + (stripRect.Height/2f)) - 4f, num + 3);
                        }
                        path.AddLine((stripRect.Left + (stripRect.Height/2f)) + 2f, num, stripRect.Right - 3f, num);
                        path.AddLine(stripRect.Right, num + 2, stripRect.Right, stripRect.Bottom - 1f);
                        path.AddLine(stripRect.Right - 4f, stripRect.Bottom - 1f, stripRect.Left, stripRect.Bottom - 1f);
                        path.CloseFigure();
                        brush = currentItem != SelectedItem
                                    ? new LinearGradientBrush(stripRect, SystemColors.ControlLightLight,
                                                              SystemColors.Control, LinearGradientMode.Vertical)
                                    : new LinearGradientBrush(stripRect, SystemColors.ControlLightLight,
                                                              SystemColors.Window, LinearGradientMode.Vertical);
                        g.FillPath(brush, path);
                        g.DrawPath(SystemPens.ControlDark, path);
                        if (currentItem == SelectedItem)
                        {
                            g.DrawLine(new Pen(brush), stripRect.Left - 9f, stripRect.Height + 2f,
                                       (stripRect.Left + stripRect.Width) - 1f,
                                       stripRect.Height + 2f);
                        }
                        layoutRectangle.Location = new PointF((stripRect.Left + stripRect.Height) - 4f,
                                                              ((float) _defHeaderHeight/2) - (stripRect.Height/2) + 2.5f);
                        layoutRectangle.Width = (stripRect.Width - (layoutRectangle.Left - stripRect.Left));// - 4f;
                        layoutRectangle.Height = ef.Height + (currentFont.Size/2f);
                        if (currentItem == SelectedItem)
                        {
                            g.DrawString(currentItem.Title, currentFont, new SolidBrush(ForeColor), layoutRectangle,
                                         _sf);
                        }
                        else
                        {
                            g.DrawString(currentItem.Title, currentFont, new SolidBrush(ForeColor), layoutRectangle,
                                         _sf);
                        }
                        break;

                    case RightToLeft.Yes:
                        if (currentItem == SelectedItem || flag)
                        {
                            path.AddLine(stripRect.Right + 10f, stripRect.Bottom - 1f,
                                         (stripRect.Right - (stripRect.Height/2f)) + 4f, num + 4);
                        }
                        else
                        {
                            path.AddLine(stripRect.Right, stripRect.Bottom - 1f, stripRect.Right,
                                         (stripRect.Bottom - (stripRect.Height/2f)) - 2f);
                            path.AddLine(stripRect.Right, (stripRect.Bottom - (stripRect.Height/2f)) - 3f,
                                         (stripRect.Right - (stripRect.Height/2f)) + 4f, num + 3);
                        }
                        path.AddLine((stripRect.Right - (stripRect.Height/2f)) - 2f, num, stripRect.Left + 3f,
                                     num);
                        path.AddLine(stripRect.Left, num + 2, stripRect.Left, stripRect.Bottom - 1f);
                        path.AddLine(stripRect.Left + 4f, stripRect.Bottom - 1f, stripRect.Right, stripRect.Bottom - 1f);
                        path.CloseFigure();
                        brush = currentItem != SelectedItem
                                    ? new LinearGradientBrush(stripRect, SystemColors.ControlLightLight,
                                                              SystemColors.Control, LinearGradientMode.Vertical)
                                    : new LinearGradientBrush(stripRect, SystemColors.ControlLightLight,
                                                              SystemColors.Window, LinearGradientMode.Vertical);
                        g.FillPath(brush, path);
                        g.DrawPath(SystemPens.ControlDark, path);
                        if (ReferenceEquals(currentItem, SelectedItem))
                        {
                            g.DrawLine(new Pen(brush), stripRect.Right + 9f, stripRect.Height + 2f,
                                       (stripRect.Right - stripRect.Width) + 1f,
                                       stripRect.Height + 2f);
                        }
                        layoutRectangle.Location = new PointF(stripRect.Left + 2f,
                                                              ((float)_defHeaderHeight / 2) - (stripRect.Height / 2) + 2.5f);
                        layoutRectangle.Width = (stripRect.Width - (layoutRectangle.Left - stripRect.Left)) - 10f;
                        layoutRectangle.Height = ef.Height + (currentFont.Size/2f);
                        if (currentItem != SelectedItem)
                        {
                            g.DrawString(currentItem.Title, currentFont, new SolidBrush(ForeColor), layoutRectangle,
                                         _sf);
                        }
                        else
                        {
                            layoutRectangle.Y--;
                            g.DrawString(currentItem.Title, currentFont, new SolidBrush(ForeColor), layoutRectangle,
                                         _sf);
                        }
                        break;
                }
            }
            if (brush != null)
            {
                brush.Dispose();
            }
            currentItem.IsDrawn = true;
        }

        private void UpdateLayout()
        {
            if (RightToLeft == RightToLeft.No)
            {
                _sf.Trimming = StringTrimming.EllipsisCharacter;
                _sf.FormatFlags |= StringFormatFlags.NoWrap;
                _stripButtonRect = new Rectangle(0, 0, ClientSize.Width - DefGlyphWidth - 2, 10);
                _menuGlyph.Bounds = new Rectangle(ClientSize.Width - DefGlyphWidth, 2, 16, 16);
                _closeButton.Bounds = new Rectangle(ClientSize.Width - 20, 2, 16, 16);
            }
            else
            {
                _sf.Trimming = StringTrimming.EllipsisCharacter;
                _sf.FormatFlags |= StringFormatFlags.NoWrap;
                _sf.FormatFlags |= StringFormatFlags.DirectionRightToLeft;
                _stripButtonRect = new Rectangle(DefGlyphWidth + 2, 0, ClientSize.Width - DefGlyphWidth - 15, 10);
                _closeButton.Bounds = new Rectangle(4, 2, 16, 16); //ClientSize.Width - DEF_GLYPH_WIDTH, 2, 16, 16);
                _menuGlyph.Bounds = new Rectangle(20 + 4, 2, 16, 16); //this.ClientSize.Width - 20, 2, 16, 16);
            }
            DockPadding.Top = _defHeaderHeight + 1;
            DockPadding.Bottom = 1;
            DockPadding.Right = 1;
            DockPadding.Left = 1;
        }

        private void OnCollectionChanged(object sender, CollectionChangeEventArgs e)
        {
            var itm = (FaTabStripItem) e.Element;
            switch (e.Action)
            {
                case CollectionChangeAction.Add:
                    Controls.Add(itm);
                    OnTabStripItemChanged(new TabStripItemChangedEventArgs(itm, FaTabStripItemChangeTypes.Added));
                    break;

                case CollectionChangeAction.Remove:
                    Controls.Remove(itm);
                    OnTabStripItemChanged(new TabStripItemChangedEventArgs(itm, FaTabStripItemChangeTypes.Removed));
                    break;

                default:
                    OnTabStripItemChanged(new TabStripItemChangedEventArgs(itm, FaTabStripItemChangeTypes.Changed));
                    break;
            }
            UpdateLayout();
            Invalidate();
        }

        public FaTabStrip()
        {
            BeginInit();
            /* Double buffering */
            SetStyle(ControlStyles.ContainerControl, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.Selectable, true);
            _items = new FaTabStripItemCollection();
            _items.CollectionChanged += OnCollectionChanged;
            base.Size = new Size(350, 200);
            CtxMenu = new ContextMenuStrip {RenderMode = ToolStripRenderMode.ManagerRenderMode};
            CtxMenu.ItemClicked += OnMenuItemClicked;
            CtxMenu.VisibleChanged += OnMenuVisibleChanged;
            _menuGlyph = new FaTabStripMenuGlyph(ToolStripRenderer);
            _closeButton = new FaTabStripCloseButton(ToolStripRenderer);
            Font = DefaultFont;
            _sf = new StringFormat();
            EndInit();
            UpdateLayout();
        }

        public new Font Font
        {
            get { return base.Font; }
            set
            {
                base.Font = value;
                _defHeaderHeight = value.Height + 4;
            }
        }

        [DefaultValue(null)]
        public ContextMenuStrip CtxMenu { get; set; }

        [DefaultValue(null)]
        [RefreshProperties(RefreshProperties.All)]
        public FaTabStripItem SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                if (_selectedItem == value)
                {
                    return;
                }
                if (value == null && Items.Count > 0)
                {
                    var itm = Items[0];
                    if (itm.Visible)
                    {
                        _selectedItem = itm;
                        _selectedItem.Selected = true;
                        _selectedItem.Dock = DockStyle.Fill;
                    }
                }
                else
                {
                    _selectedItem = value;
                }
                foreach (FaTabStripItem itm in Items)
                {
                    if (itm == _selectedItem)
                    {
                        SelectItem(itm);
                        itm.Dock = DockStyle.Fill;
                        itm.Show();
                    }
                    else
                    {
                        UnSelectItem(itm);
                        itm.Hide();
                    }
                }
                SelectItem(_selectedItem);
                Invalidate();
                if (_selectedItem != null && !_selectedItem.IsDrawn)
                {
                    Items.MoveTo(0, _selectedItem);
                    Invalidate();
                }
                OnTabStripItemChanged(new TabStripItemChangedEventArgs(_selectedItem, FaTabStripItemChangeTypes.SelectionChanged));
            }
        }

        [DefaultValue(true)]
        public bool AlwaysShowMenuGlyph
        {
            get { return _alwaysShowMenuGlyph; }
            set
            {
                if (_alwaysShowMenuGlyph == value)
                {
                    return;
                }
                _alwaysShowMenuGlyph = value;
                Invalidate();
            }
        }

        [DefaultValue(true)]
        public bool AlwaysShowClose
        {
            get { return _alwaysShowClose; }
            set
            {
                if (_alwaysShowClose == value)
                {
                    return;
                }
                _alwaysShowClose = value;
                Invalidate();
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public FaTabStripItemCollection Items
        {
            get { return _items; }
        }

        [DefaultValue(typeof (Size), "350,200")]
        public new Size Size
        {
            get { return base.Size; }
            set
            {
                if (base.Size == value)
                    return;

                base.Size = value;
                UpdateLayout();
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new ControlCollection Controls
        {
            get { return base.Controls; }
        }

        public bool ShouldSerializeFont()
        {
            return !Font.Equals(DefaultFont);
        }
        
        public bool ShouldSerializeSelectedItem()
        {
            return true;
        }

        public bool ShouldSerializeItems()
        {
            return _items.Count > 0;
        }

        public new void ResetFont()
        {
            Font = DefaultFont;
        }

        public void BeginInit()
        {
            _isIniting = true;
        }

        public void EndInit()
        {
            _isIniting = false;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _items.CollectionChanged -= OnCollectionChanged;
                CtxMenu.ItemClicked        -= OnMenuItemClicked;
                CtxMenu.VisibleChanged     -= OnMenuVisibleChanged;
                foreach (FaTabStripItem item in _items.Cast<FaTabStripItem>().Where(item => item != null && !item.IsDisposed))
                {
                    item.Dispose();
                }
                if (CtxMenu != null && !CtxMenu.IsDisposed)
                {
                    CtxMenu.Dispose();
                }
                if (_sf != null)
                {
                    _sf.Dispose();
                }
            }
            base.Dispose(disposing);
        }
    }
}