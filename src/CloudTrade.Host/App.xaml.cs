



using CloudTrade.Application.CommodityStocks;
using CloudTrade.Application.Contracts.CommodityStocks;
using CloudTrade.Application.Contracts.CustomerCategorys;
using CloudTrade.Application.Contracts.CustomerCompanys;
using CloudTrade.Application.Contracts.Departments;
using CloudTrade.Application.Contracts.Employees;
using CloudTrade.Application.Contracts.FinanceCategorys;
using CloudTrade.Application.Contracts.Homes;
using CloudTrade.Application.Contracts.ModeInfos;
using CloudTrade.Application.Contracts.OtherFinances;
using CloudTrade.Application.Contracts.PaymentAccounts;
using CloudTrade.Application.Contracts.PaymentCategorys;
using CloudTrade.Application.Contracts.Payments;
using CloudTrade.Application.Contracts.Positions;
using CloudTrade.Application.Contracts.PurchaseOrders;
using CloudTrade.Application.Contracts.PurchaseRefunds;
using CloudTrade.Application.Contracts.PurchaseWareHouses;
using CloudTrade.Application.Contracts.Receipts;
using CloudTrade.Application.Contracts.SalesExWareHouses;
using CloudTrade.Application.Contracts.SalesModes;
using CloudTrade.Application.Contracts.SalesOrders;
using CloudTrade.Application.Contracts.SalesRefunds;
using CloudTrade.Application.CustomerCategorys;
using CloudTrade.Application.CustomerCompanys;
using CloudTrade.Application.Departments;
using CloudTrade.Application.Employees;
using CloudTrade.Application.FinanceCategorys;
using CloudTrade.Application.Homes;
using CloudTrade.Application.ModeInfos;
using CloudTrade.Application.OtherFinances;
using CloudTrade.Application.PaymentAccounts;
using CloudTrade.Application.PaymentCategorys;
using CloudTrade.Application.Payments;
using CloudTrade.Application.Positions;
using CloudTrade.Application.PurchaseOrders;
using CloudTrade.Application.PurchaseRefunds;
using CloudTrade.Application.PurchaseWarehouses;
using CloudTrade.Application.Receipts;
using CloudTrade.Application.SalesExWarehouses;
using CloudTrade.Application.SalesModes;
using CloudTrade.Application.SalesOrders;
using CloudTrade.Application.SalesRefunds;
using CloudTrade.Domain.Payments;
using CloudTrade.Domain.SalesExWareHouses;
using CloudTrade.Domain.Shared.Logs;
using CloudTrade.Domain.Shared.Prisms;
using CloudTrade.Host.Resources.Helper;
using CloudTrade.Host.ViewModels.Commoditys;
using CloudTrade.Host.ViewModels.Homes;
using CloudTrade.Host.ViewModels.Mains;
using CloudTrade.Host.ViewModels.PurchaseWareHouses;
using CloudTrade.Host.Views.CommodityStocks;
using CloudTrade.Host.Views.CompanyInfos;
using CloudTrade.Host.Views.CustomerCategorys;
using CloudTrade.Host.Views.CustomerCompanys;
using CloudTrade.Host.Views.Departments;
using CloudTrade.Host.Views.Employees;
using CloudTrade.Host.Views.FinanceCategorys;
using CloudTrade.Host.Views.Homes;
using CloudTrade.Host.Views.Mains;
using CloudTrade.Host.Views.ModeInfos;
using CloudTrade.Host.Views.OtherFinances;
using CloudTrade.Host.Views.PaymentAccounts;
using CloudTrade.Host.Views.PaymentCategorys;
using CloudTrade.Host.Views.Payments;
using CloudTrade.Host.Views.Positions;
using CloudTrade.Host.Views.PurchaseOrders;
using CloudTrade.Host.Views.PurchaseRefunds;
using CloudTrade.Host.Views.PurchaseWareHouses;
using CloudTrade.Host.Views.Receipts;
using CloudTrade.Host.Views.SalesExWareHouses;
using CloudTrade.Host.Views.SalesModes;
using CloudTrade.Host.Views.SalesOrders;
using CloudTrade.Host.Views.SalesRefunds;
using CloudTrade.Host.Views.SystemInfos;
using CloudTrade.Host.Views.WareHouses;
using OfficeOpenXml;
using System.Reflection;
using Unity.Lifetime;

