using System;
using System.Collections.Generic;

namespace LedCubeAnimator.Model
{
    public class PropertiesChangedEventArgs : EventArgs
    {
        public PropertiesChangedEventArgs(IList<KeyValuePair<object, string>> changes)
        {
            Changes = changes;
        }

        public IList<KeyValuePair<object, string>> Changes { get; }
    }
}