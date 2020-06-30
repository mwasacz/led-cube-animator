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
        private Stack<IAction> _undoStack = new Stack<IAction>();
        private Stack<IAction> _redoStack = new Stack<IAction>();

        public event EventHandler<ActionExecutedEventArgs> ActionExecuted;

        public bool CanUndo => _undoStack.Count > 0 || (_currentAction != null && !_currentAction.IsEmpty);
        public bool CanRedo => _redoStack.Count > 0;

        public void RecordAction(IAction action, bool tryMerge = true)
        {
            _redoStack.Clear();
            action.Do();

            if (_currentAction != null)
            {
                if (!(tryMerge && _currentAction.TryMerge(action)))
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

            ActionExecuted?.Invoke(this, new ActionExecutedEventArgs(action, false));
        }

        public void Undo()
        {
            if (!CanUndo)
            {
                throw new InvalidOperationException("There is nothing to undo");
            }

            var action = _currentAction ?? _undoStack.Pop();
            _currentAction = null;
            action.Undo();
            _redoStack.Push(action);

            ActionExecuted?.Invoke(this, new ActionExecutedEventArgs(action, true));
        }

        public void Redo()
        {
            if (!CanRedo)
            {
                throw new InvalidOperationException("There is nothing to redo");
            }

            var action = _redoStack.Pop();
            action.Do();
            _undoStack.Push(action);

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
