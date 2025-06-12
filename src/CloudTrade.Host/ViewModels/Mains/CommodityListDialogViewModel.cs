using CloudTrade.Application.Contracts.PurchaseOrders;
using CloudTrade.Application.Contracts.PurchaseRefunds;
using CloudTrade.Application.Contracts.PurchaseWareHouses;
using CloudTrade.Application.Contracts.SalesExWareHouses;
using CloudTrade.Application.Contracts.SalesOrders;
using CloudTrade.Application.Contracts.SalesRefunds;
using CloudTrade.Domain.Commoditys;
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

namespace CloudTrade.Host.ViewModels.Mains
{
    public class CommodityListDialogViewModel : BindableBase
    {
        private readonly ICommodityService db;
        public CommodityListDialogViewModel(ICommodityService db,Guid WareHouseId=new Guid())
        {
            this.db = db;
            InitData(WareHouseId: WareHouseId);
         
        }
        public async void InitData(int PageSize = 10, string query = "", string CommodityCategoryName = "", string CommodityCompanyName = "",Guid WareHouseId = new Guid())
        {
            try
            {
                (var list, PageCount) = await db.CommoditySelectAsync(query: query, WareHouseId: WareHouseId);
                var tlist = list.Where(x => !Guids.Contains(x.Id)).ToList();
                CommodityList = new(list.Where(x => !Guids.Contains(x.Id)).ToList());

            }
            catch (Exception ex)
            {
                Growl.Error(ex.Message);
            }
        }
        public DelegateCommand<object> enterCommand
        {
            get => new DelegateCommand<object>((arg) =>
            {
                System.Collections.IList list = arg as System.Collections.IList;

                foreach (CommodityDto item in list)
                {
                    //if (item.StockCount == 0)
                    //{
                    //    continue;

                    //}
                    //else
                    //{
                       
                    //}

                    switch (Title)
                    {
                        case "采购订单":
                            PurchaseOrderItemList.Add(new PurchaseOrderItemDto()
                            {
                                CommodityId = item.Id,
                                PurchaseOrderId = OrderId,
                                Count = 0,
                                Price = 0,
                                Amount = 0,
                                TaxPrice = 0,
                                TaxAmount = 0,
                                Remark = "",
                                Tax = 0,
                                Rebate = 0,
                                CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                                LastUpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                                CommodityName = item.CommodityName,
                                StockCount = item.StockCount

                            });
                            break;
                        case "采购入库":
                            PurchaseWareHouseItemList.Add(new PurchaseWareHouseItemDto()
                            {
                                CommodityId = item.Id,
                                PurchaseWareHouseId = OrderId,
                                Count = 0,
                                Price = 0,
                                Amount = 0,
                                Remark = "",
                                Rebate = 0,
                                CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                                LastUpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                                CommodityName = item.CommodityName,
                                StockCount = item.StockCount
                            });
                            break;
                        case "采购退货":
                            PurchaseRefundItemList.Add(new()
                            {
                                CommodityId = item.Id,
                                PurchaseRefundId = OrderId,
                                Count = 0,
                                Price = 0,
                                Amount = 0,
                                Remark = "",
                                CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                                LastUpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                                CommodityName = item.CommodityName,
                                StockCount = item.StockCount
                            });
                            break;
                        case "销售订单":
                            SalesOrderItemList.Add(new()
                            {

                                CommodityId = item.Id,

                                Count = 0,
                                Price = 0,
                                Amount = 0,
                                TaxPrice = 0,
                                TaxAmount = 0,
                                Remark = "",
                                Tax = 0,
                                Rebate = 0,
                                CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                                LastUpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                                CommodityName = item.CommodityName,
                                Id = Guid.NewGuid(),
                                SalesOrderId = OrderId,
                                StockCount = item.StockCount
                            });
                            break;
                        case "销售出库":
                            SalesExWareHouseItemList.Add(new()
                            {
                                CommodityId = item.Id,
                                SalesExWareHouseId = OrderId,
                                Count = 0,
                                Price = 0,
                                Amount = 0,
                                Remark = "",
                                Rebate = 0,
                                CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                                LastUpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                                CommodityName = item.CommodityName,
                                StockCount = item.StockCount
                            });
                            break;
                        case "销售退货":
                            SalesRefundItemList.Add(new()
                            {
                                CommodityId = item.Id,
                                SalesRefundId = OrderId,
                                Count = 0,
                                Price = 0,
                                Amount = 0,
                                Remark = "",

                                CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                                LastUpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                                CommodityName = item.CommodityName,
                                StockCount = item.StockCount
                            });
                            break;

                        default:
                            break;
                    }

                }

                // 创建一个列表来收集需要删除的项
                var itemsToRemove = new List<CommodityDto>();

                foreach (CommodityDto item in list)
                {
                    //if (item.StockCount == 0)
                    //    continue;
                    itemsToRemove.Add(item);  // 收集要删除的项
                }

                foreach (var item in itemsToRemove)
                {
                    CommodityList.Remove(item);

                }

            });
        }
        public DelegateCommand<object> closeCommand
        {
            get
            {
                return new DelegateCommand<object>((arg) =>
                {
                    if (arg is System.Windows.Window view)
                    {
                        view.Close();
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

                        InitData(query:arg[0]?.ToString() ?? "");

                    }
                    catch (Exception ex)
                    {
                        Growl.Error(ex.Message);
                    }

                });
            }
        }
        public string Title { get; set; }
        public Guid OrderId { get; set; }
        public List<Guid> Guids { get; set; }
        public ObservableCollection<PurchaseOrderItemDto> PurchaseOrderItemList;
        public ObservableCollection<PurchaseWareHouseItemDto> PurchaseWareHouseItemList;
        public ObservableCollection<PurchaseRefundItemDto> PurchaseRefundItemList;

        public ObservableCollection<SalesOrderItemDto> SalesOrderItemList;
        public ObservableCollection<SalesExWareHouseItemDto> SalesExWareHouseItemList;
        public ObservableCollection<SalesRefundItemDto> SalesRefundItemList;





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
        private ObservableCollection<CommodityDto> commodityList;
        public ObservableCollection<CommodityDto> CommodityList
        {
            get => commodityList;
            set => SetProperty(ref commodityList, value);
        }

    }
}
