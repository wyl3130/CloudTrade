using CloudTrade.Domain.Shared.Prisms;
using CloudTrade.Host.Resources.Event;
using CloudTrade.Host.Views.Homes;
using CloudTrade.Host.Views.Mains;
using HandyControl.Controls;

using Prism.Navigation.Regions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
namespace CloudTrade.Host.ViewModels.Mains
{
    public class MainWindowViewModel : BindableBase, INavigationAware
    {
        private readonly IEventAggregator eventAggregator;
        private readonly IRegionManager regionManager;
        private IRegionNavigationJournal journal;
        public MainWindowViewModel(IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            this.regionManager = regionManager;
            this.eventAggregator = eventAggregator;
            eventAggregator.GetEvent<VisibilityChangeEvent>().Subscribe(OnVisibilityChange);


            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += (s, e) =>
            {
                Date = DateTime.Now.ToString("yyyy-MM-dd");
                Weekday = DateTime.Now.ToString("dddd", new System.Globalization.CultureInfo("zh-cn"));
                Time = DateTime.Now.ToString("HH:mm:ss");
            };
            timer.Start();
        }
        // 处理事件的方法
        private void OnVisibilityChange()
        {
            IsHidden = IsHidden == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed;
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {

        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return false;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }
        public DelegateCommand LoadCommand
        {
            get => new DelegateCommand(() =>
            {
                regionManager.RegisterViewWithRegion(regionName: RegionNames.LeftMenuRegion, typeof(TreeMenuView));
                regionManager.RegisterViewWithRegion(regionName: RegionNames.MainRegion, typeof(DashboardView));

                //var view = typeof(TreeMenuView);

            });
        }

        public DelegateCommand minCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    System.Windows.Application.Current.MainWindow.WindowState = WindowState.Minimized;
                });
            }
        }

        private bool IsState { get; set; }
        public DelegateCommand maxCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    var mainWindow = System.Windows.Application.Current.MainWindow;

                    // 获取屏幕可用的工作区域 (不包括任务栏)
                    var workingArea = SystemParameters.WorkArea;



                    if (IsState)
                    {
                        //mainWindow.Width = 1360;
                        //mainWindow.Height = 768;
                        //mainWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                        IsState = !IsState;

                        mainWindow.Left = rcnormal.Left;
                        mainWindow.Top = rcnormal.Top;
                        mainWindow.Width = rcnormal.Width;
                        mainWindow.Height = rcnormal.Height;
                    }
                    else
                    {
                        //mainWindow.Top = workingArea.Top;            // 设置窗口位置
                        //mainWindow.Left = workingArea.Left;          // 设置窗口位置
                        //mainWindow.Width = workingArea.Width;        // 设置窗口宽度
                        //mainWindow.Height = workingArea.Height;      // 设置窗口高度
                        //mainWindow.WindowStartupLocation = WindowStartupLocation.Manual;  // 不影响位置


                        IsState = !IsState;

                        rcnormal = new Rect(mainWindow.Left, mainWindow.Top, mainWindow.Width, mainWindow.Height);//保存下当前位置与大小
                        mainWindow.Left = 0;//设置位置
                        mainWindow.Top = 0;
                        Rect rc = SystemParameters.WorkArea;//获取工作区大小
                        mainWindow.Width = rc.Width;
                        mainWindow.Height = rc.Height;
                    }


                    //// 判断当前窗口的状态
                    //if (mainWindow.WindowState == WindowState.Maximized)
                    //{
                    //    mainWindow.WindowState = WindowState.Normal; // 恢复到原始窗口大小
                    //}
                    //else
                    //{
                    //    // 设置为最大化并避免遮挡任务栏
                    //    //mainWindow.WindowState = WindowState.Normal; // 先恢复原始状态

                    //    //mainWindow.WindowState = WindowState.Maximized; // 最后最大化窗口
                    //}



                    //if (mainWindow.WindowState == WindowState.Maximized)
                    //{
                    //    mainWindow.WindowState = WindowState.Normal;
                    //}
                    //else if (mainWindow.WindowState == WindowState.Normal)
                    //{
                    //    mainWindow.WindowState = WindowState.Maximized;
                    //    //代码在这
                    //    mainWindow.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
                    //    mainWindow.MaxWidth = SystemParameters.MaximizedPrimaryScreenWidth;

                    //    mainWindow.Top = workingArea.Top;            // 设置窗口位置
                    //    mainWindow.Left = workingArea.Left;          // 设置窗口位置
                    //    mainWindow.Width = workingArea.Width;        // 设置窗口宽度
                    //    mainWindow.Height = workingArea.Height;      // 设置窗口高度
                    //}



                });
            }
        }
        Rect rcnormal;//定义一个全局rect记录还原状态下窗口的位置和大小。
        public DelegateCommand hideCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    //System.Windows.Application.Current.MainWindow.WindowState = WindowState.Minimized;

                    System.Windows.Application.Current.MainWindow.Hide();
                });
            }
        }

        public DelegateCommand<object> SwitchItemCommand
        {
            get
            {
                return new DelegateCommand<object>((arg) =>
                {
                    //SideMenuItem sideMenuItem = arg as SideMenuItem;

                    HandyControl.Data.FunctionEventArgs<object> eventArg = arg as HandyControl.Data.FunctionEventArgs<object>;
                    if (eventArg != null)
                    {
                        SideMenuItem item = eventArg.Info as SideMenuItem;
                        switch (item.Header)
                        {
                            case "仪表盘":
                            case "Dashboard":
                                regionManager.RequestNavigate(regionName: RegionNames.MainRegion, "DashboardView", options => journal = options.Context.NavigationService.Journal);
                              //  ShowTabItem("DashboardView");
                                break;
                            case "SalesOrder":
                            case "销售订单":
                                regionManager.RequestNavigate(regionName: RegionNames.MainRegion, "SalesOrderView", options => journal = options.Context.NavigationService.Journal);
                                break;
                            case "SalesExWareHouse":
                            case "销售出库":
                                regionManager.RequestNavigate(regionName: RegionNames.MainRegion, "SalesExWareHouseView", options => journal = options.Context.NavigationService.Journal);
                                break;
                            case "SalesRefund":
                            case "销售退货":
                                regionManager.RequestNavigate(regionName: RegionNames.MainRegion, "SalesRefundView", options => journal = options.Context.NavigationService.Journal);
                                break;
                            case "PurchaseOrder":
                            case "采购订单":
                                regionManager.RequestNavigate(regionName: RegionNames.MainRegion, "PurchaseOrderView", options => journal = options.Context.NavigationService.Journal);
                              //  ShowTabItem("PurchaseOrderView");
                                break;
                            case "PurchaseExWareHouse":
                            case "采购入库":
                                regionManager.RequestNavigate(regionName: RegionNames.MainRegion, "PurchaseWareHouseView", options => journal = options.Context.NavigationService.Journal);
                               // ShowTabItem("PurchaseWareHouseView");
                                break;
                            case "PurchaseRefund":
                            case "采购退货":
                                regionManager.RequestNavigate(regionName: RegionNames.MainRegion, "PurchaseRefundView", options => journal = options.Context.NavigationService.Journal);
                               // ShowTabItem("PurchaseRefundView");
                                break;
                            case "Department":
                            case "部门管理":
                                regionManager.RequestNavigate(regionName: RegionNames.MainRegion, "DepartmentView", options => journal = options.Context.NavigationService.Journal);
                                break;
                            case "Position":
                            case "职位管理":
                                regionManager.RequestNavigate(regionName: RegionNames.MainRegion, "PositionView", options => journal = options.Context.NavigationService.Journal);
                                break;
                            case "Employees":
                            case "员工管理":
                                regionManager.RequestNavigate(regionName: RegionNames.MainRegion, "EmployeView", options => journal = options.Context.NavigationService.Journal);
                                break;

                            case "Receipt":
                            case "收款单":
                                regionManager.RequestNavigate(regionName: RegionNames.MainRegion, "ReceiptView", options => journal = options.Context.NavigationService.Journal);
                                break;
                            case "Payments":
                            case "付款单":
                                regionManager.RequestNavigate(regionName: RegionNames.MainRegion, "PaymentView", options => journal = options.Context.NavigationService.Journal);
                                break;
                            case "OtherFinance":
                            case "其他收支":
                                regionManager.RequestNavigate(regionName: RegionNames.MainRegion, "OtherFinanceView", options => journal = options.Context.NavigationService.Journal);
                                break;


                            case "CustomerCategory":
                            case "单位类别":
                                regionManager.RequestNavigate(regionName: RegionNames.MainRegion, "CustomerCategoryView", options => journal = options.Context.NavigationService.Journal);
                                break;
                            case "CustomerCompany":
                            case "往来单位":
                                regionManager.RequestNavigate(regionName: RegionNames.MainRegion, "CustomerCompanyView", options => journal = options.Context.NavigationService.Journal);
                                break;
                            case "ModeInfo":
                            case "销售方式":
                                regionManager.RequestNavigate(regionName: RegionNames.MainRegion, "ModeInfoView", options => journal = options.Context.NavigationService.Journal);
                                break;
                            case "SalesMode":
                            case "货品销售方式":
                                regionManager.RequestNavigate(regionName: RegionNames.MainRegion, "SalesModeView", options => journal = options.Context.NavigationService.Journal);
                                break;
                            case "PaymentAccount":
                            case "支付账户":
                                regionManager.RequestNavigate(regionName: RegionNames.MainRegion, "PaymentAccountView", options => journal = options.Context.NavigationService.Journal);
                                break;
                            case "PaymentCategory":
                            case "支付方式":
                                regionManager.RequestNavigate(regionName: RegionNames.MainRegion, "PaymentCategoryView", options => journal = options.Context.NavigationService.Journal);
                                break;
                            case "FinanceCategory":
                            case "项目分类":
                                regionManager.RequestNavigate(regionName: RegionNames.MainRegion, "FinanceCategoryView", options => journal = options.Context.NavigationService.Journal);
                                break;
                            case "WareHouse":
                            case "仓库信息":
                                regionManager.RequestNavigate(regionName: RegionNames.MainRegion, "WareHouseView", options => journal = options.Context.NavigationService.Journal);
                                break;
                            case "单位管理":
                            case "Company":
                                regionManager.RequestNavigate(regionName: RegionNames.MainRegion, "CommodityCompanyView", options => journal = options.Context.NavigationService.Journal);
                                break;
                            case "类别管理":
                            case "Category":
                                regionManager.RequestNavigate(regionName: RegionNames.MainRegion, "CommodityCategoryView", options => journal = options.Context.NavigationService.Journal);
                                break;
                            case "货品管理":
                            case "Commodity":
                                regionManager.RequestNavigate(regionName: RegionNames.MainRegion, "CommodityView", options => journal = options.Context.NavigationService.Journal);
                                break;
                            case "CommodityStock":
                            case "库存查询":
                                regionManager.RequestNavigate(regionName: RegionNames.MainRegion, "CommodityStockView", options => journal = options.Context.NavigationService.Journal);
                                break;
                            case "SystemInfo":
                            case "系统信息":
                                regionManager.RequestNavigate(regionName: RegionNames.MainRegion, "SystemInfoView", options => journal = options.Context.NavigationService.Journal);
                                break;
                            case "CompanyInfo":
                            case "公司信息":
                                regionManager.RequestNavigate(regionName: RegionNames.MainRegion, "CompanyInfoView", options => journal = options.Context.NavigationService.Journal);
                                break;

                        }
                    }
                    //HandyControl.Controls.MessageBox.Show(arg.Header);
                });
            }
        }

        public DelegateCommand DelegateCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    NotifyIcon notifyIcon = new NotifyIcon()
                    {
                        //Title = "CloudTrade",
                        //Message = "CloudTrade",
                        //Icon = System.Windows.Application.Current.FindResource("Icon") as ImageSource,
                        //ShowCloseButton = false,
                        //ShowMinButton = false,
                        //ShowMaxButton = false,
                        //ShowHelpButton = false,
                        //ShowInTaskbar = false,
                        //ShowInNotificationArea = true,
                        //BalloonTipTitle = "CloudTrade",
                        //BalloonTipText = "CloudTrade",
                        //BalloonTipIcon = ToolTipIcon.Info,
                        //BalloonTipClicked = () =>
                        //{
                        //    System.Windows.Application.Current.MainWindow.Show();
                        //    System.Windows.Application.Current.MainWindow.WindowState = WindowState.Normal;
                        //}
                        //Icon = System.Windows.Application.Current.FindResource("Icon") as ImageSource,
                    };
                });
            }
        }
        public DelegateCommand LogoutCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    MessageBoxResult result = HandyControl.Controls.MessageBox.Show("是否退出云商", "CloudTrade", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        System.Windows.Application.Current.MainWindow.Close();
                    }

                });
            }
        }

        public DelegateCommand AppSetCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    //regionManager.RequestNavigate("MainRegion", TargetView);
                    regionManager.RequestNavigate(regionName: RegionNames.MainRegion, source: ViewNames.ApplicationSettingView);


                });
            }
        }

        public DelegateCommand LastCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {

                    if (journal != null) journal.GoBack();


                });
            }
        }
        public DelegateCommand NextCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {

                    if (journal != null) journal.GoForward();

                });
            }
        }
        public DelegateCommand RefreshCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {

                    var view = regionManager.Regions[CloudTrade.Domain.Shared.Prisms.RegionNames.MainRegion].ActiveViews.FirstOrDefault().ToString();
                    //System.Windows. MessageBox.Show(list.ToString());
                    regionManager.RequestNavigate(regionName: RegionNames.MainRegion, view);



                });
            }
        }
        private List<System.Windows.Controls.TabItem> tabs = new List<System.Windows.Controls.TabItem>();
        private System.Windows.Controls.TabControl tabControl;
        public DelegateCommand<System.Windows.Controls.TabControl> TabControlLoadedCommand
        {
            get
            {
                return new DelegateCommand<System.Windows.Controls.TabControl>((arg) =>
                {
                    this.tabControl = arg;
                });
            }
        }

        public DelegateCommand<string> OnOpenTabItemCommand
        {
            get
            {
                return new DelegateCommand<string>((arg) =>
                {
                    regionManager.RequestNavigate(regionName: RegionNames.MainRegion, arg);
                    ShowTabItem(arg);
                });
            }
        }
        //显示某个tab页
        private void ShowTabItem(string viewName)
        {
            foreach (var item in tabControl.Items)
            {
                var name = item.GetType().Name;
                if (name == viewName)
                {
                    var tabItem = tabs.FirstOrDefault(t => t.Content.GetType().Name == viewName);
                    if (tabItem != null)
                    {
                        tabItem.Visibility = System.Windows.Visibility.Visible;
                        (tabItem.Content as UserControl).Visibility = System.Windows.Visibility.Visible;
                    }

                }
            }
        }

        public DelegateCommand<System.Windows.Controls.TabItem> RemoveTabItemCommand
        {
            get
            {
                return new DelegateCommand<System.Windows.Controls.TabItem>((arg) =>
                {
                    arg.Visibility = System.Windows.Visibility.Collapsed;
                    (arg.Content as UserControl).Visibility = System.Windows.Visibility.Collapsed;
                    tabControl.SelectedIndex = 0;
                    tabs.Add(arg);
                });
            }
        }
        //隐藏某个tab页
        private void OnRemoveTabItemCommand(System.Windows.Controls.TabItem tabItem)
        {
            tabItem.Visibility = System.Windows.Visibility.Collapsed;
            (tabItem.Content as UserControl).Visibility = System.Windows.Visibility.Collapsed;
            tabControl.SelectedIndex = 0;
            tabs.Add(tabItem);
        }
        private bool isCheck = false;
        public bool IsCheck
        {
            get { return isCheck; }
            set { SetProperty(ref isCheck, value); }
        }
        private Visibility isHidden = Visibility.Collapsed;
        public Visibility IsHidden
        {
            get { return isHidden; }
            set { SetProperty(ref isHidden, value); }
        }

        private string date;
        public string Date
        {
            get { return date; }
            set { SetProperty(ref date, value); }
        }
        private string weekday;
        public string Weekday
        {
            get { return weekday; }
            set { SetProperty(ref weekday, value); }
        }
        private string time;
        public string Time
        {
            get { return time; }
            set { SetProperty(ref time, value); }
        }
        private DispatcherTimer timer;

        public AppData AppData => AppData.Instance;
    }
}
