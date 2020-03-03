using System;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using tslib.Design;

namespace tslib.Control
{
    [Designer(typeof(FaTabStripItemDesigner))]
    [ToolboxItem(false)]
    [DefaultProperty("Title")]
    [DefaultEvent("Changed")]
    public class FaTabStripItem : Panel
    {
        public event EventHandler Changed;

        private bool _selected;
        private bool _visible = true;
        private bool _isDrawn;
        private string _title = string.Empty;
 
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new Size Size
        {
            get { return base.Size; }
            set { base.Size = value; }
        }

        [DefaultValue(true)]
        public new bool Visible
        {
            get { return _visible; }
            set
            {
                if (_visible == value)
                {
                    return;
                }
                _visible = value;
                OnChanged();
            }
        }

        internal RectangleF StripRect { get; set; }

        [Browsable(false)]
        [DefaultValue(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool IsDrawn
        {
            get { return _isDrawn; }
            set
            {
                if (_isDrawn == value)
                {
                    return;
                }
                _isDrawn = value;
            }
        }

        [DefaultValue(null)]
        public Image Image { get; set; }

        [DefaultValue(true)]
        public bool CanClose { get; set; }

        [DefaultValue("Name")]
        [Localizable(true)]
        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                if (_title == value)
                {
                    return;
                }
                _title = value;
                OnChanged();
            }
        }

        [DefaultValue(false)]
        [Browsable(false)]
        public bool Selected
        {
            get { return _selected; }
            set
            {
                if (_selected == value)
                {
                    return;
                }
                _selected = value;
            }
        }

        [Browsable(false)]
        public string Caption
        {
            get { return Title; }
        }

        public FaTabStripItem() : this(string.Empty, null)
        {
            /* Empty by deafult */
        }

        public FaTabStripItem(System.Windows.Forms.Control displayControl) : this(string.Empty, displayControl)
        {
            /* Empty by deafult */
        }

        public FaTabStripItem(string caption, System.Windows.Forms.Control displayControl) 
        {
            CanClose = true;
            Image = null;
            StripRect = Rectangle.Empty;
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.ContainerControl, true);            
            _selected = false;
            Visible = true;
            UpdateText(caption, displayControl);
            /* Add to controls */
            if(displayControl != null)
            {
                Controls.Add(displayControl);
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (!disposing)
            {
                return;
            }
            if (Image != null)
            {
                Image.Dispose();
            }
        }

        public bool ShouldSerializeIsDrawn()
        {
            return false;
        }

        public bool ShouldSerializeDock()
        {
            return false;
        }

        public bool ShouldSerializeControls()
        {
            return Controls.Count > 0;
        }
        
        public bool ShouldSerializeVisible()
        {
            return true;
        }

        private void UpdateText(string caption, System.Windows.Forms.Control displayControl)
        {
            if (displayControl != null && displayControl is ICaptionSupport)
            {
                var capControl = displayControl as ICaptionSupport;
                Title = capControl.Caption;
            }
            else if (caption.Length <= 0 && displayControl != null)
            {
                Title = displayControl.Text;
            }
            else
            {
                Title = caption;
            }
        }

        public void Assign(FaTabStripItem item)
        {
            Visible = item.Visible;
            Text = item.Text;
            CanClose = item.CanClose;
            Tag = item.Tag;
        }

        protected internal virtual void OnChanged()
        {
            if (Changed != null)
            {
                Changed(this, EventArgs.Empty);
            }
        }

        public override string ToString()
        {
            return Caption;
        }
    }
}
