using CloudTrade.Application.Contracts.PurchaseOrders;
using CloudTrade.Domain.PurchaseOrders;
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

namespace CloudTrade.Host.ViewModels.PurchaseWareHouses
{
    public class PurchaseOrderListDialogViewModel : BindableBase
    {
        private readonly IPurchaseOrderService db;

        public PurchaseOrderListDialogViewModel(IPurchaseOrderService db)
        {
            this.db = db;
 
            InitData();
        }
        public DelegateCommand<ArrayList> SelectCommand
        {
            get
            {
                return new DelegateCommand<ArrayList>((arg) =>
                {
                    try
                    {
                        InitData(PageHelper.PageSize(Convert.ToInt32(arg[0])), arg[1]?.ToString() ?? "", arg[2]?.ToString() ?? "", arg[3]?.ToString() ?? "");
                    }
                    catch (Exception ex)
                    {
                        Growl.Error(ex.Message);
                    }
                });
            }
        }
        public DelegateCommand<ArrayList> EnterCommand => new DelegateCommand<ArrayList>((arg) =>
        {
            if (arg.Count == 2)
            {
                if (arg[0] is System.Windows.Window view)
                {

                    if (arg[1] is IList item)
                    {
                        if (item.Count == 1)
                        {
                            if (item[0] is PurchaseOrder entity)
                            {
                                if (entity.OrderState == 0)
                                {
                                    HandyControl.Controls.MessageBox.Show("不能选择未审核的订单");
                                }
                                else
                                {
                                    Entity = entity;
                                    view.DialogResult = true;
                                    view.Close();
                                }
                            }
                        }
                        else
                        {
                            HandyControl.Controls.MessageBox.Show("只能选择一条数据");
                        }
                    }
                }
            }
        });
        public async void InitData(int PageSize = 10, string query = "", string StartTime = "", string EndTime = "")
        {
            try
            {

                (var list, PageCount) = await db.PurchaseOrderQueryAsync(PageIndex, PageSize, query, StartTime, EndTime);
                PurchaseOrderList = new ObservableCollection<PurchaseOrderDto>(list);
            }
            catch (Exception ex)
            {
                Growl.Error(ex.Message);
            }
        }
        private PurchaseOrder entity;
        public PurchaseOrder Entity
        {
            get { return entity; }
            set { SetProperty(ref entity, value); }
        }

        private ObservableCollection<PurchaseOrderDto> purchaseOrderList;
        public ObservableCollection<PurchaseOrderDto> PurchaseOrderList
        {
            get { return purchaseOrderList; }
            set { SetProperty(ref purchaseOrderList, value); }
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

    }
}
