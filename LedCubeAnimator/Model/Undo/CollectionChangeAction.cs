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
            if (add)
            {
                Additions.Add(item);
            }
            else
            {
                Removals.Add(item);
            }
        }

        public ICollection<T> Collection { get; }
        public ICollection<T> Additions { get; } = new HashSet<T>();
        public ICollection<T> Removals { get; } = new HashSet<T>();

        public bool IsEmpty => Additions.Count == 0 && Removals.Count == 0;

        public void Do()
        {
            foreach (var item in Removals)
            {
                Collection.Remove(item);
            }
            foreach (var item in Additions)
            {
                Collection.Add(item);
            }
        }

        public void Undo()
        {
            foreach (var item in Additions)
            {
                Collection.Remove(item);
            }
            foreach (var item in Removals)
            {
                Collection.Add(item);
            }
        }

        public bool TryMerge(IAction action)
        {
            if (action is CollectionChangeAction<T> next && next.Collection == Collection)
            {
                foreach (var item in next.Removals)
                {
                    Additions.Remove(item);
                    Removals.Add(item);
                }
                foreach (var item in next.Additions)
                {
                    Removals.Remove(item);
                    Additions.Add(item);
                }
                return true;
            }
            return false;
        }
    }
}
