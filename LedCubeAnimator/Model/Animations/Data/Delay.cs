namespace LedCubeAnimator.Model.Animations.Data
{
    public abstract class Delay : Tile
    {
        public double Value { get; set; }

        public bool WrapAround { get; set; }

        public bool Static { get; set; }

        protected double GetDelayedTime(double time, double distance)
        {
            if (Static)
            {
                time = Start;
            }

            time += distance * Value;

            if (WrapAround)
            {
                int length = GetLength();
                time = ((time - Start) % length + length) % length + Start;
            }

            return time;
        }
    }
}
