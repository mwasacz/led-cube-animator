using System;
using System.Collections.Generic;

namespace LedCubeAnimator.Model.Undo
{
    public class UndoManager : IUndoManager
    {
        private IAction _currentAction;
        private readonly Stack<IAction> _undoStack = new Stack<IAction>();
        private readonly Stack<IAction> _redoStack = new Stack<IAction>();

        public event EventHandler<ActionExecutedEventArgs> ActionExecuted;

        public bool CanUndo => _undoStack.Count > 0;
        public bool CanRedo => _redoStack.Count > 0;

        public void RecordAction(IAction action)
        {
            if (_currentAction == null)
            {
                _currentAction = action;
            }
            else if (!_currentAction.TryMerge(action))
            {
                throw new InvalidOperationException("Cannot merge with previous unfinished action");
            }

            _redoStack.Clear();
        }

        public void FinishAction(bool allowMerge = false)
        {
            if (_currentAction == null)
            {
                throw new InvalidOperationException("There is no action to finish");
            }

            var action = _currentAction;

            if (action.IsEmpty)
            {
                _currentAction = null;
            }
            else
            {
                if (!(allowMerge && CanUndo && _undoStack.Peek().TryMerge(action)))
                {
                    _undoStack.Push(action);
                }
                else if (_undoStack.Peek().IsEmpty)
                {
                    _undoStack.Pop();
                }

                _currentAction = null;

                action.Do();
                ActionExecuted?.Invoke(this, new ActionExecutedEventArgs(action, false));
            }
        }

        public void CancelAction()
        {
            if (_currentAction == null)
            {
                throw new InvalidOperationException("There is no action to cancel");
            }

            _currentAction = null;
        }

        public void Undo()
        {
            if (_currentAction != null)
            {
                throw new InvalidOperationException("The previous action has not been finished");
            }
            if (!CanUndo)
            {
                throw new InvalidOperationException("There is nothing to undo");
            }

            var action = _undoStack.Pop();
            _redoStack.Push(action);

            action.Undo();
            ActionExecuted?.Invoke(this, new ActionExecutedEventArgs(action, true));
        }

        public void Redo()
        {
            if (!CanRedo)
            {
                throw new InvalidOperationException("There is nothing to redo");
            }

            var action = _redoStack.Pop();
            _undoStack.Push(action);

            action.Do();
            ActionExecuted?.Invoke(this, new ActionExecutedEventArgs(action, false));
        }

        public void Reset()
        {
            _currentAction = null;
            _undoStack.Clear();
            _redoStack.Clear();
        }
    }
}
