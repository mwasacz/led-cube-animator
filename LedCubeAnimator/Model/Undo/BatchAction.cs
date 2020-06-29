using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LedCubeAnimator.Model.Undo
{
    public class BatchAction : IActionGroup, IDisposable
    {
        public List<IAction> Actions { get; } = new List<IAction>();

        public bool IsFinished { get; private set; }

        public bool IsEmpty => Actions.All(a => a is IActionGroup g && g.IsEmpty);

        public void Do()
        {
            foreach (var a in Actions)
            {
                a.Do();
            }
        }

        public void Undo()
        {
            foreach (var a in Actions.Reverse<IAction>())
            {
                a.Undo();
            }
        }

        public bool TryAdd(IAction action)
        {
            if (!IsFinished)
            {
                if (!(Actions.LastOrDefault() is IActionGroup group && group.TryAdd(action)))
                {
                    Actions.Add(action);
                }
                return true;
            }
            return false;
        }

        public void Dispose()
        {
            IsFinished = true;
        }
    }
}
