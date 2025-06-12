using CloudTrade.Application.Contracts.CommodityStocks;
using CloudTrade.Application.Contracts.Homes;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView.Extensions;
using LiveChartsCore.SkiaSharpView.VisualElements;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Host.ViewModels.Homes
{

    public class DashboardViewModel : BindableBase,INavigationAware
    {
        private readonly IDashboardService db;
        public DashboardViewModel(IDashboardService db)
        {
            LiveCharts.Configure(config => config.HasGlobalSKTypeface(SKFontManager.Default.MatchCharacter('汉')));

            this.db = db;
            Init();



        }
        private ISeries[] orderCount;
        public ISeries[] OrderCount
        {
            get => orderCount;
            set
            { SetProperty(ref orderCount, value); }
        }

        private Axis[] _xAxes1;
        public Axis[] XAxes1
        {
            get => _xAxes;
            set
            { SetProperty(ref _xAxes1, value); }
        }

        private Axis[] _yAxes1;
        public Axis[] YAxes1
        {
            get => _yAxes1;
            set
            { SetProperty(ref _yAxes1, value); }
        }
        private ISeries[] orders;
        public ISeries[] Orders
        {
            get => orders;
            set
            { SetProperty(ref orders, value); }
        }

        private Axis[] _ordersxAxes;
        public Axis[] OrdersxAxes
        {
            get => _ordersxAxes;
            set
            { SetProperty(ref _ordersxAxes, value); }
        }

        public async Task Init()
        {
            Orders = new ISeries[] {
                new ColumnSeries<double>(){
                    Name="采购订单",
                    Values=await db.PurchaseOrderRecentAsync()
                },
                new ColumnSeries<double>(){
                    Name="销售订单",
                    Values=await db.SalesOrderRecentAsync()
                },
            };

            OrdersxAxes = new Axis[]
            {
               new Axis(){
                Labels =days(),
                LabelsRotation = 0,
                SeparatorsPaint = new SolidColorPaint(new SKColor(200, 200, 200)),
                SeparatorsAtCenter = false,
                TicksPaint = new SolidColorPaint(new SKColor(35, 35, 35)),
                TicksAtCenter = true,
                // By default the axis tries to optimize the number of 
                // labels to fit the available space, 
                // when you need to force the axis to show all the labels then you must: 
                ForceStepToMin = true,
                MinStep = 1
               }
            };
            SalesOrderCount = await db.SalesOrderCount();
            SalesExWareHouseCount = db.SalesExWareHouseCount();

            CommodityStockList = new(db.CommodityStockList());


     






            // 1. 准备系列数据
            SalesOrder = new ISeries[]
            {
            new LineSeries<double>
            {
                Name = "销售额",
                Values = await db.SalesExWareHouseAmount(),
                Fill = null,
                Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 3 },
                GeometryStroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 3 },
                GeometrySize = 10
            },
            new ColumnSeries<double>
            {
                Name = "利润",
                Values = await db.SalesExWareHouseAmountAndProfit(),
                Fill = new SolidColorPaint(SKColors.Green)
            }
            };

            // 2. 设置X轴
            XAxes = new Axis[]
            {
            new Axis
            {
                Name = "月份",
                Labels = GetMonths(),
                LabelsRotation = 15,
                TextSize = 12,
                NameTextSize = 14
            }
            };

            // 3. 设置Y轴
            YAxes = new Axis[]
            {
            new Axis
            {
                Name = "金额(元)",
                TextSize = 12,
                NameTextSize = 14,
                Labeler = value => value.ToString("N0") // 格式化为数字
            }
            };


            OrderCount = new ISeries[] {
                new LineSeries<int>{
                     Name="销售出库单",
                     Values=await db.SalesExWareHouseCountAsync(),
                     Fill=null,
                Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 3 },
                GeometryStroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 3 },
                GeometrySize = 10,

                },
                                new LineSeries<int>{
                     Name="销售退货单",
                     Values= await db.SalesRefundCountAsync(),
                     Fill=null,
                Stroke = new SolidColorPaint(SKColors.Red) { StrokeThickness = 3 },
                GeometryStroke = new SolidColorPaint(SKColors.Red) { StrokeThickness = 3 },
                GeometrySize = 10,

                }
            };
            // 2. 设置X轴
            XAxes1 = new Axis[]
            {
            new Axis
            {
                Name = "月份",
                Labels = GetMonths(),
                LabelsRotation = 15,
                TextSize = 12,
                NameTextSize = 14
            }
            };

            // 3. 设置Y轴
            YAxes1 = new Axis[]
            {
            new Axis
            {
                Name = "数量",
                TextSize = 12,
                NameTextSize = 14,
                Labeler = value => value.ToString("N0") // 格式化为数字
            }
            };


        }
        private string[] GetMonths()
        {
            DateTime currentDate = DateTime.Now;
            string[] months = new string[6];

            for (int i = 0; i < 6; i++)
            {
                DateTime month = currentDate.AddMonths(-i);
                months[5 - i] = month.ToString("M月", CultureInfo.InvariantCulture);
            }

            return months;
        }

        private static List<string> days()
        {
            // 获取今天的日期
            DateTime today = DateTime.Today;

            // 获取前天和昨天的日期
            DateTime yesterday = today.AddDays(-1);
            DateTime dayBeforeYesterday = today.AddDays(-2);

            // 创建一个 List<string> 来保存日期
            List<string> dates = new List<string>();
            dates.Add(dayBeforeYesterday.Day.ToString());  // 前天
            dates.Add(yesterday.Day.ToString());           // 昨天
            dates.Add(today.Day.ToString());               // 今天


            return dates;
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

        private int salesOrderCount;
        public int SalesOrderCount
        {
            get { return salesOrderCount; }
            set { SetProperty(ref salesOrderCount, value); }
        }
        private int salesExWareHouseCount;
        public int SalesExWareHouseCount
        {
            get { return salesExWareHouseCount; }
            set { SetProperty(ref salesExWareHouseCount, value); }
        }





        private ISeries[] salesOrder;
        public ISeries[] SalesOrder
        {
            get => salesOrder;
            set
            { SetProperty(ref salesOrder, value); }
        }

        private Axis[] _xAxes;
        public Axis[] XAxes
        {
            get => _xAxes;
            set
            { SetProperty(ref _xAxes, value); }
        }

        private Axis[] _yAxes;
        public Axis[] YAxes
        {
            get => _yAxes;
            set
            { SetProperty(ref _yAxes, value); }
        }

        private ObservableCollection<CommodityStockDto> commodityStockList;
        public ObservableCollection<CommodityStockDto> CommodityStockList
        {
            get { return commodityStockList; }
            set { SetProperty(ref commodityStockList, value); }
        }




    }

}