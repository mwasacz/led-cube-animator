// This file is part of LED Cube Animator
// Copyright (C) 2019-2021, 2024 Mikolaj Wasacz
// SPDX-License-Identifier: GPL-3.0-only WITH GPL-3.0-linking-source-exception

using System;

namespace LedCubeAnimator.Utils
{
    public class ItemDraggedEventArgs : EventArgs
    {
        public ItemDraggedEventArgs(object item, DragMode dragMode, int positionX, int positionY, int handleOffset)
        {
            Item = item;
            DragMode = dragMode;
            PositionX = positionX;
            PositionY = positionY;
            HandleOffset = handleOffset;
        }

        public object Item { get; }
        public DragMode DragMode { get; }
        public int PositionX { get; }
        public int PositionY { get; }
        public int HandleOffset { get; }
    }

    public enum DragMode { Left, Right, Move }
}
