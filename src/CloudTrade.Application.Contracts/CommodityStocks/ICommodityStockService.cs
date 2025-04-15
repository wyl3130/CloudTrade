using CloudTrade.Domain.CommodityCategorys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Application.Contracts.CommodityStocks
{
    public interface ICommodityStockService:IBaseService
    {
        Task<(IEnumerable<CommodityStockDto>, int)> CommodityStockQueryAsync(int PageIndex = 1, int PageSize = 10, string query = "", string WareHouseName = "");
    }
}