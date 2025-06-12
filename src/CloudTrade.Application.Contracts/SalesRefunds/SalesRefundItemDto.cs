using CloudTrade.Domain.SalesRefundItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Application.Contracts.SalesRefunds
{
    public class SalesRefundItemDto: SalesRefundItem
    {
        public string CommodityName { get; set; }
        public int StockCount { get; set; }

    }
}
