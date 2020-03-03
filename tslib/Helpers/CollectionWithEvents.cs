using System.Collections;
using System.ComponentModel;

namespace tslib.Helpers
{
    public delegate void CollectionClear();
    public delegate void CollectionChange(int index, object value);

    public abstract class CollectionWithEvents : CollectionBase
    {
        /* Instance fields */
        private int _suspendCount;

        [Browsable(false)]
        public event CollectionClear Clearing;

        [Browsable(false)]
        public event CollectionClear Cleared;

        [Browsable(false)]
        public event CollectionChange Inserting;

        [Browsable(false)]
        public event CollectionChange Inserted;

        [Browsable(false)]
        public event CollectionChange Removing;

        [Browsable(false)]
        public event CollectionChange Removed;

        protected CollectionWithEvents()
        {
            /* Default to not suspended */
            _suspendCount = 0;
        }

        public void SuspendEvents()
        {
            _suspendCount++;
        }

        public void ResumeEvents()
        {
            --_suspendCount;
        }

        [Browsable(false)]
        public bool IsSuspended
        {
            get { return (_suspendCount > 0); }
        }

        protected override void OnClear()
        {
            if (IsSuspended)
            {
                return;
            }
            /* Any attached event handlers? */
            if (Clearing != null)
            {
                Clearing();
            }
        }

        protected override void OnClearComplete()
        {
            if (IsSuspended)
            {
                return;
            }
            /* Any attached event handlers? */
            if (Cleared != null)
            {
                Cleared();
            }
        }

        protected override void OnInsert(int index, object value)
        {
            if (IsSuspended)
            {
                return;
            }
            /* Any attached event handlers? */
            if (Inserting != null)
            {
                Inserting(index, value);
            }
        }

        protected override void OnInsertComplete(int index, object value)
        {
            if (IsSuspended)
            {
                return;
            }
            /* Any attached event handlers? */
            if (Inserted != null)
            {
                Inserted(index, value);
            }
        }

        protected override void OnRemove(int index, object value)
        {
            if (IsSuspended)
            {
                return;
            }
            /* Any attached event handlers? */
            if (Removing != null)
            {
                Removing(index, value);
            }
        }

        protected override void OnRemoveComplete(int index, object value)
        {
            if (IsSuspended)
            {
                return;
            }
            /* Any attached event handlers? */
            if (Removed != null)
            {
                Removed(index, value);
            }
        }

        protected int IndexOf(object value)
        {
            /* Find the 0 based index of the requested entry */
            return List.IndexOf(value);
        }
    }
}
