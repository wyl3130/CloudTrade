using CloudTrade.Domain.CommodityCompanys;
using CloudTrade.Domain.Departments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Application.Contracts.Departments
{
    public interface IDepartmentService:IBaseService
    {
        Task<(IEnumerable<Department>, int)> DepartmentQueryAsync(int PageIndex = 1, int PageSize = 10, string query = "");
    }
}
