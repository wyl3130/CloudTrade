using CloudTrade.Domain.SalesOrders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Application.Contracts.SalesOrders
{
    public class SalesOrderDto: SalesOrder
    {
        public string CustomerCompanyName { get; set; }
        public string WareHouseName { get; set; }
    }
}
