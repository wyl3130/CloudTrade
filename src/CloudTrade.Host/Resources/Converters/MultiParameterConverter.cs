using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Host.Resources.Converters
{
    public class MultiParameterConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            //List<object> result = new List<object>();

            //// 遍历所有输入的值并添加到列表中
            //foreach (var value in values)
            //{
            //    if (value is string text)
            //    {
            //        result.Add(text);  // 将非空字符串添加到列表
            //    }
            //    if (value is Guid gid)
            //    {
            //        result.Add(gid);
            //    }

            //    if (value == null)
            //    {
            //        result.Add("");
            //    }
            //    if(value is System.Windows.Controls.ComboBoxItem cbo)
            //    {
            //        result.Add(cbo.Content);
            //    }
            //    if (value is int index)
            //    {
            //        result.Add(value);
            //    }

            //}
            ArrayList list = new ArrayList();
            foreach (var item in values)
            {
                list.Add(item);
            }

            return list;  // 返回包含文本的列表
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
