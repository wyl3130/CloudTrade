using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Host.Test
{
    public class GenderTemplateSelector:DataTemplateSelector
    {
        public DataTemplate MaleTemplate { get; set; }
        public DataTemplate FemaleTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is string gender)
            {
                if (gender == "男")
                    return MaleTemplate;
                else if (gender == "女")
                    return FemaleTemplate;
            }
            return base.SelectTemplate(item, container);
        }
    }
}
