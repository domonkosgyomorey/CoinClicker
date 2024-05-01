using Microsoft.Toolkit.Mvvm.Input;
using System.IO;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace CoinClicker
{
    public class ClickerLogic : IClickerLogic
    {

        private UpgradeManager upgradeManager;
        private LoadingBarTimeManager loadingBarTimeManager;

        public delegate void UpgradeClickDelegete(IEnumerable<double> value, IEnumerable<int> times);
        public delegate void ClickedDelegate(double value);
        public delegate void OpenStockWindowDelegate();
        public delegate void CloseStockWindowDelegate();
        public delegate void OpenTutorialDelegate();
        public delegate void CloseTutorialDelegate();
        public delegate void UpgradeBuyed();

        public event ClickedDelegate OnClicked;
        public event UpgradeClickDelegete OnUpgradeClicked;
        public event OpenStockWindowDelegate OnOpenStockWindow;
        public event CloseStockWindowDelegate OnCloseStockWindow;
        public event OpenTutorialDelegate OnOpenTutorial;
        public event CloseTutorialDelegate OnCloseTutorial;
        public event UpgradeBuyed OnUpgradeBuyed;

        public ICommand BuyUpgradeCommand { get; set; }
        public ICommand BuyClickUpgradeCommand { get; set; }
        public ICommand BuyCoinUpgradeCommand { get; set; }
        public ICommand StockClickCommand { get; set; }
        public ICommand StockViewUpgradeCommand { get; set; }
        public ICommand BuyBonusIncomeUpgradeCommand { get; set; }
        public ICommand TutorialUnlockCommand { get; set; }
        public UpgradeManager UpgradeManager { get => upgradeManager; set => upgradeManager = value; }
        public List<FullUpgrade> FullUpgrades { get => UpgradeManager.GetFullUpgrade(); }
        public double LoadingBarPercentige { get; set; }
        public bool IsBonus { get; set; }

        private Player.STOCK_STATE prevStockState;
        private Player.TUTORIAL_STATE prevTutorialState;
        private Timer bonusTimer;
        private int incomeMeterPerMillis = 1000;
        private double prevMoney;

        public Player Player { get; }
        public LoadingBarTimeManager LoadingBarTimeManager { get => loadingBarTimeManager; set => loadingBarTimeManager = value; }

        public ClickerLogic(Player player)
        {
            Player = player;

            upgradeManager = new((x) =>
            {
                IEnumerable<Upgrade> u = x.Where(x => x.NumberOf > 0);
                Clicked(u.Select(x => x.AmountPerSec).ToList(), u.Select(x => x.NumberOf).ToList());
            });
            loadingBarTimeManager = new((int)player.LoadingBarUpdateInMillis, (int)player.LoadingBarTimeInSeconds, (val) => { LoadingBarPercentige = val; }, StartBonus);

            BuyUpgradeCommand = new CustomRelayCommand((o) =>
            {
                if (o is Upgrade upgrade)
                {
                    BuyUpgrade(upgrade);
                    OnUpgradeBuyed?.Invoke();
                }
            }, (o) =>
            {
                if (o is Upgrade upgrade)
                {
                    return Player.Money - upgrade.Price >= 0;
                }
                return false;
            });

            BuyClickUpgradeCommand = new RelayCommand(UpgradeClickPower, () =>
            {
                return Player.Money - Player.ClickPowerUpgradePrice >= 0;
            });

            BuyCoinUpgradeCommand = new RelayCommand(UpgradeCoin, () =>
            {
                return (Player.Money - Player.CoinUpgradePrice >= 0) && (Player.CoinLevel + 1 < Player.COIN_LEVEL.COIN_LEVEL_SIZE);
            });

            StockClickCommand = new RelayCommand(ExecuteNextStock, () =>
            {
                return Player.Money - Player.StockUnlockPrice >= 0;
            });

            StockViewUpgradeCommand = new RelayCommand(StockChartUpgrade, () =>
            {
                return Player.Money - Player.StockChartUpgradePrice >= 0;
            });

            BuyBonusIncomeUpgradeCommand = new RelayCommand(BuyBonusIncomeUpgrade, () =>
            {
                return Player.Money - Player.BonusIncomeUpgradePrice >= 0;
            });

            TutorialUnlockCommand = new RelayCommand(ExecuteNextTutorialState, () =>
            {
                return Player.Money - Player.TutorialUnlockPrice >= 0;
            });

            DispatcherTimer incomeMeter = new DispatcherTimer();
            incomeMeter.Interval = TimeSpan.FromMilliseconds(incomeMeterPerMillis);
            incomeMeter.Tick += (o, e) =>
            {
                Player.IncomePerSec = Player.Money - prevMoney;
                prevMoney = Player.Money;
            };
            incomeMeter.Start();


        }

        public void Clicked()
        {
            double withUpgrades = Player.ClickPower * Player.IncreaseAllIncomeBy * (IsBonus ? Player.IncomeBonusMultiplier : 1);
            OnClicked?.Invoke(withUpgrades);
            Player.Money += withUpgrades;
            NotifyCommonBuyCommands();
        }

        public void Clicked(List<double> value, List<int> times)
        {
            for (int i = 0; i < value.Count(); i++)
            {
                double withUpgrades = value.ElementAt(i) * Player.IncreaseAllIncomeBy * (IsBonus ? Player.IncomeBonusMultiplier : 1);
                value[i] = withUpgrades;
                Player.Money += withUpgrades * times.ElementAt(i);
            }
            OnUpgradeClicked?.Invoke(value, times);
            NotifyCommonBuyCommands();
        }


        public void LoadUpgrades(List<Upgrade> upgrades)
        {
            foreach (var upgrade in upgrades)
            {
                BitmapImage image = null;
                try
                {
                    image = new BitmapImage();
                    image.BeginInit();
                    image.UriSource = new Uri(Path.GetFullPath(Utility.UPGRADE_PATH + "\\" + upgrade.Path));
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.EndInit();
                }
                catch
                {
                    try
                    {
                        image = new BitmapImage();
                        image.BeginInit();
                        image.UriSource = new Uri(Path.GetFullPath(Utility.DEFAULT_UPGRADE_PATH));
                        image.CacheOption = BitmapCacheOption.OnLoad;
                        image.EndInit();
                    }
                    catch { }
                }
                upgradeManager.Add(upgrade, image);
            }
        }

        private void NotifyCommonBuyCommands()
        {
            (BuyClickUpgradeCommand as RelayCommand).NotifyCanExecuteChanged();
            (BuyUpgradeCommand as CustomRelayCommand).NotifyCanExecuteChanged();
            (BuyCoinUpgradeCommand as RelayCommand).NotifyCanExecuteChanged();
            (StockClickCommand as RelayCommand).NotifyCanExecuteChanged();
            (StockViewUpgradeCommand as RelayCommand).NotifyCanExecuteChanged();
            (BuyBonusIncomeUpgradeCommand as RelayCommand).NotifyCanExecuteChanged();
            (TutorialUnlockCommand as RelayCommand).NotifyCanExecuteChanged();
        }

        private void UpgradeClickPower()
        {
            Player.Money -= Player.ClickPowerUpgradePrice;
            Player.ClickPower *= Player.ClickPowerUpgradeMultiplier;
            Player.ClickPowerUpgradePrice *= Player.ClickPowerUpgradePriceMultiplier;
        }

        private void UpgradeCoin()
        {
            Player.Money -= Player.CoinUpgradePrice;
            Player.IncreaseAllIncomeBy += Player.IncreaseAllIncomeByIncrease;
            Player.CoinLevel += 1;
            Player.CoinUpgradePrice *= Player.CoinUpgradePriceMultiplier;
        }

        private void BuyUpgrade(Upgrade upgrade)
        {
            Player.Money -= upgrade.Price;
            upgrade.NumberOf++;
            upgrade.Price = ((double)upgrade.Price * upgrade.PriceIncreaseRatio);
        }

        private Player.STOCK_STATE GetNextStockState(Player.STOCK_STATE state)
        {
            return state switch
            {
                Player.STOCK_STATE.LOCKED => Player.STOCK_STATE.CLOSED,
                Player.STOCK_STATE.CLOSED => Player.STOCK_STATE.OPENED,
                Player.STOCK_STATE.OPENED => Player.STOCK_STATE.CLOSED,
                _ => Player.STOCK_STATE.LOCKED
            };
        }

        private void ExecuteNextStock()
        {
            prevStockState = Player.StockState;
            Player.StockState = GetNextStockState(Player.StockState);
            switch (Player.StockState)
            {
                case Player.STOCK_STATE.LOCKED:
                    Player.Money -= Player.StockUnlockPrice;
                    Player.StockUnlockPrice = 0;
                    break;
                case Player.STOCK_STATE.OPENED:
                    OnOpenStockWindow?.Invoke();
                    break;
                case Player.STOCK_STATE.CLOSED:
                    if (prevStockState == Player.STOCK_STATE.OPENED)
                        OnCloseStockWindow?.Invoke();
                    break;
            }
        }

        private void StockChartUpgrade()
        {
            Player.Money -= Player.StockChartUpgradePrice;
            Player.StockChartUpgradePrice *= Player.StockChartUpgradePriceMultiplier;
            Player.StockHistMemory = (int)(Player.StockHistMemory * Player.StockHistMemoryMultiplier);
        }

        private void BuyBonusIncomeUpgrade()
        {
            Player.Money -= Player.BonusIncomeUpgradePrice;
            Player.BonusIncomeUpgradePrice *= Player.BonusIncomeUpgradePriceMultiplier;
            Player.IncomeBonusMultiplier = (int)(Player.IncomeBonusMultiplier * Player.IncomeBonusUpgradeMultiplier);
        }

        private void StartBonus()
        {
            IsBonus = true;
            if (bonusTimer != null)
                bonusTimer.Dispose();
            bonusTimer = new Timer((o) =>
            {
                IsBonus = false;
            }, null, Timeout.Infinite, Timeout.Infinite);

            bonusTimer.Change(TimeSpan.FromSeconds(Player.BonusInSeconds), Timeout.InfiniteTimeSpan);
        }


        private Player.TUTORIAL_STATE GetNextTutorialState(Player.TUTORIAL_STATE state)
        {
            return state switch
            {
                Player.TUTORIAL_STATE.LOCKED => Player.TUTORIAL_STATE.CLOSED,
                Player.TUTORIAL_STATE.CLOSED => Player.TUTORIAL_STATE.OPENED,
                Player.TUTORIAL_STATE.OPENED => Player.TUTORIAL_STATE.CLOSED,
                _ => Player.TUTORIAL_STATE.LOCKED,
            };
        }

        private void ExecuteNextTutorialState()
        {
            prevTutorialState = Player.TutorialState;
            Player.TutorialState = GetNextTutorialState(Player.TutorialState);
            switch (Player.TutorialState)
            {
                case Player.TUTORIAL_STATE.LOCKED:
                    Player.Money -= Player.TutorialUnlockPrice;
                    Player.TutorialUnlockPrice = 0;
                    break;
                case Player.TUTORIAL_STATE.CLOSED:
                    if (prevTutorialState == Player.TUTORIAL_STATE.OPENED)
                        OnCloseTutorial?.Invoke();
                    break;
                case Player.TUTORIAL_STATE.OPENED:
                    OnOpenTutorial?.Invoke();
                    break;
            }
        }
    }
}
