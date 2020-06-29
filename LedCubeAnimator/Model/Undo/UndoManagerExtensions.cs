using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LedCubeAnimator.Model.Undo
{
    public static class UndoManagerExtensions
    {
        public static BatchAction Batch(this UndoManager undo)
        {
            var batch = new BatchAction();
            undo.RecordAction(batch);
            return batch;
        }

        public static void Set(this UndoManager undo, object obj, string name, object newValue)
        {
            var action = new PropertyChangeAction(obj, name, newValue);
            if (!action.IsEmpty)
            {
                undo.RecordAction(action);
            }
        }

        public static void Add<T>(this UndoManager undo, object obj, ICollection<T> collection, T item)
        {
            undo.RecordAction(new CollectionAddAction<T>(obj, collection, item));
        }

        public static void Remove<T>(this UndoManager undo, object obj, ICollection<T> collection, T item)
        {
            undo.RecordAction(new CollectionRemoveAction<T>(obj, collection, item));
        }

        public static void ChangeArray(this UndoManager undo, object obj, Array array, object newValue, params int[] indices)
        {
            var action = new ArrayChangeAction(obj, array, newValue, indices);
            if (!action.IsEmpty)
            {
                undo.RecordAction(action);
            }
        }
    }
}
