using PropertyTools.Wpf;
using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Interactivity;
using PropertyGrid = Xceed.Wpf.Toolkit.PropertyGrid.PropertyGrid;

namespace LedCubeAnimator.View.Behaviors
{
    public class MultiObjectEditor : Behavior<PropertyGrid>
    {
        public IEnumerable SelectedObjects
        {
            get => (IEnumerable)GetValue(SelectedObjectsProperty);
            set => SetValue(SelectedObjectsProperty, value);
        }

        public static readonly DependencyProperty SelectedObjectsProperty = DependencyProperty.Register("SelectedObjects",
            typeof(IEnumerable), typeof(MultiObjectEditor), new PropertyMetadata(OnSelectedObjectsChanged));

        private static void OnSelectedObjectsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((MultiObjectEditor)d).OnSelectedObjectsChanged(e);

        private void OnSelectedObjectsChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue is INotifyCollectionChanged oldCollection)
            {
                CollectionChangedEventManager.RemoveHandler(oldCollection, OnSelectedObjectsCollectionChanged);
            }

            if (e.NewValue is INotifyCollectionChanged newCollection)
            {
                CollectionChangedEventManager.AddHandler(newCollection, OnSelectedObjectsCollectionChanged);
            }

            SetSelectedObject((IEnumerable)e.NewValue);
        }

        private void OnSelectedObjectsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            SetSelectedObject((IEnumerable)sender);
        }

        private void SetSelectedObject(IEnumerable enumerable)
        {
            var oldItemsBag = AssociatedObject.SelectedObject as ItemsBag;

            var objects = enumerable?.Cast<object>().ToArray();
            switch (objects?.Length)
            {
                case null:
                case 0:
                    AssociatedObject.SelectedObject = null;
                    AssociatedObject.SelectedObjectTypeName = null;
                    AssociatedObject.SelectedObjectName = null;
                    break;
                case 1:
                    AssociatedObject.SelectedObject = objects[0];
                    AssociatedObject.SelectedObjectTypeName = GetTypeName(objects[0].GetType());
                    AssociatedObject.SelectedObjectName = null;
                    break;
                default:
                    var itemsBag = new ItemsBag(objects);
                    AssociatedObject.SelectedObject = new CustomItemsBagTypeDescriptor(itemsBag);
                    AssociatedObject.SelectedObjectTypeName = GetTypeName(itemsBag.BiggestType);
                    AssociatedObject.SelectedObjectName = $"{objects.Length} tiles selected";
                    break;
            }

            oldItemsBag?.Dispose();
        }

        private string GetTypeName(Type type)
        {
            return type.GetCustomAttributes<DisplayNameAttribute>(false).FirstOrDefault()?.DisplayName ?? type.Name;
        }

        private class CustomItemsBagTypeDescriptor : CustomTypeDescriptor
        {
            public CustomItemsBagTypeDescriptor(ItemsBag itemsBag) : base(TypeDescriptor.GetProvider(itemsBag.BiggestType).GetTypeDescriptor(itemsBag.BiggestType))
            {
                _itemsBag = itemsBag;
            }

            private readonly ItemsBag _itemsBag;

            public override object GetPropertyOwner(PropertyDescriptor pd) => _itemsBag;

            public override PropertyDescriptorCollection GetProperties() => GetCustomPropertyDescriptors(base.GetProperties());

            public override PropertyDescriptorCollection GetProperties(Attribute[] attributes) => GetCustomPropertyDescriptors(base.GetProperties(attributes));

            private PropertyDescriptorCollection GetCustomPropertyDescriptors(PropertyDescriptorCollection properties)
            {
                return new PropertyDescriptorCollection(properties
                    .Cast<PropertyDescriptor>()
                    .Select(p => new CustomItemsBagPropertyDescriptor(p))
                    .ToArray());
            }
        }

        private class CustomItemsBagPropertyDescriptor : ItemsBagPropertyDescriptor
        {
            public CustomItemsBagPropertyDescriptor(PropertyDescriptor defaultDescriptor) : base(defaultDescriptor, defaultDescriptor.ComponentType)
            {
                _defaultDescriptor = defaultDescriptor;
            }

            private readonly PropertyDescriptor _defaultDescriptor;

            public override bool IsReadOnly => _defaultDescriptor.IsReadOnly;

            public override Type PropertyType => _defaultDescriptor.PropertyType;

            public override bool ShouldSerializeValue(object component) => _defaultDescriptor.ShouldSerializeValue(component);
        }
    }
}
