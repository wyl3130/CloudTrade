using CloudTrade.Application.Contracts.Homes;
using CloudTrade.Domain.PurchaseOrderItems;
using CloudTrade.Domain.PurchaseOrders;
using CloudTrade.Domain.SalesExWareHouses;
using CloudTrade.Domain.SalesOrderItems;
using CloudTrade.Domain.SalesOrders;
using CloudTrade.Domain.SalesRefunds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Application.Homes
{
    public class DashboardService : BaseService, IDashboardService
    {
        public ISqlSugarClient db;
        public DashboardService(ISqlSugarClient db) : base(db)
        {
            this.db = db;
        }



        public int NoReceiptCount()
        {
            throw new NotImplementedException();
        }

        public int ReceiptCount()
        {
            throw new NotImplementedException();
        }

        public int SalesExWareHouseCount()
        {
            return db.Queryable<SalesExWareHouse>().Count();
        }

        public async Task<int> SalesOrderCount()
        {
            return await db.Queryable<SalesOrder>().CountAsync();
        }

        public IEnumerable<CommodityStockDto> CommodityStockList()
        {
            var list = db.Queryable<CommodityStock, WareHouse, Commodity>((cs, w, c) => new JoinQueryInfos(
                JoinType.Inner, cs.WareHouseId == w.Id,
                JoinType.Inner, cs.CommodityId == c.Id)
                )
                .Where((cs) => cs.StockCount < 10)
                .Select((cs, w, c) => new CommodityStockDto()
                {
                    CommodityName = c.CommodityName,
                    WareHouseName = w.WareHouseName,
                    StockCount = cs.StockCount,
                    SalesCount = cs.SalesCount,
                })
                .ToList();
            List<CommodityStockDto> clist = new List<CommodityStockDto>();

            return list;
        }

        public async Task<int[]> SalesExWareHouseCountAsync()
        {
            var currentDate = DateTime.Now;
            var lastSixMonthsCount = new int[6];

            // 获取最近六个月的月份
            for (int i = 0; i < 6; i++)
            {
                var startOfMonth = new DateTime(currentDate.AddMonths(-i).Year, currentDate.AddMonths(-i).Month, 1);
                var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1); // 该月的最后一天

                // 查询该月的条数
                var count = await db.Queryable<SalesExWareHouse>()
                              .Where(x => Convert.ToDateTime(Convert.ToDateTime(x.CreateTime).ToString("yyyy/MM/dd")) >= startOfMonth && Convert.ToDateTime(Convert.ToDateTime(x.CreateTime).ToString("yyyy/MM/dd")) <= endOfMonth)
                              .CountAsync();

                // 将结果存入数组
                lastSixMonthsCount[5 - i] = count;
            }
            return lastSixMonthsCount;
        }
        public async Task<int[]> SalesRefundCountAsync()
        {
            var currentDate = DateTime.Now;
            var lastSixMonthsCount = new int[6];

            // 获取最近六个月的月份
            for (int i = 0; i < 6; i++)
            {
                var startOfMonth = new DateTime(currentDate.AddMonths(-i).Year, currentDate.AddMonths(-i).Month, 1);
                var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1); // 该月的最后一天

                // 查询该月的条数
                var count = await db.Queryable<SalesRefund>()
                              .Where(x => Convert.ToDateTime(Convert.ToDateTime(x.CreateTime).ToString("yyyy/MM/dd")) >= startOfMonth && Convert.ToDateTime(Convert.ToDateTime(x.CreateTime).ToString("yyyy/MM/dd")) <= endOfMonth)
                              .CountAsync();

                // 将结果存入数组
                lastSixMonthsCount[5 - i] = count;
            }
            return lastSixMonthsCount;
        }
        public async Task<double[]> SalesExWareHouseAmount() 
        {
            var currentDate = DateTime.Now;
            var lastSixMonthsCount = new double[6];

            // 获取最近六个月的月份
            for (int i = 0; i < 6; i++)
            {
                var startOfMonth = new DateTime(currentDate.AddMonths(-i).Year, currentDate.AddMonths(-i).Month, 1);
                var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1); // 该月的最后一天

                // 查询该月的条数
                var count = await db.Queryable<SalesOrderItem>()
                              .Where(x => Convert.ToDateTime(Convert.ToDateTime(x.CreateTime).ToString("yyyy/MM/dd")) >= startOfMonth && Convert.ToDateTime(Convert.ToDateTime(x.CreateTime).ToString("yyyy/MM/dd")) <= endOfMonth)
                              .SumAsync(x=>x.Amount);

                // 将结果存入数组
                lastSixMonthsCount[5 - i] = count;
            }
            return lastSixMonthsCount;
        }
        public async Task<double[]> SalesExWareHouseAmountAndProfit()
        {
            var currentDate = DateTime.Now;
            var lastSixMonthsSalesAmount = new double[6];
            var lastSixMonthsPurchaseAmount = new double[6];
            var profit = new double[6];

            // 获取最近六个月的月份
            for (int i = 0; i < 6; i++)
            {
                var startOfMonth = new DateTime(currentDate.AddMonths(-i).Year, currentDate.AddMonths(-i).Month, 1);
                var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1); // 该月的最后一天

                // 查询销售订单的营业额
                var salesAmount = await db.Queryable<SalesOrderItem>()
                                      .Where(x => Convert.ToDateTime(Convert.ToDateTime(x.CreateTime).ToString("yyyy/MM/dd")) >= startOfMonth && Convert.ToDateTime(Convert.ToDateTime(x.CreateTime).ToString("yyyy/MM/dd")) <= endOfMonth)
                                      .SumAsync(x => x.Amount);

                lastSixMonthsSalesAmount[5 - i] = salesAmount;

                // 查询采购订单的金额
                var purchaseAmount = await db.Queryable<PurchaseOrderItem>()
                                            .Where(x => Convert.ToDateTime(Convert.ToDateTime(x.CreateTime).ToString("yyyy/MM/dd")) >= startOfMonth && Convert.ToDateTime(Convert.ToDateTime(x.CreateTime).ToString("yyyy/MM/dd")) <= endOfMonth)
                                            .SumAsync(x => x.Amount);

                lastSixMonthsPurchaseAmount[5 - i] = purchaseAmount;

                // 计算利润
                profit[5 - i] = salesAmount - purchaseAmount;
            }

            return profit;
        }

        public async Task<double[]> SalesOrderRecentAsync()
        {
            var today = DateTime.Today;
            var yesterday = today.AddDays(-1);
            var dayBeforeYesterday = today.AddDays(-2);

            // 查询今天的订单数
            int todayCount = await db.Queryable<SalesOrder>()
                .Where(x => Convert.ToDateTime(Convert.ToDateTime(x.CreateTime).ToString("yyyy/MM/dd")) == today)
                .CountAsync();

            // 查询昨天的订单数
            int yesterdayCount = await db.Queryable<SalesOrder>()
                .Where(x => Convert.ToDateTime(Convert.ToDateTime(x.CreateTime).ToString("yyyy/MM/dd")) == yesterday)
                .CountAsync();

            // 查询前天的订单数
            int dayBeforeYesterdayCount = await db.Queryable<SalesOrder>()
                .Where(x => Convert.ToDateTime(Convert.ToDateTime(x.CreateTime).ToString("yyyy/MM/dd")) == dayBeforeYesterday)
                .CountAsync();

            // 返回数组 [前天, 昨天, 今天]
            double[] counts = { dayBeforeYesterdayCount, yesterdayCount, todayCount };
            return counts;
        }
        public async Task<double[]> PurchaseOrderRecentAsync()
        {
            var today = DateTime.Today;
            var yesterday = today.AddDays(-1);
            var dayBeforeYesterday = today.AddDays(-2);

            // 查询今天的订单数
            int todayCount = await db.Queryable<PurchaseOrder>()
                .Where(x => Convert.ToDateTime(Convert.ToDateTime(x.CreateTime).ToString("yyyy/MM/dd")) == today)
                .CountAsync();

            // 查询昨天的订单数
            int yesterdayCount = await db.Queryable<PurchaseOrder>()
                .Where(x => Convert.ToDateTime(Convert.ToDateTime(x.CreateTime).ToString("yyyy/MM/dd")) == yesterday)
                .CountAsync();

            // 查询前天的订单数
            int dayBeforeYesterdayCount = await db.Queryable<PurchaseOrder>()
                .Where(x => Convert.ToDateTime(Convert.ToDateTime(x.CreateTime).ToString("yyyy/MM/dd")) == dayBeforeYesterday)
                .CountAsync();

            // 返回数组 [前天, 昨天, 今天]
            double[] counts = { dayBeforeYesterdayCount, yesterdayCount, todayCount };
            return counts;
        }
    }

}
//Convert.ToDateTime(Convert.ToDateTime(p.CreateTime).ToString("yyyy/MM/dd"))