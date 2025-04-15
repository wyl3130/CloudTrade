using HandyControl.Controls;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Host.Resources.Helper
{
    public static class ThemeModeHelper
    {
        //Violet
        public static void switchThemeMode(string mode)
        {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            switch (mode)
            {
                case "BlueMode":

                    config.AppSettings.Settings["ThemeMode"].Value = "BlueMode";
                    List<ResourceDictionary> llist = new List<ResourceDictionary>() {
        new ResourceDictionary(){
            Source = new Uri("pack://application:,,,/HandyControl;component/Themes/SkinDefault.xaml")
        },
        new ResourceDictionary(){
            Source = new Uri("pack://application:,,,/HandyControl;component/Themes/Theme.xaml")
        }
    }; foreach (var item in llist)
                    {
                        System.Windows.Application.Current.Resources.MergedDictionaries.Add(item);
                    }

                    Growl.Info((string)System.Windows.Application.Current.Resources["BlueMode"]);
                    break;
                case "DarkMode":

                    config.AppSettings.Settings["ThemeMode"].Value = "DarkMode";
                    List<ResourceDictionary> dlist = new List<ResourceDictionary>() {
        new ResourceDictionary(){
            Source = new Uri("pack://application:,,,/HandyControl;component/Themes/SkinDark.xaml")
        },
        new ResourceDictionary(){
            Source = new Uri("pack://application:,,,/HandyControl;component/Themes/Theme.xaml")
        }
    };
                    foreach (var item in dlist)
                    {
                        System.Windows.Application.Current.Resources.MergedDictionaries.Add(item);
                    }
                    Growl.Info((string)System.Windows.Application.Current.Resources["DarkMode"]);
                    break;
                case "VioletMode":

                    config.AppSettings.Settings["ThemeMode"].Value = "VioletMode";
                    List<ResourceDictionary> vlist = new List<ResourceDictionary>() {
        new ResourceDictionary(){
            Source = new Uri("pack://application:,,,/HandyControl;component/Themes/SkinViolet.xaml")
        },
        new ResourceDictionary(){
            Source = new Uri("pack://application:,,,/HandyControl;component/Themes/Theme.xaml")
        }
    }; foreach (var item in vlist)
                    {
                        System.Windows.Application.Current.Resources.MergedDictionaries.Add(item);
                    }
                    Growl.Info((string)System.Windows.Application.Current.Resources["VioletMode"]);
                    break;


                case "RedMode":

                    config.AppSettings.Settings["ThemeMode"].Value = "RedMode";
                    List<ResourceDictionary> rlist = new List<ResourceDictionary>() {
        new ResourceDictionary(){
            Source = new Uri("pack://application:,,,/CloudTrade.Host;component/Resources/Styles/Themes/Red.xaml")
        },
        new ResourceDictionary(){
            Source = new Uri("pack://application:,,,/HandyControl;component/Themes/Theme.xaml")
        }
    }; foreach (var item in rlist)
                    {
                        System.Windows.Application.Current.Resources.MergedDictionaries.Add(item);
                    }
                    Growl.Info((string)System.Windows.Application.Current.Resources["RedMode"]);
                    break;
                case "OrangeMode":

                    config.AppSettings.Settings["ThemeMode"].Value = "OrangeMode";
                    List<ResourceDictionary> olist = new List<ResourceDictionary>() {
        new ResourceDictionary(){
            Source = new Uri("pack://application:,,,/CloudTrade.Host;component/Resources/Styles/Themes/Orange.xaml")
        },
        new ResourceDictionary(){
            Source = new Uri("pack://application:,,,/HandyControl;component/Themes/Theme.xaml")
        }
    }; foreach (var item in olist)
                    {
                        System.Windows.Application.Current.Resources.MergedDictionaries.Add(item);
                    }
                    Growl.Info((string)System.Windows.Application.Current.Resources["OrangeMode"]);
                    break;
                case "YellowMode":

                    config.AppSettings.Settings["ThemeMode"].Value = "YellowMode";
                    List<ResourceDictionary> ylist = new List<ResourceDictionary>() {
        new ResourceDictionary(){
            Source = new Uri("pack://application:,,,/CloudTrade.Host;component/Resources/Styles/Themes/Yellow.xaml")
        },
        new ResourceDictionary(){
            Source = new Uri("pack://application:,,,/HandyControl;component/Themes/Theme.xaml")
        }
    }; foreach (var item in ylist)
                    {
                        System.Windows.Application.Current.Resources.MergedDictionaries.Add(item);
                    }
                    Growl.Info((string)System.Windows.Application.Current.Resources["YellowMode"]);
                    break;
                case "GreenMode":

                    config.AppSettings.Settings["ThemeMode"].Value = "GreenMode";
                    List<ResourceDictionary> glist = new List<ResourceDictionary>() {
        new ResourceDictionary(){
                Source = new Uri("pack://application:,,,/CloudTrade.Host;component/Resources/Styles/Themes/Green.xaml")
        },
        new ResourceDictionary(){
            Source = new Uri("pack://application:,,,/HandyControl;component/Themes/Theme.xaml")
        }
    }; foreach (var item in glist)
                    {
                        System.Windows.Application.Current.Resources.MergedDictionaries.Add(item);
                    }
                    Growl.Info((string)System.Windows.Application.Current.Resources["GreenMode"]);
                    break;
                case "IndigoMode":

                    config.AppSettings.Settings["ThemeMode"].Value = "IndigoMode";
                    List<ResourceDictionary> ilist = new List<ResourceDictionary>() {
        new ResourceDictionary(){
                Source = new Uri("pack://application:,,,/CloudTrade.Host;component/Resources/Styles/Themes/Indigo.xaml")
        },
        new ResourceDictionary(){
            Source = new Uri("pack://application:,,,/HandyControl;component/Themes/Theme.xaml")
        }
    }; foreach (var item in ilist)
                    {
                        System.Windows.Application.Current.Resources.MergedDictionaries.Add(item);
                    }
                    Growl.Info((string)System.Windows.Application.Current.Resources["IndigoMode"]);
                    break;

                default:
                    break;
            }
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }
    }
}
