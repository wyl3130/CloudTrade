using CloudTrade.Application.Contracts.Departments;
using CloudTrade.Domain.Commoditys;
using CloudTrade.Domain.Departments;
using CloudTrade.Domain.Positions;
using CloudTrade.Host.Resources.Event;
using CloudTrade.Host.Resources.Helper;
using CloudTrade.Host.ViewModels.CommodityCompanys;
using CloudTrade.Host.Views.Departments;
using HandyControl.Controls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Host.ViewModels.Departments
{
    public class DepartmentViewModel : BindableBase, INavigationAware
    {
        private readonly IDepartmentService db;
        private readonly IEventAggregator eventAggregator;
        public DepartmentViewModel(IDepartmentService db, IEventAggregator eventAggregator)
        {
            this.db = db;
            this.eventAggregator = eventAggregator;
            InitData();
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
                        var view = new DepartmentDialogView();
                        view.Owner = System.Windows.Application.Current.MainWindow;
                        view.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                        var vm = new DepartmentDialogViewModel(db);
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
                        }
                        else
                        {
                            eventAggregator.GetEvent<VisibilityChangeEvent>().Publish();
                            var dialog = HandyControl.Controls.MessageBox.Show("是否删除", "删除", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                            if (dialog == MessageBoxResult.Yes)
                            {
                                AppData.ShowLoadingWindow();

                                foreach (Department item in arg)
                                {
                                    var plist = await db.QueryableAsync<Position>(x => x.DepartmentId.Equals(item.Id));
                                    if (plist.Count() != 0)
                                    {
                                        Growl.Warning("当前部门关联了职位，请先处理当前部门下的职位");
                                        break;
                                    }
                                    else
                                    {
                                        await db.DeleteAsync<Department>(item);
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
                            return;
                        }
                        else if (arg[0] is Department entity)
                        {
                            eventAggregator.GetEvent<VisibilityChangeEvent>().Publish();
                            var view = new DepartmentDialogView();
                            var vm = new DepartmentDialogViewModel(db);
                            view.Owner = System.Windows.Application.Current.MainWindow;
                            view.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                            vm.Title = "修改";
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
        public DelegateCommand<ArrayList> SelectCommand
        {
            get
            {
                return new DelegateCommand<ArrayList>(async (arg) =>
                {
                    try
                    {

                        eventAggregator.GetEvent<VisibilityChangeEvent>().Publish();

                        AppData.ShowLoadingWindow();

                        InitData(PageHelper.PageSize(Convert.ToInt32(arg[0])), arg[1]?.ToString() ?? "");


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
        public async void InitData(int PageSize=10,string query = "")
        {
            (var list, PageCount) = await db.DepartmentQueryAsync(PageIndex,PageSize,query);
            DepartmentList = new ObservableCollection<Department>(list);
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

        private ObservableCollection<Department> departmentList;
        public ObservableCollection<Department> DepartmentList
        {
            get => departmentList;
            set => SetProperty(ref departmentList, value);
        }
    }
}
