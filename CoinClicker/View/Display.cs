using System.Numerics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

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

        private IClickerLogic ClickerLogic { get => mainWindowViewModel.ClickerLogic; }

        private ImageBrush enemyBrush;
        private EnemyManager enemyManager;
        private ParticleSystem<double> enemiesStealCoinAnimation;
        private int enemyWidth = 35;
        private int enemyHeight = 40;
        private int enemyLives = 125;

        private ParticleSystem<object> explosion;
        private int explosionFragmentCount = 20;
        private MySoundPlayer explosionSound;

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
            enemyBrush = new ImageBrush(new BitmapImage(new Uri(Utility.ENEMY_PATH)));

            coinBound = new Rect(ActualWidth / 2 - baseCoinSize / 2 * cointSizeScale, ActualHeight / 2 - baseCoinSize / 2 * cointSizeScale, baseCoinSize, baseCoinSize);
            coinPosition = new Vector2((float)(coinBound.X + coinBound.Width / 2f), (float)(coinBound.Y + coinBound.Height / 2f));

            coinClickAnimation = new ParticleSystem<double>(subCoinLifeTimeInFrame);
            upgradeAnimation = new ParticleSystem<double>(upgradesLifeTimeInFrame);
            
            chestCoinAnimation = new ParticleSystem<double>(chestLives);
            chests = new(chestLives);
            ClickerLogic.OnChestSpawned += () => {
                chests.AddInstance(0, new Vector2(Utility.random.Next(100, (int)ActualWidth - 100), Utility.random.Next(100, (int)ActualHeight - 100)), Particle<byte>.MovementType.STATIC); 
            };

            animatedBonusDrawing = new();

            ClickerLogic.OnUpgradeClicked += OnUpgradeClicked;
            ClickerLogic.OnClicked += OnPlayerClicked;

            autoClickSound = new MySoundPlayer(Utility.AUTO_CLICK_SOUND_PATH, 5);

            loadingBarBrush = new SolidColorBrush(Color.FromRgb(5, 153, 0));
            loadingBarBGBrush = new SolidColorBrush(Color.FromRgb(210, 180, 140));

            enemiesStealCoinAnimation = new ParticleSystem<double>(subCoinLifeTimeInFrame);
            enemyManager = new EnemyManager(enemyLives);
            ClickerLogic.OnEnemySpawned += () => {
                // Spawning at one of the window edge
                enemyManager.AddEnemy(new Vector2(Utility.random.Next(0, 2)*(int)ActualWidth, Utility.random.Next(0, 2)*(int)ActualHeight));
            };
            explosionSound = new MySoundPlayer(Utility.EXPLOSION_SOUND, 10);
            explosion = new ParticleSystem<object>(50);

            CompositionTarget.Rendering += (o, e) => Loop();
        }

        public new void KeyDown() {
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
            explosion.Update();

            for (int i = 0; i < enemyManager.Enemies.Count; i++)
            {
                Enemy enemy = enemyManager.Enemies[i];
                if (enemy.Lives == 1)
                {
                    Explode(enemy.Position+new Vector2(enemyWidth/2f, enemyHeight/2f));
                }
                if (new Rect(enemy.Position.X, enemy.Position.Y, enemyWidth, enemyHeight).Contains(Mouse.GetPosition(this)))
                {
                    enemy.Lives = 0;
                    enemiesStealCoinAnimation.AddInstance(-ClickerLogic.Player.Money * ClickerLogic.Player.EnemyStealMoneyPerc, enemy.Position, Particle<double>.MovementType.RADIAL);
                    ClickerLogic.StealMoney(ClickerLogic.Player.Money * ClickerLogic.Player.EnemyStealMoneyPerc);

                    Explode(enemy.Position + new Vector2(enemyWidth / 2f, enemyHeight / 2f));
                }
            }

            enemyManager.Update(new Vector2((float)Mouse.GetPosition(this).X-enemyWidth/2f, (float)Mouse.GetPosition(this).Y-enemyHeight/2f));
            enemiesStealCoinAnimation.Update();

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

            foreach (var enemy in enemyManager.Enemies)
            {
                drawingContext.DrawRectangle(enemyBrush, null, new Rect(enemy.Position.X, enemy.Position.Y, enemyWidth, enemyHeight));
            }

            ParticleSystemDrawer.DrawDouble(chestCoinAnimation, drawingContext, 5, 153, 0);
            ParticleSystemDrawer.DrawDouble(enemiesStealCoinAnimation, drawingContext, 255, 26, 26);
            ParticleSystemDrawer.DrawPoints(explosion, 2, drawingContext, Color.FromRgb(255, 0, 0), Color.FromRgb(0, 0, 0));
            
        }

        public void OnPlayerClicked(double value)
        {
            coinClickAnimation.AddInstance(value, coinPosition, Particle<double>.MovementType.RADIAL);
        }

        public void OnUpgradeClicked(IEnumerable<double> values, IEnumerable<int> times)
        {
            if(values.Count()>0)
                autoClickSound.Play(mainWindowViewModel.FxVolume/100f);

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
                    chestCoinAnimation.AddInstance(ClickerLogic.Player.Money*0.1, chests.Particles[i].Position, Particle<double>.MovementType.RADIAL);
                    ClickerLogic.AddMoney(ClickerLogic.Player.Money * 0.1);
                }
            }
        }

        private double TimeToRotTransform(double time) {
            return 10*Math.Sin(time/30);
        }

        private void Explode(Vector2 pos)
        {
            explosionSound.Play(mainWindowViewModel.FxVolume/100f);
            for (int j = 0; j < explosionFragmentCount; j++)
            {
                explosion.AddInstance(0, pos, Particle<object>.MovementType.RADIAL);
            }
        }
    }
}
