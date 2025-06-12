using CloudTrade.Domain.CommodityCompanys;
using CloudTrade.Domain.Commoditys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Application.Contracts.Commoditys
{
    public interface ICommodityService:IBaseService
    {
       Task<(IEnumerable<CommodityDto>,int)> CommodityQueryAsync(int PageIndex = 1, int PageSize = 10, string query = "", string CommodityCategoryName="", string CommodityCompanyName="");

        Task<(IEnumerable<CommodityDto>, int)> CommoditySelectAsync(int PageIndex = 1, int PageSize = 10, string query = "", string CommodityCategoryName = "", string CommodityCompanyName = "", Guid WareHouseId=new Guid());
    }
}
