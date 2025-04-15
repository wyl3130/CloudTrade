using CloudTrade.Application.Contracts.CommodityStocks;
using CloudTrade.Domain.WareHouses;
using CloudTrade.Host.Resources.Event;
using CloudTrade.Host.Resources.Helper;
using HandyControl.Controls;
using Prism.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace CloudTrade.Host.ViewModels.CommodityStocks
{
    public class CommodityStockViewModel : BindableBase, INavigationAware
    {
        private readonly ICommodityStockService db;
        private readonly IEventAggregator eventAggregator;
        public CommodityStockViewModel(ICommodityStockService db, IEventAggregator eventAggregator)
        {
            this.db = db;
            InitData(); WareHouseList = new ObservableCollection<WareHouse>(db.Queryable<WareHouse>());
            this.eventAggregator = eventAggregator;
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

                        await Task.Delay(2000);

                        // 在后台线程运行耗时操作
                        await Task.Run(() =>
                        {
                            // 执行初始化数据操作
                            InitData(PageHelper.PageSize(Convert.ToInt32(arg[0])), arg[1]?.ToString() ?? "", arg[2]?.ToString() ?? "");
                        });


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

        public async void InitData(int PageSize = 10, string query = "", string WareHouseName = "")
        {

            (var list, PageCount) = await db.CommodityStockQueryAsync(PageIndex, PageSize, query, WareHouseName);
            CommodityStockList = new ObservableCollection<CommodityStockDto>(list);

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

        private ObservableCollection<CommodityStockDto> commodityStockList;
        public ObservableCollection<CommodityStockDto> CommodityStockList
        {
            get => commodityStockList;
            set => SetProperty(ref commodityStockList, value);
        }
        private ObservableCollection<WareHouse> wareHouseList;
        public ObservableCollection<WareHouse> WareHouseList
        {
            get => wareHouseList;
            set => SetProperty(ref wareHouseList, value);
        }


    }
}
