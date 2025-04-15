using CloudTrade.Application.Contracts.Commoditys;
using CloudTrade.Application.Contracts.PurchaseOrders;
using CloudTrade.Domain.PurchaseOrders;
using CloudTrade.Domain.PurchaseWareHouse;
using CloudTrade.Domain.PurchaseWareHouseItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Application.Contracts.PurchaseWareHouses
{
    public interface IPurchaseWareHouseService: ICommodityService
    {
        Task<bool> PurchaseWareHouseInsertAsync(PurchaseWareHouse entity, IEnumerable<PurchaseWareHouseItem> iList);
        Task<(IEnumerable<PurchaseWareHouseDto>, int)> PurchaseWareHouseQueryAsync(int PageIndex = 1, int PageSize = 10, string query = "", string StartTime = "", string EndTime = "");
        Task<IEnumerable<PurchaseWareHouseItemDto>> PurchaseWareHouseViewAsync(Guid Id);

        Task<bool> PurchaseWareHouseDeleteAsync(PurchaseWareHouse entity);
    }
}
