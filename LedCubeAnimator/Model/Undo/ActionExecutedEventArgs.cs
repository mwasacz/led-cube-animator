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
