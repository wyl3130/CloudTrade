﻿using CloudTrade.Application.Contracts.FinanceCategorys;
using CloudTrade.Application.Contracts.Receipts;
using CloudTrade.Domain.FinanceCategorys;
using CloudTrade.Domain.Receipts;
using CloudTrade.Host.Resources.Event;
using CloudTrade.Host.Resources.Helper;
using CloudTrade.Host.ViewModels.FinanceCategorys;
using CloudTrade.Host.Views.FinanceCategorys;
using CloudTrade.Host.Views.Receipts;
using HandyControl.Controls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Host.ViewModels.Receipts
{
    public class ReceiptViewModel : BindableBase, INavigationAware
    {
        private readonly IEventAggregator eventAggregator;
        private readonly IReceiptService db;
        public ReceiptViewModel(IReceiptService db, IEventAggregator eventAggregator)
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
                        var view = new ReceiptDialogView();
                        view.Owner = System.Windows.Application.Current.MainWindow;
                        view.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                        var vm = new ReceiptDialogViewModel(db);
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
                            HandyControl.Controls.Growl.Warning("请选择一条数据");
                        }
                        else
                        {
                            eventAggregator.GetEvent<VisibilityChangeEvent>().Publish();
                            var dialog = HandyControl.Controls.MessageBox.Show("是否删除", "删除", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                            if (dialog == MessageBoxResult.Yes)
                            {
                                AppData.ShowLoadingWindow();
                                foreach (Receipt item in arg)
                                {
                                    //var clist = await db.QueryableAsync<Commodity>(x => x.CommodityCategoryId.Equals(item.Id));
                                    //if (clist.Count() != 0)
                                    //{
                                    //    Growl.Warning("当前商品类别关联了商品，要删除类别名称请先修改商品的类别");
                                    //    break;
                                    //}
                                    //else
                                    //{
                                    //    await db.DeleteAsync<CommodityCategory>(item);
                                    //}
                                    await db.DeleteAsync<Receipt>(item);
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

                        else if (arg[0] is Receipt entity)
                        {
                            eventAggregator.GetEvent<VisibilityChangeEvent>().Publish();
                            var view = new ReceiptDialogView();
                            var vm = new ReceiptDialogViewModel(db);
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
                                Growl.Success("修改成功");
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
                        InitData(PageHelper.PageSize(Convert.ToInt32(arg[0])), arg[1]?.ToString() ?? "", arg[2]?.ToString() ?? "");

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

        public async void InitData(int PageSize= 10,string query = "",string CustomerCompanyName="")
        {
            try
            {
                (var list, PageCount) = await db.ReceiptQueryAsync(PageIndex,PageSize,query,CustomerCompanyName);
                ReceiptList = new ObservableCollection<ReceiptDto>(list);
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

        private ObservableCollection<ReceiptDto> receiptList;
        public ObservableCollection<ReceiptDto> ReceiptList
        {
            get => receiptList;
            set => SetProperty(ref receiptList, value);
        }
    }
}
