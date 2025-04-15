
using CloudTrade.Domain.Commoditys;
using CloudTrade.Host.Resources.Event;
using CloudTrade.Host.Resources.Helper;
using CloudTrade.Host.ViewModels.CommodityCategorys;
using HandyControl.Controls;
using System.Collections;

namespace CloudTrade.Host.ViewModels.CommodityCompanys
{
    public class CommodityCompanyViewModel : BindableBase, INavigationAware
    {
        private readonly IEventAggregator eventAggregator;
        private readonly ICommodityCompanyService db;
        public CommodityCompanyViewModel(ICommodityCompanyService db, IEventAggregator eventAggregator)
        {
            this.db = db;
            this.eventAggregator = eventAggregator;
            InitData();
        }
        public DelegateCommand InsertCommand
        {
            get
            {
                return new DelegateCommand(async () =>
                {
                    try
                    {
                        eventAggregator.GetEvent<VisibilityChangeEvent>().Publish();
                        var view = new CommodityCompanyDialogView();
                        view.Owner = System.Windows.Application.Current.MainWindow;
                        view.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                        var vm = new CommodityCompanyDialogViewModel(db);
                        vm.Title = "添加";
                        view.DataContext = vm;
                        bool? result = view.ShowDialog();
                        if (result == true)
                        {
                            AppData.ShowLoadingWindow();
                            InitData();
                            AppData.CloseLoadingWindow();
                            Growl.Success("添加成功");
                        }
                        eventAggregator.GetEvent<VisibilityChangeEvent>().Publish();
                    }
                    catch (Exception ex)
                    {
                        Growl.Error(ex.Message);
                    }
                });
            }
        }
        public DelegateCommand<IList> DeleteCommand
        {
            get
            {
                return new DelegateCommand<IList>(async (arg) =>
                {
                    try
                    {

                
                        if (arg == null) return;
                        if (arg.Count == 0)
                        {
                            Growl.Warning("请选择一条数据");
                        }
                        else
                        {
                            eventAggregator.GetEvent<VisibilityChangeEvent>().Publish();
                            var dialog = HandyControl.Controls.MessageBox.Show("是否删除", "删除", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                            if (dialog == MessageBoxResult.Yes)
                            {
                                AppData.ShowLoadingWindow();

                                foreach (CommodityCompany item in arg)
                                {
                                    var clist = await db.QueryableAsync<Commodity>(x => x.CommodityCompanyId.Equals(item.Id));
                                    if (clist.Count() != 0)
                                    {
                                        Growl.Warning("当前商品单位关联了商品，要删除类别名称请先修改商品的单位");
                                        break;
                                    }
                                    else
                                    {
                                        await db.DeleteAsync<CommodityCompany>(item);
                                    }

                                }
                                InitData();

                                AppData.CloseLoadingWindow();
                                Growl.Success("删除成功");
                            }
                            eventAggregator.GetEvent<VisibilityChangeEvent>().Publish();
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                });
            }
        }



        public DelegateCommand<IList> UpdateCommand
        {
            get
            {
                return new DelegateCommand<IList>(async (arg) =>
                {
                    try
                    {

                        if (arg.Count == 0 || arg.Count > 1 || arg.Count == 0)
                        {
                            Growl.Warning("请选中一条数据进行修改");
                        }
                        else if (arg[0] is CommodityCompany entity)
                        {
                            eventAggregator.GetEvent<VisibilityChangeEvent>().Publish();
                            var view = new CommodityCompanyDialogView();
                            var vm = new CommodityCompanyDialogViewModel(db);
                            vm.Title = "修改";
                            view.Owner = System.Windows.Application.Current.MainWindow;
                            view.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                            view.DataContext = vm;
                            vm.Entity = entity;

                            bool? result = view.ShowDialog();
                            if (result == true)
                            {
                                AppData.ShowLoadingWindow();
                                await Task.Run(async () =>
                                {
                                    await Task.Delay(1000);
                                    InitData();
                                });
                                AppData.CloseLoadingWindow();
                                HandyControl.Controls.Growl.Success("修改成功");
                            }
                        }
                        eventAggregator.GetEvent<VisibilityChangeEvent>().Publish();
                    }
                    catch (Exception ex)
                    {
                        Growl.Error(ex.Message);
                    }


                });
            }
        }
        public DelegateCommand<ArrayList> SelectCommand
        {
            get
            {
                return new DelegateCommand<ArrayList>(async (arg) =>
                {
                    try
                    {

                        eventAggregator.GetEvent<VisibilityChangeEvent>().Publish();

                        AppData.ShowLoadingWindow();

                        InitData(PageHelper.PageSize(Convert.ToInt32(arg[0])), arg[1].ToString());


                        AppData.CloseLoadingWindow();

                        eventAggregator.GetEvent<VisibilityChangeEvent>().Publish();

                    }
                    catch (Exception ex)
                    {
                        Growl.Error(ex.Message);
                    }
                });
            }
        }

        public DelegateCommand infoCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {

                    IsOpen = !IsOpen;
                });
            }
        }

        public async void InitData(int PageSize=0,string query = "")
        {
            try
            {
                (var list, PageCount) = await db.CommodityCompanyQueryAsync(PageIndex,PageSize,query);
                CommodityCompanyList = new ObservableCollection<CommodityCompany>(list);
            }
            catch (Exception ex)
            {
                Growl.Error(ex.Message);
            }

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




        // 页数 默认1页
        private int pageCount = 1;
        public int PageCount
        {
            get => pageCount;
            set => SetProperty(ref pageCount, value);
        }

        // 页下标 默认1
        private int pageIndex = 1;
        public int PageIndex
        {
            get => pageIndex;
            set => SetProperty(ref pageIndex, value);
        }
        private bool isOpen = false;
        public bool IsOpen
        {
            get => isOpen;
            set => SetProperty(ref isOpen, value);
        }

        private ObservableCollection<CommodityCompany> commodityCompanyList;
        public ObservableCollection<CommodityCompany> CommodityCompanyList
        {
            get => commodityCompanyList;
            set => SetProperty(ref commodityCompanyList, value);
        }
    }
}
