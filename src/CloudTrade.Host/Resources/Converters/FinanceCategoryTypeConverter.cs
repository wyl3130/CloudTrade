using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Host.Resources.Converters
{
    public class FinanceCategoryTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int type)
            {
                return type == 0 ? "收入" : "支出";
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // 如果需要支持双向绑定并进行反向转换（将“收入”/“支出”转换为0/1），可以实现该方法
            return value.ToString() == "收入" ? 0 : 1;
        }
    }
}
