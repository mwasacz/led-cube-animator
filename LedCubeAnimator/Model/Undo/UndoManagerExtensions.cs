using System;
using System.Collections.Generic;

namespace LedCubeAnimator.Model.Undo
{
    public static class UndoManagerExtensions
    {
        public static void Group(this IUndoManager undo, Action action, bool allowMerge = false)
        {
            undo.RecordAction(new GroupAction());
            action();
            undo.FinishAction(allowMerge);
        }

        public static void Set(this IUndoManager undo, object obj, string name, object newValue)
        {
            undo.RecordAction(new PropertyChangeAction(obj, name, newValue));
        }

        public static void Add<T>(this IUndoManager undo, ICollection<T> collection, T item)
        {
            undo.RecordAction(new CollectionChangeAction<T>(collection, item, true));
        }

        public static void Remove<T>(this IUndoManager undo, ICollection<T> collection, T item)
        {
            undo.RecordAction(new CollectionChangeAction<T>(collection, item, false));
        }

        public static void ChangeArray<T>(this IUndoManager undo, Array array, T newValue, params int[] indices)
        {
            undo.RecordAction(new ArrayChangeAction<T>(array, newValue, indices));
        }
    }
}
