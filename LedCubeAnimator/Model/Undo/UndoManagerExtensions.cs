using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LedCubeAnimator.Model.Undo
{
    public static class UndoManagerExtensions
    {
        public static GroupAction Group(this UndoManager undo, bool allowMerge)
        {
            var group = new GroupAction();
            undo.RecordAction(group, allowMerge);
            return group;
        }

        public static void Set(this UndoManager undo, object obj, string name, object newValue, bool allowMerge)
        {
            undo.RecordAction(new PropertyChangeAction(obj, name, newValue), allowMerge);
        }

        public static void Add<T>(this UndoManager undo, ICollection<T> collection, T item, bool allowMerge)
        {
            undo.RecordAction(new CollectionChangeAction<T>(collection, item, true), allowMerge);
        }

        public static void Remove<T>(this UndoManager undo, ICollection<T> collection, T item, bool allowMerge)
        {
            undo.RecordAction(new CollectionChangeAction<T>(collection, item, false), allowMerge);
        }

        public static void ChangeArray<T>(this UndoManager undo, Array array, T newValue, bool allowMerge, params int[] indices)
        {
            undo.RecordAction(new ArrayChangeAction<T>(array, newValue, indices), allowMerge);
        }
    }
}
