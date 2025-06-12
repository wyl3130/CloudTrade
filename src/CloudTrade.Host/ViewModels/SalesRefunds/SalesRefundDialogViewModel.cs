using CloudTrade.Application.Contracts.ModeInfos;
using CloudTrade.Application.Contracts.PurchaseOrders;
using CloudTrade.Application.Contracts.PurchaseWareHouses;
using CloudTrade.Application.Contracts.SalesRefunds;
using CloudTrade.Domain.CustomerCompanys;
using CloudTrade.Domain.Employees;
using CloudTrade.Domain.ModeInfos;
using CloudTrade.Domain.PaymentAccounts;
using CloudTrade.Domain.PaymentCategorys;
using CloudTrade.Domain.PurchaseOrders;
using CloudTrade.Domain.PurchaseWareHouse;
using CloudTrade.Domain.SalesExWareHouseItem;
using CloudTrade.Domain.SalesExWareHouses;
using CloudTrade.Domain.SalesRefundItems;
using CloudTrade.Domain.SalesRefunds;
using CloudTrade.Domain.WareHouses;
using CloudTrade.Host.ViewModels.Mains;
using CloudTrade.Host.ViewModels.PurchaseWareHouses;
using CloudTrade.Host.Views.Mains;
using CloudTrade.Host.Views.PurchaseWareHouses;
using CloudTrade.Host.Views.SalesRefunds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Host.ViewModels.SalesRefunds
{
    public class SalesRefundDialogViewModel : BindableBase
    {
        private readonly ISalesRefundService db;
        public SalesRefundDialogViewModel(ISalesRefundService db)
        {
            this.db = db;
            SalesRefundItemList = new();
            //System.Windows.Application.Current.ThemeMode = HandyControl.Data.Theme.Dark;
            InitData();

        }
        public async void InitData()
        {
            Entity.Preparer = AppData.user.UserName;
            CustomerCompanyList = new(await db.QueryableAsync<CustomerCompany>());
            EmployeList = new(await db.QueryableAsync<Employe>());
            WareHouseList = new(await db.QueryableAsync<WareHouse>());
            PaymentAccountList = new(await db.QueryableAsync<PaymentAccount>());
            PaymentCategoryList = new(await db.QueryableAsync<PaymentCategory>());
            SalesExWareHouseList = new(await db.QueryableAsync<SalesExWareHouse>());
        }
        public DelegateCommand<object> enterCommand
        {
            get => new DelegateCommand<object>(async (arg) =>
            {
                if (arg is Window view)
                {
                    if (Title.Equals("添加"))
                    {

                        if (Entity != null && !string.IsNullOrEmpty(Entity.RealName) && !string.IsNullOrEmpty(Entity.RealName) && !string.IsNullOrEmpty(Entity.Preparer) && !string.IsNullOrEmpty(Entity.WareHouseDate))
                        {


                            Entity.CodeNo = "XT" + DateTime.Now.ToString("yyyyMMddHHmmss");

                            Entity.CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            Entity.LastUpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            var result = await db.SalesRefundInsertAsync(Entity, SalesRefundItemList);
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
                        if (Entity != null && !string.IsNullOrEmpty(Entity.RealName) && !string.IsNullOrEmpty(Entity.RealName) && !string.IsNullOrEmpty(Entity.Preparer) && !string.IsNullOrEmpty(Entity.WareHouseDate))
                        {

                            Entity.LastUpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            var result = await db.SalesRefundUpdateAsync(Entity, SalesRefundItemList);
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
                            vm.Title = "销售退货";
                            vm.OrderId = Entity.Id;
                            vm.SalesRefundItemList = SalesRefundItemList;
                            vm.Guids = SalesRefundItemList.Select(x => x.CommodityId).ToList();
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
                        if (arg[0] is SalesRefundItemDto entity)
                        {
                            SalesRefundItemList.Remove(entity);
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
                    var list = SalesRefundItemList;
                    foreach (var item in list)
                    {
                        //item.TaxPrice = item.Price * (1 + item.Tax);  // 含税单价 = 单价 * (1 + 税率)
                        item.Amount = item.Price * item.Count;         // 金额 = 单价 * 数量
                                                                       // item.TaxAmount = item.Amount * item.Tax;       // 税额 = 金额 * 税率
                    }
                    SalesRefundItemList = new(list);
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
                    var view = new SalesExWareHouseListDialogView();
                    view.Owner = System.Windows.Application.Current.MainWindow;
                    view.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                    var vm = new SalesExWareHouseListDialogViewModel(db);
                    view.DataContext = vm;
                    bool? result = view.ShowDialog();
                    if (result == true)
                    {
                        sEntity = vm.Entity;
                        if (sEntity != null)
                        {
                            Entity = new()
                            {
                                Id = Guid.NewGuid(),
                                CustomerCompanyId = sEntity.CustomerCompanyId,
                                WareHouseId = sEntity.WareHouseId,
                                RealName = sEntity.RealName,
                                Preparer = AppData.user.UserName

                            };
                            var list = (await db.SalesExWareHouseViewAsync(sEntity.Id));
                            SalesRefundItemList = new();
                            foreach (var item in list)
                            {
                                SalesRefundItemList.Add(new SalesRefundItemDto()
                                {
                                    Id = new Guid(),
                                    Amount = item.Amount,
                                    CommodityId = item.CommodityId,
                                    CommodityName = item.CommodityName,
                                    Count = item.Count,
                                    CreateTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
                                    LastUpdateTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
                                    SalesRefundId = Entity.Id,

                                    Price = item.Price,

                                    Remark = item.Remark,
                                });
                            }
                        }
                    }



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
        private SalesRefund entity = new() { Id = Guid.NewGuid() };
        public SalesRefund Entity
        {
            get => entity;
            set => SetProperty(ref entity, value);
        }
        private SalesExWareHouse sentity;
        public SalesExWareHouse sEntity
        {
            get => sentity;
            set => SetProperty(ref sentity, value);
        }

        private ObservableCollection<SalesRefundItemDto> salesRefundItemList;
        public ObservableCollection<SalesRefundItemDto> SalesRefundItemList
        {
            get { return salesRefundItemList; }
            set { SetProperty(ref salesRefundItemList, value); }
        }
        private ObservableCollection<SalesExWareHouse> salesExWareHouseList;
        public ObservableCollection<SalesExWareHouse> SalesExWareHouseList
        {
            get { return salesExWareHouseList; }
            set { SetProperty(ref salesExWareHouseList, value); }
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

    }
}
