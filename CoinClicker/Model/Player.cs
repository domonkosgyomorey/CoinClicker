using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace CoinClicker
{
    public class Player : ObservableObject
    {
        public enum COIN_LEVEL
        {
            BRONZE = 0,
            SILVER,
            GOLD,
            RUBY,
            COIN_LEVEL_SIZE
        }

        public enum STOCK_STATE
        {
            LOCKED,
            CLOSED,
            OPENED,
            STOCK_STATE_SIZE
        }

        public enum TUTORIAL_STATE { 
            LOCKED,
            CLOSED,
            OPENED,
            TUTORIAL_STATE_SIZE
        }

        private double money;
        private double clickPower;
        private double clickPowerUpgradeMultiplier;
        private double clickPowerUpgradePrice;
        private double clickPowerUpgradePriceMultiplier;
        private COIN_LEVEL coinLevel;
        private double coinUpgradePrice;
        private double coinUpgradePriceMultiplier;
        private double increaseAllIncomeBy;
        private double increaseAllIncomeByIncrease;
        private STOCK_STATE stockState;
        private double stockUnlockPrice;
        private double incomePerSec;
        private double stockHistMemory;
        private double stockMillisUntilNewUpdate;
        private double stockChartUpgradePrice;
        private double stockChartUpgradePriceMultiplier;
        private double stockHistMemoryMultiplier;
        private double loadingBarTimeInSeconds;
        private double loadingBarUpdateInMillis;
        private double bonusInSeconds;
        private double incomeBonusMultiplier;
        private double incomeBonusUpgradeMultiplier;
        private double bonusIncomeUpgradePrice;
        private double bonusIncomeUpgradePriceMultiplier;
        private TUTORIAL_STATE tutorialState;
        private double tutorialUnlockPrice;
        private bool keyboardUnlocked;
        private double keyboardUnlockPrice;
        private int minTimeChestSpawn;
        private int maxTimeChestSpawn;
        private double chestMoneyPerc;
        private int minTimeEnemySpawn;
        private int maxTimeEnemySpawn;
        private double enemyStealMoneyPerc;

        public double ClickPower { get => clickPower; set => SetProperty(ref clickPower, value); }
        public double Money { get => money; set => SetProperty(ref money, value); }
        public double ClickPowerUpgradeMultiplier { get => clickPowerUpgradeMultiplier; set => SetProperty(ref clickPowerUpgradeMultiplier, value); }
        public double ClickPowerUpgradePrice { get => clickPowerUpgradePrice; set => SetProperty(ref clickPowerUpgradePrice, value); }
        public double ClickPowerUpgradePriceMultiplier { get => clickPowerUpgradePriceMultiplier; set => SetProperty(ref clickPowerUpgradePriceMultiplier, value); }
        public COIN_LEVEL CoinLevel { get => coinLevel; set => SetProperty(ref coinLevel, value); }
        public double CoinUpgradePrice { get => coinUpgradePrice; set => SetProperty(ref coinUpgradePrice, value); }
        public double CoinUpgradePriceMultiplier { get => coinUpgradePriceMultiplier; set => SetProperty(ref coinUpgradePriceMultiplier, value); }
        public double IncreaseAllIncomeBy { get => increaseAllIncomeBy; set => SetProperty(ref increaseAllIncomeBy, value); }
        public double IncreaseAllIncomeByIncrease { get => increaseAllIncomeByIncrease; set => SetProperty(ref increaseAllIncomeByIncrease, value); }
        public STOCK_STATE StockState { get => stockState; set => SetProperty(ref stockState, value); }
        public double StockUnlockPrice { get => stockUnlockPrice; set => SetProperty(ref stockUnlockPrice, value); }
        public double IncomePerSec { get => incomePerSec; set => SetProperty(ref incomePerSec, value); }
        public double StockHistMemory { get => stockHistMemory; set => SetProperty(ref stockHistMemory, value); }
        public double StockMillisUntilNewUpdate { get => stockMillisUntilNewUpdate; set => SetProperty(ref stockMillisUntilNewUpdate, value); }
        public double StockChartUpgradePrice { get => stockChartUpgradePrice; set => SetProperty(ref stockChartUpgradePrice, value); }
        public double StockChartUpgradePriceMultiplier { get => stockChartUpgradePriceMultiplier; set => SetProperty(ref stockChartUpgradePriceMultiplier, value); }
        public double StockHistMemoryMultiplier { get => stockHistMemoryMultiplier; set => SetProperty(ref stockHistMemoryMultiplier, value); }
        public double LoadingBarTimeInSeconds { get => loadingBarTimeInSeconds; set => SetProperty(ref loadingBarTimeInSeconds, value); }
        public double LoadingBarUpdateInMillis { get => loadingBarUpdateInMillis; set => SetProperty(ref loadingBarUpdateInMillis, value); }
        public double BonusInSeconds { get => bonusInSeconds; set => SetProperty(ref bonusInSeconds, value); }
        public double IncomeBonusMultiplier { get => incomeBonusMultiplier; set => SetProperty(ref incomeBonusMultiplier, value); }
        public double IncomeBonusUpgradeMultiplier { get => incomeBonusUpgradeMultiplier; set => SetProperty(ref incomeBonusUpgradeMultiplier, value); }
        public double BonusIncomeUpgradePrice { get => bonusIncomeUpgradePrice; set => SetProperty(ref bonusIncomeUpgradePrice, value); }
        public double BonusIncomeUpgradePriceMultiplier { get => bonusIncomeUpgradePriceMultiplier; set => SetProperty(ref bonusIncomeUpgradePriceMultiplier, value); }
        public TUTORIAL_STATE TutorialState { get => tutorialState; set => SetProperty(ref tutorialState, value); }
        public double TutorialUnlockPrice { get => tutorialUnlockPrice; set => SetProperty(ref tutorialUnlockPrice, value); }
        public bool KeyboardUnlocked { get => keyboardUnlocked; set => SetProperty(ref keyboardUnlocked, value); }
        public double KeyboardUnlockPrice { get => keyboardUnlockPrice; set => SetProperty(ref keyboardUnlockPrice, value); }
        public int MinTimeChestSpawn { get => minTimeChestSpawn; set => SetProperty(ref minTimeChestSpawn, value); }
        public int MaxTimeChestSpawn { get => maxTimeChestSpawn; set => SetProperty(ref maxTimeChestSpawn, value); }
        public double ChestMoneyPerc { get => chestMoneyPerc; set => SetProperty(ref chestMoneyPerc, value); }
        public int MinTimeEnemySpawn { get => minTimeEnemySpawn; set => SetProperty(ref minTimeEnemySpawn, value); }
        public int MaxTimeEnemySpawn { get => maxTimeEnemySpawn; set => SetProperty(ref maxTimeEnemySpawn, value); }
        public double EnemyStealMoneyPerc { get => enemyStealMoneyPerc; set => SetProperty(ref enemyStealMoneyPerc, value); }

        public Player()
        {
            money = 0;
            
            clickPower = 1;
            clickPowerUpgradeMultiplier = 2;
            clickPowerUpgradePrice = 100;
            clickPowerUpgradePriceMultiplier = 10;
            
            coinLevel = COIN_LEVEL.BRONZE;
            coinUpgradePrice = 5000;
            coinUpgradePriceMultiplier = 100;
            
            increaseAllIncomeBy = 1.0;
            increaseAllIncomeByIncrease = 0.5;
            
            stockState = STOCK_STATE.LOCKED;
            
            stockUnlockPrice = 1000000;
            stockHistMemory = 100;
            stockMillisUntilNewUpdate = 100;
            
            stockChartUpgradePrice = 100000000;
            stockChartUpgradePriceMultiplier = 1000;
            
            stockHistMemoryMultiplier = 1.5;
            
            loadingBarTimeInSeconds = 60;
            loadingBarUpdateInMillis = 20;
            
            bonusInSeconds = 5;
            
            incomeBonusMultiplier = 5;
            incomeBonusUpgradeMultiplier = 1.5;
            
            bonusIncomeUpgradePrice = 1000000;
            bonusIncomeUpgradePriceMultiplier = 100000;
            
            tutorialState = TUTORIAL_STATE.LOCKED;
            tutorialUnlockPrice = 1000;
            
            keyboardUnlocked = false;
            keyboardUnlockPrice = 1000000;

            minTimeChestSpawn = 5000;
            maxTimeChestSpawn = 30000;
            chestMoneyPerc = 0.1;

            minTimeEnemySpawn = 5000;
            maxTimeEnemySpawn = 20000;
            enemyStealMoneyPerc = 0.5;
        }
    }
}
