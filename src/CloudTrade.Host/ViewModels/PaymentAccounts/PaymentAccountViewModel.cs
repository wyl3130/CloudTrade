using CloudTrade.Application.Contracts.PaymentAccounts;
using CloudTrade.Domain.Commoditys;
using CloudTrade.Domain.PaymentAccounts;
using CloudTrade.Host.Resources.Event;
using CloudTrade.Host.Resources.Helper;
using CloudTrade.Host.ViewModels.CommodityCompanys;
using CloudTrade.Host.Views.PaymentAccounts;
using HandyControl.Controls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Host.ViewModels.PaymentAccounts
{
    public class PaymentAccountViewModel : BindableBase, INavigationAware
    {
        private readonly IPaymentAccountService db;
        private readonly IEventAggregator eventAggregator;
        public PaymentAccountViewModel(IPaymentAccountService db, IEventAggregator eventAggregator)
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
                        var view = new PaymentAccountDialogView();
                        view.Owner = System.Windows.Application.Current.MainWindow;
                        view.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                        var vm = new PaymentAccountDialogViewModel(db);
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
                                foreach (PaymentAccount item in arg)
                                {
                                    await db.DeleteAsync<PaymentAccount>(item);
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
                        else if (arg[0] is PaymentAccount entity)
                        {
                            eventAggregator.GetEvent<VisibilityChangeEvent>().Publish();
                            var view = new PaymentAccountDialogView();
                            var vm = new PaymentAccountDialogViewModel(db);
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
                        AppData.CloseLoadingWindow();
                        eventAggregator.GetEvent<VisibilityChangeEvent>().Publish();
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
            (var list, PageCount) = await db.PaymentAccountQueryAsync(PageIndex,PageSize,query);
            PaymentAccountList = new ObservableCollection<PaymentAccount>(list);
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

        private ObservableCollection<PaymentAccount> paymentAccountList;
        public ObservableCollection<PaymentAccount> PaymentAccountList
        {
            get => paymentAccountList;
            set => SetProperty(ref paymentAccountList, value);
        }
    }
}
