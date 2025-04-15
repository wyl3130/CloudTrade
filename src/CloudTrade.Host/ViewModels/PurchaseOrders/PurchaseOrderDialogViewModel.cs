using CloudTrade.Application.Contracts.ModeInfos;
using CloudTrade.Application.Contracts.PurchaseOrders;
using CloudTrade.Domain.CustomerCompanys;
using CloudTrade.Domain.Employees;
using CloudTrade.Domain.ModeInfos;
using CloudTrade.Domain.PurchaseOrderItems;
using CloudTrade.Domain.PurchaseOrders;
using CloudTrade.Domain.WareHouses;
using CloudTrade.Host.ViewModels.Mains;
using CloudTrade.Host.Views.Mains;
using CloudTrade.Host.Views.PurchaseOrders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace CloudTrade.Host.ViewModels.PurchaseOrders
{
    public class PurchaseOrderDialogViewModel : BindableBase
    {
        private readonly IPurchaseOrderService db;
        public PurchaseOrderDialogViewModel(IPurchaseOrderService db)
        {
            this.db = db;
            PurchaseOrderItemList = new();

            InitData();
          
        }
        private DispatcherTimer _timer;
        public async void InitData()
        {
            Entity.Preparer = AppData.user.UserName;
            CustomerCompanyList = new(await db.QueryableAsync<CustomerCompany>());
            EmployeList = new(await db.QueryableAsync<Employe>());
            WareHouseList = new(await db.QueryableAsync<WareHouse>());
            ModeInfoList = new(await db.QueryableAsync<ModeInfo>());
        }
        public DelegateCommand<object> enterCommand
        {
            get => new DelegateCommand<object>(async (arg) =>
            {
                //System.Windows.MessageBox.Show(PurchaseOrderItemList[0].Price.ToString());
                if (arg is Window view)
                {
                    if (Title.Equals("添加"))
                    {

                        if (Entity != null && !string.IsNullOrEmpty(Entity.RealName))
                        {
                            Entity.CodeNo = "CD" + DateTime.Now.ToString("yyyyMMddHHmmss");
                            Entity.WareHouseDate = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                            Entity.CreateTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                            Entity.LastUpdateTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                            var result = await db.InsertAsync<PurchaseOrder>(Entity);
                            if (result)
                            {
                                //foreach (var item in PurchaseOrderItemList)
                                //{
                                //    await db.InsertAsync<PurchaseOrderItem>(item);
                                //}
                                await db.InsertAsync<PurchaseOrderItem>(PurchaseOrderItemList);
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
                        if (Entity != null && !string.IsNullOrEmpty(Entity.RealName))
                        {

                            Entity.LastUpdateTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                            var result = await db.UpdateAsync<PurchaseOrder>(Entity);
                            if (result)
                            {
                                var list = await db.QueryableAsync<PurchaseOrderItem>(x=>x.PurchaseOrderId.Equals(Entity.Id));
                                await db.DeleteAsync<PurchaseOrderItem>(list);
                              
                                foreach (var item in PurchaseOrderItemList)
                                {
                                    await db.InsertAsync<PurchaseOrderItem>(item);
                                }

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
                        // v.Hide();
                        var view = new CommodityListDialogView();

                        var vm = new CommodityListDialogViewModel(db);
                       
                        view.Owner = System.Windows.Application.Current.MainWindow;
                        view.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                        view.DataContext = vm;
                        vm.Title = "采购订单";
                        vm.OrderId = Entity.Id;
                        vm.PurchaseOrderItemList = PurchaseOrderItemList;
                        vm.Guids = PurchaseOrderItemList.Select(x => x.CommodityId).ToList();
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
                    if(arg.Count==1)
                    {
                        if (arg[0] is PurchaseOrderItemDto entity)
                        {
                            PurchaseOrderItemList.Remove(entity);
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
                    var list = PurchaseOrderItemList;
                    foreach (var item in list)
                    {
                        item.TaxPrice = item.Price * (1 + item.Tax);  // 含税单价 = 单价 * (1 + 税率)
                        item.Amount = item.Price * item.Count;         // 金额 = 单价 * 数量
                        item.TaxAmount = item.Amount * item.Tax;       // 税额 = 金额 * 税率
                    }
                    PurchaseOrderItemList = new(list);
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

        public DelegateCommand btnCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    MessageBox.Show("");
                });
            }
        }
        private string title = "添加";
        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }
        private PurchaseOrder entity = new() { Id=Guid.NewGuid()};
        public PurchaseOrder Entity
        {
            get => entity;
            set => SetProperty(ref entity, value);
        }

        private ObservableCollection<PurchaseOrderItemDto> purchaseOrderItemList;
        public ObservableCollection<PurchaseOrderItemDto> PurchaseOrderItemList
        {
            get { return purchaseOrderItemList; }
            set { SetProperty(ref purchaseOrderItemList, value); }
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
        private ObservableCollection<ModeInfo> modeInfoList;
        public ObservableCollection<ModeInfo> ModeInfoList
        {
            get { return modeInfoList; }
            set { SetProperty(ref modeInfoList, value); }
        }
    }
}
