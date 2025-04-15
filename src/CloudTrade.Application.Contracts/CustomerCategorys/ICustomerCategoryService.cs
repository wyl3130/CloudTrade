using CloudTrade.Application.Contracts.CommodityStocks;
using CloudTrade.Domain.CustomerCategorys;
using CloudTrade.SqlSugarCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Application.Contracts.CustomerCategorys
{
    public interface ICustomerCategoryService:IBaseService
    {
        Task<(IEnumerable<CustomerCategory>, int)> CustomerCategoryQueryAsync(int PageIndex = 1, int PageSize = 10,string query="");
    }
}
