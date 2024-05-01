namespace CoinClicker
{
    public class LoadingBarTimeManager
    {
        private Timer timer;
        private LoadingBar loadingBar;
        private int secondToFill;
        private double step;
        private int millisToUpdate;

        public LoadingBar LoadingBar { get => loadingBar; set => loadingBar = value; }
        public int SecondToFill { get => secondToFill; set => secondToFill = value; }
        public double Step { get => step; set => step = value; }
        public int MillisToUpdate { get => millisToUpdate; set => millisToUpdate = value; }

        public LoadingBarTimeManager(int millisToUpdate, int secondToFill, Action<double> percentageOnTick, Action finished)
        {
            loadingBar = new LoadingBar();
            this.secondToFill = secondToFill;
         
            step = millisToUpdate / (secondToFill * 1000.0);

            timer = new Timer((o) =>
            {
                percentageOnTick?.Invoke(loadingBar.Value);
                loadingBar.Value += step;
                if (loadingBar.Value >= loadingBar.MaxValue)
                {
                    finished?.Invoke();
                    loadingBar.Value = loadingBar.MinValue;
                }
            }, null, 0, millisToUpdate);
        }
    }
}
