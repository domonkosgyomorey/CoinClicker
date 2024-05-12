using System.IO;
using System.Windows.Markup;

namespace CoinClicker
{
    public class Utility
    {
        public static Random random = new Random(123);
        public static string RESOURCE_PATH = Path.GetFullPath(Environment.CurrentDirectory + "\\Resources");
        public static string MUSIC_PATH = Path.GetFullPath(RESOURCE_PATH + "\\Music");
        public static string CURSOR_PATH = Path.GetFullPath(RESOURCE_PATH + "\\Cursor\\point_cursor_emerald.cur");
        public static string UPGRADE_PATH = Path.GetFullPath(RESOURCE_PATH + "\\Upgrade");
        public static string DEFAULT_UPGRADE_PATH = Path.GetFullPath(UPGRADE_PATH + "\\default.png");
        public static string UPGRADE_DATA_PATH = Path.GetFullPath(UPGRADE_PATH + "\\upgrades.txt");
        public static string STOCK_IMAGE_PATH = Path.GetFullPath(UPGRADE_PATH + "\\stock.png");
        public static string HOME_IMAGE_PATH = Path.GetFullPath(UPGRADE_PATH + "\\home.png");
        public static string COIN_PATH = Path.GetFullPath(RESOURCE_PATH + "\\Coin");
        public static string ICON_PATH = Path.GetFullPath(COIN_PATH + "\\gold.png");
        public static string BACKGROUND_PATH = Path.GetFullPath(RESOURCE_PATH + "\\Background\\background.png");
        public static string SOUND_PATH = Path.GetFullPath(RESOURCE_PATH + "\\Sound");
        public static string AUTO_CLICK_SOUND_PATH = Path.GetFullPath(SOUND_PATH + "\\click.wav");
        public static string SAVE_PATH = Path.GetFullPath(RESOURCE_PATH + "\\Save");
        public static string PLAYER_SAVE_DATA = Path.GetFullPath(SAVE_PATH + "\\player.json");
        public static string STOCKS_SAVE_DATA = Path.GetFullPath(SAVE_PATH + "\\stocks.json");
        public static string UPGRADES_SAVE_DATA = Path.GetFullPath(SAVE_PATH + "\\upgrades.json");
        public static string UPGRADE_IMAGES_SAVE_DATA = Path.GetFullPath(SAVE_PATH + "\\upgradeImages.json");
        public static string UPGRADE_TIMERS_SAVE_DATA = Path.GetFullPath(SAVE_PATH + "\\upgradeTimers.json");
        public static string TUTORIAL_CONTENT_SAVE_DATA = Path.GetFullPath(SAVE_PATH + "\\tutorial.json");
        public static string TRESURE_CHEST_PATH = Path.GetFullPath(RESOURCE_PATH + "\\chest.png");
        public static string ENEMY_PATH = Path.GetFullPath(RESOURCE_PATH + "\\enemy.png");
        public static string ENEMY_EXPLODE_ANIMATION = Path.GetFullPath(RESOURCE_PATH + "\\explosion.mp4");
        public static string EXPLOSION_SOUND = Path.GetFullPath(SOUND_PATH + "\\explosion.mp3");

        public static double MAX_STOCK_BUY_VAL = 1E+20;
        public static double MIN_STOCK_BUY_VAL = -1E+20;

        public static double GaussianRandom(double mean, double dist)
        {
            double u1 = 1.0 - random.NextDouble();
            double u2 = 1.0 - random.NextDouble();
            return mean + dist * Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);
        }
    }
}
