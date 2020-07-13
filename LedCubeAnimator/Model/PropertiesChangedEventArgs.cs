using System;
using System.Collections.Generic;

namespace LedCubeAnimator.Model
{
    public class PropertiesChangedEventArgs : EventArgs
    {
        public PropertiesChangedEventArgs(IList<Change> changes)
        {
            Changes = changes;
        }

        public IList<Change> Changes { get; }

        public class Change
        {
            public Change(object obj, string property)
            {
                Object = obj;
                Property = property;
            }

            public object Object { get; }
            public string Property { get; }
        }
    }
}