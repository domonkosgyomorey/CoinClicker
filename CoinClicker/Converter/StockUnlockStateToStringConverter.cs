using System.Globalization;
using System.Windows.Data;

namespace CoinClicker
{
    public class StockUnlockStateToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (Player.STOCK_STATE)value switch
            {
                Player.STOCK_STATE.LOCKED => "Unlock",
                Player.STOCK_STATE.CLOSED => "Open",
                Player.STOCK_STATE.OPENED => "Close",
                Player.STOCK_STATE.STOCK_STATE_SIZE => "Unreachable",
                _ => "Unreachable"
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
