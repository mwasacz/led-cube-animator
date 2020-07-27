using System;
using System.Reflection;

namespace LedCubeAnimator.Model.Undo
{
    public class PropertyChangeAction : IAction
    {
        public PropertyChangeAction(object obj, string property, object newValue)
        {
            Object = obj;
            Property = property;
            NewValue = newValue;

            _propertyInfo = Object.GetType().GetProperty(Property);

            OldValue = _propertyInfo.GetValue(Object);
        }

        private PropertyInfo _propertyInfo;

        public object Object { get; }
        public string Property { get; }
        public object NewValue { get; private set; }
        public object OldValue { get; }

        public bool IsEmpty => AreEqual(OldValue, NewValue);

        public void Do() => _propertyInfo.SetValue(Object, NewValue);

        public void Undo() => _propertyInfo.SetValue(Object, OldValue);

        public bool TryMerge(IAction action)
        {
            if (action is PropertyChangeAction next
                && next.Object == Object
                && next.Property == Property
                && AreEqual(next.OldValue, NewValue))
            {
                NewValue = next.NewValue;
                return true;
            }
            return false;
        }

        private bool AreEqual(object obj1, object obj2) => obj1 == null ? obj2 == null : obj1.Equals(obj2);
    }
}
