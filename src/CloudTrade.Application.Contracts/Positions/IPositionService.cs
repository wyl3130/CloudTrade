using CloudTrade.Domain.CommodityCompanys;
using CloudTrade.Domain.Positions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Application.Contracts.Positions
{
    public interface IPositionService:IBaseService
    {
        Task<(IEnumerable<PositionDto>, int)> PositionQueryAsync(int PageIndex = 1, int PageSize = 10, string query = "", string DepartmentName = "");
    }
}
