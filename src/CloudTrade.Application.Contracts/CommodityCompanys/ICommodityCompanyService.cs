using CloudTrade.Domain.CommodityCategorys;
using CloudTrade.Domain.CommodityCompanys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Application.Contracts.CommodityCompanys
{
    public interface ICommodityCompanyService : IBaseService
    {
        Task<(IEnumerable<CommodityCompany>, int )> CommodityCompanyQueryAsync(int PageIndex = 1, int PageSize = 10, string query = "");
    }
}
