using System.Collections.Generic;

namespace LedCubeAnimator.Model.Undo
{
    public class CollectionChangeAction<T> : ObjectAction
    {
        public CollectionChangeAction(object obj, string property, ICollection<T> collection, T item, bool add) : base(obj, property)
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

        public override bool IsEmpty => !Added && !Removed;

        public override void Do()
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

        public override void Undo()
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

        public override bool TryMerge(IAction action)
        {
            if (action is CollectionChangeAction<T> next
                && next.Object == Object
                && next.Property == Property
                && next.Collection == Collection
                && Equals(next.Item, Item)
                && !(next.Added && Added)
                && !(next.Removed && Removed))
            {
                Added = false;
                Removed = false;
                return true;
            }
            return false;
        }
    }
}
