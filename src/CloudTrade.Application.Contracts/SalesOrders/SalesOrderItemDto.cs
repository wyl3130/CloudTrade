using CloudTrade.Domain.SalesOrderItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Application.Contracts.SalesOrders
{
    public class SalesOrderItemDto: SalesOrderItem
    {


        public string CommodityName { get; set; }
        public string ModeInfoName { get; set; }
        public int StockCount { get; set; }
    }
}
