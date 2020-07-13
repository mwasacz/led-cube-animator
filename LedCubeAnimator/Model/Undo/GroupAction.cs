using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LedCubeAnimator.Model.Undo
{
    public class GroupAction : IAction
    {
        public IList<IAction> Actions { get; } = new List<IAction>();

        public bool IsFinished { get; private set; }

        public bool IsEmpty => Actions.All(a => a.IsEmpty);

        public void Do()
        {
            foreach (var a in Actions)
            {
                a.Do();
            }
        }

        public void Undo()
        {
            foreach (var a in Actions.Reverse())
            {
                a.Undo();
            }
        }

        public bool TryMerge(IAction action)
        {
            if (!IsFinished)
            {
                if (action is GroupAction group)
                {
                    foreach (var a in group.Actions)
                    {
                        AddAction(a);
                    }
                }
                else
                {
                    AddAction(action);
                }
                return true;
            }
            return false;
        }

        public void Finish()
        {
            IsFinished = true;
        }

        private void AddAction(IAction action)
        {
            foreach (var a in Actions.Reverse())
            {
                if (a.TryMerge(action))
                {
                    return;
                }
            }
            Actions.Add(action);
        }
    }
}
