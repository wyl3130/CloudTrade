using CloudTrade.Application.Contracts.PurchaseRefunds;
using CloudTrade.Application.Contracts.PurchaseWareHouses;
using CloudTrade.Domain.PurchaseOrders;
using CloudTrade.Domain.PurchaseWareHouse;
using CloudTrade.Host.Resources.Helper;
using HandyControl.Controls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Unity.Storage.RegistrationSet;

namespace CloudTrade.Host.ViewModels.PurchaseRefunds
{
    public class PurchaseWareHouseListDialogViewModel:BindableBase
    {
        private readonly IPurchaseWareHouseService db;

        public PurchaseWareHouseListDialogViewModel(IPurchaseWareHouseService db)
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
                            if (item[0] is PurchaseWareHouse entity)
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

                (var list, PageCount) = await db.PurchaseWareHouseQueryAsync(PageIndex, PageSize, query, StartTime, EndTime);
                PurchaseWareHouseList = [.. list];
            }
            catch (Exception ex)
            {
                Growl.Error(ex.Message);
            }
        }

        private ObservableCollection<PurchaseWareHouseDto> purchaseWareHouseList;
        public ObservableCollection<PurchaseWareHouseDto> PurchaseWareHouseList
        {
            get => purchaseWareHouseList;
            set => SetProperty(ref purchaseWareHouseList, value);
        }
        private PurchaseWareHouse entity;
        public PurchaseWareHouse Entity
        {
            get { return entity; }
            set { SetProperty(ref entity, value); }
        }        // 页数 默认1页
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
