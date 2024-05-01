using System.Numerics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace CoinClicker
{
    public class Display : FrameworkElement
    {

        private List<ImageBrush> coinBrushes;

        private float cointSizeScale = 2f;
        private float defaultCoinSizeScale = 2f;
        private float mouseDownScale = 0.95f;
        
        private int baseCoinSize = 100;
        
        private int subCoinLifeTimeInFrame = 100;
        private int upgradesLifeTimeInFrame = 100;

        private Rect coinBound;

        private Vector2 coinPosition;

        private ParticleSystem<double> coinClickAnimation;
        private ParticleSystem<double> upgradeAnimation;

        private AnimatedTextDrawer animatedBonusDrawing;

        private MainWindowViewModel mainWindowViewModel;
        private SolidColorBrush loadingBarBrush;
        private SolidColorBrush loadingBarBGBrush;

        MySoundPlayer autoClickSound;

        public IClickerLogic ClickerLogic { get => mainWindowViewModel.ClickerLogic; }

        public Display(MainWindowViewModel mainWindowViewModel)
        {
            this.mainWindowViewModel = mainWindowViewModel;

            coinBrushes = new List<ImageBrush>() {
                new ImageBrush(new BitmapImage(new Uri(Utility.COIN_PATH + "\\bronze.png"))),
                new ImageBrush(new BitmapImage(new Uri(Utility.COIN_PATH + "\\silver.png"))),
                new ImageBrush(new BitmapImage(new Uri(Utility.COIN_PATH + "\\gold.png"))),
                new ImageBrush(new BitmapImage(new Uri(Utility.COIN_PATH + "\\ruby.png"))),
            };

            coinBound = new Rect(ActualWidth / 2 - baseCoinSize / 2 * cointSizeScale, ActualHeight / 2 - baseCoinSize / 2 * cointSizeScale, baseCoinSize, baseCoinSize);
            coinPosition = new Vector2((float)(coinBound.X + coinBound.Width / 2f), (float)(coinBound.Y + coinBound.Height / 2f));

            coinClickAnimation = new ParticleSystem<double>(coinPosition, subCoinLifeTimeInFrame);
            upgradeAnimation = new ParticleSystem<double>(coinPosition, upgradesLifeTimeInFrame);

            animatedBonusDrawing = new();

            mainWindowViewModel.ClickerLogic.OnUpgradeClicked += OnUpgradeClicked;
            mainWindowViewModel.ClickerLogic.OnClicked += OnPlayerClicked;

            autoClickSound = new MySoundPlayer(Utility.AUTO_CLICK_SOUND_PATH, 5);

            loadingBarBrush = new SolidColorBrush(Color.FromRgb(5, 153, 0));
            loadingBarBGBrush = new SolidColorBrush(Color.FromRgb(210, 180, 140));

            CompositionTarget.Rendering += (o, e) => Loop();
        }

        private void Loop()
        {
            coinBound = new Rect(ActualWidth / 2 - baseCoinSize * cointSizeScale / 2, ActualHeight / 2 - baseCoinSize * cointSizeScale / 2, baseCoinSize * cointSizeScale, baseCoinSize * cointSizeScale);
            coinPosition = new Vector2((float)(coinBound.X + coinBound.Width / 2f), (float)(coinBound.Y + coinBound.Height / 2f));
            coinClickAnimation.StartPosition = coinPosition;
            upgradeAnimation.StartPosition = coinPosition;

            if (Mouse.LeftButton == MouseButtonState.Released)
            {
                cointSizeScale = defaultCoinSizeScale;
            }

            coinClickAnimation.Update();
            upgradeAnimation.Update();

            InvalidateVisual();
        }



        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            if (ClickerLogic.IsBonus)
            {
                animatedBonusDrawing.StartAnimation();
                animatedBonusDrawing.DrawText(drawingContext, new Point(ActualWidth / 2, ActualHeight / 2 - 200), ClickerLogic.Player.IncomeBonusMultiplier + "x", Brushes.Gold, Brushes.DarkOrange);
            }
            else
            {
                animatedBonusDrawing.EndAnimation();
            }

            drawingContext.DrawRectangle(coinBrushes[(int)ClickerLogic.Player.CoinLevel], null, coinBound);

            ParticleSystemDrawer.DrawDouble(upgradeAnimation, drawingContext, false);

            ParticleSystemDrawer.DrawDouble(coinClickAnimation, drawingContext, true);

            double barOffset = 52;

            Pen outerPen = new Pen(loadingBarBGBrush, 1);
            Pen loadPen = new Pen(loadingBarBrush, 8);
            loadPen.StartLineCap = PenLineCap.Round;
            loadPen.EndLineCap = PenLineCap.Round;

            drawingContext.DrawRoundedRectangle(null, outerPen, new Rect(barOffset - 5, ActualHeight - barOffset - 5, (ActualWidth - barOffset * 2 + 9), 11), 5, 5);
            drawingContext.DrawLine(loadPen, new Point(barOffset, ActualHeight-barOffset), new Point((ActualWidth - barOffset * 2) * ClickerLogic.LoadingBarPercentige + barOffset, ActualHeight - barOffset));
        }

        public void OnPlayerClicked(double value)
        {
            coinClickAnimation.AddInstance(value, true);
        }

        public void OnUpgradeClicked(IEnumerable<double> values, IEnumerable<int> times)
        {
            if(values.Count()>0)
                autoClickSound.Play();

            for (int i = 0; i < values.Count(); i++)
            {
                upgradeAnimation.AddInstance(values.ElementAt(i) * times.ElementAt(i), true);
            }
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            cointSizeScale *= mouseDownScale;

            mainWindowViewModel.ClickerLogic.Clicked();
        }
    }
}
