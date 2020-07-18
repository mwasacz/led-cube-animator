using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LedCubeAnimator.Model.Undo
{
    public class CollectionChangeAction<T> : IAction
    {
        public CollectionChangeAction(ICollection<T> collection, T item, bool add)
        {
            Collection = collection;
            Item = item;
            Added = add;
            Removed = !add;
        }

        public ICollection<T> Collection { get; }
        public T Item { get; }
        public bool Added { get; private set; }
        public bool Removed { get; private set; }

        public bool IsEmpty => !Added && !Removed;

        public void Do()
        {
            if (Removed)
            {
                Collection.Remove(Item);
            }
            if (Added)
            {
                Collection.Add(Item);
            }
        }

        public void Undo()
        {
            if (Added)
            {
                Collection.Remove(Item);
            }
            if (Removed)
            {
                Collection.Add(Item);
            }
        }

        public bool TryMerge(IAction action)
        {
            if (action is CollectionChangeAction<T> next
                && next.Collection == Collection
                && AreEqual(next.Item, Item)
                && !(next.Added && Added)
                && !(next.Removed && Removed))
            {
                Added = false;
                Removed = false;
                return true;
            }
            return false;
        }

        private bool AreEqual(T obj1, T obj2) => obj1 == null ? obj2 == null : obj1.Equals(obj2);
    }
}
