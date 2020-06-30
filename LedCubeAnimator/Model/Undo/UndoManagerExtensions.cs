using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LedCubeAnimator.Model.Undo
{
    public static class UndoManagerExtensions
    {
        public static ActionGroup Group(this UndoManager undo)
        {
            var group = new ActionGroup();
            undo.RecordAction(group);
            return group;
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

        public static void ChangeArray(this UndoManager undo, Array array, object newValue, params int[] indices)
        {
            undo.RecordAction(new ArrayChangeAction(array, newValue, indices));
        }
    }
}
