using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LedCubeAnimator.Model.Undo
{
    public class PropertyChangeAction : ObjectAction, IActionGroup
    {
        public PropertyChangeAction(object obj, string property, object newValue) : base(obj)
        {
            Property = property;
            NewValue = newValue;

            _propertyInfo = Object.GetType().GetProperty(Property);

            OldValue = _propertyInfo.GetValue(Object);
        }

        private PropertyInfo _propertyInfo;

        public string Property { get; }
        public object NewValue { get; private set; }
        public object OldValue { get; }

        public bool IsEmpty => OldValue != null ? OldValue.Equals(NewValue) : NewValue == null;

        public override void Do() => _propertyInfo.SetValue(Object, NewValue);

        public override void Undo() => _propertyInfo.SetValue(Object, OldValue);

        public bool TryAdd(IAction action)
        {
            if (action is PropertyChangeAction next
                && next.Object == Object
                && next.Property == Property
                && (next.OldValue == null ? NewValue == null : next.OldValue.Equals(NewValue)))
            {
                NewValue = next.NewValue;
                return true;
            }
            return false;
        }
    }
}
