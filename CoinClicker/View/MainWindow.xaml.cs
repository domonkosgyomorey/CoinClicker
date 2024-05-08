using System.ComponentModel;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace CoinClicker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private StockWindow stockWindow;
        private StockWindowViewModel stockWindowViewModel;
        private IStockLogic stockLogic;

        private MainWindowViewModel mainWindowViewModel;
        private IClickerLogic clickerLogic;
        private Player player;

        private TutorialWindow tutorialWindow;
        private TutorialWindowViewModel tutorialWindowViewModel;
        private Tutorial tutorial;


        public MainWindow()
        {
            player = JsonSerializer.Deserialize<Player>(File.ReadAllText(Utility.PLAYER_SAVE_DATA));

            tutorial = JsonSerializer.Deserialize<Tutorial>(File.ReadAllText(Utility.TUTORIAL_CONTENT_SAVE_DATA));
            tutorialWindowViewModel = new TutorialWindowViewModel(tutorial);
            tutorialWindow = new TutorialWindow(tutorialWindowViewModel);

            stockLogic = new StockLogic(player);
            stockLogic.Market = JsonSerializer.Deserialize<List<Stock>>(File.ReadAllText(Utility.STOCKS_SAVE_DATA));

            stockWindowViewModel = new StockWindowViewModel(stockLogic);

            stockWindow = new StockWindow(stockWindowViewModel);

            clickerLogic = new ClickerLogic(player);
            clickerLogic.LoadUpgrades(JsonSerializer.Deserialize<List<Upgrade>>(File.ReadAllText(Utility.UPGRADES_SAVE_DATA)));

            mainWindowViewModel = new MainWindowViewModel(clickerLogic, stockWindow, tutorialWindow);


            DataContext = mainWindowViewModel;

            InitializeComponent();

            Cursor = new Cursor(Utility.CURSOR_PATH);

            BadgeDisplay badgeDisplay = new BadgeDisplay(mainWindowViewModel);
            Grid.SetColumn(badgeDisplay, 0);
            Grid.SetRow(badgeDisplay, 1);
            displayGrid.Children.Add(badgeDisplay);

            Display display = new Display(mainWindowViewModel);
            Grid.SetColumn(display, 0);
            Grid.SetRow(display, 1);
            displayGrid.Children.Add(display);


            stockWindow.Closing += (o, e) => { mainWindowViewModel.ClickerLogic.StockClickCommand.Execute(null); };
            tutorialWindow.Closing += (o, e) => { mainWindowViewModel.ClickerLogic.TutorialUnlockCommand.Execute(null); };
        }

        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void PlayBtn_click(object sender, RoutedEventArgs e)
        {
            (DataContext as MainWindowViewModel).GameVisible = true;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (player.StockState != Player.STOCK_STATE.LOCKED)
                player.StockState = Player.STOCK_STATE.CLOSED;

            if (player.TutorialState != Player.TUTORIAL_STATE.LOCKED)
                player.TutorialState = Player.TUTORIAL_STATE.CLOSED;

            string jsonString = JsonSerializer.Serialize(player);
            File.WriteAllText(Utility.PLAYER_SAVE_DATA, jsonString);

            jsonString = JsonSerializer.Serialize(stockLogic.Market);
            File.WriteAllText(Utility.STOCKS_SAVE_DATA, jsonString);

            jsonString = JsonSerializer.Serialize(clickerLogic.UpgradeManager.GetUpgrades());
            File.WriteAllText(Utility.UPGRADES_SAVE_DATA, jsonString);

            jsonString = JsonSerializer.Serialize(tutorial);
            File.WriteAllText(Utility.TUTORIAL_CONTENT_SAVE_DATA, jsonString);

            Application.Current.Shutdown();
            base.OnClosing(e);
        }

        private void OptionBtn_click(object sender, RoutedEventArgs e)
        {
            (DataContext as MainWindowViewModel).OptionMenuVisible = true;
        }

        private void FullscreenCheckBox_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)FullscreenCheckBox.IsChecked)
            {
                WindowState = WindowState.Maximized;
                WindowStyle = WindowStyle.None;
            }
            else
            {
                WindowState = WindowState.Normal;
                WindowStyle = WindowStyle.SingleBorderWindow;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}