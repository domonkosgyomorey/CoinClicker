using System.Windows.Input;

namespace CoinClicker
{
    public interface IStockLogic
    {
        ICommand BuyStockCommand { get; set; }
        int DepositPercent { get; set; }
        double DepositValue { get; set; }
        List<Stock> Market { get; set; }
        Player Player { get; }
        Stock SelectedStock { get; }
        string SelectedStockName { get; set; }
        ICommand SellAllStockCommand { get; set; }

        void Update();
    }
}