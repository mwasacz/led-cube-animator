// This file is part of LED Cube Animator
// Copyright (C) 2019-2021, 2024 Mikolaj Wasacz
// SPDX-License-Identifier: GPL-3.0-only WITH GPL-3.0-linking-source-exception

using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Interactivity;
using Xceed.Wpf.Toolkit.PropertyGrid;

namespace LedCubeAnimator.View.Behaviors
{
    public class PropertyOrderOverride : Behavior<PropertyGrid>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.SelectedObjectChanged += AssociatedObject_SelectedObjectChanged;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.SelectedObjectChanged -= AssociatedObject_SelectedObjectChanged;
            base.OnDetaching();
        }

        private void AssociatedObject_SelectedObjectChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var collectionView = CollectionViewSource.GetDefaultView(AssociatedObject.Properties);
            if (collectionView?.CanSort == true)
            {
                collectionView.SortDescriptions.Insert(0, new SortDescription(nameof(CustomPropertyItem.PropertyOrder), ListSortDirection.Ascending));
            }
        }
    }
}
