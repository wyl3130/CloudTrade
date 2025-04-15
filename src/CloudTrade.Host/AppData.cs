using CloudTrade.Domain.Shared.Enums;
using CloudTrade.Domain.Users;
using CloudTrade.Host.Resources.Helper;
using CloudTrade.Host.ViewModels.Mains;
using CloudTrade.Host.Views.Mains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Host
{
    public class AppData : BindableBase
    {
        private AppData()
        {

        }

        public static AppData Instance { get; set; } = new Lazy<AppData>(() => new AppData()).Value;

        public static string ApplicationName { get; set; } = "CloudTrade";
        public static Language language { get; set; }

        public static User user { get; set; } = new User()
        {
            UserName = "admin"
        };
        public static IEnumerable<CloudTrade.Domain.Menus.Menu> Menus { get; set; }


        public static void ShowLoadingWindow()
        {
            var loadingWindow = new LoadView();
            loadingWindow.Owner = System.Windows.Application.Current.MainWindow;
            loadingWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            loadingWindow.Topmost = true;
            loadingWindow.Show();
        }
        public static void CloseLoadingWindow()
        {
            var loadingWindow = System.Windows.Application.Current.Windows.OfType<LoadView>().FirstOrDefault();
            if (loadingWindow != null)
            {
                loadingWindow.Close();
            }
        }
    }
}
