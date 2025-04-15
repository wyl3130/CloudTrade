using CloudTrade.Application.Contracts.Positions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Application.Contracts.OtherFinances
{
    public interface IOtherFinanceService:IBaseService
    {
        Task<(IEnumerable<OtherFinanceDto>, int)> OtherFinanceQueryAsync(int PageIndex = 1, int PageSize = 10, string query = "");
    }
}
