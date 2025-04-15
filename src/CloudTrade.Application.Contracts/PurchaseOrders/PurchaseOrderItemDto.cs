using CloudTrade.Domain.PurchaseOrderItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Application.Contracts.PurchaseOrders
{
    public class PurchaseOrderItemDto: PurchaseOrderItem
    {
        public string CommodityName { get; set; }
        public string ModeInfoName { get; set; }
    }
}
