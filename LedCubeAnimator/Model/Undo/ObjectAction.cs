using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LedCubeAnimator.Model.Undo
{
    public abstract class ObjectAction : IAction
    {
        public ObjectAction(object obj)
        {
            Object = obj;
        }

        public object Object { get; }

        public abstract void Do();

        public abstract void Undo();
    }
}
