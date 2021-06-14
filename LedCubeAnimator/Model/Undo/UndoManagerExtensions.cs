using System;
using System.Collections.Generic;

namespace LedCubeAnimator.Model.Undo
{
    public static class UndoManagerExtensions
    {
        public static void Group(this UndoManager undo, Action action, bool allowMerge = false)
        {
            undo.RecordAction(new GroupAction());
            action();
            undo.FinishAction(allowMerge);
        }

        public static void Set(this UndoManager undo, object obj, string property, object newValue)
        {
            undo.RecordAction(new PropertyChangeAction(obj, property, newValue));
        }

        public static void Add<T>(this UndoManager undo, object obj, string property, ICollection<T> collection, T item)
        {
            undo.RecordAction(new CollectionChangeAction<T>(obj, property, collection, item, true));
        }

        public static void Remove<T>(this UndoManager undo, object obj, string property, ICollection<T> collection, T item)
        {
            undo.RecordAction(new CollectionChangeAction<T>(obj, property, collection, item, false));
        }

        public static void ChangeArray<T>(this UndoManager undo, object obj, string property, Array array, T newValue, params int[] indices)
        {
            undo.RecordAction(new ArrayChangeAction<T>(obj, property, array, newValue, indices));
        }
    }
}
