using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LedCubeAnimator.Model.Undo
{
    public class UndoManager
    {
        private IAction _currentAction;
        private readonly Stack<IAction> _undoStack = new Stack<IAction>();
        private readonly Stack<IAction> _redoStack = new Stack<IAction>();

        public event EventHandler<ActionExecutedEventArgs> ActionExecuted;

        public bool CanUndo => _undoStack.Count > 0 || _currentAction?.IsEmpty == false;
        public bool CanRedo => _redoStack.Count > 0;

        public void RecordAction(IAction action, bool allowMerge = true)
        {
            _redoStack.Clear();

            if (_currentAction != null)
            {
                if (!(allowMerge && _currentAction.TryMerge(action)))
                {
                    if (!_currentAction.IsEmpty)
                    {
                        _undoStack.Push(_currentAction);
                    }
                    _currentAction = action;
                }
            }
            else
            {
                _currentAction = action;
            }

            action.Do();
            ActionExecuted?.Invoke(this, new ActionExecutedEventArgs(action, false));
        }

        public void FinishAction()
        {
            if (_currentAction?.IsEmpty == false)
            {
                _undoStack.Push(_currentAction);
            }
            _currentAction = null;
        }

        public void Undo()
        {
            if (!CanUndo)
            {
                throw new InvalidOperationException("There is nothing to undo");
            }

            var action = _currentAction?.IsEmpty == false ? _currentAction : _undoStack.Pop();
            _currentAction = null;
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
