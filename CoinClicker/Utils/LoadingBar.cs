namespace CoinClicker
{
    public class LoadingBar
    {
        private double minValue;
        private double maxValue;
        private double value;

        public double MinValue { get => minValue; set => minValue = value; }
        public double MaxValue { get => maxValue; set => maxValue = value; }
        public double Value { get => value; set => this.value = value; }

        public LoadingBar(double minValue, double maxValue)
        {
            this.minValue = minValue;
            this.maxValue = maxValue;
        }

        public LoadingBar() : this(0, 1) { }
    }
}
