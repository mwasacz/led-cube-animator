// This file is part of LED Cube Animator
// Copyright (C) 2019-2021, 2024 Mikolaj Wasacz
// SPDX-License-Identifier: GPL-3.0-only WITH GPL-3.0-linking-source-exception

using System.Windows;
using System.Windows.Controls;

namespace LedCubeAnimator.View.Controls
{
    public class MultiSelectListBox : ListBox
    {
        static MultiSelectListBox()
        {
            SelectionModeProperty.OverrideMetadata(typeof(MultiSelectListBox), new FrameworkPropertyMetadata(SelectionMode.Extended));
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new MultiSelectListBoxItem();
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is MultiSelectListBoxItem;
        }
    }
}
