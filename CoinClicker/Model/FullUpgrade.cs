using System.Windows.Media.Imaging;

namespace CoinClicker
{
    public class FullUpgrade
    {
        public BitmapImage Image { get; set; }
        public Upgrade Upgrade { get; set; }

        public FullUpgrade(BitmapImage image, Upgrade upgrade)
        {
            Image = image;
            Upgrade = upgrade;
        }
    }
}
