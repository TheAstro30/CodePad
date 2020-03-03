using System.Windows.Forms.Design;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using tslib.Control;

namespace tslib.Design
{
    public class FaTabStripItemDesigner : ParentControlDesigner
    {
        private FaTabStripItem _tabStrip;

        public override void Initialize(IComponent component)
        {
            base.Initialize(component);
            _tabStrip = component as FaTabStripItem;
        }

        protected override void PreFilterProperties(System.Collections.IDictionary properties)
        {
            base.PreFilterProperties(properties);
            properties.Remove("Dock");
            properties.Remove("AutoScroll");
            properties.Remove("AutoScrollMargin");
            properties.Remove("AutoScrollMinSize");
            properties.Remove("DockPadding");
            properties.Remove("DrawGrid");
            properties.Remove("Font");
            properties.Remove("Padding");
            properties.Remove("MinimumSize");
            properties.Remove("MaximumSize");
            properties.Remove("Margin");
            properties.Remove("ForeColor");
            properties.Remove("BackColor");
            properties.Remove("BackgroundImage");
            properties.Remove("BackgroundImageLayout");
            properties.Remove("RightToLeft");
            properties.Remove("GridSize");
            properties.Remove("ImeMode");
            properties.Remove("BorderStyle");
            properties.Remove("AutoSize");
            properties.Remove("AutoSizeMode");
            properties.Remove("Location");
        }

        public override SelectionRules SelectionRules
        {
            get
            {
                return 0;
            }
        }

        public override bool CanBeParentedTo(IDesigner parentDesigner)
        {
            return (parentDesigner.Component is FaTabStrip);
        }

        protected override void OnPaintAdornments(PaintEventArgs pe)
        {
            if (_tabStrip == null)
            {
                return;
            }
            using (var p = new Pen(SystemColors.ControlDark))
            {
                p.DashStyle = DashStyle.Dash;
                pe.Graphics.DrawRectangle(p, 0, 0, _tabStrip.Width - 1, _tabStrip.Height - 1);
            }
        }
    }
}
