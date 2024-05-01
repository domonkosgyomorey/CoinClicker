using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace CoinClicker
{
    public class StockWindowViewModel : ObservableRecipient
    {

        private IStockLogic stockLogic;

        public string IconPath { get; set; }
        public string CoinIcon { get; set; }

        public Player Player { get => stockLogic.Player; }
        public IStockLogic StockLogic { get => stockLogic; }

        public StockWindowViewModel(IStockLogic stockLogic)
        {
            this.stockLogic = stockLogic;

            IconPath = Utility.STOCK_IMAGE_PATH;
            CoinIcon = Utility.ICON_PATH;
        }

    }
}
