// This file is part of LED Cube Animator
// Copyright (C) 2019-2021, 2024 Mikolaj Wasacz
// SPDX-License-Identifier: GPL-3.0-only WITH GPL-3.0-linking-source-exception

using System;

namespace LedCubeAnimator.Model.Animations.Data
{
    public abstract class Effect : Tile
    {
        public bool Reverse { get; set; }
        public int RepeatCount { get; set; } = 1;
        public TimeInterpolation TimeInterpolation { get; set; }

        protected double Fraction(double time)
        {
            double frac = RepeatCount * (time - Start) / GetLength() % 1;

            if (Reverse)
            {
                frac = 1 - Math.Abs(frac * 2 - 1);
            }

            switch (TimeInterpolation)
            {
                case TimeInterpolation.Accelerate:
                    frac *= frac;
                    break;
                case TimeInterpolation.Decelerate:
                    frac = Math.Sqrt(frac);
                    break;
                case TimeInterpolation.Sine:
                    frac = (1 - Math.Cos(frac * Math.PI)) / 2;
                    break;
            }

            return frac;
        }
    }
}
