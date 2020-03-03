using System;
using System.ComponentModel;
using tslib.Helpers;

namespace tslib.Control
{
    public class FaTabStripItemCollection : CollectionWithEvents
    {
        [Browsable(false)]
        public event CollectionChangeEventHandler CollectionChanged;

        private int _lockUpdate;

        public FaTabStripItemCollection()
        {
            _lockUpdate = 0;
        }

        public FaTabStripItem this[int index]
        {
            get { return index < 0 || List.Count - 1 < index ? null : (FaTabStripItem) List[index]; }
            set
            {
                List[index] = value;
            }
        }

        [Browsable(false)]
        public virtual int DrawnCount
        {
            get
            {
                int count = Count, res = 0;
                if (count == 0) return 0;
                for (var n = 0; n < count; n++)
                {
                    if (this[n].IsDrawn)
                    {
                        res++;
                    }
                }
                return res;
            }
        }

        public virtual FaTabStripItem LastVisible
        {
            get
            {
                for (var n = Count - 1; n > 0; n--)
                {
                    if (this[n].Visible)
                    {
                        return this[n];
                    }
                }
                return null;
            }
        }

        public virtual FaTabStripItem FirstVisible
        {
            get
            {
                for (var n = 0; n < Count; n++)
                {
                    if (this[n].Visible)
                    {
                        return this[n];
                    }
                }
                return null;
            }
        }

        [Browsable(false)]
        public virtual int VisibleCount
        {
            get
            {
                int count = Count, res = 0;
                if (count == 0) return 0;
                for (var n = 0; n < count; n++)
                {
                    if (this[n].Visible)
                    {
                        res++;
                    }
                }
                return res;
            }
        }

        protected virtual void OnCollectionChanged(CollectionChangeEventArgs e)
        {
            if (CollectionChanged != null)
            {
                CollectionChanged(this, e);
            }
        }

        protected virtual void BeginUpdate()
        {
            _lockUpdate++;
        }

        protected virtual void EndUpdate()
        {
            if (--_lockUpdate == 0)
            {
                OnCollectionChanged(new CollectionChangeEventArgs(CollectionChangeAction.Refresh, null));
            }
        }

        public virtual void AddRange(FaTabStripItem[] items)
        {
            BeginUpdate();
            try
            {
                foreach (var item in items)
                {
                    List.Add(item);
                }
            }
            finally
            {
                EndUpdate();
            }
        }

        public virtual void Assign(FaTabStripItemCollection collection)
        {
            BeginUpdate();
            try
            {
                Clear();
                for (var n = 0; n < collection.Count; n++)
                {
                    var item = collection[n];
                    var newItem = new FaTabStripItem();
                    newItem.Assign(item);
                    Add(newItem);
                }
            }
            finally
            {
                EndUpdate();
            }
        }

        public virtual int Add(FaTabStripItem item)
        {
            var res = IndexOf(item);
            if (res == -1)
            {
                res = List.Add(item);
            }
            return res;
        }

        public virtual void Remove(FaTabStripItem item)
        {
            if (List.Contains(item))
            {
                List.Remove(item);
            }
        }

        public virtual FaTabStripItem MoveTo(int newIndex, FaTabStripItem item)
        {
            var currentIndex = List.IndexOf(item);
            if (currentIndex >= 0)
            {
                RemoveAt(currentIndex);
                Insert(0, item);
                return item;
            }
            return null;
        }

        public virtual int IndexOf(FaTabStripItem item)
        {
            return List.IndexOf(item);
        }

        public virtual bool Contains(FaTabStripItem item)
        {
            return List.Contains(item);
        }

        public virtual void Insert(int index, FaTabStripItem item)
        {
            if (Contains(item))
            {
                return;
            }
            List.Insert(index, item);
        }

        protected override void OnInsertComplete(int index, object item)
        {
            var itm = item as FaTabStripItem;
            if (itm != null)
            {
                itm.Changed += OnItemChanged;
            }
            OnCollectionChanged(new CollectionChangeEventArgs(CollectionChangeAction.Add, item));
        }

        protected override void OnRemove(int index, object item)
        {
            base.OnRemove(index, item);
            var itm = item as FaTabStripItem;
            if (itm != null)
            {
                itm.Changed -= OnItemChanged;
            }
            OnCollectionChanged(new CollectionChangeEventArgs(CollectionChangeAction.Remove, item));
        }

        protected override void OnClear()
        {
            if (Count == 0)
            {
                return;
            }
            BeginUpdate();
            try
            {
                for (var n = Count - 1; n >= 0; n--)
                {
                    RemoveAt(n);
                }
            }
            finally
            {
                EndUpdate();
            }
        }

        protected virtual void OnItemChanged(object sender, EventArgs e)
        {
            OnCollectionChanged(new CollectionChangeEventArgs(CollectionChangeAction.Refresh, sender));
        }
    }
}
