using CloudTrade.Application.Contracts.Employees;
using CloudTrade.Application.Contracts.Positions;
using CloudTrade.Domain.Commoditys;
using CloudTrade.Domain.Departments;
using CloudTrade.Domain.Employees;
using CloudTrade.Domain.Positions;
using CloudTrade.Host.Resources.Event;
using CloudTrade.Host.Resources.Helper;
using CloudTrade.Host.ViewModels.CommodityCompanys;
using CloudTrade.Host.Views.Employees;
using HandyControl.Controls;
using Prism.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Host.ViewModels.Employees
{
    public class EmployeViewModel : BindableBase, INavigationAware
    {
        private readonly IEmployeService db;
        private readonly IPositionService pdb;
        private readonly IEventAggregator eventAggregator;
        public EmployeViewModel(IEmployeService db, IPositionService pdb, IEventAggregator eventAggregator)
        {
            this.db = db;
            this.pdb = pdb;
            this.eventAggregator = eventAggregator;
            DepartmentList = new ObservableCollection<Department>(db.Queryable<Department>());
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
                        var view = new EmployeDialogView();
                        view.Owner = System.Windows.Application.Current.MainWindow;
                        view.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                        var vm = new EmployeDialogViewModel(db);
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
                            // HandyControl.Controls.MessageBox.Show("请选择一条数据", "删除", MessageBoxButton.OK, MessageBoxImage.Error);
                            HandyControl.Controls.Growl.Warning("请选择一条数据");
                            return;
                        }
                        eventAggregator.GetEvent<VisibilityChangeEvent>().Publish();
                        var dialog = HandyControl.Controls.MessageBox.Show("是否删除", "删除", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                        if (dialog == MessageBoxResult.Yes)
                        {
                            AppData.ShowLoadingWindow();

                            foreach (Employe item in arg)
                            {
                                await db.DeleteAsync<Employe>(item);
                            }
                            InitData();

                            AppData.CloseLoadingWindow();
                            Growl.Success("删除成功");
                        }
                        eventAggregator.GetEvent<VisibilityChangeEvent>().Publish();


                    }
                    catch (Exception ex)
                    {

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
                        else if (arg[0] is Employe entity)
                        {
                            eventAggregator.GetEvent<VisibilityChangeEvent>().Publish();
                            var view = new EmployeDialogView();
                            var vm = new EmployeDialogViewModel(db);
                            view.Owner = System.Windows.Application.Current.MainWindow;
                            view.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                            view.DataContext = vm;
                            vm.Entity = entity;
                            vm.DepartmentList = DepartmentList;

                            vm.Title = "修改";
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
        public DelegateCommand<object> cboCommand
        {
            get
            {
                return new DelegateCommand<object>(async (arg) =>
                {
                    try
                    {

                        if (arg is List<object> list)
                        {
                            if (!string.IsNullOrEmpty(list[1].ToString()) && list[1].ToString() != "")
                            {
                                var did = DepartmentList.Where(x => x.DepartmentName.Equals(list[1].ToString())).FirstOrDefault();
                                if (did != null)
                                {
                                    var plist = await db.QueryableAsync<Position>(x => x.DepartmentId.Equals(did.Id));
                                    PositionList = new ObservableCollection<Position>(plist);
                                }
                            }
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
        public async void InitData(int PageSize = 10, string query = "", string PositionName = "", string DepartmentName = "")
        {
            (var list, PageCount) = await db.EmployeQueryAsync(PageSize: PageSize, query: query, PositionName: PositionName, DepartmentName: DepartmentName, PageIndex: PageIndex);
            EmployeList = new ObservableCollection<EmployeDto>(list);
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
        private ObservableCollection<EmployeDto> employeList;
        public ObservableCollection<EmployeDto> EmployeList
        {
            get => employeList;
            set => SetProperty(ref employeList, value);
        }
        private ObservableCollection<Department> departmentList;
        public ObservableCollection<Department> DepartmentList
        {
            get => departmentList;
            set => SetProperty(ref departmentList, value);
        }
        private ObservableCollection<Position> positionList;
        public ObservableCollection<Position> PositionList
        {
            get => positionList;
            set => SetProperty(ref positionList, value);
        }
    }
}
