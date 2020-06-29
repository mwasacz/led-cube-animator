using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LedCubeAnimator.Model.Undo
{
    public abstract class CollectionChangeAction<T> : ObjectAction
    {
        public CollectionChangeAction(object obj, ICollection<T> collection, T item) : base(obj)
        {
            Collection = collection;
            Item = item;
        }

        public ICollection<T> Collection { get; }
        public T Item { get; }
    }
}
