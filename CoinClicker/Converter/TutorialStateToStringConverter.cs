using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace CoinClicker
{
    public class TutorialStateToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (Player.TUTORIAL_STATE)value switch
            {
                Player.TUTORIAL_STATE.LOCKED => "Unlock",
                Player.TUTORIAL_STATE.CLOSED => "Open",
                Player.TUTORIAL_STATE.OPENED => "Close",
                Player.TUTORIAL_STATE.TUTORIAL_STATE_SIZE => "Unreachable",
                _ => "Unreachable"
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
