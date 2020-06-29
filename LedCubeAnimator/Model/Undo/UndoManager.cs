using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LedCubeAnimator.Model.Undo
{
    public class UndoManager
    {
        private IAction _lastAction;
        private Stack<IAction> _undoStack = new Stack<IAction>();
        private Stack<IAction> _redoStack = new Stack<IAction>();

        public event EventHandler<ActionExecutedEventArgs> ActionExecuted;

        public bool CanUndo => _undoStack.Count > 0 || _lastAction != null;
        public bool CanRedo => _redoStack.Count > 0;

        public void RecordAction(IAction action)
        {
            _redoStack.Clear();
            action.Do();

            var group = _lastAction as IActionGroup;
            if (group == null || !group.TryAdd(action))
            {
                if (_lastAction != null && (group == null || !group.IsEmpty))
                {
                    _undoStack.Push(_lastAction);
                }
                _lastAction = action;
            }

            ActionExecuted?.Invoke(this, new ActionExecutedEventArgs(action, false));
        }

        public void Undo()
        {
            if (!CanUndo)
            {
                throw new InvalidOperationException("There is nothing to undo");
            }

            var action = _lastAction ?? _undoStack.Pop();
            _lastAction = null;
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
            _lastAction = null;
            _undoStack.Clear();
            _redoStack.Clear();
        }
    }
}
