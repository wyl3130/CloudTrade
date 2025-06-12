using CloudTrade.Application.Contracts.SalesExWareHouses;
using CloudTrade.Application.Contracts.SalesOrders;
using CloudTrade.Domain.SalesExWareHouses;
using CloudTrade.Domain.SalesOrders;
using CloudTrade.Host.Resources.Helper;
using HandyControl.Controls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Host.ViewModels.SalesRefunds
{
    public class SalesExWareHouseListDialogViewModel:BindableBase
    {
        private readonly ISalesExWareHouseService db;
        public SalesExWareHouseListDialogViewModel(ISalesExWareHouseService db)
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
                            if (item[0] is SalesExWareHouse entity)
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

                (var list, PageCount) = await db.SalesExWarehouseQueryAsync(PageIndex, PageSize, query, StartTime, EndTime);
                SalesExWareHouseList = new ObservableCollection<SalesExWareHouseDto>(list);
            }
            catch (Exception ex)
            {
                Growl.Error(ex.Message);
            }
        }
        private SalesExWareHouse entity;
        public SalesExWareHouse Entity
        {
            get { return entity; }
            set { SetProperty(ref entity, value); }
        }

        private ObservableCollection<SalesExWareHouseDto> salesExWareHouseList;
        public ObservableCollection<SalesExWareHouseDto> SalesExWareHouseList
        {
            get { return salesExWareHouseList; }
            set { SetProperty(ref salesExWareHouseList, value); }
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
