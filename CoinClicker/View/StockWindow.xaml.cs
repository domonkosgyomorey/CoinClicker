using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace CoinClicker
{
    /// <summary>
    /// Interaction logic for Stock.xaml
    /// </summary>
    public partial class StockWindow : Window
    {
        public StockWindow(StockWindowViewModel stockWindowViewModel)
        {
            DataContext = stockWindowViewModel;

            InitializeComponent();

            StockPriceChart chart = new StockPriceChart(stockWindowViewModel.StockLogic);
            chart.Margin = new Thickness(40);

            Grid.SetRow(chart, 1);
            Grid.SetColumn(chart, 0);
            OuterGrid.Children.Add(chart);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            Hide();
            e.Cancel = true;
        }
    }
}
