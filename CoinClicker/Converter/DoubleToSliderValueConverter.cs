using System.Globalization;
using System.Windows.Data;

namespace CoinClicker
{
    public class DoubleToSliderValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double val = (double)value;
            return Math.Max(Utility.MIN_STOCK_BUY_VAL, Math.Min(Utility.MAX_STOCK_BUY_VAL, val));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
