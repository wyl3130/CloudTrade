using CloudTrade.Application.Contracts.Commoditys;
using CloudTrade.Domain.CommodityCategorys;
using CloudTrade.Domain.CommodityCompanys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CloudTrade.Application.Contracts.CommodityCategorys
{
    public interface ICommodityCategoryService :IBaseService
    {
        Task<(IEnumerable<CommodityCategory>, int)> CommodityCategoryQueryAsync(int PageIndex=1,int PageSize=10,string query="");
    }
}