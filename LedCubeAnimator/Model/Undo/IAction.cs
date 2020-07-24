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
