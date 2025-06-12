using CloudTrade.Application.Contracts.Commoditys;
using CloudTrade.Application.Contracts.PurchaseOrders;
using CloudTrade.Application.Contracts.SalesExWareHouses;
using CloudTrade.Domain.PurchaseOrders;
using CloudTrade.Domain.PurchaseWareHouse;
using CloudTrade.Domain.PurchaseWareHouseItems;
using CloudTrade.Domain.SalesRefundItems;
using CloudTrade.Domain.SalesRefunds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Application.Contracts.SalesRefunds
{
    public interface ISalesRefundService: ISalesExWareHouseService
    {
        Task<bool> SalesRefundInsertAsync(SalesRefund entity, IEnumerable<SalesRefundItem> iList);
        Task<bool> SalesRefundDeleteAsync(Guid Id);
        Task<bool> SalesRefundUpdateAsync(SalesRefund entity, IEnumerable<SalesRefundItem> iList);

        Task<(IEnumerable<SalesRefundDto>, int)> SalesRefundQueryAsync(int PageIndex = 1, int PageSize = 10, string query = "", string StartTime = "", string EndTime = "");
        Task<IEnumerable<SalesRefundItemDto>> SalesRefundViewAsync(Guid Id);
  
    }
}
