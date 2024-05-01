using System.Data;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace CoinClicker
{
    public class UpgradeManager
    {
        public List<FullUpgrade> fullUpgrades;
        public DispatcherTimer timer;

        public UpgradeManager(Action<IEnumerable<Upgrade>> predicate)
        {
            fullUpgrades = new();
            timer = new();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += (o, e) => predicate?.Invoke(fullUpgrades.Select(x => x.Upgrade));
            timer.Start();
        }

        public void Add(Upgrade upgrade, BitmapImage image)
        {
            fullUpgrades.Add(new FullUpgrade(image, upgrade));
        }

        public List<Upgrade> GetUpgrades()
        {
            return fullUpgrades.Select(x => x.Upgrade).ToList();
        }

        public List<FullUpgrade> GetFullUpgrade()
        {
            return fullUpgrades;
        }
    }
}
