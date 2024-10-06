// This file is part of LED Cube Animator
// Copyright (C) 2019-2021, 2024 Mikolaj Wasacz
// SPDX-License-Identifier: GPL-3.0-only WITH GPL-3.0-linking-source-exception

namespace LedCubeAnimator.Model.Animations.Data
{
    public abstract class Delay : Tile
    {
        public double Value { get; set; }

        public bool WrapAround { get; set; }

        public bool Static { get; set; }

        protected double GetDelayedTime(double time, double distance)
        {
            if (Static)
            {
                time = Start;
            }

            time += distance * Value;

            if (WrapAround)
            {
                int length = GetLength();
                time = ((time - Start) % length + length) % length + Start;
            }

            return time;
        }
    }
}
