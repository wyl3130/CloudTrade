using CloudTrade.Application.Contracts.ModeInfos;
using CloudTrade.Application.Contracts.PurchaseOrders;
using CloudTrade.Application.Contracts.PurchaseRefunds;
using CloudTrade.Domain.CustomerCompanys;
using CloudTrade.Domain.Employees;
using CloudTrade.Domain.ModeInfos;
using CloudTrade.Domain.PurchaseOrders;
using CloudTrade.Domain.PurchaseRefunds;
using CloudTrade.Domain.WareHouses;
using CloudTrade.Host.ViewModels.Mains;
using CloudTrade.Host.Views.Mains;
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

                        if (Entity != null && !string.IsNullOrEmpty(Entity.RealName))
                        {

                            Entity.CodeNo = "CT" + DateTime.Now.ToString("yyyyMMddHHmmss");
                            Entity.ExWareHouseDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            Entity.CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            Entity.LastUpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            var result = await db.InsertAsync<PurchaseRefund>(Entity);
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
                        if (Entity != null && !string.IsNullOrEmpty(Entity.RealName))
                        {
                            //var list = db.Queryable<Department>().Where(x => x.DepartmentName.Equals(Entity.DepartmentName));
                            //if (list.Count() != 0)
                            //{
                            //    HandyControl.Controls.MessageBox.Show("部门已存在", "修改", MessageBoxButton.OK, MessageBoxImage.Error);
                            //    return;
                            //}
                            Entity.LastUpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            var result = await db.UpdateAsync<PurchaseRefund>(Entity);
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
                        var view = new CommodityListDialogView();

                        var vm = new CommodityListDialogViewModel(db);

                        view.Owner = System.Windows.Application.Current.MainWindow;
                        view.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                        view.DataContext = vm;
                        vm.Title = "采购退库单";
                        vm.OrderId = Entity.Id;
                        vm.PurchaseRefundItemList = PurchaseRefundItemList;
                        vm.Guids = PurchaseRefundItemList.Select(x => x.CommodityId).ToList();
                        view.ShowDialog();
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
        private PurchaseRefund entity = new() { Id=Guid.NewGuid()};
        public PurchaseRefund Entity
        {
            get => entity;
            set => SetProperty(ref entity, value);
        }

        private ObservableCollection<PurchaseRefundItemDto> purchaseRefundItemList;
        public ObservableCollection<PurchaseRefundItemDto> PurchaseRefundItemList
        {
            get { return purchaseRefundItemList; }
            set { SetProperty(ref purchaseRefundItemList, value); }
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
