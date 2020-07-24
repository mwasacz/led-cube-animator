using System;
using System.Linq;

namespace LedCubeAnimator.Model.Undo
{
    public class ArrayChangeAction<T> : IAction
    {
        public ArrayChangeAction(Array array, T newValue, params int[] indices)
        {
            Array = array;
            NewValue = newValue;
            Indices = indices;

            OldValue = (T)Array.GetValue(indices);
        }

        public Array Array { get; }
        public T OldValue { get; }
        public T NewValue { get; private set; }
        public int[] Indices { get; }

        public bool IsEmpty => AreEqual(OldValue, NewValue);

        public void Do()
        {
            Array.SetValue(NewValue, Indices);
        }

        public void Undo()
        {
            Array.SetValue(OldValue, Indices);
        }

        public bool TryMerge(IAction action)
        {
            if (action is ArrayChangeAction<T> next
                && next.Array == Array
                && next.Indices.SequenceEqual(Indices)
                && AreEqual(next.OldValue, NewValue))
            {
                NewValue = next.NewValue;
                return true;
            }
            return false;
        }

        private bool AreEqual(T obj1, T obj2) => obj1 == null ? obj2 == null : obj1.Equals(obj2);
    }
}
