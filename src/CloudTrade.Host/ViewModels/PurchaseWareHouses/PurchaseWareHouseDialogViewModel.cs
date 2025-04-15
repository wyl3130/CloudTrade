using CloudTrade.Application.Contracts.PurchaseOrders;
using CloudTrade.Application.Contracts.PurchaseWareHouses;
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
using CloudTrade.Host.ViewModels.Mains;
using CloudTrade.Host.Views.Mains;
using NetTaste;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Host.ViewModels.PurchaseWareHouses
{
    public class PurchaseWareHouseDialogViewModel : BindableBase
    {
        private readonly IPurchaseWareHouseService db;
        public PurchaseWareHouseDialogViewModel(IPurchaseWareHouseService db)
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
            PurchaseOrderList = new(await db.QueryableAsync<PurchaseOrder>());
            PaymentAccountList = new(await db.QueryableAsync<PaymentAccount>());
            PaymentCategoryList = new ObservableCollection<PaymentCategory>(await db.QueryableAsync<PaymentCategory>());
        }
        public DelegateCommand<object> enterCommand
        {
            get => new DelegateCommand<object>(async (arg) =>
            {
                if (arg is Window view)
                {
                    if (Title.Equals("添加"))
                    {

                        if (Entity != null && !string.IsNullOrEmpty(Entity.RealName))
                        {
                            Entity.CodeNo = "CR" + DateTime.Now.ToString("yyyyMMddHHmmss");
                            Entity.WareHouseDate = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                            Entity.CreateTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                            Entity.LastUpdateTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                            if (Entity.OrderState == 0)
                            {
                                var result = await db.InsertAsync<PurchaseWareHouse>(Entity);
                                if (result)
                                {
                                    foreach (var item in PurchaseWareHouseItemList)
                                    {
                                        await db.InsertAsync<PurchaseWareHouseItem>(item);
                                    }
                                    view.DialogResult = true;
                                    view.Close();
                                }
                            }
                            else
                            {
                                var result = await db.PurchaseWareHouseInsertAsync(Entity, PurchaseWareHouseItemList);
                                if (result)
                                {
                                    view.DialogResult = true;
                                    view.Close();
                                }
                            }

                        }
                        else
                        {
                            HandyControl.Controls.MessageBox.Show("必填项为空", "添加", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    else
                    {
                        if (Entity != null && !string.IsNullOrEmpty(Entity.RealName))
                        {
                            Entity.LastUpdateTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                            if (Entity.OrderState == 0)
                            {
                                var result = await db.UpdateAsync<PurchaseWareHouse>(Entity);
                                if (result)
                                {
                                    var list = await db.QueryableAsync<PurchaseWareHouseItem>(x => x.PurchaseWareHouseId.Equals(Entity.Id));
                                    await db.DeleteAsync<PurchaseWareHouseItem>(list);

                                    foreach (var item in PurchaseWareHouseItemList)
                                    {
                                        await db.InsertAsync<PurchaseWareHouseItem>(item);
                                    }
                                    view.DialogResult = true;
                                    view.Close();
                                }
                            }
                            else
                            {
                                await db.PurchaseWareHouseDeleteAsync(Entity);
                                await db.PurchaseWareHouseInsertAsync(Entity, PurchaseWareHouseItemList);
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
                        var view = new CommodityListDialogView();

                        var vm = new CommodityListDialogViewModel(db);

                        view.Owner = System.Windows.Application.Current.MainWindow;
                        view.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                        view.DataContext = vm;
                        vm.Title = "采购入库单";
                        vm.OrderId = Entity.Id;

                        vm.PurchaseWareHouseItemList = PurchaseWareHouseItemList;
                        vm.Guids = PurchaseWareHouseItemList.Select(x => x.CommodityId).ToList();
                        view.ShowDialog();
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
        public DelegateCommand<object> PurchaseOrderCommand
        {
            get
            {
                return new DelegateCommand<object>(async (arg) =>
                {
                    if (arg != null)
                    {
                        Guid Id = (Guid)arg;  // 显式转换
                        var po = await db.FindAsync<PurchaseOrder>(Id);
                        if(po != null)
                        {
                            Entity = new PurchaseWareHouse()
                            {
                                CustomerCompanyId = po.CustomerCompanyId,
                                WareHouseId = po.WareHouseId,

                            };

                            PurchaseWareHouseItemList = new ObservableCollection<PurchaseWareHouseItemDto>();
                            var list = await db.QueryableAsync<PurchaseOrderItem>(x => x.PurchaseOrderId.Equals(Id));
                            foreach (PurchaseOrderItem item in list)
                            {
                                PurchaseWareHouseItemList.Add(new PurchaseWareHouseItemDto()
                                {
                                    CommodityId = item.CommodityId,
                                    Count = item.Count,
                                    Price = item.Price,
                                    Amount = item.Amount,
                                    CreateTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
                                    LastUpdateTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
                                    ModeInfoId = item.ModeInfoId,
                                    Rebate = item.Rebate,
                                    Remark = item.Remark,
                                    PurchaseWareHouseId = Entity.Id
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
        private ObservableCollection<PurchaseOrder> purchaseOrderList;
        public ObservableCollection<PurchaseOrder> PurchaseOrderList
        {
            get { return purchaseOrderList; }
            set { SetProperty(ref purchaseOrderList, value); }
        }
        private ObservableCollection<ModeInfo> modeInfoList;
        public ObservableCollection<ModeInfo> ModeInfoList
        {
            get { return modeInfoList; }
            set { SetProperty(ref modeInfoList, value); }
        }
    }
}
