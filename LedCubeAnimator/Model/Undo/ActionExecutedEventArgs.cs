// This file is part of LED Cube Animator
// Copyright (C) 2019-2021, 2024 Mikolaj Wasacz
// SPDX-License-Identifier: GPL-3.0-only WITH GPL-3.0-linking-source-exception

using System;

namespace LedCubeAnimator.Model.Undo
{
    public class ActionExecutedEventArgs : EventArgs
    {
        public ActionExecutedEventArgs(IAction action, bool undone)
        {
            Action = action;
            Undone = undone;
        }

        public IAction Action { get; }
        public bool Undone { get; }
    }
}
