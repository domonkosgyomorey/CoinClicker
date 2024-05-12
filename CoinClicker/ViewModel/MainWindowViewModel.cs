using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System.Windows.Input;

namespace CoinClicker
{
    public class MainWindowViewModel : ObservableRecipient
    {
        private bool optionMenuVisible = false;
        private bool gameVisible = false;
        private bool menuVisible = true;
        private StockWindow stockWindow;
        private TutorialWindow tutorialWindow;
        private MyMusicPlayer myMusicPlayer;

        public string BackgroundPath { get; set; }
        public string CoinIconPath { get; set; }
        public string StockImagePath { get; set; }
        public string HomeImagePath { get; set; }

        public ICommand HomeButtonClicked { get; set; }

        public IClickerLogic ClickerLogic { get; private set; }

        private float volume;

        public float Volume { get => volume; set { 
                volume = value;
                myMusicPlayer?.SetVolume(value/100f);
            } }

        private float fxVolume;

        public float FxVolume { get => fxVolume; set {
                fxVolume = value;
            } }


        public MainWindowViewModel(IClickerLogic clickerLogic, StockWindow stockWindow, TutorialWindow tutorialWindow)
        {
            this.stockWindow = stockWindow;
            this.tutorialWindow = tutorialWindow;

            BackgroundPath = Utility.BACKGROUND_PATH;
            CoinIconPath = Utility.ICON_PATH;
            StockImagePath = Utility.STOCK_IMAGE_PATH;
            HomeImagePath = Utility.HOME_IMAGE_PATH;
            ClickerLogic = clickerLogic;
            HomeButtonClicked = new RelayCommand(() => MenuVisible = true);
            ClickerLogic.OnOpenStockWindow += OpenStockWindow;
            ClickerLogic.OnCloseStockWindow += HideStockWindow;
            ClickerLogic.OnOpenTutorial += OpenTutorialWindow;
            ClickerLogic.OnCloseTutorial += HideTutorialWindow;

            myMusicPlayer = new MyMusicPlayer(Utility.MUSIC_PATH);
            Volume = 50;
            FxVolume = 50;
            myMusicPlayer.Play();
        }

        public bool OptionMenuVisible
        {
            get => optionMenuVisible;
            set
            {
                if (SetProperty(ref optionMenuVisible, value))
                    if (value)
                    {
                        GameVisible = false;
                        MenuVisible = false;
                    }
            }
        }

        public bool GameVisible
        {
            get => gameVisible;
            set
            {
                if (SetProperty(ref gameVisible, value))
                    if (value)
                    {
                        OptionMenuVisible = false;
                        MenuVisible = false;
                    }
            }
        }

        public bool MenuVisible
        {
            get => menuVisible;
            set
            {
                if (SetProperty(ref menuVisible, value))
                    if (value)
                    {
                        OptionMenuVisible = false;
                        GameVisible = false;
                    }
            }
        }


        public void OpenStockWindow()
        {
            try
            {
                stockWindow.Show();
            }
            catch (InvalidOperationException) { }
        }

        public void HideStockWindow()
        {
            stockWindow.Hide();
        }

        public void OpenTutorialWindow()
        {
            try
            {
                tutorialWindow.Show();
            }
            catch (InvalidOperationException) { }
        }

        public void HideTutorialWindow()
        {
            tutorialWindow.Hide();
        }
    }
}
