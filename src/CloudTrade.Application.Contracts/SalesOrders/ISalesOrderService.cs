using CloudTrade.Application.Contracts.Commoditys;
using CloudTrade.Application.Contracts.PurchaseOrders;
using CloudTrade.Domain.PurchaseOrders;
using CloudTrade.Domain.PurchaseWareHouse;
using CloudTrade.Domain.PurchaseWareHouseItems;
using CloudTrade.Domain.SalesOrderItems;
using CloudTrade.Domain.SalesOrders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Application.Contracts.SalesOrders
{
    public interface ISalesOrderService: ICommodityService
    {
        Task<bool> SalesOrderInsertAsync(SalesOrder entity, IEnumerable<SalesOrderItem> iList);
        Task<bool> SalesOrderDeleteAsync(Guid Id);
        Task<bool> SalesOrderUpdateAsync(SalesOrder entity, IEnumerable<SalesOrderItem> iList);
        Task<(IEnumerable<SalesOrderDto>, int)> SalesOrderQueryAsync(int PageIndex=1,int PageSize=10, string query = "", string StartTime = "", string EndTime = "");
        Task<IEnumerable<SalesOrderItemDto>> SalesOrderViewAsync(Guid Id);
  
    }
}
