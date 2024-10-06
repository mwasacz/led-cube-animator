// This file is part of LED Cube Animator
// Copyright (C) 2019-2021, 2024 Mikolaj Wasacz
// SPDX-License-Identifier: GPL-3.0-only WITH GPL-3.0-linking-source-exception

namespace LedCubeAnimator.Model.Animations.Data
{
    public enum ColorMode { Mono, MonoBrightness, RGB }

    public enum TimeInterpolation { Linear, Accelerate, Decelerate, Sine }

    public enum ColorBlendMode { Add, Multiply, Min, Max, Average }

    public enum ColorInterpolation { RGB, HSV, LongHSV }

    public enum Axis { X, Y, Z }

    public enum Plane { XY, XZ, YX, YZ, ZX, ZY }
}