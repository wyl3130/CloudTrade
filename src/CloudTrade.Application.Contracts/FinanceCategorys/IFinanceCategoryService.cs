using CloudTrade.Domain.CommodityCategorys;
using CloudTrade.Domain.FinanceCategorys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Application.Contracts.FinanceCategorys
{
    public interface IFinanceCategoryService:IBaseService
    {
        Task<(IEnumerable<FinanceCategory>, int)> FinanceCategoryQueryAsync(int PageIndex = 1, int PageSize = 10, string query = "");
    }
}
