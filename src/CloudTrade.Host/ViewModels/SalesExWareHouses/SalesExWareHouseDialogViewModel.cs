using CloudTrade.Application.Contracts.ModeInfos;
using CloudTrade.Application.Contracts.PurchaseOrders;
using CloudTrade.Application.Contracts.PurchaseWareHouses;
using CloudTrade.Application.Contracts.SalesExWareHouses;
using CloudTrade.Domain.CustomerCompanys;
using CloudTrade.Domain.Employees;
using CloudTrade.Domain.ModeInfos;
using CloudTrade.Domain.PaymentAccounts;
using CloudTrade.Domain.PaymentCategorys;
using CloudTrade.Domain.PurchaseOrders;
using CloudTrade.Domain.PurchaseWareHouse;
using CloudTrade.Domain.SalesExWareHouseItem;
using CloudTrade.Domain.SalesExWareHouses;
using CloudTrade.Domain.SalesModes;
using CloudTrade.Domain.SalesOrders;
using CloudTrade.Domain.WareHouses;
using CloudTrade.Host.ViewModels.Mains;
using CloudTrade.Host.ViewModels.PurchaseWareHouses;
using CloudTrade.Host.Views.Mains;
using CloudTrade.Host.Views.PurchaseWareHouses;
using CloudTrade.Host.Views.SalesExWareHouses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Host.ViewModels.SalesExWareHouses
{
    public class SalesExWareHouseDialogViewModel : BindableBase
    {
        private readonly ISalesExWareHouseService db;
        public SalesExWareHouseDialogViewModel(ISalesExWareHouseService db)
        {
            this.db = db;
            SalesExWareHouseItemList = new();

            InitData();

        }
        public async void InitData()
        {
            Entity.Preparer = AppData.user.UserName;
            CustomerCompanyList = new(await db.QueryableAsync<CustomerCompany>());
            EmployeList = new(await db.QueryableAsync<Employe>());
            WareHouseList = new(await db.QueryableAsync<WareHouse>());
            SalesModeList = new(await db.QueryableAsync<SalesMode>()); 
            PaymentAccountList = new(await db.QueryableAsync<PaymentAccount>());
            PaymentCategoryList = new(await db.QueryableAsync<PaymentCategory>());
            ModeInfoList = new(await db.QueryableAsync<ModeInfo>());
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

                            Entity.CodeNo = "XC" + DateTime.Now.ToString("yyyyMMddHHmmss");
                    
                            Entity.CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            Entity.LastUpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            var result = await db.SalesExWareHouseInsertAsync(Entity, SalesExWareHouseItemList);
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
                            var result = await db.SalesExWareHouseUpdateAsync(Entity, SalesExWareHouseItemList);
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
                            vm.Title = "销售出库";
                            vm.OrderId = Entity.Id;
                            vm.SalesExWareHouseItemList = SalesExWareHouseItemList;
                            vm.Guids = SalesExWareHouseItemList.Select(x => x.CommodityId).ToList();
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
                        if (arg[0] is SalesExWareHouseItemDto entity)
                        {
                            SalesExWareHouseItemList.Remove(entity);
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
                    var list = SalesExWareHouseItemList;
                    foreach (var item in list)
                    {
                        // item.TaxPrice = item.Price * (1 + item.Tax);  // 含税单价 = 单价 * (1 + 税率)
                        item.Amount = item.Price * item.Count;         // 金额 = 单价 * 数量
                        //item.TaxAmount = item.Amount * item.Tax;       // 税额 = 金额 * 税率
                    }
                    SalesExWareHouseItemList = new(list);
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
        public DelegateCommand OrderbtnCommand
        {
            get
            {
                return new DelegateCommand(async () =>
                {

                    //eventAggregator.GetEvent<VisibilityChangeEvent>().Publish();
                    var view = new SalesOrderListDialogView();
                    view.Owner = System.Windows.Application.Current.MainWindow;
                    view.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                    var vm = new SalesOrderListDialogViewModel(db);
                    view.DataContext = vm;
                    bool? result = view.ShowDialog();
                    if (result == true)
                    {
                        sEntity = vm.Entity;
                        if (sEntity != null)
                        {
                            Entity = new SalesExWareHouse()
                            {
                                Id = Guid.NewGuid(),
                                CustomerCompanyId = sEntity.CustomerCompanyId,
                                WareHouseId = sEntity.WareHouseId,
                                RealName = sEntity.RealName,
                                Preparer = AppData.user.UserName

                            };
                            var list = (await db.SalesOrderViewAsync(sEntity.Id));
                            SalesExWareHouseItemList = new ();
                            foreach (var item in list)
                            {
                                SalesExWareHouseItemList.Add(new SalesExWareHouseItemDto()
                                {
                                    Id = new Guid(),
                                    Amount = item.Amount,
                                    CommodityId = item.CommodityId,
                                    CommodityName = item.CommodityName,
                                    Count = item.Count,
                                    CreateTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
                                    LastUpdateTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
                                    ModeInfoId = item.ModeInfoId,
                                    ModeInfoName = item.ModeInfoName,
                                    Price = item.Price,
                                    SalesExWareHouseId = Entity.Id,
                                    Rebate = item.Rebate,
                                    Remark = item.Remark,
                                    StockCount=item.StockCount
                                });
                            }
                        }
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
        private SalesExWareHouse entity = new() { Id = Guid.NewGuid() };
        public SalesExWareHouse Entity
        {
            get => entity;
            set => SetProperty(ref entity, value);
        }
        private SalesOrder sentity;
        public SalesOrder sEntity
        {
            get => sentity;
            set => SetProperty(ref sentity, value);
        }


        private ObservableCollection<SalesExWareHouseItemDto> salesExWareHouseItemList;
        public ObservableCollection<SalesExWareHouseItemDto> SalesExWareHouseItemList
        {
            get { return salesExWareHouseItemList; }
            set { SetProperty(ref salesExWareHouseItemList, value); }
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
        private ObservableCollection<PaymentCategory> paymentCategoryList;
        public ObservableCollection<PaymentCategory> PaymentCategoryList
        {
            get { return paymentCategoryList; }
            set { SetProperty(ref paymentCategoryList, value); }
        }
        private ObservableCollection<PaymentAccount> paymentAccountList;
        public ObservableCollection<PaymentAccount> PaymentAccountList
        {
            get { return paymentAccountList; }
            set { SetProperty(ref paymentAccountList, value); }
        }
        private ObservableCollection<ModeInfo> modeInfoList;
        public ObservableCollection<ModeInfo> ModeInfoList
        {
            get { return modeInfoList; }
            set { SetProperty(ref modeInfoList, value); }
        }


    }
}
