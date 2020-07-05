using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LedCubeAnimator.Model.Undo
{
    public static class UndoManagerExtensions
    {
        public static void Group(this UndoManager undo)
        {
            undo.RecordAction(new ActionGroup());
        }

        public static void Set(this UndoManager undo, object obj, string name, object newValue)
        {
            undo.RecordAction(new PropertyChangeAction(obj, name, newValue));
        }

        public static void Add<T>(this UndoManager undo, ICollection<T> collection, T item)
        {
            undo.RecordAction(new CollectionChangeAction<T>(collection, item, true));
        }

        public static void Remove<T>(this UndoManager undo, ICollection<T> collection, T item)
        {
            undo.RecordAction(new CollectionChangeAction<T>(collection, item, false));
        }

        public static void ChangeArray<T>(this UndoManager undo, Array array, T newValue, params int[] indices)
        {
            undo.RecordAction(new ArrayChangeAction<T>(array, newValue, indices));
        }
    }
}
