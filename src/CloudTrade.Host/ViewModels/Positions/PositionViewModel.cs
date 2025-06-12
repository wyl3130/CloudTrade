using CloudTrade.Application.Contracts.Departments;
using CloudTrade.Application.Contracts.Employees;
using CloudTrade.Application.Contracts.Positions;
using CloudTrade.Domain.Commoditys;
using CloudTrade.Domain.Departments;
using CloudTrade.Domain.Employees;
using CloudTrade.Domain.Positions;
using CloudTrade.Host.Resources.Event;
using CloudTrade.Host.Resources.Helper;
using CloudTrade.Host.ViewModels.CommodityCompanys;
using CloudTrade.Host.Views.Positions;
using HandyControl.Controls;
using OpenTK.Graphics.ES11;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Host.ViewModels.Positions
{
    public class PositionViewModel : BindableBase, INavigationAware
    {
        private readonly IPositionService db;
        private readonly IEventAggregator eventAggregator;
        public PositionViewModel(IPositionService db, IEventAggregator eventAggregator)
        {
            this.db = db;
            this.eventAggregator = eventAggregator;
            InitData();
            DepartmentList = new ObservableCollection<Department>(db.Queryable<Department>());
        }

        public DelegateCommand InsertCommand
        {
            get
            {
                return new DelegateCommand( () =>
                {
                    try
                    {
                        eventAggregator.GetEvent<VisibilityChangeEvent>().Publish();
                        var view = new PositionDialogView();
                        view.Owner = System.Windows.Application.Current.MainWindow;
                        view.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                        var vm = new PositionDialogViewModel(db);
                        vm.Title = "添加";
                        vm.DepartmentList = DepartmentList;
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

                                foreach (Position item in arg)
                                {
                                    var elist = await db.QueryableAsync<Employe>(x => x.PositionId.Equals(item.Id));
                                    if (elist.Count() != 0)
                                    {
                                        Growl.Warning("当前职位关联了员工");
                                        break;
                                    }
                                    else
                                    {
                                        await db.DeleteAsync<Position>(item);
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
                        else if (arg[0] is Position entity)
                        {
                            eventAggregator.GetEvent<VisibilityChangeEvent>().Publish();
                            var view = new PositionDialogView();
                            var vm = new PositionDialogViewModel(db);
                            view.Owner = System.Windows.Application.Current.MainWindow;
                            view.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                            view.DataContext = vm;
                            vm.Title = "修改";
                            vm.DepartmentList = DepartmentList;
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
                        InitData(PageHelper.PageSize(Convert.ToInt32(arg[0])), arg[1]?.ToString() ?? "", arg[2]?.ToString() ?? "");

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


        public async void InitData(int PageSize=10,string query = "", string DepartmentName = "")
        {
            try
            {

                (var list, PageCount) = await db.PositionQueryAsync(PageIndex,PageSize,query,DepartmentName);
                PositionList = new ObservableCollection<Position>(list);
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

        private ObservableCollection<Position> positionList;
        public ObservableCollection<Position> PositionList
        {
            get => positionList;
            set => SetProperty(ref positionList, value);
        }
        private ObservableCollection<Department> departmentList;
        public ObservableCollection<Department> DepartmentList
        {
            get => departmentList;
            set => SetProperty(ref departmentList, value);
        }

        private ObservableCollection<CommodityCompany> commodityCompanyList;
        public ObservableCollection<CommodityCompany> CommodityCompanyList
        {
            get => commodityCompanyList;
            set => SetProperty(ref commodityCompanyList, value);
        }
    }
}
