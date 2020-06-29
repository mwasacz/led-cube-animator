using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LedCubeAnimator.Model.Undo
{
    public class CollectionAddAction<T> : CollectionChangeAction<T>
    {
        public CollectionAddAction(object obj, ICollection<T> collection, T item) : base(obj, collection, item) { }

        public override void Do() => Collection.Add(Item);

        public override void Undo() => Collection.Remove(Item);
    }
}
