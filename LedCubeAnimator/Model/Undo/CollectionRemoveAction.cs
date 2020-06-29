using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LedCubeAnimator.Model.Undo
{
    public class CollectionRemoveAction<T> : CollectionChangeAction<T>
    {
        public CollectionRemoveAction(object obj, ICollection<T> collection, T item) : base(obj, collection, item) { }

        public override void Do() => Collection.Remove(Item);

        public override void Undo() => Collection.Add(Item);
    }
}
