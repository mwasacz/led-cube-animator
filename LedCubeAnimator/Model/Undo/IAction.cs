// This file is part of LED Cube Animator
// Copyright (C) 2019-2021, 2024 Mikolaj Wasacz
// SPDX-License-Identifier: GPL-3.0-only WITH GPL-3.0-linking-source-exception

namespace LedCubeAnimator.Model.Undo
{
    public interface IAction
    {
        bool IsEmpty { get; }
        void Do();
        void Undo();
        bool TryMerge(IAction action);
    }
}
