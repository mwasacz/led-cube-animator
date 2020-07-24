using System;

namespace LedCubeAnimator.Model.Undo
{
    public interface IUndoManager
    {
        event EventHandler<ActionExecutedEventArgs> ActionExecuted;

        bool CanUndo { get; }
        bool CanRedo { get; }
        
        void RecordAction(IAction action);
        void FinishAction(bool allowMerge = false);
        void CancelAction();
        void Undo();
        void Redo();
        void Reset();
    }
}