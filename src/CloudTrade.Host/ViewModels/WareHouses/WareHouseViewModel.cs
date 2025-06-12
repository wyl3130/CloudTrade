using CloudTrade.Domain.Commoditys;
using CloudTrade.Domain.WareHouses;
using CloudTrade.Host.Resources.Event;
using CloudTrade.Host.ViewModels.CommodityCategorys;
using CloudTrade.Host.Views.WareHouses;
using HandyControl.Controls;
using Prism.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Host.ViewModels.WareHouses
{
    public class WareHouseViewModel : BindableBase, INavigationAware
    {
        private readonly IBaseService db;
        private readonly IEventAggregator eventAggregator;
        public WareHouseViewModel(IBaseService db, IEventAggregator eventAggregator)
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
                        var view = new WareHouseDialogView();
                        view.Owner = System.Windows.Application.Current.MainWindow;
                        view.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                        var vm = new WareHouseDialogViewModel(db);
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
                            // HandyControl.Controls.MessageBox.Show("请选择一条数据", "删除", MessageBoxButton.OK, MessageBoxImage.Error);
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
                                await Task.Run(async () =>
                                {
                                    await Task.Delay(1000);
                                    foreach (WareHouse item in arg)
                                    {
                                        //var list = await db.QueryableAsync<WareHouse>(x => x.CommodityCategoryId.Equals(item.Id));
                                        //if (list.Count() != 0)
                                        //{
                                        //    Growl.Warning("当前商品类别关联了商品，要删除类别名称请先修改商品的类别");
                                        //    break;
                                        //}
                                        //else
                                        //{
                                        //    await db.DeleteAsync<WareHouse>(item);
                                        //}
                                        await db.DeleteAsync<WareHouse>(item);
                                    }


                                    InitData();
                                });
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
                        eventAggregator.GetEvent<VisibilityChangeEvent>().Publish();
               
                        if (arg.Count == 0 || arg.Count > 1 || arg.Count == 0)
                        {
                            HandyControl.Controls.Growl.Warning("请选中一条数据进行修改");


                        }

                        else if (arg[0] is WareHouse entity)
                        {
                            var view = new WareHouseDialogView();
                            var vm = new WareHouseDialogViewModel(db);
                            vm.Title = "修改";
                            view.Owner = System.Windows.Application.Current.MainWindow;
                            view.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                            view.DataContext = vm;
                            vm.Entity = entity;

                            bool? result = view.ShowDialog();
                            if (result == true)
                            {
                                AppData.ShowLoadingWindow();
                                InitData();
                                AppData.CloseLoadingWindow();
                                HandyControl.Controls.Growl.Success("修改成功");
                            }
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
        public DelegateCommand<object> SelectCommand
        {
            get
            {
                return new DelegateCommand<object>((arg) =>
                {
                    try
                    {
                        if (arg is String query)
                        {
                            eventAggregator.GetEvent<VisibilityChangeEvent>().Publish();

                            AppData.ShowLoadingWindow();
                            InitData(query);
                            AppData.CloseLoadingWindow();

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

        public async void InitData(string query = "")
        {
            var queryCondition = await db.QueryableAsync<WareHouse>(x => x.WareHouseName.Contains(query));
            var list = queryCondition.Skip((PageIndex - 1) * 10).Take(10).ToList();
            WareHouseList = new ObservableCollection<WareHouse>(list);
            var totalCount = queryCondition.Count();
            PageCount = (int)Math.Ceiling((double)totalCount / 10);
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
        private string query = "";
        public string Query
        {
            get => query;
            set => SetProperty(ref query, value);
        }

        private bool isOpen = false;
        public bool IsOpen
        {
            get => isOpen;
            set => SetProperty(ref isOpen, value);
        }
        private ObservableCollection<WareHouse> wareHouseList;
        public ObservableCollection<WareHouse> WareHouseList
        {
            get => wareHouseList;
            set => SetProperty(ref wareHouseList, value);
        }
    }
}