using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Host.Resources.Converters
{
    public class MenuNameToDisplayNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;

            string menuName = value.ToString();

            // 获取资源字典中的值
            var resource = System.Windows.Application.Current.Resources[menuName] as string;
            return resource ?? menuName; // 如果资源字典中找不到该项，则返回菜单名本身
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null; // 不需要实现转换回的逻辑
        }
    }
}
