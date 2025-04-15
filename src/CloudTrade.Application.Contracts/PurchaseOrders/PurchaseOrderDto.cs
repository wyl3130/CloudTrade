using CloudTrade.Domain.PurchaseOrders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Application.Contracts.PurchaseOrders
{
    public class PurchaseOrderDto: PurchaseOrder
    {
        public string CustomerCompanyName { get; set; }
        public string WareHouseName { get; set; }
        public IEnumerable<PurchaseOrderItemDto> PurchaseOrderItem { get; set; }
       
    }
}
