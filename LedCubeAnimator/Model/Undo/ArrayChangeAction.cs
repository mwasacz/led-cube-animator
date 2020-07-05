using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LedCubeAnimator.Model.Undo
{
    public class ArrayChangeAction<T> : IAction
    {
        public ArrayChangeAction(Array array, T newValue, params int[] indices)
        {
            Array = array;

            var oldValue = (T)Array.GetValue(indices);
            if (!AreEqual(oldValue, newValue))
            {
                Changes.Add(new ArrayChange<T>(oldValue, newValue, indices));
            }
        }

        public Array Array { get; }
        public IList<ArrayChange<T>> Changes { get; } = new List<ArrayChange<T>>();

        public bool IsEmpty => Changes.Count == 0;

        public void Do()
        {
            foreach (var change in Changes)
            {
                Array.SetValue(change.NewValue, change.Indices);
            }
        }

        public void Undo()
        {
            foreach (var change in Changes)
            {
                Array.SetValue(change.OldValue, change.Indices);
            }
        }

        public bool TryMerge(IAction action)
        {
            if (action is ArrayChangeAction<T> next && next.Array == Array)
            {
                foreach (var newChange in next.Changes)
                {
                    var oldChange = Changes.SingleOrDefault(c => c.Indices.SequenceEqual(newChange.Indices));
                    if (oldChange != null)
                    {
                        Changes.Remove(oldChange);
                        if (!AreEqual(oldChange.OldValue, newChange.NewValue))
                        {
                            Changes.Add(new ArrayChange<T>(oldChange.OldValue, newChange.NewValue, oldChange.Indices));
                        }
                    }
                    else
                    {
                        Changes.Add(newChange);
                    }
                }
                return true;
            }
            return false;
        }

        private bool AreEqual(T obj1, T obj2) => obj1 == null ? obj2 == null : obj1.Equals(obj2);

        public class ArrayChange<U>
        {
            public ArrayChange(U oldValue, U newValue, int[] indices)
            {
                OldValue = oldValue;
                NewValue = newValue;
                Indices = indices;
            }

            public U OldValue { get; }
            public U NewValue { get; }
            public int[] Indices { get; }
        }
    }
}
