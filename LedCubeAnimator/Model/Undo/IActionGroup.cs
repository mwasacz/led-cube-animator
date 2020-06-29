using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LedCubeAnimator.Model.Undo
{
    public interface IActionGroup : IAction
    {
        bool TryAdd(IAction action);
        bool IsEmpty { get; }
    }
}
