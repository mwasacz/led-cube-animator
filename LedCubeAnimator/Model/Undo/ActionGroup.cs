using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LedCubeAnimator.Model.Undo
{
    public class ActionGroup : IAction
    {
        public IList<IAction> Actions { get; } = new List<IAction>();

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
            if (action is ActionGroup group)
            {
                foreach (var a in group.Actions)
                {
                    Actions.Add(a);
                }
            }
            else
            {
                Actions.Add(action);
            }
            return true;
        }
    }
}
