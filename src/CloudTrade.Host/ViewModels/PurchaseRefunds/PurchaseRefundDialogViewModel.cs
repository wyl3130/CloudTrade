using CloudTrade.Application.Contracts.ModeInfos;
using CloudTrade.Application.Contracts.PurchaseOrders;
using CloudTrade.Application.Contracts.PurchaseRefunds;
using CloudTrade.Application.Contracts.PurchaseWareHouses;
using CloudTrade.Domain.CustomerCompanys;
using CloudTrade.Domain.Employees;
using CloudTrade.Domain.ModeInfos;
using CloudTrade.Domain.PaymentAccounts;
using CloudTrade.Domain.PaymentCategorys;
using CloudTrade.Domain.PurchaseOrders;
using CloudTrade.Domain.PurchaseRefunds;
using CloudTrade.Domain.PurchaseWareHouse;
using CloudTrade.Domain.WareHouses;
using CloudTrade.Host.ViewModels.Mains;
using CloudTrade.Host.ViewModels.PurchaseWareHouses;
using CloudTrade.Host.Views.Mains;
using CloudTrade.Host.Views.PurchaseRefunds;
using CloudTrade.Host.Views.PurchaseWareHouses;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Host.ViewModels.PurchaseRefunds
{
    public class PurchaseRefundDialogViewModel:BindableBase
    {
        private readonly IPurchaseRefundService db;
        public PurchaseRefundDialogViewModel(IPurchaseRefundService db)
        {
            this.db = db;
            PurchaseRefundItemList = new();

            InitData();

        }
        public async void InitData()
        {
            Entity.Preparer = AppData.user.UserName;
            CustomerCompanyList = new(await db.QueryableAsync<CustomerCompany>());
            EmployeList = new(await db.QueryableAsync<Employe>());
            WareHouseList = new(await db.QueryableAsync<WareHouse>()); 
        
            PurchaseWareHouseList = new(await db.QueryableAsync<PurchaseWareHouse>());
            PaymentAccountList = new(await db.QueryableAsync<PaymentAccount>());
            PaymentCategoryList = new(await db.QueryableAsync<PaymentCategory>());
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

                            Entity.CodeNo = "CT" + DateTime.Now.ToString("yyyyMMddHHmmss");
    
                            Entity.CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            Entity.LastUpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            var result = await db.PurchaseRefundInsertAsync(Entity,PurchaseRefundItemList);
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

                            var result = await db.PurchaseRefundUpdateAsync(Entity, PurchaseRefundItemList);
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
                            vm.Title = "采购退货";
                            vm.OrderId = Entity.Id;
                            vm.PurchaseRefundItemList = PurchaseRefundItemList;
                            vm.Guids = PurchaseRefundItemList.Select(x => x.CommodityId).ToList();
                            view.ShowDialog();
                        }


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
                    var view = new PurchaseWareHouseListDialogView();
                    view.Owner = System.Windows.Application.Current.MainWindow;
                    view.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                    var vm = new PurchaseWareHouseListDialogViewModel(db);
                    view.DataContext = vm;
                    bool? result = view.ShowDialog();
                    if (result == true)
                    {
                        wEntity = vm.Entity;
                        if (wEntity != null)
                        {
                            Entity = new PurchaseRefund()
                            {
                                Id = Guid.NewGuid(),
                                CustomerCompanyId = wEntity.CustomerCompanyId,
                                WareHouseId = wEntity.WareHouseId,
                                RealName = wEntity.RealName,
                                Preparer = AppData.user.UserName

                            };
                            var list = (await db.PurchaseWareHouseViewAsync(wEntity.Id));
                            PurchaseRefundItemList = new ObservableCollection<PurchaseRefundItemDto>();
                            foreach (var item in list)
                            {
                                PurchaseRefundItemList.Add(new PurchaseRefundItemDto()
                                {
                                    Id = new Guid(),
                                    Amount = item.Amount,
                                    CommodityId = item.CommodityId,
                                    CommodityName = item.CommodityName,
                                    Count = item.Count,
                                    CreateTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
                                    LastUpdateTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
                    
                                    Price = item.Price,
                                   PurchaseRefundId=Entity.Id,
                             
                                    Remark = item.Remark,
                                });
                            }
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
                        if (arg[0] is PurchaseRefundItemDto entity)
                        {
                            PurchaseRefundItemList.Remove(entity);
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
                    var list = PurchaseRefundItemList;
                    foreach (var item in list)
                    {
                       // item.TaxPrice = item.Price * (1 + item.Tax);  // 含税单价 = 单价 * (1 + 税率)
                        item.Amount = item.Price * item.Count;         // 金额 = 单价 * 数量
                        //item.TaxAmount = item.Amount * item.Tax;       // 税额 = 金额 * 税率
                    }
                    PurchaseRefundItemList = new(list);
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
        private PurchaseRefund entity = new() { Id=Guid.NewGuid()};
        public PurchaseRefund Entity
        {
            get => entity;
            set => SetProperty(ref entity, value);
        }

        private PurchaseWareHouse wentity;
        public PurchaseWareHouse wEntity
        {
            get => wentity;
            set => SetProperty(ref wentity, value);
        }

        private ObservableCollection<PurchaseRefundItemDto> purchaseRefundItemList;
        public ObservableCollection<PurchaseRefundItemDto> PurchaseRefundItemList
        {
            get { return purchaseRefundItemList; }
            set { SetProperty(ref purchaseRefundItemList, value); }
        }
        private ObservableCollection<PurchaseWareHouse> purchaseWareHouseList;
        public ObservableCollection<PurchaseWareHouse> PurchaseWareHouseList
        {
            get { return purchaseWareHouseList; }
            set { SetProperty(ref purchaseWareHouseList, value); }
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
        private ObservableCollection<WareHouse> warehouseList;
        public ObservableCollection<WareHouse> WareHouseList
        {
            get { return warehouseList; }
            set { SetProperty(ref warehouseList, value); }
        }

    }
}
