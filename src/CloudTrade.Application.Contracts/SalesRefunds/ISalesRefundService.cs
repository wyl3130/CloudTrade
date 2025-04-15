using CloudTrade.Application.Contracts.Commoditys;
using CloudTrade.Application.Contracts.PurchaseOrders;
using CloudTrade.Domain.PurchaseOrders;
using CloudTrade.Domain.SalesRefunds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Application.Contracts.SalesRefunds
{
    public interface ISalesRefundService: ICommodityService
    {
        Task<(IEnumerable<SalesRefundDto>, int)> SalesRefundQueryAsync(int PageIndex = 1, int PageSize = 10, string query = "", string StartTime = "", string EndTime = "");
        Task<IEnumerable<SalesRefundItemDto>> SalesRefundViewAsync(Guid Id);
        Task<bool> SalesRefundDeleteAsync(SalesRefund entity);
    }
}
