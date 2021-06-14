using System;
using System.Reflection;

namespace LedCubeAnimator.Model.Undo
{
    public class PropertyChangeAction : ObjectAction
    {
        public PropertyChangeAction(object obj, string property, object newValue) : base(obj, property)
        {
            NewValue = newValue;

            _propertyInfo = Object.GetType().GetProperty(Property);

            OldValue = _propertyInfo.GetValue(Object);
        }

        private readonly PropertyInfo _propertyInfo;

        public object NewValue { get; private set; }
        public object OldValue { get; }

        public override bool IsEmpty => Equals(OldValue, NewValue);

        public override void Do() => _propertyInfo.SetValue(Object, NewValue);

        public override void Undo() => _propertyInfo.SetValue(Object, OldValue);

        public override bool TryMerge(IAction action)
        {
            if (action is PropertyChangeAction next
                && next.Object == Object
                && next.Property == Property
                && Equals(next.OldValue, NewValue))
            {
                NewValue = next.NewValue;
                return true;
            }
            return false;
        }
    }
}
