using System.Windows.Input;

namespace CoinClicker
{
    public interface IClickerLogic
    {
        ICommand BuyBonusIncomeUpgradeCommand { get; set; }
        ICommand BuyClickUpgradeCommand { get; set; }
        ICommand BuyCoinUpgradeCommand { get; set; }
        ICommand BuyUpgradeCommand { get; set; }
        List<FullUpgrade> FullUpgrades { get; }
        bool IsBonus { get; set; }
        double LoadingBarPercentige { get; set; }
        LoadingBarTimeManager LoadingBarTimeManager { get; set; }
        Player Player { get; }
        ICommand StockClickCommand { get; set; }
        ICommand StockViewUpgradeCommand { get; set; }
        ICommand TutorialUnlockCommand { get; set; }
        ICommand BuyKeyboardClickCommand { get; set; }
        UpgradeManager UpgradeManager { get; set; }

        public delegate void UpgradeClickDelegete(IEnumerable<double> value, IEnumerable<int> times);
        public delegate void ClickedDelegate(double value);
        public delegate void OpenStockWindowDelegate();
        public delegate void CloseStockWindowDelegate();
        public delegate void OpenTutorialDelegate();
        public delegate void CloseTutorialDelegate();
        public delegate void UpgradeBuyed();
        public delegate void ChestSpawned();
        public delegate void EnemySpawned();

        public event ClickedDelegate OnClicked;
        public event CloseStockWindowDelegate OnCloseStockWindow;
        public event CloseTutorialDelegate OnCloseTutorial;
        public event OpenStockWindowDelegate OnOpenStockWindow;
        public event OpenTutorialDelegate OnOpenTutorial;
        public event UpgradeBuyed OnUpgradeBuyed;
        public event UpgradeClickDelegete OnUpgradeClicked;
        public event ChestSpawned OnChestSpawned;
        public event EnemySpawned OnEnemySpawned;

        public void Clicked();
        public void Clicked(List<double> value, List<int> times);
        public void LoadUpgrades(List<Upgrade> upgrades);
        public void AddMoney(double amount);
        public void StealMoney(double amount);
    }
}