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
