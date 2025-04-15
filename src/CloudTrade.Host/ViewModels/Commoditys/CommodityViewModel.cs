
using CloudTrade.Domain.Commoditys;
using CloudTrade.Host.Resources.Event;
using CloudTrade.Host.Resources.Helper;
using CloudTrade.Host.ViewModels.CommodityCompanys;
using HandyControl.Controls;
using System.Collections;

namespace CloudTrade.Host.ViewModels.Commoditys
{
    public class CommodityViewModel : BindableBase, INavigationAware
    {
        private readonly ICommodityService db;
        private readonly IEventAggregator eventAggregator;
        public CommodityViewModel(ICommodityService db, IEventAggregator eventAggregator)
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
                        var view = new CommodityDialogView();
                        view.Owner = System.Windows.Application.Current.MainWindow;
                        view.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                        var vm = new CommodityDialogViewModel(db);
                        vm.Title = "添加";
                        view.DataContext = vm;
                        vm.CommodityCategoryList = this.CommodityCategoryList;
                        vm.CommodityCompanyList = this.CommodityCompanyList;
                        bool? result = view.ShowDialog();
                        if (result == true)
                        {
                            AppData.ShowLoadingWindow();
                            InitData();
                            AppData.CloseLoadingWindow();
                            HandyControl.Controls.Growl.Success("添加成功");


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
                            HandyControl.Controls.Growl.Warning("请选择一条数据");
                        }
                        else
                        {
                            eventAggregator.GetEvent<VisibilityChangeEvent>().Publish();
                            var dialog = HandyControl.Controls.MessageBox.Show("是否删除", "删除", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                            if (dialog == MessageBoxResult.Yes)
                            {
                                AppData.ShowLoadingWindow();

                                foreach (CommodityDto item in arg)
                                {
                                    await db.DeleteAsync<Commodity>(item);
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
                        Growl.Error(ex.Message);
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
                            HandyControl.Controls.Growl.Warning("请选中一条数据进行修改");
                        }
                        else if (arg[0] is Commodity entity)
                        {
                            eventAggregator.GetEvent<VisibilityChangeEvent>().Publish();
                            var view = new CommodityDialogView();
                            var vm = new CommodityDialogViewModel(db);
                            vm.Title = "修改";
                            view.Owner = System.Windows.Application.Current.MainWindow;
                            view.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                            view.DataContext = vm;
                            vm.Entity = entity;
                            vm.CommodityCategoryList = this.CommodityCategoryList;
                            vm.CommodityCompanyList = this.CommodityCompanyList;
                            bool? result = view.ShowDialog();
                            if (result == true)
                            {
                                AppData.ShowLoadingWindow();
                                InitData();
                                AppData.CloseLoadingWindow();
                                HandyControl.Controls.Growl.Success("修改成功");

                            }
                            eventAggregator.GetEvent<VisibilityChangeEvent>().Publish();
                        }


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
                return new DelegateCommand<ArrayList>((arg) =>
                {
                    try
                    {
        
                        eventAggregator.GetEvent<VisibilityChangeEvent>().Publish();
                        AppData.ShowLoadingWindow();
                        //var category = arg[2]?.ToString() ?? "";
                        //System.Windows.MessageBox.Show(category);

                        InitData(PageHelper.PageSize(Convert.ToInt32(arg[0])), arg[1]?.ToString() ?? "", arg[2]?.ToString() ?? "", arg[3]?.ToString() ?? "");
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

        public async void InitData(int PageSize = 10, string query = "", string CommodityCategoryName = "", string CommodityCompanyName = "")
        {
            try
            {
                commodityCompanyList = new ObservableCollection<CommodityCompany>(db.Queryable<CommodityCompany>().ToList());
                commodityCategoryList = new ObservableCollection<CommodityCategory>(db.Queryable<CommodityCategory>().ToList());
                (var list, PageCount) = await db.CommodityQueryAsync(PageIndex,PageSize,query, CommodityCategoryName, CommodityCompanyName);
                CommodityList = new ObservableCollection<CommodityDto>(list);
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

        private int pageCount = 1;
        public int PageCount
        {
            get => pageCount;
            set => SetProperty(ref pageCount, value);
        }
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
        private ObservableCollection<CommodityDto> commodityList;
        public ObservableCollection<CommodityDto> CommodityList
        {
            get => commodityList;
            set => SetProperty(ref commodityList, value);
        }

        private ObservableCollection<CommodityCategory> commodityCategoryList;
        public ObservableCollection<CommodityCategory> CommodityCategoryList
        {
            get => commodityCategoryList;
            set => SetProperty(ref commodityCategoryList, value);
        }
        private ObservableCollection<CommodityCompany> commodityCompanyList;
        public ObservableCollection<CommodityCompany> CommodityCompanyList
        {
            get => commodityCompanyList;
            set => SetProperty(ref commodityCompanyList, value);
        }
    }
}
