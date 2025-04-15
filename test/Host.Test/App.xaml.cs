using Hardcodet.Wpf.TaskbarNotification;
using Host.Test.Views;
using System.Configuration;
using System.Data;
using System.Windows;
using System.Windows.Input;

namespace Host.Test
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        public App()
        {
           
        }
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void OnStartup(StartupEventArgs e)
        {

            _taskbar = (TaskbarIcon)FindResource("Taskbar");
            base.OnStartup(e);
        }

        private TaskbarIcon _taskbar;

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<MainWindow>();

            containerRegistry.RegisterForNavigation<ViewA>();
            containerRegistry.RegisterForNavigation<ViewB>();
        }

        public ICommand btnCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    MessageBox.Show("admin");
                });
            }
        }
    }

}
