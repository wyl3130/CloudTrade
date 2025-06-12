using CloudTrade.Application.Contracts.PurchaseOrders;
using CloudTrade.Application.Contracts.SalesOrders;
using CloudTrade.Domain.CustomerCompanys;
using CloudTrade.Domain.Employees;
using CloudTrade.Domain.ModeInfos;
using CloudTrade.Domain.PurchaseOrders;
using CloudTrade.Domain.SalesModes;
using CloudTrade.Domain.SalesOrders;
using CloudTrade.Domain.WareHouses;
using CloudTrade.Host.ViewModels.Mains;
using CloudTrade.Host.Views.Mains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Host.ViewModels.SalesOrders
{
    public class SalesOrderDialogViewModel : BindableBase
    {
        private readonly ISalesOrderService db;
        public SalesOrderDialogViewModel(ISalesOrderService db)
        {
            this.db = db;
            SalesOrderItemList = new();

            InitData();

        }
        public async void InitData()
        {
            Entity.Preparer = AppData.user.UserName;
            CustomerCompanyList = new(await db.QueryableAsync<CustomerCompany>());
            EmployeList = new(await db.QueryableAsync<Employe>());
            WareHouseList = new(await db.QueryableAsync<WareHouse>());
            SalesModeList = new(await db.QueryableAsync<SalesMode>());
            ModeInfoList = new ObservableCollection<ModeInfo>(await db.QueryableAsync<ModeInfo>());
        }
        public DelegateCommand<object> enterCommand
        {
            get => new DelegateCommand<object>(async (arg) =>
            {
                if (arg is Window view)
                {
                    if (Title.Equals("添加"))
                    {

                        if (Entity != null && !string.IsNullOrEmpty(Entity.RealName) && !string.IsNullOrEmpty(Entity.RealName) && !string.IsNullOrEmpty(Entity.Preparer) && !string.IsNullOrEmpty(Entity.ExWareHouseDate))
                        {

                            Entity.CodeNo = "XD" + DateTime.Now.ToString("yyyyMMddHHmmss");
                            Entity.CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            Entity.LastUpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            var result = await db.SalesOrderInsertAsync(Entity,SalesOrderItemList);
                            if (result)
                            {

                                view.DialogResult = true;
                                view.Close();
                            }
                        }
                        else
                        {
                            HandyControl.Controls.MessageBox.Show("必填项为空", "添加", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    else
                    {
                        if (Entity != null && !string.IsNullOrEmpty(Entity.RealName) && !string.IsNullOrEmpty(Entity.RealName) && !string.IsNullOrEmpty(Entity.Preparer) && !string.IsNullOrEmpty(Entity.ExWareHouseDate))
                        {
                            Entity.LastUpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            var result = await db.SalesOrderUpdateAsync(Entity,SalesOrderItemList);
                            if (result)
                            {

                                view.DialogResult = true;
                                view.Close();
                            }
                        }
                        else
                        {
                            HandyControl.Controls.MessageBox.Show("必填项为空", "添加", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }




                }
            });
        }
        public DelegateCommand<object> AddCommand
        {
            get
            {
                return new DelegateCommand<object>((arg) =>
                {
                    if (arg is Window v)
                    {
                        if (Entity.WareHouseId == new Guid())
                        {
                            HandyControl.Controls.MessageBox.Show("选择仓库");
                        }
                        else
                        {
                            var view = new CommodityListDialogView();

                            var vm = new CommodityListDialogViewModel(db, Entity.WareHouseId);
                            
                            view.Owner = System.Windows.Application.Current.MainWindow;
                            view.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                            view.DataContext = vm;
                            vm.Title = "销售订单";
                            vm.OrderId = Entity.Id;

                            vm.SalesOrderItemList = SalesOrderItemList;
                            vm.Guids = SalesOrderItemList.Select(x => x.CommodityId).ToList();
                            view.ShowDialog();
                        }

                    }


                });
            }
        }
        public DelegateCommand<System.Collections.IList> DeleteCommand
        {
            get
            {
                return new DelegateCommand<System.Collections.IList>((arg) =>
                {
                    if (arg.Count == 1)
                    {
                        if (arg[0] is SalesOrderItemDto entity)
                        {
                            SalesOrderItemList.Remove(entity);
                        }
                    }
                });
            }
        }
        public DelegateCommand<object> ColumnUpdateCommand
        {
            get
            {
                return new DelegateCommand<object>((arg) =>
                {
                    var list = SalesOrderItemList;
                    foreach (var item in list)
                    {
                        item.TaxPrice = item.Price * (1 + item.Tax);  // 含税单价 = 单价 * (1 + 税率)
                        item.Amount = item.Price * item.Count;         // 金额 = 单价 * 数量
                        item.TaxAmount = item.Amount * item.Tax;       // 税额 = 金额 * 税率
                    }
                    SalesOrderItemList = new(list);
                });
            }
        }
        public DelegateCommand<object> closeCommand
        {
            get
            {
                return new DelegateCommand<object>((arg) =>
                {
                    if (arg is Window view)
                    {
                        view.Close();
                    }

                });
            }
        }
        private string title = "添加";
        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }
        private SalesOrder entity = new() { Id = Guid.NewGuid() };
        public SalesOrder Entity
        {
            get => entity;
            set => SetProperty(ref entity, value);
        }

        private ObservableCollection<SalesOrderItemDto> salesOrderItemList;
        public ObservableCollection<SalesOrderItemDto> SalesOrderItemList
        {
            get { return salesOrderItemList; }
            set { SetProperty(ref salesOrderItemList, value); }
        }
        private ObservableCollection<CustomerCompany> customerCompanyList;
        public ObservableCollection<CustomerCompany> CustomerCompanyList
        {
            get { return customerCompanyList; }
            set { SetProperty(ref customerCompanyList, value); }
        }
        private ObservableCollection<Employe> employeList;
        public ObservableCollection<Employe> EmployeList
        {
            get { return employeList; }
            set { SetProperty(ref employeList, value); }
        }
        private ObservableCollection<WareHouse> warehouseList;
        public ObservableCollection<WareHouse> WareHouseList
        {
            get { return warehouseList; }
            set { SetProperty(ref warehouseList, value); }
        }
        private ObservableCollection<SalesMode> salesModeList;
        public ObservableCollection<SalesMode> SalesModeList
        {
            get { return salesModeList; }
            set { SetProperty(ref salesModeList, value); }
        }
        private ObservableCollection<ModeInfo> modeInfoList;
        public ObservableCollection<ModeInfo> ModeInfoList
        {
            get { return modeInfoList; }
            set { SetProperty(ref modeInfoList, value); }
        }
    }
}

