// This file is part of LED Cube Animator
// Copyright (C) 2019-2021, 2024 Mikolaj Wasacz
// SPDX-License-Identifier: GPL-3.0-only WITH GPL-3.0-linking-source-exception

using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace LedCubeAnimator.View.Behaviors
{
    public class RoutedCommandBinding : Behavior<FrameworkElement>
    {
        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register("Command",
            typeof(ICommand), typeof(RoutedCommandBinding));

        public ICommand RoutedCommand { get; set; }

        private CommandBinding _binding;

        protected override void OnAttached()
        {
            base.OnAttached();
            _binding = new CommandBinding(RoutedCommand, HandleExecuted, HandleCanExecute);
            AssociatedObject.CommandBindings.Add(_binding);
        }

        protected override void OnDetaching()
        {
            AssociatedObject.CommandBindings.Remove(_binding);
            base.OnDetaching();
        }

        private void HandleExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Command?.Execute(e.Parameter);
            e.Handled = true;
        }

        private void HandleCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = Command?.CanExecute(e.Parameter) == true;
            e.Handled = true;
        }
    }
}
