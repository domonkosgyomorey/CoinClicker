using Microsoft.Toolkit.Mvvm.ComponentModel;
using System.Text.Json.Serialization;

namespace CoinClicker
{
    public class Stock : ObservableObject
    {
        private List<double> buyedStockPrice;
        private string name;
        private double prevPrice;
        private double time;
        private double trendValue;
        private int trendTime;
        private double mean;
        private double stdDev;
        private double trendValueMultiplier;
        private int minTrendTime;
        private int maxTrendTime;
        private bool isDefaultStock;

        [JsonIgnore]
        public CircularBuffer<double> Prices { get; set; }

        [JsonIgnore]
        public CircularBuffer<double> Dates { get; set; }

        public List<double> PriceInList { get => Prices.ToList(); set => Prices = CircularBuffer<double>.FromList(value, value.Count); }
        public List<double> DatesInList { get => Dates.ToList(); set => Dates = CircularBuffer<double>.FromList(value, value.Count); }


        private double buyedStock;

        public List<double> BuyedStockPrice { get => buyedStockPrice; set => SetProperty(ref buyedStockPrice, value); }

        public double BuyedStock { get => buyedStock; set => SetProperty(ref buyedStock, value); }
        public double PrevPrice { get => prevPrice; set => prevPrice = value; }
        public double Time { get => time; set => time = value; }
        public double TrendValue { get => trendValue; set => trendValue = value; }
        public int TrendTime { get => trendTime; set => trendTime = value; }
        public double Mean { get => mean; set => mean = value; }
        public double StdDev { get => stdDev; set => stdDev = value; }
        public double TrendValueMultiplier { get => trendValueMultiplier; set => trendValueMultiplier = value; }
        public int MinTrendTime { get => minTrendTime; set => minTrendTime = value; }
        public int MaxTrendTime { get => maxTrendTime; set => maxTrendTime = value; }
        public string Name { get => name; set => name = value; }
        public bool IsDefaultStock { get => isDefaultStock; set => isDefaultStock = value; }

        public Stock(string name, int histMemory, double basePrice, bool isDefaultStock = true, double mean = 0, double standardDeviation = 5, double trendValueMultiplier = 10, int minTrendTime = 1, int maxTrendTime = 5)
        {
            Prices = new(histMemory);
            Dates = new(histMemory);
            buyedStock = new();
            buyedStockPrice = new();
            Prices.Add(basePrice);
            Dates.Add(time++);
            prevPrice = Prices[0];
            this.mean = mean;
            stdDev = standardDeviation;
            this.trendValueMultiplier = trendValueMultiplier;
            this.minTrendTime = minTrendTime;
            this.maxTrendTime = maxTrendTime;
            this.name = name;
            this.isDefaultStock = isDefaultStock;
        }

        public Stock()
        {

        }
    }
}
