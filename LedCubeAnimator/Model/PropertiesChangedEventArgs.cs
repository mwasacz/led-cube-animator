// This file is part of LED Cube Animator
// Copyright (C) 2019-2021, 2024 Mikolaj Wasacz
// SPDX-License-Identifier: GPL-3.0-only WITH GPL-3.0-linking-source-exception

using System;
using System.Collections.Generic;

namespace LedCubeAnimator.Model
{
    public class PropertiesChangedEventArgs : EventArgs
    {
        public PropertiesChangedEventArgs(IList<KeyValuePair<object, string>> changes)
        {
            Changes = changes;
        }

        public IList<KeyValuePair<object, string>> Changes { get; }
    }
}