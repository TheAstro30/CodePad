using System;
using tslib.Control;

namespace tslib
{
    public class TabStripItemClosingEventArgs : EventArgs
    {
        public TabStripItemClosingEventArgs(FaTabStripItem item)
        {
            Cancel = false;
            Item = item;
        }

        public FaTabStripItem Item { get; set; }

        public bool Cancel { get; set; }
    }

    public class TabStripItemChangedEventArgs : EventArgs
    {
        private readonly FaTabStripItem _itm;
        private readonly FaTabStripItemChangeTypes _changeType;

        public TabStripItemChangedEventArgs(FaTabStripItem item, FaTabStripItemChangeTypes type)
        {
            _changeType = type;
            _itm = item;
        }

        public FaTabStripItemChangeTypes ChangeType
        {
            get { return _changeType; }
        }

        public FaTabStripItem Item
        {
            get { return _itm; }
        }
    }

    public delegate void TabStripItemChangedHandler(TabStripItemChangedEventArgs e);
    public delegate void TabStripItemClosingHandler(TabStripItemClosingEventArgs e);
}
