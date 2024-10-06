// This file is part of LED Cube Animator
// Copyright (C) 2019-2021, 2024 Mikolaj Wasacz
// SPDX-License-Identifier: GPL-3.0-only WITH GPL-3.0-linking-source-exception

using System;
using System.Windows.Media;

namespace LedCubeAnimator.ViewModel
{
    public class ColorClickedEventArgs : EventArgs
    {
        public ColorClickedEventArgs(Color color)
        {
            Color = color;
        }

        public Color Color { get; }
    }
}