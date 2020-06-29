using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LedCubeAnimator.Model.Undo
{
    public class ArrayChangeAction : ObjectAction, IActionGroup
    {
        public ArrayChangeAction(object obj, Array array, object newValue, params int[] indices) : base(obj)
        {
            Array = array;

            var oldValue = Array.GetValue(indices);
            if (!AreEqual(oldValue, newValue))
            {
                Changes.Add(new ArrayChange(oldValue, newValue, indices));
            }
        }

        public Array Array { get; }
        public List<ArrayChange> Changes { get; } = new List<ArrayChange>();

        public bool IsEmpty => Changes.Count == 0;

        public override void Do()
        {
            foreach (var change in Changes)
            {
                Array.SetValue(change.NewValue, change.Indices);
            }
        }

        public override void Undo()
        {
            foreach (var change in Changes)
            {
                Array.SetValue(change.OldValue, change.Indices);
            }
        }

        public bool TryAdd(IAction action)
        {
            if (action is ArrayChangeAction next && next.Array == Array)
            {
                foreach (var newChange in next.Changes)
                {
                    var oldChange = Changes.SingleOrDefault(c => c.Indices.SequenceEqual(newChange.Indices));
                    if (oldChange != null)
                    {
                        Changes.Remove(oldChange);
                        if (!AreEqual(oldChange.OldValue, newChange.NewValue))
                        {
                            Changes.Add(new ArrayChange(oldChange.OldValue, newChange.NewValue, oldChange.Indices));
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

        private bool AreEqual(object obj1, object obj2) => obj1 == null ? obj2 == null : obj1.Equals(obj2);

        public class ArrayChange
        {
            public ArrayChange(object oldValue, object newValue, int[] indices)
            {
                OldValue = oldValue;
                NewValue = newValue;
                Indices = indices;
            }

            public object OldValue { get; }
            public object NewValue { get; }
            public int[] Indices { get; }
        }
    }
}
