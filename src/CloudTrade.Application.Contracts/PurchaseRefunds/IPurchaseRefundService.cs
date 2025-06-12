using CloudTrade.Application.Contracts.Commoditys;
using CloudTrade.Application.Contracts.PurchaseOrders;
using CloudTrade.Application.Contracts.PurchaseWareHouses;
using CloudTrade.Domain.PurchaseOrders;
using CloudTrade.Domain.PurchaseRefundItems;
using CloudTrade.Domain.PurchaseRefunds;
using CloudTrade.Domain.PurchaseWareHouse;
using CloudTrade.Domain.PurchaseWareHouseItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Application.Contracts.PurchaseRefunds
{
    public interface IPurchaseRefundService: IPurchaseWareHouseService
    {
        Task<bool> PurchaseRefundInsertAsync(PurchaseRefund entity, IEnumerable<PurchaseRefundItem> iList);
        Task<bool> PurchaseRefundDeleteAsync(Guid Id);
        Task<bool> PurchaseRefundUpdateAsync(PurchaseRefund entity, IEnumerable<PurchaseRefundItem> iList);
        Task<(IEnumerable<PurchaseRefundDto>, int)> PurchaseRefundQueryAsync(int PageIndex = 1, int PageSize = 10, string query = "", string StartTime = "", string EndTime = "");
        Task<IEnumerable<PurchaseRefundItemDto>> PurchaseRefundViewAsync(Guid Id);





    }
}
