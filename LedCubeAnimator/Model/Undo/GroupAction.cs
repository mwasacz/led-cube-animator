using System.Collections.Generic;
using System.Linq;

namespace LedCubeAnimator.Model.Undo
{
    public class GroupAction : IAction
    {
        public IList<IAction> Actions { get; } = new List<IAction>();

        public bool IsEmpty => Actions.All(a => a.IsEmpty);

        public void Do()
        {
            RemoveEmptyActions();
            foreach (var a in Actions)
            {
                a.Do();
            }
        }

        public void Undo()
        {
            RemoveEmptyActions();
            foreach (var a in Actions.Reverse())
            {
                a.Undo();
            }
        }

        public bool TryMerge(IAction action)
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

        private void RemoveEmptyActions()
        {
            for (int i = Actions.Count - 1; i >= 0; i--)
            {
                if (Actions[i].IsEmpty)
                {
                    Actions.RemoveAt(i);
                }
            }
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