namespace CloudTrade.Host
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        private readonly Configuration configuration;
        public App()
        {
            ExcelPackage.License.SetNonCommercialPersonal("Your Name");
            //应用程序域
            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
            {
                Log.LogError((Exception)(e.ExceptionObject));
            };
            //应用程序
            Current.DispatcherUnhandledException += (s, e) =>
            {
                Log.LogError(e.Exception);
            };
            //多线程
            TaskScheduler.UnobservedTaskException += (s, e) =>
            {
                Log.LogError(e.Exception);
            };
            this.configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None); 
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            Log.LogInfo("原神启动");
            var mode = configuration.AppSettings.Settings["ThemeMode"].Value;
            ThemeModeHelper.switchThemeMode(mode);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            Log.LogInfo("应用程序关闭");
        }
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindowView>();
        }
        protected override void ConfigureViewModelLocator()
        {
            base.ConfigureViewModelLocator();
        }
        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            base.ConfigureModuleCatalog(moduleCatalog);
        }

        protected override void ConfigureDefaultRegionBehaviors(IRegionBehaviorFactory regionBehaviors)
        {
            base.ConfigureDefaultRegionBehaviors(regionBehaviors);
        }

       
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            //服务
            containerRegistry.RegisterSingleton<ISqlSugarClient>(x =>
            {
                SqlSugarScope client = new SqlSugarScope(new ConnectionConfig()
                {
                    DbType = SqlSugar.DbType.SqlServer,
                    ConnectionString = configuration.AppSettings.Settings["connStr"].Value,
                    IsAutoCloseConnection = true,
                    InitKeyType = InitKeyType.Attribute,
                }, db =>
                {
                    db.Aop.OnLogExecuting = (sql, pars) =>
                    {

                    };
                });
                return client;
            });

            //main
            containerRegistry.RegisterSingleton<IBaseService, BaseService>();
            containerRegistry.RegisterSingleton<IUserService, UserService>();
            containerRegistry.RegisterSingleton<IMenuService, MenuService>();
            containerRegistry.RegisterSingleton<IDashboardService, DashboardService>();

            //进货
            containerRegistry.RegisterSingleton<IPurchaseOrderService, PurchaseOrderService>();
            containerRegistry.RegisterSingleton<IPurchaseRefundService, PurchaseRefundService>();
            containerRegistry.RegisterSingleton<IPurchaseWareHouseService, PurchaseWareHouseService>();

            //销售
            containerRegistry.RegisterSingleton<ISalesExWareHouseService, SalesExWareHouseService>();
            containerRegistry.RegisterSingleton<ISalesOrderService, SalesOrderService>();
            containerRegistry.RegisterSingleton<ISalesRefundService, SalesRefundService>();
            //人事
            containerRegistry.RegisterSingleton<IDepartmentService, DepartmentService>();
            containerRegistry.RegisterSingleton<IPositionService, PositionService>();
            containerRegistry.RegisterSingleton<IEmployeService, EmployeService>();

            //综合
            containerRegistry.RegisterSingleton<IFinanceCategoryService, FinanceCategoryService>();
            containerRegistry.RegisterSingleton<IReceiptService, ReceiptService>();
            containerRegistry.RegisterSingleton<IPaymentService, PaymentService>();
            containerRegistry.RegisterSingleton<IOtherFinanceService, OtherFinanceService>();

            //支付
            containerRegistry.RegisterSingleton<IPaymentCategoryService, PaymentCategoryService>();
            containerRegistry.RegisterSingleton<IPaymentAccountService, PaymentAccountService>();
            containerRegistry.RegisterSingleton<ICustomerCompanyService, CustomerCompanyService>();
            containerRegistry.RegisterSingleton<ICustomerCategoryService, CustomerCategoryService>();

            //方式
            containerRegistry.RegisterSingleton<ISalesModeService, SalesModeService>();
            containerRegistry.RegisterSingleton<IModeInfoService, ModeInfoService>();

            //商品
            containerRegistry.RegisterSingleton<ICommodityService, CommodityService>();
            containerRegistry.RegisterSingleton<ICommodityCategoryService, CommodityCategoryService>();
            containerRegistry.RegisterSingleton<ICommodityCompanyService, CommodityCompanyService>();
            containerRegistry.RegisterSingleton<ICommodityStockService, CommodityStockService>();




            //导航视图

            //main

            //首页
            containerRegistry.RegisterForNavigation<DashboardView>();


            //
            containerRegistry.RegisterForNavigation<PurchaseOrderView>(); 
            containerRegistry.RegisterForNavigation<PurchaseRefundView>(); 
            containerRegistry.RegisterForNavigation<PurchaseWareHouseView>();
            //PurchaseRefundView
            containerRegistry.RegisterForNavigation<SalesExWareHouseView>();
            containerRegistry.RegisterForNavigation<SalesOrderView>();
            containerRegistry.RegisterForNavigation<SalesRefundView>();

            //人事
            containerRegistry.RegisterForNavigation<DepartmentView>();
            containerRegistry.RegisterForNavigation<PositionView>();
            containerRegistry.RegisterForNavigation<EmployeView>();

            //财务
            containerRegistry.RegisterForNavigation<PaymentView>();
            containerRegistry.RegisterForNavigation<ReceiptView>();
            containerRegistry.RegisterForNavigation<OtherFinanceView>();
            //综合
            containerRegistry.RegisterForNavigation<PaymentCategoryView>();
            containerRegistry.RegisterForNavigation<PaymentAccountView>();
            containerRegistry.RegisterForNavigation<ModeInfoView>();
            containerRegistry.RegisterForNavigation<SalesModeView>();
            containerRegistry.RegisterForNavigation<CustomerCategoryView>();
            containerRegistry.RegisterForNavigation<CustomerCompanyView>();
            containerRegistry.RegisterForNavigation<FinanceCategoryView>();


            //仓库
            containerRegistry.RegisterForNavigation<WareHouseView>();



            //商品
            containerRegistry.RegisterForNavigation<CommodityView>();
            containerRegistry.RegisterForNavigation<CommodityCategoryView>();
            containerRegistry.RegisterForNavigation<CommodityCompanyView>();


            //查询
            containerRegistry.RegisterForNavigation<CommodityStockView>();


            //系统
            containerRegistry.RegisterForNavigation<SystemInfoView>();
            containerRegistry.RegisterForNavigation<CompanyInfoView>();
            containerRegistry.RegisterForNavigation<ApplicationSettingView>();


            

        }

    }

}
