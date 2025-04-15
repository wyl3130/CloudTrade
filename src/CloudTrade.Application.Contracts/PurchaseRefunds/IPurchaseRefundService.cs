using CloudTrade.Application.Contracts.Commoditys;
using CloudTrade.Application.Contracts.PurchaseOrders;
using CloudTrade.Domain.PurchaseOrders;
using CloudTrade.Domain.PurchaseRefunds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Application.Contracts.PurchaseRefunds
{
    public interface IPurchaseRefundService: ICommodityService
    {
        Task<(IEnumerable<PurchaseRefundDto>, int)> PurchaseRefundQueryAsync(int PageIndex = 1, int PageSize = 10, string query = "", string StartTime = "", string EndTime = "");
        Task<IEnumerable<PurchaseRefundItemDto>> PurchaseRefundViewAsync(Guid Id);
        Task<bool> PurchaseRefundDeleteAsync(PurchaseRefund entity);
    }
}
