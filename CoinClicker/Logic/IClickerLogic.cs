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

        event ClickerLogic.ClickedDelegate OnClicked;
        event ClickerLogic.CloseStockWindowDelegate OnCloseStockWindow;
        event ClickerLogic.CloseTutorialDelegate OnCloseTutorial;
        event ClickerLogic.OpenStockWindowDelegate OnOpenStockWindow;
        event ClickerLogic.OpenTutorialDelegate OnOpenTutorial;
        event ClickerLogic.UpgradeBuyed OnUpgradeBuyed;
        event ClickerLogic.UpgradeClickDelegete OnUpgradeClicked;

        void Clicked();
        void Clicked(List<double> value, List<int> times);
        void LoadUpgrades(List<Upgrade> upgrades);
    }
}