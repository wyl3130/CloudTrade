using CloudTrade.Domain.CustomerCategorys;
using CloudTrade.Domain.SalesModes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Application.Contracts.SalesModes
{
    public interface ISalesModeService:IBaseService
    {
        Task<(IEnumerable<SalesMode>, int)> SalesModeQueryAsync(int PageIndex = 1, int PageSize = 10, string query = "");
    }
}
