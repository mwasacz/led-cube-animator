using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LedCubeAnimator.Model.Undo
{
    public interface IAction
    {
        bool IsEmpty { get; }
        void Do();
        void Undo();
        bool TryMerge(IAction action);
    }
}
