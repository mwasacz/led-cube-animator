// This file is part of LED Cube Animator
// Copyright (C) 2019-2021, 2024 Mikolaj Wasacz
// SPDX-License-Identifier: GPL-3.0-only WITH GPL-3.0-linking-source-exception

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace LedCubeAnimator.View
{
    public static class ScrollSynchronizer
    {
        public static readonly DependencyProperty VerticalScrollGroupProperty = DependencyProperty.RegisterAttached("VerticalScrollGroup",
            typeof(object), typeof(ScrollSynchronizer), new PropertyMetadata(OnVerticalScrollGroupChanged));

        public static void SetVerticalScrollGroup(DependencyObject obj, object verticalScrollGroup) => obj.SetValue(VerticalScrollGroupProperty, verticalScrollGroup);

        public static object GetVerticalScrollGroup(DependencyObject obj) => obj.GetValue(VerticalScrollGroupProperty);

        private static readonly Dictionary<object, ScrollGroup> verticalScrollGroups = new Dictionary<object, ScrollGroup>();

        private static void OnVerticalScrollGroupChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ScrollViewer scrollViewer)
            {
                scrollViewer.ScrollChanged -= ScrollViewer_VerticalScrollChanged;

                object oldGroupKey = e.OldValue;
                if (oldGroupKey != null && verticalScrollGroups.TryGetValue(oldGroupKey, out var oldGroup))
                {
                    oldGroup.ScrollViewers.RemoveAll(r => !r.TryGetTarget(out var s) || s == scrollViewer);
                    if (oldGroup.ScrollViewers.Count == 0)
                    {
                        verticalScrollGroups.Remove(oldGroupKey);
                    }
                }

                object newGroupKey = e.NewValue;
                if (newGroupKey != null)
                {
                    if (verticalScrollGroups.TryGetValue(newGroupKey, out var newGroup))
                    {
                        scrollViewer.ScrollToVerticalOffset(newGroup.Offset);
                    }
                    else
                    {
                        newGroup = new ScrollGroup(scrollViewer.VerticalOffset);
                        verticalScrollGroups.Add(newGroupKey, newGroup);
                    }
                    newGroup.ScrollViewers.Add(new WeakReference<ScrollViewer>(scrollViewer));
                    scrollViewer.ScrollChanged += ScrollViewer_VerticalScrollChanged;
                }
            }
        }

        private static void ScrollViewer_VerticalScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (e.VerticalChange != 0)
            {
                var scrollGroup = verticalScrollGroups[GetVerticalScrollGroup((ScrollViewer)sender)];
                scrollGroup.Offset = e.VerticalOffset;

                foreach (var reference in scrollGroup.ScrollViewers)
                {
                    if (reference.TryGetTarget(out var scrollViewer) && scrollViewer.VerticalOffset != scrollGroup.Offset)
                    {
                        scrollViewer.ScrollToVerticalOffset(scrollGroup.Offset);
                    }
                }
            }
        }



        public static readonly DependencyProperty HorizontalScrollGroupProperty = DependencyProperty.RegisterAttached("HorizontalScrollGroup",
            typeof(object), typeof(ScrollSynchronizer), new PropertyMetadata(OnHorizontalScrollGroupChanged));

        public static void SetHorizontalScrollGroup(DependencyObject obj, object horizontalScrollGroup) => obj.SetValue(HorizontalScrollGroupProperty, horizontalScrollGroup);

        public static object GetHorizontalScrollGroup(DependencyObject obj) => obj.GetValue(HorizontalScrollGroupProperty);

        private static readonly Dictionary<object, ScrollGroup> horizontalScrollGroups = new Dictionary<object, ScrollGroup>();

        private static void OnHorizontalScrollGroupChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ScrollViewer scrollViewer)
            {
                scrollViewer.ScrollChanged -= ScrollViewer_HorizontalScrollChanged;

                object oldGroupKey = e.OldValue;
                if (oldGroupKey != null && horizontalScrollGroups.TryGetValue(oldGroupKey, out var oldGroup))
                {
                    oldGroup.ScrollViewers.RemoveAll(r => !r.TryGetTarget(out var s) || s == scrollViewer);
                    if (oldGroup.ScrollViewers.Count == 0)
                    {
                        horizontalScrollGroups.Remove(oldGroupKey);
                    }
                }

                object newGroupKey = e.NewValue;
                if (newGroupKey != null)
                {
                    if (horizontalScrollGroups.TryGetValue(newGroupKey, out var newGroup))
                    {
                        scrollViewer.ScrollToHorizontalOffset(newGroup.Offset);
                    }
                    else
                    {
                        newGroup = new ScrollGroup(scrollViewer.HorizontalOffset);
                        horizontalScrollGroups.Add(newGroupKey, newGroup);
                    }
                    newGroup.ScrollViewers.Add(new WeakReference<ScrollViewer>(scrollViewer));
                    scrollViewer.ScrollChanged += ScrollViewer_HorizontalScrollChanged;
                }
            }
        }

        private static void ScrollViewer_HorizontalScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (e.HorizontalChange != 0)
            {
                var scrollGroup = horizontalScrollGroups[GetHorizontalScrollGroup((ScrollViewer)sender)];
                scrollGroup.Offset = e.HorizontalOffset;

                foreach (var reference in scrollGroup.ScrollViewers)
                {
                    if (reference.TryGetTarget(out var scrollViewer) && scrollViewer.HorizontalOffset != scrollGroup.Offset)
                    {
                        scrollViewer.ScrollToHorizontalOffset(scrollGroup.Offset);
                    }
                }
            }
        }



        private class ScrollGroup
        {
            public ScrollGroup(double offset)
            {
                Offset = offset;
            }

            public double Offset { get; set; }
            public List<WeakReference<ScrollViewer>> ScrollViewers { get; } = new List<WeakReference<ScrollViewer>>();
        }
    }
}
