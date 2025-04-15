using CloudTrade.Application.Contracts.PurchaseOrders;
using CloudTrade.Application.Contracts.PurchaseRefunds;
using CloudTrade.Application.Contracts.PurchaseWareHouses;
using CloudTrade.Application.Contracts.SalesExWareHouses;
using CloudTrade.Application.Contracts.SalesRefunds;
using CloudTrade.Domain.Commoditys;
using HandyControl.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Host.ViewModels.Mains
{
    public class CommodityListDialogViewModel:BindableBase
    {
        private readonly ICommodityService db;
        public CommodityListDialogViewModel(ICommodityService db)
        {
            this.db = db;
            InitData();
        }
        //public CommodityListDialogViewModel()
        //{
            
        //}
        public async void InitData()
        {
            try
            {
                //commodityCompanyList = new ObservableCollection<CommodityCompany>(db.Queryable<CommodityCompany>().ToList());
                //commodityCategoryList = new ObservableCollection<CommodityCategory>(db.Queryable<CommodityCategory>().ToList());
                (var list,PageCount) = await db.CommodityQueryAsync(query:"");
                var tlist = list.Where(x=>!Guids.Contains(x.Id)).ToList();
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
                    switch (Title)
                    {
                        case "采购订单":
                            PurchaseOrderItemList.Add(new PurchaseOrderItemDto()
                            {
                                CommodityId = item.Id,
                                PurchaseOrderId = OrderId,
                                Count = 0,
                                Price = 0,
                                Amount=0,
                                TaxPrice=0,
                                TaxAmount = 0,
                                Remark="",
                                Tax= 0,
                                Rebate = 0,
                                CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                                LastUpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                                CommodityName = item.CommodityName,
                   
                            });
                            break;
                        case "采购入库单":
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
                                
                            });
                            break;
                        case "采购退库单":
                            PurchaseRefundItemList.Add(new() {
                                CommodityId = item.Id,
                                PurchaseRefundId = OrderId,
                                Count = 0,
                                Price = 0,
                                Amount = 0,
                                Remark = "",
                                CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                                LastUpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                                CommodityName = item.CommodityName,
                            });
                            break;
                        case "销售订单":
                            break;
                        case "销售入库":
                            break;
                        case "销售退货":
                            break;

                        default:
                            break;
                    }
                }
                // 创建一个列表来收集需要删除的项
                var itemsToRemove = new List<CommodityDto>();

                foreach (CommodityDto item in list)
                {
                    itemsToRemove.Add(item);  // 收集要删除的项
                }

                foreach (var item in itemsToRemove)
                {
                    CommodityList.Remove(item);

                }
                //System.Windows.MessageBox.Show(OrderId.ToString());
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
        public string Title { get; set; }
        public Guid OrderId { get; set; }
        public List<Guid> Guids { get; set; }
        public ObservableCollection<PurchaseOrderItemDto> PurchaseOrderItemList;
        public ObservableCollection<PurchaseWareHouseItemDto> PurchaseWareHouseItemList;

        public ObservableCollection<PurchaseRefundItemDto> PurchaseRefundItemList;
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
