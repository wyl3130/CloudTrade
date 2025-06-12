using CloudTrade.Application.Contracts.PurchaseOrders;
using CloudTrade.Application.Contracts.PurchaseWareHouses;
using CloudTrade.Application.PurchaseOrders;
using CloudTrade.Domain.CustomerCompanys;
using CloudTrade.Domain.Employees;
using CloudTrade.Domain.ModeInfos;
using CloudTrade.Domain.PaymentAccounts;
using CloudTrade.Domain.PaymentCategorys;
using CloudTrade.Domain.PurchaseOrderItems;
using CloudTrade.Domain.PurchaseOrders;
using CloudTrade.Domain.PurchaseWareHouse;
using CloudTrade.Domain.PurchaseWareHouseItems;
using CloudTrade.Domain.WareHouses;
using CloudTrade.Host.Resources.Event;
using CloudTrade.Host.ViewModels.Mains;
using CloudTrade.Host.Views.Mains;
using CloudTrade.Host.Views.PurchaseWareHouses;
using NetTaste;
using Prism.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Host.ViewModels.PurchaseWareHouses
{
    public class PurchaseWareHouseDialogViewModel : BindableBase
    {
        private readonly IPurchaseWareHouseService db;

        public PurchaseWareHouseDialogViewModel(IPurchaseWareHouseService db )
        {
            this.db = db;
     
            PurchaseWareHouseItemList = new();
            InitData();

        }
        public async void InitData()
        {
            Entity.Preparer = AppData.user.UserName;
            CustomerCompanyList = new(await db.QueryableAsync<CustomerCompany>());
            EmployeList = new(await db.QueryableAsync<Employe>());
            WareHouseList = new(await db.QueryableAsync<WareHouse>());
            ModeInfoList = new(await db.QueryableAsync<ModeInfo>());
          

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

                        if (Entity != null && !string.IsNullOrEmpty(Entity.RealName) && !string.IsNullOrEmpty(Entity.RealName) && !string.IsNullOrEmpty(Entity.Preparer) && !string.IsNullOrEmpty(Entity.WareHouseDate))
                        {
                            Entity.CodeNo = "CR" + DateTime.Now.ToString("yyyyMMddHHmmss");
                            Entity.CreateTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                            Entity.LastUpdateTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                            if (oEntity == null)
                                Entity.PurchaseOrderId = Guid.Empty;
                            else
                                Entity.PurchaseOrderId = oEntity.Id;

                            var result = await db.PurchaseWareHouseInsertAsync(Entity, PurchaseWareHouseItemList);
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
                            Entity.LastUpdateTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                            Entity.PurchaseOrderId = oEntity.Id;
                            var result = await db.PurchaseWareHouseUpdateAsync(Entity, PurchaseWareHouseItemList);
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
                            vm.Title = "采购入库";
                            vm.OrderId = Entity.Id;

                            vm.PurchaseWareHouseItemList = PurchaseWareHouseItemList;
                            vm.Guids = PurchaseWareHouseItemList.Select(x => x.CommodityId).ToList();
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
                        if (arg[0] is PurchaseWareHouseItemDto entity)
                        {
                            PurchaseWareHouseItemList.Remove(entity);
                        }
                    }
                });
            }
        }

        public DelegateCommand ColumnUpdateCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    var list = PurchaseWareHouseItemList;
                    foreach (var item in list)
                    {
                        // item.TaxPrice = item.Price * (1 + item.Tax);  // 含税单价 = 单价 * (1 + 税率)
                        item.Amount = item.Price * item.Count;         // 金额 = 单价 * 数量
                        //item.TaxAmount = item.Amount * item.Tax;       // 税额 = 金额 * 税率
                    }
                    PurchaseWareHouseItemList = new(list);
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
                    var view = new PurchaseOrderListDialogView();
                    view.Owner = System.Windows.Application.Current.MainWindow;
                    view.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                    var vm = new PurchaseOrderListDialogViewModel(db);
                    view.DataContext = vm;
                    bool? result = view.ShowDialog();
                    if (result == true)
                    {
                        oEntity = vm.Entity;
                        if (oEntity != null)
                        {
                            Entity = new PurchaseWareHouse()
                            {
                                Id= Guid.NewGuid(),
                                CustomerCompanyId = oEntity.CustomerCompanyId,
                                WareHouseId = oEntity.WareHouseId,
                                RealName = oEntity.RealName,
                                Preparer = AppData.user.UserName

                            };
                            var list =(await db.PurchaseOrderViewAsync(oEntity.Id));
                            PurchaseWareHouseItemList = new ObservableCollection<PurchaseWareHouseItemDto>();
                            foreach (var item in list)
                            {
                                PurchaseWareHouseItemList.Add(new PurchaseWareHouseItemDto()
                                {
                                    Id=new Guid(),
                                    Amount = item.Amount,
                                    CommodityId=item.CommodityId,
                                    CommodityName= item.CommodityName,
                                    Count = item.Count,
                                    CreateTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
                                    LastUpdateTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
                                    ModeInfoId = item.ModeInfoId,
                                    ModeInfoName = item.ModeInfoName,
                                    Price   = item.Price,
                                    PurchaseWareHouseId=Entity.Id,
                                    Rebate= item.Rebate,
                                    Remark = item.Remark,
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
        private PurchaseWareHouse entity = new() { Id = Guid.NewGuid() };
        public PurchaseWareHouse Entity
        {
            get => entity;
            set => SetProperty(ref entity, value);
        }
        private PurchaseOrder oentity;
        public PurchaseOrder oEntity
        {
            get => oentity;
            set => SetProperty(ref oentity, value);
        }
        
        private ObservableCollection<PurchaseWareHouseItemDto> purchaseWareHouseItemList;
        public ObservableCollection<PurchaseWareHouseItemDto> PurchaseWareHouseItemList
        {
            get { return purchaseWareHouseItemList; }
            set { SetProperty(ref purchaseWareHouseItemList, value); }
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

        private ObservableCollection<ModeInfo> modeInfoList;
        public ObservableCollection<ModeInfo> ModeInfoList
        {
            get { return modeInfoList; }
            set { SetProperty(ref modeInfoList, value); }
        }
    }
}
