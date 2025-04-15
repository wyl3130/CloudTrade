using CloudTrade.Application.Contracts.CustomerCategorys;
using CloudTrade.Domain.Commoditys;
using CloudTrade.Domain.CustomerCategorys;
using CloudTrade.Domain.CustomerCompanys;
using CloudTrade.Host.Resources.Event;
using CloudTrade.Host.Resources.Helper;
using CloudTrade.Host.ViewModels.CommodityCategorys;
using CloudTrade.Host.Views.CustomerCategorys;
using HandyControl.Controls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Host.ViewModels.CustomerCategorys
{
    public class CustomerCategoryViewModel : BindableBase, INavigationAware
    {
        private readonly ICustomerCategoryService db;
        private readonly IEventAggregator eventAggregator;
        public CustomerCategoryViewModel(ICustomerCategoryService db, IEventAggregator eventAggregator)
        {
            this.db = db;
            this.eventAggregator = eventAggregator;
            InitData();
        }
        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return false;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {

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
                        var view = new CustomerCategoryDialogView();
                        view.Owner = System.Windows.Application.Current.MainWindow;
                        view.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                        var vm = new CustomerCategoryDialogViewModel(db);
                        vm.Title = "添加";
                        view.DataContext = vm;
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

                                await Task.Delay(1000);
                                foreach (CustomerCategory item in arg)
                                {
                                    var clist = await db.QueryableAsync<CustomerCompany>(x => x.CustomerCategoryId.Equals(item.Id));
                                    if (clist.Count() != 0)
                                    {
                                        Growl.Warning("当前关联了客户分类，删除前请先处理客户公司");
                                        break;
                                    }
                                    else
                                    {
                                        await db.DeleteAsync<CustomerCategory>(item);
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

                        else if (arg[0] is CustomerCategory entity)
                        {
                            eventAggregator.GetEvent<VisibilityChangeEvent>().Publish();
                            var view = new CustomerCategoryDialogView();
                            var vm = new CustomerCategoryDialogViewModel(db);
                            vm.Title = "修改";
                            view.Owner = System.Windows.Application.Current.MainWindow;
                            view.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                            view.DataContext = vm;
                            vm.Entity = entity;

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
                return new DelegateCommand<ArrayList>(async (arg) =>
                {
                    try
                    {

                        eventAggregator.GetEvent<VisibilityChangeEvent>().Publish();

                        AppData.ShowLoadingWindow();

                        InitData(PageHelper.PageSize(Convert.ToInt32(arg[0])), arg[1]?.ToString()??"");


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

        public async void InitData(int PageSize = 10, string query = "")
        {
            try
            {
                (var list, PageCount) = await db.CustomerCategoryQueryAsync(PageIndex, PageSize, query);
                CustomerCategoryList = new ObservableCollection<CustomerCategory>(list);
            }
            catch (Exception ex)
            {
                Growl.Error(ex.Message);
            }
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

        private ObservableCollection<CustomerCategory> customerCategoryList;
        public ObservableCollection<CustomerCategory> CustomerCategoryList
        {
            get => customerCategoryList;
            set => SetProperty(ref customerCategoryList, value);
        }
    }
}
