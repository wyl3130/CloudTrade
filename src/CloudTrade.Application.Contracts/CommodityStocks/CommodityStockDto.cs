using CloudTrade.Domain.CommodityStocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Application.Contracts.CommodityStocks
{
    public class CommodityStockDto:CommodityStock
    {
        public string WareHouseName { get; set; }
        public string CommodityName { get; set; }
    }
}
