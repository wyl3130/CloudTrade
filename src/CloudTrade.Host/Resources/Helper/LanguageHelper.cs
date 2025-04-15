using CloudTrade.Domain.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Host.Resources.Helper
{
    public class LanguageHelper
    {
        public static void switchLanguage(Language language)
        {
            var mergedDictionaries = System.Windows.Application.Current.Resources.MergedDictionaries;
            // 资源字典的源路径
            string[] pathsToRemove = new string[]
            {
                        "pack://application:,,,/CloudTrade.Host;component/Resources/Language/zh.xaml",
                        "pack://application:,,,/CloudTrade.Host;component/Resources/Language/en.xaml"
            };

            // 遍历并移除匹配的资源字典
            foreach (var dictionary in mergedDictionaries.ToList()) // 使用 ToList() 避免在遍历时修改集合
            {
                // 只移除具有有效 Source 属性的 ResourceDictionary
                if (dictionary.Source != null && pathsToRemove.Contains(dictionary.Source.OriginalString))
                {
                    mergedDictionaries.Remove(dictionary);
                }
            }
            switch (language)
            {
                case Language.简体中文:

                    System.Windows.Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary()
                    {
                        Source = new Uri("pack://application:,,,/CloudTrade.Host;component/Resources/Language/zh.xaml")
                    });
                    break;
                case Language.English:

                    System.Windows.Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary()
                    {
                        Source = new Uri("pack://application:,,,/CloudTrade.Host;component/Resources/Language/en.xaml")
                    });
                    break;
                default:
                    break;
            }
        }
    }
}
