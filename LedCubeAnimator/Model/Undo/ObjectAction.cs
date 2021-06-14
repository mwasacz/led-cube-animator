namespace LedCubeAnimator.Model.Undo
{
    public abstract class ObjectAction : IAction
    {
        public ObjectAction(object obj, string property)
        {
            Object = obj;
            Property = property;
        }

        public object Object { get; }
        public string Property { get; }

        public abstract bool IsEmpty { get; }

        public abstract void Do();
        public abstract void Undo();
        public abstract bool TryMerge(IAction action);
    }
}
