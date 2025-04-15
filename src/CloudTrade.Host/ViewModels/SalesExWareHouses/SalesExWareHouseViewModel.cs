using CloudTrade.Application.Contracts.PurchaseOrders;
using CloudTrade.Application.Contracts.SalesExWareHouses;
using CloudTrade.Domain.Positions;
using CloudTrade.Domain.PurchaseOrderItems;
using CloudTrade.Domain.PurchaseOrders;
using CloudTrade.Domain.SalesExWareHouseItem;
using CloudTrade.Domain.SalesExWareHouses;
using CloudTrade.Host.Resources.Event;
using CloudTrade.Host.Resources.Helper;
using CloudTrade.Host.Views.SalesExWareHouses;
using HandyControl.Controls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Unity.Storage.RegistrationSet;

namespace CloudTrade.Host.ViewModels.SalesExWareHouses
{
    public class SalesExWareHouseViewModel : BindableBase, INavigationAware
    {
        private readonly ISalesExWareHouseService db;
        private readonly IEventAggregator eventAggregator;
        public SalesExWareHouseViewModel(ISalesExWareHouseService db, IEventAggregator eventAggregator)
        {
            this.db = db;
            this.eventAggregator = eventAggregator;
            InitData();
          
        }

        public DelegateCommand InsertCommand
        {
            get
            {
                return new DelegateCommand(async () =>
                {
                    try
                    {
                        eventAggregator.GetEvent<VisibilityChangeEvent>().Publish();
                        var view = new SalesExWareHouseDialogView();
                        view.Owner = System.Windows.Application.Current.MainWindow;
                        view.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                        var vm = new SalesExWareHouseDialogViewModel(db);
                        vm.Title = "添加";
 
                        view.DataContext = vm;
                        bool? result = view.ShowDialog();
                        if (result == true)
                        {
                            AppData.ShowLoadingWindow();
                            InitData();
                            AppData.CloseLoadingWindow();
                            HandyControl.Controls.Growl.Success("添加成功");

                        }
                        eventAggregator.GetEvent<VisibilityChangeEvent>().Publish();
                    }
                    catch (Exception ex)
                    {
                        Growl.Error(ex.Message);
                    }
                });
            }
        }
        public DelegateCommand<IList> DeleteCommand
        {
            get
            {
                return new DelegateCommand<IList>(async (arg) =>
                {
                    try
                    {

                  
                        if (arg == null) return;
                        if (arg.Count == 0)
                        {
                            HandyControl.Controls.Growl.Warning("请选择一条数据");
                            return;
                        }
                        else
                        {
                            eventAggregator.GetEvent<VisibilityChangeEvent>().Publish();
                            var dialog = HandyControl.Controls.MessageBox.Show("是否删除", "删除", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                            if (dialog == MessageBoxResult.Yes)
                            {
                                AppData.ShowLoadingWindow();

                                foreach (SalesExWareHouse item in arg)
                                {
                                    if (item.OrderState == 1)
                                    {
                                        // Growl.Info("已跳过已审核订单删除");
                                        continue;

                                    }
                                    else
                                    {
                                        await db.DeleteAsync<SalesExWareHouse>(item);
                                        await db.DeleteAsync<SalesExWareHouseItem>(item.Id);
                                    }
                                }
                                InitData();
                                AppData.CloseLoadingWindow();
                                Growl.Success("删除成功");
                            }
                            eventAggregator.GetEvent<VisibilityChangeEvent>().Publish();
                        }
                    }
                    catch (Exception ex)
                    {
                        Growl.Error(ex.Message);
                    }
                });
            }
        }



        public DelegateCommand<IList> UpdateCommand
        {
            get
            {
                return new DelegateCommand<IList>(async (arg) =>
                {
                    try
                    {

                  
                        if (arg.Count == 0 || arg.Count > 1 || arg.Count == 0)
                        {
                            HandyControl.Controls.Growl.Warning("请选中一条数据进行修改");
                        }
                        else if (arg[0] is SalesExWareHouse entity)
                        {
                            eventAggregator.GetEvent<VisibilityChangeEvent>().Publish();
                            var view = new SalesExWareHouseDialogView();
                            var vm = new SalesExWareHouseDialogViewModel(db);
                            view.Owner = System.Windows.Application.Current.MainWindow;
                            view.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                            view.DataContext = vm;
                            vm.Title = "修改";

                            vm.Entity = entity;

                            bool? result = view.ShowDialog();
                            if (result == true)
                            {

                                AppData.ShowLoadingWindow();
                                InitData();
                                AppData.CloseLoadingWindow();

                                HandyControl.Controls.Growl.Success("修改成功");
                            }
                            eventAggregator.GetEvent<VisibilityChangeEvent>().Publish();
                        }

                    }
                    catch (Exception ex)
                    {
                        Growl.Error(ex.Message);
                    }
                });
            }
        }
        public DelegateCommand<ArrayList> SelectCommand
        {
            get
            {
                return new DelegateCommand<ArrayList>((arg) =>
                {
                    try
                    {

                        eventAggregator.GetEvent<VisibilityChangeEvent>().Publish();
                        AppData.ShowLoadingWindow();
                        InitData(PageHelper.PageSize(Convert.ToInt32(arg[0])), arg[1]?.ToString() ?? "", arg[2]?.ToString() ?? "", arg[3]?.ToString() ?? "");

                        AppData.CloseLoadingWindow();
                        eventAggregator.GetEvent<VisibilityChangeEvent>().Publish();

                    }
                    catch (Exception ex)
                    {
                        Growl.Error(ex.Message);
                    }
                });
            }
        }

        public DelegateCommand<IList> ViewCommand
        {
            get => new DelegateCommand<IList>(async (arg) =>
            {
                try
                {

                    //var list = arg as System.Collections.IList;
                    if (arg.Count == 0 || arg.Count > 1 || arg.Count == 0)
                    {
                        HandyControl.Controls.Growl.Warning("请选中一条数据");
                    }
                    else if (arg[0] is SalesExWareHouseDto entity)
                    {
                        //eventAggregator.GetEvent<VisibilityChangeEvent>().Publish();


                        //eventAggregator.GetEvent<VisibilityChangeEvent>().Publish();

                        IsOpen = !IsOpen;
                        Entity = entity;
                    
                        SalesExWareHouseItemList = new(await db.SalesExWareHouseViewAsync(entity.Id));

                    }

                }
                catch (Exception ex)
                {
                    Growl.Error(ex.Message);
                }

            });
        }
        public DelegateCommand infoCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {

                    IsOpen = !IsOpen;
                });
            }
        }


        public async void InitData(int PageSize=10,string query = "",string StartTime="",string EndTime="")
        {
            try
            {

                (var list, PageCount) = await db.SalesExWarehouseQueryAsync(PageIndex,PageSize,query,StartTime,EndTime);
                SalesExWarehouseList = new(list);
            }
            catch (Exception ex)
            {
                Growl.Error(ex.Message);
            }
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {

        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return false;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {

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


        private bool isOpen = false;
        public bool IsOpen
        {
            get => isOpen;
            set => SetProperty(ref isOpen, value);
        }

        //private ObservableCollection<Position> positionList;
        //public ObservableCollection<Position> PositionList
        //{
        //    get => positionList;
        //    set => SetProperty(ref positionList, value);
        //}
        //private ObservableCollection<Department> departmentList;
        //public ObservableCollection<Department> DepartmentList
        //{
        //    get => departmentList;
        //    set => SetProperty(ref departmentList, value);
        //}

        private ObservableCollection<SalesExWareHouseDto> salesExWarehouseList;
        public ObservableCollection<SalesExWareHouseDto> SalesExWarehouseList
        {
            get => salesExWarehouseList;
            set => SetProperty(ref salesExWarehouseList, value);
        }
        private ObservableCollection<SalesExWareHouseItemDto> salesExWareHouseItemList;
        public ObservableCollection<SalesExWareHouseItemDto> SalesExWareHouseItemList
        {
            get => salesExWareHouseItemList;
            set => SetProperty(ref salesExWareHouseItemList, value);
        }
        private SalesExWareHouseDto entity;
        public SalesExWareHouseDto Entity
        {
            get => entity;
            set => SetProperty(ref entity, value);
        }
    }
}
