using CloudTrade.Application.Contracts.Commoditys;
using CloudTrade.Application.Contracts.PurchaseOrders;
using CloudTrade.Application.Contracts.SalesOrders;
using CloudTrade.Domain.PurchaseOrders;
using CloudTrade.Domain.PurchaseWareHouse;
using CloudTrade.Domain.PurchaseWareHouseItems;
using CloudTrade.Domain.SalesExWareHouseItem;
using CloudTrade.Domain.SalesExWareHouses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Application.Contracts.SalesExWareHouses
{
    public interface ISalesExWareHouseService : ISalesOrderService
    {
        Task<bool> SalesExWareHouseInsertAsync(SalesExWareHouse entity, IEnumerable<SalesExWareHouseItem> iList);
        Task<bool> SalesExWareHouseDeleteAsync(Guid Id);
        Task<bool> SalesExWareHouseUpdateAsync(SalesExWareHouse entity, IEnumerable<SalesExWareHouseItem> iList);
        Task<(IEnumerable<CloudTrade.Application.Contracts.SalesExWareHouses.SalesExWareHouseDto>, int)> SalesExWarehouseQueryAsync(int PageIndex = 1, int PageSize = 10, string query = "", string StartTime = "", string EndTime = "");
        Task<IEnumerable<SalesExWareHouseItemDto>> SalesExWareHouseViewAsync(Guid Id);

    }
}
