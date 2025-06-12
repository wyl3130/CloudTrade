using CloudTrade.Application.Contracts.PaymentCategorys;
using CloudTrade.Domain.Commoditys;
using CloudTrade.Domain.PaymentAccounts;
using CloudTrade.Domain.PaymentCategorys;
using CloudTrade.Host.Resources.Event;
using CloudTrade.Host.Resources.Helper;
using CloudTrade.Host.ViewModels.CommodityCompanys;
using CloudTrade.Host.ViewModels.PaymentAccounts;
using CloudTrade.Host.Views.PaymentAccounts;
using CloudTrade.Host.Views.PaymentCategorys;
using HandyControl.Controls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Host.ViewModels.PaymentCategorys
{
    public class PaymentCategoryViewModel : BindableBase, INavigationAware
    {
        private readonly IPaymentCategoryService db;
        private readonly IEventAggregator eventAggregator;
        public PaymentCategoryViewModel(IPaymentCategoryService db, IEventAggregator eventAggregator)
        {
            this.db = db;
            this.eventAggregator = eventAggregator;
            InitData();
        }
        public DelegateCommand InsertCommand
        {
            get
            {
                return new DelegateCommand( () =>
                {
                    try
                    {
                        eventAggregator.GetEvent<VisibilityChangeEvent>().Publish();
                        var view = new PaymentCategoryDialogView();
                        view.Owner = System.Windows.Application.Current.MainWindow;
                        view.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                        var vm = new PaymentCategoryDialogViewModel(db);
                        vm.Title = "添加";
                        view.DataContext = vm;
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

                                foreach (PaymentCategory item in arg)
                                {
                                    await db.DeleteAsync<PaymentCategory>(item);
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
                        else if (arg[0] is PaymentCategory entity)
                        {
                            eventAggregator.GetEvent<VisibilityChangeEvent>().Publish();
                            var view = new PaymentCategoryDialogView();
                            var vm = new PaymentCategoryDialogViewModel(db);
                            view.Owner = System.Windows.Application.Current.MainWindow;
                            view.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                            vm.Title = "修改";
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
                return new DelegateCommand<ArrayList>((arg) =>
                {
                    try
                    {

                        eventAggregator.GetEvent<VisibilityChangeEvent>().Publish();
                        AppData.ShowLoadingWindow();
                        InitData(PageHelper.PageSize(Convert.ToInt32(arg[0])), arg[1]?.ToString() ?? "");

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

        public async void InitData(int PageSize=10,string query = "")
        {
            (var list, PageCount) = await db.PaymentCategoryQueryAsync(PageIndex,PageSize,query);
            PaymentCategoryList = new ObservableCollection<PaymentCategory>(list);
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

        private ObservableCollection<PaymentCategory> paymentCategoryList;
        public ObservableCollection<PaymentCategory> PaymentCategoryList
        {
            get => paymentCategoryList;
            set => SetProperty(ref paymentCategoryList, value);
        }
    }
}

