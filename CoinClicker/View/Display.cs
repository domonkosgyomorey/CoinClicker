using System.Numerics;
using System.Windows;
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

        private MySoundPlayer autoClickSound;

        private ImageBrush chestBrush;
        private ParticleSystem<double> chestCoinAnimation;
        private ParticleSystem<byte> chests;
        private int chestWidth = 50;
        private int chestHeight = 50;
        private int chestLives = 150;

        private double time;

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

            chestBrush = new ImageBrush(new BitmapImage(new Uri(Utility.TRESURE_CHEST_PATH)));
            DispatcherTimer chestTimer = new DispatcherTimer();
            Player player = mainWindowViewModel.ClickerLogic.Player;
            chests = new ParticleSystem<byte>(chestLives);
            chestTimer.Interval = TimeSpan.FromMilliseconds(Utility.random.Next(player.MinTimeChestSpawn, player.MaxTimeChestSpawn));
            chestTimer.Tick += (o, e) => {
                chests.AddInstance(0, new Vector2(Utility.random.Next(100, (int)ActualWidth)-100, Utility.random.Next(100, (int)ActualHeight-100)), Particle<byte>.MovementType.STATIC); 
                chestTimer.Interval = TimeSpan.FromMilliseconds(Utility.random.Next(player.MinTimeChestSpawn, player.MaxTimeChestSpawn));
            };
            chestTimer.Start();

            coinBound = new Rect(ActualWidth / 2 - baseCoinSize / 2 * cointSizeScale, ActualHeight / 2 - baseCoinSize / 2 * cointSizeScale, baseCoinSize, baseCoinSize);
            coinPosition = new Vector2((float)(coinBound.X + coinBound.Width / 2f), (float)(coinBound.Y + coinBound.Height / 2f));

            coinClickAnimation = new ParticleSystem<double>(subCoinLifeTimeInFrame);
            upgradeAnimation = new ParticleSystem<double>(upgradesLifeTimeInFrame);
            chestCoinAnimation = new ParticleSystem<double>(chestLives);

            animatedBonusDrawing = new();

            mainWindowViewModel.ClickerLogic.OnUpgradeClicked += OnUpgradeClicked;
            mainWindowViewModel.ClickerLogic.OnClicked += OnPlayerClicked;

            autoClickSound = new MySoundPlayer(Utility.AUTO_CLICK_SOUND_PATH, 5);

            loadingBarBrush = new SolidColorBrush(Color.FromRgb(5, 153, 0));
            loadingBarBGBrush = new SolidColorBrush(Color.FromRgb(210, 180, 140));

            CompositionTarget.Rendering += (o, e) => Loop();
        }

        public void KeyDown() {
            cointSizeScale *= mouseDownScale;
            mainWindowViewModel.ClickerLogic.Clicked();
        }

        private void Loop()
        {
            coinBound = new Rect(ActualWidth / 2 - baseCoinSize * cointSizeScale / 2, ActualHeight / 2 - baseCoinSize * cointSizeScale / 2, baseCoinSize * cointSizeScale, baseCoinSize * cointSizeScale);
            coinPosition = new Vector2((float)(coinBound.X + coinBound.Width / 2f), (float)(coinBound.Y + coinBound.Height / 2f));

            if (Mouse.LeftButton == MouseButtonState.Released)
            {
                cointSizeScale = defaultCoinSizeScale;
            }

            coinClickAnimation.Update();
            upgradeAnimation.Update();
            chests.Update();
            chestCoinAnimation.Update();

            InvalidateVisual();
        }



        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            if (ClickerLogic.IsBonus)
            {
                animatedBonusDrawing.StartAnimation();
                animatedBonusDrawing.DrawText(drawingContext, new Point(ActualWidth / 2, ActualHeight/2f-baseCoinSize*2), ClickerLogic.Player.IncomeBonusMultiplier + "x", Brushes.Gold, Brushes.DarkOrange);
            }
            else
            {
                animatedBonusDrawing.EndAnimation();
            }

            drawingContext.PushTransform(new RotateTransform(TimeToRotTransform(time++), coinBound.X+ coinBound.Width/2f, coinBound.Y + coinBound.Height / 2f));
            drawingContext.DrawRectangle(coinBrushes[(int)ClickerLogic.Player.CoinLevel], null, coinBound);
            drawingContext.Pop();


            ParticleSystemDrawer.DrawDouble(upgradeAnimation, drawingContext, 5, 153, 0);

            ParticleSystemDrawer.DrawDouble(coinClickAnimation, drawingContext, 255, 255, 255);

            double barOffset = 52;

            Pen outerPen = new Pen(loadingBarBGBrush, 1);
            Pen loadPen = new Pen(loadingBarBrush, 8);
            loadPen.StartLineCap = PenLineCap.Round;
            loadPen.EndLineCap = PenLineCap.Round;

            drawingContext.DrawRoundedRectangle(null, outerPen, new Rect(barOffset - 5, ActualHeight - barOffset - 5, (ActualWidth - barOffset * 2 + 9), 11), 5, 5);
            drawingContext.DrawLine(loadPen, new Point(barOffset, ActualHeight-barOffset), new Point((ActualWidth - barOffset * 2) * ClickerLogic.LoadingBarPercentige + barOffset, ActualHeight - barOffset));

            foreach (var chest in chests.Particles)
            {
                chestBrush.Opacity = 2*chest.Lives/(float)chests.Lives;
                drawingContext.DrawRectangle(chestBrush, null, new Rect(chest.Position.X, chest.Position.Y, chestWidth, chestHeight));
            }

            ParticleSystemDrawer.DrawDouble(chestCoinAnimation, drawingContext, 255, 26, 26);
        }

        public void OnPlayerClicked(double value)
        {
            coinClickAnimation.AddInstance(value, coinPosition, Particle<double>.MovementType.RADIAL);
        }

        public void OnUpgradeClicked(IEnumerable<double> values, IEnumerable<int> times)
        {
            if(values.Count()>0)
                autoClickSound.Play();

            for (int i = 0; i < values.Count(); i++)
            {
                upgradeAnimation.AddInstance(values.ElementAt(i) * times.ElementAt(i), coinPosition, Particle<double>.MovementType.RADIAL);
            }
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            if (coinBound.Contains(e.GetPosition(this))){
                cointSizeScale *= mouseDownScale;
                mainWindowViewModel.ClickerLogic.Clicked();
            }

            for(int i = 0; i < chests.Particles.Count; i++)
            {
                if(new Rect(chests.Particles[i].Position.X, chests.Particles[i].Position.Y, chestWidth, chestHeight).Contains(e.GetPosition(this))) {
                    chests.Particles[i].Lives = 0;
                    chestCoinAnimation.AddInstance(mainWindowViewModel.ClickerLogic.Player.Money*0.1, chests.Particles[i].Position, Particle<double>.MovementType.RADIAL);
                    mainWindowViewModel.ClickerLogic.Player.Money += mainWindowViewModel.ClickerLogic.Player.Money * 0.1;
                }
            }

        }

        private double TimeToRotTransform(double time) {
            return 10*Math.Sin(time/30);
        }
    }
}
