using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView.Extensions;
using LiveChartsCore.SkiaSharpView.VisualElements;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Host.ViewModels.Homes
{

    public class DashboardViewModel:BindableBase
    {
        public  string Title { get; set; } = "仪表盘";
        private readonly IRegionManager regionManager;
        private readonly Random _random = new();

        public ISeries[] Series6 { get; }

        public ICartesianAxis[] XAxes6 { get; }
        public DashboardViewModel(IRegionManager regionManager)
        {
            this.regionManager=regionManager;

            var trend = 100;
            var values = new List<int>();

            for (var i = 0; i < 100; i++)
            {
                trend += _random.Next(-30, 50);
                values.Add(trend);
            }

            Series6 = [new ColumnSeries<int>(values)];
            XAxes6 = [new Axis()];
        }

        public DelegateCommand GoToPage1
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    var axis = XAxes6[0];
                    axis.MinLimit = -0.5;
                    axis.MaxLimit = 10.5;
                });
            }
        }
        public DelegateCommand GoToPage2
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    var axis = XAxes6[0];
                    axis.MinLimit = 9.5;
                    axis.MaxLimit = 20.5;
                });
            }
        }
        public DelegateCommand GoToPage3
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    var axis = XAxes6[0];
                    axis.MinLimit = 19.5;
                    axis.MaxLimit = 30.5;
                });
            }
        }
        public DelegateCommand SeeAll
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    var axis = XAxes6[0];
                    axis.MinLimit = null;
                    axis.MaxLimit = null;
                });
            }
        }




        public ISeries[] Series1 { get; set; }
            = new ISeries[]
            {
                new LineSeries<double>
                {
                    Values = new double[] { 2, 1, 3, 5, 3, 4, 6 ,10},
                    Fill = null
                }
            };
        public ISeries[] Series2 { get; set; } = [
        new LineSeries<ObservablePoint>
        {
            Values = [
                new ObservablePoint(0, 4),
                new ObservablePoint(1, 3),
                new ObservablePoint(3, 8),
                new ObservablePoint(18, 6),
                new ObservablePoint(20, 12)
            ]
        }
    ];
        public ISeries[] Series3 { get; set; } =
        {
    new ColumnSeries<DateTimePoint>
    {
        Values = new List<DateTimePoint>
        {
            new DateTimePoint(new DateTime(2021, 1, 1), 3),
            new DateTimePoint(new DateTime(2021, 1, 2), 6),
            new DateTimePoint(new DateTime(2021, 1, 3), 5),
            new DateTimePoint(new DateTime(2021, 1, 4), 3),
            new DateTimePoint(new DateTime(2021, 1, 5), 5),
            new DateTimePoint(new DateTime(2021, 1, 6), 8),
            new DateTimePoint(new DateTime(2021, 1, 7), 6)
        }
    }
};


        // You can use the DateTimeAxis class to define a date time based axis 

        // The first parameter is the time between each point, in this case 1 day 
        // you can also use 1 year, 1 month, 1 hour, 1 minute, 1 second, 1 millisecond, etc 

        // The second parameter is a function that receives the value and returns the label 
        public Axis[] XAxes { get; set; } =
        {
        new DateTimeAxis(TimeSpan.FromDays(1), date => date.ToString("yyyy-MM-dd"))
    };
        private static int _index = 0;
        private static string[] _names = ["Maria", "Susan", "Charles", "Fiona", "George"];

        public IEnumerable<ISeries> Series4 { get; set; } =
             new[] { 8, 6, 5, 3, 3 }.AsPieSeries((value, series) =>
             {
                 series.Name = _names[_index++ % _names.Length];
                 series.DataLabelsPosition = LiveChartsCore.Measure.PolarLabelsPosition.Outer;
                 series.DataLabelsSize = 15;
                 series.DataLabelsPaint = new SolidColorPaint(new SKColor(30, 30, 30));
                 series.DataLabelsFormatter =
                    point =>
                        $"This slide takes {point.Coordinate.PrimaryValue} " +
                        $"out of {point.StackedValue!.Total} parts";
                 series.ToolTipLabelFormatter = point => $"{point.StackedValue!.Share:P2}";
             });


        public ISeries[] Series5 { get; set; } = [
    new StackedAreaSeries<double>([3, 2, 3, 5, 3, 4, 6]),
        new StackedAreaSeries<double>([6, 5, 6, 3, 8, 5, 2]),
        new StackedAreaSeries<double>([4, 8, 2, 8, 9, 5, 3])
];


        // you can convert any array, list or IEnumerable<T> to a pie series collection:
        public IEnumerable<ISeries> Series7 { get; set; } =
            new[] { 2, 4, 1, 4, 3 }.AsPieSeries();

        // the expression above is equivalent to the next series collection:
        public IEnumerable<ISeries> Series8 { get; set; } =
            [
                new PieSeries<int> { Values = [2] },
            new PieSeries<int> { Values = [4] },
            new PieSeries<int> { Values = [1] },
            new PieSeries<int> { Values = [4] },
            new PieSeries<int> { Values = [3] },
        ];

        //public LabelVisual Title { get; set; } =
        //    new LabelVisual
        //    {
        //        Text = "My chart title",
        //        TextSize = 25,
        //        Padding = new LiveChartsCore.Drawing.Padding(15)
        //    };

        public IEnumerable<ISeries> Series9 { get; set; } =
    new[] { 6, 5, 4, 3, 2 }.AsPieSeries((value, series) =>
    {
        // pushes out the slice with the value of 6 to 30 pixels.
        if (value != 6) return;

        series.Pushout = 30;
    });

        public string view = "DashboardView";






        public DelegateCommand btnCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    regionManager.RequestNavigate("MainRegion", view);

                });
            }
        }

    }

}

public  interface inter
{

}