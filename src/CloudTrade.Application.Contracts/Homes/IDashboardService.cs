using CloudTrade.Application.Contracts.CommodityStocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Application.Contracts.Homes
{
    public interface IDashboardService:IBaseService
    {
        Task<int> SalesOrderCount();
        int SalesExWareHouseCount();
        int ReceiptCount();
        int NoReceiptCount();

        IEnumerable<CommodityStockDto> CommodityStockList();

        Task<int[]> SalesExWareHouseCountAsync();

        Task<int[]> SalesRefundCountAsync();

        Task<double[]> SalesExWareHouseAmount();

        Task<double[]> SalesExWareHouseAmountAndProfit();
         Task<double[]> SalesOrderRecentAsync();
        Task<double[]> PurchaseOrderRecentAsync();
    }
}
