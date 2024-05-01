using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System.Windows.Input;
using System.Windows.Threading;

namespace CoinClicker
{
    public class StockLogic : ObservableRecipient, IStockLogic
    {
        private List<Stock> market;

        private int depositPercent;
        private double depositValue;
        private string selectedStockName;
        private DispatcherTimer updateTimer;


        public ICommand BuyStockCommand { get; set; }
        public ICommand SellAllStockCommand { get; set; }
        public Player Player { get; set; }
        public Stock SelectedStock
        {
            get
            {
                Stock stock = market.FirstOrDefault(x => x.Name == SelectedStockName);
                // On load there is no SelectedStockName
                if (stock == default)
                {
                    stock = market.FirstOrDefault(x => x.IsDefaultStock);
                }
                // If every stock are not the default one
                if (stock == default)
                {
                    stock = new Stock("error", 100, 10000000000);
                }
                SelectedStockName = stock.Name;
                return stock;
            }
        }

        public string SelectedStockName { get => selectedStockName; set => SetProperty(ref selectedStockName, value); }
        public int DepositPercent
        {
            get => depositPercent; set
            {
                SetProperty(ref depositPercent, value);
                DepositValue = Player.Money * DepositPercent / 100.0;
            }
        }
        public double DepositValue { get => depositValue; set => SetProperty(ref depositValue, value); }
        public List<Stock> Market
        {
            get => market; set
            {
                market = value;
                foreach (var stock in market)
                {
                    CircularBuffer<double> n = new((int)Player.StockHistMemory);
                    foreach (var i in stock.Prices) n.Add(i);
                    stock.Prices = n;
                    CircularBuffer<double> m = new((int)Player.StockHistMemory);
                    foreach (var i in stock.Dates) m.Add(i);
                    stock.Dates = m;
                }
            }
        }

        public StockLogic(Player player)
        {
            market = new();
            Player = player;

            updateTimer = new DispatcherTimer();
            updateTimer.Interval = TimeSpan.FromMilliseconds(Player.StockMillisUntilNewUpdate);
            updateTimer.Tick += (o, a) => Update();
            updateTimer.Start();

            BuyStockCommand = new RelayCommand(BuyStock, () => Player.Money >= DepositValue);
            SellAllStockCommand = new RelayCommand(SellAllStock);

            Player.PropertyChanged += (sender, e) =>
            {
                switch (e.PropertyName)
                {
                    case "Money":
                        DepositValue = Player.Money * DepositPercent / 100.0;
                        break;
                    case "StockHistMemory":
                        foreach (var stock in market)
                        {
                            CircularBuffer<double> n = new((int)Player.StockHistMemory);
                            foreach (var i in stock.Prices) n.Add(i);
                            stock.Prices = n;
                            CircularBuffer<double> m = new((int)Player.StockHistMemory);
                            foreach (var i in stock.Dates) m.Add(i);
                            stock.Dates = m;
                        }
                        break;
                    case "StockMillisUnitlNewUpdate":
                        updateTimer = new DispatcherTimer();
                        updateTimer.Interval = TimeSpan.FromMilliseconds(Player.StockMillisUntilNewUpdate);
                        updateTimer.Tick += (o, a) => Update();
                        updateTimer.Start();
                        break;
                    default: break;
                }
            };
        }

        public void Update()
        {
            foreach (var stock in market)
            {
                if (stock.TrendTime == 0)
                {
                    stock.TrendTime = Utility.random.Next(stock.MinTrendTime, stock.MaxTrendTime);
                    stock.TrendValue = (Utility.random.NextDouble() - 0.5) * stock.TrendValueMultiplier;
                }
                else
                {
                    stock.TrendTime--;
                }

                double inc = Utility.GaussianRandom(stock.Mean, stock.StdDev) + stock.TrendValue;

                if ((stock.PrevPrice + inc) < 0)
                {
                    stock.Prices.Add(stock.PrevPrice + 0);
                    stock.Dates.Add(stock.Time++);
                }
                else { 
                    stock.PrevPrice += inc;
                    stock.Prices.Add(stock.PrevPrice + inc);
                    stock.Dates.Add(stock.Time++);
                }

            }

        }

        private void BuyStock()
        {
            double stockPrice = SelectedStock.Prices.Last();
            SelectedStock.BuyedStockPrice.Add(stockPrice);
            SelectedStock.BuyedStock += DepositValue / stockPrice;
            Player.Money -= DepositValue;
        }

        private void SellAllStock()
        {
            double stockResult = SelectedStock.BuyedStock * SelectedStock.Prices.Last();
            Player.Money += stockResult;
            SelectedStock.BuyedStockPrice.Clear();
            SelectedStock.BuyedStock = 0;
        }
    }
}
