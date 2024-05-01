using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace CoinClicker
{
    public class Upgrade : ObservableObject
    {
        private double amountPerSec;
        private double price;
        private double priceIncreaseRatio;
        private string path;
        private int numberOf;

        public double AmountPerSec
        {
            get => amountPerSec;
            set { SetProperty(ref amountPerSec, value); }
        }

        public double Price
        {
            get => price;
            set { SetProperty(ref price, value); }
        }

        public int NumberOf
        {
            get => numberOf;
            set { SetProperty(ref numberOf, value); }
        }

        public double PriceIncreaseRatio
        {
            get => priceIncreaseRatio;
            set { SetProperty(ref priceIncreaseRatio, value); }
        }

        public string Path { get => path; set => path = value; }

        public Upgrade() { }

        public Upgrade(double amountPerSec, double price, double priceIncreaseRatio, string path)
        {

            this.amountPerSec = amountPerSec;
            this.price = price;
            this.path = path;
            this.priceIncreaseRatio = priceIncreaseRatio;
        }
    }
}
