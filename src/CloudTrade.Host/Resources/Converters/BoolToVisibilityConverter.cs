using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Host.Resources.Converters
{
    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int num)
            {
                if (num== 0)
                {
                    return System.Windows.Visibility.Hidden;
                }
                else
                {
                    return Visibility.Visible;
                }

            }
            else
            {
                throw new ArgumentException("value is not int");
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
