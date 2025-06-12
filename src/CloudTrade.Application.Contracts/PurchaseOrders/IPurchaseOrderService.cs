using CloudTrade.Application.Contracts.Commoditys;
using CloudTrade.Application.Contracts.Positions;
using CloudTrade.Domain.PurchaseOrderItems;
using CloudTrade.Domain.PurchaseOrders;
using CloudTrade.Domain.PurchaseRefundItems;
using CloudTrade.Domain.PurchaseRefunds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Application.Contracts.PurchaseOrders
{
    public interface IPurchaseOrderService:ICommodityService
    {
        Task<bool> PurchaseOrderInsertAsync(PurchaseOrder entity, IEnumerable<PurchaseOrderItem> iList);
        Task<bool> PurchaseOrderDeleteAsync(Guid Id);
        Task<bool> PurchaseOrderUpdateAsync(PurchaseOrder entity, IEnumerable<PurchaseOrderItem> iList);
        Task<(IEnumerable<PurchaseOrderDto>, int PageCount)> PurchaseOrderQueryAsync(int PageIndex = 1,int PageSize=10,string query="",string StartTime="",string EndTime="");
        Task<IEnumerable<PurchaseOrderItemDto>> PurchaseOrderViewAsync(Guid Id);
      
    }
}
