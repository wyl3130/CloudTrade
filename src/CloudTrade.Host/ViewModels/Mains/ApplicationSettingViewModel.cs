using CloudTrade.Domain.Shared.Enums;
using CloudTrade.Host.Resources.Event;
using CloudTrade.Host.Resources.Helper;
using HandyControl.Controls;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Host.ViewModels.Mains
{
    public class ApplicationSettingViewModel : BindableBase
    {
        private readonly IEventAggregator eventAggregator;
        public ApplicationSettingViewModel(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;

            if (ConfigurationManager.AppSettings["Language"] == "简体中文")
            {
                LanguageIndex = 0;
            }
            else
            {
                LanguageIndex = 1;
            }



        }

        public ICommand colorSelectCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {

         
                });
            }
        }
                    

        public DelegateCommand<string> ThemeCommand
        {
            get
            {
                return new DelegateCommand<string>((arg) =>
                {
                    string mode = arg;


                    var mergedDictionaries = System.Windows.Application.Current.Resources.MergedDictionaries;
                    string[] pathsToRemove = new string[]
                    {
                        "pack://application:,,,/HandyControl;component/Themes/SkinDefault.xaml",
                        "pack://application:,,,/HandyControl;component/Themes/SkinDark.xaml",
                        "pack://application:,,,/HandyControl;component/Themes/SkinViolet.xaml",
                        "pack://application:,,,/CloudTrade.Host;component/Resources/Styles/Themes/Red.xaml",
                        "pack://application:,,,/CloudTrade.Host;component/Resources/Styles/Themes/Green.xaml",
                        "pack://application:,,,/CloudTrade.Host;component/Resources/Styles/Themes/Yellow.xaml",
                        "pack://application:,,,/CloudTrade.Host;component/Resources/Styles/Themes/Orange.xaml",
                        "pack://application:,,,/CloudTrade.Host;component/Resources/Styles/Themes/Indigo.xaml",

                    };

                    foreach (var dictionary in mergedDictionaries.ToList())
                    {
                        if (dictionary.Source != null && pathsToRemove.Contains(dictionary.Source.OriginalString))
                        {
                            mergedDictionaries.Remove(dictionary);
                        }
                    }
                    var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                    ThemeModeHelper.switchThemeMode(arg);

                });

            }
        }
        public DelegateCommand<object> LanguageSwitchCommand
        {
            get
            {
                return new DelegateCommand<object>((arg) =>
                {
                    // 获取应用程序配置文件
                    var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                    switch (Convert.ToInt32(arg))
                    {
                        case 0:
                            AppData.language = Language.简体中文;
                            LanguageHelper.switchLanguage(Language.简体中文);
                            config.AppSettings.Settings["Language"].Value = "简体中文";
                            config.Save(ConfigurationSaveMode.Modified);
                            ConfigurationManager.RefreshSection("appSettings");
                            break;
                        case 1:
                            AppData.language = Language.English;
                            LanguageHelper.switchLanguage(Language.English);
                            config.AppSettings.Settings["Language"].Value = "English";
                            config.Save(ConfigurationSaveMode.Modified);
                            ConfigurationManager.RefreshSection("appSettings");
                            break;
                        default:
                            break;
                    }

                    eventAggregator.GetEvent<LanguageChangeEvent>().Publish();
                });
            }
        }
        public DelegateCommand SwaitchCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    var appName = AppData.ApplicationName;  // 自定义应用名称
                    IsCheck = !IsCheck;
                    if (IsCheck)
                    {
                        var appPath = System.Reflection.Assembly.GetExecutingAssembly().Location; // 获取当前程序的路径
                        // 打开注册表的 Run 键
                        var registryKey = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);
                        // 将应用程序路径添加到注册表中
                        registryKey.SetValue(appName, appPath);
                    }
                    else
                    {
                        // 打开注册表的 Run 键
                        var registryKey = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);
                        // 删除对应的注册表项
                        registryKey.DeleteValue(appName, false);
                    }

                });
            }
        }

        private bool isCheck;
        public bool IsCheck
        {
            get { return isCheck; }
            set { SetProperty(ref isCheck, value); }
        }
        private int languageIndex;
        public int LanguageIndex
        {
            get { return languageIndex; }
            set { SetProperty(ref languageIndex, value); }
        }
        public AppData AppData => AppData.Instance;

    }
}
