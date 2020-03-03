using System;
using System.Windows.Forms.Design;
using System.ComponentModel.Design;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;
using tslib.Control;

namespace tslib.Design
{
    public class FaTabStripDesigner : ParentControlDesigner
    {
        private IComponentChangeService _changeService;

        public override void Initialize(System.ComponentModel.IComponent component)
        {
            base.Initialize(component);            
            /* Design services */
            _changeService = (IComponentChangeService)GetService(typeof(IComponentChangeService));
            /* Bind design events */
            if (_changeService != null)
            {
                _changeService.ComponentRemoving += OnRemoving;
            }
            Verbs.Add(new DesignerVerb("Add TabStrip", OnAddTabStrip));
            Verbs.Add(new DesignerVerb("Remove TabStrip", OnRemoveTabStrip));
        }

        protected override void Dispose(bool disposing)
        {
            if (_changeService != null)
            {
                _changeService.ComponentRemoving -= OnRemoving;
            }
            base.Dispose(disposing);
        }

        public override ICollection AssociatedComponents
        {
            get
            {
                return Control.Items;
            }
        }

        public new virtual FaTabStrip Control
        {
            get
            {
                return base.Control as FaTabStrip;
            }
        }

        private void OnRemoving(object sender, ComponentEventArgs e)
        {
            var host = (IDesignerHost) GetService(typeof (IDesignerHost));
            /* Removing a button */
            if (e.Component is FaTabStripItem)
            {
                var itm = e.Component as FaTabStripItem;
                if (Control.Items.Contains(itm))
                {
                    _changeService.OnComponentChanging(Control, null);
                    Control.RemoveTab(itm);
                    _changeService.OnComponentChanged(Control, null, null, null);
                    return;
                }
            }
            if (!(e.Component is FaTabStrip))
            {
                return;
            }
            for (var i = Control.Items.Count - 1; i >= 0; i--)
            {
                var itm = Control.Items[i];
                _changeService.OnComponentChanging(Control, null);
                Control.RemoveTab(itm);
                if (host != null)
                {
                    host.DestroyComponent(itm);
                }
                _changeService.OnComponentChanged(Control, null, null, null);
            }
        }

        private void OnAddTabStrip(object sender, EventArgs e)
        {
            var host = (IDesignerHost)GetService(typeof(IDesignerHost));
            if (host == null)
            {
                return;
            }
            var transaction = host.CreateTransaction("Add TabStrip");
            var itm = (FaTabStripItem)host.CreateComponent(typeof(FaTabStripItem));
            _changeService.OnComponentChanging(Control, null);
            Control.AddTab(itm);
            var indx = Control.Items.IndexOf(itm) + 1;
            itm.Title = "TabStrip Page " + indx.ToString();
            Control.SelectItem(itm);
            _changeService.OnComponentChanged(Control, null, null, null);
            transaction.Commit();
        }

        private void OnRemoveTabStrip(object sender, EventArgs e)
        {
            var host = (IDesignerHost)GetService(typeof(IDesignerHost));
            if (host == null)
            {
                return;
            }
            var transaction = host.CreateTransaction("Remove Button");
            _changeService.OnComponentChanging(Control, null);
            var itm = Control.Items[Control.Items.Count - 1];
            Control.UnSelectItem(itm);
            Control.Items.Remove(itm);
            _changeService.OnComponentChanged(Control, null, null, null);
            transaction.Commit();
        }

        protected override bool GetHitTest(Point point)
        {
            var result = Control.HitTest(point);
            return result == HitTestResult.CloseButton || result == HitTestResult.MenuGlyph;
        }

        protected override void PreFilterProperties(IDictionary properties)
        {
            base.PreFilterProperties(properties);
            properties.Remove("DockPadding");
            properties.Remove("DrawGrid");
            properties.Remove("Margin");
            properties.Remove("Padding");
            properties.Remove("BorderStyle");
            properties.Remove("ForeColor");
            properties.Remove("BackColor");
            properties.Remove("BackgroundImage");
            properties.Remove("BackgroundImageLayout");
            properties.Remove("GridSize");
            properties.Remove("ImeMode");
        }

        protected override void WndProc(ref Message msg)
        {
            if (msg.Msg == 0x201)
            {
                var pt = Control.PointToClient(Cursor.Position);
                var itm = Control.GetTabItemByPoint(pt);
                if (itm != null)
                {
                    Control.SelectedItem = itm;
                    var selection = new ArrayList {itm};
                    var selectionService = (ISelectionService)GetService(typeof(ISelectionService));
                    if (selectionService != null)
                    {
                        selectionService.SetSelectedComponents(selection);
                    }
                }
            }
            base.WndProc(ref msg);
        }
    }
}
