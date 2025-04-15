using CloudTrade.Application.Contracts.Positions;
using CloudTrade.Application.Contracts.PurchaseOrders;
using CloudTrade.Domain.Departments;
using CloudTrade.Domain.Employees;
using CloudTrade.Domain.Positions;
using CloudTrade.Domain.PurchaseOrderItems;
using CloudTrade.Domain.PurchaseOrders;
using CloudTrade.Host.Resources.Event;
using CloudTrade.Host.Resources.Helper;
using CloudTrade.Host.ViewModels.Positions;
using CloudTrade.Host.Views.Positions;
using CloudTrade.Host.Views.PurchaseOrders;
using HandyControl.Controls;
using Microsoft.Toolkit.Uwp.Notifications;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace CloudTrade.Host.ViewModels.PurchaseOrders
{
    public class PurchaseOrderViewModel : BindableBase, INavigationAware
    {
        private readonly IPurchaseOrderService db;
        private readonly IEventAggregator eventAggregator;
        public PurchaseOrderViewModel(IPurchaseOrderService db, IEventAggregator eventAggregator)
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
                        var view = new PurchaseOrderDialogView();
                        var vm = new PurchaseOrderDialogViewModel(db);
                        view.Owner = System.Windows.Application.Current.MainWindow;
                        view.WindowStartupLocation = WindowStartupLocation.CenterOwner;
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

                        //System.Collections.IList list = arg as System.Collections.IList;
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
                                foreach (PurchaseOrder item in arg)
                                {
                                    if(item.OrderState==1)
                                    {
                                       // Growl.Info("已跳过已审核订单删除");
                                        continue;
                                        
                                    }
                                    else
                                    {
                                        await db.PurchaseOrderDeleteAsync(item);
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

                        // var list = arg as System.Collections.IList;
                        if (arg.Count == 0 || arg.Count > 1 || arg.Count == 0)
                        {
                            HandyControl.Controls.Growl.Warning("请选中一条数据进行修改");
                        }
                        else if (arg[0] is PurchaseOrder entity)
                        {
                            if (entity.OrderState == 0)
                            {
                                eventAggregator.GetEvent<VisibilityChangeEvent>().Publish();
                                var view = new PurchaseOrderDialogView();
                                var vm = new PurchaseOrderDialogViewModel(db);
                                view.Owner = System.Windows.Application.Current.MainWindow;
                                view.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                                view.DataContext = vm;
                                vm.Title = "修改";
                                // vm.DepartmentList = DepartmentList;

                                vm.Entity = entity;
                                //var list = await db.
                                //

                                vm.PurchaseOrderItemList = new ObservableCollection<PurchaseOrderItemDto>(await db.PurchaseOrderViewAsync(entity.Id));
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
                            else
                            {
                                Growl.Info("已审核订单无法修改");
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
        public DelegateCommand<ArrayList> SelectCommand
        {
            get
            {
                return new DelegateCommand<ArrayList>((arg) =>
                {
                    try
                    {

                        //System.Windows.MessageBox.Show(DateTime.Now.ToString());
                        eventAggregator.GetEvent<VisibilityChangeEvent>().Publish();
                        //AppData.ShowLoadingWindow();
                        //System.Windows.MessageBox.Show(Convert.ToDateTime(arg[3]).ToString("yyyy-MM-dd"));
                        InitData(PageHelper.PageSize(Convert.ToInt32(arg[0])), arg[1]?.ToString() ?? "", arg[2]?.ToString() ?? "", arg[3]?.ToString() ?? "");

                        // InitData(arg.ToString());
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


        public async void InitData(int PageSize = 10, string query = "", string StartTime = "", string EndTime = "")
        {
            try
            {

                (var list, PageCount) = await db.PurchaseOrderQueryAsync(PageIndex, PageSize, query, StartTime, EndTime);
                PurchaseOrderList = new ObservableCollection<PurchaseOrderDto>(list);
            }
            catch (Exception ex)
            {
                Growl.Error(ex.Message);
            }
        }
        public DelegateCommand btnCommand
        {
            get => new DelegateCommand(() =>
            {
                var builder = new ToastContentBuilder()
                    //.AddArgument("action", "viewConversation")
                    //.AddArgument("conversationId", 9813)

                    .AddText("今晚又要当机长起飞了")

                    .AddButton(new ToastButton()
                        .SetContent("确定")
                        .AddArgument("action", "archive"))
                    .AddButton(new ToastButton()
                        .SetContent("取消")
                        .AddArgument("action", "archive"))
                    ;
                builder.Show();
            });
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
                    else if (arg[0] is PurchaseOrderDto entity)
                    {
                        //eventAggregator.GetEvent<VisibilityChangeEvent>().Publish();


                        //eventAggregator.GetEvent<VisibilityChangeEvent>().Publish();

                        IsOpen = !IsOpen;
                        Entity = entity;
  
                        PurchaseOrderItemList = new(await db.PurchaseOrderViewAsync(entity.Id));

                    }

                }
                catch (Exception ex)
                {
                    Growl.Error(ex.Message);
                }

            });
        }
        public DelegateCommand<object> ExportCommand
        {
            get
            {
                return new DelegateCommand<object>((arg) =>
                {

                    if (arg is IList list)
                    {
                        if (list.Count == 0)
                        {
                            HandyControl.Controls.Growl.Warning("请选择一条数据");
                            return;
                        }

                        using (var package = new ExcelPackage())
                        {
                            var worksheet = package.Workbook.Worksheets.Add("采购订单");


                            worksheet.Cells[1, 1].Value = "编码";
                            worksheet.Cells[1, 2].Value = "客户公司";
                            worksheet.Cells[1, 3].Value = "仓库名称";
                            worksheet.Cells[1, 4].Value = "负责人";
                            worksheet.Cells[1, 5].Value = "订单状态";
                            worksheet.Cells[1, 6].Value = "制单人";
                            worksheet.Cells[1, 7].Value = "入库时间";
                            worksheet.Cells[1, 8].Value = "备注";
                            worksheet.Cells[1, 9].Value = "创建时间";
                            worksheet.Cells[1, 10].Value = "最后修改时间";
                            int row = 2;
                            foreach (PurchaseOrderDto item in list)
                            {
                                worksheet.Cells[row, 1].Value = item.CodeNo;
                                worksheet.Cells[row, 2].Value = item.CustomerCompanyName;
                                worksheet.Cells[row, 3].Value = item.WareHouseName;
                                worksheet.Cells[row, 4].Value = item.RealName;
                                worksheet.Cells[row, 5].Value = item.OrderState;
                                worksheet.Cells[row, 6].Value = item.Preparer;
                                worksheet.Cells[row, 7].Value = item.WareHouseDate;
                                worksheet.Cells[row, 8].Value = item.Remark;
                                worksheet.Cells[row, 9].Value = item.CreateTime;
                                worksheet.Cells[row, 10].Value = item.LastUpdateTime;
                                row++;
                            }
                            worksheet.Cells.AutoFitColumns();
                            // 获取桌面路径
                            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                            // 设置文件保存路径（桌面路径 + 文件名）
                            var filePath = System.IO.Path.Combine(desktopPath, $"采购订单.xlsx");

                            // 保存到桌面
                            FileInfo fileInfo = new FileInfo(filePath);
                            package.SaveAs(fileInfo);

                            System.Windows.MessageBox.Show($"Excel文件已成功导出到桌面: {filePath}");
                        }
                    }
                    else
                    {
                        //System.Windows.MessageBox.Show(Entity.Id.ToString());
                        // 创建Excel包
                        using (var package = new ExcelPackage())
                        {
                            // 创建工作表
                            var worksheet = package.Workbook.Worksheets.Add("采购订单详情");



                            // 添加标题行
                            // 合并A1到I2的单元格
                            worksheet.Cells["A1:J2"].Merge = true;

                            // 在合并后的单元格中添加内容
                            worksheet.Cells["A1"].Value = "采购订单";

                            // 设置合并单元格的水平和垂直居中
                            worksheet.Cells["A1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            worksheet.Cells["A1"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;


                            worksheet.Cells["A3:B3"].Merge = true;
                            worksheet.Cells["A3"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            worksheet.Cells["A3"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            worksheet.Cells["A3"].Value = "客户公司";
                            worksheet.Cells["C3:D3"].Merge = true;
                            worksheet.Cells["C3"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            worksheet.Cells["C3"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            worksheet.Cells["C3"].Value = Entity.CustomerCompanyName;
                            worksheet.Cells["F3:G3"].Merge = true;
                            worksheet.Cells["F3"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            worksheet.Cells["F3"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            worksheet.Cells["F3"].Value = "仓库名称";
                            worksheet.Cells["H3:I3"].Merge = true;
                            worksheet.Cells["H3"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            worksheet.Cells["H3"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            worksheet.Cells["H3"].Value = Entity.WareHouseName;

                            worksheet.Cells["A4:B4"].Merge = true;
                            worksheet.Cells["A4"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            worksheet.Cells["A4"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            worksheet.Cells["A4"].Value = "负责人";
                            worksheet.Cells["C4:D4"].Merge = true;
                            worksheet.Cells["C4"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            worksheet.Cells["C4"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            worksheet.Cells["C4"].Value = Entity.RealName;
                            worksheet.Cells["F4:G4"].Merge = true;
                            worksheet.Cells["F4"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            worksheet.Cells["F4"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            worksheet.Cells["F4"].Value = "入库时间";
                            worksheet.Cells["H4:I4"].Merge = true;
                            worksheet.Cells["H4"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            worksheet.Cells["H4"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            worksheet.Cells["H4"].Value = Entity.WareHouseDate;

                            worksheet.Cells["A5:B5"].Merge = true;
                            worksheet.Cells["A5"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            worksheet.Cells["A5"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            worksheet.Cells["A5"].Value = "编号";
                            worksheet.Cells["C5:D5"].Merge = true;
                            worksheet.Cells["C5"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            worksheet.Cells["C5"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            worksheet.Cells["C5"].Value = Entity.CodeNo;
                            worksheet.Cells["F5:G5"].Merge = true;
                            worksheet.Cells["F5"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            worksheet.Cells["F5"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            worksheet.Cells["F5"].Value = "备注";
                            worksheet.Cells["H5:I5"].Merge = true;
                            worksheet.Cells["H5"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            worksheet.Cells["H5"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            worksheet.Cells["H5"].Value = Entity.Remark;


                            //worksheet.Cells[3, 1].Value = "客户公司";
                            //worksheet.Cells[3, 2].Value = Entity.CustomerCompanyName;
                            //worksheet.Cells[3, 3].Value = "负责人";
                            //worksheet.Cells[3, 4].Value = Entity.RealName;
                            //worksheet.Cells[3, 5].Value = "仓库名称";
                            //worksheet.Cells[3, 6].Value = Entity.WareHouseName;
                            //worksheet.Cells[3, 7].Value = "入库时间";


                            //worksheet.Cells["H3:J3"].Merge = true;
                            //worksheet.Cells["H3"].Value = Entity.WareHouseDate; 
                            //worksheet.Cells["H3"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            //worksheet.Cells["H3"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            worksheet.Cells[6, 1].Value = "商品名称";
                            worksheet.Cells[6, 2].Value = "金额";
                            worksheet.Cells[6, 3].Value = "含税单价";
                            worksheet.Cells[6, 4].Value = "含税金额";
                            worksheet.Cells[6, 5].Value = "单价";
                            worksheet.Cells[6, 6].Value = "数量";
                            worksheet.Cells[6, 7].Value = "税率";
                            worksheet.Cells[6, 8].Value = "返利";
                            worksheet.Cells[6, 9].Value = "方式";
                            worksheet.Cells[6, 10].Value = "备注";

                            // 填充数据
                            int row = 7;
                            double total = 0.0d;
                            foreach (var item in PurchaseOrderItemList)
                            {
                                worksheet.Cells[row, 1].Value = item.CommodityName;
                                worksheet.Cells[row, 2].Value = item.Amount;
                                worksheet.Cells[row, 3].Value = item.TaxPrice;
                                worksheet.Cells[row, 4].Value = item.TaxAmount;
                                worksheet.Cells[row, 5].Value = item.Price;
                                worksheet.Cells[row, 6].Value = item.Count;
                                worksheet.Cells[row, 7].Value = item.Tax;
                                worksheet.Cells[row, 8].Value = item.Rebate;
                                worksheet.Cells[row, 9].Value = item.ModeInfoName;
                                worksheet.Cells[row, 10].Value = item.Remark;
                                total += item.Amount;
                                row++;
                            }

                            worksheet.Cells[row, 1].Value = "合计";
                            worksheet.Cells[row, 2].Value = total;

                            worksheet.Cells[row, 4].Value = "制单人";
                            worksheet.Cells[row, 5].Value = Entity.Preparer;

                            // 获取桌面路径
                            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                            // 设置文件保存路径（桌面路径 + 文件名）
                            var filePath = System.IO.Path.Combine(desktopPath, $"{Entity.CodeNo}.xlsx");

                            // 保存到桌面
                            FileInfo fileInfo = new FileInfo(filePath);
                            package.SaveAs(fileInfo);

                            System.Windows.MessageBox.Show($"Excel文件已成功导出到桌面: {filePath}");
                        }
                    }
                });
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

        private ObservableCollection<PurchaseOrderDto> purchaseOrderList;
        public ObservableCollection<PurchaseOrderDto> PurchaseOrderList
        {
            get => purchaseOrderList;
            set => SetProperty(ref purchaseOrderList, value);
        }

        private ObservableCollection<PurchaseOrderItemDto> purchaseOrderItemList;
        public ObservableCollection<PurchaseOrderItemDto> PurchaseOrderItemList
        {
            get => purchaseOrderItemList;
            set => SetProperty(ref purchaseOrderItemList, value);
        }
        private PurchaseOrderDto entity;
        public PurchaseOrderDto Entity
        {
            get => entity;
            set => SetProperty(ref entity, value);
        }

    }
}
