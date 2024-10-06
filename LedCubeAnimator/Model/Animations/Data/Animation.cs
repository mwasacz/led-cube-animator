// This file is part of LED Cube Animator
// Copyright (C) 2019-2021, 2024 Mikolaj Wasacz
// SPDX-License-Identifier: GPL-3.0-only WITH GPL-3.0-linking-source-exception

using System.Windows.Media;

namespace LedCubeAnimator.Model.Animations.Data
{
    public class Animation : Group
    {
        public int Size { get; set; } = 1;
        public ColorMode ColorMode { get; set; }
        public Color MonoColor { get; set; }
        public int FrameDuration { get; set; } = 1;
    }
}
